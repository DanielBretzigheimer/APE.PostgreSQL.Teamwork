// <copyright file="FileSystemAccess.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using log4net;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Implementation that provides access to the file system.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // This class encapsulates system borders and therefor cannot be unit tested.
    public class FileSystemAccess : IFileSystemAccess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Holds the identity of the user that executes this application.
        /// </summary>
        private static readonly WindowsPrincipal CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        public DriveInfo[] GetDrives()
        {
            return DriveInfo.GetDrives();
        }

        public string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        public FileAttributes GetAttributes(string path)
        {
            return System.IO.File.GetAttributes(path);
        }

        public string GetTempFileName()
        {
            return Path.GetTempFileName();
        }

        public string CreateAndGetTempDirectory()
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public string[] ReadAllLines(string fileName)
        {
            return System.IO.File.ReadAllLines(fileName);
        }

        public void WriteToFile(MemoryStream stream, string fileName)
        {
            using (var outFile = System.IO.File.Create(fileName))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(outFile);
            }
        }

        public void WriteAllText(string fileName, string content)
        {
            System.IO.File.WriteAllText(fileName, content);
        }

        public void DeleteFile(string filePath)
        {
            System.IO.File.Delete(filePath);
        }

        public void DeleteFolder(string folderPath)
        {
            Directory.Delete(folderPath, true);
        }

        public void CopyFile(string sourceFileName, string destinationFileName)
        {
            System.IO.File.Copy(sourceFileName, destinationFileName);
        }

        public void CopyFile(string sourceFileName, string destinationFileName, bool overriteExisting)
        {
            System.IO.File.Copy(sourceFileName, destinationFileName, overriteExisting);
        }

        public long GetFileSize(string file)
        {
            if (!System.IO.File.Exists(file))
                throw new FileNotFoundException("File not found", file);
            return new FileInfo(file).Length;
        }

        public void MoveFile(string sourceFilePath, string destinationFilePath)
        {
            System.IO.File.Move(sourceFilePath, destinationFilePath);
        }

        public string GetEntryAssemblyDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        public FileVersionInfo GetFileVersionInfo(string file)
        {
            return FileVersionInfo.GetVersionInfo(file);
        }

        public string[] GetFiles(string path)
        {
            return !Directory.Exists(path)
                ? new string[0]
                : Directory.GetFiles(path);
        }

        public string[] GetFiles(string path, string pattern)
        {
            return !Directory.Exists(path)
                ? new string[0]
                : Directory.GetFiles(path, pattern);
        }

        public string[] GetFilesRecursively(string path, string searchPattern, int maxResults)
        {
            var retval = new List<string>();
            this.GetFiles(path, searchPattern, retval, maxResults);
            return retval.ToArray();
        }

        /// <summary>
        /// Get whether given file exists.
        /// </summary>
        public bool FileExists(string path)
        {
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// Get whether given file is locked or in use.
        /// </summary>
        public bool FileIsLocked(string path)
        {
            FileInfo fi = null;
            FileStream fs = null;

            try
            {
                fi = new FileInfo(path);
                fs = System.IO.File.OpenWrite(fi.FullName);
                fs.Close();
                return false;
            }
            catch (Exception)
            {
                return true;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        /// <summary>
        /// Get whether given directory exists.
        /// </summary>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Creates given directory.
        /// </summary>
        public void CreateDirectory(string path)
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
        }

        public bool CurrentUserHasWriteAccess(string path)
        {
            try
            {
                // Is the read-only flag set?
                var attributes = System.IO.File.GetAttributes(path);
                if ((attributes & FileAttributes.ReadOnly) != 0 ||
                    (attributes & FileAttributes.System) != 0)
                    return false;

                // Get all R/W - ACLs that affect the current user.
                var accessRulez = Directory.GetAccessControl(path)
                    .GetAccessRules(true, true, typeof(NTAccount))
                    .OfType<FileSystemAccessRule>()
                    .Where(x => CurrentPrincipal.IsInRole(x.IdentityReference.Value))
                    .Where(x => x.FileSystemRights.HasFlag(FileSystemRights.Write) && x.FileSystemRights.HasFlag(FileSystemRights.Read))
                    .ToArray();

                // Evaluate the rules.
                var permit = accessRulez.Any(x => x.AccessControlType == AccessControlType.Allow);
                var deny = accessRulez.Any(x => x.AccessControlType == AccessControlType.Deny);

                return permit && !deny;
            }
            catch (UnauthorizedAccessException)
            {
                /* GetAccessControl() raises an UnauthorizedAccessException if the caller has insufficent rights
                 * to list the ACLs, which results in not having write access. */
                return false;
            }
            catch (Exception ex)
            {
                // Exceptions other that UnauthorizedAccessException are not expected.
                Log.Error(string.Format("{0} while querying ACL for {1}", ex.GetType().Name, path), ex);
                return false;
            }
        }

        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public void SetCurrentDirectory(string path)
        {
            Directory.SetCurrentDirectory(path);
        }

        public MemoryStream GetMemoryStreamFromFile(string fileName)
        {
            return new MemoryStream(System.IO.File.ReadAllBytes(fileName));
        }

        public string ReadAllText(string fileName)
        {
            return System.IO.File.ReadAllText(fileName);
        }

        private void GetFiles(string path, string searchPattern, IList<string> files, int max)
        {
            if (files.Count >= max)
                return;

            try
            {
                Directory.GetFiles(path, searchPattern)
                    .ToList()
                    .ForEach(s => files.Add(s));

                Directory.GetDirectories(path)
                    .ToList()
                    .ForEach(s => this.GetFiles(s, searchPattern, files, max));
            }
            catch (UnauthorizedAccessException)
            {
                // ok, so we are not allowed to dig into that directory. Move on.
            }
        }

        public void SetCreationTime(string path, DateTime creationTime)
        {
            System.IO.File.SetCreationTime(path, creationTime);
        }
    }
}