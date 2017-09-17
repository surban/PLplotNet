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

        /// <summary>pl_setcontlabelformat: Set format of numerical label for contours</summary>
        /// <param name="lexp">If the contour numerical label is greater than 10^(lexp) or less than 10^(-lexp), then the exponential format is used. Default value of lexp is 4.</param>
        /// <param name="sigdig">Number of significant digits. Default value is 2.</param>
        /// <remarks>Set format of numerical label for contours.</remarks>
        [DllImport(DllName, EntryPoint = "c_pl_setcontlabelformat")]
        public static extern void setcontlabelformat(PLINT lexp, PLINT sigdig);

        /// <summary>pl_setcontlabelparam: Set parameters of contour labelling other than format of numerical label</summary>
        /// <param name="active">Activate labels. Set to 1 if you want contour labels on. Default is off (0).</param>
        /// <param name="offset">Offset of label from contour line (if set to 0.0, labels are printed on the lines). Default value is 0.006.</param>
        /// <param name="size">Font height for contour labels (normalized). Default value is 0.3.</param>
        /// <param name="spacing">Spacing parameter for contour labels. Default value is 0.1.</param>
        /// <remarks>Set parameters of contour labelling other than those handled by pl_setcontlabelformat.</remarks>
        [DllImport(DllName, EntryPoint = "c_pl_setcontlabelparam")]
        public static extern void setcontlabelparam(PLFLT offset, PLFLT size, PLFLT spacing, PLINT active);

        /// <summary>pladv: Advance the (sub-)page</summary>
        /// <param name="page">Specifies the subpage number (starting from 1 in the top left corner and increasing along the rows) to which to advance. Set to zero to advance to the next subpage (or to the next page if subpages are not being used).</param>
        /// <remarks>Advances to the next subpage if sub=0, performing a page advance if there are no remaining subpages on the current page. If subpages aren't being used, pladv(0) will always advance the page. If pagegt0, PLplot switches to the specified subpage. Note that this allows you to overwrite a plot on the specified subpage; if this is not what you intended, use pleop followed by plbop to first advance the page. This routine is called automatically (with page=0) by plenv, but if plenv is not used, pladv must be called after initializing PLplot but before defining the viewport.</remarks>
        [DllImport(DllName, EntryPoint = "c_pladv")]
        public static extern void adv(PLINT page);

        /// <summary>plarc: Draw a circular or elliptical arc</summary>
        /// <param name="a">Length of the semimajor axis of the arc.</param>
        /// <param name="angle1">Starting angle of the arc relative to the semimajor axis.</param>
        /// <param name="angle2">Ending angle of the arc relative to the semimajor axis.</param>
        /// <param name="b">Length of the semiminor axis of the arc.</param>
        /// <param name="fill">Draw a filled arc.</param>
        /// <param name="rotate">Angle of the semimajor axis relative to the X-axis.</param>
        /// <param name="x">X coordinate of arc center.</param>
        /// <param name="y">Y coordinate of arc center.</param>
        /// <remarks>Draw a possibly filled arc centered at x, y with semimajor axis a and semiminor axis b, starting at angle1 and ending at angle2.</remarks>
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

        /// <summary>Plot a histogram using x to store data values and y to store frequencies</summary>
        [DllImport(DllName, EntryPoint = "c_plbin")]
        private static extern void _bin(PLINT nbin,
                                                [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y, Bin opt);

        /// <summary>plbin: Plot a histogram from binned data</summary>
        /// <param name="opt">Is a combination of several flags: opt=PL_BIN_DEFAULT: The x represent the lower bin boundaries, the outer bins are expanded to fill up the entire x-axis and bins of zero height are simply drawn. opt=PL_BIN_CENTRED|...: The bin boundaries are to be midway between the x values. If the values in x are equally spaced, the values are the center values of the bins. opt=PL_BIN_NOEXPAND|...: The outer bins are drawn with equal size as the ones inside. opt=PL_BIN_NOEMPTY|...: Bins with zero height are not drawn (there is a gap for such bins).</param>
        /// <param name="x">A vector containing values associated with bins. These must form a strictly increasing sequence.</param>
        /// <param name="y">A vector containing a number which is proportional to the number of points in each bin. This is a PLFLT (instead of PLINT) vector so as to allow histograms of probabilities, etc.</param>
        /// <remarks>Plots a histogram consisting of nbin bins. The value associated with the i'th bin is placed in x[i], and the number of points in the bin is placed in y[i]. For proper operation, the values in x[i] must form a strictly increasing sequence. By default, x[i] is the left-hand edge of the i'th bin. If opt=PL_BIN_CENTRED is used, the bin boundaries are placed midway between the values in the x vector. Also see plhist for drawing histograms from unbinned data.</remarks>
        public static void bin(PLFLT[] x, PLFLT[] y, Bin opt)
        {
            _bin(GetSize(x, y), x, y, opt);
        }

        /// <summary>plbtime: Calculate broken-down time from continuous time for the current stream</summary>
        /// <param name="ctime">Continuous time from which the broken-down time is calculated.</param>
        /// <param name="day">Returned value of day within the month in the range from 1 to 31.</param>
        /// <param name="hour">Returned value of hour within the day in the range from 0 to 23.</param>
        /// <param name="min">Returned value of minute within the hour in the range from 0 to 59</param>
        /// <param name="month">Returned value of month within the year in the range from 0 (January) to 11 (December).</param>
        /// <param name="sec">Returned value of second within the minute in range from 0. to 60.</param>
        /// <param name="year">Returned value of years with positive values corresponding to CE (i.e., 1 = 1 CE, etc.) and non-negative values corresponding to BCE (e.g., 0 = 1 BCE, -1 = 2 BCE, etc.)</param>
        /// <remarks>Calculate broken-down time; year, month, day, hour, min, sec; from continuous time, ctime for the current stream. This function is the inverse of plctime.</remarks>
        [DllImport(DllName, EntryPoint = "c_plbtime")]
        public static extern void btime(out PLINT year, out PLINT month, out PLINT day, out PLINT hour,
                                                out PLINT min, out PLFLT sec, PLFLT ctime);

        /// <summary>plbop: Begin a new page</summary>
        /// <remarks>Begins a new page. For a file driver, the output file is opened if necessary. Advancing the page via pleop and plbop is useful when a page break is desired at a particular point when plotting to subpages. Another use for pleop and plbop is when plotting pages to different files, since you can manually set the file name by calling plsfnam after the call to pleop. (In fact some drivers may only support a single page per file, making this a necessity.) One way to handle this case automatically is to page advance via pladv, but enable familying (see plsfam) with a small limit on the file size so that a new family member file will be created on each page break.</remarks>
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

        /// <summary>plcalc_world: Calculate world coordinates and corresponding window index from relative device coordinates</summary>
        /// <param name="rx">Input relative device coordinate (0.0-1.0) for the x coordinate.</param>
        /// <param name="ry">Input relative device coordinate (0.0-1.0) for the y coordinate.</param>
        /// <param name="window">Returned value of the last defined window index that corresponds to the input relative device coordinates (and the returned world coordinates). To give some background on the window index, for each page the initial window index is set to zero, and each time plwind is called within the page, world and device coordinates are stored for the window and the window index is incremented. Thus, for a simple page layout with non-overlapping viewports and one window per viewport, window corresponds to the viewport index (in the order which the viewport/windows were created) of the only viewport/window corresponding to rx and ry. However, for more complicated layouts with potentially overlapping viewports and possibly more than one window (set of world coordinates) per viewport, window and the corresponding output world coordinates corresponds to the last window created that fulfills the criterion that the relative device coordinates are inside it. Finally, in all cases where the input relative device coordinates are not inside any viewport/window, then the returned value of the last defined window index is set to -1.</param>
        /// <param name="wx">Returned value of the x world coordinate corresponding to the relative device coordinates rx and ry.</param>
        /// <param name="wy">Returned value of the y world coordinate corresponding to the relative device coordinates rx and ry.</param>
        /// <remarks>Calculate world coordinates, wx and wy, and corresponding window index from relative device coordinates, rx and ry.</remarks>
        [DllImport(DllName, EntryPoint = "c_plcalc_world")]
        public static extern void calc_world(PLFLT rx, PLFLT ry, out PLFLT wx, out PLFLT wy, out PLINT window);

        /// <summary>plclear: Clear current (sub)page</summary>
        /// <remarks>Clears the current page, effectively erasing everything that have been drawn. This command only works with interactive drivers; if the driver does not support this, the page is filled with the background color in use. If the current page is divided into subpages, only the current subpage is erased. The nth subpage can be selected with pladv(n).</remarks>
        [DllImport(DllName, EntryPoint = "c_plclear")]
        public static extern void clear();

        /// <summary>plcol0: Set color, cmap0</summary>
        /// <param name="icol0">Integer representing the color. The defaults at present are (these may change):  0 black (default background) 1 red (default foreground) 2 yellow 3 green 4 aquamarine 5 pink 6 wheat 7 grey 8 brown 9 blue10 BlueViolet11 cyan12 turquoise13 magenta14 salmon15 white Use plscmap0 to change the entire cmap0 color palette and plscol0 to change an individual color in the cmap0 color palette.</param>
        /// <remarks>Sets the color index for cmap0 (see ).</remarks>
        [DllImport(DllName, EntryPoint = "c_plcol0")]
        public static extern void col0(PLINT icol0);

        /// <summary>plcol1: Set color, cmap1</summary>
        /// <param name="col1">This value must be in the range (0.0-1.0) and is mapped to color using the continuous cmap1 palette which by default ranges from blue to the background color to red. The cmap1 palette can also be straightforwardly changed by the user with plscmap1 or plscmap1l.</param>
        /// <remarks>Sets the color for cmap1 (see ).</remarks>
        [DllImport(DllName, EntryPoint = "c_plcol1")]
        public static extern void col1(PLFLT col1);

        /// <summary>plconfigtime: Configure the transformation between continuous and broken-down time for the current stream</summary>
        /// <param name="ccontrol">ccontrol contains bits controlling the transformation. If the 0x1 bit is set, then the proleptic Julian calendar is used for broken-down time rather than the proleptic Gregorian calendar. If the 0x2 bit is set, then leap seconds that have been historically used to define UTC are inserted into the broken-down time. Other possibilities for additional control bits for ccontrol exist such as making the historical time corrections in the broken-down time corresponding to ET (ephemeris time) or making the (slightly non-constant) corrections from international atomic time (TAI) to what astronomers define as terrestrial time (TT). But those additional possibilities have not been implemented yet in the qsastime library (one of the PLplot utility libraries).</param>
        /// <param name="day">Day of epoch in range from 1 to 31.</param>
        /// <param name="hour">Hour of epoch in range from 0 to 23</param>
        /// <param name="ifbtime_offset">ifbtime_offset controls how the epoch of the continuous time scale is specified by the user. If ifbtime_offset is false, then offset1 and offset2 are used to specify the epoch, and the following broken-down time parameters are completely ignored. If ifbtime_offset is true, then offset1 and offset2 are completely ignored, and the following broken-down time parameters are used to specify the epoch.</param>
        /// <param name="min">Minute of epoch in range from 0 to 59.</param>
        /// <param name="month">Month of epoch in range from 0 (January) to 11 (December).</param>
        /// <param name="offset1">If ifbtime_offset is true, the parameters offset1 and offset2 are completely ignored. Otherwise, the sum of these parameters (with units in days) specify the epoch of the continuous time relative to the MJD epoch corresponding to the Gregorian calendar date of 1858-11-17T00:00:00Z or JD = 2400000.5. Two PLFLT numbers are used to specify the origin to allow users (by specifying offset1 as an integer that can be exactly represented by a floating-point variable and specifying offset2 as a number in the range from 0. to 1) the chance to minimize the numerical errors of the continuous time representation.</param>
        /// <param name="offset2">See documentation of offset1.</param>
        /// <param name="scale">The number of days per continuous time unit. As a special case, if scale is 0., then all other arguments are ignored, and the result (the default used by PLplot) is the equivalent of a call to plconfigtime(1./86400., 0., 0., 0x0, 1, 1970, 0, 1, 0, 0, 0.). That is, for this special case broken-down time is calculated with the proleptic Gregorian calendar with no leap seconds inserted, and the continuous time is defined as the number of seconds since the Unix epoch of 1970-01-01T00:00:00Z.</param>
        /// <param name="sec">Second of epoch in range from 0. to 60.</param>
        /// <param name="year">Year of epoch.</param>
        /// <remarks>Configure the transformation between continuous and broken-down time for the current stream. This transformation is used by both plbtime and plctime.</remarks>
        [DllImport(DllName, EntryPoint = "c_plconfigtime")]
        public static extern void configtime(PLFLT scale, PLFLT offset1, PLFLT offset2, PLINT ccontrol,
                                                     [MarshalAs(UnmanagedType.Bool)] PLBOOL ifbtime_offset,
                                                     PLINT year, PLINT month, PLINT day, PLINT hour, PLINT min, PLFLT sec);

        /// <summary>Draws a contour plot from data in f(nx,ny).  Is just a front-end to plfcont, with a particular choice for f2eval and f2eval_data.</summary>
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
                CheckRange("kx", 0, mat_f.NX, kx); CheckRange("lx", kx + 1, mat_f.NX, lx);
                CheckRange("ky", 0, mat_f.NY, ky); CheckRange("ly", ky + 1, mat_f.NY, ly);
                _cont(mat_f.Ptr, mat_f.NX, mat_f.NY, kx + 1, lx + 1, ky + 1, ly + 1, clevel, clevel.Length, pltr, pltr_data);
            }
        }

        /// <summary>plcpstrm: Copy state parameters from the reference stream to the current stream</summary>
        /// <param name="flags">If flags is set to true the device coordinates are not copied from the reference to current stream.</param>
        /// <param name="iplsr">Number of reference stream.</param>
        /// <remarks>Copies state parameters from the reference stream to the current stream. Tell driver interface to map device coordinates unless flags == 1.</remarks>
        [DllImport(DllName, EntryPoint = "c_plcpstrm")]
        public static extern void cpstrm(PLINT iplsr, [MarshalAs(UnmanagedType.Bool)] PLBOOL flags);

        /// <summary>plctime: Calculate continuous time from broken-down time for the current stream</summary>
        /// <param name="ctime">Returned value of the continuous time calculated from the broken-down time specified by the previous parameters.</param>
        /// <param name="day">Input day in range from 1 to 31.</param>
        /// <param name="hour">Input hour in range from 0 to 23</param>
        /// <param name="min">Input minute in range from 0 to 59.</param>
        /// <param name="month">Input month in range from 0 (January) to 11 (December).</param>
        /// <param name="sec">Input second in range from 0. to 60.</param>
        /// <param name="year">Input year.</param>
        /// <remarks>Calculate continuous time, ctime, from broken-down time for the current stream. The broken-down time is specified by the following parameters: year, month, day, hour, min, and sec. This function is the inverse of plbtime.</remarks>
        [DllImport(DllName, EntryPoint = "c_plctime")]
        public static extern void ctime(PLINT year, PLINT month, PLINT day, PLINT hour, PLINT min, PLFLT sec, out PLFLT ctime);

        /// <summary>Converts input values from relative device coordinates to relative plot coordinates.</summary>
        [DllImport(DllName, EntryPoint = "pldid2pc")]
        public static extern void did2pc(out PLFLT xmin, out PLFLT ymin, out PLFLT xmax, out PLFLT ymax);

        /// <summary>Converts input values from relative plot coordinates to relative device coordinates.</summary>
        [DllImport(DllName, EntryPoint = "pldip2dc")]
        public static extern void dip2dc(out PLFLT xmin, out PLFLT ymin, out PLFLT xmax, out PLFLT ymax);

        /// <summary>plend: End plotting session</summary>
        /// <remarks>Ends a plotting session, tidies up all the output files, switches interactive devices back into text mode and frees up any memory that was allocated. Must be called before end of program.</remarks>
        [DllImport(DllName, EntryPoint = "c_plend")]
        public static extern void end();

        /// <summary>plend1: End plotting session for current stream</summary>
        /// <remarks>Ends a plotting session for the current output stream only. See plsstrm for more info.</remarks>
        [DllImport(DllName, EntryPoint = "c_plend1")]
        public static extern void end1();

        /// <summary>plenv: Set up standard window and draw box</summary>
        /// <param name="axis">Controls drawing of the box around the plot: -2: draw no box, no tick marks, no numeric tick labels, no axes. -1: draw box only. 0: draw box, ticks, and numeric tick labels. 1: also draw coordinate axes at x=0 and y=0. 2: also draw a grid at major tick positions in both coordinates. 3: also draw a grid at minor tick positions in both coordinates. 10: same as 0 except logarithmic x tick marks. (The x data have to be converted to logarithms separately.) 11: same as 1 except logarithmic x tick marks. (The x data have to be converted to logarithms separately.) 12: same as 2 except logarithmic x tick marks. (The x data have to be converted to logarithms separately.) 13: same as 3 except logarithmic x tick marks. (The x data have to be converted to logarithms separately.) 20: same as 0 except logarithmic y tick marks. (The y data have to be converted to logarithms separately.) 21: same as 1 except logarithmic y tick marks. (The y data have to be converted to logarithms separately.) 22: same as 2 except logarithmic y tick marks. (The y data have to be converted to logarithms separately.) 23: same as 3 except logarithmic y tick marks. (The y data have to be converted to logarithms separately.) 30: same as 0 except logarithmic x and y tick marks. (The x and y data have to be converted to logarithms separately.) 31: same as 1 except logarithmic x and y tick marks. (The x and y data have to be converted to logarithms separately.) 32: same as 2 except logarithmic x and y tick marks. (The x and y data have to be converted to logarithms separately.) 33: same as 3 except logarithmic x and y tick marks. (The x and y data have to be converted to logarithms separately.) 40: same as 0 except date / time x labels. 41: same as 1 except date / time x labels. 42: same as 2 except date / time x labels. 43: same as 3 except date / time x labels. 50: same as 0 except date / time y labels. 51: same as 1 except date / time y labels. 52: same as 2 except date / time y labels. 53: same as 3 except date / time y labels. 60: same as 0 except date / time x and y labels. 61: same as 1 except date / time x and y labels. 62: same as 2 except date / time x and y labels. 63: same as 3 except date / time x and y labels. 70: same as 0 except custom x and y labels. 71: same as 1 except custom x and y labels. 72: same as 2 except custom x and y labels. 73: same as 3 except custom x and y labels.</param>
        /// <param name="just">Controls how the axes will be scaled: -1: the scales will not be set, the user must set up the scale before calling plenv using plsvpa, plvasp or other. 0: the x and y axes are scaled independently to use as much of the screen as possible. 1: the scales of the x and y axes are made equal. 2: the axis of the x and y axes are made equal, and the plot box will be square.</param>
        /// <param name="xmax">Value of x at right-hand edge of window (in world coordinates).</param>
        /// <param name="xmin">Value of x at left-hand edge of window (in world coordinates).</param>
        /// <param name="ymax">Value of y at top edge of window (in world coordinates).</param>
        /// <param name="ymin">Value of y at bottom edge of window (in world coordinates).</param>
        /// <remarks>Sets up plotter environment for simple graphs by calling pladv and setting up viewport and window to sensible default values. plenv leaves a standard margin (left-hand margin of eight character heights, and a margin around the other three sides of five character heights) around most graphs for axis labels and a title. When these defaults are not suitable, use the individual routines plvpas, plvpor, or plvasp for setting up the viewport, plwind for defining the window, and plbox for drawing the box.</remarks>
        [DllImport(DllName, EntryPoint = "c_plenv")]
        public static extern void env(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax,
                                              AxesScale just, AxisBox axis);

        /// <summary>plenv0: Same as plenv but if in multiplot mode does not advance the subpage, instead clears it</summary>
        /// <param name="axis">Controls drawing of the box around the plot: -2: draw no box, no tick marks, no numeric tick labels, no axes. -1: draw box only. 0: draw box, ticks, and numeric tick labels. 1: also draw coordinate axes at x=0 and y=0. 2: also draw a grid at major tick positions in both coordinates. 3: also draw a grid at minor tick positions in both coordinates. 10: same as 0 except logarithmic x tick marks. (The x data have to be converted to logarithms separately.) 11: same as 1 except logarithmic x tick marks. (The x data have to be converted to logarithms separately.) 12: same as 2 except logarithmic x tick marks. (The x data have to be converted to logarithms separately.) 13: same as 3 except logarithmic x tick marks. (The x data have to be converted to logarithms separately.) 20: same as 0 except logarithmic y tick marks. (The y data have to be converted to logarithms separately.) 21: same as 1 except logarithmic y tick marks. (The y data have to be converted to logarithms separately.) 22: same as 2 except logarithmic y tick marks. (The y data have to be converted to logarithms separately.) 23: same as 3 except logarithmic y tick marks. (The y data have to be converted to logarithms separately.) 30: same as 0 except logarithmic x and y tick marks. (The x and y data have to be converted to logarithms separately.) 31: same as 1 except logarithmic x and y tick marks. (The x and y data have to be converted to logarithms separately.) 32: same as 2 except logarithmic x and y tick marks. (The x and y data have to be converted to logarithms separately.) 33: same as 3 except logarithmic x and y tick marks. (The x and y data have to be converted to logarithms separately.) 40: same as 0 except date / time x labels. 41: same as 1 except date / time x labels. 42: same as 2 except date / time x labels. 43: same as 3 except date / time x labels. 50: same as 0 except date / time y labels. 51: same as 1 except date / time y labels. 52: same as 2 except date / time y labels. 53: same as 3 except date / time y labels. 60: same as 0 except date / time x and y labels. 61: same as 1 except date / time x and y labels. 62: same as 2 except date / time x and y labels. 63: same as 3 except date / time x and y labels. 70: same as 0 except custom x and y labels. 71: same as 1 except custom x and y labels. 72: same as 2 except custom x and y labels. 73: same as 3 except custom x and y labels.</param>
        /// <param name="just">Controls how the axes will be scaled: -1: the scales will not be set, the user must set up the scale before calling plenv0 using plsvpa, plvasp or other. 0: the x and y axes are scaled independently to use as much of the screen as possible. 1: the scales of the x and y axes are made equal. 2: the axis of the x and y axes are made equal, and the plot box will be square.</param>
        /// <param name="xmax">Value of x at right-hand edge of window (in world coordinates).</param>
        /// <param name="xmin">Value of x at left-hand edge of window (in world coordinates).</param>
        /// <param name="ymax">Value of y at top edge of window (in world coordinates).</param>
        /// <param name="ymin">Value of y at bottom edge of window (in world coordinates).</param>
        /// <remarks>Sets up plotter environment for simple graphs by calling pladv and setting up viewport and window to sensible default values. plenv0 leaves a standard margin (left-hand margin of eight character heights, and a margin around the other three sides of five character heights) around most graphs for axis labels and a title. When these defaults are not suitable, use the individual routines plvpas, plvpor, or plvasp for setting up the viewport, plwind for defining the window, and plbox for drawing the box.</remarks>
        [DllImport(DllName, EntryPoint = "c_plenv0")]
        public static extern void env0(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax,
                                               AxesScale just, AxisBox axis);

        /// <summary>pleop: Eject current page</summary>
        /// <remarks>Clears the graphics screen of an interactive device, or ejects a page on a plotter. See plbop for more information.</remarks>
        [DllImport(DllName, EntryPoint = "c_pleop")]
        public static extern void eop();

        /// <summary>Plot horizontal error bars (xmin(i),y(i)) to (xmax(i),y(i))</summary>
        [DllImport(DllName, EntryPoint = "c_plerrx")]
        private static extern void _errx(PLINT n,
                                                [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] xmin,
                                                [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] xmax,
                                                [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y);

        /// <summary>plerrx: Draw error bars in x direction</summary>
        /// <param name="xmax">A vector containing the x coordinates of the right-hand endpoints of the error bars.</param>
        /// <param name="xmin">A vector containing the x coordinates of the left-hand endpoints of the error bars.</param>
        /// <param name="y">A vector containing the y coordinates of the error bars.</param>
        /// <remarks>Draws a set of n error bars in x direction, the i'th error bar extending from xmin[i] to xmax[i] at y coordinate y[i]. The terminals of the error bars are of length equal to the minor tick length (settable using plsmin).</remarks>
        public static void errx(PLFLT[] xmin, PLFLT[] xmax, PLFLT[] y)
        {
            _errx(GetSize(xmin, xmax, y), xmin, xmax, y);
        }

        /// <summary>Plot vertical error bars (x,ymin(i)) to (x(i),ymax(i))</summary>
        [DllImport(DllName, EntryPoint = "c_plerry")]
        private static extern void _erry(PLINT n,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] ymin,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] ymax);

        /// <summary>plerry: Draw error bars in the y direction</summary>
        /// <param name="x">A vector containing the x coordinates of the error bars.</param>
        /// <param name="ymax">A vector containing the y coordinates of the upper endpoints of the error bars.</param>
        /// <param name="ymin">A vector containing the y coordinates of the lower endpoints of the error bars.</param>
        /// <remarks>Draws a set of n error bars in the y direction, the i'th error bar extending from ymin[i] to ymax[i] at x coordinate x[i]. The terminals of the error bars are of length equal to the minor tick length (settable using plsmin).</remarks>
        public static void erry(PLFLT[] x, PLFLT[] ymin, PLFLT[] ymax)
        {
            _erry(GetSize(x, ymin, ymax), x, ymin, ymax);
        }

        /// <summary>plfamadv: Advance to the next family file on the next new page</summary>
        /// <remarks>Advance to the next family file on the next new page.</remarks>
        [DllImport(DllName, EntryPoint = "c_plfamadv")]
        public static extern void famadv();

        /// <summary>Pattern fills the polygon bounded by the input points.</summary>
        [DllImport(DllName, EntryPoint = "c_plfill")]
        private static extern void _fill(PLINT n,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y);

        /// <summary>plfill: Draw filled polygon</summary>
        /// <param name="x">A vector containing the x coordinates of vertices.</param>
        /// <param name="y">A vector containing the y coordinates of vertices.</param>
        /// <remarks>Fills the polygon defined by the n points (x[i], y[i]) using the pattern defined by plpsty or plpat. The default fill style is a solid fill. The routine will automatically close the polygon between the last and first vertices. If multiple closed polygons are passed in x and y then plfill will fill in between them.</remarks>
        public static void fill(PLFLT[] x, PLFLT[] y)
        {
            _fill(GetSize(x, y), x, y);
        }

        /// <summary>Pattern fills the 3d polygon bounded by the input points.</summary>
        [DllImport(DllName, EntryPoint = "c_plfill3")]
        private static extern void _fill3(PLINT n,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z);

        /// <summary>plfill3: Draw filled polygon in 3D</summary>
        /// <param name="x">A vector containing the x coordinates of vertices.</param>
        /// <param name="y">A vector containing the y coordinates of vertices.</param>
        /// <param name="z">A vector containing the z coordinates of vertices.</param>
        /// <remarks>Fills the 3D polygon defined by the n points in the x, y, and z vectors using the pattern defined by plpsty or plpat. The routine will automatically close the polygon between the last and first vertices. If multiple closed polygons are passed in x, y, and z then plfill3 will fill in between them.</remarks>
        public static void fill3(PLFLT[] x, PLFLT[] y, PLFLT[] z)
        {
            _fill3(GetSize(x, y, z), x, y, z);
        }

        /// <summary>plflush: Flushes the output stream</summary>
        /// <remarks>Flushes the output stream. Use sparingly, if at all.</remarks>
        [DllImport(DllName, EntryPoint = "c_plflush")]
        public static extern void flush();

        /// <summary>plfont: Set font</summary>
        /// <param name="ifont">Specifies the font: 1: Sans serif font (simplest and fastest) 2: Serif font 3: Italic font 4: Script font</param>
        /// <remarks>Sets the font used for subsequent text and symbols. For devices that still use Hershey fonts this routine has no effect unless the Hershey fonts with extended character set are loaded (see plfontld). For unicode-aware devices that use system fonts instead of Hershey fonts, this routine calls the plsfci routine with argument set up appropriately for the various cases below. However, this method of specifying the font for unicode-aware devices is deprecated, and the much more flexible method of calling plsfont directly is recommended instead (where plsfont provides a user-friendly interface to plsfci),</remarks>
        [DllImport(DllName, EntryPoint = "c_plfont")]
        public static extern void font(FontFlag ifont);

        /// <summary>plfontld: Load Hershey fonts</summary>
        /// <param name="fnt">Specifies the type of Hershey fonts to load. A zero value specifies Hershey fonts with the standard character set and a non-zero value (the default assumed if plfontld is never called) specifies Hershey fonts with the extended character set.</param>
        /// <remarks>Loads the Hershey fonts used for text and symbols. This routine may be called before or after initializing PLplot. If not explicitly called before PLplot initialization, then by default that initialization loads Hershey fonts with the extended character set. This routine only has a practical effect for devices that still use Hershey fonts (as opposed to modern devices that use unicode-aware system fonts instead of Hershey fonts).</remarks>
        [DllImport(DllName, EntryPoint = "c_plfontld")]
        public static extern void fontld(PLINT fnt);

        /// <summary>plgchr: Get character default height and current (scaled) height</summary>
        /// <param name="p_def">Returned value of the default character height (mm).</param>
        /// <param name="p_ht">Returned value of the scaled character height (mm).</param>
        /// <remarks>Get character default height and current (scaled) height.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgchr")]
        public static extern void gchr(out PLFLT p_def, out PLFLT p_ht);

        /// <summary>plgcol0: Returns 8-bit RGB values for given color index from cmap0</summary>
        /// <param name="b">Returned value of the 8-bit blue value.</param>
        /// <param name="g">Returned value of the 8-bit green value.</param>
        /// <param name="icol0">Index of desired cmap0 color.</param>
        /// <param name="r">Returned value of the 8-bit red value.</param>
        /// <remarks>Returns 8-bit RGB values (0-255) for given color from cmap0 (see ). Values are negative if an invalid color id is given.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgcol0")]
        public static extern void gcol0(PLINT icol0, out PLINT r, out PLINT g, out PLINT b);

        /// <summary>plgcol0a: Returns 8-bit RGB values and PLFLT alpha transparency value for given color index from cmap0</summary>
        /// <param name="alpha">Returned value of the alpha transparency in the range from (0.0-1.0).</param>
        /// <param name="b">Returned value of the blue intensity in the range from 0 to 255.</param>
        /// <param name="g">Returned value of the green intensity in the range from 0 to 255.</param>
        /// <param name="icol0">Index of desired cmap0 color.</param>
        /// <param name="r">Returned value of the red intensity in the range from 0 to 255.</param>
        /// <remarks>Returns 8-bit RGB values (0-255) and PLFLT alpha transparency value (0.0-1.0) for given color from cmap0 (see ). Values are negative if an invalid color id is given.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgcol0a")]
        public static extern void gcol0a(PLINT icol0, out PLINT r, out PLINT g, out PLINT b, out PLFLT alpha);

        /// <summary>plgcolbg: Returns the background color (cmap0[0]) by 8-bit RGB value</summary>
        /// <param name="b">Returned value of the blue intensity in the range from 0 to 255.</param>
        /// <param name="g">Returned value of the green intensity in the range from 0 to 255.</param>
        /// <param name="r">Returned value of the red intensity in the range from 0 to 255.</param>
        /// <remarks>Returns the background color (cmap0[0]) by 8-bit RGB value.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgcolbg")]
        public static extern void gcolbg(out PLINT r, out PLINT g, out PLINT b);

        /// <summary>plgcolbga: Returns the background color (cmap0[0]) by 8-bit RGB value and PLFLT alpha transparency value</summary>
        /// <param name="alpha">Returned value of the alpha transparency in the range (0.0-1.0).</param>
        /// <param name="b">Returned value of the blue intensity in the range from 0 to 255.</param>
        /// <param name="g">Returned value of the green intensity in the range from 0 to 255.</param>
        /// <param name="r">Returned value of the red intensity in the range from 0 to 255.</param>
        /// <remarks>Returns the background color (cmap0[0]) by 8-bit RGB value and PLFLT alpha transparency value.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgcolbga")]
        public static extern void gcolbga(out PLINT r, out PLINT g, out PLINT b, out PLFLT alpha);

        /// <summary>plgcompression: Get the current device-compression setting</summary>
        /// <param name="compression">Returned value of the compression setting for the current device.</param>
        /// <remarks>Get the current device-compression setting. This parameter is only used for drivers that provide compression.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgcompression")]
        public static extern void gcompression(out PLINT compression);

        /// <summary>Get the current device (keyword) name (max. 80 chars)</summary>
        [DllImport(DllName, EntryPoint = "c_plgdev")]
        private static extern void _gdev([MarshalAs(UnmanagedType.LPStr)] StringBuilder p_dev);

        /// <summary>plgdev: Get the current device (keyword) name</summary>
        /// <param name="p_dev">Returned ascii character string (with preallocated length of 80 characters or more) containing the device (keyword) name.</param>
        /// <remarks>Get the current device (keyword) name. Note: you must have allocated space for this (80 characters is safe).</remarks>
        public static void gdev(out string p_dev)
        {
            var sb_p_dev = new StringBuilder(MaxLength);
            _gdev(sb_p_dev);
            p_dev = sb_p_dev.ToString();
        }

        /// <summary>plgdidev: Get parameters that define current device-space window</summary>
        /// <param name="p_aspect">Returned value of the aspect ratio.</param>
        /// <param name="p_jx">Returned value of the relative justification in x.</param>
        /// <param name="p_jy">Returned value of the relative justification in y.</param>
        /// <param name="p_mar">Returned value of the relative margin width.</param>
        /// <remarks>Get relative margin width, aspect ratio, and relative justification that define current device-space window. If plsdidev has not been called the default values pointed to by p_mar, p_aspect, p_jx, and p_jy will all be 0.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgdidev")]
        public static extern void gdidev(out PLFLT p_mar, out PLFLT p_aspect, out PLFLT p_jx, out PLFLT p_jy);

        /// <summary>plgdiori: Get plot orientation</summary>
        /// <param name="p_rot">Returned value of the orientation parameter.</param>
        /// <remarks>Get plot orientation parameter which is multiplied by 90deg to obtain the angle of rotation. Note, arbitrary rotation parameters such as 0.2 (corresponding to 18deg) are possible, but the usual values for the rotation parameter are 0., 1., 2., and 3. corresponding to 0deg (landscape mode), 90deg (portrait mode), 180deg (seascape mode), and 270deg (upside-down mode). If plsdiori has not been called the default value pointed to by p_rot will be 0.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgdiori")]
        public static extern void gdiori(out PLFLT p_rot);

        /// <summary>plgdiplt: Get parameters that define current plot-space window</summary>
        /// <param name="p_xmax">Returned value of the relative maximum in x.</param>
        /// <param name="p_xmin">Returned value of the relative minimum in x.</param>
        /// <param name="p_ymax">Returned value of the relative maximum in y.</param>
        /// <param name="p_ymin">Returned value of the relative minimum in y.</param>
        /// <remarks>Get relative minima and maxima that define current plot-space window. If plsdiplt has not been called the default values pointed to by p_xmin, p_ymin, p_xmax, and p_ymax will be 0., 0., 1., and 1.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgdiplt")]
        public static extern void gdiplt(out PLFLT p_xmin, out PLFLT p_ymin, out PLFLT p_xmax, out PLFLT p_ymax);

        /// <summary>plgdrawmode: Get drawing mode (depends on device support!)</summary>
        /// <remarks>Get drawing mode. Note only one device driver (cairo) currently supports this at the moment, and for that case the PLINT value returned by this function is one of PL_DRAWMODE_DEFAULT, PL_DRAWMODE_REPLACE, PL_DRAWMODE_XOR, or PL_DRAWMODE_UNKNOWN. This function returns PL_DRAWMODE_UNKNOWN for the rest of the device drivers. See also plsdrawmode.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgdrawmode")]
        public static extern DrawMode gdrawmode();

        /// <summary>plgfci: Get FCI (font characterization integer)</summary>
        /// <param name="p_fci">Returned value of the current FCI value.</param>
        /// <remarks>Gets information about the current font using the FCI approach. See  for more information.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgfci")]
        public static extern void gfci(out FCI p_fci);

        /// <summary>plgfam: Get family file parameters</summary>
        /// <param name="p_bmax">Returned value of the maximum file size (in bytes) for a family file.</param>
        /// <param name="p_fam">Returned value of the current family flag value. If nonzero, familying is enabled for the current device.</param>
        /// <param name="p_num">Returned value of the current family file number.</param>
        /// <remarks>Gets information about current family file, if familying is enabled. See  for more information.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgfam")]
        public static extern void gfam(out PLINT p_fam, out PLINT p_num, out PLINT p_bmax);

        /// <summary>Get the (current) output file name.  Must be preallocated to &gt;80 bytes</summary>
        [DllImport(DllName, EntryPoint = "c_plgfnam")]
        private static extern void _gfnam([MarshalAs(UnmanagedType.LPStr)] StringBuilder fnam);

        /// <summary>plgfnam: Get output file name</summary>
        /// <param name="fnam">Returned ascii character string (with preallocated length of 80 characters or more) containing the file name.</param>
        /// <remarks>Gets the current output file name, if applicable.</remarks>
        public static void gfnam(out string fnam)
        {
            var sb_fnam = new StringBuilder(MaxLength);
            _gfnam(sb_fnam);
            fnam = sb_fnam.ToString();
        }

        /// <summary>plgfont: Get family, style and weight of the current font</summary>
        /// <param name="p_family">Returned value of the current font family. The available values are given by the PL_FCI_* constants in plplot.h. Current options are PL_FCI_SANS, PL_FCI_SERIF, PL_FCI_MONO, PL_FCI_SCRIPT and PL_FCI_SYMBOL. If p_family is NULL then the font family is not returned.</param>
        /// <param name="p_style">Returned value of the current font style. The available values are given by the PL_FCI_* constants in plplot.h. Current options are PL_FCI_UPRIGHT, PL_FCI_ITALIC and PL_FCI_OBLIQUE. If p_style is NULL then the font style is not returned.</param>
        /// <param name="p_weight">Returned value of the current font weight. The available values are given by the PL_FCI_* constants in plplot.h. Current options are PL_FCI_MEDIUM and PL_FCI_BOLD. If p_weight is NULL then the font weight is not returned.</param>
        /// <remarks>Gets information about current font. See  for more information on font selection.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgfont")]
        public static extern void gfont(out FontFamily p_family, out FontStyle p_style, out FontWeight p_weight);

        /// <summary>plglevel: Get the (current) run level</summary>
        /// <param name="p_level">Returned value of the run level.</param>
        /// <remarks>Get the (current) run level. Valid settings are:  0, uninitialized  1, initialized  2, viewport defined  3, world coordinates defined</remarks>
        [DllImport(DllName, EntryPoint = "c_plglevel")]
        public static extern void glevel(out RunLevel p_level);

        /// <summary>plgpage: Get page parameters</summary>
        /// <param name="p_xleng">Returned value of the x page length.</param>
        /// <param name="p_xoff">Returned value of the x page offset.</param>
        /// <param name="p_xp">Returned value of the number of pixels/inch (DPI) in x.</param>
        /// <param name="p_yleng">Returned value of the y page length.</param>
        /// <param name="p_yoff">Returned value of the y page offset.</param>
        /// <param name="p_yp">Returned value of the number of pixels/inch (DPI) in y.</param>
        /// <remarks>Gets the current page configuration. The length and offset values are expressed in units that are specific to the current driver. For instance: screen drivers will usually interpret them as number of pixels, whereas printer drivers will usually use mm.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgpage")]
        public static extern void gpage(out PLFLT p_xp, out PLFLT p_yp,
                                                out PLINT p_xleng, out PLINT p_yleng, out PLINT p_xoff, out PLINT p_yoff);

        /// <summary>plgra: Switch to graphics screen</summary>
        /// <remarks>Sets an interactive device to graphics mode, used in conjunction with pltext to allow graphics and text to be interspersed. On a device which supports separate text and graphics windows, this command causes control to be switched to the graphics window. If already in graphics mode, this command is ignored. It is also ignored on devices which only support a single window or use a different method for shifting focus. See also pltext.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgra")]
        public static extern void gra();

        /// <summary>Draw gradient in polygon.</summary>
        [DllImport(DllName, EntryPoint = "c_plgradient")]
        private static extern void _gradient(PLINT n,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y, PLFLT angle);

        /// <summary>plgradient: Draw linear gradient inside polygon</summary>
        /// <param name="angle">Angle (degrees) of gradient vector from x axis.</param>
        /// <param name="x">A vector containing the x coordinates of vertices.</param>
        /// <param name="y">A vector containing the y coordinates of vertices.</param>
        /// <remarks>Draw a linear gradient using cmap1 inside the polygon defined by the n points (x[i], y[i]). Interpretation of the polygon is the same as for plfill. The polygon coordinates and the gradient angle are all expressed in world coordinates. The angle from the x axis for both the rotated coordinate system and the gradient vector is specified by angle. The magnitude of the gradient vector is the difference between the maximum and minimum values of x for the vertices in the rotated coordinate system. The origin of the gradient vector can be interpreted as being anywhere on the line corresponding to the minimum x value for the vertices in the rotated coordinate system. The distance along the gradient vector is linearly transformed to the independent variable of color map 1 which ranges from 0. at the tail of the gradient vector to 1. at the head of the gradient vector. What is drawn is the RGBA color corresponding to the independent variable of cmap1. For more information about cmap1 (see ).</remarks>
        public static void gradient(PLFLT[] x, PLFLT[] y, PLFLT angle)
        {
            _gradient(GetSize(x, y), x, y, angle);
        }

        /// <summary>grid irregularly sampled data</summary>
        [DllImport(DllName, EntryPoint = "c_plgriddata")]
        private static extern void _griddata(
                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z, PLINT npts,
                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] xg, PLINT nptsx,
                            [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] yg, PLINT nptsy,
                            PLFLT_NC_MATRIX zg, Grid type, PLFLT data);

        /// <summary>plgriddata: Grid data from irregularly sampled data</summary>
        /// <param name="data">Some gridding algorithms require extra data, which can be specified through this argument. Currently, for algorithm: GRID_NNIDW, data specifies the number of neighbors to use, the lower the value, the noisier (more local) the approximation is. GRID_NNLI, data specifies what a thin triangle is, in the range [1. .. 2.]. High values enable the usage of very thin triangles for interpolation, possibly resulting in error in the approximation. GRID_NNI, only weights greater than data will be accepted. If 0, all weights will be accepted.</param>
        /// <param name="type">The type of grid interpolation algorithm to use, which can be: GRID_CSA: Bivariate Cubic Spline approximation GRID_DTLI: Delaunay Triangulation Linear Interpolation GRID_NNI: Natural Neighbors Interpolation GRID_NNIDW: Nearest Neighbors Inverse Distance Weighted GRID_NNLI: Nearest Neighbors Linear Interpolation GRID_NNAIDW: Nearest Neighbors Around Inverse Distance Weighted  For details of the algorithms read the source file plgridd.c.</param>
        /// <param name="x">The input x vector.</param>
        /// <param name="xg">A vector that specifies the grid spacing in the x direction. Usually xg has nptsx equally spaced values from the minimum to the maximum values of the x input vector.</param>
        /// <param name="y">The input y vector.</param>
        /// <param name="yg">A vector that specifies the grid spacing in the y direction. Similar to the xg parameter.</param>
        /// <param name="z">The input z vector. Each triple x[i], y[i], z[i] represents one data sample coordinate.</param>
        /// <param name="zg">The matrix of interpolated results where data lies in the grid specified by xg and yg. Therefore the zg matrix must be dimensioned nptsx by nptsy.</param>
        /// <remarks>Real world data is frequently irregularly sampled, but PLplot 3D plots require data organized as a grid, i.e., with x sample point values independent of y coordinate and vice versa. This function takes irregularly sampled data from the x[npts], y[npts], and z[npts] vectors; reads the desired grid location from the input vectors xg[nptsx] and yg[nptsy]; and returns the interpolated result on that grid using the output matrix zg[nptsx][nptsy]. The algorithm used to interpolate the data to the grid is specified with the argument type which can have one parameter specified in argument data.</remarks>
        public static void griddata(PLFLT[] x, PLFLT[] y, PLFLT[] z, PLFLT[] xg, PLFLT[] yg, PLFLT[,] zg, Grid type, PLFLT data)
        {
            using (var mat_zg = new MatrixMarshaller(zg))
            {
                if (mat_zg.NX != xg.Length || mat_zg.NY != yg.Length)
                    throw new ArgumentException("zg must have size of xg times yg");
                _griddata(x, y, z, GetSize(x, y, z), xg, xg.Length, yg, yg.Length, mat_zg.Ptr, type, data);
            }
        }

        /// <summary>plgspa: Get current subpage parameters</summary>
        /// <param name="xmax">Returned value of the position of the right hand edge of the subpage in millimeters.</param>
        /// <param name="xmin">Returned value of the position of the left hand edge of the subpage in millimeters.</param>
        /// <param name="ymax">Returned value of the position of the top edge of the subpage in millimeters.</param>
        /// <param name="ymin">Returned value of the position of the bottom edge of the subpage in millimeters.</param>
        /// <remarks>Gets the size of the current subpage in millimeters measured from the bottom left hand corner of the output device page or screen. Can be used in conjunction with plsvpa for setting the size of a viewport in absolute coordinates (millimeters).</remarks>
        [DllImport(DllName, EntryPoint = "c_plgspa")]
        public static extern void gspa(out PLFLT xmin, out PLFLT xmax, out PLFLT ymin, out PLFLT ymax);

        /// <summary>plgstrm: Get current stream number</summary>
        /// <param name="p_strm">Returned value of the current stream value.</param>
        /// <remarks>Gets the number of the current output stream. See also plsstrm.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgstrm")]
        public static extern void gstrm(out PLINT p_strm);

        /// <summary>Get the current library version number</summary>
        [DllImport(DllName, EntryPoint = "c_plgver")]
        private static extern void _gver([MarshalAs(UnmanagedType.LPStr)] StringBuilder p_ver);

        /// <summary>plgver: Get the current library version number</summary>
        /// <param name="p_ver">Returned ascii character string (with preallocated length of 80 characters or more) containing the PLplot version number.</param>
        /// <remarks>Get the current library version number. Note: you must have allocated space for this (80 characters is safe).</remarks>
        public static void gver(out string p_ver)
        {
            var sb_p_ver = new StringBuilder(MaxLength);
            _gver(sb_p_ver);
            p_ver = sb_p_ver.ToString();
        }

        /// <summary>plgvpd: Get viewport limits in normalized device coordinates</summary>
        /// <param name="p_xmax">Returned value of the upper viewport limit of the normalized device coordinate in x.</param>
        /// <param name="p_xmin">Returned value of the lower viewport limit of the normalized device coordinate in x.</param>
        /// <param name="p_ymax">Returned value of the upper viewport limit of the normalized device coordinate in y.</param>
        /// <param name="p_ymin">Returned value of the lower viewport limit of the normalized device coordinate in y.</param>
        /// <remarks>Get viewport limits in normalized device coordinates.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgvpd")]
        public static extern void gvpd(out PLFLT p_xmin, out PLFLT p_xmax, out PLFLT p_ymin, out PLFLT p_ymax);

        /// <summary>plgvpw: Get viewport limits in world coordinates</summary>
        /// <param name="p_xmax">Returned value of the upper viewport limit of the world coordinate in x.</param>
        /// <param name="p_xmin">Returned value of the lower viewport limit of the world coordinate in x.</param>
        /// <param name="p_ymax">Returned value of the upper viewport limit of the world coordinate in y.</param>
        /// <param name="p_ymin">Returned value of the lower viewport limit of the world coordinate in y.</param>
        /// <remarks>Get viewport limits in world coordinates.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgvpw")]
        public static extern void gvpw(out PLFLT p_xmin, out PLFLT p_xmax, out PLFLT p_ymin, out PLFLT p_ymax);

        /// <summary>plgxax: Get x axis parameters</summary>
        /// <param name="p_digits">Returned value of the actual number of digits for the numeric labels (x axis) from the last plot.</param>
        /// <param name="p_digmax">Returned value of the maximum number of digits for the x axis. If nonzero, the printed label has been switched to a floating-point representation when the number of digits exceeds this value.</param>
        /// <remarks>Returns current values of the p_digmax and p_digits flags for the x axis. p_digits is updated after the plot is drawn, so this routine should only be called after the call to plbox (or plbox3) is complete. See  for more information.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgxax")]
        public static extern void gxax(out PLINT p_digmax, out PLINT p_digits);

        /// <summary>plgyax: Get y axis parameters</summary>
        /// <param name="p_digits">Returned value of the actual number of digits for the numeric labels (y axis) from the last plot.</param>
        /// <param name="p_digmax">Returned value of the maximum number of digits for the y axis. If nonzero, the printed label has been switched to a floating-point representation when the number of digits exceeds this value.</param>
        /// <remarks>Identical to plgxax, except that arguments are flags for y axis. See the description of plgxax for more detail.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgyax")]
        public static extern void gyax(out PLINT p_digmax, out PLINT p_digits);

        /// <summary>plgzax: Get z axis parameters</summary>
        /// <param name="p_digits">Returned value of the actual number of digits for the numeric labels (z axis) from the last plot.</param>
        /// <param name="p_digmax">Returned value of the maximum number of digits for the z axis. If nonzero, the printed label has been switched to a floating-point representation when the number of digits exceeds this value.</param>
        /// <remarks>Identical to plgxax, except that arguments are flags for z axis. See the description of plgxax for more detail.</remarks>
        [DllImport(DllName, EntryPoint = "c_plgzax")]
        public static extern void gzax(out PLINT p_digmax, out PLINT p_digits);

        /// <summary>Draws a histogram of n values of a variable in array data[0..n-1]</summary>
        [DllImport(DllName, EntryPoint = "c_plhist")]
        private static extern void _hist(PLINT n, [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] data,
                                                 PLFLT datmin, PLFLT datmax, PLINT nbin, Hist opt);

        /// <summary>plhist: Plot a histogram from unbinned data</summary>
        /// <param name="data">A vector containing the values of the n data points.</param>
        /// <param name="datmax">Right-hand edge of highest-valued bin.</param>
        /// <param name="datmin">Left-hand edge of lowest-valued bin.</param>
        /// <param name="nbin">Number of (equal-sized) bins into which to divide the interval xmin to xmax.</param>
        /// <param name="opt">Is a combination of several flags: opt=PL_HIST_DEFAULT: The axes are automatically rescaled to fit the histogram data, the outer bins are expanded to fill up the entire x-axis, data outside the given extremes are assigned to the outer bins and bins of zero height are simply drawn. opt=PL_HIST_NOSCALING|...: The existing axes are not rescaled to fit the histogram data, without this flag, plenv is called to set the world coordinates. opt=PL_HIST_IGNORE_OUTLIERS|...: Data outside the given extremes are not taken into account. This option should probably be combined with opt=PL_HIST_NOEXPAND|..., so as to properly present the data. opt=PL_HIST_NOEXPAND|...: The outer bins are drawn with equal size as the ones inside. opt=PL_HIST_NOEMPTY|...: Bins with zero height are not drawn (there is a gap for such bins).</param>
        /// <remarks>Plots a histogram from n data points stored in the data vector. This routine bins the data into nbin bins equally spaced between datmin and datmax, and calls plbin to draw the resulting histogram. Parameter opt allows, among other things, the histogram either to be plotted in an existing window or causes plhist to call plenv with suitable limits before plotting the histogram.</remarks>
        public static void hist(PLFLT[] data, PLFLT datmin, PLFLT datmax, PLINT nbin, Hist opt)
        {
            _hist(data.Length, data, datmin, datmax, nbin, opt);
        }

        /// <summary>plhlsrgb: Convert HLS color to RGB</summary>
        /// <param name="h">Hue in degrees (0.0-360.0) on the color cylinder.</param>
        /// <param name="l">Lightness expressed as a fraction (0.0-1.0) of the axis of the color cylinder.</param>
        /// <param name="p_b">Returned value of the blue intensity (0.0-1.0) of the color.</param>
        /// <param name="p_g">Returned value of the green intensity (0.0-1.0) of the color.</param>
        /// <param name="p_r">Returned value of the red intensity (0.0-1.0) of the color.</param>
        /// <param name="s">Saturation expressed as a fraction (0.0-1.0) of the radius of the color cylinder.</param>
        /// <remarks>Convert HLS color coordinates to RGB.</remarks>
        [DllImport(DllName, EntryPoint = "c_plhlsrgb")]
        public static extern void hlsrgb(PLFLT h, PLFLT l, PLFLT s, out PLFLT p_r, out PLFLT p_g, out PLFLT p_b);

        /// <summary>plinit: Initialize PLplot</summary>
        /// <remarks>Initializing the plotting package. The program prompts for the device keyword or number of the desired output device. Hitting a RETURN in response to the prompt is the same as selecting the first device. plinit will issue no prompt if either the device was specified previously (via command line flag, the plsetopt function, or the plsdev function), or if only one device is enabled when PLplot is installed. If subpages have been specified, the output device is divided into nx by ny subpages, each of which may be used independently. If plinit is called again during a program, the previously opened file will be closed. The subroutine pladv is used to advance from one subpage to the next.</remarks>
        [DllImport(DllName, EntryPoint = "c_plinit")]
        public static extern void init();

        /// <summary>pljoin: Draw a line between two points</summary>
        /// <param name="x1">x coordinate of first point.</param>
        /// <param name="x2">x coordinate of second point.</param>
        /// <param name="y1">y coordinate of first point.</param>
        /// <param name="y2">y coordinate of second point.</param>
        /// <remarks>Joins the point (x1, y1) to (x2, y2).</remarks>
        [DllImport(DllName, EntryPoint = "c_pljoin")]
        public static extern void join(PLFLT x1, PLFLT y1, PLFLT x2, PLFLT y2);

        /// <summary>pllab: Simple routine to write labels</summary>
        /// <param name="tlabel">A UTF-8 character string specifying the title of the plot.</param>
        /// <param name="xlabel">A UTF-8 character string specifying the label for the x axis.</param>
        /// <param name="ylabel">A UTF-8 character string specifying the label for the y axis.</param>
        /// <remarks>Routine for writing simple labels. Use plmtex for more complex labels.</remarks>
        [DllImport(DllName, EntryPoint = "c_pllab")]
        public static extern void lab([MarshalAs(UnmanagedType.LPStr)] string xlabel,
                                              [MarshalAs(UnmanagedType.LPStr)] string ylabel,
                                              [MarshalAs(UnmanagedType.LPStr)] string tlabel);

        /// <summary>Routine for drawing discrete line, symbol, or cmap0 legends</summary>
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

        /// <summary>pllegend: Plot legend using discretely annotated filled boxes, lines, and/or lines of symbols</summary>
        /// <param name="bb_color">The cmap0 color of the bounding-box line for the legend (PL_LEGEND_BOUNDING_BOX).</param>
        /// <param name="bb_style">The pllsty style number for the bounding-box line for the legend (PL_LEGEND_BACKGROUND).</param>
        /// <param name="bg_color">The cmap0 color of the background for the legend (PL_LEGEND_BACKGROUND).</param>
        /// <param name="box_colors">A vector containing nlegend cmap0 colors for the discrete colored boxes (PL_LEGEND_COLOR_BOX).</param>
        /// <param name="box_line_widths">A vector containing nlegend line widths for the patterns specified by box_patterns (PL_LEGEND_COLOR_BOX).</param>
        /// <param name="box_patterns">A vector containing nlegend patterns (plpsty indices) for the discrete colored boxes (PL_LEGEND_COLOR_BOX).</param>
        /// <param name="box_scales">A vector containing nlegend scales (units of fraction of character height) for the height of the discrete colored boxes (PL_LEGEND_COLOR_BOX).</param>
        /// <param name="line_colors">A vector containing nlegend cmap0 line colors (PL_LEGEND_LINE).</param>
        /// <param name="line_styles">A vector containing nlegend line styles (plsty indices) (PL_LEGEND_LINE).</param>
        /// <param name="line_widths">A vector containing nlegend line widths (PL_LEGEND_LINE).</param>
        /// <param name="ncolumn">The cmap0 index of the background color for the legend (PL_LEGEND_BACKGROUND).</param>
        /// <param name="nrow">The cmap0 index of the background color for the legend (PL_LEGEND_BACKGROUND).</param>
        /// <param name="opt">opt contains bits controlling the overall legend. If the PL_LEGEND_TEXT_LEFT bit is set, put the text area on the left of the legend and the plotted area on the right. Otherwise, put the text area on the right of the legend and the plotted area on the left. If the PL_LEGEND_BACKGROUND bit is set, plot a (semitransparent) background for the legend. If the PL_LEGEND_BOUNDING_BOX bit is set, plot a bounding box for the legend. If the PL_LEGEND_ROW_MAJOR bit is set and (both of the possibly internally transformed) nrow gt 1 and ncolumn gt 1, then plot the resulting array of legend entries in row-major order. Otherwise, plot the legend entries in column-major order.</param>
        /// <param name="opt_array">A vector of nlegend values of options to control each individual plotted area corresponding to a legend entry. If the PL_LEGEND_NONE bit is set, then nothing is plotted in the plotted area. If the PL_LEGEND_COLOR_BOX, PL_LEGEND_LINE, and/or PL_LEGEND_SYMBOL bits are set, the area corresponding to a legend entry is plotted with a colored box; a line; and/or a line of symbols.</param>
        /// <param name="p_legend_height">Returned value of the legend height in adopted coordinates. This quantity is calculated from text_scale, text_spacing, and nrow (possibly modified inside the routine depending on nlegend and nrow).</param>
        /// <param name="p_legend_width">Returned value of the legend width in adopted coordinates. This quantity is calculated from plot_width, text_offset, ncolumn (possibly modified inside the routine depending on nlegend and nrow), and the length (calculated internally) of the longest text string.</param>
        /// <param name="plot_width">Horizontal width in adopted coordinates of the plot area (where the colored boxes, lines, and/or lines of symbols are drawn) of the legend.</param>
        /// <param name="position">position contains bits which control the overall position of the legend and the definition of the adopted coordinates used for positions just like what is done for the position argument for plcolorbar. However, note that the defaults for the position bits (see below) are different than the plcolorbar case. The combination of the PL_POSITION_LEFT, PL_POSITION_RIGHT, PL_POSITION_TOP, PL_POSITION_BOTTOM, PL_POSITION_INSIDE, and PL_POSITION_OUTSIDE bits specifies one of the 16 possible standard positions (the 4 corners and centers of the 4 sides for both the inside and outside cases) of the legend relative to the adopted coordinate system. The corner positions are specified by the appropriate combination of two of the PL_POSITION_LEFT, PL_POSITION_RIGHT, PL_POSITION_TOP, and PL_POSITION_BOTTOM bits while the sides are specified by a single value of one of those bits. The adopted coordinates are normalized viewport coordinates if the PL_POSITION_VIEWPORT bit is set or normalized subpage coordinates if the PL_POSITION_SUBPAGE bit is set. Default position bits: If none of PL_POSITION_LEFT, PL_POSITION_RIGHT, PL_POSITION_TOP, or PL_POSITION_BOTTOM are set, then use the combination of PL_POSITION_RIGHT and PL_POSITION_TOP. If neither of PL_POSITION_INSIDE or PL_POSITION_OUTSIDE is set, use PL_POSITION_INSIDE. If neither of PL_POSITION_VIEWPORT or PL_POSITION_SUBPAGE is set, use PL_POSITION_VIEWPORT.</param>
        /// <param name="symbol_colors">A vector containing nlegend cmap0 symbol colors (PL_LEGEND_SYMBOL).</param>
        /// <param name="symbol_numbers">A vector containing nlegend numbers of symbols to be drawn across the width of the plotted area (PL_LEGEND_SYMBOL).</param>
        /// <param name="symbol_scales">A vector containing nlegend scale values for the symbol height (PL_LEGEND_SYMBOL).</param>
        /// <param name="symbols">A vector of nlegend UTF-8 character strings containing the legend symbols. (PL_LEGEND_SYMBOL).</param>
        /// <param name="text">A vector of nlegend UTF-8 character strings containing the legend annotations.</param>
        /// <param name="text_colors">A vector containing nlegend cmap0 text colors.</param>
        /// <param name="text_justification">Justification parameter used for text justification. The most common values of text_justification are 0., 0.5, or 1. corresponding to a text that is left justified, centred, or right justified within the text area, but other values are allowed as well.</param>
        /// <param name="text_offset">Offset of the text area from the plot area in units of character width. N.B. The total horizontal width of the legend in adopted coordinates is calculated internally from plot_width (see above), text_offset, and length (calculated internally) of the longest text string.</param>
        /// <param name="text_scale">Character height scale for text annotations. N.B. The total vertical height of the legend in adopted coordinates is calculated internally from nlegend (see above), text_scale, and text_spacing (see below).</param>
        /// <param name="text_spacing">Vertical spacing in units of the character height from one legend entry to the next. N.B. The total vertical height of the legend in adopted coordinates is calculated internally from nlegend (see above), text_scale (see above), and text_spacing.</param>
        /// <param name="x">X offset of the legend position in adopted coordinates from the specified standard position of the legend. For positive x, the direction of motion away from the standard position is inward/outward from the standard corner positions or standard left or right positions if the PL_POSITION_INSIDE/PL_POSITION_OUTSIDE bit is set in position. For the standard top or bottom positions, the direction of motion is toward positive X.</param>
        /// <param name="y">Y offset of the legend position in adopted coordinates from the specified standard position of the legend. For positive y, the direction of motion away from the standard position is inward/outward from the standard corner positions or standard top or bottom positions if the PL_POSITION_INSIDE/PL_POSITION_OUTSIDE bit is set in position. For the standard left or right positions, the direction of motion is toward positive Y.</param>
        /// <remarks>Routine for creating a discrete plot legend with a plotted filled box, line, and/or line of symbols for each annotated legend entry. (See plcolorbar for similar functionality for creating continuous color bars.) The arguments of pllegend provide control over the location and size of the legend as well as the location and characteristics of the elements (most of which are optional) within that legend. The resulting legend is clipped at the boundaries of the current subpage. (N.B. the adopted coordinate system used for some of the parameters is defined in the documentation of the position parameter.)</remarks>
        public static void legend(
                            out PLFLT p_legend_width, out PLFLT p_legend_height,
                            Legend opt, Position position, PLFLT x, PLFLT y, PLFLT plot_width,
                            PLINT bg_color, PLINT bb_color, LineStyle bb_style,
                            PLINT nrow, PLINT ncolumn,
                            LegendEntry[] opt_array,
                            PLFLT text_offset, PLFLT text_scale, PLFLT text_spacing, PLFLT text_justification,
                            PLINT[] text_colors, PLUTF8_STRING[] text, PLINT[] box_colors, Pattern[] box_patterns,
                            PLFLT[] box_scales, PLFLT[] box_line_widths, PLINT[] line_colors, LineStyle[] line_styles,
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

        /// <summary>Routine for drawing continuous colour legends</summary>
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

        /// <summary>plcolorbar: Plot color bar for image, shade or gradient plots</summary>
        /// <param name="axis_opts">A vector of n_axes ascii character strings containing options (interpreted as for plbox) for the color bar's axis definitions.</param>
        /// <param name="bb_color">The cmap0 color of the bounding-box line for the color bar (PL_COLORBAR_BOUNDING_BOX).</param>
        /// <param name="bb_style">The pllsty style number for the bounding-box line for the color bar (PL_COLORBAR_BACKGROUND).</param>
        /// <param name="bg_color">The cmap0 color of the background for the color bar (PL_COLORBAR_BACKGROUND).</param>
        /// <param name="cont_color">The cmap0 contour color for PL_COLORBAR_SHADE plots. This is passed directly to plshades, so it will be interpreted according to the design of plshades.</param>
        /// <param name="cont_width">Contour width for PL_COLORBAR_SHADE plots. This is passed directly to plshades, so it will be interpreted according to the design of plshades.</param>
        /// <param name="high_cap_color">The cmap1 color of the high-end color bar cap, if it is drawn (PL_COLORBAR_CAP_HIGH).</param>
        /// <param name="label_opts">A vector of options for each of n_labels labels.</param>
        /// <param name="labels">A vector of n_labels UTF-8 character strings containing the labels for the color bar. Ignored if no label position is specified with one of the PL_COLORBAR_LABEL_RIGHT, PL_COLORBAR_LABEL_TOP, PL_COLORBAR_LABEL_LEFT, or PL_COLORBAR_LABEL_BOTTOM bits in the corresponding label_opts field.</param>
        /// <param name="low_cap_color">The cmap1 color of the low-end color bar cap, if it is drawn (PL_COLORBAR_CAP_LOW).</param>
        /// <param name="opt">opt contains bits controlling the overall color bar. The orientation (direction of the maximum value) of the color bar is specified with PL_ORIENT_RIGHT, PL_ORIENT_TOP, PL_ORIENT_LEFT, or PL_ORIENT_BOTTOM. If none of these bits are specified, the default orientation is toward the top if the colorbar is placed on the left or right of the viewport or toward the right if the colorbar is placed on the top or bottom of the viewport. If the PL_COLORBAR_BACKGROUND bit is set, plot a (semitransparent) background for the color bar. If the PL_COLORBAR_BOUNDING_BOX bit is set, plot a bounding box for the color bar. The type of color bar must be specified with one of PL_COLORBAR_IMAGE, PL_COLORBAR_SHADE, or PL_COLORBAR_GRADIENT. If more than one of those bits is set only the first one in the above list is honored. The position of the (optional) label/title can be specified with PL_LABEL_RIGHT, PL_LABEL_TOP, PL_LABEL_LEFT, or PL_LABEL_BOTTOM. If no label position bit is set then no label will be drawn. If more than one of this list of bits is specified, only the first one on the list is honored. End-caps for the color bar can added with PL_COLORBAR_CAP_LOW and PL_COLORBAR_CAP_HIGH. If a particular color bar cap option is not specified then no cap will be drawn for that end. As a special case for PL_COLORBAR_SHADE, the option PL_COLORBAR_SHADE_LABEL can be specified. If this option is provided then any tick marks and tick labels will be placed at the breaks between shaded segments. TODO: This should be expanded to support custom placement of tick marks and tick labels at custom value locations for any color bar type.</param>
        /// <param name="p_colorbar_height">Returned value of the labelled and decorated color bar height in adopted coordinates.</param>
        /// <param name="p_colorbar_width">Returned value of the labelled and decorated color bar width in adopted coordinates.</param>
        /// <param name="position">position contains bits which control the overall position of the color bar and the definition of the adopted coordinates used for positions just like what is done for the position argument for pllegend. However, note that the defaults for the position bits (see below) are different than the pllegend case. The combination of the PL_POSITION_LEFT, PL_POSITION_RIGHT, PL_POSITION_TOP, PL_POSITION_BOTTOM, PL_POSITION_INSIDE, and PL_POSITION_OUTSIDE bits specifies one of the 16 possible standard positions (the 4 corners and centers of the 4 sides for both the inside and outside cases) of the color bar relative to the adopted coordinate system. The corner positions are specified by the appropriate combination of two of the PL_POSITION_LEFT, PL_POSITION_RIGHT, PL_POSITION_TOP, and PL_POSITION_BOTTOM bits while the sides are specified by a single value of one of those bits. The adopted coordinates are normalized viewport coordinates if the PL_POSITION_VIEWPORT bit is set or normalized subpage coordinates if the PL_POSITION_SUBPAGE bit is set. Default position bits: If none of PL_POSITION_LEFT, PL_POSITION_RIGHT, PL_POSITION_TOP, or PL_POSITION_BOTTOM are set, then use PL_POSITION_RIGHT. If neither of PL_POSITION_INSIDE or PL_POSITION_OUTSIDE is set, use PL_POSITION_OUTSIDE. If neither of PL_POSITION_VIEWPORT or PL_POSITION_SUBPAGE is set, use PL_POSITION_VIEWPORT.</param>
        /// <param name="sub_ticks">A vector of n_axes values of the number of subticks (interpreted as for plbox) for the color bar's axis definitions.</param>
        /// <param name="ticks">A vector of n_axes values of the spacing of the major tick marks (interpreted as for plbox) for the color bar's axis definitions.</param>
        /// <param name="values">A matrix containing the numeric values for the data range represented by the color bar. For a row index of i_axis (where 0 lt i_axis lt n_axes), the number of elements in the row is specified by n_values[i_axis]. For PL_COLORBAR_IMAGE and PL_COLORBAR_GRADIENT the number of elements is 2, and the corresponding row elements of the values matrix are the minimum and maximum value represented by the colorbar. For PL_COLORBAR_SHADE, the number and values of the elements of a row of the values matrix is interpreted the same as the nlevel and clevel arguments of plshades.</param>
        /// <param name="x">X offset of the color bar position in adopted coordinates from the specified standard position of the color bar. For positive x, the direction of motion away from the standard position is inward/outward from the standard corner positions or standard left or right positions if the PL_POSITION_INSIDE/PL_POSITION_OUTSIDE bit is set in position. For the standard top or bottom positions, the direction of motion is toward positive X.</param>
        /// <param name="x_length">Length of the body of the color bar in the X direction in adopted coordinates.</param>
        /// <param name="y">Y offset of the color bar position in adopted coordinates from the specified standard position of the color bar. For positive y, the direction of motion away from the standard position is inward/outward from the standard corner positions or standard top or bottom positions if the PL_POSITION_INSIDE/PL_POSITION_OUTSIDE bit is set in position. For the standard left or right positions, the direction of motion is toward positive Y.</param>
        /// <param name="y_length">Length of the body of the color bar in the Y direction in adopted coordinates.</param>
        /// <remarks>Routine for creating a continuous color bar for image, shade, or gradient plots. (See pllegend for similar functionality for creating legends with discrete elements). The arguments of plcolorbar provide control over the location and size of the color bar as well as the location and characteristics of the elements (most of which are optional) within that color bar. The resulting color bar is clipped at the boundaries of the current subpage. (N.B. the adopted coordinate system used for some of the parameters is defined in the documentation of the position parameter.)</remarks>
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
                for (int row = 0; row < mat_values.NX; row++)
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

        /// <summary>pllightsource: Sets the 3D position of the light source</summary>
        /// <param name="x">X-coordinate of the light source.</param>
        /// <param name="y">Y-coordinate of the light source.</param>
        /// <param name="z">Z-coordinate of the light source.</param>
        /// <remarks>Sets the 3D position of the light source for use with plsurf3d and plsurf3dl</remarks>
        [DllImport(DllName, EntryPoint = "c_pllightsource")]
        public static extern void lightsource(PLFLT x, PLFLT y, PLFLT z);

        /// <summary>Draws line segments connecting a series of points.</summary>
        [DllImport(DllName, EntryPoint = "c_plline")]
        private static extern void _line(PLINT n,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y);

        /// <summary>plline: Draw a line</summary>
        /// <param name="x">A vector containing the x coordinates of points.</param>
        /// <param name="y">A vector containing the y coordinates of points.</param>
        /// <remarks>Draws line defined by n points in x and y.</remarks>
        public static void line(PLFLT[] x, PLFLT[] y)
        {
            _line(GetSize(x, y), x, y);
        }

        /// <summary>Draws a line in 3 space.</summary>
        [DllImport(DllName, EntryPoint = "c_plline3")]
        private static extern void _line3(PLINT n,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z);

        /// <summary>plline3: Draw a line in 3 space</summary>
        /// <param name="x">A vector containing the x coordinates of points.</param>
        /// <param name="y">A vector containing the y coordinates of points.</param>
        /// <param name="z">A vector containing the z coordinates of points.</param>
        /// <remarks>Draws line in 3 space defined by n points in x, y, and z. You must first set up the viewport, the 2d viewing window (in world coordinates), and the 3d normalized coordinate box. See x18c.c for more info.</remarks>
        public static void line3(PLFLT[] x, PLFLT[] y, PLFLT[] z)
        {
            _line3(GetSize(x, y, z), x, y, z);
        }

        /// <summary>pllsty: Select line style</summary>
        /// <param name="lin">Integer value between 1 and 8. Line style 1 is a continuous line, line style 2 is a line with short dashes and gaps, line style 3 is a line with long dashes and gaps, line style 4 has long dashes and short gaps and so on.</param>
        /// <remarks>This sets the line style according to one of eight predefined patterns (also see plstyl).</remarks>
        [DllImport(DllName, EntryPoint = "c_pllsty")]
        public static extern void lsty(LineStyle lin);

        /// <summary>plmap: Plot continental outline or shapefile data in world coordinates</summary>
        /// <param name="mapform">A user supplied function to transform the original map data coordinates to a new coordinate system. The PLplot-supplied map data is provided as latitudes and longitudes; other Shapefile data may be provided in other coordinate systems as can be found in their .prj plain text files. For example, by using this transform we can change from a longitude, latitude coordinate to a polar stereographic projection. Initially, x[0]..[n-1] are the original x coordinates (longitudes for the PLplot-supplied data) and y[0]..y[n-1] are the corresponding y coordinates (latitudes for the PLplot supplied data). After the call to mapform(), x[] and y[] should be replaced by the corresponding plot coordinates. If no transform is desired, mapform can be replaced by NULL.</param>
        /// <param name="maxx">The maximum x value of map elements to be drawn</param>
        /// <param name="maxy">The maximum y value of map elements to be drawn.</param>
        /// <param name="minx">The minimum x value of map elements to be drawn. For the built in maps this is a measure of longitude. For Shapefiles the units must match the projection. The value of minx must be less than the value of maxx. Specifying a useful limit for these limits provides a useful optimization for complex or detailed maps.</param>
        /// <param name="miny">The minimum y value of map elements to be drawn. For the built in maps this is a measure of latitude. For Shapefiles the units must match the projection. The value of miny must be less than the value of maxy.</param>
        /// <param name="name">An ascii character string specifying the type of map plotted. This is either one of the PLplot built-in maps or the file name of a set of Shapefile files without the file extensions. For the PLplot built-in maps the possible values are: "globe" -- continental outlines "usa" -- USA and state boundaries "cglobe" -- continental outlines and countries "usaglobe" -- USA, state boundaries and continental outlines</param>
        /// <remarks>Plots continental outlines or shapefile data in world coordinates. A demonstration of how to use this function to create different projections can be found in examples/c/x19c. PLplot is provided with basic coastal outlines and USA state borders. These can be used irrespective of whether Shapefile support is built into PLplot. With Shapefile support this function can also be used with user Shapefiles, in which case it will plot the entire contents of a Shapefile joining each point of each Shapefile element with a line. Shapefiles have become a popular standard for geographical data and data in this format can be easily found from a number of online sources. Shapefile data is actually provided as three or more files with the same filename, but different extensions. The .shp and .shx files are required for plotting Shapefile data with PLplot.</remarks>
        [DllImport(DllName, EntryPoint = "c_plmap")]
        public static extern void map(MapFunc mapform, [MarshalAs(UnmanagedType.LPStr)] string name,
                                              PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy);

        /// <summary>Plot map outlines</summary>
        [DllImport(DllName, EntryPoint = "c_plmapline")]
        private static extern void _mapline(MapFunc mapform, [MarshalAs(UnmanagedType.LPStr)] string name,
                                                    PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] plotentries, PLINT nplotentries);

        /// <summary>plmapline: Plot all or a subset of Shapefile data using lines in world coordinates</summary>
        /// <param name="mapform">A user supplied function to transform the coordinates given in the shapefile into a plot coordinate system. By using this transform, we can change from a longitude, latitude coordinate to a polar stereographic project, for example. Initially, x[0]..[n-1] are the longitudes and y[0]..y[n-1] are the corresponding latitudes. After the call to mapform(), x[] and y[] should be replaced by the corresponding plot coordinates. If no transform is desired, mapform can be replaced by NULL.</param>
        /// <param name="maxx">The maximum x value to be plotted. You could use a very large number to plot everything, but you can improve performance by limiting the area drawn.</param>
        /// <param name="maxy">The maximum y value to be plotted. You could use a very large number to plot everything, but you can improve performance by limiting the area drawn.</param>
        /// <param name="minx">The minimum x value to be plotted. This must be in the same units as used by the Shapefile. You could use a very large negative number to plot everything, but you can improve performance by limiting the area drawn. The units must match those of the Shapefile projection, which may be for example longitude or distance. The value of minx must be less than the value of maxx.</param>
        /// <param name="miny">The minimum y value to be plotted. This must be in the same units as used by the Shapefile. You could use a very large negative number to plot everything, but you can improve performance by limiting the area drawn. The units must match those of the Shapefile projection, which may be for example latitude or distance. The value of miny must be less than the value of maxy.</param>
        /// <param name="name">An ascii character string specifying the file name of a set of Shapefile files without the file extension.</param>
        /// <param name="plotentries">A vector containing the zero-based indices of the Shapefile elements which will be drawn. Setting plotentries to NULL will plot all elements of the Shapefile.</param>
        /// <remarks>Plot all or a subset of Shapefile data using lines in world coordinates. Our 19th standard example demonstrates how to use this function. This function plots data from a Shapefile using lines as in plmap, however it also has the option of also only drawing specified elements from the Shapefile. The vector of indices of the required elements are passed as a function argument. The Shapefile data should include a metadata file (extension.dbf) listing all items within the Shapefile. This file can be opened by most popular spreadsheet programs and can be used to decide which indices to pass to this function.</remarks>
        public static void mapline(MapFunc mapform, string name,
                                           PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                           PLINT[] plotentries)
        {
            _mapline(mapform, name, minx, maxx, miny, maxy, plotentries, plotentries.Length);
        }

        /// <summary>Plot map points</summary>
        [DllImport(DllName, EntryPoint = "c_plmapstring")]
        private static extern void _mapstring(MapFunc mapform,
                                                      [MarshalAs(UnmanagedType.LPStr)] string name,
                                                      [MarshalAs(UnmanagedType.LPStr)] string str,
                                                      PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                                      [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] plotentries, PLINT nplotentries);

        /// <summary>plmapstring: Plot all or a subset of Shapefile data using strings or points in world coordinates</summary>
        /// <param name="mapform">A user supplied function to transform the coordinates given in the shapefile into a plot coordinate system. By using this transform, we can change from a longitude, latitude coordinate to a polar stereographic project, for example. Initially, x[0]..[n-1] are the longitudes and y[0]..y[n-1] are the corresponding latitudes. After the call to mapform(), x[] and y[] should be replaced by the corresponding plot coordinates. If no transform is desired, mapform can be replaced by NULL.</param>
        /// <param name="maxx">The maximum x value to be plotted. You could use a very large number to plot everything, but you can improve performance by limiting the area drawn.</param>
        /// <param name="maxy">The maximum y value to be plotted. You could use a very large number to plot everything, but you can improve performance by limiting the area drawn.</param>
        /// <param name="minx">The minimum x value to be plotted. This must be in the same units as used by the Shapefile. You could use a very large negative number to plot everything, but you can improve performance by limiting the area drawn. The units must match those of the Shapefile projection, which may be for example longitude or distance. The value of minx must be less than the value of maxx.</param>
        /// <param name="miny">The minimum y value to be plotted. This must be in the same units as used by the Shapefile. You could use a very large negative number to plot everything, but you can improve performance by limiting the area drawn. The units must match those of the Shapefile projection, which may be for example latitude or distance. The value of miny must be less than the value of maxy.</param>
        /// <param name="name">An ascii character string specifying the file name of a set of Shapefile files without the file extension.</param>
        /// <param name="plotentries">A vector containing the zero-based indices of the Shapefile elements which will be drawn. Setting plotentries to NULL will plot all elements of the Shapefile.</param>
        /// <remarks>As per plmapline, however the items are plotted as strings or points in the same way as plstring.</remarks>
        public static void mapstring(MapFunc mapform, string name, string str,
                                             PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                             PLINT[] plotentries)
        {
            _mapstring(mapform, name, str, minx, maxx, miny, maxy, plotentries, plotentries.Length);
        }

        /// <summary>plmaptex: Draw text at points defined by Shapefile data in world coordinates</summary>
        /// <param name="dx">Used to define the slope of the texts which is dy/dx.</param>
        /// <param name="dy">Used to define the slope of the texts which is dy/dx.</param>
        /// <param name="just">Set the justification of the text. The value given will be the fraction of the distance along the string that sits at the given point. 0.0 gives left aligned text, 0.5 gives centralized text and 1.0 gives right aligned text.</param>
        /// <param name="mapform">A user supplied function to transform the coordinates given in the shapefile into a plot coordinate system. By using this transform, we can change from a longitude, latitude coordinate to a polar stereographic project, for example. Initially, x[0]..[n-1] are the longitudes and y[0]..y[n-1] are the corresponding latitudes. After the call to mapform(), x[] and y[] should be replaced by the corresponding plot coordinates. If no transform is desired, mapform can be replaced by NULL.</param>
        /// <param name="maxx">The maximum x value to be plotted. You could use a very large number to plot everything, but you can improve performance by limiting the area drawn.</param>
        /// <param name="maxy">The maximum y value to be plotted. You could use a very large number to plot everything, but you can improve performance by limiting the area drawn.</param>
        /// <param name="minx">The minimum x value to be plotted. This must be in the same units as used by the Shapefile. You could use a very large negative number to plot everything, but you can improve performance by limiting the area drawn. The units must match those of the Shapefile projection, which may be for example longitude or distance. The value of minx must be less than the value of maxx.</param>
        /// <param name="miny">The minimum y value to be plotted. This must be in the same units as used by the Shapefile. You could use a very large negative number to plot everything, but you can improve performance by limiting the area drawn. The units must match those of the Shapefile projection, which may be for example latitude or distance. The value of miny must be less than the value of maxy.</param>
        /// <param name="name">An ascii character string specifying the file name of a set of Shapefile files without the file extension.</param>
        /// <param name="plotentry">An integer indicating which text string of the Shapefile (zero indexed) will be drawn.</param>
        /// <param name="text">A UTF-8 character string to be drawn.</param>
        /// <remarks>As per plmapline, however the items are plotted as text in the same way as plptex.</remarks>
        [DllImport(DllName, EntryPoint = "c_plmaptex")]
        public static extern void maptex(MapFunc mapform,
                                                 [MarshalAs(UnmanagedType.LPStr)] string name, PLFLT dx, PLFLT dy, PLFLT just,
                                                 [MarshalAs(UnmanagedType.LPStr)] string text,
                                                 PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                                 PLINT plotentry);

        /// <summary>Plot map fills</summary>
        [DllImport(DllName, EntryPoint = "c_plmapfill")]
        private static extern void _mapfill(MapFunc mapform,
                                                    [MarshalAs(UnmanagedType.LPStr)] string name, PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] plotentries, PLINT nplotentries);

        /// <summary>plmapfill: Plot all or a subset of Shapefile data, filling the polygons</summary>
        /// <param name="mapform">A user supplied function to transform the coordinates given in the shapefile into a plot coordinate system. By using this transform, we can change from a longitude, latitude coordinate to a polar stereographic project, for example. Initially, x[0]..[n-1] are the longitudes and y[0]..y[n-1] are the corresponding latitudes. After the call to mapform(), x[] and y[] should be replaced by the corresponding plot coordinates. If no transform is desired, mapform can be replaced by NULL.</param>
        /// <param name="maxx">The maximum x value to be plotted. You could use a very large number to plot everything, but you can improve performance by limiting the area drawn.</param>
        /// <param name="maxy">The maximum y value to be plotted. You could use a very large number to plot everything, but you can improve performance by limiting the area drawn.</param>
        /// <param name="minx">The minimum x value to be plotted. This must be in the same units as used by the Shapefile. You could use a very large negative number to plot everything, but you can improve performance by limiting the area drawn. The units must match those of the Shapefile projection, which may be for example longitude or distance. The value of minx must be less than the value of maxx.</param>
        /// <param name="miny">The minimum y value to be plotted. This must be in the same units as used by the Shapefile. You could use a very large negative number to plot everything, but you can improve performance by limiting the area drawn. The units must match those of the Shapefile projection, which may be for example latitude or distance. The value of miny must be less than the value of maxy.</param>
        /// <param name="name">An ascii character string specifying the file name of a set of Shapefile files without the file extension.</param>
        /// <param name="plotentries">A vector containing the zero-based indices of the Shapefile elements which will be drawn. Setting plotentries to NULL will plot all elements of the Shapefile.</param>
        /// <remarks>As per plmapline, however the items are filled in the same way as plfill.</remarks>
        public static void mapfill(MapFunc mapform, string name, PLFLT minx, PLFLT maxx, PLFLT miny, PLFLT maxy,
                                           PLINT[] plotentries)
        {
            _mapfill(mapform, name, minx, maxx, miny, maxy, plotentries, plotentries.Length);
        }

        /// <summary>plmeridians: Plot latitude and longitude lines</summary>
        /// <param name="dlat">The interval in degrees at which the latitude lines are to be plotted.</param>
        /// <param name="dlong">The interval in degrees at which the longitude lines are to be plotted.</param>
        /// <param name="mapform">A user supplied function to transform the coordinate longitudes and latitudes to a plot coordinate system. By using this transform, we can change from a longitude, latitude coordinate to a polar stereographic project, for example. Initially, x[0]..[n-1] are the longitudes and y[0]..y[n-1] are the corresponding latitudes. After the call to mapform(), x[] and y[] should be replaced by the corresponding plot coordinates. If no transform is desired, mapform can be replaced by NULL.</param>
        /// <param name="maxlat">The maximum latitudes to be plotted on the background. One can always use 90.0 as the boundary outside the plot window will be automatically eliminated.</param>
        /// <param name="maxlong">The value of the longitude on the right side of the plot.</param>
        /// <param name="minlat">The minimum latitude to be plotted on the background. One can always use -90.0 as the boundary outside the plot window will be automatically eliminated. However, the program will be faster if one can reduce the size of the background plotted.</param>
        /// <param name="minlong">The value of the longitude on the left side of the plot. The value of minlong must be less than the value of maxlong, and the quantity maxlong-minlong must be less than or equal to 360.</param>
        /// <remarks>Displays latitude and longitude on the current plot. The lines are plotted in the current color and line style.</remarks>
        [DllImport(DllName, EntryPoint = "c_plmeridians")]
        public static extern void meridians(MapFunc mapform,
                                                    PLFLT dlong, PLFLT dlat,
                                                    PLFLT minlong, PLFLT maxlong, PLFLT minlat, PLFLT maxlat);

        /// <summary>Plots a mesh representation of the function z[x][y].</summary>
        [DllImport(DllName, EntryPoint = "c_plmesh")]
        private static extern void _mesh([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                 PLFLT_MATRIX z,
                                                 PLINT nx, PLINT ny, Mesh opt);

        /// <summary>plmesh: Plot surface mesh</summary>
        /// <param name="opt">Determines the way in which the surface is represented: opt=DRAW_LINEX : Lines are drawn showing z as a function of x for each value of y[j] . opt=DRAW_LINEY : Lines are drawn showing z as a function of y for each value of x[i] . opt=DRAW_LINEXY : Network of lines is drawn connecting points at which function is defined.</param>
        /// <param name="x">A vector containing the x coordinates at which the function is evaluated.</param>
        /// <param name="y">A vector containing the y coordinates at which the function is evaluated.</param>
        /// <param name="z">A matrix containing function values to plot. Should have dimensions of nx by ny.</param>
        /// <remarks>Plots a surface mesh within the environment set up by plw3d. The surface is defined by the matrix z[nx][ny] , the point z[i][j] being the value of the function at (x[i], y[j]). Note that the points in vectors x and y do not need to be equally spaced, but must be stored in ascending order. The parameter opt controls the way in which the surface is displayed. For further details see .</remarks>
        public static void mesh(PLFLT[] x, PLFLT[] y, PLFLT[,] z, Mesh opt)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                _mesh(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt);
            }
        }

        /// <summary>Plots a mesh representation of the function z[x][y] with contour</summary>
        [DllImport(DllName, EntryPoint = "c_plmeshc")]
        private static extern void _meshc([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                  PLFLT_MATRIX z,
                                                  PLINT nx, PLINT ny, MeshContour opt,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel);

        /// <summary>plmeshc: Magnitude colored plot surface mesh with contour</summary>
        /// <param name="clevel">A vector containing the contour levels.</param>
        /// <param name="opt">Determines the way in which the surface is represented. To specify more than one option just add the options, e.g. DRAW_LINEXY + MAG_COLOR opt=DRAW_LINEX : Lines are drawn showing z as a function of x for each value of y[j] . opt=DRAW_LINEY : Lines are drawn showing z as a function of y for each value of x[i] . opt=DRAW_LINEXY : Network of lines is drawn connecting points at which function is defined. opt=MAG_COLOR : Each line in the mesh is colored according to the z value being plotted. The color is used from the current cmap1. opt=BASE_CONT : A contour plot is drawn at the base XY plane using parameters nlevel and clevel. opt=DRAW_SIDES : draws a curtain between the base XY plane and the borders of the plotted function.</param>
        /// <param name="x">A vector containing the x coordinates at which the function is evaluated.</param>
        /// <param name="y">A vector containing the y coordinates at which the function is evaluated.</param>
        /// <param name="z">A matrix containing function values to plot. Should have dimensions of nx by ny.</param>
        /// <remarks>A more powerful form of plmesh: the surface mesh can be colored accordingly to the current z value being plotted, a contour plot can be drawn at the base XY plane, and a curtain can be drawn between the plotted function border and the base XY plane.</remarks>
        public static void meshc(PLFLT[] x, PLFLT[] y, PLFLT[,] z, MeshContour opt, PLFLT[] clevel)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                _meshc(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt, clevel, clevel.Length);
            }
        }

        /// <summary>plmkstrm: Creates a new stream and makes it the default</summary>
        /// <param name="p_strm">Returned value of the stream number of the created stream.</param>
        /// <remarks>Creates a new stream and makes it the default. Differs from using plsstrm, in that a free stream number is found, and returned. Unfortunately, I have to start at stream 1 and work upward, since stream 0 is preallocated. One of the big flaws in the PLplot API is that no initial, library-opening call is required. So stream 0 must be preallocated, and there is no simple way of determining whether it is already in use or not.</remarks>
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

        /// <summary>Plots a 3-d representation of the function z[x][y].</summary>
        [DllImport(DllName, EntryPoint = "c_plot3d")]
        private static extern void _plot3d([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                   [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                   PLFLT_MATRIX z,
                                                   PLINT nx, PLINT ny, Mesh opt,
                                                   [MarshalAs(UnmanagedType.Bool)] PLBOOL side);

        /// <summary>plot3d: Plot 3-d surface plot</summary>
        /// <param name="opt">Determines the way in which the surface is represented: opt=DRAW_LINEX : Lines are drawn showing z as a function of x for each value of y[j] . opt=DRAW_LINEY : Lines are drawn showing z as a function of y for each value of x[i] . opt=DRAW_LINEXY : Network of lines is drawn connecting points at which function is defined.</param>
        /// <param name="side">Flag to indicate whether or not ``sides'' should be draw on the figure. If side is true sides are drawn, otherwise no sides are drawn.</param>
        /// <param name="x">A vector containing the x coordinates at which the function is evaluated.</param>
        /// <param name="y">A vector containing the y coordinates at which the function is evaluated.</param>
        /// <param name="z">A matrix containing function values to plot. Should have dimensions of nx by ny.</param>
        /// <remarks>Plots a three-dimensional surface plot within the environment set up by plw3d. The surface is defined by the matrix z[nx][ny] , the point z[i][j] being the value of the function at (x[i],y[j]). Note that the points in vectors x and y do not need to be equally spaced, but must be stored in ascending order. The parameter opt controls the way in which the surface is displayed. For further details see . The only difference between plmesh and plot3d is that plmesh draws the bottom side of the surface, while plot3d only draws the surface as viewed from the top.</remarks>
        public static void plot3d(PLFLT[] x, PLFLT[] y, PLFLT[,] z, Mesh opt, PLBOOL side)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                _plot3d(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt, side);
            }
        }

        /// <summary>Plots a 3-d representation of the function z[x][y] with contour.</summary>
        [DllImport(DllName, EntryPoint = "c_plot3dc")]
        private static extern void _plot3dc([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                    PLFLT_MATRIX z,
                                                    PLINT nx, PLINT ny, MeshContour opt,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel);

        /// <summary>plot3dc: Magnitude colored plot surface with contour</summary>
        /// <param name="clevel">A vector containing the contour levels.</param>
        /// <param name="opt">Determines the way in which the surface is represented. To specify more than one option just add the options, e.g. DRAW_LINEXY + MAG_COLOR opt=DRAW_LINEX : Lines are drawn showing z as a function of x for each value of y[j] . opt=DRAW_LINEY : Lines are drawn showing z as a function of y for each value of x[i] . opt=DRAW_LINEXY : Network of lines is drawn connecting points at which function is defined. opt=MAG_COLOR : Each line in the mesh is colored according to the z value being plotted. The color is used from the current cmap1. opt=BASE_CONT : A contour plot is drawn at the base XY plane using parameters nlevel and clevel. opt=DRAW_SIDES : draws a curtain between the base XY plane and the borders of the plotted function.</param>
        /// <param name="x">A vector containing the x coordinates at which the function is evaluated.</param>
        /// <param name="y">A vector containing the y coordinates at which the function is evaluated.</param>
        /// <param name="z">A matrix containing function values to plot. Should have dimensions of nx by ny.</param>
        /// <remarks>Aside from dropping the side functionality this is a more powerful form of plot3d: the surface mesh can be colored accordingly to the current z value being plotted, a contour plot can be drawn at the base XY plane, and a curtain can be drawn between the plotted function border and the base XY plane. The arguments are identical to those of plmeshc. The only difference between plmeshc and plot3dc is that plmeshc draws the bottom side of the surface, while plot3dc only draws the surface as viewed from the top.</remarks>
        public static void plot3dc(PLFLT[] x, PLFLT[] y, PLFLT[,] z, MeshContour opt, PLFLT[] clevel)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                _plot3dc(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt, clevel, clevel.Length);
            }
        }

        /// <summary>Plots a 3-d representation of the function z[x][y] with contour and y index limits.</summary>
        [DllImport(DllName, EntryPoint = "c_plot3dcl")]
        private static extern void _plot3dcl([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                     PLFLT_MATRIX z,
                                                     PLINT nx, PLINT ny, MeshContour opt,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel,
                                                     PLINT indexxmin, PLINT indexxmax,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] indexymin,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] indexymax);

        /// <summary>plot3dcl: Magnitude colored plot surface with contour for z[x][y] with y index limits</summary>
        /// <param name="clevel">A vector containing the contour levels.</param>
        /// <param name="indexxmin">The index value (which must be ≥ 0) that corresponds to the first x index where z is defined.</param>
        /// <param name="indexymax">A vector containing y index values which all must be ≤ ny. These values correspond (by convention) to one more than the last y index where z is defined for a particular x index in the range from indexxmin to indexxmax - 1. The dimension of indexymax is indexxmax.</param>
        /// <param name="indexymin">A vector containing y index values which all must be ≥ 0. These values are the first y index where z is defined for a particular x index in the range from indexxmin to indexxmax - 1. The dimension of indexymin is indexxmax.</param>
        /// <param name="opt">Determines the way in which the surface is represented. To specify more than one option just add the options, e.g. DRAW_LINEXY + MAG_COLOR opt=DRAW_LINEX : Lines are drawn showing z as a function of x for each value of y[j] . opt=DRAW_LINEY : Lines are drawn showing z as a function of y for each value of x[i] . opt=DRAW_LINEXY : Network of lines is drawn connecting points at which function is defined. opt=MAG_COLOR : Each line in the mesh is colored according to the z value being plotted. The color is used from the current cmap1. opt=BASE_CONT : A contour plot is drawn at the base XY plane using parameters nlevel and clevel. opt=DRAW_SIDES : draws a curtain between the base XY plane and the borders of the plotted function.</param>
        /// <param name="x">A vector containing the x coordinates at which the function is evaluated.</param>
        /// <param name="y">A vector containing the y coordinates at which the function is evaluated.</param>
        /// <param name="z">A matrix containing function values to plot. Should have dimensions of nx by ny.</param>
        /// <remarks>When the implementation is completed this variant of plot3dc (see that function's documentation for more details) should be suitable for the case where the area of the x, y coordinate grid where z is defined can be non-rectangular. The implementation is incomplete so the last 4 parameters of plot3dcl; indexxmin, indexxmax, indexymin, and indexymax; are currently ignored and the functionality is otherwise identical to that of plot3dc.</remarks>
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

        /// <summary>Set fill pattern directly.</summary>
        [DllImport(DllName, EntryPoint = "c_plpat")]
        private static extern void _pat(PLINT nlin,
                                                [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] inc,
                                                [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] del);

        /// <summary>plpat: Set area line fill pattern</summary>
        /// <param name="del">A vector containing nlin values of the spacing in micrometers between the lines making up the pattern.</param>
        /// <param name="inc">A vector containing nlin values of the inclination in tenths of a degree. (Should be between -900 and 900).</param>
        /// <remarks>Sets the area line fill pattern to be used, e.g., for calls to plfill. The pattern consists of 1 or 2 sets of parallel lines with specified inclinations and spacings. The arguments to this routine are the number of sets to use (1 or 2) followed by two vectors (with 1 or 2 elements) specifying the inclinations in tenths of a degree and the spacing in micrometers. (See also plpsty)</remarks>
        public static void pat(PLINT[] inc, PLINT[] del)
        {
            PLINT nlin = GetSize(inc, del);
            if (nlin != 1 && nlin != 2)
                throw new ArgumentException("inc and del must be of length 1 or 2");
            _pat(nlin, inc, del);
        }

        /// <summary>plpath: Draw a line between two points, accounting for coordinate transforms</summary>
        /// <param name="n">number of points to use to approximate the path.</param>
        /// <param name="x1">x coordinate of first point.</param>
        /// <param name="x2">x coordinate of second point.</param>
        /// <param name="y1">y coordinate of first point.</param>
        /// <param name="y2">y coordinate of second point.</param>
        /// <remarks>Joins the point  (x1, y1)  to  (x2, y2) . If a global coordinate transform is defined then the line is broken in to n segments to approximate the path. If no transform is defined then this simply acts like a call to pljoin.</remarks>
        [DllImport(DllName, EntryPoint = "c_plpath")]
        public static extern void path(PLINT n, PLFLT x1, PLFLT y1, PLFLT x2, PLFLT y2);

        /// <summary>Plots array y against x for n points using ASCII code "code".</summary>
        [DllImport(DllName, EntryPoint = "c_plpoin")]
        private static extern void _poin(PLINT n,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                 PLINT code);

        /// <summary>plpoin: Plot a glyph at the specified points</summary>
        /// <param name="code">Hershey symbol code (in "ascii-indexed" form with -1 lt= code lt= 127) corresponding to a glyph to be plotted at each of the n points.</param>
        /// <param name="x">A vector containing the x coordinates of points.</param>
        /// <param name="y">A vector containing the y coordinates of points.</param>
        /// <remarks>Plot a glyph at the specified points. (This function is largely superseded by plstring which gives access to many[!] more glyphs.) code=-1  means try to just draw a point. Right now it's just a move and a draw at the same place. Not ideal, since a sufficiently intelligent output device may optimize it away, or there may be faster ways of doing it. This is OK for now, though, and offers a 4X speedup over drawing a Hershey font "point" (which is actually diamond shaped and therefore takes 4 strokes to draw). If 0 lt code lt 32, then a useful (but small subset) of Hershey symbols is plotted. If 32 lt= code lt= 127 the corresponding printable ASCII character is plotted.</remarks>
        public static void poin(PLFLT[] x, PLFLT[] y, PLINT code)
        {
            _poin(GetSize(x, y), x, y, code);
        }

        /// <summary>Draws a series of points in 3 space.</summary>
        [DllImport(DllName, EntryPoint = "c_plpoin3")]
        private static extern void _poin3(PLINT n,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z,
                                                  PLINT code);

        /// <summary>plpoin3: Plot a glyph at the specified 3D points</summary>
        /// <param name="code">Hershey symbol code (in "ascii-indexed" form with -1 lt= code lt= 127) corresponding to a glyph to be plotted at each of the n points.</param>
        /// <param name="x">A vector containing the x coordinates of points.</param>
        /// <param name="y">A vector containing the y coordinates of points.</param>
        /// <param name="z">A vector containing the z coordinates of points.</param>
        /// <remarks>Plot a glyph at the specified 3D points. (This function is largely superseded by plstring3 which gives access to many[!] more glyphs.) Set up the call to this function similar to what is done for plline3. code=-1  means try to just draw a point. Right now it's just a move and a draw at the same place. Not ideal, since a sufficiently intelligent output device may optimize it away, or there may be faster ways of doing it. This is OK for now, though, and offers a 4X speedup over drawing a Hershey font "point" (which is actually diamond shaped and therefore takes 4 strokes to draw). If 0 lt code lt 32, then a useful (but small subset) of Hershey symbols is plotted. If 32 lt= code lt= 127 the corresponding printable ASCII character is plotted.</remarks>
        public static void poin3(PLFLT[] x, PLFLT[] y, PLFLT[] z, PLINT code)
        {
            _poin3(GetSize(x, y, z), x, y, z, code);
        }

        /// <summary>Draws a polygon in 3 space.</summary>
        [DllImport(DllName, EntryPoint = "c_plpoly3")]
        private static extern void _poly3(PLINT n,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z,
                                                  [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Bool)] PLBOOL[] draw,
                                                  [MarshalAs(UnmanagedType.Bool)] PLBOOL ifcc);

        /// <summary>plpoly3: Draw a polygon in 3 space</summary>
        /// <param name="draw">A vector containing n-1 Boolean values which control drawing the segments of the polygon. If draw[i] is true, then the polygon segment from index [i] to [i+1] is drawn, otherwise, not.</param>
        /// <param name="ifcc">If ifcc is true the directionality of the polygon is determined by assuming the points are laid out in a counter-clockwise order. Otherwise, the directionality of the polygon is determined by assuming the points are laid out in a clockwise order.</param>
        /// <param name="x">A vector containing n x coordinates of points.</param>
        /// <param name="y">A vector containing n y coordinates of points.</param>
        /// <param name="z">A vector containing n z coordinates of points.</param>
        /// <remarks>Draws a polygon in 3 space defined by n points in x, y, and z. Setup like plline3, but differs from that function in that plpoly3 attempts to determine if the polygon is viewable depending on the order of the points within the vector and the value of ifcc. If the back of polygon is facing the viewer, then it isn't drawn. If this isn't what you want, then use plline3 instead.</remarks>
        public static void poly3(PLFLT[] x, PLFLT[] y, PLFLT[] z, PLBOOL[] draw, PLBOOL ifcc)
        {
            PLINT n = GetSize(x, y, z);
            if (draw.Length != n)
                throw new ArgumentException("draw must have same length as x, y and z");
            _poly3(n, x, y, z, draw, ifcc);
        }

        /// <summary>plprec: Set precision in numeric labels</summary>
        /// <param name="prec">The number of characters to draw after the decimal point in numeric labels.</param>
        /// <param name="setp">If setp is equal to 0 then PLplot automatically determines the number of places to use after the decimal point in numeric labels (like those used to label axes). If setp is 1 then prec sets the number of places.</param>
        /// <remarks>Sets the number of places after the decimal point in numeric labels.</remarks>
        [DllImport(DllName, EntryPoint = "c_plprec")]
        public static extern void prec(PLINT setp, PLINT prec);

        /// <summary>plpsty: Select area fill pattern</summary>
        /// <param name="patt">The desired pattern index. If patt is zero or less, then a solid fill is (normally, see qualifiers above) used. For patt in the range from 1 to 8 and assuming the driver has not supplied line fill capability itself (most deliberately do not so that line fill patterns look identical for those drivers), the patterns consist of (1) horizontal lines, (2) vertical lines, (3) lines at 45 degrees, (4) lines at -45 degrees, (5) lines at 30 degrees, (6) lines at -30 degrees, (7) both vertical and horizontal lines, and (8) lines at both 45 degrees and -45 degrees.</param>
        /// <remarks>If patt is zero or less use either a hardware solid fill if the drivers have that capability (virtually all do) or fall back to a software emulation of a solid fill using the eighth area line fill pattern. If 0 lt patt le 8, then select one of eight predefined area line fill patterns to use (see plpat if you desire other patterns).</remarks>
        [DllImport(DllName, EntryPoint = "c_plpsty")]
        public static extern void psty(Pattern patt);

        /// <summary>plptex: Write text inside the viewport</summary>
        /// <param name="dx">Together with dy, this specifies the inclination of the string. The baseline of the string is parallel to a line joining  (x, y)  to  (x+dx, y+dy) .</param>
        /// <param name="dy">Together with dx, this specifies the inclination of the string.</param>
        /// <param name="just">Specifies the position of the string relative to its reference point. If just=0. , the reference point is at the left and if just=1. , it is at the right of the string. Other values of just give intermediate justifications.</param>
        /// <param name="text">A UTF-8 character string to be written out.</param>
        /// <param name="x">x coordinate of reference point of string.</param>
        /// <param name="y">y coordinate of reference point of string.</param>
        /// <remarks>Writes text at a specified position and inclination within the viewport. Text is clipped at the viewport boundaries. The reference point of a string lies along a line passing through the string at half the height of a capital letter. The position of the reference point along this line is determined by just, the reference point is placed at world coordinates  (x, y)  within the viewport. The inclination of the string is specified in terms of differences of world coordinates making it easy to write text parallel to a line in a graph.</remarks>
        [DllImport(DllName, EntryPoint = "c_plptex")]
        public static extern void ptex(PLFLT x, PLFLT y, PLFLT dx, PLFLT dy, PLFLT just,
                                               [MarshalAs(UnmanagedType.LPStr)] string text);

        /// <summary>plptex3: Write text inside the viewport of a 3D plot</summary>
        /// <param name="dx">Together with dy and  dz , this specifies the inclination of the string. The baseline of the string is parallel to a line joining  (x, y, z)  to  (x+dx, y+dy, z+dz) .</param>
        /// <param name="dy">Together with dx and dz, this specifies the inclination of the string.</param>
        /// <param name="dz">Together with dx and dy, this specifies the inclination of the string.</param>
        /// <param name="just">Specifies the position of the string relative to its reference point. If just=0. , the reference point is at the left and if just=1. , it is at the right of the string. Other values of just give intermediate justifications.</param>
        /// <param name="sx">Together with sy and  sz , this specifies the shear of the string. The string is sheared so that the characters are vertically parallel to a line joining  (x, y, z)  to  (x+sx, y+sy, z+sz) . If sx = sy = sz = 0.)  then the text is not sheared.</param>
        /// <param name="sy">Together with sx and sz, this specifies shear of the string.</param>
        /// <param name="sz">Together with sx and sy, this specifies shear of the string.</param>
        /// <param name="text">A UTF-8 character string to be written out.</param>
        /// <param name="wx">x world coordinate of reference point of string.</param>
        /// <param name="wy">y world coordinate of reference point of string.</param>
        /// <param name="wz">z world coordinate of reference point of string.</param>
        /// <remarks>Writes text at a specified position and inclination and with a specified shear within the viewport. Text is clipped at the viewport boundaries. The reference point of a string lies along a line passing through the string at half the height of a capital letter. The position of the reference point along this line is determined by just, and the reference point is placed at world coordinates  (wx, wy, wz)  within the viewport. The inclination and shear of the string is specified in terms of differences of world coordinates making it easy to write text parallel to a line in a graph.</remarks>
        [DllImport(DllName, EntryPoint = "c_plptex3")]
        public static extern void ptex3(PLFLT wx, PLFLT wy, PLFLT wz, PLFLT dx, PLFLT dy, PLFLT dz,
                                                PLFLT sx, PLFLT sy, PLFLT sz, PLFLT just,
                                                [MarshalAs(UnmanagedType.LPStr)] string text);

        /// <summary>plrandd: Random number generator returning a real random number in the range [0,1]</summary>
        /// <remarks>Random number generator returning a real random number in the range [0,1]. The generator is based on the Mersenne Twister. Most languages / compilers provide their own random number generator, and so this is provided purely for convenience and to give a consistent random number generator across all languages supported by PLplot. This is particularly useful for comparing results from the test suite of examples.</remarks>
        [DllImport(DllName, EntryPoint = "c_plrandd")]
        public static extern PLFLT randd();

        /// <summary>plreplot: Replays contents of plot buffer to current device/file</summary>
        /// <remarks>Replays contents of plot buffer to current device/file.</remarks>
        [DllImport(DllName, EntryPoint = "c_plreplot")]
        public static extern void replot();

        /// <summary>plrgbhls: Convert RGB color to HLS</summary>
        /// <param name="b">Blue intensity (0.0-1.0) of the color.</param>
        /// <param name="g">Green intensity (0.0-1.0) of the color.</param>
        /// <param name="p_h">Returned value of the hue in degrees (0.0-360.0) on the color cylinder.</param>
        /// <param name="p_l">Returned value of the lightness expressed as a fraction (0.0-1.0) of the axis of the color cylinder.</param>
        /// <param name="p_s">Returned value of the saturation expressed as a fraction (0.0-1.0) of the radius of the color cylinder.</param>
        /// <param name="r">Red intensity (0.0-1.0) of the color.</param>
        /// <remarks>Convert RGB color coordinates to HLS</remarks>
        [DllImport(DllName, EntryPoint = "c_plrgbhls")]
        public static extern void rgbhls(PLFLT r, PLFLT g, PLFLT b, out PLFLT p_h, out PLFLT p_l, out PLFLT p_s);

        /// <summary>plschr: Set character size</summary>
        /// <param name="def">The default height of a character in millimeters, should be set to zero if the default height is to remain unchanged. For rasterized drivers the dx and dy values specified in plspage are used to convert from mm to pixels (note the different unit systems used). This dpi aware scaling is not implemented for all drivers yet.</param>
        /// <param name="scale">Scale factor to be applied to default to get actual character height.</param>
        /// <remarks>This sets up the size of all subsequent characters drawn. The actual height of a character is the product of the default character size and a scaling factor.</remarks>
        [DllImport(DllName, EntryPoint = "c_plschr")]
        public static extern void schr(PLFLT def, PLFLT scale);

        /// <summary>Set color map 0 colors by 8 bit RGB values</summary>
        [DllImport(DllName, EntryPoint = "c_plscmap0")]
        private static extern void _scmap0([In, MarshalAs(UnmanagedType.LPArray)] PLINT[] r,
                                                   [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] g,
                                                   [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] b,
                                                   PLINT ncol0);

        /// <summary>plscmap0: Set cmap0 colors by 8-bit RGB values</summary>
        /// <param name="b">A vector containing unsigned 8-bit integers (0-255) representing the degree of blue in the color.</param>
        /// <param name="g">A vector containing unsigned 8-bit integers (0-255) representing the degree of green in the color.</param>
        /// <param name="r">A vector containing unsigned 8-bit integers (0-255) representing the degree of red in the color.</param>
        /// <remarks>Set cmap0 colors using 8-bit RGB values (see ). This sets the entire color map ndash only as many colors as specified will be allocated.</remarks>
        public static void scmap0(PLINT[] r, PLINT[] g, PLINT[] b)
        {
            _scmap0(r, g, b, GetSize(r, g, b));
        }

        /// <summary>Set color map 0 colors by 8 bit RGB values and alpha values</summary>
        [DllImport(DllName, EntryPoint = "c_plscmap0a")]
        private static extern void _scmap0a([In, MarshalAs(UnmanagedType.LPArray)] PLINT[] r,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] g,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] b,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] alpha,
                                                    PLINT ncol0);

        /// <summary>plscmap0a: Set cmap0 colors by 8-bit RGB values and PLFLT alpha transparency value</summary>
        /// <param name="alpha">A vector containing values (0.0-1.0) representing the alpha transparency of the color.</param>
        /// <param name="b">A vector containing unsigned 8-bit integers (0-255) representing the degree of blue in the color.</param>
        /// <param name="g">A vector containing unsigned 8-bit integers (0-255) representing the degree of green in the color.</param>
        /// <param name="r">A vector containing unsigned 8-bit integers (0-255) representing the degree of red in the color.</param>
        /// <remarks>Set cmap0 colors using 8-bit RGB values (see ) and PLFLT alpha transparency value. This sets the entire color map ndash only as many colors as specified will be allocated.</remarks>
        public static void scmap0a(PLINT[] r, PLINT[] g, PLINT[] b, PLFLT[] alpha)
        {
            _scmap0a(r, g, b, alpha, GetSize(r, g, b, alpha));
        }

        /// <summary>plscmap0n: Set number of colors in cmap0</summary>
        /// <param name="ncol0">Number of colors that will be allocated in the cmap0 palette. If this number is zero or less, then the value from the previous call to plscmap0n is used and if there is no previous call, then a default value is used.</param>
        /// <remarks>Set number of colors in cmap0 (see ). Allocate (or reallocate) cmap0, and fill with default values for those colors not previously allocated. The first 16 default colors are given in the plcol0 documentation. For larger indices the default color is red.</remarks>
        [DllImport(DllName, EntryPoint = "c_plscmap0n")]
        public static extern void scmap0n(PLINT ncol0);

        /// <summary>Set color map 1 colors by 8 bit RGB values</summary>
        [DllImport(DllName, EntryPoint = "c_plscmap1")]
        private static extern void _scmap1([In, MarshalAs(UnmanagedType.LPArray)] PLINT[] r,
                                                   [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] g,
                                                   [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] b,
                                                   PLINT ncol1);

        /// <summary>plscmap1: Set opaque RGB cmap1 colors values</summary>
        /// <param name="b">A vector that represents (using unsigned 8-bit integers in the range from 0-255) the degree of blue in the color as a continuous function of the integer index of the vector.</param>
        /// <param name="g">A vector that represents (using unsigned 8-bit integers in the range from 0-255) the degree of green in the color as a continuous function of the integer index of the vector.</param>
        /// <param name="r">A vector that represents (using unsigned 8-bit integers in the range from 0-255) the degree of red in the color as a continuous function of the integer index of the vector.</param>
        /// <remarks>Set opaque cmap1 colors (see ) using RGB vector values. This function also sets the number of cmap1 colors. N.B. Continuous cmap1 colors are indexed with a floating-point index in the range from 0.0-1.0 which is linearly transformed (e.g., by plcol1) to an integer index of these RGB vectors in the range from 0 to ncol1-1. So in order for this continuous color model to work properly, it is the responsibility of the user of plscmap1 to insure that these RGB vectors are continuous functions of their integer indices.</remarks>
        public static void scmap1(PLINT[] r, PLINT[] g, PLINT[] b)
        {
            _scmap1(r, g, b, GetSize(r, g, b));
        }

        /// <summary>Set color map 1 colors by 8 bit RGB and alpha values</summary>
        [DllImport(DllName, EntryPoint = "c_plscmap1a")]
        private static extern void _scmap1a([In, MarshalAs(UnmanagedType.LPArray)] PLINT[] r,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] g,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] b,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] alpha,
                                                    PLINT ncol0);

        /// <summary>plscmap1a: Set semitransparent cmap1 RGBA colors.</summary>
        /// <param name="alpha">A vector that represents (using PLFLT values in the range from 0.0-1.0 where 0.0 corresponds to completely transparent and 1.0 corresponds to completely opaque) the alpha transparency of the color as a continuous function of the integer index of the vector.</param>
        /// <param name="b">A vector that represents (using unsigned 8-bit integers in the range from 0-255) the degree of blue in the color as a continuous function of the integer index of the vector.</param>
        /// <param name="g">A vector that represents (using unsigned 8-bit integers in the range from 0-255) the degree of green in the color as a continuous function of the integer index of the vector.</param>
        /// <param name="r">A vector that represents (using unsigned 8-bit integers in the range from 0-255) the degree of red in the color as a continuous function of the integer index of the vector.</param>
        /// <remarks>Set semitransparent cmap1 colors (see ) using RGBA vector values. This function also sets the number of cmap1 colors. N.B. Continuous cmap1 colors are indexed with a floating-point index in the range from 0.0-1.0 which is linearly transformed (e.g., by plcol1) to an integer index of these RGBA vectors in the range from 0 to ncol1-1. So in order for this continuous color model to work properly, it is the responsibility of the user of plscmap1 to insure that these RGBA vectors are continuous functions of their integer indices.</remarks>
        public static void scmap1a(PLINT[] r, PLINT[] g, PLINT[] b, PLFLT[] alpha)
        {
            _scmap1a(r, g, b, alpha, GetSize(r, g, b, alpha));
        }

        /// <summary>Set color map 1 colors using a piece-wise linear relationship between intensity [0,1] (cmap 1 index) and position in HLS or RGB color space.</summary>
        [DllImport(DllName, EntryPoint = "c_plscmap1l")]
        private static extern void _scmap1l(ColorSpace itype, PLINT npts,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] intensity,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord1,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord2,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord3,
                                                    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Bool)] PLBOOL[] alt_hue_path);

        /// <summary>plscmap1l: Set cmap1 colors using a piece-wise linear relationship</summary>
        /// <param name="alt_hue_path">A vector (with npts - 1 elements) containing the alternative interpolation method Boolean value for each control point interval. (alt_hue_path[i] refers to the interpolation interval between the i and i + 1 control points).</param>
        /// <param name="coord1">A vector containing the first coordinate (H or R) for each control point.</param>
        /// <param name="coord2">A vector containing the second coordinate (L or G) for each control point.</param>
        /// <param name="coord3">A vector containing the third coordinate (S or B) for each control point.</param>
        /// <param name="intensity">A vector containing the cmap1 intensity index (0.0-1.0) in ascending order for each control point.</param>
        /// <param name="itype">true: RGB, false: HLS.</param>
        /// <remarks>Set cmap1 colors using a piece-wise linear relationship between the cmap1 intensity index (0.0-1.0) and position in HLS or RGB color space (see ). May be called at any time.</remarks>
        public static void scmap1l(ColorSpace itype, PLFLT[] intensity, PLFLT[] coord1, PLFLT[] coord2, PLFLT[] coord3,
                                           PLBOOL[] alt_hue_path)
        {
            PLINT npts = GetSize(intensity, coord1, coord2, coord3);
            if (alt_hue_path.Length != npts - 1)
                throw new ArgumentException("alt_hue_path must have one element less than intensity");
            _scmap1l(itype, npts, intensity, coord1, coord2, coord3, alt_hue_path);
        }

        /// <summary>Set color map 1 colors using a piece-wise linear relationship between intensity [0,1] (cmap 1 index) and position in HLS or RGB color space. Will also linear interpolate alpha values.</summary>
        [DllImport(DllName, EntryPoint = "c_plscmap1la")]
        private static extern void _scmap1la(ColorSpace itype, PLINT npts,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] intensity,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord1,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord2,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] coord3,
                                                     [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] alpha,
                                                     [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Bool)] PLBOOL[] alt_hue_path);

        /// <summary>plscmap1la: Set cmap1 colors and alpha transparency using a piece-wise linear relationship</summary>
        /// <param name="alpha">A vector containing the alpha transparency value (0.0-1.0) for each control point.</param>
        /// <param name="alt_hue_path">A vector (with npts - 1 elements) containing the alternative interpolation method Boolean value for each control point interval. (alt_hue_path[i] refers to the interpolation interval between the i and i + 1 control points).</param>
        /// <param name="coord1">A vector containing the first coordinate (H or R) for each control point.</param>
        /// <param name="coord2">A vector containing the second coordinate (L or G) for each control point.</param>
        /// <param name="coord3">A vector containing the third coordinate (S or B) for each control point.</param>
        /// <param name="intensity">A vector containing the cmap1 intensity index (0.0-1.0) in ascending order for each control point.</param>
        /// <param name="itype">true: RGB, false: HLS.</param>
        /// <remarks>This is a variant of plscmap1l that supports alpha channel transparency. It sets cmap1 colors using a piece-wise linear relationship between cmap1 intensity index (0.0-1.0) and position in HLS or RGB color space (see ) with alpha transparency value (0.0-1.0). It may be called at any time.</remarks>
        public static void scmap1la(ColorSpace itype, PLFLT[] intensity, PLFLT[] coord1, PLFLT[] coord2, PLFLT[] coord3, PLFLT[] alpha,
                                            PLBOOL[] alt_hue_path)
        {
            PLINT npts = GetSize(intensity, coord1, coord2, coord3, alpha);
            if (alt_hue_path.Length != npts - 1)
                throw new ArgumentException("alt_hue_path must have one element less than intensity");
            _scmap1la(itype, npts, intensity, coord1, coord2, coord3, alpha, alt_hue_path);
        }

        /// <summary>plscmap1n: Set number of colors in cmap1</summary>
        /// <param name="ncol1">Number of colors that will be allocated in the cmap1 palette. If this number is zero or less, then the value from the previous call to plscmap1n is used and if there is no previous call, then a default value is used.</param>
        /// <remarks>Set number of colors in cmap1, (re-)allocate cmap1, and set default values if this is the first allocation (see ).</remarks>
        [DllImport(DllName, EntryPoint = "c_plscmap1n")]
        public static extern void scmap1n(PLINT ncol1);

        /// <summary>plscmap1_range: Set the cmap1 argument range for continuous color plots</summary>
        /// <param name="max_color">The maximum cmap1 argument. If greater than 1.0, then 1.0 is used instead.</param>
        /// <param name="min_color">The minimum cmap1 argument. If less than 0.0, then 0.0 is used instead.</param>
        /// <remarks>Set the cmap1 argument range for continuous color plots that corresponds to the range of data values. The maximum range corresponding to the entire cmap1 palette is 0.0-1.0, and the smaller the cmap1 argument range that is specified with this routine, the smaller the subset of the cmap1 color palette that is used to represent the continuous data being plotted. If min_color is greater than max_color or max_color is greater than 1.0 or min_color is less than 0.0 then no change is made to the cmap1 argument range. (Use plgcmap1_range to get the cmap1 argument range.)</remarks>
        [DllImport(DllName, EntryPoint = "c_plscmap1_range")]
        public static extern void scmap1_range(PLFLT min_color, PLFLT max_color);

        /// <summary>plgcmap1_range: Get the cmap1 argument range for continuous color plots</summary>
        /// <param name="max_color">Returned value of the current maximum cmap1 argument.</param>
        /// <param name="min_color">Returned value of the current minimum cmap1 argument.</param>
        /// <remarks>Get the cmap1 argument range for continuous color plots. (Use plscmap1_range to set the cmap1 argument range.)</remarks>
        [DllImport(DllName, EntryPoint = "c_plgcmap1_range")]
        public static extern void gcmap1_range(out PLFLT min_color, out PLFLT max_color);

        /// <summary>plscol0: Set 8-bit RGB values for given cmap0 color index</summary>
        /// <param name="b">Unsigned 8-bit integer (0-255) representing the degree of blue in the color.</param>
        /// <param name="g">Unsigned 8-bit integer (0-255) representing the degree of green in the color.</param>
        /// <param name="icol0">Color index. Must be less than the maximum number of colors (which is set by default, by plscmap0n, or even by plscmap0).</param>
        /// <param name="r">Unsigned 8-bit integer (0-255) representing the degree of red in the color.</param>
        /// <remarks>Set 8-bit RGB values for given cmap0 (see ) index. Overwrites the previous color value for the given index and, thus, does not result in any additional allocation of space for colors.</remarks>
        [DllImport(DllName, EntryPoint = "c_plscol0")]
        public static extern void scol0(PLINT icol0, PLINT r, PLINT g, PLINT b);

        /// <summary>plscol0a: Set 8-bit RGB values and PLFLT alpha transparency value for given cmap0 color index</summary>
        /// <param name="alpha">Value of the alpha transparency in the range (0.0-1.0).</param>
        /// <param name="b">Unsigned 8-bit integer (0-255) representing the degree of blue in the color.</param>
        /// <param name="g">Unsigned 8-bit integer (0-255) representing the degree of green in the color.</param>
        /// <param name="icol0">Color index. Must be less than the maximum number of colors (which is set by default, by plscmap0n, or even by plscmap0).</param>
        /// <param name="r">Unsigned 8-bit integer (0-255) representing the degree of red in the color.</param>
        /// <remarks>Set 8-bit RGB value and PLFLT alpha transparency value for given cmap0 (see ) index. Overwrites the previous color value for the given index and, thus, does not result in any additional allocation of space for colors.</remarks>
        [DllImport(DllName, EntryPoint = "c_plscol0a")]
        public static extern void scol0a(PLINT icol0, PLINT r, PLINT g, PLINT b, PLFLT alpha);

        /// <summary>plscolbg: Set the background color by 8-bit RGB value</summary>
        /// <param name="b">Unsigned 8-bit integer (0-255) representing the degree of blue in the color.</param>
        /// <param name="g">Unsigned 8-bit integer (0-255) representing the degree of green in the color.</param>
        /// <param name="r">Unsigned 8-bit integer (0-255) representing the degree of red in the color.</param>
        /// <remarks>Set the background color (color 0 in cmap0) by 8-bit RGB value (see ).</remarks>
        [DllImport(DllName, EntryPoint = "c_plscolbg")]
        public static extern void scolbg(PLINT r, PLINT g, PLINT b);

        /// <summary>plscolbga: Set the background color by 8-bit RGB value and PLFLT alpha transparency value.</summary>
        /// <param name="alpha">Value of the alpha transparency in the range (0.0-1.0).</param>
        /// <param name="b">Unsigned 8-bit integer (0-255) representing the degree of blue in the color.</param>
        /// <param name="g">Unsigned 8-bit integer (0-255) representing the degree of green in the color.</param>
        /// <param name="r">Unsigned 8-bit integer (0-255) representing the degree of red in the color.</param>
        /// <remarks>Set the background color (color 0 in cmap0) by 8-bit RGB value and PLFLT alpha transparency value (see ).</remarks>
        [DllImport(DllName, EntryPoint = "c_plscolbga")]
        public static extern void scolbga(PLINT r, PLINT g, PLINT b, PLFLT alpha);

        /// <summary>plscolor: Used to globally turn color output on/off</summary>
        /// <param name="color">Color flag (Boolean). If zero, color is turned off. If non-zero, color is turned on.</param>
        /// <remarks>Used to globally turn color output on/off for those drivers/devices that support it.</remarks>
        [DllImport(DllName, EntryPoint = "c_plscolor")]
        public static extern void scolor(PLINT color);

        /// <summary>plscompression: Set device-compression level</summary>
        /// <param name="compression">The desired compression level. This is a device-dependent value. Currently only the jpeg and png devices use these values. For jpeg value is the jpeg quality which should normally be in the range 0-95. Higher values denote higher quality and hence larger image sizes. For png values are in the range -1 to 99. Values of 0-9 are taken as the compression level for zlib. A value of -1 denotes the default zlib compression level. Values in the range 10-99 are divided by 10 and then used as the zlib compression level. Higher compression levels correspond to greater compression and small file sizes at the expense of more computation.</param>
        /// <remarks>Set device-compression level. Only used for drivers that provide compression. This function, if used, should be invoked before a call to plinit.</remarks>
        [DllImport(DllName, EntryPoint = "c_plscompression")]
        public static extern void scompression(PLINT compression);

        /// <summary>plsdev: Set the device (keyword) name</summary>
        /// <param name="devname">An ascii character string containing the device name keyword of the required output device. If devname is NULL or if the first character of the string is a ``?'', the normal (prompted) start up is used.</param>
        /// <remarks>Set the device (keyword) name.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsdev")]
        public static extern void sdev([MarshalAs(UnmanagedType.LPStr)] string devname);

        /// <summary>plsdidev: Set parameters that define current device-space window</summary>
        /// <param name="aspect">Aspect ratio.</param>
        /// <param name="jx">Relative justification in x. Value must lie in the range -0.5 to 0.5.</param>
        /// <param name="jy">Relative justification in y. Value must lie in the range -0.5 to 0.5.</param>
        /// <param name="mar">Relative margin width.</param>
        /// <remarks>Set relative margin width, aspect ratio, and relative justification that define current device-space window. If you want to just use the previous value for any of these, just pass in the magic value PL_NOTSET. It is unlikely that one should ever need to change the aspect ratio but it's in there for completeness. If plsdidev is not called the default values of mar, jx, and jy are all 0. aspect is set to a device-specific value.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsdidev")]
        public static extern void sdidev(PLFLT mar, PLFLT aspect, PLFLT jx, PLFLT jy);

        /// <summary>plsdimap: Set up transformation from metafile coordinates</summary>
        /// <param name="dimxmax">NEEDS DOCUMENTATION</param>
        /// <param name="dimxmin">NEEDS DOCUMENTATION</param>
        /// <param name="dimxpmm">NEEDS DOCUMENTATION</param>
        /// <param name="dimymax">NEEDS DOCUMENTATION</param>
        /// <param name="dimymin">NEEDS DOCUMENTATION</param>
        /// <param name="dimypmm">NEEDS DOCUMENTATION</param>
        /// <remarks>Set up transformation from metafile coordinates. The size of the plot is scaled so as to preserve aspect ratio. This isn't intended to be a general-purpose facility just yet (not sure why the user would need it, for one).</remarks>
        [DllImport(DllName, EntryPoint = "c_plsdimap")]
        public static extern void sdimap(PLINT dimxmin, PLINT dimxmax, PLINT dimymin, PLINT dimymax,
                                                 PLFLT dimxpmm, PLFLT dimypmm);

        /// <summary>plsdiori: Set plot orientation</summary>
        /// <param name="rot">Plot orientation parameter.</param>
        /// <remarks>Set plot orientation parameter which is multiplied by 90deg to obtain the angle of rotation. Note, arbitrary rotation parameters such as 0.2 (corresponding to 18deg) are possible, but the usual values for the rotation parameter are 0., 1., 2., and 3. corresponding to 0deg (landscape mode), 90deg (portrait mode), 180deg (seascape mode), and 270deg (upside-down mode). If plsdiori is not called the default value of rot is 0.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsdiori")]
        public static extern void sdiori(PLFLT rot);

        /// <summary>plsdiplt: Set parameters that define current plot-space window</summary>
        /// <param name="xmax">Relative maximum in x.</param>
        /// <param name="xmin">Relative minimum in x.</param>
        /// <param name="ymax">Relative maximum in y.</param>
        /// <param name="ymin">Relative minimum in y.</param>
        /// <remarks>Set relative minima and maxima that define the current plot-space window. If plsdiplt is not called the default values of xmin, ymin, xmax, and ymax are 0., 0., 1., and 1.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsdiplt")]
        public static extern void sdiplt(PLFLT xmin, PLFLT ymin, PLFLT xmax, PLFLT ymax);

        /// <summary>plsdiplz: Set parameters incrementally (zoom mode) that define current plot-space window</summary>
        /// <param name="xmax">Relative (incremental) maximum in x.</param>
        /// <param name="xmin">Relative (incremental) minimum in x.</param>
        /// <param name="ymax">Relative (incremental) maximum in y.</param>
        /// <param name="ymin">Relative (incremental) minimum in y.</param>
        /// <remarks>Set relative minima and maxima incrementally (zoom mode) that define the current plot-space window. This function has the same effect as plsdiplt if that function has not been previously called. Otherwise, this function implements zoom mode using the transformation min_used = old_min + old_length*min  and max_used = old_min + old_length*max  for each axis. For example, if min = 0.05 and max = 0.95 for each axis, repeated calls to plsdiplz will zoom in by 10 per cent for each call.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsdiplz")]
        public static extern void sdiplz(PLFLT xmin, PLFLT ymin, PLFLT xmax, PLFLT ymax);

        /// <summary>plseed: Set seed for internal random number generator.</summary>
        /// <param name="seed">Seed for random number generator.</param>
        /// <remarks>Set the seed for the internal random number generator. See plrandd for further details.</remarks>
        [DllImport(DllName, EntryPoint = "c_plseed")]
        public static extern void seed(uint seed);

        /// <summary>plsesc: Set the escape character for text strings</summary>
        /// <param name="esc">Escape character.</param>
        /// <remarks>Set the escape character for text strings. From C (in contrast to Fortran, see plsescfortran) you pass esc as a character. Only selected characters are allowed to prevent the user from shooting himself in the foot (For example, a \ isn't allowed since it conflicts with C's use of backslash as a character escape). Here are the allowed escape characters and their corresponding decimal ASCII values: !, ASCII 33 #, ASCII 35 $, ASCII 36 %, ASCII 37 amp, ASCII 38 *, ASCII 42 @, ASCII 64 ^, ASCII 94 ~, ASCII 126</remarks>
        [DllImport(DllName, EntryPoint = "c_plsesc")]
        public static extern void sesc(char esc);

        /// <summary>plsfam: Set family file parameters</summary>
        /// <param name="bmax">Maximum file size (in bytes) for a family file.</param>
        /// <param name="fam">Family flag (Boolean). If nonzero, familying is enabled.</param>
        /// <param name="num">Current family file number.</param>
        /// <remarks>Sets variables dealing with output file familying. Does nothing if familying not supported by the driver. This routine, if used, must be called before initializing PLplot. See  for more information.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsfam")]
        public static extern void sfam(PLINT fam, PLINT num, PLINT bmax);

        /// <summary>plsfci: Set FCI (font characterization integer)</summary>
        /// <param name="fci">PLUNICODE (unsigned 32-bit integer) value of FCI.</param>
        /// <remarks>Sets font characteristics to be used at the start of the next string using the FCI approach. See  for more information. Note, plsfont (which calls plsfci internally) provides a more user-friendly API for setting the font characterisitics.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsfci")]
        public static extern void sfci(FCI fci);

        /// <summary>plsfnam: Set output file name</summary>
        /// <param name="fnam">An ascii character string containing the file name.</param>
        /// <remarks>Sets the current output file name, if applicable. If the file name has not been specified and is required by the driver, the user will be prompted for it. If using the X-windows output driver, this sets the display name. This routine, if used, must be called before initializing PLplot.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsfnam")]
        public static extern void sfnam([MarshalAs(UnmanagedType.LPStr)] string fnam);

        /// <summary>plsfont: Set family, style and weight of the current font</summary>
        /// <param name="family">Font family to select for the current font. The available values are given by the PL_FCI_* constants in plplot.h. Current options are PL_FCI_SANS, PL_FCI_SERIF, PL_FCI_MONO, PL_FCI_SCRIPT and PL_FCI_SYMBOL. A negative value signifies that the font family should not be altered.</param>
        /// <param name="style">Font style to select for the current font. The available values are given by the PL_FCI_* constants in plplot.h. Current options are PL_FCI_UPRIGHT, PL_FCI_ITALIC and PL_FCI_OBLIQUE. A negative value signifies that the font style should not be altered.</param>
        /// <param name="weight">Font weight to select for the current font. The available values are given by the PL_FCI_* constants in plplot.h. Current options are PL_FCI_MEDIUM and PL_FCI_BOLD. A negative value signifies that the font weight should not be altered.</param>
        /// <remarks>Sets the current font. See  for more information on font selection.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsfont")]
        public static extern void sfont(FontFamily family, FontStyle style, FontWeight weight);

        /// <summary>Shade region.</summary>
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

        /// <summary>plshade: Shade individual region on the basis of value</summary>
        /// <param name="a">A matrix containing function values to plot. Should have dimensions of nx by ny.</param>
        /// <param name="defined">Callback function specifying the region that should be plotted in the shade plot. This function accepts x and y coordinates as input arguments and must return 1 if the point is to be included in the shade plot and 0 otherwise. If you want to plot the entire shade plot (the usual case), this argument should be set to NULL.</param>
        /// <param name="fill">Routine used to fill the region. Use plfill. Future version of PLplot may have other fill routines.</param>
        /// <param name="max_color">Defines pen color, width used by the boundary of shaded region. The min values are used for the shade_min boundary, and the max values are used on the shade_max boundary. Set color and width to zero for no plotted boundaries.</param>
        /// <param name="max_width">Defines pen color, width used by the boundary of shaded region. The min values are used for the shade_min boundary, and the max values are used on the shade_max boundary. Set color and width to zero for no plotted boundaries.</param>
        /// <param name="min_color">Defines pen color, width used by the boundary of shaded region. The min values are used for the shade_min boundary, and the max values are used on the shade_max boundary. Set color and width to zero for no plotted boundaries.</param>
        /// <param name="min_width">Defines pen color, width used by the boundary of shaded region. The min values are used for the shade_min boundary, and the max values are used on the shade_max boundary. Set color and width to zero for no plotted boundaries.</param>
        /// <param name="pltr">A callback function that defines the transformation between the zero-based indices of the matrix a and world coordinates. If pltr is not supplied (e.g., is set to NULL in the C case), then the x indices of a are mapped to the range xmin through xmax and the y indices of a are mapped to the range ymin through ymax.For the C case, transformation functions are provided in the PLplot library: pltr0 for the identity mapping, and pltr1 and pltr2 for arbitrary mappings respectively defined by vectors and matrices. In addition, C callback routines for the transformation can be supplied by the user such as the mypltr function in examples/c/x09c.c which provides a general linear transformation between index coordinates and world coordinates.For languages other than C you should consult  for the details concerning how PLTRANSFORM_callback arguments are interfaced. However, in general, a particular pattern of callback-associated arguments such as a tr vector with 6 elements; xg and yg vectors; or xg and yg matrices are respectively interfaced to a linear-transformation routine similar to the above mypltr function; pltr1; and pltr2. Furthermore, some of our more sophisticated bindings (see, e.g., ) support native language callbacks for handling index to world-coordinate transformations. Examples of these various approaches are given in examples/ltlanguagegtx09*, examples/ltlanguagegtx16*, examples/ltlanguagegtx20*, examples/ltlanguagegtx21*, and examples/ltlanguagegtx22*, for all our supported languages.</param>
        /// <param name="pltr_data">Extra parameter to help pass information to pltr0, pltr1, pltr2, or whatever routine that is externally supplied.</param>
        /// <param name="rectangular">Set rectangular to true if rectangles map to rectangles after coordinate transformation with pltrl. Otherwise, set rectangular to false. If rectangular is set to true, plshade tries to save time by filling large rectangles. This optimization fails if the coordinate transformation distorts the shape of rectangles. For example a plot in polar coordinates has to have rectangular set to false.</param>
        /// <param name="sh_cmap">Defines color map. If sh_cmap=0, then sh_color is interpreted as a cmap0 (integer) index. If sh_cmap=1, then sh_color is interpreted as a cmap1 argument in the range (0.0-1.0).</param>
        /// <param name="sh_color">Defines color map index with integer value if cmap0 or value in range (0.0-1.0) if cmap1.</param>
        /// <param name="sh_width">Defines width used by the fill pattern.</param>
        /// <param name="shade_max">Defines the upper end of the interval to be shaded. If shade_max leq shade_min, plshade does nothing.</param>
        /// <param name="shade_min">Defines the lower end of the interval to be shaded. If shade_max leq shade_min, plshade does nothing.</param>
        /// <remarks>Shade individual region on the basis of value. Use plshades if you want to shade a number of contiguous regions using continuous colors. In particular the edge contours are treated properly in plshades. If you attempt to do contiguous regions with plshade the contours at the edge of the shade are partially obliterated by subsequent plots of contiguous shaded regions.</remarks>
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

        /// <summary>plshades: Shade regions on the basis of value</summary>
        /// <param name="a">A matrix containing function values to plot. Should have dimensions of nx by ny.</param>
        /// <param name="clevel">A vector containing the data levels corresponding to the edges of each shaded region that will be plotted by this function. To work properly the levels should be monotonic.</param>
        /// <param name="cont_color">Defines cmap0 pen color used for contours defining edges of shaded regions. The pen color is only temporary set for the contour drawing. Set this value to zero or less if no shade edge contours are wanted.</param>
        /// <param name="cont_width">Defines line width used for contours defining edges of shaded regions. This value may not be honored by all drivers. The pen width is only temporary set for the contour drawing. Set this value to zero or less if no shade edge contours are wanted.</param>
        /// <param name="defined">Callback function specifying the region that should be plotted in the shade plot. This function accepts x and y coordinates as input arguments and must return 1 if the point is to be included in the shade plot and 0 otherwise. If you want to plot the entire shade plot (the usual case), this argument should be set to NULL.</param>
        /// <param name="fill">Callback routine used to fill the region. Use plfill for this purpose.</param>
        /// <param name="fill_width">Defines the line width used by the fill pattern.</param>
        /// <param name="pltr">A callback function that defines the transformation between the zero-based indices of the matrix a and world coordinates. If pltr is not supplied (e.g., is set to NULL in the C case), then the x indices of a are mapped to the range xmin through xmax and the y indices of a are mapped to the range ymin through ymax.For the C case, transformation functions are provided in the PLplot library: pltr0 for the identity mapping, and pltr1 and pltr2 for arbitrary mappings respectively defined by vectors and matrices. In addition, C callback routines for the transformation can be supplied by the user such as the mypltr function in examples/c/x09c.c which provides a general linear transformation between index coordinates and world coordinates.For languages other than C you should consult  for the details concerning how PLTRANSFORM_callback arguments are interfaced. However, in general, a particular pattern of callback-associated arguments such as a tr vector with 6 elements; xg and yg vectors; or xg and yg matrices are respectively interfaced to a linear-transformation routine similar to the above mypltr function; pltr1; and pltr2. Furthermore, some of our more sophisticated bindings (see, e.g., ) support native language callbacks for handling index to world-coordinate transformations. Examples of these various approaches are given in examples/ltlanguagegtx09*, examples/ltlanguagegtx16*, examples/ltlanguagegtx20*, examples/ltlanguagegtx21*, and examples/ltlanguagegtx22*, for all our supported languages.</param>
        /// <param name="pltr_data">Extra parameter to help pass information to pltr0, pltr1, pltr2, or whatever routine that is externally supplied.</param>
        /// <param name="rectangular">Set rectangular to true if rectangles map to rectangles after coordinate transformation with pltrl. Otherwise, set rectangular to false. If rectangular is set to true, plshade tries to save time by filling large rectangles. This optimization fails if the coordinate transformation distorts the shape of rectangles. For example a plot in polar coordinates has to have rectangular set to false.</param>
        /// <remarks>Shade regions on the basis of value. This is the high-level routine for making continuous color shaded plots with cmap1 while plshade should be used to plot individual shaded regions using either cmap0 or cmap1. examples/;ltlanguagegt/x16* shows how to use plshades for each of our supported languages.</remarks>
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

        /// <summary>plslabelfunc: Assign a function to use for generating custom axis labels</summary>
        /// <param name="label_data">This parameter may be used to pass data to the label_func function.</param>
        /// <param name="label_func">This is the custom label function. In order to reset to the default labelling, set this to NULL. The labelling function parameters are, in order: axis This indicates which axis a label is being requested for. The value will be one of PL_X_AXIS, PL_Y_AXIS or PL_Z_AXIS. value This is the value along the axis which is being labelled. label_text The string representation of the label value. length The maximum length in characters allowed for label_text.</param>
        /// <remarks>This function allows a user to provide their own function to provide axis label text. The user function is given the numeric value for a point on an axis and returns a string label to correspond with that value. Custom axis labels can be enabled by passing appropriate arguments to plenv, plbox, plbox3 and similar functions.</remarks>
        [DllImport(DllName, EntryPoint = "c_plslabelfunc")]
        public static extern void slabelfunc(LabelFunc label_func, PLPointer label_data);

        /// <summary>plsmaj: Set length of major ticks</summary>
        /// <param name="def">The default length of a major tick in millimeters, should be set to zero if the default length is to remain unchanged.</param>
        /// <param name="scale">Scale factor to be applied to default to get actual tick length.</param>
        /// <remarks>This sets up the length of the major ticks. The actual length is the product of the default length and a scaling factor as for character height.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsmaj")]
        public static extern void smaj(PLFLT def, PLFLT scale);

        /// <summary>plsmem: Set the memory area to be plotted (RGB)</summary>
        /// <param name="maxx">Size of memory area in the X coordinate.</param>
        /// <param name="maxy">Size of memory area in the Y coordinate.</param>
        /// <param name="plotmem">Pointer to the beginning of a user-supplied writeable memory area.</param>
        /// <remarks>Set the memory area to be plotted (with the mem or memcairo driver) as the dev member of the stream structure. Also set the number of pixels in the memory passed in plotmem, which is a block of memory maxy by maxx by 3 bytes long, say: 480 x 640 x 3 (Y, X, RGB)</remarks>
        [DllImport(DllName, EntryPoint = "c_plsmem")]
        public static extern void smem(PLINT maxx, PLINT maxy, PLPointer plotmem);

        /// <summary>plsmema: Set the memory area to be plotted (RGBA)</summary>
        /// <param name="maxx">Size of memory area in the X coordinate.</param>
        /// <param name="maxy">Size of memory area in the Y coordinate.</param>
        /// <param name="plotmem">Pointer to the beginning of a user-supplied writeable memory area.</param>
        /// <remarks>Set the memory area to be plotted (with the memcairo driver) as the dev member of the stream structure. Also set the number of pixels in the memory passed in plotmem, which is a block of memory maxy by maxx by 4 bytes long, say: 480 x 640 x 4 (Y, X, RGBA)</remarks>
        [DllImport(DllName, EntryPoint = "c_plsmema")]
        public static extern void smema(PLINT maxx, PLINT maxy, PLPointer plotmem);

        /// <summary>plsmin: Set length of minor ticks</summary>
        /// <param name="def">The default length of a minor tick in millimeters, should be set to zero if the default length is to remain unchanged.</param>
        /// <param name="scale">Scale factor to be applied to default to get actual tick length.</param>
        /// <remarks>This sets up the length of the minor ticks and the length of the terminals on error bars. The actual length is the product of the default length and a scaling factor as for character height.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsmin")]
        public static extern void smin(PLFLT def, PLFLT scale);

        /// <summary>plsdrawmode: Set drawing mode (depends on device support!)</summary>
        /// <param name="mode">Control variable which species the drawing mode (one of PL_DRAWMODE_DEFAULT, PL_DRAWMODE_REPLACE, or PL_DRAWMODE_XOR) to use.</param>
        /// <remarks>Set drawing mode. Note only one device driver (cairo) currently supports this at the moment. See also plgdrawmode.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsdrawmode")]
        public static extern void sdrawmode(DrawMode mode);

        /// <summary>plsori: Set orientation</summary>
        /// <param name="ori">Orientation value (0 for landscape, 1 for portrait, etc.) The value is multiplied by 90 degrees to get the angle.</param>
        /// <remarks>Set integer plot orientation parameter. This function is identical to plsdiori except for the type of the argument, and should be used in the same way. See the documentation of plsdiori for details.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsori")]
        public static extern void sori(Orientation ori);

        /// <summary>plspage: Set page parameters</summary>
        /// <param name="xleng">Page length, x.</param>
        /// <param name="xoff">Page offset, x.</param>
        /// <param name="xp">Number of pixels per inch (DPI), x. Used only by raster drivers, ignored by drivers which use "real world" units (e.g. mm).</param>
        /// <param name="yleng">Page length, y.</param>
        /// <param name="yoff">Page offset, y.</param>
        /// <param name="yp">Number of pixels per inch (DPI), y. Used only by raster drivers, ignored by drivers which use "real world" units (e.g. mm).</param>
        /// <remarks>Sets the page configuration (optional). If an individual parameter is zero then that parameter value is not updated. Not all parameters are recognized by all drivers and the interpretation is device-dependent. The X-window driver uses the length and offset parameters to determine the window size and location. The length and offset values are expressed in units that are specific to the current driver. For instance: screen drivers will usually interpret them as number of pixels, whereas printer drivers will usually use mm.</remarks>
        [DllImport(DllName, EntryPoint = "c_plspage")]
        public static extern void spage(PLFLT xp, PLFLT yp, PLINT xleng, PLINT yleng,
                                                PLINT xoff, PLINT yoff);

        /// <summary>plspal0: Set the cmap0 palette using the specified cmap0*.pal format file</summary>
        /// <param name="filename">An ascii character string containing the name of the cmap0*.pal file. If this string is empty, use the default cmap0*.pal file.</param>
        /// <remarks>Set the cmap0 palette using the specified cmap0*.pal format file.</remarks>
        [DllImport(DllName, EntryPoint = "c_plspal0")]
        public static extern void spal0([MarshalAs(UnmanagedType.LPStr)] string filename);

        /// <summary>plspal1: Set the cmap1 palette using the specified cmap1*.pal format file</summary>
        /// <param name="filename">An ascii character string containing the name of the cmap1*.pal file. If this string is empty, use the default cmap1*.pal file.</param>
        /// <param name="interpolate">If this parameter is true, the columns containing the intensity index, r, g, b, alpha and alt_hue_path in the cmap1*.pal file are used to set the cmap1 palette with a call to plscmap1la. (The cmap1*.pal header contains a flag which controls whether the r, g, b data sent to plscmap1la are interpreted as HLS or RGB.)  If this parameter is false, the intensity index and alt_hue_path columns are ignored and the r, g, b (interpreted as RGB), and alpha columns of the cmap1*.pal file are used instead to set the cmap1 palette directly with a call to plscmap1a.</param>
        /// <remarks>Set the cmap1 palette using the specified cmap1*.pal format file.</remarks>
        [DllImport(DllName, EntryPoint = "c_plspal1")]
        public static extern void spal1([MarshalAs(UnmanagedType.LPStr)] string filename, [MarshalAs(UnmanagedType.Bool)] PLBOOL interpolate);

        /// <summary>plspause: Set the pause (on end-of-page) status</summary>
        /// <param name="pause">If pause is true there will be a pause on end-of-page for those drivers which support this. Otherwise there is no pause.</param>
        /// <remarks>Set the pause (on end-of-page) status.</remarks>
        [DllImport(DllName, EntryPoint = "c_plspause")]
        public static extern void spause([MarshalAs(UnmanagedType.Bool)] PLBOOL pause);

        /// <summary>plsstrm: Set current output stream</summary>
        /// <param name="strm">The current stream number.</param>
        /// <remarks>Sets the number of the current output stream. The stream number defaults to 0 unless changed by this routine. The first use of this routine must be followed by a call initializing PLplot (e.g. plstar).</remarks>
        [DllImport(DllName, EntryPoint = "c_plsstrm")]
        public static extern void sstrm(PLINT strm);

        /// <summary>plssub: Set the number of subpages in x and y</summary>
        /// <param name="nx">Number of windows in x direction (i.e., number of window columns).</param>
        /// <param name="ny">Number of windows in y direction (i.e., number of window rows).</param>
        /// <remarks>Set the number of subpages in x and y.</remarks>
        [DllImport(DllName, EntryPoint = "c_plssub")]
        public static extern void ssub(PLINT nx, PLINT ny);

        /// <summary>plssym: Set symbol size</summary>
        /// <param name="def">The default height of a symbol in millimeters, should be set to zero if the default height is to remain unchanged.</param>
        /// <param name="scale">Scale factor to be applied to default to get actual symbol height.</param>
        /// <remarks>This sets up the size of all subsequent symbols drawn by plpoin and plsym. The actual height of a symbol is the product of the default symbol size and a scaling factor as for the character height.</remarks>
        [DllImport(DllName, EntryPoint = "c_plssym")]
        public static extern void ssym(PLFLT def, PLFLT scale);

        /// <summary>plstar: Initialization</summary>
        /// <param name="nx">Number of subpages to divide output page in the x direction.</param>
        /// <param name="ny">Number of subpages to divide output page in the y direction.</param>
        /// <remarks>Initializing the plotting package. The program prompts for the device keyword or number of the desired output device. Hitting a RETURN in response to the prompt is the same as selecting the first device. If only one device is enabled when PLplot is installed, plstar will issue no prompt. The output device is divided into nx by ny subpages, each of which may be used independently. The subroutine pladv is used to advance from one subpage to the next.</remarks>
        [DllImport(DllName, EntryPoint = "c_plstar")]
        public static extern void star(PLINT nx, PLINT ny);

        /// <summary>plstart: Initialization</summary>
        /// <param name="devname">An ascii character string containing the device name keyword of the required output device. If devname is NULL or if the first character of the string is a ``?'', the normal (prompted) start up is used.</param>
        /// <param name="nx">Number of subpages to divide output page in the x direction.</param>
        /// <param name="ny">Number of subpages to divide output page in the y direction.</param>
        /// <remarks>Alternative to plstar for initializing the plotting package. The device name keyword for the desired output device must be supplied as an argument. These keywords are the same as those printed out by plstar. If the requested device is not available, or if the input string is empty or begins with ``?'', the prompted start up of plstar is used. This routine also divides the output device page into nx by ny subpages, each of which may be used independently. The subroutine pladv is used to advance from one subpage to the next.</remarks>
        [DllImport(DllName, EntryPoint = "c_plstart")]
        public static extern void start([MarshalAs(UnmanagedType.LPStr)] string devname, PLINT nx, PLINT ny);

        /// <summary>plstransform: Set a global coordinate transform function</summary>
        /// <param name="coordinate_transform">A callback function that defines the transformation from the input (x, y) world coordinates to new PLplot world coordinates. If coordinate_transform is not supplied (e.g., is set to NULL in the C case), then no transform is applied.</param>
        /// <param name="coordinate_transform_data">Optional extra data for coordinate_transform.</param>
        /// <remarks>This function can be used to define a coordinate transformation which affects all elements drawn within the current plot window. The coordinate_transform callback function is similar to that provided for the plmap and plmeridians functions. The coordinate_transform_data parameter may be used to pass extra data to coordinate_transform.</remarks>
        [DllImport(DllName, EntryPoint = "c_plstransform")]
        public static extern void stransform(TransformFunc coordinate_transform, PLPointer coordinate_transform_data);

        /// <summary>Prints out the same string repeatedly at the n points in world coordinates given by the x and y arrays.  Supersedes plpoin and plsymbol for the case where text refers to a unicode glyph either directly as UTF-8 or indirectly via the standard text escape sequences allowed for PLplot input strings.</summary>
        [DllImport(DllName, EntryPoint = "c_plstring")]
        private static extern void _string2(PLINT n,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                    [MarshalAs(UnmanagedType.LPStr)] string str);

        public static void string2(PLFLT[] x, PLFLT[] y, string str)
        {
            _string2(GetSize(x, y), x, y, str);
        }

        /// <summary>Prints out the same string repeatedly at the n points in world coordinates given by the x, y, and z arrays.  Supersedes plpoin3 for the case where text refers to a unicode glyph either directly as UTF-8 or indirectly via the standard text escape sequences allowed for PLplot input strings.</summary>
        [DllImport(DllName, EntryPoint = "c_plstring3")]
        private static extern void _string3(PLINT n,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] z,
                                                    [MarshalAs(UnmanagedType.LPStr)] string str);

        /// <summary>plstring3: Plot a glyph at the specified 3D points</summary>
        /// <param name="x">A vector containing the x coordinates of the points.</param>
        /// <param name="y">A vector containing the y coordinates of the points.</param>
        /// <param name="z">A vector containing the z coordinates of the points.</param>
        /// <remarks>Plot a glyph at the specified 3D points. (Supersedes plpoin3 because many[!] more glyphs are accessible with plstring3.) Set up the call to this function similar to what is done for plline3. The glyph is specified with a PLplot user string. Note that the user string is not actually limited to one glyph so it is possible (but not normally useful) to plot more than one glyph at the specified points with this function. As with plmtex and plptex, the user string can contain FCI escapes to determine the font, UTF-8 code to determine the glyph or else PLplot escapes for Hershey or unicode text to determine the glyph.</remarks>
        public static void string3(PLFLT[] x, PLFLT[] y, PLFLT[] z, string str)
        {
            _string3(GetSize(x, y, z), x, y, z, str);
        }

        /// <summary>plstripa: Add a point to a strip chart</summary>
        /// <param name="id">Identification number of the strip chart (set up in plstripc).</param>
        /// <param name="pen">Pen number (ranges from 0 to 3).</param>
        /// <param name="x">X coordinate of point to plot.</param>
        /// <param name="y">Y coordinate of point to plot.</param>
        /// <remarks>Add a point to a given pen of a given strip chart. There is no need for all pens to have the same number of points or to be equally sampled in the x coordinate. Allocates memory and rescales as necessary.</remarks>
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
        /// <param name="xspec">An ascii character string containing the x-axis specification as in plbox.</param>
        /// <param name="y_ascl">Autoscale y between x jumps if y_ascl is true, otherwise not.</param>
        /// <param name="ylpos">Y legend box position (range from 0 to 1).</param>
        /// <param name="ymax">Initial coordinates of plot box; they will change as data are added.</param>
        /// <param name="ymin">Initial coordinates of plot box; they will change as data are added.</param>
        /// <param name="yspec">An ascii character string containing the y-axis specification as in plbox.</param>
        /// <remarks>Create a 4-pen strip chart, to be used afterwards by plstripa</remarks>
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

        /// <summary>plstripd: Deletes and releases memory used by a strip chart</summary>
        /// <param name="id">Identification number of strip chart to delete.</param>
        /// <remarks>Deletes and releases memory used by a strip chart.</remarks>
        [DllImport(DllName, EntryPoint = "c_plstripd")]
        public static extern void stripd(PLINT id);

        /// <summary>plots a 2d image (or a matrix too large for plshade() )</summary>
        [DllImport(DllName, EntryPoint = "c_plimagefr")]
        private static extern void _imagefr(PLFLT_MATRIX idata, PLINT nx, PLINT ny,
                                                    PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax, PLFLT zmin, PLFLT zmax,
                                                    PLFLT valuemin, PLFLT valuemax,
                                                    TransformFunc pltr, PLPointer pltr_data);

        /// <summary>plimagefr: Plot a 2D matrix using cmap1</summary>
        /// <param name="idata">A matrix of values (intensities) to plot. Should have dimensions of nx by ny.</param>
        /// <param name="pltr">A callback function that defines the transformation between the zero-based indices of the matrix idata and world coordinates. If pltr is not supplied (e.g., is set to NULL in the C case), then the x indices of idata are mapped to the range xmin through xmax and the y indices of idata are mapped to the range ymin through ymax.For the C case, transformation functions are provided in the PLplot library: pltr0 for the identity mapping, and pltr1 and pltr2 for arbitrary mappings respectively defined by vectors and matrices. In addition, C callback routines for the transformation can be supplied by the user such as the mypltr function in examples/c/x09c.c which provides a general linear transformation between index coordinates and world coordinates.For languages other than C you should consult  for the details concerning how PLTRANSFORM_callback arguments are interfaced. However, in general, a particular pattern of callback-associated arguments such as a tr vector with 6 elements; xg and yg vectors; or xg and yg matrices are respectively interfaced to a linear-transformation routine similar to the above mypltr function; pltr1; and pltr2. Furthermore, some of our more sophisticated bindings (see, e.g., ) support native language callbacks for handling index to world-coordinate transformations. Examples of these various approaches are given in examples/ltlanguagegtx09*, examples/ltlanguagegtx16*, examples/ltlanguagegtx20*, examples/ltlanguagegtx21*, and examples/ltlanguagegtx22*, for all our supported languages.</param>
        /// <param name="pltr_data">Extra parameter to help pass information to pltr0, pltr1, pltr2, or whatever routine is externally supplied.</param>
        /// <remarks>Plot a 2D matrix using cmap1.</remarks>
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

        /// <summary>plots a 2d image (or a matrix too large for plshade() ) - colors automatically scaled</summary>
        [DllImport(DllName, EntryPoint = "c_plimage")]
        private static extern void _image(PLFLT_MATRIX idata, PLINT nx, PLINT ny,
                                                  PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax, PLFLT zmin, PLFLT zmax,
                                                  PLFLT Dxmin, PLFLT Dxmax, PLFLT Dymin, PLFLT Dymax);

        /// <summary>plimage: Plot a 2D matrix using cmap1 with automatic color adjustment</summary>
        /// <param name="idata">A matrix containing function values to plot. Should have dimensions of nx by ny.</param>
        /// <remarks>Plot a 2D matrix using the cmap1 palette. The color scale is automatically adjusted to use the maximum and minimum values in idata as valuemin and valuemax in a call to plimagefr.</remarks>
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

        /// <summary>Set up a new line style</summary>
        [DllImport(DllName, EntryPoint = "c_plstyl")]
        private static extern void _styl(PLINT nms,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] mark,
                                                 [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] space);

        /// <summary>plstyl: Set line style</summary>
        /// <param name="mark">A vector containing the lengths of the segments during which the pen is down, measured in micrometers.</param>
        /// <param name="space">A vector containing the lengths of the segments during which the pen is up, measured in micrometers.</param>
        /// <remarks>This sets up the line style for all lines subsequently drawn. A line consists of segments in which the pen is alternately down and up. The lengths of these segments are passed in the vectors mark and space respectively. The number of mark-space pairs is specified by nms. In order to return the line style to the default continuous line, plstyl should be called with nms =0 .(see also pllsty)</remarks>
        public static void styl(PLINT[] mark, PLINT[] space)
        {
            _styl(GetSize(mark, space), mark, space);
        }

        /// <summary>Plots the 3d surface representation of the function z[x][y].</summary>
        [DllImport(DllName, EntryPoint = "c_plsurf3d")]
        private static extern void _surf3d([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                   [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                   PLFLT_MATRIX z,
                                                   PLINT nx, PLINT ny, Surf opt,
                                                   [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel);

        /// <summary>plsurf3d: Plot shaded 3-d surface plot</summary>
        /// <param name="clevel">A vector containing the contour levels.</param>
        /// <param name="opt">Determines the way in which the surface is represented. To specify more than one option just add the options, e.g. FACETED + SURF_CONT opt=FACETED : Network of lines is drawn connecting points at which function is defined. opt=BASE_CONT : A contour plot is drawn at the base XY plane using parameters nlevel and clevel. opt=SURF_CONT : A contour plot is drawn at the surface plane using parameters nlevel and clevel. opt=DRAW_SIDES : draws a curtain between the base XY plane and the borders of the plotted function. opt=MAG_COLOR : the surface is colored according to the value of Z; if MAG_COLOR is not used, then the surface is colored according to the intensity of the reflected light in the surface from a light source whose position is set using pllightsource.</param>
        /// <param name="x">A vector containing the x coordinates at which the function is evaluated.</param>
        /// <param name="y">A vector containing the y coordinates at which the function is evaluated.</param>
        /// <param name="z">A matrix containing function values to plot. Should have dimensions of nx by ny.</param>
        /// <remarks>Plots a three-dimensional shaded surface plot within the environment set up by plw3d. The surface is defined by the two-dimensional matrix z[nx][ny], the point z[i][j] being the value of the function at (x[i],y[j]). Note that the points in vectors x and y do not need to be equally spaced, but must be stored in ascending order. For further details see .</remarks>
        public static void surf3d(PLFLT[] x, PLFLT[] y, PLFLT[,] z, Surf opt, PLFLT[] clevel)
        {
            using (var mat_z = new MatrixMarshaller(z))
            {
                CheckSize(x, y, z);
                _surf3d(x, y, mat_z.Ptr, mat_z.NX, mat_z.NY, opt, clevel, clevel.Length);
            }
        }

        /// <summary>Plots the 3d surface representation of the function z[x][y] with y index limits.</summary>
        [DllImport(DllName, EntryPoint = "c_plsurf3dl")]
        private static extern void _surf3dl([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                    PLFLT_MATRIX z,
                                                    PLINT nx, PLINT ny, Surf opt,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] clevel, PLINT nlevel,
                                                    PLINT indexxmin, PLINT indexxmax,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] indexymin,
                                                    [In, MarshalAs(UnmanagedType.LPArray)] PLINT[] indexymax);

        /// <summary>plsurf3dl: Plot shaded 3-d surface plot for z[x][y] with y index limits</summary>
        /// <param name="clevel">A vector containing the contour levels.</param>
        /// <param name="indexxmin">The index value (which must be ≥ 0) that corresponds to the first x index where z is defined.</param>
        /// <param name="indexymax">A vector containing the y index values which all must be ≤ ny. These values correspond (by convention) to one more than the last y index where z is defined for a particular x index in the range from indexxmin to indexxmax - 1. The dimension of indexymax is indexxmax.</param>
        /// <param name="indexymin">A vector containing the y index values which all must be ≥ 0. These values are the first y index where z is defined for a particular x index in the range from indexxmin to indexxmax - 1. The dimension of indexymin is indexxmax.</param>
        /// <param name="opt">Determines the way in which the surface is represented. To specify more than one option just add the options, e.g. FACETED + SURF_CONT opt=FACETED : Network of lines is drawn connecting points at which function is defined. opt=BASE_CONT : A contour plot is drawn at the base XY plane using parameters nlevel and clevel. opt=SURF_CONT : A contour plot is drawn at the surface plane using parameters nlevel and clevel. opt=DRAW_SIDES : draws a curtain between the base XY plane and the borders of the plotted function. opt=MAG_COLOR : the surface is colored according to the value of Z; if MAG_COLOR is not used, then the surface is colored according to the intensity of the reflected light in the surface from a light source whose position is set using pllightsource.</param>
        /// <param name="x">A vector containing the x coordinates at which the function is evaluated.</param>
        /// <param name="y">A vector containing the y coordinates at which the function is evaluated.</param>
        /// <param name="z">A matrix containing function values to plot. Should have dimensions of nx by ny.</param>
        /// <remarks>This variant of plsurf3d (see that function's documentation for more details) should be suitable for the case where the area of the x, y coordinate grid where z is defined can be non-rectangular. The limits of that grid are provided by the parameters indexxmin, indexxmax, indexymin, and indexymax.</remarks>
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

        /// <summary>Set arrow style for vector plots.</summary>
        [DllImport(DllName, EntryPoint = "c_plsvect")]
        private static extern void _svect([In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] arrowx,
                                                  [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] arrowy,
                                                  PLINT npts, [MarshalAs(UnmanagedType.Bool)] PLBOOL fill);

        /// <summary>plsvect: Set arrow style for vector plots</summary>
        /// <param name="fill">If fill is true then the arrow is closed, if fill is false then the arrow is open.</param>
        /// <remarks>Set the style for the arrow used by plvect to plot vectors.</remarks>
        public static void svect(PLFLT[] arrowx, PLFLT[] arrowy, PLBOOL fill)
        {
            _svect(arrowx, arrowy, GetSize(arrowx, arrowy), fill);
        }

        /// <summary>plsvpa: Specify viewport in absolute coordinates</summary>
        /// <param name="xmax">The distance of the right-hand edge of the viewport from the left-hand edge of the subpage in millimeters.</param>
        /// <param name="xmin">The distance of the left-hand edge of the viewport from the left-hand edge of the subpage in millimeters.</param>
        /// <param name="ymax">The distance of the top edge of the viewport from the bottom edge of the subpage in millimeters.</param>
        /// <param name="ymin">The distance of the bottom edge of the viewport from the bottom edge of the subpage in millimeters.</param>
        /// <remarks>Alternate routine to plvpor for setting up the viewport. This routine should be used only if the viewport is required to have a definite size in millimeters. The routine plgspa is useful for finding out the size of the current subpage.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsvpa")]
        public static extern void svpa(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax);

        /// <summary>plsxax: Set x axis parameters</summary>
        /// <param name="digits">Field digits value. Currently, changing its value here has no effect since it is set only by plbox or plbox3. However, the user may obtain its value after a call to either of these functions by calling plgxax.</param>
        /// <param name="digmax">Variable to set the maximum number of digits for the x axis. If nonzero, the printed label will be switched to a floating-point representation when the number of digits exceeds digmax.</param>
        /// <remarks>Sets values of the digmax and digits flags for the x axis. See  for more information.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsxax")]
        public static extern void sxax(PLINT digmax, PLINT digits);

        /// <summary>Set inferior X window</summary>
        [DllImport(DllName, EntryPoint = "plsxwin")]
        public static extern void sxwin(PLINT window_id);

        /// <summary>plsyax: Set y axis parameters</summary>
        /// <param name="digits">Field digits value. Currently, changing its value here has no effect since it is set only by plbox or plbox3. However, the user may obtain its value after a call to either of these functions by calling plgyax.</param>
        /// <param name="digmax">Variable to set the maximum number of digits for the y axis. If nonzero, the printed label will be switched to a floating-point representation when the number of digits exceeds digmax.</param>
        /// <remarks>Identical to plsxax, except that arguments are flags for y axis. See the description of plsxax for more detail.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsyax")]
        public static extern void syax(PLINT digmax, PLINT digits);

        /// <summary>Plots array y against x for n points using Hershey symbol "code"</summary>
        [DllImport(DllName, EntryPoint = "c_plsym")]
        private static extern void _sym(PLINT n,
                                                [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] x,
                                                [In, MarshalAs(UnmanagedType.LPArray)] PLFLT[] y,
                                                PLINT code);

        /// <summary>plsym: Plot a glyph at the specified points</summary>
        /// <param name="code">Hershey symbol code corresponding to a glyph to be plotted at each of the n points.</param>
        /// <param name="x">A vector containing the x coordinates of the points.</param>
        /// <param name="y">A vector containing the y coordinates of the points.</param>
        /// <remarks>Plot a glyph at the specified points. (This function is largely superseded by plstring which gives access to many[!] more glyphs.)</remarks>
        public static void sym(PLFLT[] x, PLFLT[] y, PLINT code)
        {
            _sym(GetSize(x, y), x, y, code);
        }

        /// <summary>plszax: Set z axis parameters</summary>
        /// <param name="digits">Field digits value. Currently, changing its value here has no effect since it is set only by plbox or plbox3. However, the user may obtain its value after a call to either of these functions by calling plgzax.</param>
        /// <param name="digmax">Variable to set the maximum number of digits for the z axis. If nonzero, the printed label will be switched to a floating-point representation when the number of digits exceeds digmax.</param>
        /// <remarks>Identical to plsxax, except that arguments are flags for z axis. See the description of plsxax for more detail.</remarks>
        [DllImport(DllName, EntryPoint = "c_plszax")]
        public static extern void szax(PLINT digmax, PLINT digits);

        /// <summary>pltext: Switch to text screen</summary>
        /// <remarks>Sets an interactive device to text mode, used in conjunction with plgra to allow graphics and text to be interspersed. On a device which supports separate text and graphics windows, this command causes control to be switched to the text window. This can be useful for printing diagnostic messages or getting user input, which would otherwise interfere with the plots. The program must switch back to the graphics window before issuing plot commands, as the text (or console) device will probably become quite confused otherwise. If already in text mode, this command is ignored. It is also ignored on devices which only support a single window or use a different method for shifting focus (see also plgra).</remarks>
        [DllImport(DllName, EntryPoint = "c_pltext")]
        public static extern void text();

        /// <summary>pltimefmt: Set format for date / time labels</summary>
        /// <param name="fmt">An ascii character string which is interpreted similarly to the format specifier of typical system strftime routines except that PLplot ignores locale and also supplies some useful extensions in the context of plotting. All text in the string is printed as-is other than conversion specifications which take the form of a '%' character followed by further conversion specification character. The conversion specifications which are similar to those provided by system strftime routines are the following: %a: The abbreviated (English) weekday name. %A: The full (English) weekday name. %b: The abbreviated (English) month name. %B: The full (English) month name. %c: Equivalent to %a %b %d %T %Y (non-ISO). %C: The century number (year/100) as a 2-digit integer. %d: The day of the month as a decimal number (range 01 to 31). %D: Equivalent to %m/%d/%y (non-ISO). %e: Like %d, but a leading zero is replaced by a space. %F: Equivalent to %Y-%m-%d (the ISO 8601 date format). %h: Equivalent to %b. %H: The hour as a decimal number using a 24-hour clock (range 00 to 23). %I: The hour as a decimal number using a 12-hour clock (range 01 to 12). %j: The day of the year as a decimal number (range 001 to 366). %k: The hour (24-hour clock) as a decimal number (range 0 to 23); single digits are preceded by a blank. (See also %H.) %l: The hour (12-hour clock) as a decimal number (range 1 to 12); single digits are preceded by a blank. (See also %I.) %m: The month as a decimal number (range 01 to 12). %M: The minute as a decimal number (range 00 to 59). %n: A newline character. %p: Either "AM" or "PM" according to the given time value. Noon is treated as "PM" and midnight as "AM". %r: Equivalent to %I:%M:%S %p. %R: The time in 24-hour notation (%H:%M). For a version including the seconds, see %T below. %s: The number of seconds since the Epoch, 1970-01-01 00:00:00 +0000 (UTC). %S: The second as a decimal number (range 00 to 60). (The range is up to 60 to allow for occasional leap seconds.) %t: A tab character. %T: The time in 24-hour notation (%H:%M:%S). %u: The day of the week as a decimal, range 1 to 7, Monday being 1. See also %w. %U: The week number of the current year as a decimal number, range 00 to 53, starting with the first Sunday as the first day of week 01. See also %V and %W. %v: Equivalent to %e-%b-%Y. %V: The ISO 8601 week number of the current year as a decimal number, range 01 to 53, where week 1 is the first week that has at least 4 days in the new year. See also %U and %W. %w: The day of the week as a decimal, range 0 to 6, Sunday being 0. See also %u. %W: The week number of the current year as a decimal number, range 00 to 53, starting with the first Monday as the first day of week 01. %x: Equivalent to %a %b %d %Y. %X: Equivalent to %T. %y: The year as a decimal number without a century (range 00 to 99). %Y: The year as a decimal number including a century. %z: The UTC time-zone string = "+0000". %Z: The UTC time-zone abbreviation = "UTC". %+: The UTC date and time in default format of the Unix date command which is equivalent to %a %b %d %T %Z %Y. %%: A literal "%" character.  The conversion specifications which are extensions to those normally provided by system strftime routines are the following: %(0-9): The fractional part of the seconds field (including leading decimal point) to the specified accuracy. Thus %S%3 would give seconds to millisecond accuracy (00.000). %.: The fractional part of the seconds field (including leading decimal point) to the maximum available accuracy. Thus %S%. would give seconds with fractional part up to 9 decimal places if available.</param>
        /// <remarks>Sets the format for date / time labels. To enable date / time format labels see the options to plbox, plbox3, and plenv.</remarks>
        [DllImport(DllName, EntryPoint = "c_pltimefmt")]
        public static extern void timefmt([MarshalAs(UnmanagedType.LPStr)] string fmt);

        /// <summary>plvasp: Specify viewport using aspect ratio only</summary>
        /// <param name="aspect">Ratio of length of y axis to length of x axis of resulting viewport.</param>
        /// <remarks>Selects the largest viewport with the given aspect ratio within the subpage that leaves a standard margin (left-hand margin of eight character heights, and a margin around the other three sides of five character heights).</remarks>
        [DllImport(DllName, EntryPoint = "c_plvasp")]
        public static extern void vasp(PLFLT aspect);

        /// <summary>simple arrow plotter.</summary>
        [DllImport(DllName, EntryPoint = "c_plvect")]
        private static extern void _vect(PLFLT_MATRIX u, PLFLT_MATRIX v, PLINT nx, PLINT ny, PLFLT scale,
                                                 TransformFunc pltr, PLPointer pltr_data);

        /// <summary>plvect: Vector plot</summary>
        /// <param name="pltr">A callback function that defines the transformation between the zero-based indices of the matrices u and v and world coordinates.For the C case, transformation functions are provided in the PLplot library: pltr0 for the identity mapping, and pltr1 and pltr2 for arbitrary mappings respectively defined by vectors and matrices. In addition, C callback routines for the transformation can be supplied by the user such as the mypltr function in examples/c/x09c.c which provides a general linear transformation between index coordinates and world coordinates.For languages other than C you should consult  for the details concerning how PLTRANSFORM_callback arguments are interfaced. However, in general, a particular pattern of callback-associated arguments such as a tr vector with 6 elements; xg and yg vectors; or xg and yg matrices are respectively interfaced to a linear-transformation routine similar to the above mypltr function; pltr1; and pltr2. Furthermore, some of our more sophisticated bindings (see, e.g., ) support native language callbacks for handling index to world-coordinate transformations. Examples of these various approaches are given in examples/ltlanguagegtx09*, examples/ltlanguagegtx16*, examples/ltlanguagegtx20*, examples/ltlanguagegtx21*, and examples/ltlanguagegtx22*, for all our supported languages.</param>
        /// <param name="pltr_data">Extra parameter to help pass information to pltr0, pltr1, pltr2, or whatever callback routine that is externally supplied.</param>
        /// <param name="scale">Parameter to control the scaling factor of the vectors for plotting. If scale = 0  then the scaling factor is automatically calculated for the data. If scale lt 0 then the scaling factor is automatically calculated for the data and then multiplied by -scale. If scale gt 0 then the scaling factor is set to scale.</param>
        /// <remarks>Draws a plot of vector data contained in the matrices  (u[nx][ny],v[nx][ny]) . The scaling factor for the vectors is given by scale. A transformation routine pointed to by pltr with a pointer pltr_data for additional data required by the transformation routine to map indices within the matrices to the world coordinates. The style of the vector arrow may be set using plsvect.</remarks>
        public static void vect(PLFLT[,] u, PLFLT[,] v, PLFLT scale,
                                        TransformFunc pltr, PLPointer pltr_data)
        {
            using (var u_mat = new MatrixMarshaller(u))
            {
                using (var v_mat = new MatrixMarshaller(v))
                {
                    CheckSize(u, v);
                    _vect(u_mat.Ptr, v_mat.Ptr, u_mat.NX, u_mat.NY, scale,
                          pltr, pltr_data);
                }
            }
        }

        /// <summary>plvpas: Specify viewport using coordinates and aspect ratio</summary>
        /// <param name="aspect">Ratio of length of y axis to length of x axis.</param>
        /// <param name="xmax">The normalized subpage coordinate of the right-hand edge of the viewport.</param>
        /// <param name="xmin">The normalized subpage coordinate of the left-hand edge of the viewport.</param>
        /// <param name="ymax">The normalized subpage coordinate of the top edge of the viewport.</param>
        /// <param name="ymin">The normalized subpage coordinate of the bottom edge of the viewport.</param>
        /// <remarks>Device-independent routine for setting up the viewport. The viewport is chosen to be the largest with the given aspect ratio that fits within the specified region (in terms of normalized subpage coordinates). This routine is functionally equivalent to plvpor when a ``natural'' aspect ratio (0.0) is chosen. Unlike plvasp, this routine reserves no extra space at the edges for labels.</remarks>
        [DllImport(DllName, EntryPoint = "c_plvpas")]
        public static extern void vpas(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax, PLFLT aspect);

        /// <summary>plvpor: Specify viewport using normalized subpage coordinates</summary>
        /// <param name="xmax">The normalized subpage coordinate of the right-hand edge of the viewport.</param>
        /// <param name="xmin">The normalized subpage coordinate of the left-hand edge of the viewport.</param>
        /// <param name="ymax">The normalized subpage coordinate of the top edge of the viewport.</param>
        /// <param name="ymin">The normalized subpage coordinate of the bottom edge of the viewport.</param>
        /// <remarks>Device-independent routine for setting up the viewport. This defines the viewport in terms of normalized subpage coordinates which run from 0.0 to 1.0 (left to right and bottom to top) along each edge of the current subpage. Use the alternate routine plsvpa in order to create a viewport of a definite size.</remarks>
        [DllImport(DllName, EntryPoint = "c_plvpor")]
        public static extern void vpor(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax);

        /// <summary>plvsta: Select standard viewport</summary>
        /// <remarks>Selects the largest viewport within the subpage that leaves a standard margin (left-hand margin of eight character heights, and a margin around the other three sides of five character heights).</remarks>
        [DllImport(DllName, EntryPoint = "c_plvsta")]
        public static extern void vsta();

        /// <summary>plw3d: Configure the transformations required for projecting a 3D surface on a 2D window</summary>
        /// <param name="alt">The viewing altitude in degrees above the xy plane of the rectangular cuboid in normalized coordinates.</param>
        /// <param name="az">The viewing azimuth in degrees of the rectangular cuboid in normalized coordinates. When az=0, the observer is looking face onto the zx plane of the rectangular cuboid in normalized coordinates, and as az is increased, the observer moves clockwise around that cuboid when viewed from above the xy plane.</param>
        /// <param name="basex">The normalized x coordinate size of the rectangular cuboid.</param>
        /// <param name="basey">The normalized y coordinate size of the rectangular cuboid.</param>
        /// <param name="height">The normalized z coordinate size of the rectangular cuboid.</param>
        /// <param name="xmax">The maximum x world coordinate of the rectangular cuboid.</param>
        /// <param name="xmin">The minimum x world coordinate of the rectangular cuboid.</param>
        /// <param name="ymax">The maximum y world coordinate of the rectangular cuboid.</param>
        /// <param name="ymin">The minimum y world coordinate of the rectangular cuboid.</param>
        /// <param name="zmax">The maximum z world coordinate of the rectangular cuboid.</param>
        /// <param name="zmin">The minimum z world coordinate of the rectangular cuboid.</param>
        /// <remarks>Configure the transformations required for projecting a 3D surface on an existing 2D window. Those transformations (see ) are done to a rectangular cuboid enclosing the 3D surface which has its limits expressed in 3D world coordinates and also normalized 3D coordinates (used for interpreting the altitude and azimuth of the viewing angle). The transformations consist of the linear transform from 3D world coordinates to normalized 3D coordinates, and the 3D rotation of normalized coordinates required to align the pole of the new 3D coordinate system with the viewing direction specified by altitude and azimuth so that x and y of the surface elements in that transformed coordinate system are the projection of the 3D surface with given viewing direction on the 2D window.</remarks>
        [DllImport(DllName, EntryPoint = "c_plw3d")]
        public static extern void w3d(PLFLT basex, PLFLT basey, PLFLT height, PLFLT xmin,
                                              PLFLT xmax, PLFLT ymin, PLFLT ymax, PLFLT zmin,
                                              PLFLT zmax, PLFLT alt, PLFLT az);

        /// <summary>plwidth: Set pen width</summary>
        /// <param name="width">The desired pen width. If width is negative or the same as the previous value no action is taken. width = 0.  should be interpreted as as the minimum valid pen width for the device. The interpretation of positive width values is also device dependent.</param>
        /// <remarks>Sets the pen width.</remarks>
        [DllImport(DllName, EntryPoint = "c_plwidth")]
        public static extern void width(PLFLT width);

        /// <summary>plwind: Specify window</summary>
        /// <param name="xmax">The world x coordinate of the right-hand edge of the viewport.</param>
        /// <param name="xmin">The world x coordinate of the left-hand edge of the viewport.</param>
        /// <param name="ymax">The world y coordinate of the top edge of the viewport.</param>
        /// <param name="ymin">The world y coordinate of the bottom edge of the viewport.</param>
        /// <remarks>Specify the window, i.e., the world coordinates of the edges of the viewport.</remarks>
        [DllImport(DllName, EntryPoint = "c_plwind")]
        public static extern void wind(PLFLT xmin, PLFLT xmax, PLFLT ymin, PLFLT ymax);

        /// <summary>plxormod: Enter or leave xor mode</summary>
        /// <param name="mode">mode is true means enter xor mode and mode is false means leave xor mode.</param>
        /// <param name="status">Returned value of the status. modestatus of true (false) means driver is capable (incapable) of xor mode.</param>
        /// <remarks>Enter (when mode is true) or leave (when mode is false) xor mode for those drivers (e.g., the xwin driver) that support it. Enables erasing plots by drawing twice the same line, symbol, etc. If driver is not capable of xor operation it returns a status of false.</remarks>
        [DllImport(DllName, EntryPoint = "c_plxormod")]
        public static extern void xormod([MarshalAs(UnmanagedType.Bool)] PLBOOL mode,
                                                 [MarshalAs(UnmanagedType.Bool)] out PLBOOL status);

        /// <summary>Returns a list of file-oriented device names and their menu strings</summary>
        [DllImport(DllName, EntryPoint = "plgFileDevs")]
        private static extern void _gFileDevs(ref IntPtr p_menustr, ref IntPtr p_devname, ref int p_ndev);

        /// <summary>Returns a list of file-oriented device names and their menu strings</summary>
        /// <param name="p_devname">device name</param>
        /// <param name="p_menustr">human-readable name</param>
        public static void gFileDevs(out string[] p_menustr, out string[] p_devname)
        {
            GetDevHelper(_gFileDevs, out p_menustr, out p_devname);
        }

        /// <summary>Returns a list of all device names and their menu strings</summary>
        [DllImport(DllName, EntryPoint = "plgDevs")]
        private static extern void _gDevs(ref IntPtr p_menustr, ref IntPtr p_devname, ref int p_ndev);

        /// <summary>Returns a list of all device names and their menu strings</summary>
        /// <param name="p_devname">device name</param>
        /// <param name="p_menustr">human-readable name</param>
        public static void gDevs(out string[] p_menustr, out string[] p_devname)
        {
            GetDevHelper(_gDevs, out p_menustr, out p_devname);
        }

        /// <summary>Set the function pointer for the keyboard event handler</summary>
        [DllImport(DllName, EntryPoint = "plsKeyEH")]
        public static extern void sKeyEH(KeyHandler keyEH, PLPointer KeyEH_data);

        /// <summary>Set the function pointer for the (mouse) button event handler</summary>
        [DllImport(DllName, EntryPoint = "plsButtonEH")]
        public static extern void sButtonEH(ButtonHandler buttonEH, PLPointer ButtonEH_data);

        /// <summary>Sets an optional user bop handler</summary>
        [DllImport(DllName, EntryPoint = "plsbopH")]
        public static extern void sbopH(BopHandler handler, PLPointer handler_data);

        /// <summary>Sets an optional user eop handler</summary>
        [DllImport(DllName, EntryPoint = "plseopH")]
        public static extern void seopH(EopHandler handler, PLPointer handler_data);

        /// <summary>plsexit: Set exit handler</summary>
        /// <param name="handler">Error exit handler.</param>
        /// <remarks>Sets an optional user exit handler. See plexit for details.</remarks>
        [DllImport(DllName, EntryPoint = "plsexit")]
        public static extern void sexit(ExitHandler handler);

        /// <summary>plsabort: Set abort handler</summary>
        /// <param name="handler">Error abort handler.</param>
        /// <remarks>Sets an optional user abort handler. See plabort for details.</remarks>
        [DllImport(DllName, EntryPoint = "plsabort")]
        public static extern void sabort(AbortHandler handler);

        /// <summary>pltr0: Identity transformation for matrix index to world coordinate mapping</summary>
        /// <param name="pltr_data">A pointer to additional data that is passed as an argument to PLplot routines that potentially could use the pltr0 callback (i.e., plcont, plimagefr, plshade, plshades, and plvect); which then internally pass that argument on to this callback.</param>
        /// <param name="tx">Transformed x value in world coordinates corresponding to x index of matrix.</param>
        /// <param name="ty">Transformed y value in world coordinates corresponding to y index of matrix.</param>
        /// <param name="x">X index of matrix.</param>
        /// <param name="y">Y index of matrix.</param>
        /// <remarks>Identity transformation for matrix index to world coordinate mapping. This routine can be used for the PLTRANSFORM_callback argument of plcont, plshade, plshades, plimagefr, or plvect.</remarks>
        [DllImport(DllName, EntryPoint = "pltr0")]
        public static extern void tr0(PLFLT x, PLFLT y, out PLFLT tx, out PLFLT ty, PLPointer pltr_data);

        /// <summary>pltr1: Linear interpolation for matrix index to world coordinate mapping using singly dimensioned coordinate arrays</summary>
        /// <param name="pltr_data">A pointer to additional data that is passed as an argument to PLplot routines that potentially could use the pltr1 callback (i.e., plcont, plimagefr, plshade, plshades, and plvect); which then internally pass that argument on to this callback.</param>
        /// <param name="tx">Transformed x value in world coordinates corresponding to x index of matrix.</param>
        /// <param name="ty">Transformed y value in world coordinates corresponding to y index of matrix.</param>
        /// <param name="x">X index of matrix.</param>
        /// <param name="y">Y index of matrix.</param>
        /// <remarks>Linear interpolation for matrix index to world coordinate mapping using one-dimensional x and y coordinate arrays. This routine can be used for the PLTRANSFORM_callback argument of plcont, plshade, plshades, plimagefr, or plvect.</remarks>
        [DllImport(DllName, EntryPoint = "pltr1")]
        public static extern void tr1(PLFLT x, PLFLT y, out PLFLT tx, out PLFLT ty, PLPointer pltr_data);

        /// <summary>pltr2: Linear interpolation for grid to world mapping using doubly dimensioned coordinate arrays (row-major order as per normal C 2d arrays)</summary>
        /// <param name="pltr_data">A pointer to additional data that is passed as an argument to PLplot routines that potentially could use the pltr2 callback (i.e., plcont, plimagefr, plshade, plshades, and plvect); which then internally pass that argument on to this callback.</param>
        /// <param name="tx">Transformed x value in world coordinates corresponding to x index of matrix.</param>
        /// <param name="ty">Transformed y value in world coordinates corresponding to y index of matrix.</param>
        /// <param name="x">X index of matrix.</param>
        /// <param name="y">Y index of matrix.</param>
        /// <remarks>Linear interpolation for grid to world mapping using two-dimensional x and y coordinate arrays. This routine can be used for the PLTRANSFORM_callback argument of plcont, plshade, plshades, plimagefr, or plvect.</remarks>
        [DllImport(DllName, EntryPoint = "pltr2")]
        public static extern void tr2(PLFLT x, PLFLT y, out PLFLT tx, out PLFLT ty, PLPointer pltr_data);

        /// <summary>Just like pltr2() but uses pointer arithmetic to get coordinates from 2d grid tables.</summary>
        [DllImport(DllName, EntryPoint = "pltr2p")]
        public static extern void tr2p(PLFLT x, PLFLT y, out PLFLT tx, out PLFLT ty, PLPointer pltr_data);

        /// <summary>Does linear interpolation from doubly dimensioned coord arrays (row dominant, i.e. Fortran ordering).</summary>
        [DllImport(DllName, EntryPoint = "pltr2f")]
        public static extern void tr2f(PLFLT x, PLFLT y, out PLFLT tx, out PLFLT ty, PLPointer pltr_data);

        /// <summary>plClearOpts: Clear internal option table info structure.</summary>
        /// <remarks>Clear the internal options table info structure. This removes any option added with plMergeOpts as well as all default entries.</remarks>
        [DllImport(DllName, EntryPoint = "plClearOpts")]
        public static extern void ClearOpts();

        /// <summary>plResetOpts: Reset internal option table info structure.</summary>
        /// <remarks>Resets the internal command line options table to the default built in value. Any user options added with plMergeOpts will be cleared. See plparseopts for details of how to parse these options in a program.</remarks>
        [DllImport(DllName, EntryPoint = "plResetOpts")]
        public static extern void ResetOpts();

        /// <summary>plSetUsage: Set the ascii character strings used in usage and syntax messages.</summary>
        /// <param name="program_string">An ascii character string to appear as the name of program.</param>
        /// <param name="usage_string">An ascii character string to appear as the usage text.</param>
        /// <remarks>Sets the program string and usage string displayed by the command line help option (-h) and by plOptUsage.</remarks>
        [DllImport(DllName, EntryPoint = "plSetUsage")]
        public static extern void SetUsage([MarshalAs(UnmanagedType.LPStr)] string program_string,
                                                   [MarshalAs(UnmanagedType.LPStr)] string usage_string);

        /// <summary>plsetopt: Set any command-line option</summary>
        /// <param name="opt">An ascii character string containing the command-line option.</param>
        /// <param name="optarg">An ascii character string containing the argument of the command-line option.</param>
        /// <remarks>Set any command-line option internally from a program before it invokes plinit. opt is the name of the command-line option and optarg is the corresponding command-line option argument.</remarks>
        [DllImport(DllName, EntryPoint = "c_plsetopt")]
        public static extern PLINT setopt([MarshalAs(UnmanagedType.LPStr)] string opt,
                                                  [MarshalAs(UnmanagedType.LPStr)] string optarg);

        /// <summary>plparseopts: Parse command-line arguments</summary>
        /// <param name="argv">A vector of character strings containing *p_argc command-line arguments.</param>
        /// <param name="mode">Parsing mode with the following possibilities:  PL_PARSE_FULL (1) -- Full parsing of command line and all error messages enabled, including program exit when an error occurs. Anything on the command line that isn't recognized as a valid option or option argument is flagged as an error.  PL_PARSE_QUIET (2) -- Turns off all output except in the case of errors.  PL_PARSE_NODELETE (4) -- Turns off deletion of processed arguments.  PL_PARSE_SHOWALL (8) -- Show invisible options  PL_PARSE_NOPROGRAM (32) -- Specified if argv[0] is NOT a pointer to the program name.  PL_PARSE_NODASH (64) -- Set if leading dash is NOT required.  PL_PARSE_SKIP (128) -- Set to quietly skip over any unrecognized arguments.</param>
        /// <param name="p_argc">Number of arguments.</param>
        /// <remarks>Parse command-line arguments.</remarks>
        [DllImport(DllName, EntryPoint = "c_plparseopts")]
        public static extern PLINT parseopts(ref int p_argc,
                                                     [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] argv,
                                                     Parse mode);

        /// <summary>plOptUsage: Print usage and syntax message.</summary>
        /// <remarks>Prints the usage and syntax message. The message can also be displayed using the -h command line option. There is a default message describing the default PLplot options. The usage message is also modified by plSetUsage and plMergeOpts.</remarks>
        [DllImport(DllName, EntryPoint = "plOptUsage")]
        public static extern void OptUsage();

        /// <summary>plgfile: Get output file handle</summary>
        /// <remarks>Gets the current output file handle, if applicable.</remarks>
        [DllImport(DllName, EntryPoint = "plgfile")]
        public static extern void gfile(out FILE p_file);

        /// <summary>plsfile: Set output file handle</summary>
        /// <param name="file">File pointer. The type (i.e. text or binary) doesn't matter on *ix systems. On systems where it might matter it should match the type of file that the output driver would produce, i.e. text for the postscript driver.</param>
        /// <remarks>Sets the current output file handle, if applicable. If the file has has not been previously opened and is required by the driver, the user will be prompted for the file name. This routine, if used, must be called before initializing PLplot.</remarks>
        [DllImport(DllName, EntryPoint = "plsfile")]
        public static extern void sfile(FILE file);

        /// <summary>Get the escape character for text strings.</summary>
        [DllImport(DllName, EntryPoint = "plgesc")]
        public static extern void gesc(out PLCHAR p_esc);

        /// <summary>Front-end to driver escape function.</summary>
        [DllImport(DllName, EntryPoint = "pl_cmd")]
        public static extern void cmd(PLINT op, PLPointer ptr);

        /// <summary>plAlloc2dGrid: Allocate a block of memory for use as a matrix of type PLFLT_MATRIX.</summary>
        /// <param name="f">If the allocation is a success, a pointer to a matrix (organized as an Iliffe column vector of pointers to row vectors) is returned. If the allocation is a failure, then the returned value of f will be NULL.</param>
        /// <remarks>Allocates the memory for a matrix of type PLFLT_MATRIX (organized as an Iliffe column vector of pointers to row vectors). As a result the matrix can be accessed using C/C++ syntax like *f[i][j]. The memory associated with this matrix must be freed by calling plFree2dGrid once it is no longer required.</remarks>
        [DllImport(DllName, EntryPoint = "plAlloc2dGrid")]
        public static extern void Alloc2dGrid(out PLFLT_NC_MATRIX f, PLINT nx, PLINT ny);

        /// <summary>plFree2dGrid: Free the memory associated with a PLFLT matrix allocated using plAlloc2dGrid.</summary>
        /// <param name="f">A matrix to be freed after allocation by plAlloc2dGrid and all use of the matrix has been completed.</param>
        /// <remarks>Frees a block of memory allocated using plAlloc2dGrid.</remarks>
        [DllImport(DllName, EntryPoint = "plFree2dGrid")]
        public static extern void Free2dGrid(PLFLT_NC_MATRIX f, PLINT nx, PLINT ny);

        /// <summary>plMinMax2dGrid: Find the minimum and maximum of a PLFLT matrix of type PLFLT_MATRIX allocated using plAlloc2dGrid.</summary>
        /// <param name="f">A matrix to find the maximum / minimum of.</param>
        /// <remarks>Find the minimum and maximum of a PLFLT matrix of type PLFLT_MATRIX allocated using plAlloc2dGrid.</remarks>
        [DllImport(DllName, EntryPoint = "plMinMax2dGrid")]
        public static extern void MinMax2dGrid(PLFLT_MATRIX f, PLINT nx, PLINT ny, out PLFLT fmax, out PLFLT fmin);

        /// <summary>plGetCursor: Wait for graphics input event and translate to world coordinates.</summary>
        /// <param name="gin">Pointer to PLGraphicsIn structure which will contain the output. The structure is not allocated by the routine and must exist before the function is called.</param>
        /// <remarks>Wait for graphics input event and translate to world coordinates. Returns 0 if no translation to world coordinates is possible.</remarks>
        [DllImport(DllName, EntryPoint = "plGetCursor")]
        public static extern PLINT GetCursor(out GraphicsIn gin);

        /// <summary>plTranslateCursor: Convert device to world coordinates</summary>
        /// <param name="gin">Pointer to a PLGraphicsIn structure to hold the input and output coordinates.</param>
        /// <remarks>Convert from device to world coordinates. The variable gin must have members dX and dY set before the call.These represent the coordinates of the point as a fraction of the total drawing area. If the point passed in is on a window then the function returns 1, members wX and wY will be filled with the world coordinates of that point and the subwindow member will be filled with the index of the window on which the point falls. If the point falls on more than one window (because they overlap) then the window with the lowest index is used. If the point does not fall on a window then the function returns 0, wX and wY are set to 0 and subwindow remains unchanged.</remarks>
        [DllImport(DllName, EntryPoint = "plTranslateCursor")]
        public static extern PLINT TranslateCursor(out GraphicsIn gin);

        /// <summary>Set the pointer to the data used in driver initialisation</summary>
        [DllImport(DllName, EntryPoint = "plsdevdata")]
        public static extern void sdevdata(PLPointer data);

    }
}
