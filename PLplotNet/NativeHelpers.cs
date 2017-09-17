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


    // Marshals a PLFLT[,] as a PLFLT_MATRIX 
    internal class MatrixMarshaller : IDisposable
    {
        private GCHandle gcHnd;
        private IntPtr RowArray;
        private int nRows;
        private int nCols;

        public MatrixMarshaller (PLFLT[,] matrix)
        {
            nRows = matrix.GetLength(0);           
            nCols = matrix.GetLength(1);

            gcHnd = GCHandle.Alloc(matrix, GCHandleType.Pinned);
            IntPtr basePtr = gcHnd.AddrOfPinnedObject();

            RowArray = Marshal.AllocHGlobal(nRows * Marshal.SizeOf<IntPtr>());
            for (var row=0; row < nRows; row++)
            {
                Marshal.WriteIntPtr(RowArray, row * Marshal.SizeOf<IntPtr>(), 
                                    basePtr + row * nCols * Marshal.SizeOf<PLFLT>());
            }
        }

        void IDisposable.Dispose()
        {
            Marshal.FreeHGlobal(RowArray);
            gcHnd.Free();
        }

        public PLFLT_MATRIX Ptr {
            get { return RowArray; }
        }

        public int NX {
            get { return nRows; }
        }

        public int NY {
            get { return nCols; }
        }        
    }


    /// <summary>Native PLplot functions</summary>
    public static partial class Native
    {
        // library name
        const string DllName = "plplot";    

        const int MaxLength = 1024;

        private static void CheckRange(string param, PLINT minVal, PLINT smallerThan, PLINT value)
        {
            if (!(minVal <= value && value < smallerThan))
            {
                var msg = String.Format("Argument {0} must be between {1} and {2}, but it is {3}.",
                                        param, minVal, smallerThan, value);
                throw new ArgumentOutOfRangeException(msg);
            }
        }        

        private static PLINT GetSize(PLFLT[] x, PLFLT[] y)
        {
            if (x.Length != y.Length)
                throw new ArgumentException("argument arrays must have same length");
            return x.Length;
        }

        private static PLINT GetSize(PLINT[] x, PLINT[] y)
        {
            if (x.Length != y.Length)
                throw new ArgumentException("argument arrays must have same length");
            return x.Length;
        }        

        private static PLINT GetSize(PLFLT[] x, PLFLT[] y, PLFLT[] z)
        {
            if (x.Length != y.Length || y.Length != z.Length)
                throw new ArgumentException("argument arrays must have same length");
            return x.Length;
        }        

        private static void CheckSize(PLFLT[,] x, PLFLT[,] y)
        {
            if (x.GetLength(0) != y.GetLength(0) || x.GetLength(1) != y.GetLength(1))
                throw new ArgumentException("argument matrices must have same dimensions");
        }                     

        private static void CheckSize(PLFLT[] x, PLFLT[] y, PLFLT[,] z)
        {
            if (x.Length != y.Length || x.Length != z.GetLength(0) || y.Length != z.GetLength(1))
                throw new ArgumentException("argument arrays must have matching dimensions");
        }               

        private static PLINT GetSize(PLFLT[] x, PLFLT[] y, PLFLT[] z, PLFLT[] a)
        {
            if (x.Length != y.Length || y.Length != z.Length || z.Length != a.Length)
                throw new ArgumentException("argument arrays must have same length");
            return x.Length;
        }        

        private static PLINT GetSize(PLFLT[] x, PLFLT[] y, PLFLT[] z, PLFLT[] a, PLFLT[] b)
        {
            if (x.Length != y.Length || y.Length != z.Length || z.Length != a.Length || a.Length != b.Length)
                throw new ArgumentException("argument arrays must have same length");
            return x.Length;
        }        
        
        private static PLINT GetSize(PLINT[] x, PLINT[] y, PLINT[] z)
        {
            if (x.Length != y.Length || y.Length != z.Length)
                throw new ArgumentException("argument arrays must have same length");
            return x.Length;
        }                

        private static PLINT GetSize(PLINT[] x, PLINT[] y, PLINT[] z, PLFLT[] a)
        {
            if (x.Length != y.Length || y.Length != z.Length || z.Length != a.Length)
                throw new ArgumentException("argument arrays must have same length");
            return x.Length;
        }                

        private delegate void GetDevsFunc(ref IntPtr p_menustr, ref IntPtr p_devname, ref int p_ndev);

        private static void GetDevHelper(GetDevsFunc gd, out string[] p_menustr, out string[] p_devname)
        {
            int ndevs = 100;
            IntPtr menustrs = Marshal.AllocHGlobal(ndevs * Marshal.SizeOf<IntPtr>());
            IntPtr devnames = Marshal.AllocHGlobal(ndevs * Marshal.SizeOf<IntPtr>());
            gd(ref menustrs, ref devnames, ref ndevs);

            p_menustr = new string[ndevs];
            p_devname = new string[ndevs];
            for (int dev=0; dev < ndevs; dev++)
            {
                p_menustr[dev] = Marshal.PtrToStringAnsi(menustrs + dev * Marshal.SizeOf<IntPtr>());
                p_devname[dev] = Marshal.PtrToStringAnsi(devnames + dev * Marshal.SizeOf<IntPtr>());
            }

            Marshal.FreeHGlobal(menustrs);
            Marshal.FreeHGlobal(devnames);
        }

    }    
}
