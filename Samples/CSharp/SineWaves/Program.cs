using PLplot;
using System;
using System.Reflection;

namespace SineWaves
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // generate data for plotting
            const double sineFactor = 0.012585;
            const int exampleCount = 1000;
            var phaseOffset = 125;

            var x0 = new double[exampleCount];
            var y0 = new double[exampleCount];

            for (var j = 0; j < exampleCount; j++)
            {
                x0[j] = j;
                y0[j] = (double)(System.Math.Sin(sineFactor * (j + phaseOffset)) * 1);
            }

            var x1 = new double[exampleCount];
            var y1 = new double[exampleCount];
            phaseOffset = 250;

            for (var j = 0; j < exampleCount; j++)
            {
                x1[j] = j;
                y1[j] = (double)(System.Math.Sin(sineFactor * (j + phaseOffset)) * 1);
            }

            var x2 = new double[exampleCount];
            var y2 = new double[exampleCount];
            phaseOffset = 375;

            for (var j = 0; j < exampleCount; j++)
            {
                x2[j] = j;
                y2[j] = (double)(System.Math.Sin(sineFactor * (j + phaseOffset)) * 1);
            }

            var x3 = new double[exampleCount];
            var y3 = new double[exampleCount];
            phaseOffset = 500;

            for (var j = 0; j < exampleCount; j++)
            {
                x3[j] = j;
                y3[j] = (double)(System.Math.Sin(sineFactor * (j + phaseOffset)) * 1);
            }

            // create PLplot object
            var pl = new PLStream();

            // use SVG backend and write to SineWaves.svg in current directory
            if (args.Length == 1 && args[0] == "svg") 
            {
                pl.sdev("svg");
                pl.sfnam("SineWaves.svg");
            } 
            else 
            {
                pl.sdev("pngcairo");
                pl.sfnam("SineWaves.png");
            }

            // use white background with black foreground
            pl.spal0("cmap0_alternate.pal");

            // Initialize plplot
            pl.init();

            // set axis limits
            const int xMin = 0;
            const int xMax = 1000;
            const int yMin = -1;
            const int yMax = 1;
            pl.env(xMin, xMax, yMin, yMax, AxesScale.Independent, AxisBox.BoxTicksLabelsAxes);

            // Set scaling for mail title text 125% size of default
            pl.schr(0, 1.25);

            // The main title
            pl.lab("X", "Y", "PLplot demo of four sine waves");

            // plot using different colors
            // see http://plplot.sourceforge.net/examples.php?demo=02 for palette indices
            pl.col0(9);
            pl.line(x0, y0);
            pl.col0(1);
            pl.line(x1, y1);
            pl.col0(2);
            pl.line(x2, y2);
            pl.col0(4);
            pl.line(x3, y3);

            // end page (writes output to disk)
            pl.eop();

            // output version
            pl.gver(out var verText);
            Console.WriteLine("PLplot version " + verText);
        }

    }

}