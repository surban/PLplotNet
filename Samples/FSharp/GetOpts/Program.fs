open System
open PLplot


[<EntryPoint>]
let main argv =
    use pl = new PLStream()
    let mutable argv = argv

    printfn "before parseopts: %A" argv
    pl.parseopts(&argv, ParseOpts.Full ||| ParseOpts.NoProgram) |> ignore
    printfn "after parseopts: %A" argv
    
    0
    