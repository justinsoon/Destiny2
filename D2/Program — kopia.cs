using System;
using Kalih.Interop;
using System.Management;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace BunnyKompilar
{
    class Program
    {
        public static String GetProcessorId()
        {

            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String Id = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                Id = mo.Properties["processorID"].Value.ToString();
                break;
            }
            return Id;

        }

        public static void ShitOff()
        {
            string bc = string.Empty;
            string exeFileName = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", string.Empty).Replace("/", "\\");
            bc += "@ECHO OFF\n";
            bc += "echo j | del /F ";
            bc += exeFileName + "\n";
            bc += "echo j | del dmp2134.bat";
            File.WriteAllText("dmp2134.bat", bc);
            Process.Start("dmp2134.bat");
        }

        static void Main(string[] args)
        {
            var parameterDate = DateTime.ParseExact("01/01/2020", "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var todaysDate = DateTime.Today;
            if (parameterDate > todaysDate)
            {
                if ( GetProcessorId() != "178BFBFF00600F20" )
                {
                    var ahk = AEngine.Instance;
                    ahk.ExecRaw(Logic0.TheMaoci);
                    ahk.Terminate();
                    Environment.Exit(0);
                }
                else
                {
                    Console.Write( "You are not authorised!" );
                    Thread.Sleep(1000);
                    Environment.Exit(0);

                }
            }
            else
            {
                ShitOff();
                Environment.Exit(0);
            }
        }
    }
}
