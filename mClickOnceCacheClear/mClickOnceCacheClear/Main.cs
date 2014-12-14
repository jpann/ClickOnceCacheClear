using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using mClickOnceCacheClear.Utilities;
using System.IO;
using Ionic.Zip;
using log4net;
using log4net.Config;
using mClickOnceCacheClear.Events;

namespace mClickOnceCacheClear
{
    public partial class Main : Form
    {
        #region Private Variables
        private ILog mLog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool mOverrideBackups = false;
        private bool mDebug = false;
        private string[] mSearchTerms = null;
        private List<EventArgs> mFailureList = new List<EventArgs>();
        #endregion

        private void LoadConfiguration()
        {
            this.mDebug = Properties.Settings.Default.Debug;
            this.mSearchTerms = Properties.Settings.Default.SearchTerms.Split('|');

            if (Environment.GetCommandLineArgs().Length > 1)
            {
                string[] args = Environment.GetCommandLineArgs();

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].ToLower() == "-nobackup" || args[i].ToLower() == "-nb")
                        this.mOverrideBackups = true;
                }
            }
        }

        public Main()
        {
            InitializeComponent();

            XmlConfigurator.Configure();

            try
            {
                this.LoadConfiguration();
                
#if RELEASE  
                if (UacHelper.IsUacEnabled)
                {
                    if (UacHelper.IsProcessElevated)
                    {
                        string sMsg = "You must run this program with UAC elevation.";

                        MessageBox.Show(
                           sMsg,
                           "UAC Error",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);

                        mLog.Fatal(sMsg);

                        Environment.Exit(-1);
                    }
                }
#endif
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Start up error:\n{0}", er.Message);

                MessageBox.Show(
                   sMsg,
                   "Start Up Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);

                mLog.Fatal(sMsg);

                Environment.Exit(-1);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (this.mOverrideBackups)
            {
                btnFilesDelete.Enabled = true;
                btnRegistryDelete.Enabled = true;
            }

            LoadCache();
        }

        #region Backup Methods
        private void btnFilesBackup_Click(object sender, EventArgs e)
        {
            try
            {
                ClickOnceCache oCC = new ClickOnceCache();
                string sCachePath = oCC.GetCachePath();
                string sSaveFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "_Backup.zip");

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(sCachePath);
                    zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                    zip.Save(sSaveFile);

                    btnFilesDelete.Enabled = true;

                    string sMsg = string.Format("Backed up cache files to '{0}'", sSaveFile);

                    lblStatus.Text = sMsg;
                    mLog.Info(sMsg);

                    System.Diagnostics.Process.Start(Path.GetDirectoryName(Application.ExecutablePath));
                }
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Error backing up cache files:\n{0}", er.Message);

                mLog.Error(sMsg, er);

                MessageBox.Show(
                    sMsg,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnRegistryBackup_Click(object sender, EventArgs e)
        {
            try
            {
                ClickOnceCache oCC = new ClickOnceCache();
                string sSaveFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "_Backup.reg");

                RegistryUtils.BackupRegistryKey(oCC.RegistryCachePath, sSaveFile);

                btnRegistryDelete.Enabled = true;

                lblStatus.Text = string.Format("Backed up cache registry to '{0}'", sSaveFile);

                System.Diagnostics.Process.Start(Path.GetDirectoryName(Application.ExecutablePath));
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Error backing up cache registry:\n{0}", er.Message);

                mLog.Error(sMsg, er);

                MessageBox.Show(
                    sMsg,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Cache Loading Methods
        private void LoadCache()
        {
            try
            {
                lstFiles.Items.Clear();

                ClickOnceCache oCache = new ClickOnceCache();

                LoadCacheFiles(oCache);
                LoadCacheDirectories(oCache);
                LoadCacheRegistry(oCache);
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Error loading cache:\n{0}", er.Message);

                mLog.Error(sMsg, er);

                MessageBox.Show(
                    sMsg,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadCacheFiles(ClickOnceCache cache)
        {
            string sCachePath = cache.GetCachePath();

            List<string> oFiles = null;

            if (this.mSearchTerms.Any())
            {
                oFiles = cache.GetFiles(sCachePath, this.mSearchTerms);
            }
            else
            {
                oFiles = cache.GetFiles(sCachePath);
            } 

            if (!oFiles.Any())
            {
                string sMsg = string.Format("No cache files found in '{0}'!", sCachePath);

                mLog.Info(sMsg);

                MessageBox.Show(sMsg,
                    "Cache Files Not Found");
                
                return;
            }

            foreach (string sFile in oFiles)
            {
                ListViewItem oItem = new ListViewItem(Path.GetFileName(sFile));
                oItem.SubItems.Add(sFile);
                oItem.Tag = false;
                oItem.Checked = true;

                lstFiles.Items.Add(oItem);
            }
        }

        private void LoadCacheDirectories(ClickOnceCache cache)
        {
            string sCachePath = cache.GetCachePath();

            List<string> oDirectories = null;

            if (this.mSearchTerms.Any())
            {
                oDirectories = cache.GetDirectories(sCachePath, this.mSearchTerms);
            }
            else
            {
                oDirectories = cache.GetDirectories(sCachePath);
            }

            if (!oDirectories.Any())
            {
                string sMsg = string.Format("No cache directories found in '{0}'!", sCachePath);

                mLog.Info(sMsg);

                MessageBox.Show(sMsg,
                    "Cache Directories Not Found");

                return;
            }

            foreach (string sDir in oDirectories)
            {
                ListViewItem oItem = new ListViewItem(Path.GetFileName(sDir));
                oItem.SubItems.Add(sDir);
                oItem.Tag = true;
                oItem.Checked = true;

                lstFiles.Items.Add(oItem);
            }
        }

        private void LoadCacheRegistry(ClickOnceCache cache)
        {
            try
            {
                List<string> oKeys = null;

                if (this.mSearchTerms.Any())
                {
                    oKeys = cache.GetRegistryKeys(this.mSearchTerms);
                }
                else
                {
                    oKeys = cache.GetRegistryKeys();
                }

                if (!oKeys.Any())
                {
                    string sMsg = string.Format("No cache registry keys found in '{0}'!", cache.RegistryCachePath);

                    mLog.Info(sMsg);

                    MessageBox.Show(sMsg,
                        "Cache Registry Keys Not Found");

                    return;
                }

                foreach (string sKey in oKeys)
                {
                    ListViewItem oItem = new ListViewItem(sKey.Substring(sKey.LastIndexOf('\\') + 1));
                    oItem.SubItems.Add(sKey);
                    oItem.Checked = true;

                    lstRegistry.Items.Add(oItem);
                }

            }
            catch (Exception er)
            {
                string sMsg  = string.Format("Error loading cache registry:\n{0}", er.Message);

                mLog.Error(sMsg, er);

                MessageBox.Show(
                    sMsg,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Deletion Event Handlers
        public void DirectoryDeleteEventHandler(ClickOnceCache sender, DirectoryDeleteEvent e)
        {

        }

        public void DirectoryDeleteFailedEventHandler(ClickOnceCache sender, DirectoryDeleteFailedEvent e)
        {
            this.mFailureList.Add(e);
        }

        public void FileDeleteEventHandler(ClickOnceCache sender, FileDeleteEvent e)
        {

        }

        public void FileDeleteFailedEventHandler(ClickOnceCache sender, FileDeleteFailedEvent e)
        {
            this.mFailureList.Add(e);
        }

        public void RegistryKeyDeleteEventHandler(ClickOnceCache sender, RegistryKeyDeleteEvent e)
        {

        }

        public void RegistryKeyDeleteFailedEventHandler(ClickOnceCache sender, RegistryKeyDeleteFailedEvent e)
        {
            this.mFailureList.Add(e);
        }
        #endregion

        #region Deletion Methods
        private void btnFilesDelete_Click(object sender, EventArgs e)
        {
            ClickOnceCache oCleaner = new ClickOnceCache();

            try
            {
                oCleaner.DirectoryDelete += DirectoryDeleteEventHandler;
                oCleaner.DirectoryDeleteFailed += DirectoryDeleteFailedEventHandler;
                oCleaner.FileDelete += FileDeleteEventHandler;
                oCleaner.FileDeleteFailed += FileDeleteFailedEventHandler;

                List <string> oFiles = new List<string>();
                List<string> oDirectories = new List<string>();

                foreach (ListViewItem item in lstFiles.Items)
                {
                    if (!item.Checked)
                        continue;

                    if (((bool)item.Tag) == false)
                        oFiles.Add(item.SubItems[1].Text);
                    else
                        oDirectories.Add(item.SubItems[1].Text);

                    lstFiles.Items.Remove(item);
                }

                List<string> oFailedFiles = oCleaner.DeleteFiles(oFiles);
                List<string> oFailedDirs = oCleaner.DeleteDirectories(oDirectories);

                if (oFailedFiles.Any() || oFailedDirs.Any())
                {

                    MessageBox.Show(
                        string.Format("Failed to delete {0} cache registry files and {1} registry cache directories.",
                        oFailedFiles.Count,
                        oFailedDirs.Count),
                        "Delete Status",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    foreach (string sFile in oFailedFiles)
                    {
                        ListViewItem oItem = new ListViewItem(Path.GetFileName(sFile));
                        oItem.SubItems.Add(sFile);
                        oItem.Tag = false;
                        oItem.Checked = true;

                        lstFiles.Items.Add(oItem);
                    }

                    foreach (string sDir in oFailedDirs)
                    {
                        ListViewItem oItem = new ListViewItem(Path.GetFileName(sDir));
                        oItem.SubItems.Add(sDir);
                        oItem.Tag = true;
                        oItem.Checked = true;

                        lstFiles.Items.Add(oItem);
                    }
                }
                else
                {
                    lblStatus.Text = "Cache file and directory deletion successful!";
                }
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Error deleting files cache:\n{0}", er.Message);

                mLog.Error(sMsg, er);

                MessageBox.Show(
                    sMsg,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                oCleaner.DirectoryDelete -= DirectoryDeleteEventHandler;
                oCleaner.DirectoryDeleteFailed -= DirectoryDeleteFailedEventHandler;
                oCleaner.FileDelete -= FileDeleteEventHandler;
                oCleaner.FileDeleteFailed -= FileDeleteFailedEventHandler;
            }
          
        }

        private void btnRegistryDelete_Click(object sender, EventArgs e)
        {
            ClickOnceCache oCleaner = new ClickOnceCache();

            try
            {
                oCleaner.RegistryKeyDelete += RegistryKeyDeleteEventHandler;
                oCleaner.RegistryKeyDeleteFailed += RegistryKeyDeleteFailedEventHandler;

                List<string> oKeys = new List<string>();

                foreach (ListViewItem item in lstRegistry.Items)
                {
                    if (!item.Checked)
                        continue;

                    oKeys.Add(@"HKEY_CURRENT_USER\" + item.SubItems[1].Text);

                    lstRegistry.Items.Remove(item);
                }

                List<string> oFailedKeys = oCleaner.DeleteRegistryKeys(oKeys);

                if (oFailedKeys.Any())
                {
                    MessageBox.Show(string.Format("Failed to delete {0} cache registry keys.", oFailedKeys.Count),
                        "Delete Status",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    foreach (string sKey in oFailedKeys)
                    {
                        ListViewItem oItem = new ListViewItem(sKey.Substring(sKey.LastIndexOf('\\') + 1));
                        oItem.SubItems.Add(sKey);
                        oItem.Checked = true;

                        lstRegistry.Items.Add(oItem);
                    }
                }

                else
                {
                    lblStatus.Text = "Cache registry deletion successful!";
                }
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Error deleting registry cache:\n{0}", er.Message);

                mLog.Error(sMsg, er);

                MessageBox.Show(
                    sMsg,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                oCleaner.RegistryKeyDelete -= RegistryKeyDeleteEventHandler;
                oCleaner.RegistryKeyDeleteFailed -= RegistryKeyDeleteFailedEventHandler;
            }
          
        }
        #endregion

    }
}
