﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APE.PostgreSQL.Teamwork.Model.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("APE.PostgreSQL.Teamwork.Model.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SET datestyle = dmy;
        ///INSERT INTO &quot;[Schema]&quot;.&quot;ExecutionHistory&quot; (&quot;Version&quot;, &quot;ExecutionDate&quot;, &quot;FileType&quot;, &quot;Message&quot;) VALUES (&apos;[Version]&apos;, &apos;[Time]&apos;, &apos;[FileType]&apos;, &apos;[Message]&apos;);
        ///[Ignore] INSERT INTO &quot;[Schema]&quot;.&quot;ExecutedFile&quot; VALUES (&apos;[Version]&apos;, &apos;[Time]&apos;, &apos;[Message]&apos;);.
        /// </summary>
        internal static string AddExecutedFile {
            get {
                return ResourceManager.GetString("AddExecutedFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///CREATE DATABASE &quot;[Database]&quot;
        ///	WITH OWNER = postgres
        ///			 ENCODING = &apos;UTF8&apos;
        ///			 TABLESPACE = pg_default
        ///			 CONNECTION LIMIT = -1;.
        /// </summary>
        internal static string CreateDatabase {
            get {
                return ResourceManager.GetString("CreateDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///CREATE SCHEMA IF NOT EXISTS &quot;[Schema]&quot;;
        ///
        ///DO
        ///-- needs to be in one line, else Npgsql splits the statement wrong!
        ///$$ BEGIN CREATE TYPE &quot;[Schema]&quot;.&quot;SqlFileType&quot; AS ENUM (&apos;UndoDiff&apos;, &apos;Diff&apos;, &apos;Dump&apos;); EXCEPTION WHEN others THEN /* do nothing because its already existing */ END $$;
        ///ALTER TYPE &quot;[Schema]&quot;.&quot;SqlFileType&quot;
        ///	OWNER TO postgres;
        ///COMMENT ON TYPE &quot;[Schema]&quot;.&quot;SqlFileType&quot;
        ///	IS &apos;the type of an sql file which was executed&apos;;
        ///
        ///CREATE TABLE IF NOT EXISTS &quot;[Schema]&quot;.&quot;ExecutedFile&quot;
        ///(
        ///	&quot;Version&quot; text NO [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateTeamworkSchema {
            get {
                return ResourceManager.GetString("CreateTeamworkSchema", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to REVOKE CONNECT ON DATABASE &quot;[Database]&quot; FROM public;
        ///ALTER DATABASE &quot;[Database]&quot; CONNECTION LIMIT 0;
        ///SELECT pg_terminate_backend(pid)
        ///  FROM pg_stat_activity
        ///  WHERE pid &lt;&gt; pg_backend_pid()
        ///  AND datname=&apos;[Database]&apos;;.
        /// </summary>
        internal static string DisconnectDatabase {
            get {
                return ResourceManager.GetString("DisconnectDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP DATABASE IF EXISTS &quot;[Database]&quot;;.
        /// </summary>
        internal static string DropDatabase {
            get {
                return ResourceManager.GetString("DropDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT DISTINCT &quot;Version&quot;
        ///FROM 	&quot;[Schema]&quot;.&quot;ExecutedFile&quot;
        ///ORDER BY &quot;Version&quot;;.
        /// </summary>
        internal static string GetAppliedVersions {
            get {
                return ResourceManager.GetString("GetAppliedVersions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT 	COUNT(*) 
        ///FROM 	information_schema.schemata
        ///WHERE	schema_name = &apos;[Schema]&apos;
        ///	AND	catalog_name = &apos;[Database]&apos;;.
        /// </summary>
        internal static string GetSchema {
            get {
                return ResourceManager.GetString("GetSchema", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT datname FROM pg_database
        ///WHERE datistemplate = false;.
        /// </summary>
        internal static string GetTables {
            get {
                return ResourceManager.GetString("GetTables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM &quot;[Schema]&quot;.&quot;ExecutedFile&quot; sqlFile WHERE sqlFile.&quot;Version&quot; ILIKE &apos;%[LastAppliedVersion]%&apos;;.
        /// </summary>
        internal static string RemoveVersion {
            get {
                return ResourceManager.GetString("RemoveVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER DATABASE &quot;[NewDatabaseName]&quot; RENAME TO &quot;[OldDatabaseName]&quot;;.
        /// </summary>
        internal static string RenameDatabase {
            get {
                return ResourceManager.GetString("RenameDatabase", resourceCulture);
            }
        }
    }
}
