open System
open PLplot


[<EntryPoint>]
let main argv =

    // generate data
    let nsize = 101
    let xmin, xmax, ymin, ymax = 0., 1., 0., 100.
    let x, y =
        Array.init nsize (fun i ->
            let x = float i / float (nsize - 1)
            let y = ymax * x * x
            x, y)
        |> Array.unzip

    // Initialize plplot
    use pl = new PLStream()
    pl.init()

    // output version
    printfn "PLplot version %s" (pl.gver())

    // Create a labelled box to hold the plot.
    pl.env( xmin, xmax, ymin, ymax, AxesScale.Independent, AxisBox.BoxTicksLabelsAxes )
    pl.lab( "x", "y=100 x#u2#d", "Simple PLplot demo of a 2D line plot" )

    // Plot the data that was prepared above.
    pl.line( x, y )

    // PLplot is automatically closed when pl is disposed.
    0
    