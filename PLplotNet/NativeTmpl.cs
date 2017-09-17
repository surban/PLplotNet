using System;
using System.Text;
using System.Runtime.InteropServices;


namespace PLplot
{
    // type aliases
    using PLBOOL = Boolean;
    using PLCHAR = Char;
    using PLUTF8 = Byte;
    using PLINT = Int32;
    using PLFLT = Double;
    using PLUNICODE = UInt32;
    using PLPointer = IntPtr;
    using FILE = IntPtr;
    using PLUTF8_STRING = String;     // 8-bit string in UTF8 encoding
    using PLCHAR_STRING = String;     // 8-bit string in ANSI encoding
    using PLFLT_MATRIX = IntPtr;      // input matrix
    using PLFLT_NC_MATRIX = IntPtr;   // output matrix


    // Native types and functions from PLplot
    public static partial class Native
    {
        // Set format of numerical label for contours
        [DllImport(DllName, EntryPoint = "c_pl_setcontlabelformat")]
        public static extern void setcontlabelformat(PLINT lexp, PLINT sigdig);

        // Set parameters of contour labelling other than format of numerical label
        [DllImport(DllName, EntryPoint = "c_pl_setcontlabelparam")]
        public static extern void setcontlabelparam(PLFLT offset, PLFLT size, PLFLT spacing, PLINT active);

        // Advance to subpage "page", or to the next one if "page" = 0.
        [DllImport(DllName, EntryPoint = "c_pladv")]
        public static extern void adv(PLINT page);

        // Plot an arc
        [DllImport(DllName, EntryPoint = "c_plarc")]
        public static extern void arc(PLFLT x, PLFLT y, PLFLT a, PLFLT b, PLFLT angle1, PLFLT angle2,
                                      PLFLT rotate, [MarshalAs(UnmanagedType.Bool)] PLBOOL fill);

        /// <summary>plaxes: Draw a box with axes, etc. with arbitrary origin</summary>
        /// <param name="nxsub">Number of subintervals between major x axis ticks for minor ticks. If it is set to zero, PLplot automatically generates a suitable minor tick interval.</param>
        /// <param name="nysub">Number of subintervals between major y axis ticks for minor ticks. If it is set to zero, PLplot automatically generates a suitable minor tick interval.</param>
        /// <param name="x0">World X coordinate of origin.</param>
        /// <param name="xopt">One or more string concatenated from <see cref="XYAxisOpt"/>.</param>
        /// <param name="xtick">World coordinate interval between major ticks on the x axis. If it is set to zero, PLplot automatically generates a suitable tick interval.</param>
        /// <param name="y0">World Y coordinate of origin.</param>
        /// <param name="yopt">One or more string concatenated from <see cref="XYAxisOpt"/>.</param>
        /// <param name="ytick">World coordinate interval between major ticks on the y axis. If it is set to zero, PLplot automatically generates a suitable tick interval.</param>
        /// <remarks>Draws a box around the currently defined viewport with arbitrary world-coordinate origin specified by x0 and y0 and labels it with world coordinate values appropriate to the window. Thus plaxes should only be called after defining both viewport and window. The ascii character strings xopt and yopt specify how the box should be drawn as described below. If ticks and/or subticks are to be drawn for a particular axis, the tick intervals and number of subintervals may be specified explicitly, or they may be defaulted by setting the appropriate arguments to zero.</remarks>
        [DllImport(DllName, EntryPoint = "c_plaxes")]
        public static extern void axes(PLFLT x0, PLFLT y0,
                                       [MarshalAs(UnmanagedType.LPStr)] string xopt, PLFLT xtick, PLINT nxsub,
                                       [MarshalAs(UnmanagedType.LPStr)] string yopt, PLFLT ytick, PLINT nysub);

        // Plot a histogram using x to store data values and y to store frequencies
        [DllImport(DllName, EntryPoint = "c_plbin")]
        private static extern void _bin(PLINT nbin,
                                        [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                        [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y, Bin opt);

        public static void bin(PLFLT[] x, PLFLT[] y, Bin opt)
        {
            _bin(GetSize(x, y), x, y, opt);
        }

        // Calculate broken-down time from continuous time for current stream.
        [DllImport(DllName, EntryPoint = "c_plbtime")]
        public static extern void btime(out PLINT year, out PLINT month, out PLINT day, out PLINT hour,
                                        out PLINT min, out PLFLT sec, PLFLT ctime);

        // Start new page.  Should only be used with pleop().
        [DllImport(DllName, EntryPoint = "c_plbop")]
        public static extern void bop();

        /// <summary>plbox: Draw a box with axes, etc</summary>
        /// <param name="nxsub">Number of subintervals between major x axis ticks for minor ticks. If it is set to zero, PLplot automatically generates a suitable minor tick interval.</param>
        /// <param name="nysub">Number of subintervals between major y axis ticks for minor ticks. If it is set to zero, PLplot automatically generates a suitable minor tick interval.</param>
        /// <param name="xopt">One or more string concatenated from <see cref="XYAxisOpt"/>.</param>
        /// <param name="xtick">World coordinate interval between major ticks on the x axis. If it is set to zero, PLplot automatically generates a suitable tick interval.</param>
        /// <param name="yopt">One or more string concatenated from <see cref="XYAxisOpt"/>.</param>
        /// <param name="ytick">World coordinate interval between major ticks on the y axis. If it is set to zero, PLplot automatically generates a suitable tick interval.</param>
        /// <remarks>Draws a box around the currently defined viewport, and labels it with world coordinate values appropriate to the window. Thus plbox should only be called after defining both viewport and window. The ascii character strings xopt and yopt specify how the box should be drawn as described below. If ticks and/or subticks are to be drawn for a particular axis, the tick intervals and number of subintervals may be specified explicitly, or they may be defaulted by setting the appropriate arguments to zero.</remarks>
        [DllImport(DllName, EntryPoint = "c_plbox")]
        public static extern void box([MarshalAs(UnmanagedType.LPStr)] string xopt, PLFLT xtick, PLINT nxsub,
                                      [MarshalAs(UnmanagedType.LPStr)] string yopt, PLFLT ytick, PLINT nysub);

        /// <summary>plbox3: Draw a box with axes, etc, in 3-d</summary>
        /// <param name="nxsub">Number of subintervals between major x axis ticks for minor ticks. If it is set to zero, PLplot automatically generates a suitable minor tick interval.</param>
        /// <param name="nysub">Number of subintervals between major y axis ticks for minor ticks. If it is set to zero, PLplot automatically generates a suitable minor tick interval.</param>
        /// <param name="nzsub">Number of subintervals between major z axis ticks for minor ticks. If it is set to zero, PLplot automatically generates a suitable minor tick interval.</param>
        /// <param name="xlabel">A UTF-8 character string specifying the text label for the x axis. It is only drawn if u is in the xopt string.</param>
        /// <param name="xopt">One or more string concatenated from <see cref="XYAxisOpt"/>.</param>
        /// <param name="xtick">World coordinate interval between major ticks on the x axis. If it is set to zero, PLplot automatically generates a suitable tick interval.</param>
        /// <param name="ylabel">A UTF-8 character string specifying the text label for the y axis. It is only drawn if u is in the yopt string.</param>
        /// <param name="yopt">One or more string concatenated from <see cref="XYAxisOpt"/>.</param>
        /// <param name="ytick">World coordinate interval between major ticks on the y axis. If it is set to zero, PLplot automatically generates a suitable tick interval.</param>
        /// <param name="zlabel">A UTF-8 character string specifying the text label for the z axis. It is only drawn if u or v are in the zopt string.</param>
        /// <param name="zopt">One or more string concatenated from <see cref="ZAxisOpt"/>.</param>
        /// <param name="ztick">World coordinate interval between major ticks on the z axis. If it is set to zero, PLplot automatically generates a suitable tick interval.</param>
        /// <remarks>Draws axes, numeric and text labels for a three-dimensional surface plot. For a more complete description of three-dimensional plotting see .</remarks>
        [DllImport(DllName, EntryPoint = "c_plbox3")]
        public static extern void box3([MarshalAs(UnmanagedType.LPStr)] string xopt, [MarshalAs(UnmanagedType.LPStr)] string xlabel, PLFLT xtick, PLINT nxsub,
                                       [MarshalAs(UnmanagedType.LPStr)] string yopt, [MarshalAs(UnmanagedType.LPStr)] string ylabel, PLFLT ytick, PLINT nysub,
                                       [MarshalAs(UnmanagedType.LPStr)] string zopt, [MarshalAs(UnmanagedType.LPStr)] string zlabel, PLFLT ztick, PLINT nzsub);

        // Calculate world coordinates and subpage from relative device coordinates.
        [DllImport(DllName, EntryPoint = "c_plcalc_world")]
        public static extern void calc_world(PLFLT rx, PLFLT ry, out PLFLT wx, out PLFLT wy, out PLINT window);

        // Clear current subpage.
        [DllImport(DllName, EntryPoint = "c_plclear")]
        public static extern void clear();

        // Set color, map 0.  Argument is integer between 0 and 15.
        [DllImport(DllName, EntryPoint = "c_plcol0")]
        public static extern void col0(PLINT icol0);

        // Set color, map 1.  Argument is a float between 0. and 1.
        [DllImport(DllName, EntryPoint = "c_plcol1")]
        public static extern void col1(PLFLT col1);

        // Configure transformation between continuous and broken-down time (and
        // vice versa) for current stream.
        [DllImport(DllName, EntryPoint = "c_plconfigtime")]
        public static extern void configtime(PLFLT scale, PLFLT offset1, PLFLT offset2, PLINT ccontrol,
                                             [MarshalAs(UnmanagedType.Bool)] PLBOOL ifbtime_offset,
                                             PLINT year, PLINT month, PLINT day, PLINT hour, PLINT min, PLFLT sec);

        // Draws a contour plot from data in f(nx,ny).  Is just a front-end to
        // plfcont, with a particular choice for f2eval and f2eval_data.
        [DllImport(DllName, EntryPoint = "c_plcont")]
        private static extern void _cont(PLFLT_MATRIX f, PLINT nx, PLINT ny, PLINT kx, PLINT lx,
                                         PLINT ky, PLINT ly, [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel,
                                         TransformFunc pltr, PLPointer pltr_data);

        /// <summary>plcont: Contour plot</summary>
        /// <param name="clevel">A vector specifying the levels at which to draw contours.</param>
        /// <param name="f">A matrix containing data to be contoured.</param>
        /// <param name="kx">Start of x indices to consider</param>
        /// <param name="lx">End (exclusive) of x indices to consider</param>
        /// <param name="ky">Start of y indices to consider</param>
        /// <param name="ly">End (exclusive) of y indices to consider</param>        
        /// <param name="pltr">A callback function that defines the transformation between the zero-based indices of the matrix f and the world coordinates.For the C case, transformation functions are provided in the PLplot library: pltr0 for the identity mapping, and pltr1 and pltr2 for arbitrary mappings respectively defined by vectors and matrices. In addition, C callback routines for the transformation can be supplied by the user such as the mypltr function in examples/c/x09c.c which provides a general linear transformation between index coordinates and world coordinates.For languages other than C you should consult  for the details concerning how PLTRANSFORM_callback arguments are interfaced. However, in general, a particular pattern of callback-associated arguments such as a tr vector with 6 elements; xg and yg vectors; or xg and yg matrices are respectively interfaced to a linear-transformation routine similar to the above mypltr function; pltr1; and pltr2. Furthermore, some of our more sophisticated bindings (see, e.g., ) support native language callbacks for handling index to world-coordinate transformations. Examples of these various approaches are given in examples/ltlanguagegtx09*, examples/ltlanguagegtx16*, examples/ltlanguagegtx20*, examples/ltlanguagegtx21*, and examples/ltlanguagegtx22*, for all our supported languages.</param>
        /// <param name="pltr_data">Extra parameter to help pass information to pltr0, pltr1, pltr2, or whatever callback routine that is externally supplied.</param>
        /// <remarks>Draws a contour plot of the data in f[nx][ny], using the nlevel contour levels specified by clevel. Only the region of the matrix from kx to lx and from ky to ly is plotted out where all these index ranges are interpreted as one-based for historical reasons. A transformation routine pointed to by pltr with a generic pointer pltr_data for additional data required by the transformation routine is used to map indices within the matrix to the world coordinates.</remarks>
        public static void cont(PLFLT[,] f, PLINT kx, PLINT lx, PLINT ky, PLINT ly,
                                PLFLT[] clevel, TransformFunc pltr, PLPointer pltr_data)
        {
            using (var mat_f = new MatrixMarshaller(f))
            {
                CheckRange("kx", 0, mat_f.NX, kx); CheckRange("lx", kx+1, mat_f.NX, lx);
                CheckRange("ky", 0, mat_f.NY, ky); CheckRange("ly", ky+1, mat_f.NY, ly);
                _cont(mat_f.Ptr, mat_f.NX, mat_f.NY, kx+1, lx+1, ky+1, ly+1, clevel, clevel.Length, pltr, pltr_data);
            }            
        }

        // Copies state parameters from the reference stream to the current stream.
        [DllImport(DllName, EntryPoint = "c_plcpstrm")]
        public static extern void cpstrm(PLINT iplsr, [MarshalAs(UnmanagedType.Bool)] PLBOOL flags);

        // Calculate continuous time from broken-down time for current stream.
        [DllImport(DllName, EntryPoint = "c_plctime")]
        public static extern void ctime(PLINT year, PLINT month, PLINT day, PLINT hour, PLINT min, PLFLT sec, out PLFLT ctime);

        // Converts input values from relative device coordinates to relative plot
        // coordinates.
        [DllImport(DllName, EntryPoint = "pldid2pc")]
        public static extern void did2pc(out PLFLT xmin, out PLFLT ymin, out PLFLT xmax, out PLFLT ymax);

        // Converts input values from relative plot coordinates to relative
        // device coordinates.
        [DllImport(DllName, EntryPoint = "pldip2dc")]
        public static extern void dip2dc(out PLFLT xmin, out PLFLT ymin, out PLFLT xmax, out PLFLT ymax);

        // End a plotting session for all open streams.
        [DllImport(DllName, EntryPoint = "c_plend")]
        public static extern void end();

        // End a plotting session for the current stream only.
        [DllImport(DllName, EntryPoint = "c_plend1")]
        public static extern void end1();

        // Simple interface for defining viewport and window.
        [DllImport(DllName, EntryPoint = "c_plenv")]
        public static extern void env(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax,
                                      AxesScale just, AxisBox axis);

        // similar to plenv() above, but in multiplot mode does not advance the subpage,
        // instead the current subpage is cleared
        [DllImport(DllName, EntryPoint = "c_plenv0")]
        public static extern void env0(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax,
                                       AxesScale just, AxisBox axis);

        // End current page.  Should only be used with plbop().
        [DllImport(DllName, EntryPoint = "c_pleop")]
        public static extern void eop();

        // Plot horizontal error bars (xmin(i),y(i)) to (xmax(i),y(i))
        [DllImport(DllName, EntryPoint = "c_plerrx")]
        private static extern void _errx(PLINT n,
                                        [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] xmin,
                                        [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] xmax,
                                        [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y);

        public static void _errx(PLFLT[] xmin, PLFLT[] xmax, PLFLT[] y)
        {
            _errx(GetSize(xmin, xmax, y), xmin, xmax, y);
        }                                    

        // Plot vertical error bars (x,ymin(i)) to (x(i),ymax(i))
        [DllImport(DllName, EntryPoint = "c_plerry")]
        private static extern void _erry(PLINT n,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] ymin,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] ymax);

        public static void erry(PLFLT[] x, PLFLT[] ymin, PLFLT[] ymax)
        {
            _erry(GetSize(x, ymin, ymax), x, ymin, ymax);
        }

        // Advance to the next family file on the next new page
        [DllImport(DllName, EntryPoint = "c_plfamadv")]
        public static extern void famadv();

        // Pattern fills the polygon bounded by the input points.
        [DllImport(DllName, EntryPoint = "c_plfill")]
        private static extern void _fill(PLINT n,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y);

        public static void fill(PLFLT[] x, PLFLT[] y)
        {
            _fill(GetSize(x, y), x, y);
        }                                                 

        // Pattern fills the 3d polygon bounded by the input points.
        [DllImport(DllName, EntryPoint = "c_plfill3")]
        private static extern void _fill3(PLINT n,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z);

        public static void fill3(PLFLT[] x, PLFLT[] y, PLFLT[] z)
        {
            _fill3(GetSize(x, y, z), x, y, z);
        }

        // Flushes the output stream.  Use sparingly, if at all.
        [DllImport(DllName, EntryPoint = "c_plflush")]
        public static extern void flush();

        // Sets the global font flag to 'ifont'.
        [DllImport(DllName, EntryPoint = "c_plfont")]
        public static extern void font(FontFlag ifont);

        // Load specified font set.
        [DllImport(DllName, EntryPoint = "c_plfontld")]
        public static extern void fontld(PLINT fnt);

        // Get character default height and current (scaled) height
        [DllImport(DllName, EntryPoint = "c_plgchr")]
        public static extern void gchr(out PLFLT p_def, out PLFLT p_ht);

        // Returns 8 bit RGB values for given color from color map 0
        [DllImport(DllName, EntryPoint = "c_plgcol0")]
        public static extern void gcol0(PLINT icol0, out PLINT r, out PLINT g, out PLINT b);

        // Returns 8 bit RGB values for given color from color map 0 and alpha value
        [DllImport(DllName, EntryPoint = "c_plgcol0a")]
        public static extern void gcol0a(PLINT icol0, out PLINT r, out PLINT g, out PLINT b, out PLFLT alpha);

        // Returns the background color by 8 bit RGB value
        [DllImport(DllName, EntryPoint = "c_plgcolbg")]
        public static extern void gcolbg(out PLINT r, out PLINT g, out PLINT b);

        // Returns the background color by 8 bit RGB value and alpha value
        [DllImport(DllName, EntryPoint = "c_plgcolbga")]
        public static extern void gcolbga(out PLINT r, out PLINT g, out PLINT b, out PLFLT alpha);

        // Returns the current compression setting
        [DllImport(DllName, EntryPoint = "c_plgcompression")]
        public static extern void gcompression(out PLINT compression);

        // Get the current device (keyword) name (max. 80 chars)
        [DllImport(DllName, EntryPoint = "c_plgdev")]
        private static extern void _gdev([MarshalAs(UnmanagedType.LPStr)] StringBuilder p_dev);

        public static void gdev(out string p_dev)
        {
            var sb_p_dev = new StringBuilder(MaxLength);
            _gdev(sb_p_dev);
            p_dev = sb_p_dev.ToString();
        }

        // Retrieve current window into device space
        [DllImport(DllName, EntryPoint = "c_plgdidev")]
        public static extern void gdidev(out PLFLT p_mar, out PLFLT p_aspect, out PLFLT p_jx, out PLFLT p_jy);

        // Get plot orientation
        [DllImport(DllName, EntryPoint = "c_plgdiori")]
        public static extern void gdiori(out PLFLT p_rot);

        // Retrieve current window into plot space
        [DllImport(DllName, EntryPoint = "c_plgdiplt")]
        public static extern void gdiplt(out PLFLT p_xmin, out PLFLT p_ymin, out PLFLT p_xmax, out PLFLT p_ymax);

        // Get the drawing mode
        [DllImport(DllName, EntryPoint = "c_plgdrawmode")]
        public static extern DrawMode gdrawmode();

        // Get FCI (font characterization integer)
        [DllImport(DllName, EntryPoint = "c_plgfci")]
        public static extern void gfci(out FCI p_fci);

        // Get family file parameters
        [DllImport(DllName, EntryPoint = "c_plgfam")]
        public static extern void gfam(out PLINT p_fam, out PLINT p_num, out PLINT p_bmax);

        // Get the (current) output file name.  Must be preallocated to >80 bytes
        [DllImport(DllName, EntryPoint = "c_plgfnam")]
        private static extern void _gfnam([MarshalAs(UnmanagedType.LPStr)] StringBuilder fnam);

        public static void gfnam(out string fnam)
        {
            var sb_fnam = new StringBuilder(MaxLength);
            _gfnam(sb_fnam);
            fnam = sb_fnam.ToString();
        }

        // Get the current font family, style and weight
        [DllImport(DllName, EntryPoint = "c_plgfont")]
        public static extern void gfont(out FontFamily p_family, out FontStyle p_style, out FontWeight p_weight);

        // Get the (current) run level.
        [DllImport(DllName, EntryPoint = "c_plglevel")]
        public static extern void glevel(out RunLevel p_level);

        // Get output device parameters.
        [DllImport(DllName, EntryPoint = "c_plgpage")]
        public static extern void gpage(out PLFLT p_xp, out PLFLT p_yp,
                                        out PLINT p_xleng, out PLINT p_yleng, out PLINT p_xoff, out PLINT p_yoff);

        // Switches to graphics screen.
        [DllImport(DllName, EntryPoint = "c_plgra")]
        public static extern void gra();

        // Draw gradient in polygon.
        [DllImport(DllName, EntryPoint = "c_plgradient")]
        private static extern void _gradient(PLINT n,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y, PLFLT angle);

        public static void gradient(PLFLT[] x, PLFLT[] y, PLFLT angle)
        {
            _gradient(GetSize(x, y), x, y, angle);
        }

        // grid irregularly sampled data
        [DllImport(DllName, EntryPoint = "c_plgriddata")]
        private static extern void _griddata(
                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z, PLINT npts,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] xg, PLINT nptsx,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] yg, PLINT nptsy,
                    PLFLT_NC_MATRIX zg, Grid type, PLFLT data);

        public static void griddata(PLFLT[] x, PLFLT[] y, PLFLT[] z, PLFLT[] xg, PLFLT[] yg, PLFLT[,] zg, Grid type, PLFLT data)
        {
            using (var mat_zg = new MatrixMarshaller(zg))
            {
                if (mat_zg.NX != xg.Length || mat_zg.NY != yg.Length)
                    throw new ArgumentException("zg must have size of xg times yg");
                _griddata(x, y, z, GetSize(x, y, z), xg, xg.Length, yg, yg.Length, mat_zg.Ptr, type, data);
            }
        }                  

        // Get subpage boundaries in absolute coordinates
        [DllImport(DllName, EntryPoint = "c_plgspa")]
        public static extern void gspa(out PLFLT xmin, out PLFLT xmax, out PLFLT ymin, out PLFLT ymax);

        // Get current stream number.
        [DllImport(DllName, EntryPoint = "c_plgstrm")]
        public static extern void gstrm(out PLINT p_strm);

        // Get the current library version number
        [DllImport(DllName, EntryPoint = "c_plgver")]
        private static extern void _gver([MarshalAs(UnmanagedType.LPStr)] StringBuilder p_ver);
        
        public static void gver(out string p_ver)
        {
            var sb_p_ver = new StringBuilder(MaxLength);
            _gver(sb_p_ver);
            p_ver = sb_p_ver.ToString();
        }

        // Get viewport boundaries in normalized device coordinates
        [DllImport(DllName, EntryPoint = "c_plgvpd")]
        public static extern void gvpd(out PLFLT p_xmin, out PLFLT p_xmax, out PLFLT p_ymin, out PLFLT p_ymax);

        // Get viewport boundaries in world coordinates
        [DllImport(DllName, EntryPoint = "c_plgvpw")]
        public static extern void gvpw(out PLFLT p_xmin, out PLFLT p_xmax, out PLFLT p_ymin, out PLFLT p_ymax);

        // Get x axis labeling parameters
        [DllImport(DllName, EntryPoint = "c_plgxax")]
        public static extern void gxax(out PLINT p_digmax, out PLINT p_digits);

        // Get y axis labeling parameters
        [DllImport(DllName, EntryPoint = "c_plgyax")]
        public static extern void gyax(out PLINT p_digmax, out PLINT p_digits);

        // Get z axis labeling parameters
        [DllImport(DllName, EntryPoint = "c_plgzax")]
        public static extern void gzax(out PLINT p_digmax, out PLINT p_digits);

        // Draws a histogram of n values of a variable in array data[0..n-1]
        [DllImport(DllName, EntryPoint = "c_plhist")]
        private static extern void _hist(PLINT n, [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] data, 
                                         PLFLT datmin, PLFLT datmax, PLINT nbin, Hist opt);

        public static void hist(PLFLT[] data, PLFLT datmin, PLFLT datmax, PLINT nbin, Hist opt)
        {
            _hist(data.Length, data, datmin, datmax, nbin, opt);
        }                                         

        // Functions for converting between HLS and RGB color space
        [DllImport(DllName, EntryPoint = "c_plhlsrgb")]
        public static extern void hlsrgb(PLFLT h, PLFLT l, PLFLT s, out PLFLT p_r, out PLFLT p_g, out PLFLT p_b);

        // Initializes PLplot, using preset or default options
        [DllImport(DllName, EntryPoint = "c_plinit")]
        public static extern void init();

        // Draws a line segment from (x1, y1) to (x2, y2).
        [DllImport(DllName, EntryPoint = "c_pljoin")]
        public static extern void join(PLFLT x1, PLFLT y1, PLFLT x2, PLFLT y2);

        // Simple routine for labelling graphs.
        [DllImport(DllName, EntryPoint = "c_pllab")]
        public static extern void lab([MarshalAs(UnmanagedType.LPStr)] string xlabel,
                                      [MarshalAs(UnmanagedType.LPStr)] string ylabel,
                                      [MarshalAs(UnmanagedType.LPStr)] string tlabel);

        // Routine for drawing discrete line, symbol, or cmap0 legends
        [DllImport(DllName, EntryPoint = "c_pllegend")]
        private static extern void _legend(
                    out PLFLT p_legend_width, out PLFLT p_legend_height,
                    Legend opt, Position position, PLFLT x, PLFLT y, PLFLT plot_width,
                    PLINT bg_color, PLINT bb_color, LineStyle bb_style,
                    PLINT nrow, PLINT ncolumn,
                    PLINT nlegend, 
                    [In, MarshalAs(UnmanagedType.LPArray)] LegendEntry[] opt_array,
                    PLFLT text_offset, PLFLT text_scale, PLFLT text_spacing, PLFLT text_justification,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] text_colors,
                    [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] PLUTF8_STRING[] text,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] box_colors, 
                    [In, MarshalAs(UnmanagedType.LPArray)] Pattern[] box_patterns,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] box_scales, 
                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] box_line_widths,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] line_colors, 
                    [In, MarshalAs(UnmanagedType.LPArray)] LineStyle[] line_styles,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] line_widths,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] symbol_colors, 
                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] symbol_scales,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] symbol_numbers,
                    [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] PLUTF8_STRING[] symbols);

        public static void legend(
                    out PLFLT p_legend_width, out PLFLT p_legend_height,
                    Legend opt, Position position, PLFLT x, PLFLT y, PLFLT plot_width,
                    PLINT bg_color, PLINT bb_color, LineStyle bb_style,
                    PLINT nrow, PLINT ncolumn,
                    LegendEntry[] opt_array,
                    PLFLT text_offset, PLFLT text_scale, PLFLT text_spacing, PLFLT text_justification,
                    PLINT[] text_colors, PLUTF8_STRING[] text, PLINT[] box_colors, Pattern[] box_patterns,
                    PLFLT[] box_scales, PLFLT[] box_line_widths, PLINT[] line_colors,  LineStyle[] line_styles,
                    PLFLT[] line_widths, PLINT[] symbol_colors, PLFLT[] symbol_scales, PLINT[] symbol_numbers,
                    PLUTF8_STRING[] symbols)
        {
            PLINT nlegend = opt_array.Length;
            if (nlegend != text_colors.Length)
                throw new ArgumentException("opt_array and text_colors must have same length");  
            if (nlegend != text.Length)
                throw new ArgumentException("opt_array and text must have same length");  
            if (nlegend != box_colors.Length)
                throw new ArgumentException("opt_array and box_colors must have same length");  
            if (nlegend != box_patterns.Length)
                throw new ArgumentException("opt_array and box_patterns must have same length");  
            if (nlegend != box_scales.Length)
                throw new ArgumentException("opt_array and box_scales must have same length");  
            if (nlegend != box_line_widths.Length)
                throw new ArgumentException("opt_array and box_line_widths must have same length");  
            if (nlegend != line_styles.Length)
                throw new ArgumentException("opt_array and line_styles must have same length");  
            if (nlegend != line_widths.Length)
                throw new ArgumentException("opt_array and line_widths must have same length");  
            if (nlegend != symbol_colors.Length)
                throw new ArgumentException("opt_array and symbol_colors must have same length");  
            if (nlegend != symbol_scales.Length)
                throw new ArgumentException("opt_array and symbol_scales must have same length");  
            if (nlegend != symbol_numbers.Length)
                throw new ArgumentException("opt_array and symbol_numbers must have same length");       
            if (nlegend != symbols.Length)
                throw new ArgumentException("opt_array and symbols must have same length");                                                                                                                                                                            
            _legend(out p_legend_width, out p_legend_height,
                    opt, position, x, y, plot_width,
                    bg_color, bb_color, bb_style,
                    nrow, ncolumn, nlegend,
                    opt_array,
                    text_offset, text_scale, text_spacing, text_justification,
                    text_colors, text, box_colors, box_patterns,
                    box_scales, box_line_widths, line_colors, line_styles,
                    line_widths, symbol_colors, symbol_scales, symbol_numbers,
                    symbols);                
        }                    

        // Routine for drawing continuous colour legends
        [DllImport(DllName, EntryPoint = "c_plcolorbar")]
        private static extern void _colorbar(
                    out PLFLT p_colorbar_width, out PLFLT p_colorbar_height,
                    Colorbar opt, Position position, PLFLT x, PLFLT y,
                    PLFLT x_length, PLFLT y_length,
                    PLINT bg_color, PLINT bb_color, PLINT bb_style,
                    PLFLT low_cap_color, PLFLT high_cap_color,
                    PLINT cont_color, PLFLT cont_width,
                    PLINT n_labels, 
                    [In, MarshalAs(UnmanagedType.LPArray)] ColorbarLabel[] label_opts,
                    [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] PLUTF8_STRING[] labels,
                    PLINT n_axes, 
                    [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] PLCHAR_STRING[] axis_opts,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] ticks, 
                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] sub_ticks,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] n_values, PLFLT_MATRIX values);

        public static void colorbar(
                    out PLFLT p_colorbar_width, out PLFLT p_colorbar_height,
                    Colorbar opt, Position position, PLFLT x, PLFLT y,
                    PLFLT x_length, PLFLT y_length,
                    PLINT bg_color, PLINT bb_color, PLINT bb_style,
                    PLFLT low_cap_color, PLFLT high_cap_color,
                    PLINT cont_color, PLFLT cont_width,
                    ColorbarLabel[] label_opts, PLUTF8_STRING[] labels,
                    PLCHAR_STRING[] axis_opts, PLFLT[] ticks, PLINT[] sub_ticks,
                    PLFLT[,] values)
        {
            using (var mat_values = new MatrixMarshaller(values))
            {
                if (label_opts.Length != labels.Length)
                    throw new ArgumentException("label_opts and labels must have same length");                
                if (axis_opts.Length != ticks.Length || ticks.Length != sub_ticks.Length)
                    throw new ArgumentException("axis_opts and ticks and sub_ticks must have same length");                
                PLINT[] n_values = new PLINT[mat_values.NX];
                for (int row=0; row < mat_values.NX; row++)
                    n_values[row] = mat_values.NY;
                _colorbar(out p_colorbar_width, out p_colorbar_height,
                          opt, position, x, y,
                          x_length, y_length,
                          bg_color, bb_color, bb_style,
                          low_cap_color, high_cap_color,
                          cont_color, cont_width,
                          label_opts.Length, label_opts, labels, 
                          axis_opts.Length, axis_opts, ticks, sub_ticks,
                          n_values, mat_values.Ptr);
            }
        }                    

        // Sets position of the light source
        [DllImport(DllName, EntryPoint = "c_pllightsource")]
        public static extern void lightsource(PLFLT x, PLFLT y, PLFLT z);

        // Draws line segments connecting a series of points.
        [DllImport(DllName, EntryPoint = "c_plline")]
        private static extern void _line(PLINT n,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y);

        public static void line(PLFLT[] x, PLFLT[] y)
        {
            _line(GetSize(x, y), x, y);
        }

        // Draws a line in 3 space.
        [DllImport(DllName, EntryPoint = "c_plline3")]
        private static extern void _line3(PLINT n,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z);

        public static void line3(PLFLT[] x, PLFLT[] y, PLFLT[] z)
        {
            _line3(GetSize(x, y, z), x, y, z);
        }

        // Set line style.
        [DllImport(DllName, EntryPoint = "c_pllsty")]
        public static extern void lsty(LineStyle lin);

        // Plot continental outline in world coordinates
        [DllImport(DllName, EntryPoint = "c_plmap")]
        public static extern void map(MapFunc mapform, [MarshalAs(UnmanagedType.LPStr)] string name,
                                      PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy);

        // Plot map outlines
        [DllImport(DllName, EntryPoint = "c_plmapline")]
        private static extern void _mapline(MapFunc mapform, [MarshalAs(UnmanagedType.LPStr)] string name,
                                            PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] plotentries, PLINT nplotentries);

        public static void mapline(MapFunc mapform, string name,
                                   PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                   PLINT[] plotentries)
        {
            _mapline(mapform, name, minx, maxx, miny, maxy, plotentries, plotentries.Length);
        }                                   

        // Plot map points
        [DllImport(DllName, EntryPoint = "c_plmapstring")]
        private static extern void _mapstring(MapFunc mapform,
                                              [MarshalAs(UnmanagedType.LPStr)] string name, 
                                              [MarshalAs(UnmanagedType.LPStr)] string str,
                                              PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                              [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] plotentries, PLINT nplotentries);

        public static void mapstring(MapFunc mapform, string name, string str,
                                     PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                     PLINT[] plotentries)                                              
        {
            _mapstring(mapform, name, str, minx, maxx, miny, maxy, plotentries, plotentries.Length);
        }                                              

        // Plot map text
        [DllImport(DllName, EntryPoint = "c_plmaptex")]
        public static extern void maptex(MapFunc mapform,
                                         [MarshalAs(UnmanagedType.LPStr)] string name, PLFLT dx, PLFLT dy, PLFLT just, 
                                         [MarshalAs(UnmanagedType.LPStr)] string text,
                                         PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                         PLINT plotentry);

        // Plot map fills
        [DllImport(DllName, EntryPoint = "c_plmapfill")]
        private static extern void _mapfill(MapFunc mapform,
                                            [MarshalAs(UnmanagedType.LPStr)] string name, PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] plotentries, PLINT nplotentries);

        public static void mapfill(MapFunc mapform, string name, PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                   PLINT[] plotentries)
        {
            _mapfill(mapform, name, minx, maxx, miny, maxy, plotentries, plotentries.Length);
        }                                   

        // Plot the latitudes and longitudes on the background.
        [DllImport(DllName, EntryPoint = "c_plmeridians")]
        public static extern void meridians(MapFunc mapform,
                                            PLFLT dlong, PLFLT dlat,
                                            PLFLT minlong, PLFLT maxlong, PLFLT minlat, PLFLT maxlat);

        // Plots a mesh representation of the function z[x][y].
        [DllImport(DllName, EntryPoint = "c_plmesh")]
        private static extern void _mesh([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                         PLFLT_MATRIX z,
                                         PLINT nx, PLINT ny, Mesh opt);

        public static void mesh(PLFLT[] x, PLFLT[] y, PLFLT[,] z, Mesh opt)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                _mesh(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt);
            }
        }                         
                                       

        // Plots a mesh representation of the function z[x][y] with contour
        [DllImport(DllName, EntryPoint = "c_plmeshc")]
        private static extern void _meshc([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                          PLFLT_MATRIX z,
                                          PLINT nx, PLINT ny, MeshContour opt,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel);
        
        public static void meshc(PLFLT[] x, PLFLT[] y, PLFLT[,] z, MeshContour opt, PLFLT[] clevel)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                _meshc(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt, clevel, clevel.Length);
            }
        }                                                

        // Creates a new stream and makes it the default.
        [DllImport(DllName, EntryPoint = "c_plmkstrm")]
        public static extern void mkstrm(out PLINT p_strm);

        /// <summary>plmtex: Write text relative to viewport boundaries</summary>
        /// <param name="disp">Position of the reference point of string, measured outwards from the specified viewport edge in units of the current character height. Use negative disp to write within the viewport.</param>
        /// <param name="just">Specifies the position of the string relative to its reference point. If just=0. , the reference point is at the left and if just=1. , it is at the right of the string. Other values of just give intermediate justifications.</param>
        /// <param name="pos">Position of the reference point of string along the specified edge, expressed as a fraction of the length of the edge.</param>
        /// <param name="side">One string from <see cref="Side"/>.</param>
        /// <param name="text">A UTF-8 character string to be written out.</param>
        /// <remarks>Writes text at a specified position relative to the viewport boundaries. Text may be written inside or outside the viewport, but is clipped at the subpage boundaries. The reference point of a string lies along a line passing through the string at half the height of a capital letter. The position of the reference point along this line is determined by just, and the position of the reference point relative to the viewport is set by disp and pos.</remarks>
        [DllImport(DllName, EntryPoint = "c_plmtex")]
        public static extern void mtex([MarshalAs(UnmanagedType.LPStr)] string side, PLFLT disp, PLFLT pos, PLFLT just,
                                       [MarshalAs(UnmanagedType.LPStr)] string text);

        /// <summary>plmtex3: Write text relative to viewport boundaries in 3D plots</summary>
        /// <param name="disp">Position of the reference point of string, measured outwards from the specified viewport edge in units of the current character height. Use negative disp to write within the viewport.</param>
        /// <param name="just">Specifies the position of the string relative to its reference point. If just=0. , the reference point is at the left and if just=1. , it is at the right of the string. Other values of just give intermediate justifications.</param>
        /// <param name="pos">Position of the reference point of string along the specified edge, expressed as a fraction of the length of the edge.</param>
        /// <param name="side">One or more strings concatenated from <see cref="Side3"/>.</param>
        /// <param name="text">A UTF-8 character string to be written out.</param>
        /// <remarks>Writes text at a specified position relative to the viewport boundaries. Text may be written inside or outside the viewport, but is clipped at the subpage boundaries. The reference point of a string lies along a line passing through the string at half the height of a capital letter. The position of the reference point along this line is determined by just, and the position of the reference point relative to the viewport is set by disp and pos.</remarks>
        [DllImport(DllName, EntryPoint = "c_plmtex3")]
        public static extern void mtex3([MarshalAs(UnmanagedType.LPStr)] string side, PLFLT disp, PLFLT pos, PLFLT just,
                                        [MarshalAs(UnmanagedType.LPStr)] string text);

        // Plots a 3-d representation of the function z[x][y].
        [DllImport(DllName, EntryPoint = "c_plot3d")]
        private static extern void _plot3d([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                           [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                           PLFLT_MATRIX z,
                                           PLINT nx, PLINT ny, Mesh opt,
                                           [MarshalAs(UnmanagedType.Bool)] PLBOOL side);

        public static void plot3d(PLFLT[] x, PLFLT[] y, PLFLT[,] z, Mesh opt, PLBOOL side)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                _plot3d(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt, side);
            }            
        }

        // Plots a 3-d representation of the function z[x][y] with contour.
        [DllImport(DllName, EntryPoint = "c_plot3dc")]
        private static extern void _plot3dc([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                            PLFLT_MATRIX z,
                                            PLINT nx, PLINT ny, MeshContour opt,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel);

        public static void plot3dc(PLFLT[] x, PLFLT[] y, PLFLT[,] z, MeshContour opt, PLFLT[] clevel)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                _plot3dc(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt, clevel, clevel.Length);
            }            
        }

        // Plots a 3-d representation of the function z[x][y] with contour and
        // y index limits.
        [DllImport(DllName, EntryPoint = "c_plot3dcl")]
        private static extern void _plot3dcl([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                             PLFLT_MATRIX z,
                                             PLINT nx, PLINT ny, MeshContour opt,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel,
                                             PLINT indexxmin, PLINT indexxmax,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] indexymin,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] indexymax);

        public static void plot3dcl(PLFLT[] x, PLFLT[] y, PLFLT[,] z, MeshContour opt, PLFLT[] clevel,
                                    PLINT indexxmin, PLINT[] indexymin, PLINT[] indexymax)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                PLINT indexxmax = indexxmin + GetSize(indexymax, indexymax);
                _plot3dcl(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt, clevel, clevel.Length,
                          indexxmin, indexxmax, indexymin, indexymax);
            }                 
        }                                    

        // Set fill pattern directly.
        [DllImport(DllName, EntryPoint = "c_plpat")]
        private static extern void _pat(PLINT nlin,
                                        [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] inc,
                                        [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] del);

        public static void pat(PLINT[] inc, PLINT[] del)                                     
        {
            PLINT nlin = GetSize(inc, del);
            if (nlin != 1 && nlin != 2)
                throw new ArgumentException("inc and del must be of length 1 or 2");
            _pat(nlin, inc, del);
        }

        // Draw a line connecting two points, accounting for coordinate transforms
        [DllImport(DllName, EntryPoint = "c_plpath")]
        public static extern void path(PLINT n, PLFLT x1, PLFLT y1, PLFLT x2, PLFLT y2);

        // Plots array y against x for n points using ASCII code "code".
        [DllImport(DllName, EntryPoint = "c_plpoin")]
        private static extern void _poin(PLINT n,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                         PLINT code);

        public static void poin(PLFLT[] x, PLFLT[] y, PLINT code)
        {
            _poin(GetSize(x, y), x, y, code);
        }

        // Draws a series of points in 3 space.
        [DllImport(DllName, EntryPoint = "c_plpoin3")]
        private static extern void _poin3(PLINT n,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z,
                                          PLINT code);

        public static void poin3(PLFLT[] x, PLFLT[] y, PLFLT[] z, PLINT code)
        {
            _poin3(GetSize(x, y, z), x, y, z, code);
        }

        // Draws a polygon in 3 space.
        [DllImport(DllName, EntryPoint = "c_plpoly3")]
        private static extern void _poly3(PLINT n,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z,
                                          [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Bool)] PLBOOL[] draw,
                                          [MarshalAs(UnmanagedType.Bool)] PLBOOL ifcc);

        public static void poly3(PLFLT[] x, PLFLT[] y, PLFLT[] z, PLBOOL[] draw, PLBOOL ifcc)
        {
            PLINT n = GetSize(x, y, z);
            if (draw.Length != n)
                throw new ArgumentException("draw must have same length as x, y and z");
            _poly3(n, x, y, z, draw, ifcc);
        }

        // Set the floating point precision (in number of places) in numeric labels.
        [DllImport(DllName, EntryPoint = "c_plprec")]
        public static extern void prec(PLINT setp, PLINT prec);

        // Set fill pattern, using one of the predefined patterns.
        [DllImport(DllName, EntryPoint = "c_plpsty")]
        public static extern void psty(Pattern patt);

        // Prints out "text" at world cooordinate (x,y).
        [DllImport(DllName, EntryPoint = "c_plptex")]
        public static extern void ptex(PLFLT x, PLFLT y, PLFLT dx, PLFLT dy, PLFLT just,
                                       [MarshalAs(UnmanagedType.LPStr)] string text);

        // Prints out "text" at world cooordinate (x,y,z).
        [DllImport(DllName, EntryPoint = "c_plptex3")]
        public static extern void ptex3(PLFLT wx, PLFLT wy, PLFLT wz, PLFLT dx, PLFLT dy, PLFLT dz,
                                        PLFLT sx, PLFLT sy, PLFLT sz, PLFLT just,
                                        [MarshalAs(UnmanagedType.LPStr)] string text);

        // Random number generator based on Mersenne Twister.
        // Obtain real random number in range [0,1].
        [DllImport(DllName, EntryPoint = "c_plrandd")]
        public static extern PLFLT randd();

        // Replays contents of plot buffer to current device/file.
        [DllImport(DllName, EntryPoint = "c_plreplot")]
        public static extern void replot();

        // Functions for converting between HLS and RGB color space
        [DllImport(DllName, EntryPoint = "c_plrgbhls")]
        public static extern void rgbhls(PLFLT r, PLFLT g, PLFLT b, out PLFLT p_h, out PLFLT p_l, out PLFLT p_s);

        // Set character height.
        [DllImport(DllName, EntryPoint = "c_plschr")]
        public static extern void schr(PLFLT def, PLFLT scale);

        // Set color map 0 colors by 8 bit RGB values
        [DllImport(DllName, EntryPoint = "c_plscmap0")]
        private static extern void _scmap0([In, MarshalAs(UnmanagedType.LPArray)] PLINT[] r,
                                           [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] g,
                                           [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] b,
                                           PLINT ncol0);

        public static void scmap0(PLINT[] r, PLINT[] g, PLINT[] b)
        {
            _scmap0(r, g, b, GetSize(r, g, b));
        }                             

        // Set color map 0 colors by 8 bit RGB values and alpha values
        [DllImport(DllName, EntryPoint = "c_plscmap0a")]
        private static extern void _scmap0a([In, MarshalAs(UnmanagedType.LPArray)] PLINT[] r,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] g,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] b,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] alpha,
                                            PLINT ncol0);

        public static void scmap0a(PLINT[] r, PLINT[] g, PLINT[] b, PLFLT[] alpha)
        {
            _scmap0a(r, g, b, alpha, GetSize(r, g, b, alpha));
        }

        // Set number of colors in cmap 0
        [DllImport(DllName, EntryPoint = "c_plscmap0n")]
        public static extern void scmap0n(PLINT ncol0);

        // Set color map 1 colors by 8 bit RGB values
        [DllImport(DllName, EntryPoint = "c_plscmap1")]
        private static extern void _scmap1([In, MarshalAs(UnmanagedType.LPArray)] PLINT[] r,
                                           [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] g,
                                           [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] b,
                                           PLINT ncol1);

        public static void scmap1(PLINT[] r, PLINT[] g, PLINT[] b)
        {
            _scmap1(r, g, b, GetSize(r, g, b));
        }                       

        // Set color map 1 colors by 8 bit RGB and alpha values
        [DllImport(DllName, EntryPoint = "c_plscmap1a")]
        private static extern void _scmap1a([In, MarshalAs(UnmanagedType.LPArray)] PLINT[] r,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] g,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] b,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] alpha,
                                            PLINT ncol0);

        public static void scmap1a(PLINT[] r, PLINT[] g, PLINT[] b, PLFLT[] alpha)
        {
            _scmap1a(r, g, b, alpha, GetSize(r, g, b, alpha));
        }

        // Set color map 1 colors using a piece-wise linear relationship between
        // intensity [0,1] (cmap 1 index) and position in HLS or RGB color space.
        [DllImport(DllName, EntryPoint = "c_plscmap1l")]
        private static extern void _scmap1l(ColorSpace itype, PLINT npts,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] intensity,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord1,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord2,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord3,
                                            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Bool)] PLBOOL[] alt_hue_path);

        public static void scmap1l(ColorSpace itype, PLFLT[] intensity, PLFLT[] coord1, PLFLT[] coord2, PLFLT[] coord3,
                                   PLBOOL[] alt_hue_path)
        {
            PLINT npts = GetSize(intensity, coord1, coord2, coord3);
            if (alt_hue_path.Length != npts-1)
                throw new ArgumentException("alt_hue_path must have one element less than intensity");
            _scmap1l(itype, npts, intensity, coord1, coord2, coord3, alt_hue_path);
        }                                   

        // Set color map 1 colors using a piece-wise linear relationship between
        // intensity [0,1] (cmap 1 index) and position in HLS or RGB color space.
        // Will also linear interpolate alpha values.
        [DllImport(DllName, EntryPoint = "c_plscmap1la")]
        private static extern void _scmap1la(ColorSpace itype, PLINT npts,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] intensity,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord1,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord2,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord3,
                                             [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] alpha,
                                             [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Bool)] PLBOOL[] alt_hue_path);

        public static void scmap1la(ColorSpace itype, PLFLT[] intensity, PLFLT[] coord1, PLFLT[] coord2, PLFLT[] coord3, PLFLT[] alpha,
                                    PLBOOL[] alt_hue_path)
        {
            PLINT npts = GetSize(intensity, coord1, coord2, coord3, alpha);
            if (alt_hue_path.Length != npts-1)
                throw new ArgumentException("alt_hue_path must have one element less than intensity");
            _scmap1la(itype, npts, intensity, coord1, coord2, coord3, alpha, alt_hue_path);
        }                                   

        // Set number of colors in cmap 1
        [DllImport(DllName, EntryPoint = "c_plscmap1n")]
        public static extern void scmap1n(PLINT ncol1);

        // Set the color map 1 range used in continuous plots
        [DllImport(DllName, EntryPoint = "c_plscmap1_range")]
        public static extern void scmap1_range(PLFLT min_color, PLFLT max_color);

        // Get the color map 1 range used in continuous plots
        [DllImport(DllName, EntryPoint = "c_plgcmap1_range")]
        public static extern void gcmap1_range(out PLFLT min_color, out PLFLT max_color);

        // Set a given color from color map 0 by 8 bit RGB value
        [DllImport(DllName, EntryPoint = "c_plscol0")]
        public static extern void scol0(PLINT icol0, PLINT r, PLINT g, PLINT b);

        // Set a given color from color map 0 by 8 bit RGB value
        [DllImport(DllName, EntryPoint = "c_plscol0a")]
        public static extern void scol0a(PLINT icol0, PLINT r, PLINT g, PLINT b, PLFLT alpha);

        // Set the background color by 8 bit RGB value
        [DllImport(DllName, EntryPoint = "c_plscolbg")]
        public static extern void scolbg(PLINT r, PLINT g, PLINT b);

        // Set the background color by 8 bit RGB value and alpha value
        [DllImport(DllName, EntryPoint = "c_plscolbga")]
        public static extern void scolbga(PLINT r, PLINT g, PLINT b, PLFLT alpha);

        // Used to globally turn color output on/off
        [DllImport(DllName, EntryPoint = "c_plscolor")]
        public static extern void scolor(PLINT color);

        // Set the compression level
        [DllImport(DllName, EntryPoint = "c_plscompression")]
        public static extern void scompression(PLINT compression);

        // Set the device (keyword) name
        [DllImport(DllName, EntryPoint = "c_plsdev")]
        public static extern void sdev([MarshalAs(UnmanagedType.LPStr)] string devname);

        // Set window into device space using margin, aspect ratio, and justification
        [DllImport(DllName, EntryPoint = "c_plsdidev")]
        public static extern void sdidev(PLFLT mar, PLFLT aspect, PLFLT jx, PLFLT jy);

        // Set up transformation from metafile coordinates.
        [DllImport(DllName, EntryPoint = "c_plsdimap")]
        public static extern void sdimap(PLINT dimxmin, PLINT dimxmax, PLINT dimymin, PLINT dimymax,
                                         PLFLT dimxpmm, PLFLT dimypmm);

        // Set plot orientation, specifying rotation in units of pi/2.
        [DllImport(DllName, EntryPoint = "c_plsdiori")]
        public static extern void sdiori(PLFLT rot);

        // Set window into plot space
        [DllImport(DllName, EntryPoint = "c_plsdiplt")]
        public static extern void sdiplt(PLFLT xmin, PLFLT ymin, PLFLT xmax, PLFLT ymax);

        // Set window into plot space incrementally (zoom)
        [DllImport(DllName, EntryPoint = "c_plsdiplz")]
        public static extern void sdiplz(PLFLT xmin, PLFLT ymin, PLFLT xmax, PLFLT ymax);

        // Set seed for internal random number generator
        [DllImport(DllName, EntryPoint = "c_plseed")]
        public static extern void seed(uint seed);

        // Set the escape character for text strings.
        [DllImport(DllName, EntryPoint = "c_plsesc")]
        public static extern void sesc(char esc);

        // Set family file parameters
        [DllImport(DllName, EntryPoint = "c_plsfam")]
        public static extern void sfam(PLINT fam, PLINT num, PLINT bmax);

        // Set FCI (font characterization integer)
        [DllImport(DllName, EntryPoint = "c_plsfci")]
        public static extern void sfci(FCI fci);

        // Set the output file name.
        [DllImport(DllName, EntryPoint = "c_plsfnam")]
        public static extern void sfnam([MarshalAs(UnmanagedType.LPStr)] string fnam);

        // Set the current font family, style and weight
        [DllImport(DllName, EntryPoint = "c_plsfont")]
        public static extern void sfont(FontFamily family, FontStyle style, FontWeight weight);

        // Shade region.
        [DllImport(DllName, EntryPoint = "c_plshade")]
        private static extern void _shade(
                PLFLT_MATRIX a, PLINT nx, PLINT ny, DefinedFunc defined,
                PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax,
                PLFLT shade_min, PLFLT shade_max,
                PLINT sh_cmap, PLFLT sh_color, PLFLT sh_width,
                PLINT min_color, PLFLT min_width,
                PLINT max_color, PLFLT max_width,
                FillFunc fill, [MarshalAs(UnmanagedType.Bool)] PLBOOL rectangular,
                TransformFunc pltr, PLPointer pltr_data);

        public static void shade(
                PLFLT[,] a, DefinedFunc defined,
                PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax,
                PLFLT shade_min, PLFLT shade_max,
                PLINT sh_cmap, PLFLT sh_color, PLFLT sh_width,
                PLINT min_color, PLFLT min_width,
                PLINT max_color, PLFLT max_width,
                FillFunc fill, PLBOOL rectangular,
                TransformFunc pltr, PLPointer pltr_data)
        {
            using (var mat_a = new MatrixMarshaller(a))
            {
                _shade(mat_a.Ptr, mat_a.NX, mat_a.NY, defined, xmin, xmax, ymin, ymax,
                    shade_min, shade_max, sh_cmap, sh_color, sh_width, min_color, min_width,
                    max_color, max_width, fill, rectangular, pltr, pltr_data);
            }
        }                

        [DllImport(DllName, EntryPoint = "c_plshades")]
        private static extern void _shades(
                PLFLT_MATRIX a, PLINT nx, PLINT ny, DefinedFunc defined,
                PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax,
                [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel, PLFLT fill_width,
                PLINT cont_color, PLFLT cont_width,
                FillFunc fill, [MarshalAs(UnmanagedType.Bool)] PLBOOL rectangular,
                TransformFunc pltr, PLPointer pltr_data);

        public static void shades(
                PLFLT[,] a, DefinedFunc defined,
                PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax,
                PLFLT[] clevel, PLFLT fill_width,
                PLINT cont_color, PLFLT cont_width,
                FillFunc fill, PLBOOL rectangular,
                TransformFunc pltr, PLPointer pltr_data)
        {
            using (var mat_a = new MatrixMarshaller(a))
            {
                _shades(mat_a.Ptr, mat_a.NX, mat_a.NY, defined, xmin, xmax, ymin, ymax,
                    clevel, clevel.Length, fill_width, cont_color, cont_width, fill, rectangular,
                    pltr, pltr_data);
            }
        }                

        // Setup a user-provided custom labeling function
        [DllImport(DllName, EntryPoint = "c_plslabelfunc")]
        public static extern void slabelfunc(LabelFunc label_func, PLPointer label_data);

        // Set up lengths of major tick marks.
        [DllImport(DllName, EntryPoint = "c_plsmaj")]
        public static extern void smaj(PLFLT def, PLFLT scale);

        // Set the RGB memory area to be plotted (with the 'mem' or 'memcairo' drivers)
        [DllImport(DllName, EntryPoint = "c_plsmem")]
        public static extern void smem(PLINT maxx, PLINT maxy, PLPointer plotmem);

        // Set the RGBA memory area to be plotted (with the 'memcairo' driver)
        [DllImport(DllName, EntryPoint = "c_plsmema")]
        public static extern void smema(PLINT maxx, PLINT maxy, PLPointer plotmem);

        // Set up lengths of minor tick marks.
        [DllImport(DllName, EntryPoint = "c_plsmin")]
        public static extern void smin(PLFLT def, PLFLT scale);

        // Set the drawing mode
        [DllImport(DllName, EntryPoint = "c_plsdrawmode")]
        public static extern void sdrawmode(DrawMode mode);

        // Set orientation.  Must be done before calling plinit.
        [DllImport(DllName, EntryPoint = "c_plsori")]
        public static extern void sori(Orientation ori);

        // Set output device parameters.  Usually ignored by the driver.
        [DllImport(DllName, EntryPoint = "c_plspage")]
        public static extern void spage(PLFLT xp, PLFLT yp, PLINT xleng, PLINT yleng,
                                        PLINT xoff, PLINT yoff);

        // Set the colors for color table 0 from a cmap0 file
        [DllImport(DllName, EntryPoint = "c_plspal0")]
        public static extern void spal0([MarshalAs(UnmanagedType.LPStr)] string filename);

        // Set the colors for color table 1 from a cmap1 file
        [DllImport(DllName, EntryPoint = "c_plspal1")]
        public static extern void spal1([MarshalAs(UnmanagedType.LPStr)] string filename, [MarshalAs(UnmanagedType.Bool)] PLBOOL interpolate);

        // Set the pause (on end-of-page) status
        [DllImport(DllName, EntryPoint = "c_plspause")]
        public static extern void spause([MarshalAs(UnmanagedType.Bool)] PLBOOL pause);

        // Set stream number.
        [DllImport(DllName, EntryPoint = "c_plsstrm")]
        public static extern void sstrm(PLINT strm);

        // Set the number of subwindows in x and y
        [DllImport(DllName, EntryPoint = "c_plssub")]
        public static extern void ssub(PLINT nx, PLINT ny);

        // Set symbol height.
        [DllImport(DllName, EntryPoint = "c_plssym")]
        public static extern void ssym(PLFLT def, PLFLT scale);

        // Initialize PLplot, passing in the windows/page settings.
        [DllImport(DllName, EntryPoint = "c_plstar")]
        public static extern void star(PLINT nx, PLINT ny);

        // Initialize PLplot, passing the device name and windows/page settings.
        [DllImport(DllName, EntryPoint = "c_plstart")]
        public static extern void start([MarshalAs(UnmanagedType.LPStr)] string devname, PLINT nx, PLINT ny);

        // Set the coordinate transform
        [DllImport(DllName, EntryPoint = "c_plstransform")]
        public static extern void stransform(TransformFunc coordinate_transform, PLPointer coordinate_transform_data);

        // Prints out the same string repeatedly at the n points in world
        // coordinates given by the x and y arrays.  Supersedes plpoin and
        // plsymbol for the case where text refers to a unicode glyph either
        // directly as UTF-8 or indirectly via the standard text escape
        // sequences allowed for PLplot input strings.
        [DllImport(DllName, EntryPoint = "c_plstring")]
        private static extern void _string2(PLINT n,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                            [MarshalAs(UnmanagedType.LPStr)] string str);

        public static void string2(PLFLT[] x, PLFLT[] y, string str)
        {
            _string2(GetSize(x, y), x, y, str);
        }

        // Prints out the same string repeatedly at the n points in world
        // coordinates given by the x, y, and z arrays.  Supersedes plpoin3
        // for the case where text refers to a unicode glyph either directly
        // as UTF-8 or indirectly via the standard text escape sequences
        // allowed for PLplot input strings.
        [DllImport(DllName, EntryPoint = "c_plstring3")]
        private static extern void _string3(PLINT n,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z,
                                            [MarshalAs(UnmanagedType.LPStr)] string str);

        public static void string3(PLFLT[] x, PLFLT[] y, PLFLT[] z, string str)
        {
            _string3(GetSize(x, y, z), x, y, z, str);
        }

        // Add a point to a stripchart.
        [DllImport(DllName, EntryPoint = "c_plstripa")]
        public static extern void stripa(PLINT id, Pen pen, PLFLT x, PLFLT y);

        /// <summary>plstripc: Create a 4-pen strip chart</summary>
        /// <param name="acc">Accumulate strip plot if acc is true, otherwise slide display.</param>
        /// <param name="colbox">Plot box color index (cmap0).</param>
        /// <param name="collab">Legend color index (cmap0).</param>
        /// <param name="colline">A vector containing the cmap0 color indices for the 4 pens.</param>
        /// <param name="id">Returned value of the identification number of the strip chart to use on plstripa and plstripd.</param>
        /// <param name="labtop">A UTF-8 character string containing the plot title.</param>
        /// <param name="labx">A UTF-8 character string containing the label for the x axis.</param>
        /// <param name="laby">A UTF-8 character string containing the label for the y axis.</param>
        /// <param name="legline">A vector of UTF-8 character strings containing legends for the 4 pens.</param>
        /// <param name="styline">A vector containing the line style indices for the 4 pens.</param>
        /// <param name="xjump">When x attains xmax, the length of the plot is multiplied by the factor  (1 + xjump) .</param>
        /// <param name="xlpos">X legend box position (range from 0 to 1).</param>
        /// <param name="xmax">Initial coordinates of plot box; they will change as data are added.</param>
        /// <param name="xmin">Initial coordinates of plot box; they will change as data are added.</param>
        /// <param name="xspec">One or more string concatenated from <see cref="XYAxisOpt"/>.</param>
        /// <param name="y_ascl">Autoscale y between x jumps if y_ascl is true, otherwise not.</param>
        /// <param name="ylpos">Y legend box position (range from 0 to 1).</param>
        /// <param name="ymax">Initial coordinates of plot box; they will change as data are added.</param>
        /// <param name="ymin">Initial coordinates of plot box; they will change as data are added.</param>
        /// <param name="yspec">One or more string concatenated from <see cref="XYAxisOpt"/>.</param>
        /// <remarks>Create a 4-pen strip chart, to be used afterwards by plstripa</remarks>
        [DllImport(DllName, EntryPoint = "c_plstripc")]
        private static extern void _stripc(
                    out PLINT id,
                    [MarshalAs(UnmanagedType.LPStr)] string xspec,
                    [MarshalAs(UnmanagedType.LPStr)] string yspec,
                    PLFLT xmin, PLFLT xmax, PLFLT xjump, PLFLT ymin, PLFLT ymax,
                    PLFLT xlpos, PLFLT ylpos,
                    [MarshalAs(UnmanagedType.Bool)] PLBOOL y_ascl,
                    [MarshalAs(UnmanagedType.Bool)] PLBOOL acc,
                    PLINT colbox, PLINT collab,
                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] colline,
                    [In, MarshalAs(UnmanagedType.LPArray)] LineStyle[] styline,
                    [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] PLUTF8_STRING[] legline,
                    [MarshalAs(UnmanagedType.LPStr)] string labx,
                    [MarshalAs(UnmanagedType.LPStr)] string laby,
                    [MarshalAs(UnmanagedType.LPStr)] string labtop);

        public static void stripc(
                    out PLINT id, string xspec, string yspec,
                    PLFLT xmin, PLFLT xmax, PLFLT xjump, PLFLT ymin, PLFLT ymax,
                    PLFLT xlpos, PLFLT ylpos, PLBOOL y_ascl,
                    PLBOOL acc, PLINT colbox, PLINT collab, PLINT[] colline,
                    LineStyle[] styline, PLUTF8_STRING[] legline, string labx,
                    string laby, string labtop)
        {
            if (colline.Length != 4 || styline.Length != 4 || legline.Length != 4)
                throw new ArgumentException("colline, styline and legline must be of length 4");
            _stripc(out id, xspec, yspec, xmin, xmax, xjump, ymin, ymax, xlpos, ylpos, 
                    y_ascl, acc, colbox, collab, colline, styline, legline, labx, laby, labtop);                
        }                                    

        // Deletes and releases memory used by a stripchart.
        [DllImport(DllName, EntryPoint = "c_plstripd")]
        public static extern void stripd(PLINT id);

        // plots a 2d image (or a matrix too large for plshade() )
        [DllImport(DllName, EntryPoint = "c_plimagefr")]
        private static extern void _imagefr(PLFLT_MATRIX idata, PLINT nx, PLINT ny,
                                            PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax, PLFLT zmin, PLFLT zmax,
                                            PLFLT valuemin, PLFLT valuemax,
                                            TransformFunc pltr, PLPointer pltr_data);

        public static void imagefr(PLFLT[,] idata, 
                                   PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax, PLFLT zmin, PLFLT zmax,
                                   PLFLT valuemin, PLFLT valuemax,
                                   TransformFunc pltr, PLPointer pltr_data)
        {
            using (var mat_idata = new MatrixMarshaller(idata))
            {
                _imagefr(mat_idata.Ptr, mat_idata.NX, mat_idata.NY, 
                         xmin, xmax, ymin, ymax, zmin, zmax, valuemin, valuemax, 
                         pltr, pltr_data);
            }
        }                                   

        // plots a 2d image (or a matrix too large for plshade() ) - colors
        // automatically scaled
        [DllImport(DllName, EntryPoint = "c_plimage")]
        private static extern void _image(PLFLT_MATRIX idata, PLINT nx, PLINT ny,
                                          PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax, PLFLT zmin, PLFLT zmax,
                                          PLFLT Dxmin, PLFLT Dxmax, PLFLT Dymin, PLFLT Dymax);

        public static void image(PLFLT[,] idata, 
                                 PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax, PLFLT zmin, PLFLT zmax,
                                 PLFLT Dxmin, PLFLT Dxmax, PLFLT Dymin, PLFLT Dymax)
        {
            using (var mat_idata = new MatrixMarshaller(idata))
            {
                _image(mat_idata.Ptr, mat_idata.NX, mat_idata.NY, 
                       xmin, xmax, ymin, ymax, zmin, zmax, Dxmin, Dxmax, Dymin, Dymax);
            }            
        }                                 

        // Set up a new line style
        [DllImport(DllName, EntryPoint = "c_plstyl")]
        private static extern void _styl(PLINT nms,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] mark,
                                         [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] space);

        public static void styl(PLINT[] mark, PLINT[] space)
        {
            _styl(GetSize(mark, space), mark, space);
        }        

        // Plots the 3d surface representation of the function z[x][y].
        [DllImport(DllName, EntryPoint = "c_plsurf3d")]
        private static extern void _surf3d([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                           [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                           PLFLT_MATRIX z,
                                           PLINT nx, PLINT ny, Surf opt,
                                           [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel);

        public static void surf3d(PLFLT[] x, PLFLT[] y, PLFLT[,] z, Surf opt, PLFLT[] clevel)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                _surf3d(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt, clevel, clevel.Length);
            }                
        }

        // Plots the 3d surface representation of the function z[x][y] with y index limits.
        [DllImport(DllName, EntryPoint = "c_plsurf3dl")]
        private static extern void _surf3dl([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                            PLFLT_MATRIX z,
                                            PLINT nx, PLINT ny, Surf opt,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel,
                                            PLINT indexxmin, PLINT indexxmax,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] indexymin,
                                            [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] indexymax);

        public static void surf3dl(PLFLT[] x, PLFLT[] y, PLFLT[,] z, Surf opt, PLFLT[] clevel,
                                   PLINT indexxmin, PLINT[] indexymin, PLINT[] indexymax)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                PLINT indexxmax = indexxmin + GetSize(indexymax, indexymax);
                _surf3dl(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt, clevel, clevel.Length,
                         indexxmin, indexxmax, indexymin, indexymax);
            }                
        }

        // Set arrow style for vector plots.
        [DllImport(DllName, EntryPoint = "c_plsvect")]
        private static extern void _svect([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] arrowx,
                                          [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] arrowy,
                                          PLINT npts, [MarshalAs(UnmanagedType.Bool)] PLBOOL fill);

        public static void svect(PLFLT[] arrowx, PLFLT[] arrowy, PLBOOL fill)
        {
            _svect(arrowx, arrowy, GetSize(arrowx, arrowy), fill);
        }

        // Sets the edges of the viewport to the specified absolute coordinates
        [DllImport(DllName, EntryPoint = "c_plsvpa")]
        public static extern void svpa(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax);

        // Set x axis labeling parameters
        [DllImport(DllName, EntryPoint = "c_plsxax")]
        public static extern void sxax(PLINT digmax, PLINT digits);

        // Set inferior X window
        [DllImport(DllName, EntryPoint = "plsxwin")]
        public static extern void sxwin(PLINT window_id);

        // Set y axis labeling parameters
        [DllImport(DllName, EntryPoint = "c_plsyax")]
        public static extern void syax(PLINT digmax, PLINT digits);

        // Plots array y against x for n points using Hershey symbol "code"
        [DllImport(DllName, EntryPoint = "c_plsym")]
        private static extern void _sym(PLINT n,
                                        [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                        [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                        PLINT code);

        public static void sym(PLFLT[] x, PLFLT[] y, PLINT code)
        {
            _sym(GetSize(x, y), x, y, code);
        }

        // Set z axis labeling parameters
        [DllImport(DllName, EntryPoint = "c_plszax")]
        public static extern void szax(PLINT digmax, PLINT digits);

        // Switches to text screen.
        [DllImport(DllName, EntryPoint = "c_pltext")]
        public static extern void text();

        // Set the format for date / time labels for current stream.
        [DllImport(DllName, EntryPoint = "c_pltimefmt")]
        public static extern void timefmt([MarshalAs(UnmanagedType.LPStr)] string fmt);

        // Sets the edges of the viewport with the given aspect ratio, leaving
        // room for labels.
        [DllImport(DllName, EntryPoint = "c_plvasp")]
        public static extern void vasp(PLFLT aspect);

        // simple arrow plotter.
        [DllImport(DllName, EntryPoint = "c_plvect")]
        private static extern void _vect(PLFLT_MATRIX u, PLFLT_MATRIX v, PLINT nx, PLINT ny, PLFLT scale,
                                         TransformFunc pltr, PLPointer pltr_data);

        public static void vect(PLFLT[,] u, PLFLT[,] v, PLFLT scale,
                                TransformFunc pltr, PLPointer pltr_data)
        {
            using (var u_mat=new MatrixMarshaller(u))
            {
                using (var v_mat=new MatrixMarshaller(v))
                {
                    CheckSize(u, v);
                    _vect(u_mat.Ptr, v_mat.Ptr, u_mat.NX, u_mat.NY, scale,
                          pltr, pltr_data);
                }
            }
        }                                

        // Creates the largest viewport of the specified aspect ratio that fits
        // within the specified normalized subpage coordinates.
        [DllImport(DllName, EntryPoint = "c_plvpas")]
        public static extern void vpas(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax, PLFLT aspect);

        // Creates a viewport with the specified normalized subpage coordinates.
        [DllImport(DllName, EntryPoint = "c_plvpor")]
        public static extern void vpor(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax);

        // Defines a "standard" viewport with seven character heights for
        // the left margin and four character heights everywhere else.
        [DllImport(DllName, EntryPoint = "c_plvsta")]
        public static extern void vsta();

        // Set up a window for three-dimensional plotting.
        [DllImport(DllName, EntryPoint = "c_plw3d")]
        public static extern void w3d(PLFLT basex, PLFLT basey, PLFLT height, PLFLT xmin,
                                      PLFLT xmax, PLFLT ymin, PLFLT ymax, PLFLT zmin,
                                      PLFLT zmax, PLFLT alt, PLFLT az);

        // Set pen width.
        [DllImport(DllName, EntryPoint = "c_plwidth")]
        public static extern void width(PLFLT width);

        // Set up world coordinates of the viewport boundaries (2d plots).
        [DllImport(DllName, EntryPoint = "c_plwind")]
        public static extern void wind(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax);

        // Set xor mode; mode = 1-enter, 0-leave, status = 0 if not interactive device
        [DllImport(DllName, EntryPoint = "c_plxormod")]
        public static extern void xormod([MarshalAs(UnmanagedType.Bool)] PLBOOL mode, 
                                         [MarshalAs(UnmanagedType.Bool)] out PLBOOL status);


        /*
        //--------------------------------------------------------------------------
        //		Functions for use from C or C++ only
        //--------------------------------------------------------------------------
        */

        // Returns a list of file-oriented device names and their menu strings 
        [DllImport(DllName, EntryPoint="plgFileDevs")]
        private static extern void _gFileDevs(ref IntPtr p_menustr, ref IntPtr p_devname, out int p_ndev);

        /// <summary>Returns a list of file-oriented device names and their menu strings</summary>
        /// <param name="p_devname">device name</param>
        /// <param name="p_menustr">human-readable name</param>
        public static void gFileDevs(out string[] p_menustr, out string[] p_devname)
        {
            GetDevHelper(_gFileDevs, out p_menustr, out p_devname);
        }

        // Returns a list of all device names and their menu strings
        [DllImport(DllName, EntryPoint="plgDevs")]
        private static extern void _gDevs(ref IntPtr p_menustr, ref IntPtr p_devname, out int p_ndev);        

        /// <summary>Returns a list of all device names and their menu strings</summary>
        /// <param name="p_devname">device name</param>
        /// <param name="p_menustr">human-readable name</param>
        public static void gDevs(out string[] p_menustr, out string[] p_devname)
        {
            GetDevHelper(_gDevs, out p_menustr, out p_devname);
        }

        // Set the function pointer for the keyboard event handler
        [DllImport(DllName, EntryPoint = "plsKeyEH")]
        public static extern void sKeyEH(KeyHandler keyEH, PLPointer KeyEH_data);

        // Set the function pointer for the (mouse) button event handler
        [DllImport(DllName, EntryPoint = "plsButtonEH")]
        public static extern void sButtonEH(ButtonHandler buttonEH, PLPointer ButtonEH_data);

        // Sets an optional user bop handler
        [DllImport(DllName, EntryPoint = "plsbopH")]
        public static extern void sbopH(BopHandler handler, PLPointer handler_data);

        // Sets an optional user eop handler
        [DllImport(DllName, EntryPoint = "plseopH")]
        public static extern void seopH(EopHandler handler, PLPointer handler_data);

        /*
        // Set the variables to be used for storing error info
        [DllImport(DllName, EntryPoint = "plsError")]
        public static extern void sError(out PLINT errcode, [MarshalAs(UnmanagedType.LPStr)] out string errmsg);
        */

        // Sets an optional user exit handler.
        [DllImport(DllName, EntryPoint = "plsexit")]
        public static extern void sexit(ExitHandler handler);

        // Sets an optional user abort handler.
        [DllImport(DllName, EntryPoint = "plsabort")]
        public static extern void sabort(AbortHandler handler);

        /* Transformation routines */

        // Identity transformation.
        [DllImport(DllName, EntryPoint = "pltr0")]
        public static extern void tr0(PLFLT x, PLFLT y, out PLFLT tx, out PLFLT ty, PLPointer pltr_data);

        // Does linear interpolation from singly dimensioned coord arrays.
        [DllImport(DllName, EntryPoint = "pltr1")]
        public static extern void tr1(PLFLT x, PLFLT y, out PLFLT tx, out PLFLT ty, PLPointer pltr_data);

        // Does linear interpolation from doubly dimensioned coord arrays
        // (column dominant, as per normal C 2d arrays).
        [DllImport(DllName, EntryPoint = "pltr2")]
        public static extern void tr2(PLFLT x, PLFLT y, out PLFLT tx, out PLFLT ty, PLPointer pltr_data);

        // Just like pltr2() but uses pointer arithmetic to get coordinates from
        // 2d grid tables.
        [DllImport(DllName, EntryPoint = "pltr2p")]
        public static extern void tr2p(PLFLT x, PLFLT y, out PLFLT tx, out PLFLT ty, PLPointer pltr_data);

        // Does linear interpolation from doubly dimensioned coord arrays
        // (row dominant, i.e. Fortran ordering).
        [DllImport(DllName, EntryPoint = "pltr2f")]
        public static extern void tr2f(PLFLT x, PLFLT y, out PLFLT tx, out PLFLT ty, PLPointer pltr_data);

        /* Command line parsing utilities */

        // Clear internal option table info structure.
        [DllImport(DllName, EntryPoint = "plClearOpts")]
        public static extern void ClearOpts();

        // Reset internal option table info structure.
        [DllImport(DllName, EntryPoint = "plResetOpts")]
        public static extern void ResetOpts();

        /*
        // Merge user option table into internal info structure.
        [DllImport(DllName, EntryPoint="plMergeOpts")]
        public static extern PLINT MergeOpts( PLOptionTable *options, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string *notes );
        */

        // Set the strings used in usage and syntax messages.
        [DllImport(DllName, EntryPoint = "plSetUsage")]
        public static extern void SetUsage([MarshalAs(UnmanagedType.LPStr)] string program_string,
                                           [MarshalAs(UnmanagedType.LPStr)] string usage_string);

        // Process input strings, treating them as an option and argument pair.
        // The first is for the external API, the second the work routine declared
        // here for backward compatibilty.
        [DllImport(DllName, EntryPoint = "c_plsetopt")]
        public static extern PLINT setopt([MarshalAs(UnmanagedType.LPStr)] string opt,
                                          [MarshalAs(UnmanagedType.LPStr)] string optarg);

        // Process options list using current options info.
        [DllImport(DllName, EntryPoint = "c_plparseopts")]
        public static extern PLINT parseopts(ref int p_argc,
                                             [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] argv,
                                             Parse mode);

        // Print usage & syntax message.
        [DllImport(DllName, EntryPoint = "plOptUsage")]
        public static extern void OptUsage();

        /* Miscellaneous */

        // Get the output file pointer
        [DllImport(DllName, EntryPoint = "plgfile")]
        public static extern void gfile(out FILE p_file);

        // Set the output file pointer
        [DllImport(DllName, EntryPoint = "plsfile")]
        public static extern void sfile(FILE file);

        // Get the escape character for text strings.
        [DllImport(DllName, EntryPoint = "plgesc")]
        public static extern void gesc(out PLCHAR p_esc);

        // Front-end to driver escape function.
        [DllImport(DllName, EntryPoint = "pl_cmd")]
        public static extern void cmd(PLINT op, PLPointer ptr);

        /*
        // Return full pathname for given file if executable
        [DllImport(DllName, EntryPoint = "plFindName")]
        public static extern PLINT FindName([MarshalAs(UnmanagedType.LPStr)] out string p);

        // Looks for the specified executable file according to usual search path.
        [DllImport(DllName, EntryPoint = "plFindCommand")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string FindCommand([MarshalAs(UnmanagedType.LPStr)] string fn);

        // Gets search name for file by concatenating the dir, subdir, and file
        // name, allocating memory as needed.
        [DllImport(DllName, EntryPoint = "plGetName")]
        public static extern void GetName([MarshalAs(UnmanagedType.LPStr)] string dir,
                                           [MarshalAs(UnmanagedType.LPStr)] string subdir,
                                           [MarshalAs(UnmanagedType.LPStr)] string filename,
                                           [MarshalAs(UnmanagedType.LPStr)] StringBuilder filespec);

        // Prompts human to input an integer in response to given message.
        [DllImport(DllName, EntryPoint = "plGetInt")]
        public static extern PLINT GetInt([MarshalAs(UnmanagedType.LPStr)] string s);

        // Prompts human to input a float in response to given message.
        [DllImport(DllName, EntryPoint = "plGetFlt")]
        public static extern PLFLT GetFlt([MarshalAs(UnmanagedType.LPStr)] string s);
        */

        /* Nice way to allocate space for a vectored 2d grid */

        // Allocates a block of memory for use as a 2-d grid of PLFLT's.
        [DllImport(DllName, EntryPoint = "plAlloc2dGrid")]
        public static extern void Alloc2dGrid(out PLFLT_NC_MATRIX f, PLINT nx, PLINT ny);

        // Frees a block of memory allocated with plAlloc2dGrid().
        [DllImport(DllName, EntryPoint = "plFree2dGrid")]
        public static extern void Free2dGrid(PLFLT_NC_MATRIX f, PLINT nx, PLINT ny);

        // Find the maximum and minimum of a 2d matrix allocated with plAllc2dGrid().
        [DllImport(DllName, EntryPoint = "plMinMax2dGrid")]
        public static extern void MinMax2dGrid(PLFLT_MATRIX f, PLINT nx, PLINT ny, out PLFLT fmax, out PLFLT fmin);

        // Wait for graphics input event and translate to world coordinates
        [DllImport(DllName, EntryPoint = "plGetCursor")]
        public static extern PLINT GetCursor(out GraphicsIn gin);

        // Translates relative device coordinates to world coordinates.
        [DllImport(DllName, EntryPoint = "plTranslateCursor")]
        public static extern PLINT TranslateCursor(out GraphicsIn gin);

        // Set the pointer to the data used in driver initialisation
        [DllImport(DllName, EntryPoint = "plsdevdata")]
        public static extern void sdevdata(PLPointer data);

    }
}
