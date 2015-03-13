using System;
using System.Runtime.InteropServices;

namespace ExternalDLLSample
{
    // This class is essentially the same as is defined in imc's examples
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DLL_Wave
    {
        public const int UNIT_STRING_LENGTH = 13;
        public const int NAME_STRING_LENGTH = 12;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = UNIT_STRING_LENGTH)]
        public char[] xUnit;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = UNIT_STRING_LENGTH)]
        public char[] yUnit;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = NAME_STRING_LENGTH)]
        public char[] Name;
        [MarshalAs(UnmanagedType.R4)]
        public Single dX;
        [MarshalAs(UnmanagedType.R4)]
        public Single X0;
        [MarshalAs(UnmanagedType.U2)]
        public ushort Flags;
        [MarshalAs(UnmanagedType.U4)]
        public uint Samples;
        [MarshalAs(UnmanagedType.U4)]
        public uint Time;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4)]
        public Single[] y;
    }
}
