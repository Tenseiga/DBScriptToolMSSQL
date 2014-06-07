//using System;
//using System.IO;
//using System.Data;
//using System.Text;
//using System.Data.SqlClient;
//using System.Collections.Generic;
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Common;
//using System.Collections.Specialized;
//using Microsoft.SqlServer.Management.Sdk.Sfc;
//namespace DBScripter
//{
//    class ScriptDB
//    {
//        static int ObjectCnt = 0;

//        public void ScriptDBobjects(string[] args)
//        {
//            DateTime began = DateTime.Now;

//            // Either 3 or 5 parameters are required in the sequence shown in the assignments
//            if (args.Length != 3 && args.Length != 5)
//            {
//                PrintInstructions();
//                return;
//            }

//            string argPath = args[0]
//                 , argServer = args[1]
//                 , argDatabase = args[2];

//            ServerConnection conn = new ServerConnection();

//            if (args.Length == 5)
//            {
//                conn.LoginSecure = false;
//                conn.Login = args[3];
//                conn.Password = args[4];
//            }
//            conn.ServerInstance = argServer;
//            conn.DatabaseName = argDatabase;

//            Server srvr = new Server(conn);
//            string srvrVersion;

//            try
//            {
//                srvr.Initialize();
//                srvr.SetDefaultInitFields(true);
//                srvrVersion = srvr.Information.VersionString;
//            }
//            catch
//            {
//                Console.WriteLine(
//                                     "\nERROR: Connection to Server " + argServer
//                                   + ", Database " + argDatabase + " failed\n"
//                                 );
//                return;
//            }

//            Database db = srvr.Databases[argDatabase];
//            if (db == null)
//            {
//                Console.WriteLine("\nERROR: Database " + argDatabase + " does not exist\n");
//                return;
//            }

//            // srvrPath will be our root directory
//            string srvrPath = argPath;
//            while (srvrPath.EndsWith(@"\"))
//            {
//                srvrPath = srvrPath.Substring(0, srvrPath.Length - 1);
//            }
//            srvrPath += @"\" + argServer.Replace(@"\", @"$").ToUpper();
//            if (Directory.Exists(srvrPath) == false)
//            {
//                try
//                {
//                    Directory.CreateDirectory(srvrPath);
//                }
//                catch
//                {
//                    Console.WriteLine("\nERROR: the Path \"" + argPath + "\" does not exist\n");
//                    return;
//                }
//            }

//            string dbPath = srvrPath + @"\" + argDatabase + @"\Schema Objects";
//            // if dbPath already exists, delete it -- we want to start with a clean slate
//            if (Directory.Exists(dbPath))
//            {
//                Directory.Delete(dbPath, true); // true deletes any files and subdirectories
//            }
//            // recreate the database directory
//            Directory.CreateDirectory(dbPath);

//            //Microsoft.SqlServer.Management.Sdk.Sfc.Urn[] urn = new Microsoft.SqlServer.Management.Sdk.Sfc.Urn[1];
//            Scripter scrp = new Scripter(srvr);
//            string filename;

//            // set common scipter options
//            scrp.Options.AnsiFile = true;
//            scrp.Options.AppendToFile = false;
//            scrp.Options.ContinueScriptingOnError = true;
//            scrp.Options.ExtendedProperties = true;
//            scrp.Options.IncludeHeaders = true;
//            scrp.Options.PrimaryObject = true;
//            scrp.Options.SchemaQualify = true;
//            scrp.Options.ToFileOnly = true;
//            scrp.Options.ConvertUserDefinedDataTypesToBaseType = true;

//            if (srvrVersion.StartsWith("8."))
//            {
//                scrp.Options.TargetServerVersion = SqlServerVersion.Version80;
//                scrp.Options.Permissions = false;
//            }
//            else if (srvrVersion.StartsWith("9."))
//            {
//                scrp.Options.TargetServerVersion = SqlServerVersion.Version90;
//                scrp.Options.Permissions = true;
//            }
//            else
//            {
//                Console.WriteLine("\nERROR: Only SQL Server 2000 and SQL Server 2005 databases are supported.\n");
//                return;
//            }

//            /*******************************************************************************
//            *
//            * Script the Database
//            *
//            *******************************************************************************/
//            //urn[0] = db.Urn;

//            filename = dbPath + @"\" + db.Name + ".database.sql";
//            Console.WriteLine("Database: " + db.Name);

//            // script the database
//            ScriptIt(scrp, filename);

//            /*******************************************************************************
//            *
//            * Script Tables
//            *
//            *******************************************************************************/
//            string tblPath = dbPath + @"\Tables";
//            Directory.CreateDirectory(tblPath);

//            foreach (Table tbl in db.Tables)
//            {
//                // skip system tables
//                if (tbl.IsSystemObject)
//                {
//                    continue;
//                }

//                scrp.Options.DriAll = false;
//                scrp.Options.Indexes = false;
//                scrp.Options.Triggers = false;
//                scrp.Options.NoFileGroup = false;
//                scrp.Options.DriForeignKeys = false;  // added 2007.02.19
//                scrp.Options.NoTablePartitioningSchemes = false;

//                filename = tblPath + @"\" + tbl.Schema.Replace(@"\", @"$") + "." + tbl.Name + ".table.sql";
//                Console.WriteLine("  Table: " + tbl.Schema + "." + tbl.Name);

//                // script the table
//                Scripttables(tbl, scrp, filename);

//                // SQL-SMO can generate either SQL Server 2000 or SQL Server 2005 formats.
//                // But unfortunately it generates permissions at the column level for 
//                // SQL Server 2000 databases, so we will roll our own with ADO.NET [2007.03.08]
//                if (srvrVersion.StartsWith("8."))
//                {
//                    string connect = "Data Source=" + argServer
//                                   + ";Initial Catalog=" + argDatabase;

//                    if (args.Length == 5)
//                    {
//                        connect += ";Trusted_Connection=false"
//                                 + ";User ID=" + args[3]
//                                 + ";Password=" + args[4];
//                    }
//                    else
//                    {
//                        connect += ";Integrated Security=SSPI";
//                    }

//                    string command = "EXEC sp_helprotect"
//                                   + "  @name = '" + tbl.Name + "'"
//                                   + ", @grantorname = '" + tbl.Schema + "'";

//                    ScriptPermissions(connect, command, filename);
//                }

//                /****************************************************************************
//                *
//                * Script Table Indexes
//                *
//                ****************************************************************************/
//                string keyPath = tblPath + @"\Keys";
//                Directory.CreateDirectory(keyPath);

//                string ndxPath = tblPath + @"\Indexes";
//                Directory.CreateDirectory(ndxPath);

//                foreach (Index ndx in tbl.Indexes)
//                {
//                    Console.WriteLine("    Index: " + ndx.Name);
//                    //urn[0] = ndx.Urn;

//                    if (ndx.IndexKeyType.ToString() == "DriUniqueKey")
//                    {
//                        filename = keyPath + @"\" + tbl.Schema.Replace(@"\", @"$") + "." + tbl.Name
//                                 + "." + ndx.Name + ".ukey.sql";
//                    }
//                    else if (ndx.IndexKeyType.ToString() == "DriPrimaryKey")
//                    {
//                        filename = keyPath + @"\" + tbl.Schema.Replace(@"\", @"$") + "." + tbl.Name
//                                 + "." + ndx.Name + ".pkey.sql";
//                    }
//                    else
//                    {
//                        filename = ndxPath + @"\" + tbl.Schema.Replace(@"\", @"$") + "." + tbl.Name
//                                 + "." + ndx.Name + ".index.sql";
//                    }

//                    // script the index
//                    ScriptIt(scrp, filename);
//                }

//                /****************************************************************************
//                *
//                * Script Table Triggers
//                *
//                ****************************************************************************/
//                string trgPath = tblPath + @"\Triggers";
//                Directory.CreateDirectory(trgPath);

//                foreach (Trigger trg in tbl.Triggers)
//                {
//                    Console.WriteLine("    Trigger: " + trg.Name);
//                    //urn[0] = trg.Urn;

//                    filename = trgPath + @"\" + tbl.Schema.Replace(@"\", @"$") + "." + tbl.Name
//                             + "." + trg.Name + ".trigger.sql";

//                    // script the trigger
//                    ScriptIt(scrp, filename);
//                }

//                /****************************************************************************
//                *
//                * Script Check Constraints
//                *
//                ****************************************************************************/
//                string chkPath = tblPath + @"\Constraints";
//                Directory.CreateDirectory(chkPath);

//                scrp.Options.DriChecks = true;

//                foreach (Check chk in tbl.Checks)
//                {
//                    Console.WriteLine("    Constraint: " + chk.Name);

//                    filename = chkPath + @"\" + tbl.Schema.Replace(@"\", @"$") + "." + tbl.Name
//                             + "." + chk.Name + ".chkconst.sql";

//                    // script the constraint
//                    ScriptIt(scrp, filename);
//                }
//            }


//            //        /****************************************************************************
//            //        *
//            //        * Script Default Constraints
//            //        *
//            //        ****************************************************************************/
//            //        string defPath = chkPath;

//            //        scrp.Options.DriChecks = false;

//            //        foreach (Column col in tbl.Columns)
//            //        {
//            //            if (col.DefaultConstraint != null)
//            //            {
//            //                Console.WriteLine("    Constraint: " + col.DefaultConstraint.Name);
//            //                urn[0] = col.DefaultConstraint.Urn;

//            //                filename = defPath + @"\" + tbl.Schema.Replace(@"\", @"$") + "." + tbl.Name
//            //                         + "." + col.DefaultConstraint.Name + ".defconst.sql";

//            //                // script the constraint
//            //                ScriptIt(urn, scrp, filename);
//            //            }
//            //        }

//            //        /****************************************************************************
//            //        *
//            //        * Script Foreign Keys
//            //        *
//            //        ****************************************************************************/
//            //        scrp.Options.DriForeignKeys = true;
//            //        scrp.Options.SchemaQualifyForeignKeysReferences = true;  // added 2007.02.19

//            //        foreach (ForeignKey fk in tbl.ForeignKeys)
//            //        {
//            //            Console.WriteLine("    Foreign Key: " + fk.Name);
//            //            urn[0] = fk.Urn;

//            //            filename = keyPath + @"\" + tbl.Schema.Replace(@"\", @"$") + "." + tbl.Name
//            //                     + "." + fk.Name + ".fkey.sql";

//            //            // script the constraint
//            //            ScriptIt(urn, scrp, filename);
//            //        }
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script Views
//            //    *
//            //    *******************************************************************************/
//            //    string vwPath = dbPath + @"\Views";
//            //    Directory.CreateDirectory(vwPath);

//            //    foreach (View vw in db.Views)
//            //    {
//            //        // skip system views
//            //        if (vw.IsSystemObject)
//            //        {
//            //            continue;
//            //        }

//            //        urn[0] = vw.Urn;

//            //        scrp.Options.Indexes = false;
//            //        scrp.Options.Triggers = false;

//            //        filename = vwPath + @"\" + vw.Schema.Replace(@"\", @"$") + "." + vw.Name + ".view.sql";
//            //        Console.WriteLine("  View: " + vw.Schema + "." + vw.Name);

//            //        // script the view
//            //        ScriptIt(urn, scrp, filename);

//            //        // SQL-SMO can generate either SQL Server 2000 or SQL Server 2005 formats.
//            //        // But unfortunately it generates permissions at the column level for 
//            //        // SQL Server 2000 databases, so we will roll our own with ADO.NET [2007.03.08]
//            //        if (srvrVersion.StartsWith("8."))
//            //        {
//            //            string connect = "Data Source=" + argServer
//            //                           + ";Initial Catalog=" + argDatabase;

//            //            if (args.Length == 5)
//            //            {
//            //                connect += ";Trusted_Connection=false"
//            //                         + ";User ID=" + args[3]
//            //                         + ";Password=" + args[4];
//            //            }
//            //            else
//            //            {
//            //                connect += ";Integrated Security=SSPI";
//            //            }

//            //            string command = "EXEC sp_helprotect"
//            //                           + "  @name = '" + vw.Name + "'"
//            //                           + ", @grantorname = '" + vw.Schema + "'";

//            //            ScriptPermissions(connect, command, filename);
//            //        }

//            //        /****************************************************************************
//            //        *
//            //        * Script View Indexes
//            //        *
//            //        ****************************************************************************/
//            //        string ndxPath = vwPath + @"\Indexes";
//            //        Directory.CreateDirectory(ndxPath);

//            //        foreach (Index ndx in vw.Indexes)
//            //        {
//            //            Console.WriteLine("    Index: " + ndx.Name);
//            //            urn[0] = ndx.Urn;

//            //            filename = ndxPath + @"\" + vw.Schema.Replace(@"\", @"$") + "." + vw.Name
//            //                     + "." + ndx.Name + ".index.sql";

//            //            // script the index
//            //            ScriptIt(urn, scrp, filename);
//            //        }

//            //        /****************************************************************************
//            //        *
//            //        * Script View Triggers
//            //        *
//            //        ****************************************************************************/
//            //        string trgPath = vwPath + @"\Triggers";
//            //        Directory.CreateDirectory(trgPath);

//            //        foreach (Trigger trg in vw.Triggers)
//            //        {
//            //            Console.WriteLine("    Trigger: " + trg.Name);
//            //            urn[0] = trg.Urn;

//            //            filename = trgPath + @"\" + vw.Schema.Replace(@"\", @"$") + "." + vw.Name
//            //                     + "." + trg.Name + ".trigger.sql";

//            //            // script the trigger
//            //            ScriptIt(urn, scrp, filename);
//            //        }
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script Stored Procedures
//            //    *
//            //    *******************************************************************************/
//            //    scrp.Options.Permissions = true;
//            //    string procPath = dbPath + @"\Stored Procedures";
//            //    Directory.CreateDirectory(procPath);

//            //    foreach (StoredProcedure proc in db.StoredProcedures)
//            //    {
//            //        // skip system procedures
//            //        if (proc.IsSystemObject)
//            //        {
//            //            continue;
//            //        }

//            //        urn[0] = proc.Urn;

//            //        filename = procPath + @"\" + proc.Schema.Replace(@"\", @"$") + "." + proc.Name + ".proc.sql";
//            //        Console.WriteLine("  Stored Procedure: " + proc.Schema + "." + proc.Name);

//            //        // script the procedure
//            //        ScriptIt(urn, scrp, filename);
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script User Defined Functions
//            //    *
//            //    *******************************************************************************/
//            //    string funcPath = dbPath + @"\Functions";
//            //    Directory.CreateDirectory(funcPath);

//            //    foreach (UserDefinedFunction func in db.UserDefinedFunctions)
//            //    {
//            //        // skip system functions
//            //        if (func.IsSystemObject)
//            //        {
//            //            continue;
//            //        }

//            //        urn[0] = func.Urn;

//            //        filename = funcPath + @"\" + func.Schema.Replace(@"\", @"$") + "." + func.Name + ".function.sql";
//            //        Console.WriteLine("  User Defined Function: " + func.Schema + "." + func.Name);

//            //        // script the function
//            //        ScriptIt(urn, scrp, filename);
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script Application Roles
//            //    *
//            //    *******************************************************************************/
//            //    string securPath = dbPath + @"\Security";
//            //    Directory.CreateDirectory(securPath);

//            //    string approlePath = securPath + @"\Roles\Application Roles";
//            //    Directory.CreateDirectory(approlePath);

//            //    foreach (ApplicationRole approle in db.ApplicationRoles)
//            //    {
//            //        urn[0] = approle.Urn;

//            //        filename = approlePath + @"\" + approle.Name + ".approle.sql";
//            //        Console.WriteLine("  Application Role: " + approle.Name);

//            //        // script the role
//            //        ScriptIt(urn, scrp, filename);
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script Database Roles
//            //    *
//            //    *******************************************************************************/
//            //    string dbrolePath = securPath + @"\Roles\Database Roles";
//            //    Directory.CreateDirectory(dbrolePath);

//            //    foreach (DatabaseRole dbrole in db.Roles)
//            //    {
//            //        // skip fixed database roles
//            //        if (dbrole.IsFixedRole || dbrole.Name == "public")
//            //        {
//            //            continue;
//            //        }

//            //        urn[0] = dbrole.Urn;

//            //        filename = dbrolePath + @"\" + dbrole.Name + ".role.sql";
//            //        Console.WriteLine("  Database Role: " + dbrole.Name);

//            //        // script the role
//            //        ScriptIt(urn, scrp, filename);
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script Schemas
//            //    *
//            //    *******************************************************************************/
//            //    if (srvrVersion.StartsWith("9."))
//            //    {
//            //        string schemaPath = securPath + @"\Schemas";
//            //        Directory.CreateDirectory(schemaPath);

//            //        foreach (Schema schema in db.Schemas)
//            //        {
//            //            // skip system & fixed schemas
//            //            if (
//            //                    schema.Name.StartsWith("db_")
//            //                //added 2007.02.19
//            //                 || schema.Name == "dbo"
//            //                 || schema.Name == "guest"
//            //                 || schema.Name == "INFORMATION_SCHEMA"
//            //                 || schema.Name == "sys"
//            //               )
//            //            {
//            //                continue;
//            //            }

//            //            urn[0] = schema.Urn;

//            //            filename = schemaPath + @"\" + schema.Name.Replace(@"\", @"$") + ".schema.sql";
//            //            Console.WriteLine("  Schema: " + schema.Name);

//            //            // script the schema
//            //            ScriptIt(urn, scrp, filename);
//            //        }
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script Users
//            //    *
//            //    *******************************************************************************/
//            //    string userPath = securPath + @"\Users";
//            //    Directory.CreateDirectory(userPath);

//            //    foreach (User user in db.Users)
//            //    {
//            //        // skip system users [added 2007.02.19]
//            //        if (
//            //                user.Name == "dbo"
//            //             || user.Name == "guest"
//            //             || user.Name == "INFORMATION_SCHEMA"
//            //             || user.Name == "sys"
//            //             || user.Name == @"BUILTIN\Administrators"
//            //             || user.IsSystemObject
//            //           )
//            //        {
//            //            continue;
//            //        }

//            //        urn[0] = user.Urn;

//            //        filename = userPath + @"\" + user.Name.Replace(@"\", @"$") + ".user.sql";
//            //        Console.WriteLine("  User: " + user.Name);

//            //        // script the schema
//            //        ScriptIt(urn, scrp, filename);
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script Full Text Catalogs
//            //    *
//            //    ******************************************************************************/
//            //    if (srvrVersion.StartsWith("9."))
//            //    {
//            //        string storagePath = dbPath + @"\Storage";
//            //        Directory.CreateDirectory(storagePath);

//            //        string catPath = storagePath + @"\Full Text Catalogs";
//            //        Directory.CreateDirectory(catPath);

//            //        foreach (FullTextCatalog cat in db.FullTextCatalogs)
//            //        {
//            //            urn[0] = cat.Urn;

//            //            filename = catPath + @"\" + cat.Name + ".fulltext.sql";
//            //            Console.WriteLine("  Full Text Catalog: " + cat.Name);

//            //            // script the full text catalog
//            //            ScriptIt(urn, scrp, filename);
//            //        }
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script Database Triggers
//            //    *
//            //    *******************************************************************************/
//            //    if (srvrVersion.StartsWith("9."))
//            //    {
//            //        string dbtrgPath = dbPath + @"\Database Triggers";
//            //        Directory.CreateDirectory(dbtrgPath);

//            //        foreach (DatabaseDdlTrigger dbtrg in db.Triggers)
//            //        {
//            //            urn[0] = dbtrg.Urn;

//            //            filename = dbtrgPath + @"\" + dbtrg.Name + ".ddltrigger.sql";
//            //            Console.WriteLine("  Database Trigger: " + dbtrg.Name);

//            //            // script the database trigger
//            //            ScriptIt(urn, scrp, filename);
//            //        }
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script Synonyms
//            //    *
//            //    *******************************************************************************/
//            //    if (srvrVersion.StartsWith("9."))
//            //    {
//            //        string synPath = dbPath + @"\Synonyms";
//            //        Directory.CreateDirectory(synPath);

//            //        foreach (Synonym syn in db.Synonyms)
//            //        {
//            //            urn[0] = syn.Urn;

//            //            filename = synPath + @"\" + syn.Schema.Replace(@"\", @"$") + "." + syn.Name + ".synonym.sql";
//            //            Console.WriteLine("  Synonym: " + syn.Schema + "." + syn.Name);

//            //            // script the synonym
//            //            ScriptIt(urn, scrp, filename);
//            //        }
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script User-defined Types
//            //    *
//            //    *******************************************************************************/
//            //    string typePath = dbPath + @"\Types";
//            //    Directory.CreateDirectory(typePath);

//            //    string uddtPath = typePath + @"\User-defined Data Types";
//            //    Directory.CreateDirectory(uddtPath);

//            //    foreach (UserDefinedDataType uddt in db.UserDefinedDataTypes)
//            //    {
//            //        urn[0] = uddt.Urn;

//            //        filename = uddtPath + @"\" + uddt.Schema.Replace(@"\", @"$") + "." + uddt.Name + ".uddt.sql";
//            //        Console.WriteLine("  User-defined Type: " + uddt.Schema + "." + uddt.Name);

//            //        // script the user-defined data type
//            //        ScriptIt(urn, scrp, filename);
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Script XML Schema Collections
//            //    *
//            //    *******************************************************************************/
//            //    if (srvrVersion.StartsWith("9."))
//            //    {
//            //        string xmlPath = typePath + @"\XML Schema Collections";
//            //        Directory.CreateDirectory(xmlPath);

//            //        foreach (XmlSchemaCollection xml in db.XmlSchemaCollections)
//            //        {
//            //            urn[0] = xml.Urn;

//            //            filename = xmlPath + @"\" + xml.Schema.Replace(@"\", @"$") + "." + xml.Name + ".xmlschema.sql";
//            //            Console.WriteLine("  XML Schema Collection: " + xml.Schema + "." + xml.Name);

//            //            // script the xml schema collection
//            //            ScriptIt(scrp, filename);
//            //        }
//            //    }

//            //    /*******************************************************************************
//            //    *
//            //    * Done
//            //    *
//            //    *******************************************************************************/
//            //    DateTime ended = DateTime.Now;

//            //    Console.WriteLine("\nBegan: " + began.ToLongTimeString());
//            //    Console.WriteLine("Ended: " + ended.ToLongTimeString());
//            //    Console.WriteLine("\nNumber of objects scripted: " + ObjectCnt.ToString());
//            //    Console.WriteLine("\nDone.\n");
//        }

//        static void ScriptIt(Scripter scrp, string filename)
//        {
//            scrp.Options.FileName = filename;

//            try
//            {
//                StringCollection sc = scrp.Script(urn);
//                String str = ConvertStringArrayToString(sc);
//                File.WriteAllText(filename, str);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                return;
//            }

//            ObjectCnt++;
//        }


//        static void Scripttables(Table tb, Scripter scrp, string filename)
//        {
//            scrp.Options.FileName = filename;

//            try
//            {
//                StringCollection sc = scrp.Script(new Urn[] { tb.Urn });
//                String str = ConvertStringArrayToString(sc);
//                File.WriteAllText(filename, str);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                return;
//            }

//            ObjectCnt++;
//        }
//        static string ConvertStringArrayToString(StringCollection strcoll)
//        {
//            //
//            // Concatenate all the elements into a StringBuilder.
//            //
//            StringBuilder builder = new StringBuilder();
//            foreach (string value in strcoll)
//            {
//                builder.Append(value);
//                builder.Append('\n');
//            }
//            return builder.ToString();
//        }

//        static void ScriptPermissions(string connect, string command, string filename)
//        {
//            SqlConnection cn = new SqlConnection(connect);
//            cn.Open();
//            SqlCommand cmd = new SqlCommand(command, cn);

//            // issue the query
//            SqlDataReader rdr = null;
//            try
//            {
//                rdr = cmd.ExecuteReader();
//            }
//            catch
//            {
//                // Some tables/views don't have granted permissions; ignore error
//                ;
//            }

//            // if the query returned any rows, constuct the permissions
//            string text = "";
//            StringBuilder perms = new StringBuilder(1024);
//            if (rdr != null && !rdr.IsClosed)
//            {
//                if (rdr.HasRows)
//                {
//                    while (rdr.Read())
//                    {
//                        if (rdr["ProtectType"].ToString() == "Grant_WGO ")
//                        {
//                            text = "GRANT";
//                        }
//                        else
//                        {
//                            text = rdr["ProtectType"].ToString().ToUpper().Trim();
//                        }
//                        text += " " + rdr["Action"].ToString().ToUpper().Trim();

//                        if (
//                                rdr["Column"].ToString().Trim() != "(All)"
//                             && rdr["Column"].ToString().Trim() != "(All+New)"
//                             && rdr["Column"].ToString().Trim() != "."
//                           )
//                        {
//                            text += " ( [" + rdr["Column"].ToString().Trim() + "] )";
//                        }

//                        text += " ON [" + rdr["Owner"].ToString().Trim() + "].["
//                              + rdr["Object"].ToString().Trim() + "] TO "
//                              + rdr["Grantee"].ToString().Trim();

//                        if (rdr["ProtectType"].ToString() == "Grant_WGO ")
//                        {
//                            text += " WITH GRANT OPTION";
//                        }

//                        perms.AppendLine(text);
//                        perms.AppendLine("GO\r\n");
//                    }
//                    // convert the StringBuilder back to a string for the StreamWriter
//                    text = "\r\n" + perms.ToString();

//                    FileStream FS = new FileStream(filename, FileMode.Append, FileAccess.Write);
//                    StreamWriter SW = new StreamWriter(FS, Encoding.ASCII);

//                    // write the permissions to the file
//                    try
//                    {
//                        SW.Write(text);
//                    }
//                    catch (Exception e)
//                    {
//                        Console.WriteLine(e);
//                    }
//                    finally
//                    {
//                        SW.Flush();
//                        SW.Dispose();
//                        FS.Dispose();
//                    }
//                }
//                cn.Close();
//            }
//        }

//        static void PrintInstructions()
//        {
//            Console.WriteLine("\n  Usage:");
//            Console.Write("\n  ScriptDB ");
//            Console.Write("\"<Root Path>\" ");
//            Console.Write("<ServerName> ");
//            Console.WriteLine("<DatabaseName> [<Username> <Password>]\n"); // 2007.02.19
//            Console.WriteLine("  Where <Root Path> is the starting point for writing files.\n");
//            Console.WriteLine("  That is, at the Root Path a folder named after the server");
//            Console.WriteLine("  will be created [if it does not already exist].\n");
//            Console.WriteLine("  Below the ServerName folder is where the DatabaseName folder");
//            Console.WriteLine("  will go. If the DatabaseName folder already exists, it will");
//            Console.WriteLine("  be deleted and then recreated. This ensures a clean set of");
//            Console.WriteLine("  files which will represent the current state of the database.\n");
//            Console.WriteLine("  Below the DatabaseName folder will be a folder called");
//            Console.WriteLine("  \"Schema Objects\", and all other folders and files will be");
//            Console.WriteLine("  created below this folder.\n");
//            // added 2007.02.19
//            Console.WriteLine("  Optionally, for users who cannot connect using Windows");
//            Console.WriteLine("  Authentication, the Username and Password may be entered as");
//            Console.WriteLine("  the 4th and 5th parameters.\n");

//            Console.WriteLine("  Note: only SQL Server 2000 & SQL Server 2005 databases are");
//            Console.WriteLine("  supported by ScriptDB.exe.");
//        }
//    }
//}
