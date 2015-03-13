using System;
using System.Runtime.InteropServices;

namespace ExternalDLLSample
{
    public class FAMOSDataObject
    {
        #region Properties
        public string Name { get; set; }
        public string xUnit { get; set; }
        public string yUnit { get; set; }
        public Single dX { get; set; }
        public Single X0 { get; set; }
        public ushort Flags { get; set; }
        public uint Samples { get; set; }
        public uint Time { get; set; }
        public Single[] y { get; set; }
        #endregion

        #region Methods
        public static FAMOSDataObject FromDLL_WavePointer(IntPtr ptr)
        {
            DLL_Wave dllWave = (DLL_Wave)Marshal.PtrToStructure(ptr, typeof(DLL_Wave));
            FAMOSDataObject result = new FAMOSDataObject();

            result.xUnit = TrimNullTerminatedString(dllWave.xUnit);
            result.yUnit = TrimNullTerminatedString(dllWave.yUnit);
            result.Name = TrimNullTerminatedString(dllWave.Name);
            result.dX = dllWave.dX;
            result.X0 = dllWave.X0;
            result.Flags = dllWave.Flags;
            result.Samples = dllWave.Samples;
            result.Time = dllWave.Time;

            var newPtr = new IntPtr(ptr.ToInt32() + 56); // move to first
            result.y = new Single[dllWave.Samples];
            Marshal.Copy(newPtr, result.y, 0, result.y.Length);

            return result;
        }

        public IntPtr ToDLL_WavePointer()
        {
            DLL_Wave result = new DLL_Wave();

            result.Name = StringToCCharArray(this.Name, DLL_Wave.NAME_STRING_LENGTH);
            result.xUnit = StringToCCharArray(this.xUnit, DLL_Wave.UNIT_STRING_LENGTH);
            result.yUnit = StringToCCharArray(this.yUnit, DLL_Wave.UNIT_STRING_LENGTH);
            result.X0 = this.X0;
            result.dX = this.dX;
            result.Flags = this.Flags;
            result.Time = this.Time;
            result.Samples = this.Samples;
            result.y = new float[this.Samples];
            for (int i = 0; i < this.Samples; i++)
            {
                result.y[i] = this.y[i];
            }

            //Allocate managed structure
            int bytesToAllocate = (int)result.Samples * 4 + 56;
            IntPtr ptr = Marshal.AllocHGlobal(bytesToAllocate); // Marshal.SizeOf(result));
            //Marshal the structure
            Marshal.StructureToPtr(result, ptr, false);

            var newPtr = new IntPtr(ptr.ToInt32() + 56); // move to first
            Marshal.Copy(result.y, 0, newPtr, result.y.Length);

            return ptr;
        }

        private static string TrimNullTerminatedString(char[] inputChars)
        {
            string tmpStr = new string(inputChars);
            int pos = tmpStr.IndexOf('\0');
            if (pos >= 0)
                return tmpStr.Substring(0, pos);
            else
                return tmpStr;
        }

        private char[] StringToCCharArray(string inputString, int numberOfCharacters)
        {
            char[] result = inputString.Substring(0, Math.Min(numberOfCharacters - 1, inputString.Length)).PadRight(numberOfCharacters, (char)0).ToCharArray();
            return result;
        }
        #endregion
    }
}
