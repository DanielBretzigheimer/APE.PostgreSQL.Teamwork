// <copyright file="FileSystemAccess.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Implementation that provides access to the file system.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // This class encapsulates system borders and therefor cannot be unit tested.
    public class FileSystemAccess : IFileSystemAccess
    {
        public DriveInfo[] GetDrives() => DriveInfo.GetDrives();

        public string[] GetDirectories(string path) => Directory.GetDirectories(path);

        public FileAttributes GetAttributes(string path) => File.GetAttributes(path);

        public string GetTempFileName() => Path.GetTempFileName();

        public string CreateAndGetTempDirectory()
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public string[] ReadAllLines(string fileName)
        {
            var lines = new List<string>();
            using (var sr = new StreamReader(fileName))
            {
                while (true)
                {
                    var line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }

        public void WriteToFile(MemoryStream stream, string fileName)
        {
            using (var outFile = File.Create(fileName))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(outFile);
            }
        }

        public void WriteAllText(string fileName, string content) => File.WriteAllText(fileName, content);

        public void DeleteFile(string filePath) => File.Delete(filePath);

        public void DeleteFolder(string folderPath) => Directory.Delete(folderPath, true);

        public void CopyFile(string sourceFileName, string destinationFileName) => File.Copy(sourceFileName, destinationFileName);

        public void CopyFile(string sourceFileName, string destinationFileName, bool overriteExisting) => File.Copy(sourceFileName, destinationFileName, overriteExisting);

        public long GetFileSize(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("File not found", file);
            }

            return new FileInfo(file).Length;
        }

        public void MoveFile(string sourceFilePath, string destinationFilePath) => File.Move(sourceFilePath, destinationFilePath);

        public FileVersionInfo GetFileVersionInfo(string file) => FileVersionInfo.GetVersionInfo(file);

        public string[] GetFiles(string path)
        {
            return !Directory.Exists(path)
                ? Array.Empty<string>()
                : Directory.GetFiles(path);
        }

        public string[] GetFiles(string path, string pattern)
        {
            return !Directory.Exists(path)
                ? Array.Empty<string>()
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
        public bool FileExists(string path) => File.Exists(path);

        /// <summary>
        /// Get whether given file is locked or in use.
        /// </summary>
        public bool FileIsLocked(string path)
        {
            FileStream? fs = null;

            try
            {
                var fi = new FileInfo(path);
                fs = File.OpenWrite(fi.FullName);
                return false;
            }
            catch (Exception)
            {
                return true;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        /// <summary>
        /// Get whether given directory exists.
        /// </summary>
        public bool DirectoryExists(string path) => Directory.Exists(path);

        /// <summary>
        /// Creates given directory.
        /// </summary>
        public void CreateDirectory(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }

        public string GetCurrentDirectory() => Directory.GetCurrentDirectory();

        public void SetCurrentDirectory(string path) => Directory.SetCurrentDirectory(path);

        public MemoryStream GetMemoryStreamFromFile(string fileName) => new(File.ReadAllBytes(fileName));

        public string ReadAllText(string fileName) => File.ReadAllText(fileName);

        public void SetCreationTime(string path, DateTime creationTime) => File.SetCreationTime(path, creationTime);

        private void GetFiles(string path, string searchPattern, IList<string> files, int max)
        {
            if (files.Count >= max)
            {
                return;
            }

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
    }
}