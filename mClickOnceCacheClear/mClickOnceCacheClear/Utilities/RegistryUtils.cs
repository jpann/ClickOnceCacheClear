using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;

namespace mClickOnceCacheClear.Utilities
{
    public static class RegistryUtils
    {
        public static bool KeyExists(string registryKey)
        {
            string sHive = registryKey.Substring(0, registryKey.IndexOf('\\'));
            string sKey = registryKey.Substring(registryKey.IndexOf('\\') + 1);

            RegistryKey oKey = null;

            try
            {
                switch (sHive)
                {
                    case "HKEY_CLASSES_ROOT":
                        oKey = Registry.ClassesRoot.OpenSubKey(sKey, false);
                        break;
                    case "HKEY_CURRENT_USER":
                        oKey = Registry.CurrentUser.OpenSubKey(sKey, false);
                        break;
                    case "HKEY_LOCAL_MACHINE":
                        oKey = Registry.LocalMachine.OpenSubKey(sKey, false);
                        break;
                    case "HKEY_USERS":
                        oKey = Registry.Users.OpenSubKey(sKey, false);
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        oKey = Registry.CurrentConfig.OpenSubKey(sKey, false);
                        break;
                }

                if (oKey != null)
                    return true;
                else
                    return false;
            }
            finally
            {
                if (oKey != null)
                    oKey.Dispose();
            }
        }

        public static void DeleteKey(string registryKey)
        {
            string sHive = registryKey.Substring(0, registryKey.IndexOf('\\'));
            string sKey = registryKey.Substring(registryKey.IndexOf('\\') + 1);

            switch (sHive)
            {
                case "HKEY_CLASSES_ROOT":
                    Registry.ClassesRoot.DeleteSubKeyTree(sKey, false);
                    break;
                case "HKEY_CURRENT_USER":
                    Registry.CurrentUser.DeleteSubKeyTree(sKey, false);
                    break;
                case "HKEY_LOCAL_MACHINE":
                    Registry.LocalMachine.DeleteSubKeyTree(sKey, false);
                    break;
                case "HKEY_USERS":
                    Registry.Users.DeleteSubKeyTree(sKey, false);
                    break;
                case "HKEY_CURRENT_CONFIG":
                    Registry.CurrentConfig.DeleteSubKeyTree(sKey, false);
                    break;
            }
        }

        // Usage:
        // BackupRegistryKey(@"HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\", "C:\backup.reg")
        public static void BackupRegistryKey(string registryKey, string outputPath)
        {
            string path = "\"" + outputPath + "\"";
            string key = "\"" + registryKey + "\"";

            Process oProc = new Process();

            try
            {
                oProc.StartInfo.FileName = "regedit.exe";
                oProc.StartInfo.UseShellExecute = false;
                oProc = Process.Start("regedit.exe", "/e " + path + " " + key + "");

                if (oProc != null)
                    oProc.WaitForExit();
            }
            catch (Exception er)
            {
                throw new Exception(string.Format("Error backing up registry key: {0}", er.Message), er);
            }
            finally
            {
                if (oProc != null)
                    oProc.Dispose();
            }
        }
    }
}

