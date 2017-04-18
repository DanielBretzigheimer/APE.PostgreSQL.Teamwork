// APE CodeGeneration Generated Code
namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using APE.CodeGeneration.Attributes;
    using APE.PostgreSQL.Teamwork.Model;
    using APE.PostgreSQL.Teamwork.Model.Templates;
    using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
    using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
    using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
    using log4net;
    using Npgsql;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class DatabaseExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallUpdateVersion(this Database targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("UpdateVersion", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("UpdateVersion with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallTestSQLFiles(this Database targetClass, int progressStart, int progressEnd)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("TestSQLFiles", new object[] { progressStart, progressEnd });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("TestSQLFiles with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static List<ExecutedFile> CallGetExecutedFiles(this Database targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (List<ExecutedFile>)po.Invoke("GetExecutedFiles", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("GetExecutedFiles with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallCreateDiffs(this Database targetClass, string previousDump, string currentDump, string diff, string undoDiff)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("CreateDiffs", new object[] { previousDump, currentDump, diff, undoDiff });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CreateDiffs with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallSearchFiles(this Database targetClass, bool force)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("SearchFiles", new object[] { force });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SearchFiles with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static string CallGenerateFileLocation(this Database targetClass, int version, string extension)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string)po.Invoke("GenerateFileLocation", new object[] { version, extension });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("GenerateFileLocation with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static string CallGenerateFileLocation(this Database targetClass, string prefix, string extension)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string)po.Invoke("GenerateFileLocation", new object[] { prefix, extension });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("GenerateFileLocation with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallSetProgress(this Database targetClass, double progress, string message = null)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("SetProgress", new object[] { progress, message });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SetProgress with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ILog GetDatabaseLog()
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(Database));
            return (ILog)pt.GetStaticFieldOrProperty("Log");
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetDatabaseLog(ILog value)
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(Database));
            try
            {
                pt.SetStaticFieldOrProperty("Log", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Log with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static object GetupdateLock(this Database targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (object)po.GetFieldOrProperty("updateLock");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("updateLock with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetupdateLock(this Database targetClass, object value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("updateLock", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("updateLock with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static string[] GetcachedFiles(this Database targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string[])po.GetFieldOrProperty("cachedFiles");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("cachedFiles with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetcachedFiles(this Database targetClass, string[] value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("cachedFiles", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("cachedFiles with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static bool Getexporting(this Database targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (bool)po.GetFieldOrProperty("exporting");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("exporting with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void Setexporting(this Database targetClass, bool value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("exporting", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("exporting with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using log4net;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class FileSystemAccessExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallGetFiles(this FileSystemAccess targetClass, string path, string searchPattern, IList<string> files, int max)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("GetFiles", new object[] { path, searchPattern, files, max });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("GetFiles with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ILog GetFileSystemAccessLog()
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(FileSystemAccess));
            return (ILog)pt.GetStaticFieldOrProperty("Log");
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetFileSystemAccessLog(ILog value)
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(FileSystemAccess));
            try
            {
                pt.SetStaticFieldOrProperty("Log", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Log with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static WindowsPrincipal GetFileSystemAccessCurrentPrincipal()
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(FileSystemAccess));
            return (WindowsPrincipal)pt.GetStaticFieldOrProperty("CurrentPrincipal");
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetFileSystemAccessCurrentPrincipal(WindowsPrincipal value)
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(FileSystemAccess));
            try
            {
                pt.SetStaticFieldOrProperty("CurrentPrincipal", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CurrentPrincipal with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using APE.CodeGeneration.Attributes;
    using APE.PostgreSQL.Teamwork.Model;
    using APE.PostgreSQL.Teamwork.Model.Templates;
    using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
    using log4net;
    using Npgsql;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class SQLFileExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static List<IStatement> CallGetSQLStatements(this SQLFile targetClass, bool ignoreTeamworkExecution = false)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (List<IStatement>)po.Invoke("GetSQLStatements", new object[] { ignoreTeamworkExecution });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("GetSQLStatements with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ILog GetSQLFileLog()
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(SQLFile));
            return (ILog)pt.GetStaticFieldOrProperty("Log");
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetSQLFileLog(ILog value)
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(SQLFile));
            try
            {
                pt.SetStaticFieldOrProperty("Log", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Log with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static IDatabase Getdatabase(this SQLFile targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (IDatabase)po.GetFieldOrProperty("database");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("database with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void Setdatabase(this SQLFile targetClass, IDatabase value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("database", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("database with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static IFileSystemAccess Getfile(this SQLFile targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (IFileSystemAccess)po.GetFieldOrProperty("file");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("file with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void Setfile(this SQLFile targetClass, IFileSystemAccess value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("file", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("file with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static string GetPath(this SQLFile targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string)po.GetFieldOrProperty("Path");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Path with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetPath(this SQLFile targetClass, string value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("Path", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Path with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static string GetFileName(this SQLFile targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string)po.GetFieldOrProperty("FileName");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("FileName with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetFileName(this SQLFile targetClass, string value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("FileName", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("FileName with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static DatabaseVersion GetVersion(this SQLFile targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (DatabaseVersion)po.GetFieldOrProperty("Version");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Version with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetVersion(this SQLFile targetClass, DatabaseVersion value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("Version", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Version with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static FileType GetFileType(this SQLFile targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (FileType)po.GetFieldOrProperty("FileType");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("FileType with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetFileType(this SQLFile targetClass, FileType value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("FileType", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("FileType with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
    using APE.PostgreSQL.Teamwork.Model.Utils;
    using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
    using APE.PostgreSQL.Teamwork.ViewModel.Postgres.Loader;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class SQLFileTesterExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallCreateData(this SQLFileTester targetClass, Database database, ISQLFile target, PgSchema schema, PgTable table, Dictionary<PgTable, bool> executedTables)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("CreateData", new object[] { database, target, schema, table, executedTables });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CreateData with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static string CallGetRandomValue(this SQLFileTester targetClass, string type, ISQLFile target)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string)po.Invoke("GetRandomValue", new object[] { type, target });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("GetRandomValue with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System;
    using System.IO;
    using APE.CodeGeneration.Attributes;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class StatementExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static string CallGetTitle(this Statement targetClass, string sql)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string)po.Invoke("GetTitle", new object[] { sql });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("GetTitle with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static IDatabase Getdatabase(this Statement targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (IDatabase)po.GetFieldOrProperty("database");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("database with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void Setdatabase(this Statement targetClass, IDatabase value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("database", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("database with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static string GetSearchPath(this Statement targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string)po.GetFieldOrProperty("SearchPath");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SearchPath with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetSearchPath(this Statement targetClass, string value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("SearchPath", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SearchPath with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;
    using APE.CodeGeneration.Attributes;
    using APE.PostgreSQL.Teamwork.Model;
    using APE.PostgreSQL.Teamwork.Model.Setting;
    using APE.PostgreSQL.Teamwork.Model.Templates;
    using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
    using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class AddDatabaseViewModelExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallInitializeCommands(this AddDatabaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("InitializeCommands", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("InitializeCommands with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallCheckData(this AddDatabaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("CheckData", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CheckData with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows;
    using APE.CodeGeneration.Attributes;
    using APE.PostgreSQL.Teamwork.Model;
    using log4net;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class BaseViewModelExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallExecuteInTask(this BaseViewModel targetClass, Action action, Action<bool> setExecuting = null)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("ExecuteInTask", new object[] { action, setExecuting });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExecuteInTask with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ILog GetBaseViewModelLog()
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(BaseViewModel));
            return (ILog)pt.GetStaticFieldOrProperty("Log");
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetBaseViewModelLog(ILog value)
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(BaseViewModel));
            try
            {
                pt.SetStaticFieldOrProperty("Log", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Log with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using APE.CodeGeneration.Attributes;
    using APE.PostgreSQL.Teamwork.Model;
    using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
    using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class ImportWindowViewModelExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallCreateCommands(this ImportWindowViewModel targetClass, Dispatcher uiDispatcher)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("CreateCommands", new object[] { uiDispatcher });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CreateCommands with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallExecuteFiles(this ImportWindowViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("ExecuteFiles", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExecuteFiles with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallSuccesfullyExecutedMessageBoxClosing(this ImportWindowViewModel targetClass, MaterialMessageBoxResult result)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("SuccesfullyExecutedMessageBoxClosing", new object[] { result });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SuccesfullyExecutedMessageBoxClosing with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetExecuteFileCommand(this ImportWindowViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("ExecuteFileCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExecuteFileCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetExecuteFileCommand(this ImportWindowViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("ExecuteFileCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExecuteFileCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetOpenFileCommand(this ImportWindowViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("OpenFileCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("OpenFileCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetOpenFileCommand(this ImportWindowViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("OpenFileCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("OpenFileCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetExecuteCommand(this ImportWindowViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("ExecuteCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExecuteCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetExecuteCommand(this ImportWindowViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("ExecuteCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExecuteCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetMarkAsExecutedCommand(this ImportWindowViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("MarkAsExecutedCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("MarkAsExecutedCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetMarkAsExecutedCommand(this ImportWindowViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("MarkAsExecutedCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("MarkAsExecutedCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using APE.CodeGeneration.Attributes;
    using APE.PostgreSQL.Teamwork.Model;
    using APE.PostgreSQL.Teamwork.Model.Setting;
    using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
    using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
    using APE.PostgreSQL.Teamwork.ViewModel.ViewModels;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class MainWindowViewModelExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallUpdateWorkerTick(this BaseViewModel targetClass, object sender, EventArgs e)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("UpdateWorkerTick", new object[] { sender, e });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("UpdateWorkerTick with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallCheckSettings(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("CheckSettings", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CheckSettings with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static string CallSearchFileRecursivly(this BaseViewModel targetClass, string filename, string path)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string)po.Invoke("SearchFileRecursivly", new object[] { filename, path });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SearchFileRecursivly with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static object CallConnectionMessageBoxClosingEventHandler(this BaseViewModel targetClass, MaterialMessageBoxResult result)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (object)po.Invoke("ConnectionMessageBoxClosingEventHandler", new object[] { result });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ConnectionMessageBoxClosingEventHandler with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallDatabaseRemoved(this BaseViewModel targetClass, object sender, EventArgs e)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("DatabaseRemoved", new object[] { sender, e });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("DatabaseRemoved with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallRefreshDatabases(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("RefreshDatabases", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("RefreshDatabases with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallOpenSettings(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("OpenSettings", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("OpenSettings with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallCreateCommands(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("CreateCommands", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CreateCommands with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallSizeChanged(this BaseViewModel targetClass, SizeChangedEventArgs args)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("SizeChanged", new object[] { args });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SizeChanged with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static object GetupdateLock(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (object)po.GetFieldOrProperty("updateLock");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("updateLock with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetupdateLock(this BaseViewModel targetClass, object value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("updateLock", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("updateLock with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static DispatcherTimer Getworker(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (DispatcherTimer)po.GetFieldOrProperty("worker");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("worker with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void Setworker(this BaseViewModel targetClass, DispatcherTimer value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("worker", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("worker with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static Dispatcher GetuiDispatcher(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (Dispatcher)po.GetFieldOrProperty("uiDispatcher");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("uiDispatcher with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetuiDispatcher(this BaseViewModel targetClass, Dispatcher value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("uiDispatcher", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("uiDispatcher with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static List<DatabaseDisplayData> GetunfilteredDatabases(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (List<DatabaseDisplayData>)po.GetFieldOrProperty("unfilteredDatabases");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("unfilteredDatabases with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetunfilteredDatabases(this BaseViewModel targetClass, List<DatabaseDisplayData> value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("unfilteredDatabases", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("unfilteredDatabases with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static string GetWindowTitle(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string)po.GetFieldOrProperty("WindowTitle");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("WindowTitle with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetWindowTitle(this BaseViewModel targetClass, string value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("WindowTitle", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("WindowTitle with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetSettingCommand(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("SettingCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SettingCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetSettingCommand(this BaseViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("SettingCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SettingCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetAddDatabaseCommand(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("AddDatabaseCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("AddDatabaseCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetAddDatabaseCommand(this BaseViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("AddDatabaseCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("AddDatabaseCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetRefreshDatabasesCommand(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("RefreshDatabasesCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("RefreshDatabasesCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetRefreshDatabasesCommand(this BaseViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("RefreshDatabasesCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("RefreshDatabasesCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetSearchCommand(this BaseViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("SearchCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SearchCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetSearchCommand(this BaseViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("SearchCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SearchCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System;
    using System.Windows.Forms;
    using System.Windows.Input;
    using APE.CodeGeneration.Attributes;
    using APE.PostgreSQL.Teamwork.Model.Setting;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class SettingViewModelExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallLoad(this SettingViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("Load", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Load with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallUpdateConnectionStringPreview(this SettingViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("UpdateConnectionStringPreview", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("UpdateConnectionStringPreview with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallSave(this SettingViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("Save", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Save with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallSelectPgDumpPath(this SettingViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("SelectPgDumpPath", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SelectPgDumpPath with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallSelectDefaultDatabaseFolderPath(this SettingViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("SelectDefaultDatabaseFolderPath", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SelectDefaultDatabaseFolderPath with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static IConnectionManager GetconnectionManager(this SettingViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (IConnectionManager)po.GetFieldOrProperty("connectionManager");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("connectionManager with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetconnectionManager(this SettingViewModel targetClass, IConnectionManager value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("connectionManager", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("connectionManager with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetSaveCommand(this SettingViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("SaveCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SaveCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetSaveCommand(this SettingViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("SaveCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SaveCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetSelectDefaultDatabaseFolderPathCommand(this SettingViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("SelectDefaultDatabaseFolderPathCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SelectDefaultDatabaseFolderPathCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetSelectDefaultDatabaseFolderPathCommand(this SettingViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("SelectDefaultDatabaseFolderPathCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SelectDefaultDatabaseFolderPathCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetSelectPgDumpPathCommand(this SettingViewModel targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("SelectPgDumpPathCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SelectPgDumpPathCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetSelectPgDumpPathCommand(this SettingViewModel targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("SelectPgDumpPathCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SelectPgDumpPathCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using APE.CodeGeneration.Attributes;
    using APE.PostgreSQL.Teamwork.Model;
    using APE.PostgreSQL.Teamwork.Model.Setting;
    using APE.PostgreSQL.Teamwork.Model.Templates;
    using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
    using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
    using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
    using log4net;
    using Npgsql;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class DatabaseDisplayDataExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static string RemovedName(this DatabaseDisplayData targetClass)
        {
            return "Removed";
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallConnectDatabase(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("ConnectDatabase", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ConnectDatabase with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallUpdateApplicableSQLFiles(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("UpdateApplicableSQLFiles", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("UpdateApplicableSQLFiles with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallRemove(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("Remove", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Remove with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallSave(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("Save", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Save with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallEditPath(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("EditPath", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("EditPath with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallInitializeCommands(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("InitializeCommands", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("InitializeCommands with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallCreateDatabase(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("CreateDatabase", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CreateDatabase with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallStartImport(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("StartImport", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("StartImport with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallUndoChanges(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("UndoChanges", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("UndoChanges with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallReset(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("Reset", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Reset with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallCreateDump(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("CreateDump", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CreateDump with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallReduceVersion(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("ReduceVersion", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ReduceVersion with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallExport(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("Export", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Export with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallTestDatabase(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("TestDatabase", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("TestDatabase with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallExecuteInTask(this DatabaseDisplayData targetClass, Action action)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("ExecuteInTask", new object[] { action });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExecuteInTask with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallExecuteInTask(this DatabaseDisplayData targetClass, Func<Task> action)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("ExecuteInTask", new object[] { action });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExecuteInTask with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ILog GetDatabaseDisplayDataLog()
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(DatabaseDisplayData));
            return (ILog)pt.GetStaticFieldOrProperty("Log");
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetDatabaseDisplayDataLog(ILog value)
        {
            var pt = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType(typeof(DatabaseDisplayData));
            try
            {
                pt.SetStaticFieldOrProperty("Log", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("Log with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static bool GetdataInitialized(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (bool)po.GetFieldOrProperty("dataInitialized");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("dataInitialized with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetdataInitialized(this DatabaseDisplayData targetClass, bool value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("dataInitialized", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("dataInitialized with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetExportCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("ExportCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExportCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetExportCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("ExportCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExportCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetOpenImportWindowCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("OpenImportWindowCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("OpenImportWindowCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetOpenImportWindowCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("OpenImportWindowCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("OpenImportWindowCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetCreateDumpCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("CreateDumpCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CreateDumpCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetCreateDumpCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("CreateDumpCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CreateDumpCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetReduceVersionCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("ReduceVersionCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ReduceVersionCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetReduceVersionCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("ReduceVersionCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ReduceVersionCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetTestCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("TestCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("TestCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetTestCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("TestCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("TestCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetOpenPathCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("OpenPathCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("OpenPathCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetOpenPathCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("OpenPathCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("OpenPathCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetEditCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("EditCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("EditCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetEditCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("EditCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("EditCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetSaveCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("SaveCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SaveCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetSaveCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("SaveCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SaveCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetEditPathCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("EditPathCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("EditPathCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetEditPathCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("EditPathCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("EditPathCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetRemoveCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("RemoveCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("RemoveCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetRemoveCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("RemoveCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("RemoveCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetResetCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("ResetCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ResetCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetResetCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("ResetCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ResetCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetExpandCommand(this DatabaseDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("ExpandCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExpandCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetExpandCommand(this DatabaseDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("ExpandCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("ExpandCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{        using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using APE.CodeGeneration.Attributes;
    using Npgsql;
    using APE.PostgreSQL.Teamwork.ViewModel;

    internal static class StatementDisplayDataExtension
    {
        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallCreateCommands(this StatementDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("CreateCommands", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CreateCommands with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void CallCopyToClipboard(this StatementDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.Invoke("CopyToClipboard", new object[] { });
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CopyToClipboard with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static string GetoriginalSQL(this StatementDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (string)po.GetFieldOrProperty("originalSQL");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("originalSQL with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetoriginalSQL(this StatementDisplayData targetClass, string value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("originalSQL", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("originalSQL with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetRetryCommand(this StatementDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("RetryCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("RetryCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetRetryCommand(this StatementDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("RetryCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("RetryCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetCopyCommand(this StatementDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("CopyCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CopyCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetCopyCommand(this StatementDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("CopyCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("CopyCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetEditCommand(this StatementDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("EditCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("EditCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetEditCommand(this StatementDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("EditCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("EditCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static ICommand GetSaveCommand(this StatementDisplayData targetClass)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                return (ICommand)po.GetFieldOrProperty("SaveCommand");
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SaveCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        internal static void SetSaveCommand(this StatementDisplayData targetClass, ICommand value)
        {
            var po = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(targetClass);
            try
            {
                po.SetFieldOrProperty("SaveCommand", value);
            }
            catch (System.MissingMethodException missingMethodException)
            {
                throw new System.NotSupportedException("SaveCommand with requested parameters is not found. Rerun code generation.", missingMethodException);
            }
        }
    }
}
