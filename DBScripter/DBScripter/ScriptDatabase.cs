
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.SqlServer.Management.Smo;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System.Text;
using System.Collections.Specialized;
namespace DBScripter
{
    class ScriptDatabase
    {
        string rootdirectory = String.Empty;
        string serverName = String.Empty;
        string password = String.Empty;
        string userName = String.Empty;
        public ScriptDatabase(string servName, string usrName, string pass, string dbType)
        {
            serverName = servName;
            userName = usrName;
            password = pass;
            rootdirectory = Path.Combine(@"C:\DBScripter\SchemaObjects", dbType);
            if (!Directory.Exists(rootdirectory))
                Directory.CreateDirectory(rootdirectory);
        }
        public void GenerateScripts(ArrayList DBlist)
        {
            string filename = String.Empty;

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server srv = new Microsoft.SqlServer.Management.Smo.Server(svrconn);
            foreach (string dbnam in DBlist)
            {
                GenerateTableScript(srv, dbnam.ToString());
                GenerateViewsScript(srv, dbnam.ToString());
                GenerateStoredprocedureScript(srv, dbnam.ToString());
                GenerateUserDefinedFunctionsScript(srv, dbnam.ToString());
                GenerateApplicationRolesScript(srv, dbnam.ToString());
                GenerateDatabaseRolesScript(srv, dbnam.ToString());
                GenerateSecuritySchemaScript(srv, dbnam.ToString());
                GenerateUsersScript(srv, dbnam.ToString());
                GenerateDatabaseTriggersScript(srv, dbnam.ToString());
            }




        }
        #region TableandTablerelatedobjectsSCriptgeneration

        private void GenerateTableScript(Server CurrServer, string CurrdbName)
        {
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Tables");
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];
            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.AnsiFile = true;
            scriptOptions.AppendToFile = false;
            scriptOptions.ContinueScriptingOnError = true;
            scriptOptions.ExtendedProperties = true;
            scriptOptions.IncludeHeaders = true;
            scriptOptions.PrimaryObject = true;
            scriptOptions.SchemaQualify = true;
            scriptOptions.ToFileOnly = true;
            scriptOptions.ConvertUserDefinedDataTypesToBaseType = true;
            scriptOptions.DriAll = false;
            scriptOptions.Indexes = false;
            scriptOptions.Triggers = false;
            scriptOptions.NoFileGroup = false;
            scriptOptions.DriForeignKeys = false;  // added 2007.02.19
            scriptOptions.NoTablePartitioningSchemes = false;
            scriptOptions.IncludeIfNotExists = true;
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (Table myTable in CurrDatabase.Tables)
            {
                if (myTable.IsSystemObject == false)
                {
                    try
                    {
                        GenerateIndexScripts(myTable);
                        GenerateTriggersScripts(myTable);
                        GenerateCheckConstraintsScripts(myTable);
                        GenerateDefaultConstraintsScripts(myTable);
                        GenerateForeignKeyScripts(myTable);

                        //actaul scripting the table
                        StringCollection tableScripts = myTable.Script(scriptOptions);
                        filename = Path.Combine(workingdir, "dbo." + myTable.Name + ".Table.sql");
                        /* Generating CREATE TABLE command */
                        tableScripts = myTable.Script();
                        String str = ConvertStringArrayToString(tableScripts);
                        File.WriteAllText(filename, str);

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void GenerateIndexScripts(Table myTable)
        {

            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Index");
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (Index ndx in myTable.Indexes)
            {
                try
                {
                    if (ndx.IndexKeyType.ToString() == "DriUniqueKey")
                    {
                        filename = Path.Combine(workingdir, "dbo." + ndx.Name + ".ukey.sql");
                    }
                    else if (ndx.IndexKeyType.ToString() == "DriPrimaryKey")
                    {
                        filename = Path.Combine(workingdir, "dbo." + ndx.Name + ".pkey.sql");
                    }
                    else
                    {
                        filename = Path.Combine(workingdir, "dbo." + ndx.Name + ".Index.sql");
                    }
                    StringCollection ndxScripts = ndx.Script();
                    ndxScripts = ndx.Script();
                    String str = ConvertStringArrayToString(ndxScripts);
                    File.WriteAllText(filename, str);
                }

                catch (Exception)
                {

                }
            }
        }

        private void GenerateCheckConstraintsScripts(Table myTable)
        {

            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Constraints");
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (Check chkcon in myTable.Checks)
            {
                try
                {
                    filename = Path.Combine(workingdir, "dbo." + myTable.Name + "." + chkcon.Name + ".chkconst.sql");

                    StringCollection ndxScripts = chkcon.Script();
                    ndxScripts = chkcon.Script();
                    String str = ConvertStringArrayToString(ndxScripts);
                    File.WriteAllText(filename, str);
                }

                catch (Exception)
                {

                }
            }
        }

        private void GenerateDefaultConstraintsScripts(Table myTable)
        {

            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Constraints");
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (Column col in myTable.Columns)
            {
                try
                {
                    if (col.DefaultConstraint != null)
                    {
                        filename = Path.Combine(workingdir, "dbo." + myTable.Name + "." + col.Name + ".defconst.sql");

                        StringCollection ndxScripts = col.DefaultConstraint.Script();
                        ndxScripts = col.DefaultConstraint.Script();
                        String str = ConvertStringArrayToString(ndxScripts);
                        File.WriteAllText(filename, str);
                    }

                }
                catch (Exception)
                {

                }
            }

        }

        private void GenerateForeignKeyScripts(Table myTable)
        {

            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Constraints");
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (ForeignKey col in myTable.ForeignKeys)
            {
                try
                {
                    filename = Path.Combine(workingdir, "dbo." + myTable.Name + "." + col.Name + ".fkey.sql");

                    StringCollection ndxScripts = col.Script();
                    ndxScripts = col.Script();
                    String str = ConvertStringArrayToString(ndxScripts);
                    File.WriteAllText(filename, str);

                }
                catch (Exception)
                {

                }
            }
        }

        private void GenerateTriggersScripts(Table myTable)
        {

            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Database Triggers");
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (Trigger trg in myTable.Triggers)
            {
                try
                {
                    StringCollection ndxScripts = trg.Script();
                    filename = Path.Combine(workingdir, "dbo." + myTable.Name + "." + trg.Name + ".trig.sql");
                    /* Generating CREATE TABLE command */
                    ndxScripts = trg.Script();
                    String str = ConvertStringArrayToString(ndxScripts);
                    File.WriteAllText(filename, str);

                }
                catch (Exception)
                {

                }
            }

        }

        #endregion

        private void GenerateStoredprocedureScript(Server CurrServer, string CurrdbName)
        {
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "StoredProcedures");
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.ScriptDrops = false;
            scriptOptions.WithDependencies = true;
            scriptOptions.Indexes = true;   // To include indexes
            scriptOptions.DriAllConstraints = true;   // to include r
            scriptOptions.Permissions = true;
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (StoredProcedure mysp in CurrDatabase.StoredProcedures)
            {
                try
                {
                    if (mysp.IsSystemObject == false)
                    {
                        string drop = string.Format("IF OBJECTPROPERTY(object_id('dbo.{0}'), N'IsProcedure') = 1{1} DROP PROCEDURE [dbo].[{0}]{1}GO{1}", mysp.Name, System.Environment.NewLine);
                        StringCollection tableScripts = mysp.Script(scriptOptions);
                        filename = Path.Combine(workingdir, "dbo." + mysp.Name + ".proc.sql");
                        tableScripts = mysp.Script();
                        String str = ConvertStringArrayToString(tableScripts);

                        File.WriteAllText(filename, drop + str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void GenerateUserDefinedFunctionsScript(Server CurrServer, string CurrdbName)
        {
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Functions");
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.ScriptDrops = false;
            scriptOptions.WithDependencies = true;
            scriptOptions.Indexes = true;   // To include indexes
            scriptOptions.DriAllConstraints = true;   // to include r
            scriptOptions.Permissions = true;
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (UserDefinedFunction myfns in CurrDatabase.UserDefinedFunctions)
            {
                try
                {
                    if (myfns.IsSystemObject == false)
                    {
                        string drop = string.Format("IF OBJECTPROPERTY(object_id('dbo.{0}'), N'IsProcedure') = 1{1} DROP FUNCTION [dbo].[{0}]{1}GO{1}", myfns.Name, System.Environment.NewLine);
                        StringCollection tableScripts = myfns.Script(scriptOptions);
                        filename = Path.Combine(workingdir, "dbo." + myfns.Name + ".function.sql");
                        tableScripts = myfns.Script();
                        String str = ConvertStringArrayToString(tableScripts);

                        File.WriteAllText(filename, drop + str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        #region Security

        private void GenerateApplicationRolesScript(Server CurrServer, string CurrdbName)
        {
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Security\Roles\Application Roles");
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.ScriptDrops = false;
            scriptOptions.WithDependencies = true;
            scriptOptions.Indexes = true;   // To include indexes
            scriptOptions.DriAllConstraints = true;   // to include r
            scriptOptions.Permissions = true;
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (ApplicationRole myApproles in CurrDatabase.ApplicationRoles)
            {

                try
                {
                    StringCollection tableScripts = myApproles.Script(scriptOptions);
                    filename = Path.Combine(workingdir, "dbo." + myApproles.Name + ".approle.sql");
                    tableScripts = myApproles.Script();
                    String str = ConvertStringArrayToString(tableScripts);

                    File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                }
                catch (Exception)
                {

                }
            }

        }

        private void GenerateDatabaseRolesScript(Server CurrServer, string CurrdbName)
        {
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Security\Roles\Database Roles");
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.ScriptDrops = false;
            scriptOptions.WithDependencies = true;
            scriptOptions.Indexes = true;   // To include indexes
            scriptOptions.DriAllConstraints = true;   // to include r
            scriptOptions.Permissions = true;
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (DatabaseRole dbrole in CurrDatabase.Roles)
            {
                if (dbrole.IsFixedRole || dbrole.Name == "public")
                {
                    continue;
                }
                try
                {
                    StringCollection tableScripts = dbrole.Script(scriptOptions);
                    filename = Path.Combine(workingdir, "dbo." + dbrole.Name + ".role.sql");
                    tableScripts = dbrole.Script();
                    String str = ConvertStringArrayToString(tableScripts);

                    File.WriteAllText(filename,  str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                }
                catch (Exception)
                {

                }
            }
        }

        private void GenerateSecuritySchemaScript(Server CurrServer, string CurrdbName)
        {
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Security\Schemas");
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.ScriptDrops = false;
            scriptOptions.WithDependencies = true;
            scriptOptions.Indexes = true;   // To include indexes
            scriptOptions.DriAllConstraints = true;   // to include r
            scriptOptions.Permissions = true;
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (Schema mysecschema in CurrDatabase.Schemas)
            {
                if (mysecschema.IsSystemObject == false)
                {
                    try
                    {
                        StringCollection tableScripts = mysecschema.Script(scriptOptions);
                        filename = Path.Combine(workingdir, "dbo." + mysecschema.Name + ".schema.sql");
                        tableScripts = mysecschema.Script();
                        String str = ConvertStringArrayToString(tableScripts);

                        File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void GenerateUsersScript(Server CurrServer, string CurrdbName)
        {
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Security\Schemas");
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.ScriptDrops = false;
            scriptOptions.WithDependencies = true;
            scriptOptions.Indexes = true;   // To include indexes
            scriptOptions.DriAllConstraints = true;   // to include r
            scriptOptions.Permissions = true;
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (User user in CurrDatabase.Users)
            {
                //Skips system users
                if (
                    user.Name == "dbo"
                 || user.Name == "guest"
                 || user.Name == "INFORMATION_SCHEMA"
                 || user.Name == "sys"
                 || user.Name == @"BUILTIN\Administrators"
                 || user.IsSystemObject
               )
                {
                    continue;
                }
                if (user.IsSystemObject == false)
                {
                    try
                    {
                        StringCollection tableScripts = user.Script(scriptOptions);
                        filename = Path.Combine(workingdir, "dbo." + user.Name + ".user.sql");
                        tableScripts = user.Script();
                        String str = ConvertStringArrayToString(tableScripts);

                        File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        #endregion

        #region views

        private void GenerateViewsScript(Server CurrServer, string CurrdbName)
        {
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Views");
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.ScriptDrops = false;
            scriptOptions.WithDependencies = true;
            scriptOptions.Indexes = true;   // To include indexes
            scriptOptions.DriAllConstraints = true;   // to include r
            scriptOptions.Permissions = true;
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (View myvws in CurrDatabase.Views)
            {
                GenerateViewIndexScripts(workingdir, myvws);
                GenerateViewTriggersScripts(workingdir, myvws);
                try
                {
                    if (myvws.IsSystemObject == false)
                    {
                        StringCollection tableScripts = myvws.Script(scriptOptions);
                        filename = Path.Combine(workingdir, "dbo." + myvws.Name + ".views.sql");
                        tableScripts = myvws.Script();
                        String str = ConvertStringArrayToString(tableScripts);

                        File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void GenerateViewIndexScripts(string workingdir, View myView)
        {

            string filename = String.Empty;
            workingdir = Path.Combine(workingdir, "Index");
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (Index ndx in myView.Indexes)
            {
                try
                {
                    StringCollection ndxScripts = ndx.Script();
                    ndxScripts = ndx.Script();
                    String str = ConvertStringArrayToString(ndxScripts);
                    File.WriteAllText(filename, str);
                }

                catch (Exception)
                {

                }
            }
        }

        private void GenerateViewTriggersScripts(string workingdir, View myView)
        {

            string filename = String.Empty;
            workingdir = Path.Combine(workingdir, "Triggers");
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (Trigger trg in myView.Triggers)
            {
                try
                {
                    StringCollection ndxScripts = trg.Script();
                    filename = Path.Combine(workingdir, "dbo." + myView.Name + "." + trg.Name + ".trig.sql");
                    /* Generating CREATE TABLE command */
                    ndxScripts = trg.Script();
                    String str = ConvertStringArrayToString(ndxScripts);
                    File.WriteAllText(filename, str);

                }
                catch (Exception)
                {

                }
            }

        }

        #endregion

        private void GenerateDatabaseTriggersScript(Server CurrServer, string CurrdbName)
        {
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"\Database Triggers");
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.ScriptDrops = false;
            scriptOptions.WithDependencies = true;
            scriptOptions.Indexes = true;   // To include indexes
            scriptOptions.DriAllConstraints = true;   // to include r
            scriptOptions.Permissions = true;
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (DatabaseDdlTrigger mysp in CurrDatabase.Triggers)
            {
                try
                {
                    if (mysp.IsSystemObject == false)
                    {
                        StringCollection tableScripts = mysp.Script(scriptOptions);
                        filename = Path.Combine(workingdir, "dbo." + mysp.Name + ".ddltrigger.sql");
                        tableScripts = mysp.Script();
                        String str = ConvertStringArrayToString(tableScripts);

                        File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        static string ConvertStringArrayToString(StringCollection strcoll)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            foreach (string value in strcoll)
            {
                builder.Append(value);
                builder.Append('\n');
            }
            return builder.ToString() + "\nGO\n";
        }
    }
}
