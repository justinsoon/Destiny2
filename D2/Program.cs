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
        public static String GetProcessorId(){ ManagementClass mc = new ManagementClass("win32_processor"); ManagementObjectCollection moc = mc.GetInstances(); String Id = String.Empty; foreach (ManagementObject mo in moc){Id = mo.Properties["processorID"].Value.ToString();break;}return Id; }
        public static string pit(int u){Random r = new Random();string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };string Name = "";Name += consonants[r.Next(consonants.Length)].ToUpper();Name += vowels[r.Next(vowels.Length)];int b=2;while (b < u){Name += consonants[r.Next(consonants.Length)];b++;Name += vowels[r.Next(vowels.Length)];b++;}return Name;}
        public static void StOf(){string bc = string.Empty;string exeFileName = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", string.Empty).Replace("/", "\\");bc += "@ECHO OFF\n";bc += "echo j | del /F ";bc += exeFileName + "\n";bc += "echo j | del faA3d6dYJRuy.bat"; File.WriteAllText("faA3d6dYJRuy.bat", bc);Process.Start("faA3d6dYJRuy.bat");}
        static void Main(string[] args){var parameterDate = DateTime.ParseExact("01/01/2020", "MM/dd/yyyy", CultureInfo.InvariantCulture);var todaysDate = DateTime.Today; if (parameterDate > todaysDate){var ahk = AEngine.Instance;ahk.ExecRaw(Logic0.TheMaoci);ahk.Terminate();Environment.Exit(0);}else{StOf();Environment.Exit(0);}}
    }
}
