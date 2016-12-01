// <copyright file="IFileSystemAccess.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Interface for an implementation that provides access to the file system.
    /// </summary>
    public interface IFileSystemAccess
    {
        /// <summary>
        /// Returns the current machine's logical drives.
        /// </summary>
        DriveInfo[] GetDrives();

        /// <summary>
        /// Gets the given directory's sub directories.
        /// </summary>
        string[] GetDirectories(string path);

        /// <summary>
        /// Gets a file's size in bytes.
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown if file not found.</exception>
        long GetFileSize(string file);

        /// <summary>
        /// Gets a file's attributes.
        /// </summary>
        FileAttributes GetAttributes(string path);

        /// <summary>
        /// Enumerates a directory's files.
        /// </summary>
        string[] GetFiles(string path);

        /// <summary>
        /// Enumerates a directory's files.
        /// </summary>
        string[] GetFiles(string path, string pattern);

        /// <summary>
        /// Enumerates a directory's files.
        /// </summary>
        string[] GetFilesRecursively(string path, string pattern, int maxResults);

        /// <summary>
        /// Gets a temp file name.
        /// </summary>
        string GetTempFileName();

        /// <summary>
        /// Reads a file into a memory stream.
        /// </summary>
        MemoryStream GetMemoryStreamFromFile(string fileName);

        /// <summary>
        /// Reads a file as string.
        /// </summary>
        string ReadAllText(string fileName);

        /// <summary>
        /// Reads a file as string array.
        /// </summary>
        string[] ReadAllLines(string fileName);

        /// <summary>
        /// Writes a memory stream's content to a file.
        /// </summary>
        void WriteToFile(MemoryStream stream, string fileName);

        /// <summary>
        /// Writes text to a file.
        /// </summary>
        void WriteAllText(string fileName, string content);

        /// <summary>
        /// Get whether given directory exists.
        /// </summary>
        bool DirectoryExists(string path);

        /// <summary>
        /// Get whether given file exists.
        /// </summary>
        bool FileExists(string path);

        /// <summary>
        /// Get whether given file is locked or in use.
        /// </summary>
        bool FileIsLocked(string path);

        /// <summary>
        /// Creates given directory.
        /// </summary>
        void CreateDirectory(string path);

        /// <summary>
        /// Gets a temp directory name.
        /// </summary>
        string CreateAndGetTempDirectory();

        /// <summary>
        /// Copies a file.
        /// </summary>
        void CopyFile(string sourceFileName, string destinationFileName);

        /// <summary>
        /// Copies a file and overwrites an exsiting Fil.
        /// </summary>
        void CopyFile(string sourceFileName, string destinationFileName, bool overriteExisting);

        /// <summary>
        /// Deletes a file.
        /// </summary>
        void DeleteFile(string filePath);

        /// <summary>
        /// Deletes a folder.
        /// </summary>
        void DeleteFolder(string folderPath);

        /// <summary>
        /// Moves a file.
        /// </summary>
        void MoveFile(string sourceFilePath, string desinationFilePath);

        /// <summary>
        /// Gets the entry assembly's binary path.
        /// </summary>
        string GetEntryAssemblyDirectory();

        /// <summary>
        /// Gets version information about a file.
        /// </summary>
        FileVersionInfo GetFileVersionInfo(string file);

        /// <summary>
        /// Gets a value indicating whether the current user has write access to the specified path.
        /// </summary>
        bool CurrentUserHasWriteAccess(string path);

        /// <summary>
        /// Gets the current working directory.
        /// </summary>
        string GetCurrentDirectory();

        /// <summary>
        /// Sets the current working directory.
        /// </summary>
        void SetCurrentDirectory(string path);

        /// <summary>
        /// Sets date and time the file was created.
        /// </summary>
        void SetCreationTime(string path, DateTime creationTime);
    }
}