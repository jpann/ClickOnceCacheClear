using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;
using log4net;
using log4net.Config;
using mClickOnceCacheClear.Events;
using mClickOnceCacheClear.Utilities;

namespace mClickOnceCacheClear
{
    public class ClickOnceCache
    {
        #region Constants
        private string[] cDefaultSearchTerms = new string[]
        {
            "prol...dll",
            "prol..vsto",
            "xcee..grid",
            "xcee..rols",
            "outl...dll",
            "outl..vsto"
        };

        private string[] cDefaultRegistryKeys = new string[] 
		{
			@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Assemblies",
			@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Categories",
			@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Components",
			@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Installations",
			@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Marks",
			@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\StateManager\Applications",
			@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\StateManager\Families",
			@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Visibility",
			@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\VisibilityRoots",
			@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\PackageMetadata"
		};

        private const string cCachePath = "\\Apps\\2.0\\";
        private const string cRegistryCachePath = @"HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\";

        private const string cClearCacheCommand = "rundll32 dfshim CleanOnlineAppCache";
        #endregion

        #region Private Variables
        private ILog mLog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool mDebug = false;
        private string[] mSearchTerms;
        private string[] mRegistryKeys;
        #endregion

        #region Public Properties
        public bool Debug
        {
            get { return this.mDebug; }
            set { this.mDebug = value; }
        }

        public string[] SearchTerms
        {
            get { return this.mSearchTerms; }
            set { this.mSearchTerms = value; }
        }

        public string[] RegistryKeys
        {
            get { return this.mRegistryKeys; }
            set { this.mRegistryKeys = value; }
        }

        public string RegistryCachePath
        {
            get { return cRegistryCachePath; }
        }
        #endregion

        #region Events
        public event DirectoryDeleteEventHandler DirectoryDelete;
        public delegate void DirectoryDeleteEventHandler(ClickOnceCache sender, DirectoryDeleteEvent e);

        public event DirectoryDeleteFailedEventHandler DirectoryDeleteFailed;
        public delegate void DirectoryDeleteFailedEventHandler(ClickOnceCache sender, DirectoryDeleteFailedEvent e);

        public event FileDeleteEventHandler FileDelete;
        public delegate void FileDeleteEventHandler(ClickOnceCache sender, FileDeleteEvent e);

        public event FileDeleteFailedEventHandler FileDeleteFailed;
        public delegate void FileDeleteFailedEventHandler(ClickOnceCache sender, FileDeleteFailedEvent e);

        public event RegistryKeyDeleteEventHandler RegistryKeyDelete;
        public delegate void RegistryKeyDeleteEventHandler(ClickOnceCache sender, RegistryKeyDeleteEvent e);

        public event RegistryKeyDeleteFailedEventHandler RegistryKeyDeleteFailed;
        public delegate void RegistryKeyDeleteFailedEventHandler(ClickOnceCache sender, RegistryKeyDeleteFailedEvent e);
        #endregion

        public ClickOnceCache()
        {
            XmlConfigurator.Configure();
        }

        public string GetCachePath()
        {
            string cachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + cCachePath;
            
            this.mLog.Debug(string.Format("Cache path: {0}", cachePath));

            return cachePath;
        }

        #region Registry Methods
        public List<string> GetRegistryKeys()
        {
            return this.GetRegistryKeys(cDefaultRegistryKeys, cDefaultSearchTerms);
        }

        public List<string> GetRegistryKeys(string[] searchTerms)
        {
            return this.GetRegistryKeys(cDefaultRegistryKeys, searchTerms);
        }

        public List<string> GetRegistryKeys(string[] registryKeys, string[] searchTerms)
        {
            List<string> oKeys = new List<string>();

            foreach (string sRegistryKey in registryKeys)
            {
                mLog.InfoFormat("Checking key '{0}'...", sRegistryKey);

                using (RegistryKey oKey = Registry.CurrentUser.OpenSubKey(sRegistryKey, false))
                {
			        // If the key doesn't exist or has a permissions issue, skip it.
                    if (oKey == null)
                    {
                        this.mLog.Warn(string.Format("Key '{0}' not found or permissions denied.", sRegistryKey));

                        continue;
                    }

                    string[] keys = oKey.GetSubKeyNames();

                    for (int i = 0; i < keys.Length; i++)
                    {
                        string sSubKeyName = keys[i];
                        string sSubKeyPath = sRegistryKey + "\\" + sSubKeyName;

                        mLog.InfoFormat("Checking sub key '{0}'...", sSubKeyPath);

                        using (RegistryKey oSubKey = Registry.CurrentUser.OpenSubKey(sSubKeyPath, false))
                        {
                            string[] subKeys = oSubKey.GetSubKeyNames();

                            for (int x = 0; x < subKeys.Length; x++)
                            {
                                foreach (string sSearchTerm in searchTerms)
                                {
                                    var regex = new Regex(sSearchTerm, RegexOptions.Compiled);

                                    if (regex.IsMatch(subKeys[x]))
                                    {
                                        mLog.InfoFormat("Match found for term '{0}' in key '{1}!", sSearchTerm, sSubKeyPath + "\\" + subKeys[x]);

                                        oKeys.Add(sSubKeyPath + "\\" + subKeys[x]);
                                    }
                                }
                            }
                        }

                        foreach (string sSearchTerm in searchTerms)
                        {
                            var regex = new Regex(sSearchTerm, RegexOptions.Compiled);
                            if (regex.IsMatch(sSubKeyName))
                            {
                                mLog.InfoFormat("Match found for term '{0}' in key '{1}!", sSearchTerm, sSubKeyPath);

                                oKeys.Add(sSubKeyPath);
                            }
                        }
                    }
                }
            }

            return oKeys;
        }

        public List<string> DeleteRegistryKeys(List<string> keys)
        {
            List<string> oFailedKeys = new List<string>();

            foreach (string sKey in keys)
            {
                try
                {
                    if (this.mDebug)
                        mLog.DebugFormat("Deleting registry key '{0}'...", sKey);

                    if (!RegistryUtils.KeyExists(sKey))
                        throw new Exception(string.Format("Registry key '{0}' does not exist.", sKey));

                    RegistryUtils.DeleteKey(sKey);

                    if (this.RegistryKeyDelete != null)
                    {
                        RegistryKeyDeleteEvent oEvent = new RegistryKeyDeleteEvent();
                        oEvent.Key = sKey;

                        this.RegistryKeyDelete(this, oEvent);
                    }
                }
                catch (Exception er)
                {
                    mLog.Error(er);

                    oFailedKeys.Add(sKey);

                    if (this.RegistryKeyDeleteFailed != null)
                    {
                        RegistryKeyDeleteFailedEvent oFailedEvent = new RegistryKeyDeleteFailedEvent();
                        oFailedEvent.Key = sKey;
                        oFailedEvent.Message = er.Message;
                        oFailedEvent.Ex = er;

                        this.RegistryKeyDeleteFailed(this, oFailedEvent);
                    }
                }
            }

            return oFailedKeys;
        }
        #endregion

        #region File Methods
        public List<string> GetFiles(string path)
        {
            return this.GetFiles(path, cDefaultSearchTerms);
        }

        public List<string> GetFiles(string path, string[] searchTerms)
        {
            List<string> oFiles = new List<string>();

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);

            foreach (string sSearchTerm in searchTerms)
            {
                mLog.InfoFormat("Checking for files in path '{0}' using term '{1}'...", path, sSearchTerm);

                var regex = new Regex(sSearchTerm, RegexOptions.Compiled);

                List<string> files = new DirectoryInfo(path)
                    .EnumerateFiles("*.*", SearchOption.AllDirectories)
                    .Where(fi => regex.IsMatch(fi.Name))
                    .Select(fi => fi.FullName)
                    .ToList<string>();

                mLog.InfoFormat("{0} files found in path matched term '{1}'.", files.Count, sSearchTerm);

                oFiles.AddRange(files);
            }

            return oFiles;
        }

        public List<string> DeleteFiles(List<string> files)
        {
            List<string> oFailedFiles = new List<string>();

            foreach (string sFile in files)
            {
                try
                {
                    if (this.mDebug)
                        mLog.DebugFormat("Deleting file '{0}'...", sFile);

                    if (!File.Exists(sFile))
                        throw new FileNotFoundException(sFile);

                    File.Delete(sFile);

                    if (this.FileDelete != null)
                    {
                        FileDeleteEvent oEvent = new FileDeleteEvent();
                        oEvent.File = sFile;

                        this.FileDelete(this, oEvent);
                    }
                }
                catch (Exception er)
                {
                    mLog.Error(er);

                    oFailedFiles.Add(sFile);

                    if (this.FileDeleteFailed != null)
                    {
                        FileDeleteFailedEvent oFailedEvent = new FileDeleteFailedEvent();
                        oFailedEvent.File = sFile;
                        oFailedEvent.Message = er.Message;
                        oFailedEvent.Ex = er;

                        this.FileDeleteFailed(this, oFailedEvent);
                    }
                }
            }

            return oFailedFiles;
        }
        #endregion

        #region Directory Methods
        public List<string> GetDirectories(string path)
        {
            return this.GetDirectories(path, cDefaultSearchTerms);
        }

        public List<string> GetDirectories(string path, string[] searchTerms)
        {
            List<string> oDirectories = new List<string>();

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);

            foreach (string sSearchTerm in searchTerms)
            {
                var regex = new Regex(sSearchTerm, RegexOptions.Compiled);

                mLog.InfoFormat("Checking for directories in path '{0}' using term '{1}'...", path, sSearchTerm);

                List<string> files = new DirectoryInfo(path)
                    .EnumerateDirectories("*.*", SearchOption.AllDirectories)
                    .Where(fi => regex.IsMatch(fi.Name))
                    .Select(fi => fi.FullName)
                    .ToList<string>();

                mLog.InfoFormat("{0} directories found in path matched term '{1}'.", files.Count, sSearchTerm);

                oDirectories.AddRange(files);
            }

            return oDirectories;
        }

        public List<string> DeleteDirectories(List<string> directories)
        {
            List<string> oFailedDirectories = new List<string>();

            foreach (string sDir in directories)
            {
                try
                {
                    if (this.mDebug)
                        mLog.DebugFormat("Deleting directory '{0}'...", sDir);

                    if (!Directory.Exists(sDir))
                        throw new DirectoryNotFoundException(sDir);

                    Directory.Delete(sDir, true);

                    if (this.DirectoryDelete != null)
                    {
                        DirectoryDeleteEvent oEvent = new DirectoryDeleteEvent();
                        oEvent.Directory = sDir;

                        this.DirectoryDelete(this, oEvent);
                    }
                }
                catch (Exception er)
                {
                    mLog.Error(er);

                    oFailedDirectories.Add(sDir);

                    if (this.DirectoryDeleteFailed != null)
                    {
                        DirectoryDeleteFailedEvent oFailedEvent = new DirectoryDeleteFailedEvent();
                        oFailedEvent.Directory = sDir;
                        oFailedEvent.Message = er.Message;
                        oFailedEvent.Ex = er;

                        this.DirectoryDeleteFailed(this, oFailedEvent);
                    }
                }
            }

            return oFailedDirectories;
        }
        #endregion
    }
}
