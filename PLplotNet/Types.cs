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


    /// <summary>Run level of PLplot.</summary>
    public enum RunLevel : PLINT
    {
        /// <summary>Uninitialized</summary>
        Uninitialized = 0,
        /// <summary>Initialized</summary>
        Initialized = 1,
        /// <summary>Viewport has been defined</summary>
        ViewportDefined = 2,
        /// <summary>Viewport and world coordinates have been defined defined</summary>
        WorldCoordinatesDefined = 3
    }

    /// <summary>Command line parsing mode.</summary>
    [Flags]
    public enum ParseOpts : PLINT
    {
        /// <summary>Full parsing of command line and all error messages enabled, including program exit when an error occurs. Anything on the command line that isn't recognized as a valid option or option argument is flagged as an error.</summary>
        Full = 0x0001, 
        /// <summary>Turns off all output except in the case of errors.</summary>
        Quiet = 0x0002, 
        /// <summary>Turns off deletion of processed arguments.</summary>
        NoDelete = 0x0004, 
        /// <summary>Show invisible options</summary>
        ShowAll = 0x0008, 
        /// <summary>Specified if argv[0] is NOT a pointer to the program name.</summary>
        NoProgram = 0x0020, 
        /// <summary>Set if leading dash NOT required</summary>
        NoDash = 0x0040, 
        /// <summary>Set to quietly skip over any unrecognized arguments.</summary>
        Skip = 0x0080 
    }

    /// <summary>Predefined colors on default color map.</summary>
    public static class Color
    {
        /// <summary>Black</summary>
        const PLINT Black = 0;
        /// <summary>Red</summary>
        const PLINT Red = 1;
        /// <summary>Yellow</summary>
        const PLINT Yellow = 2;
        /// <summary>Green</summary>
        const PLINT Green = 3;
        /// <summary>Aquamarine</summary>
        const PLINT Aquamarine = 4;
        /// <summary>Pink</summary>
        const PLINT Pink = 5;
        /// <summary>Wheat</summary>
        const PLINT Wheat = 6;
        /// <summary>Grey</summary>
        const PLINT Grey = 7;
        /// <summary>Brown</summary>
        const PLINT Brown = 8;
        /// <summary>Blue</summary>
        const PLINT Blue = 9;
        /// <summary>BlueViolet</summary>
        const PLINT BlueViolet = 10;
        /// <summary>Cyan</summary>
        const PLINT Cyan = 11;
        /// <summary>Turquoise</summary>
        const PLINT Turquoise = 12;
        /// <summary>Magenta</summary>
        const PLINT Magenta = 13;
        /// <summary>Salmon</summary>
        const PLINT Salmon = 14;
        /// <summary>White</summary>
        const PLINT White = 15;
    }

    /// <summary>Options for an X-axis or Y-axis.</summary>
    public static class XYAxisOpt
    {
        /// <summary>No options.</summary>
        const string None = "";       
        /// <summary>Draws axis, X-axis is horizontal line (y=0), and Y-axis is vertical line (x=0).</summary>
        const string DrawAxis = "a";
        /// <summary>Draws left (for Y-axis) edge of frame.</summary>
        const string DrawLeft = "b";
        /// <summary>Draws bottom (for X-axis) edge of frame.</summary>
        const string DrawBottom = "b";
        /// <summary>Draws top (for X-axis) edge of frame.</summary>
        const string DrawTop = "c";
        /// <summary>Draws right (for Y-axis) edge of frame.</summary>
        const string DrawRight = "c";
        /// <summary>Plot labels as date / time. Values are assumed to be seconds since the epoch (as used by gmtime).</summary>
        const string DateTime = "d";
        /// <summary>Always use fixed point numeric labels. </summary>
        const string FixedPoint = "f";
        /// <summary>Draws a grid at the major tick interval. </summary>
        const string MajorGrid = "g";
        /// <summary>Draws a grid at the minor tick interval. </summary>
        const string MinorGrid = "h";
        /// <summary>Inverts tick marks, so they are drawn outwards, rather than inwards. </summary>
        const string InvertTickMarks = "i";
        /// <summary>Labels axis logarithmically. This only affects the labels, not the data, and so it is necessary to compute the logarithms of data points before passing them to any of the drawing routines.</summary>
        const string Log = "l";
        /// <summary>Writes numeric labels at major tick intervals in the unconventional location (above box for X, right of box for Y).</summary>
        const string UnconventionalLabels = "m";
        /// <summary>Writes numeric labels at major tick intervals in the conventional location (below box for X, left of box for Y).</summary>
        const string ConventionalLabels = "n";
        /// <summary>Use custom labelling function to generate axis label text. The custom labelling function can be defined with the plslabelfunc command.</summary>
        const string CustomLabels = "o";
        /// <summary>Enables subticks between major ticks, only valid if t is also specified. </summary>
        const string SubTicks = "s";
        /// <summary>Draws major ticks. </summary>
        const string MajorTicks = "t";
        /// <summary>Draws left (for Y-axis) edge of frame without edge line.</summary>
        const string DrawLeftWithoutEdgeLine = "u";
        /// <summary>Draws bottom (for X-axis) edge of frame without edge line.</summary>
        const string DrawBottomWithoutEdgeLine = "u";
        /// <summary>Draws top (for X-axis) edge of frame without edge line.</summary>
        const string DrawTopWithoutEdgeLine = "w";
        /// <summary>Draws right (for Y-axis) edge of frame without edge line.</summary>
        const string DrawRightWithoutEdgeLine = "w";
        /// <summary>Draws major ticks (including the side effect of the numerical labels for the major ticks) except exclude drawing the major and minor tick marks.</summary>
        const string MajorTicksWithoutMarks = "x";
        /// <summary>Write numeric labels for the y axis parallel to the base of the graph, rather than parallel to the axis.</summary>
        const string BaseParallelLabels = "v";        
    }

    /// <summary>Options for an Z-axis.</summary>
    public static class ZAxisOpt
    {
        /// <summary>No options.</summary>
        const string None = "";
        /// <summary>Draws z axis to the left of the surface plot. </summary>
        const string DrawLeft = "b";
        /// <summary>Draws z axis to the right of the surface plot. </summary>
        const string DrawRight = "c";
        /// <summary>Draws grid lines parallel to the x-y plane behind the figure. These lines are not drawn until after plot3d or plmesh are called because of the need for hidden line removal.</summary>
        const string Grid = "d";
        /// <summary>Plot labels as date / time. Values are assumed to be seconds since the epoch (as used by gmtime).</summary>
        const string DateTime = "e";        
        /// <summary>Always use fixed point numeric labels. </summary>
        const string FixedPoint = "f";
        /// <summary>Inverts tick marks, so they are drawn outwards, rather than inwards. </summary>
        const string InvertTickMarks = "i";
        /// <summary>Labels axis logarithmically. This only affects the labels, not the data, and so it is necessary to compute the logarithms of data points before passing them to any of the drawing routines.</summary>
        const string Log = "l";
        /// <summary>Writes numeric labels at major tick intervals on the right-hand z axis.</summary>
        const string RightHandLabels = "m";
        /// <summary>Writes numeric labels at major tick intervals on the left-hand z axis.</summary>
        const string LeftHandLabels = "n";
        /// <summary>Use custom labelling function to generate axis label text. The custom labelling function can be defined with the plslabelfunc command.</summary>
        const string CustomLabels = "o";
        /// <summary>Enables subticks between major ticks, only valid if t is also specified. </summary>
        const string SubTicks = "s";
        /// <summary>Draws major ticks. </summary>
        const string MajorTicks = "t";
        /// <summary>If this is specified, the text label is written beside the left-hand axis.</summary>
        const string LabelLeft = "u";
        /// <summary>If this is specified, the text label is written beside the right-hand axis.</summary>
        const string LabelRight = "v";        
    }    

    /// <summary>The side of the viewport along which the text is to be written.</summary>
    public static class Side
    {
        /// <summary>Bottom of viewport, text written parallel to edge.</summary>
        const string BottomParallel = "b";
        /// <summary>Bottom of viewport, text written at right angles to edge.</summary>
        const string BottomPerpendicular = "bv";
        /// <summary>Left of viewport, text written parallel to edge.</summary>
        const string LeftParallel = "l";
        /// <summary>Left of viewport, text written at right angles to edge.</summary>
        const string LeftPerpendicular = "lv";        
        /// <summary>Right of viewport, text written parallel to edge.</summary>
        const string RightParallel = "r";
        /// <summary>Right of viewport, text written at right angles to edge.</summary>
        const string RightPerpendicular = "rv";
        /// <summary>Top of viewport, text written parallel to edge.</summary>
        const string TopParallel = "t";
        /// <summary>Top of viewport, text written at right angles to edge.</summary>
        const string TopPerpendicular = "tv";
    }

    /// <summary>The side of the viewport along which the text is to be written.</summary>
    public static class Side3
    {
        /// <summary>Label the X axis.</summary>
        const string XAxis = "x";
        /// <summary>Label the Y axis.</summary>
        const string YAxis = "y";
        /// <summary>Label the Z axis.</summary>
        const string ZAxis = "z";
        /// <summary>Label the “primary” axis. For Z this is the leftmost Z axis. For X it is the axis that starts at y-min. For Y it is the axis that starts at x-min.</summary>
        const string PrimaryAxis = "p";        
        /// <summary>Label the “secondary” axis.</summary>
        const string SecondaryAxis = "s";
        /// <summary>Draw the text perpendicular to the axis.</summary>
        const string Perpendicular = "v";
    }    

    /// <summary>Font characterization integer flags and masks.</summary>
    [Flags]
    public enum FCI : PLUNICODE
    {
        /// <summary>Mark (must always be set for FCI)</summary>
        Mark = 0x80000000,
        /// <summary>Mask for font family</summary>
        FamilyMask = 0x00f,
        /// <summary>Flag for sans-serif font family</summary>
        FamilySans = 0x000,
        /// <summary>Flag for serif font family</summary>
        FamilySerif = 0x001,
        /// <summary>Flag for monospaced font family</summary>
        FamilyMono = 0x002,
        /// <summary>Flag for script font family</summary>
        FamilyScript = 0x003,
        /// <summary>Flag for symbol font family</summary>
        FamilySymbol = 0x004,
        /// <summary>Mask for font style</summary>
        StyleMask = 0x0f0,
        /// <summary>Flag for upright font style</summary>
        StyleUpright = 0x000,
        /// <summary>Flag for italic font style</summary>
        StyleItalic = 0x010,
        /// <summary>Flag for olique font style</summary>
        StyleOblique = 0x020,
        /// <summary>Mask for font weight</summary>
        WeightMask = 0xf00,
        /// <summary>Flag for medium font weight</summary>
        WeightMedium = 0x000,
        /// <summary>Flag for bold font weight</summary>
        WeightBold = 0x100,
    }

    /// <summary>Font flag</summary>
    public enum FontFlag : PLINT
    {
        /// <summary>Sans-serif font</summary>
        Sans = 1,
        /// <summary>Serif font</summary>
        Serif = 2,
        /// <summary>Italic font</summary>
        Italic = 3,
        /// <summary>Script font</summary>
        Script = 4
    }

    /// <summary>Font family</summary>
    public enum FontFamily : PLINT
    {
        /// <summary>Sans-serif</summary>
        Sans = 0,
        /// <summary>Serif</summary>
        Serif = 1,
        /// <summary>Monospaced</summary>
        Mono = 2,
        /// <summary>Script</summary>
        Script = 3,
        /// <summary>Symbol</summary>
        Symbol = 4
    }

    /// <summary>Font style</summary>
    public enum FontStyle : PLINT
    {
        /// <summary>Upright</summary>
        Upright = 0,
        /// <summary>Italic</summary>
        Italic = 1,
        /// <summary>Olique</summary>
        Oblique = 2
    }

    /// <summary>Font weight</summary>
    public enum FontWeight : PLINT
    {
        /// <summary>Medium</summary>
        Medium = 0,
        /// <summary>Bold</summary>
        Bold = 1
    }

    /// <summary>Option flags for plbin()</summary>
    [Flags]
    public enum Bin : PLINT
    {
        /// <summary>The x represent the lower bin boundaries, the outer bins are expanded to fill up the entire x-axis and bins of zero height are simply drawn.</summary>
        Default = 0x0,
        /// <summary>The bin boundaries are to be midway between the x values. If the values in x are equally spaced, the values are the center values of the bins.</summary>
        Centred = 0x1,
        /// <summary>The outer bins are drawn with equal size as the ones inside.</summary>
        NoExpand = 0x2,
        /// <summary>Bins with zero height are not drawn (there is a gap for such bins).</summary>
        NoEmpty = 0x4
    }

    /// <summary>Position flags for legend and colorbar.</summary>
    [Flags]
    public enum Position : PLINT
    {
        /// <summary>Left</summary>
        Left = 0x1,
        /// <summary>Right</summary>
        Right = 0x2,
        /// <summary>Top</summary>
        Top = 0x4,
        /// <summary>Bottom</summary>
        Bottom = 0x8,
        /// <summary>Inside plot</summary>
        Inside = 0x10,
        /// <summary>Outside plot</summary>
        Outside = 0x20,
        /// <summary>The adopted coordinates are normalized viewport coordinates.</summary>
        Viewport = 0x40,
        /// <summary>The adopted coordinates are normalized subpage coordinates.</summary>
        Subpage = 0x80
    }

    /// <summary>Option flags for a legend.</summary>
    [Flags]
    public enum Legend : PLINT
    {
        /// <summary>No flags</summary>
        None = 0x00,
        /// <summary>If set, put the text area on the left of the legend and the plotted area on the right. Otherwise, put the text area on the right of the legend and the plotted area on the left.</summary>
        TextLeft = 0x10,
        /// <summary>Plot a (semitransparent) background for the legend.</summary>
        Background = 0x20,
        /// <summary>Plot a bounding box for the legend</summary>
        BoundingBox = 0x40,
        /// <summary>If set, plot the resulting array of legend entries in row-major order. Otherwise, plot the legend entries in column-major order.</summary>
        RowMajor = 0x80,
    }

    /// <summary>Option flags for a legend entry.</summary>
    [Flags]
    public enum LegendEntry : PLINT
    {
        /// <summary>Entry is plotted with nothing in the plotted area.</summary>
        None = 0x1,
        /// <summary>Entry is plotted with colored box.</summary>
        ColorBox = 0x2,
        /// <summary>Entry is plotted with a line.</summary>
        Line = 0x4,
        /// <summary>Entry is plotted with a symbol.</summary>
        Symbol = 0x8
    }        

    /// <summary>Option flags for colorbar.</summary>
    [Flags]
    public enum Colorbar : PLINT
    {
        /// <summary>Title is left</summary>
        TitleLeft = 0x1,
        /// <summary>Title is right</summary>
        TitleRight = 0x2,
        /// <summary>Title is at top</summary>
        TitleTop = 0x4,
        /// <summary>Title is at bottom</summary>
        TitleBottom = 0x8,
        /// <summary>Colorbar type is image</summary>
        Image = 0x10,
        /// <summary>Colorbar type is shade</summary>
        Shade = 0x20,
        /// <summary>Colorbar type is gradient</summary>
        Gradient = 0x40,
        /// <summary>No end-cap</summary>
        CapNone = 0x80,
        /// <summary>Low end-cap</summary>
        CapLow = 0x100,
        /// <summary>High end-cap</summary>
        CapHigh = 0x200,
        /// <summary>Any tick marks and tick labels will be placed at the breaks between shaded segments.</summary>
        ShadeLabel = 0x400,
        /// <summary>Direction of maximum value is right</summary>
        OrientRight = 0x800,
        /// <summary>Direction of maximum value is top</summary>
        OrientTop = 0x1000,
        /// <summary>Direction of maximum value is left</summary>
        OrientLeft = 0x2000,
        /// <summary>Direction of maximum value is bottom</summary>
        OrientBottom = 0x4000,
        /// <summary>Plot a (semitransparent) background for the color bar.</summary>
        Background = 0x8000,
        /// <summary>Plot a bounding box for the color bar.</summary>
        BoundingBox = 0x10000,
    }

    /// <summary>Option flags for colorbar label.</summary>
    public enum ColorbarLabel : PLINT
    {
        /// <summary>Label is ignored</summary>
        None = 0x0,
        /// <summary>Label is left</summary>
        Left = 0x1,
        /// <summary>Label is right</summary>
        Right = 0x2,
        /// <summary>Label is at top</summary>
        Top = 0x4,
        /// <summary>Label is at bottom</summary>
        Bottom = 0x8
    }

    /// <summary>Drawing mode</summary>
    public enum DrawMode : PLINT
    {
        /// <summary>Unknown</summary>
        Unknown = 0x0,
        /// <summary>Default</summary>
        Default = 0x1,
        /// <summary>Replace</summary>
        Replace = 0x2,
        /// <summary>Xor</summary>
        Xor = 0x4
    }

    /// <summary>Axis label tags</summary>
    public enum Axis : PLINT
    {
        /// <summary>X axis</summary>
        X = 1,
        /// <summary>Y axis</summary>
        Y = 2,
        /// <summary>Z axis</summary>
        Z = 3
    }

    /// <summary>Controls how the axes will be scaled</summary>
    public enum AxesScale : PLINT
    {
        /// <summary>The scales will not be set, the user must set up the scale before calling plenv using plsvpa, plvasp or other.</summary>
        NotSet = -1,
        /// <summary>The x and y axes are scaled independently to use as much of the screen as possible.</summary>
        Independent = 0,
        /// <summary>The scales of the x and y axes are made equal.</summary>
        Equal = 1,
        /// <summary>The axis of the x and y axes are made equal, and the plot box will be square.</summary>
        Square = 2
    }

    /// <summary>Controls drawing of the box around the plot:</summary>
    public enum AxisBox : PLINT
    {
        /// <summary>Draw no box, no tick marks, no numeric tick labels, no axes.</summary>
        Off = -2,
        /// <summary>Draw box only.</summary>
        Box = -1,
        /// <summary>Draw box, ticks, and numeric tick labels.</summary>
        BoxTicksLabels = 0,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw coordinate axes at x=0 and y=0.</summary>
        BoxTicksLabelsAxes = 1,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at major tick positions in both coordinates.</summary>
        BoxTicksLabelsAxesMajorGrid = 2,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at minor tick positions in both coordinates.</summary>
        BoxTicksLabelsAxesMinorGrid = 3,
        /// <summary>Draw box, ticks, and numeric tick labels with logarithmic x tick marks.</summary>
        LogXBoxTicksLabels = 10,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw coordinate axes at x=0 and y=0 with logarithmic x tick marks </summary>
        LogXBoxTicksLabelsAxes = 11,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at major tick positions in both coordinates with logarithmic x tick marks.</summary>
        LogXBoxTicksLabelsAxesMajorGrid = 12,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at minor tick positions in both coordinates with logarithmic x tick marks.</summary>
        LogXBoxTicksLabelsAxesMinorGrid = 13,
        /// <summary>Draw box, ticks, and numeric tick labels with logarithmic y tick marks.</summary>
        LogYBoxTicksLabels = 20,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw coordinate axes at x=0 and y=0 with logarithmic y tick marks </summary>
        LogYBoxTicksLabelsAxes = 21,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at major tick positions in both coordinates with logarithmic y tick marks.</summary>
        LogYBoxTicksLabelsAxesMajorGrid = 22,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at minor tick positions in both coordinates with logarithmic y tick marks.</summary>
        LogYBoxTicksLabelsAxesMinorGrid = 23,
        /// <summary>Draw box, ticks, and numeric tick labels with logarithmic x and y tick marks.</summary>
        LogXYBoxTicksLabels = 30,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw coordinate axes at x=0 and y=0 with logarithmic x and y tick marks </summary>
        LogXYBoxTicksLabelsAxes = 31,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at major tick positions in both coordinates with logarithmic x and y tick marks.</summary>
        LogXYBoxTicksLabelsAxesMajorGrid = 32,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at minor tick positions in both coordinates with logarithmic x and y tick marks.</summary>
        LogXYBoxTicksLabelsAxesMinorGrid = 33,
        /// <summary>Draw box, ticks, and numeric tick labels with date/time x labels.</summary>
        DateTimeXBoxTicksLabels = 40,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw coordinate axes at x=0 and y=0 with date/time x labels </summary>
        DateTimeXBoxTicksLabelsAxes = 41,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at major tick positions in both coordinates with date/time x labels.</summary>
        DateTimeXBoxTicksLabelsAxesMajorGrid = 42,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at minor tick positions in both coordinates with date/time x labels.</summary>
        DateTimeXBoxTicksLabelsAxesMinorGrid = 43,
        /// <summary>Draw box, ticks, and numeric tick labels with date/time y labels.</summary>
        DateTimeYBoxTicksLabels = 50,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw coordinate axes at x=0 and y=0 with date/time y labels </summary>
        DateTimeYBoxTicksLabelsAxes = 51,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at major tick positions in both coordinates with date/time y labels.</summary>
        DateTimeYBoxTicksLabelsAxesMajorGrid = 52,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at minor tick positions in both coordinates with date/time y labels.</summary>
        DateTimeYBoxTicksLabelsAxesMinorGrid = 53,
        /// <summary>Draw box, ticks, and numeric tick labels with date/time x and y labels.</summary>
        DateTimeXYBoxTicksLabels = 60,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw coordinate axes at x=0 and y=0 with date/time x and y labels </summary>
        DateTimeXYBoxTicksLabelsAxes = 61,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at major tick positions in both coordinates with date/time x and y labels.</summary>
        DateTimeXYBoxTicksLabelsAxesMajorGrid = 62,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at minor tick positions in both coordinates with date/time x and y labels.</summary>
        DateTimeXYBoxTicksLabelsAxesMinorGrid = 63,
        /// <summary>Draw box, ticks, and numeric tick labels with custom x and y labels.</summary>
        CustomXYBoxTicksLabels = 70,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw coordinate axes at x=0 and y=0 with custom x and y labels </summary>
        CustomXYBoxTicksLabelsAxes = 71,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at major tick positions in both coordinates with custom x and y labels.</summary>
        CustomXYBoxTicksLabelsAxesMajorGrid = 72,
        /// <summary>Draw box, ticks, and numeric tick labels, also draw a grid at minor tick positions in both coordinates with custom x and y labels.</summary>
        CustomXYBoxTicksLabelsAxesMinorGrid = 73
    }

    /// <summary>Type of gridding algorithm for plgriddata()</summary>
    public enum Grid : PLINT
    {
        /// <summary>Bivariate Cubic Spline approximation</summary>
        CSA = 1,  
        /// <summary>Delaunay Triangulation Linear Interpolation</summary>
        DTLI = 2, 
        /// <summary>Natural Neighbors Interpolation</summary>
        NNI = 3,  
        /// <summary>Nearest Neighbors Inverse Distance Weighted</summary>
        NNIDW = 4, 
        /// <summary>Nearest Neighbors Linear Interpolation</summary>
        NNLI = 5,
        /// <summary>Nearest Neighbors Around Inverse Distance Weighted</summary>
        NNAIDW = 6
    }

    /// <summary>Line style</summary>
    public enum LineStyle : PLINT
    {
        /// <summary>Continous line</summary>
        Continuous = 1,
        /// <summary>short dashes followed by short gaps</summary>
        ShortDashesShortGaps = 2,
        /// <summary>long dashes followed by long gaps</summary>
        LongDashesLongGaps = 3,
        /// <summary>long dashes followed by short gaps</summary>
        LongDashesShortGaps = 4,
        /// <summary>short dashes followed by long gaps</summary>
        ShortDashesLongGaps = 5, 
        /// <summary>line style 6</summary>
        Style6 = 6, 
        /// <summary>line style 7</summary>
        Style7 = 7, 
        /// <summary>line style 8</summary>
        Style8 = 8  
    }

    /// <summary>Option flags for histogram</summary>
    [Flags]
    public enum Hist
    {
        /// <summary>The axes are automatically rescaled to fit the histogram data, the outer bins are expanded to fill up the entire x-axis, data outside the given extremes are assigned to the outer bins and bins of zero height are simply drawn.</summary>
        Default = 0x00,
        /// <summary>The existing axes are not rescaled to fit the histogram data, without this flag, plenv is called to set the world coordinates.</summary>
        NoScaling = 0x01,
        /// <summary>Data outside the given extremes are not taken into account. This option should probably be combined with opt=PL_HIST_NOEXPAND|..., so as to properly present the data. </summary>
        IgnoreOutliers = 0x02,
        /// <summary>The outer bins are drawn with equal size as the ones inside.</summary>
        NoExpand = 0x08,
        /// <summary>Bins with zero height are not drawn (there is a gap for such bins).</summary>
        NoEmpty = 0x10
    }

    /// <summary>Determines the way in which the surface is represented</summary>
    public enum Mesh : PLINT
    {
        /// <summary>Lines are drawn showing z as a function of x for each value of y[j].</summary>
        LineX = 0x001,  
        /// <summary>Lines are drawn showing z as a function of y for each value of x[i].</summary>
        LineY = 0x002,  
        /// <summary>Network of lines is drawn connecting points at which function is defined.</summary>
        LineXY = 0x003
    }

    /// <summary>Determines the way in which the surface is represented.</summary>
    [Flags]
    public enum MeshContour : PLINT
    {
        /// <summary>Lines are drawn showing z as a function of x for each value of y[j].</summary>
        LineX = 0x001,  
        /// <summary>Lines are drawn showing z as a function of y for each value of x[i].</summary>
        LineY = 0x002,  
        /// <summary>Network of lines is drawn connecting points at which function is defined.</summary>
        LineXY = 0x003,
        /// <summary>Each line in the mesh is colored according to the z value being plotted. The color is used from the current cmap1.</summary>
        MagColor = 0x004,  
        /// <summary>A contour plot is drawn at the base XY plane using parameters nlevel and clevel. </summary>
        BaseCont = 0x008,  
        /// <summary>Draws a curtain between the base XY plane and the borders of the plotted function.</summary>
        DrawSides = 0x040
    }

    /// <summary>Determines the way in which the surface is represented.</summary>
    [Flags]
    public enum Surf : PLINT
    {
        /// <summary>The surface is colored according to the value of Z; if MAG_COLOR is not used, then the surface is colored according to the intensity of the reflected light in the surface from a light source whose position is set using pllightsource.</summary>
        MagColor = 0x004,  
        /// <summary>A contour plot is drawn at the base XY plane using parameters nlevel and clevel.</summary>
        BaseCont = 0x008,  
        /// <summary>A contour plot is drawn at the surface plane using parameters nlevel and clevel.</summary>
        SurfCont = 0x020,  
        /// <summary>draws a curtain between the base XY plane and the borders of the plotted function. </summary>
        DrawSides = 0x040, 
        /// <summary>Network of lines is drawn connecting points at which function is defined.</summary>
        Faceted = 0x080
    }

    /// <summary>standard fill patterns</summary>
    public enum Pattern : PLINT
    {
        /// <summary>horizontal lines</summary>
        Horizontal = 1,
        /// <summary>vertical lines</summary>
        Vertical = 2,
        /// <summary>lines at 45 degrees</summary>
        Deg45 = 3,
        /// <summary>lines at -45 degrees</summary>
        DegMinus45 = 4,
        /// <summary>lines at 30 degrees</summary>
        Deg30 = 5,
        /// <summary>lines at -30 degrees</summary>
        DegMinus30 = 6,
        /// <summary>both vertical and horizontal lines</summary>
        HorizontalAndVertical = 7,
        /// <summary>lines at both 45 degrees and -45 degrees</summary>
        Deg45AndMinus45 = 8
    }

    /// <summary>Page orientation</summary>
    public enum Orientation : PLINT
    {
        /// <summary>landscape</summary>
        Landscape = 0,
        /// <summary>portrait</summary>
        Portrait = 1,
        /// <summary>upsidedown landscape</summary>
        FlippedLandscape = 2,
        /// <summary>upsidedown portrait</summary>
        FlippedPortrait = 3
    }

    /// <summary>Pen for strip chart</summary>
    public enum Pen : PLINT
    {
        /// <summary>Pen 0</summary>
        Pen0 = 0,
        /// <summary>Pen 1</summary>
        Pen1 = 1,
        /// <summary>Pen 2</summary>
        Pen2 = 2,
        /// <summary>Pen 3</summary>
        Pen3 = 3
    }

    /// <summary>Color space</summary>
    public enum ColorSpace : PLINT
    {
        /// <summary>HLS color space</summary>
        HLS = 0,
        /// <summary>RGB color space</summary>
        RGB = 1
    }

    /// <summary>Graphics input event state flags</summary>
    [Flags]
    public enum KeyButtonMask : uint
    {
        /// <summary>shift key</summary>
        Shift = 0x1,
        /// <summary>caps lock</summary>
        Caps = 0x2,
        /// <summary>control key</summary>
        Control = 0x4,
        /// <summary>alt key</summary>
        Alt = 0x8,
        /// <summary>num lock</summary>
        Num = 0x10,
        /// <summary>alt gr key</summary>
        AltGr = 0x20, 
        /// <summary>windows key</summary>
        Win = 0x40,
        /// <summary>scroll lock</summary>
        Scroll = 0x80,
        /// <summary>mouse button 1</summary>
        Button1 = 0x100,
        /// <summary>mouse button 2</summary>
        Button2 = 0x200,
        /// <summary>mouse button 3</summary>
        Button3 = 0x400, 
        /// <summary>mouse button 4</summary>
        Button4 = 0x800, 
        /// <summary>mouse button 5</summary>
        Button5 = 0x1000 
    }

    /// <summary>PLplot Graphics Input structure</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GraphicsIn
    {
        /// <summary>event type (unused)</summary>
        int Type;       
        /// <summary>key and button mask</summary>
        KeyButtonMask State;     
        /// <summary>key selected</summary>
        uint KeySym;    
        /// <summary>mouse button selected</summary>
        uint Button;    
        /// <summary>Subwindow (or subpage / subplot) number.</summary>
        PLINT SubWindow;
        /// <summary>Translated ascii character string.</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        string Str;  
        /// <summary>Absolute X device coordinate of mouse pointer.</summary>
        int PX;
        /// <summary>Absolute Y device coordinate of mouse pointer.</summary>
        int PY;      
        /// <summary>Relative X device coordinate of mouse pointer. </summary>
        PLFLT DX;
        /// <summary>Relative Y device coordinate of mouse pointer. </summary>
        PLFLT DY;    
        /// <summary>World X coordinate of mouse pointer.</summary>
        PLFLT WX;
        /// <summary>World Y coordinate of mouse pointer.</summary>
        PLFLT WY;    
    }

    /// <summary>A user supplied function to transform the original map data coordinates to a new coordinate system.</summary>
    /// <param name="n">Number of elements in the x and y vectors.</param>
    /// <param name="x">original X coordinates that should be replaced by the corresponding plot X coordinates</param>
    /// <param name="y">original Y coordinates that should be replaced by the corresponding plot Y coordinates</param>
    public delegate void MapFunc(PLINT n,
                                    [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] PLFLT[] x,
                                    [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] PLFLT[] y);

    /// <summary>A callback function that defines the transformation between the zero-based indices of the matrix f and the world coordinates.</summary>
    /// <param name="x">x-position to be transformed.</param>
    /// <param name="y">y-position to be transformed.</param>
    /// <param name="xp">transformed x-position</param>
    /// <param name="yp">transformed y-position</param>
    /// <param name="data">Generic pointer to additional input data that may be required by the callback routine in order to implement the transformation.</param>
    public delegate void TransformFunc(PLFLT x, PLFLT y, out PLFLT xp, out PLFLT yp, PLPointer data);

    /// <summary>Custom label generation function</summary>
    /// <param name="axis">This indicates which axis a label is being requested for.</param>
    /// <param name="value">This is the value along the axis which is being labelled.</param>
    /// <param name="label">generated label</param>
    /// <param name="length">maximum length of generated laben</param>
    /// <param name="data">Generic pointer to additional input data that may be required by the callback routine in order to generate label.</param>
    public delegate void LabelFunc(Axis axis, PLFLT value,
                                    [MarshalAs(UnmanagedType.LPStr, SizeParamIndex=3)] out string label,
                                    PLINT length, PLPointer data);

    /// <summary>A callback function that fills a region.</summary>
    /// <param name="n">Number of elements in the x and y vectors.</param>
    /// <param name="x">Vector of x-coordinates of vertices.</param>
    /// <param name="y">Vector of y-coordinates of vertices.</param>
    public delegate void FillFunc(PLINT n,
                                    [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] PLFLT[] x,
                                    [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] PLFLT[] y);

    /// <summary>Callback function specifying the region that should be plotted in the shade plot</summary>
    /// <param name="x">x-coordinate to be tested for whether it is in the defined region.</param>
    /// <param name="y">y-coordinate to be tested for whether it is in the defined region.</param>
    /// <return>true if within defined area, otherwise false.</return>
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate PLBOOL DefinedFunc(PLFLT x, PLFLT y);

    // event handler delegates
    public delegate void KeyHandler([In] GraphicsIn evnt, PLPointer data, [MarshalAs(UnmanagedType.Bool)] out PLBOOL exit_eventloop);
    public delegate void ButtonHandler([In] GraphicsIn evnt, PLPointer data, [MarshalAs(UnmanagedType.Bool)] out PLBOOL exit_eventloop);
    public delegate void LocateHandler([In] GraphicsIn evnt, PLPointer data, [MarshalAs(UnmanagedType.Bool)] out PLBOOL locate_mode);
    public delegate void BopHandler(PLPointer bop_data, [MarshalAs(UnmanagedType.Bool)] out PLBOOL skip_driver_bop);
    public delegate void EopHandler(PLPointer eop_data, [MarshalAs(UnmanagedType.Bool)] out PLBOOL skip_driver_eop);
    public delegate int ExitHandler([In, MarshalAs(UnmanagedType.LPStr)] string msg);
    public delegate void AbortHandler([In, MarshalAs(UnmanagedType.LPStr)] string msg);

}
