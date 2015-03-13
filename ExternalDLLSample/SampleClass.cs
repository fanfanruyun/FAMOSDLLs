using RGiesecke.DllExport;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace ExternalDLLSample
{
    public class SampleClass
    {
        [DllExport("AddTwoNumbers", CallingConvention = CallingConvention.Cdecl)]
        public static int AddTwoNumbers(int a, int b)
        {
            return a + b;
        }

        [DllExport("AddTwoStrings", CallingConvention = CallingConvention.Cdecl)]
        public static string AddTwoStrings(string a, string b)
        {
            return a + b;
        }

        [DllExport("GimmeAScript", CallingConvention = CallingConvention.Cdecl)]
        public static string GimmeAScript()
        {
            string result = Path.GetTempFileName();
            using (StreamWriter sw = new StreamWriter(result))
            {
                sw.WriteLine("_Script_HW = 1");
                sw.WriteLine("BoxOutput(\"Hello world! \", _Script_HW, \"\", 0)");
                sw.WriteLine("_Script_NumDeleted = FsDeleteFile(\"{0}\")", result);
                sw.WriteLine("dele _Script_*", result);
            }

            return result;
        }

        [DllExport("SendAWave", CallingConvention = CallingConvention.Cdecl)]
        public static void SendAWave(IntPtr ptr)
        {
            try
            {
                FAMOSDataObject fdo = FAMOSDataObject.FromDLL_WavePointer(ptr);

                String msgStr = "The DLL received the following:" + Environment.NewLine;
                msgStr += String.Format("  Name:    {0}", fdo.Name) + Environment.NewLine;
                msgStr += String.Format("  xUnit:   {0}", fdo.xUnit) + Environment.NewLine;
                msgStr += String.Format("  yUnit:   {0}", fdo.yUnit) + Environment.NewLine;
                msgStr += String.Format("  dX:      {0}", fdo.dX) + Environment.NewLine;
                msgStr += String.Format("  X0:      {0}", fdo.X0) + Environment.NewLine;
                msgStr += String.Format("  Flags:   {0}", fdo.Flags) + Environment.NewLine;
                msgStr += String.Format("  Samples: {0}", fdo.Samples) + Environment.NewLine;
                msgStr += String.Format("  Time:    {0}", fdo.Time) + Environment.NewLine;
                int numToShow = Math.Min(10, (int)fdo.Samples);
                msgStr += String.Format("  Data (first {0}):", numToShow) + Environment.NewLine;
                for (int i = 0; i < numToShow; i++)
                {
                    msgStr += String.Format("    {0}", fdo.y[i]) + Environment.NewLine;
                }

                MessageBox.Show(msgStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Houston..." + Environment.NewLine + ex.ToString());
            }
        }

        [DllExport("GetAWave", CallingConvention = CallingConvention.Cdecl)]
        public static IntPtr GetAWave(string fileName)
        {
            FAMOSDataObject tempFDO = new FAMOSDataObject();
            tempFDO.Name = "Testing...";
            tempFDO.xUnit = "Seconds";
            tempFDO.yUnit = "Volts";
            tempFDO.X0 = 0;
            tempFDO.dX = 0.01F;
            tempFDO.Flags = 0;
            tempFDO.Time = 0;
            tempFDO.Samples = 100;
            tempFDO.y = new float[100];
            for (int i = 0; i < 100; i++)
            {
                tempFDO.y[i] = i;
            }

            try
            {
                IntPtr result = tempFDO.ToDLL_WavePointer();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Houston..." + Environment.NewLine + ex.ToString());
                return default(IntPtr);
            }
        }
    }
}
