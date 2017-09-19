open System
open PLplot

let pl = new PLStream()

let pi = atan 1.0 * 4.0

(* Pairs of points making the line segments used to plot the user defined
   arrow. *)
let arrow_x = [|-0.5; 0.5; 0.3; 0.5; 0.3; 0.5|]
let arrow_y = [|0.0; 0.0; 0.2; 0.0; -0.2; 0.0|]
let arrow2_x = [|-0.5; 0.3; 0.3; 0.5; 0.3; 0.3|]
let arrow2_y = [|0.0; 0.0; 0.2; 0.0; -0.2; 0.0|]

(*--------------------------------------------------------------------------*\
 * Generates several simple vector plots.
 \*--------------------------------------------------------------------------*)

(*
 * Vector plot of the circulation about the origin
 *)
let circulation () =
  let nx = 20
  let ny = 20
  let dx = 1.0
  let dy = 1.0

  let xmin = - float nx / 2.0 * dx
  let xmax = float nx / 2.0 * dx
  let ymin = - float ny / 2.0 * dy
  let ymax = float ny / 2.0 * dy

  let xg = Array2D.zeroCreate nx ny
  let yg = Array2D.zeroCreate nx ny 
  let u = Array2D.zeroCreate nx ny
  let v = Array2D.zeroCreate nx ny

  (* Create data - circulation around the origin. *)
  for i = 0 to nx - 1 do
    let x = (float i - float nx / 2.0 + 0.5) * dx
    for j = 0 to ny - 1 do
      let y = (float j - float ny / 2.0 + 0.5) * dy
      xg.[i,j] <- x;
      yg.[i,j] <- y;
      u.[i,j] <- y;
      v.[i,j] <- - x;

  (* Plot vectors with default arrows *)
  pl.env(xmin, xmax, ymin, ymax, AxesScale.Independent, AxisBox.BoxTicksLabels);
  pl.lab("(x)", "(y)", "#frPLplot Example 22 - circulation");
  pl.col0 2;
  pl.vect (u, v, 0.0, pl.tr2(xg, yg))
  pl.col0 1;
  ()

(*
 * Vector plot of flow through a constricted pipe
 *)
let constriction astyle =
  let nx = 20
  let ny = 20
  let dx = 1.0
  let dy = 1.0

  let xmin = - float nx / 2.0 * dx
  let xmax = float nx / 2.0 * dx
  let ymin = - float ny / 2.0 * dy
  let ymax = float ny / 2.0 * dy

  let xg = Array2D.zeroCreate nx ny
  let yg = Array2D.zeroCreate nx ny 
  let u = Array2D.zeroCreate nx ny
  let v = Array2D.zeroCreate nx ny

  let q = 2.0
  for i = 0 to nx - 1 do
    let x = (float i - float nx / 2.0 + 0.5) * dx
    for j = 0 to ny - 1 do
      let y = (float j - float ny / 2.0 + 0.5) * dy
      xg.[i,j] <- x;
      yg.[i,j] <- y;
      let b = ymax / 4.0 * (3.0 - cos (pi * x / xmax))
      if abs y < b then 
        let dbdx = ymax / 4.0 * sin (pi * x / xmax) * pi / xmax * y / b
        u.[i,j] <- q * ymax / b;
        v.[i,j] <- dbdx * u.[i,j];
      else 
        u.[i,j] <- 0.0;
        v.[i,j] <- 0.0;

  pl.env(xmin, xmax, ymin, ymax, AxesScale.Independent, AxisBox.BoxTicksLabels);
  let title = sprintf "%s%d%s" "#frPLplot Example 22 - constriction (arrow style " astyle ")"
  pl.lab("(x)", "(y)", title);
  pl.col0 2;
  pl.vect (u, v, (-1.0), pl.tr2(xg, yg)) ;
  pl.col0 1;
  ()

let f2mnmx (f: float[,]) =
  let fmax = ref f.[0,0]
  let fmin = ref f.[0,0]

  for i = 0 to Array2D.length1 f - 1 do
    for j = 0 to Array2D.length2 f - 1 do
      fmax := max !fmax f.[i,j];
      fmin := min !fmin f.[i,j];
  !fmin, !fmax

(*
 * Vector plot of the gradient of a shielded potential (see example 9)
 *)
let potential () =
  let nper = 100
  let nlevel = 10
  let nr = 20
  let ntheta = 20

  (* Potentialside a conducting cylinder (or sphere) by method of images.
     Charge 1 is placed at (d1, d1), with image charge at (d2, d2).
     Charge 2 is placed at (d1, -d1), with image charge at (d2, -d2).
     Also put smoothing term at small distances. *)

  let rmax = float nr

  let eps = 2.0

  let q1 = 1.0
  let d1 = rmax / 4.0

  let q1i = - q1 * rmax / d1
  let d1i = rmax**2.0 / d1

  let q2 = -1.0
  let d2 = rmax / 4.0

  let q2i = - q2 * rmax / d2
  let d2i = rmax**2.0 / d2

  let xg = Array2D.zeroCreate nr ntheta 
  let yg = Array2D.zeroCreate nr ntheta 
  let u = Array2D.zeroCreate nr ntheta 
  let v = Array2D.zeroCreate nr ntheta 
  let z = Array2D.zeroCreate nr ntheta 

  for i = 0 to nr - 1 do
    let r = 0.5 + float i
    for j = 0 to ntheta - 1 do
      let theta = 2.0 * pi / float (ntheta - 1) * (0.5 + float j)  
      let x = r * cos theta
      let y = r * sin theta
      xg.[i,j] <- x;
      yg.[i,j] <- y;
      let div1 = sqrt ((x - d1)**2.0 + (y - d1)**2.0 + eps**2.0)
      let div1i = sqrt ((x - d1i)**2.0 + (y - d1i)**2.0 + eps**2.0)
      let div2 = sqrt ((x - d2)**2.0 + (y + d2)**2.0 + eps**2.0)
      let div2i = sqrt ((x - d2i)**2.0 + (y + d2i)**2.0 + eps**2.0)
      z.[i,j] <- q1 / div1 + q1i / div1i + q2 / div2 + q2i / div2i;
      u.[i,j] <-
        - q1 * (x - d1) / div1**3.0 - q1i * (x - d1i) / div1i**3.0
        - q2 * (x - d2) / div2**3.0 - q2i * (x - d2i) / div2i**3.0;
      v.[i,j] <-
        - q1 * (y - d1) / div1**3.0 - q1i * (y - d1i) / div1i**3.0
        - q2 * (y + d2) / div2**3.0 - q2i * (y + d2i) / div2i**3.0;

  let xmin, xmax = f2mnmx xg
  let ymin, ymax = f2mnmx yg
  let zmin, zmax = f2mnmx z

  pl.env(xmin, xmax, ymin, ymax, AxesScale.Independent, AxisBox.BoxTicksLabels);
  pl.lab("(x)", "(y)", "#frPLplot Example 22 - potential gradient vector plot");
  (* Plot contours of the potential *)
  let dz = (zmax - zmin) / float nlevel
  let clevel =
    Array.init nlevel (fun i -> zmin + (float i + 0.5) * dz)
 
  pl.col0 3;
  pl.lsty LineStyle.ShortDashesShortGaps;
  pl.cont (z, 0, nr-1, 0, ntheta-1, clevel, pl.tr2(xg,yg));
  pl.lsty LineStyle.Continuous;
  pl.col0 1;

  (* Plot the vectors of the gradient of the potential *)
  pl.col0 2;
  pl.vect (u, v, 25.0, pl.tr2(xg, yg));
  pl.col0 1;

  let px = Array.zeroCreate nper 
  let py = Array.zeroCreate nper 
  (* Plot the perimeter of the cylinder *)
  for i=0 to nper - 1 do
    let theta = (2.0 * pi / float (nper - 1)) * float i
    px.[i] <- rmax * cos theta;
    py.[i] <- rmax * sin theta;
  pl.line(px, py);
  ()

let transform2 xmax x y (xt: float byref) (yt: float byref) =
  xt <- x
  yt <- y / 4.0 * (3.0 - cos (pi * x / xmax))

(* Vector plot of flow through a constricted pipe
   with a coordinate transform *)
let constriction2 () =
  let nx, ny = 20, 20
  let nc = 11
  let nseg = 20
  let dx, dy = 1.0, 1.0
  let xmin = float -nx / 2.0 * dx
  let xmax = float nx / 2.0 * dx
  let ymin = float -ny / 2.0 * dy
  let ymax = float ny / 2.0 * dy

  pl.stransform (TransformFunc(fun x y xt yt _ -> transform2 xmax x y &xt &yt))

  let cgrid2_xg = Array2D.zeroCreate nx ny 
  let cgrid2_yg =  Array2D.zeroCreate nx ny 
  let u = Array2D.zeroCreate nx ny 
  let v = Array2D.zeroCreate nx ny 
  let q = 2.0
  for i = 0 to nx - 1 do
    let x = (float i - float nx / 2.0 + 0.5) * dx
    for j = 0 to ny - 1 do
      let y = (float j - float ny / 2.0 + 0.5) * dy
      cgrid2_xg.[i,j] <- x;
      cgrid2_yg.[i,j] <- y;
      let b = ymax / 4.0 * (3.0 - cos (pi * x / xmax))
      u.[i,j] <- q * ymax / b;
      v.[i,j] <- 0.0
  let clev = Array.init nc (fun i -> q + float i * q / float (nc - 1))

  pl.env(xmin, xmax, ymin, ymax, AxesScale.Independent, AxisBox.BoxTicksLabels);
  pl.lab ("(x)", "(y)", "#frPLplot Example 22 - constriction with plstransform");
  pl.col0 2;
  pl.shades (u, DefinedFunc(fun _ _ -> true),
    (xmin + dx / 2.0), (xmax - dx / 2.0),
    (ymin + dy / 2.0), (ymax - dy / 2.0),
    clev, 0.0, 1, 1.0, false, null);
  pl.vect (u, v, -1.0, pl.tr2(cgrid2_xg, cgrid2_yg));
  pl.path (nseg, xmin, ymin, xmax, ymin);
  pl.path (nseg, xmin, ymax, xmax, ymax);
  pl.col0 1;

  pl.stransform null
  ()


[<EntryPoint>]
let main argv =

  let mutable argv = argv
  (* Parse and process command line arguments *)
  pl.parseopts( &argv, ParseOpts.Full ||| ParseOpts.NoProgram ) |> ignore

  (*itialize plplot *)
  pl.init ();

  circulation ();

  let fill = false

  (* Set arrow style using arrow_x and arrow_y then
     plot using these arrows. *)
  pl.svect (arrow_x, arrow_y, fill);
  constriction ( 1 );

  (* Set arrow style using arrow2_x and arrow2_y then
     plot using these filled arrows. *)
  let fill = true
  pl.svect (arrow2_x, arrow2_y, fill);
  constriction ( 2 );

  constriction2 ();

  (* Reset arrow style to the default *)
  pl.svect (null, null, fill)

  potential ();

  (pl :> IDisposable).Dispose()
  0

