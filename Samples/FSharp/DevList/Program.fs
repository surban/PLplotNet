open System
open PLplot


[<EntryPoint>]
let main argv =

    use pl = new PLStream()

    let mutable titles = Array.empty
    let mutable devs = Array.empty
    pl.gDevs(&titles, &devs)

    printfn "PLplot available devices"
    printfn "========================"
    for title, dev in Array.zip titles devs do
        printfn "%s: %s" dev title
    
    0
    