
open System
open System.Text.RegularExpressions
open System.IO
open System.Xml
open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.Formatting
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax
open Microsoft.CodeAnalysis.CSharp.Formatting
open Microsoft.CodeAnalysis.Editing


let skipStreamMethods =
    ["end"; "end1"; "gstrm"; "mkstrm"; "sstrm"; "cpstrm"]
    |> Set.ofList


type FunctionDoc = {
    Name: string
    Summary: string
    Remarks: string
    Parameters: Map<string, string>
}

let readDocs xmlPath =
    // remove entitites and whitespace
    let text = File.ReadAllText(xmlPath)
    let text = Regex.Replace(text, "PLplot-website", "w", RegexOptions.Compiled)
    let text = Regex.Replace(text, "\&(\w+);", "$1", RegexOptions.Compiled)
    let text = Regex.Replace(text, "\s+", " ", RegexOptions.Compiled)
    let xml = new XmlDocument()
    xml.LoadXml(text)

    let mutable docs = Map.empty
    for sect in xml.SelectNodes("//sect1") do
        let funcNodes = sect.SelectNodes("./title/function")
        if funcNodes.Count > 0 then
            let pars = 
                sect.SelectNodes("./variablelist/varlistentry")
                |> Seq.cast<XmlNode>
                |> Seq.map (fun parEntry ->
                    let parName = parEntry.SelectSingleNode("./term/parameter").InnerText.Trim()
                    let parDesc = parEntry.SelectSingleNode("./listitem").InnerText.Trim()
                    parName, parDesc)
                |> Map.ofSeq
            let funcName = funcNodes.[0].InnerText
            docs <- docs |> Map.add funcName {Name=funcName; 
                                              Summary=sect.SelectSingleNode("./title").InnerText.Trim(); 
                                              Remarks=sect.SelectNodes("./para").[1].InnerText.Trim(); 
                                              Parameters=pars}   
    docs    


type StreamGen () =
    let mutable cu = SyntaxFactory.CompilationUnit()
    let mutable ns = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("PLplot"));
    let cl = SyntaxFactory.ClassDeclaration("PLStream")
    let mutable cl = cl.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), 
                                     SyntaxFactory.Token(SyntaxKind.PartialKeyword))
    
    member this.AddUsing(ud: UsingDirectiveSyntax) =
        if ud.Alias = null then
            cu <- cu.AddUsings(ud)
        else
            ns <- ns.AddUsings(ud)

    member this.AddMethod(md: MethodDeclarationSyntax) = 
        let name = md.Identifier.Text
        if not (name.StartsWith "_" || skipStreamMethods |> Set.contains name) then 
            let lt = md.GetLeadingTrivia()
            let md = md.WithAttributeLists(SyntaxList())
            let md = md.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            let newPars =
                md.ParameterList.Parameters
                |> Seq.map (fun par -> par.WithAttributeLists(SyntaxList()))
                |> SyntaxFactory.SeparatedList
            let parList = md.ParameterList.WithParameters newPars
            let md = md.WithParameterList parList

            let actStr = SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("ActivateStream"));
            let actStr = SyntaxFactory.ExpressionStatement(actStr)
           
            let args =
                newPars
                |> Seq.map (fun par ->
                    let modf = if par.Modifiers.Any() then par.Modifiers.First() else SyntaxFactory.Token(SyntaxKind.None)
                    SyntaxFactory.Argument(null, modf, SyntaxFactory.IdentifierName(par.Identifier)))
                |> SyntaxFactory.SeparatedList                    
                |> SyntaxFactory.ArgumentList
            let nativeCall = SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("Native." + name))
            let nativeCall = nativeCall.WithArgumentList args
            let nativeCall = 
                if md.ReturnType.ToFullString().Trim() = "void" then
                    SyntaxFactory.ExpressionStatement(nativeCall) :> StatementSyntax
                else
                    SyntaxFactory.ReturnStatement(nativeCall) :> StatementSyntax

            let execBlock = SyntaxFactory.Block(actStr, nativeCall)
            let lockLib = SyntaxFactory.LockStatement(SyntaxFactory.IdentifierName("libLock"), execBlock);

            //SyntaxFactory.Token(SyntaxKind.None)
            let md = md.WithBody(SyntaxFactory.Block(lockLib)).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.None))
            let md = md.WithLeadingTrivia(lt)
            cl <- cl.AddMembers(md)

    member this.Finish() =
        let ns = ns.AddMembers(cl)
        let cu = cu.AddMembers(ns)    
        cu :> SyntaxNode



type DocRewriter (docs: Map<string, FunctionDoc>, sg: StreamGen) =
    inherit CSharpSyntaxRewriter()
    override this.VisitUsingDirective (ud) =
        sg.AddUsing(ud)
        ud :> SyntaxNode

    override this.VisitMethodDeclaration (md) =
        let func = md.Identifier.Text
        let plFunc = 
            match func with
            | _ when func.StartsWith("pl") -> func 
            | _ when func.StartsWith("setcontlabel") -> "pl_" + func
            | _ -> "pl" + func
        match docs |> Map.tryFind plFunc with
        | Some doc ->
            let availPars =
                md.ParameterList.Parameters
                |> Seq.map (fun p -> p.Identifier.Text)
                |> Set.ofSeq
            let paramSyn = 
                doc.Parameters
                |> Map.toList
                |> List.collect (fun (name, desc) -> 
                    if availPars |> Set.contains name then
                        [SyntaxFactory.XmlParamElement(name, SyntaxFactory.XmlText desc) :> XmlNodeSyntax;
                         SyntaxFactory.XmlNewLine("\n") :> XmlNodeSyntax]
                    else [])
            let content =
                [SyntaxFactory.XmlSummaryElement(SyntaxFactory.XmlText doc.Summary) :> XmlNodeSyntax; 
                 SyntaxFactory.XmlNewLine("\n") :> XmlNodeSyntax]
                @ paramSyn
                @ [SyntaxFactory.XmlRemarksElement(SyntaxFactory.XmlText doc.Remarks) :> XmlNodeSyntax]
                |> Seq.toArray
            let docSyn = SyntaxFactory.DocumentationComment(content)

            let md = 
                if md.GetLeadingTrivia().ToFullString().Contains("///") then md
                else md.WithLeadingTrivia(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.CarriageReturnLineFeed,
                            SyntaxFactory.Trivia(docSyn), 
                            SyntaxFactory.CarriageReturnLineFeed)) 
            sg.AddMethod md
            md :> SyntaxNode
        | None ->
            printfn "No documentation found for %s" plFunc
            let lt = md.GetLeadingTrivia()
            let docs = 
                [for trv in lt do
                    if trv.IsKind(SyntaxKind.SingleLineCommentTrivia) then
                        yield trv.ToFullString().Replace("//", "").Trim()]
                |> String.concat " "            
            if docs.Length > 0 then
                let st = SyntaxTrivia()            
                let doc = 
                    SyntaxFactory.DocumentationComment(
                        SyntaxFactory.XmlSummaryElement(SyntaxFactory.XmlText(docs))                
                    )
                let md = 
                    md.WithLeadingTrivia(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.CarriageReturnLineFeed,
                            SyntaxFactory.Trivia(doc), 
                            SyntaxFactory.CarriageReturnLineFeed)) 
                sg.AddMethod md
                md :> SyntaxNode            
            else
                sg.AddMethod md
                md :> SyntaxNode


[<EntryPoint>]
let main argv =
    let apiDocs = readDocs "/home/surban/dev/plplot/doc/docbook/src/api.xml"
    let cDocs = readDocs "/home/surban/dev/plplot/doc/docbook/src/api-c.xml"
    let docs = (Map.toList apiDocs) @ (Map.toList cDocs) |> Map.ofList

    use workspace = new AdhocWorkspace()
    let generator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);

    let nativeTmplSrc = Text.SourceText.From(File.ReadAllText("/home/surban/dev/plplot/bindings/net/NativeTmpl.cs"))
    let nativeTmpl = CSharpSyntaxTree.ParseText(nativeTmplSrc)

    let streamGenerator = StreamGen ()
    let rewriter = DocRewriter (docs, streamGenerator)
    let nativeGen = rewriter.Visit(nativeTmpl.GetRoot())
    let streamGen = streamGenerator.Finish()

    let nativeGenSrc = Formatter.Format(nativeGen, workspace)          
    File.WriteAllText("/home/surban/dev/plplot/bindings/net/NativeGenerated.cs", nativeGenSrc.ToFullString())

    let streamGenSrc = Formatter.Format(streamGen, workspace)          
    File.WriteAllText("/home/surban/dev/plplot/bindings/net/PLStreamGenerated.cs", streamGenSrc.ToFullString())


    0
    
    