using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace mClickOnceCacheClear.Utilities
{
    public static class FileUtils
    {
        public static List<string> GetFiles(string path, string[] searchTerms)
        {
            List<string> oFiles = new List<string>();

            foreach (string sSearchTerm in searchTerms)
            {
                var regex = new Regex(sSearchTerm, RegexOptions.Compiled);

                List<string> files = new DirectoryInfo(path)
                    .EnumerateFiles("*.*", SearchOption.AllDirectories)
                    .Where(fi => regex.IsMatch(fi.Name))
                    .Select(fi => fi.FullName)
                    .ToList<string>();

                oFiles.AddRange(files);
            }

            return oFiles;
        }

        public static List<string> GetDirectories(string path, string[] searchTerms)
        {
            List<string> oFiles = new List<string>();

            foreach (string sSearchTerm in searchTerms)
            {
                var regex = new Regex(sSearchTerm, RegexOptions.Compiled);

                List<string> files = new DirectoryInfo(path)
                    .EnumerateDirectories("*.*", SearchOption.AllDirectories)
                    .Where(fi => regex.IsMatch(fi.Name))
                    .Select(fi => fi.FullName)
                    .ToList<string>();

                oFiles.AddRange(files);
            }

            return oFiles;
        }

        public static void BackupDirectory(string path, string zipFile)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(path);
                    zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                    zip.Save(zipFile);
                }
            }
            catch (Exception er)
            {
                throw new Exception(string.Format("Error backing up directory: {0}", er.Message), er);
            }
        }
    }
}
