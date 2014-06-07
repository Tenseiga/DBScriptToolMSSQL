using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Specialized;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System.Collections;
using System.Diagnostics;

/*
     Author: Srinath
     Date: 14 july 2011
     * For Scripting the datadase in sql server 2008 and 2008R2.Since the smo dlls are made available in this package itself this tool should work with sql server 2005 also.:) i m not sure of it..try ur luck..
     * Note new smo instaces are created for each object during scripting so to increase the performance   
 * 
 * Enhancements: This tool will script all the components but instead only the required components can be scripted:) This enhancement was done on 18 jul 2011
 */

namespace DBScripter
{
    public partial class DbScpritForm : Form
    {
        string rootdirectory = String.Empty;
        string maindir = string.Empty;
        string serverName = String.Empty;
        string password = String.Empty;
        string userName = String.Empty;
        DataSet dssummary = new DataSet();
        string Logfilename = String.Empty;
        DateTime starttime = DateTime.Now;
        DataTable dtSelectedObjects = new DataTable();
        ArrayList selvalues = new ArrayList();
        StringBuilder strCumulativetext = new StringBuilder();
        bool blnScriptData = false;

        public DbScpritForm()
        {
            InitializeComponent();
            btnnext2.Focus();
        }

        private void GetDatabaselist()
        {
            try
            {
                DataTable dtDatabases = new DataTable();
                string sqlcmd = "SELECT name as DatabaseName,database_id as DatabaseID,create_date as CreationDate FROM sys.databases";
                SqlDataReader SqlDR = null;
                string sqlconnstr = @"Server=" + txtServername.Text + ";Database=master;" + "User id=" + txtUsrname.Text + ";password=" + txtpass.Text;
                using (SqlConnection sqlConx = new SqlConnection(sqlconnstr))
                {
                    try
                    {
                        sqlConx.Open();
                        SqlCommand SqlCom = new System.Data.SqlClient.SqlCommand();
                        SqlCom.Connection = sqlConx;
                        SqlCom.CommandType = CommandType.Text;
                        SqlCom.CommandText = sqlcmd;
                        SqlDR = SqlCom.ExecuteReader();
                        dtDatabases.Load(SqlDR);

                    }
                    finally
                    {
                        if (SqlDR != null)
                            SqlDR.Dispose();
                        if (sqlConx != null)
                            sqlConx.Close();
                    }
                }
                dgDatabasesForScriptObj.DataSource = dtDatabases;
                dgDatabasesForScriptObj.FirstDisplayedScrollingColumnIndex = 1;
                dgDatabasesForScriptObj.AutoGenerateColumns = false;
                dgDatabasesForScriptObj.Focus();
                dgDatabasesForScriptObj.Visible = true;

            }
            catch (Exception ex)
            {
                string str = "Error Occured while Connecting to the database" + txtServername.Text;
                str += Environment.NewLine + "Exception: " + ex.Message;
                str += Environment.NewLine + "Inner Exception: " + ex.InnerException;
                str += Environment.NewLine + Environment.NewLine + "Please check the user name and password or check your network connectivity";
                MessageBox.Show(str, "RBT Database Scripting tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

        }

        private void GetTablesinaDatabase(string DBNAme)
        {
            try
            {
                DataTable dtDatabases = new DataTable();
                string sqlcmd = "SELECT name FROM sysobjects WHERE xtype='U'";
                SqlDataReader SqlDR = null;
                string sqlconnstr = @"Server=" + txtServername.Text + ";Database=" + DBNAme + ";User id=" + txtUsrname.Text + ";password=" + txtpass.Text;
                using (SqlConnection sqlConx = new SqlConnection(sqlconnstr))
                {
                    try
                    {
                        sqlConx.Open();
                        SqlCommand SqlCom = new System.Data.SqlClient.SqlCommand();
                        SqlCom.Connection = sqlConx;
                        SqlCom.CommandType = CommandType.Text;
                        SqlCom.CommandText = sqlcmd;
                        SqlDR = SqlCom.ExecuteReader();
                        dtDatabases.Load(SqlDR);

                    }
                    finally
                    {
                        if (SqlDR != null)
                            SqlDR.Dispose();
                        if (sqlConx != null)
                            sqlConx.Close();
                    }
                }

                dgTableNames.DataSource = dtDatabases;
                dgTableNames.FirstDisplayedScrollingColumnIndex = 1;
                dgTableNames.Focus();
            }
            catch (Exception ex)
            {
                string str = "Error Occured while Connecting to the database" + txtServername.Text;
                str += Environment.NewLine + "Exception: " + ex.Message;
                str += Environment.NewLine + "Inner Exception: " + ex.InnerException;
                str += Environment.NewLine + Environment.NewLine + "Please check the user name and password or check your network connectivity";
                MessageBox.Show(str, "RBT Database Scripting tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        #region ScriptingEngine

        #region Objects
        public void GenerateScripts(ArrayList DBlist)
        {
            try
            {
                timer1.Enabled = true;
                timer1.Start();
                timer1.Interval = 1000;
                starttime = DateTime.Now;
                timer1.Tick += new EventHandler(timer_Tick);
                string filename = String.Empty;
                int counter = 0;
                foreach (string dbnam in DBlist)
                {
                    StringBuilder strfinished = new StringBuilder();

                    String strpending = GetSummaryLabelText(0);

                    rootdirectory = Path.Combine(maindir, dbnam, "SchemaObjects");
                    if (!Directory.Exists(rootdirectory))
                        Directory.CreateDirectory(rootdirectory);

                    Logfilename = Path.Combine(maindir, dbnam, dbnam + "_Log.txt");
                    File.WriteAllText(Logfilename, "Log File for " + dbnam + Environment.NewLine + "Start Time :" + DateTime.Now + Environment.NewLine);
                    lblcurrentdb.Text = dbnam;
                    lblTables.Text = strpending;
                    Application.DoEvents();

                    if (chkScriptDB.Checked)
                    {
                        GenerateDatabaseScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(13);
                        Application.DoEvents();
                    }

                    //Actaul scripting start here
                    if (ScriptValidator(counter, 1))
                    {
                        GenerateTableScript(dbnam.ToString(), counter);
                        lblTables.Text = GetSummaryLabelText(1);
                    }
                    if (chktoSingleFile.Checked && strCumulativetext.Length != 0)
                    {
                        CreateSingleFile(dbnam.ToString(), "schema");
                    }
                    Application.DoEvents();

                    if (ScriptValidator(counter, 4))
                    {
                        GenerateViewsScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(2);
                        Application.DoEvents();
                    }
                    if (ScriptValidator(counter, 6))
                    {
                        GenerateUserDefinedFunctionsScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(4);
                        Application.DoEvents();
                    }
                    if (ScriptValidator(counter, 5))
                    {
                        GenerateStoredprocedureScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(3);
                        Application.DoEvents();
                    }

                    if (ScriptValidator(counter, 7))
                    {
                        GenerateApplicationRolesScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(5);
                        Application.DoEvents();

                        GenerateDatabaseRolesScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(6);
                        Application.DoEvents();
                    }

                    if (ScriptValidator(counter, 8))
                    {
                        GenerateSecuritySchemaScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(7);
                        Application.DoEvents();
                    }

                    if (ScriptValidator(counter, 9))
                    {
                        GenerateUsersScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(8);
                        Application.DoEvents();
                    }
                    if (ScriptValidator(counter, 10))
                    {
                        GenerateStorageFullTextCatalogsScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(9);
                        Application.DoEvents();
                    }
                    if (ScriptValidator(counter, 11))
                    {
                        GenerateDatabaseTriggersScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(10);
                        Application.DoEvents();
                    }
                    if (ScriptValidator(counter, 12))
                    {
                        GenerateSynonymsScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(11);
                        Application.DoEvents();
                    }
                    if (ScriptValidator(counter, 13))
                    {
                        GenerateUserdefinedTypesScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(12);
                        Application.DoEvents();

                        GenerateXMLSchemasScript(dbnam.ToString());
                        lblTables.Text = GetSummaryLabelText(13);
                        Application.DoEvents();
                    }

                    if (chktoSingleFile.Checked && strCumulativetext.Length != 0)
                    {
                        CreateSingleFile(dbnam.ToString(), "script");
                    }
                    createFinishLog();
                    counter++;
                }
                timer1.Stop();
                timer1.Dispose();
                MessageBox.Show("All Object Scripted Successfully ", "RBT Database Scripting tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                File.AppendAllText(Logfilename, ex.Message + ex.StackTrace);
                string str = "Error Occured while Scripting";
                str += Environment.NewLine + "Exception: " + ex.Message;
                str += Environment.NewLine + "Inner Exception: " + ex.InnerException;
                str += Environment.NewLine + Environment.NewLine + "Note: Not All Components were Scripted successfully. Please See the log for more deatils";
                MessageBox.Show(str, "RBT Database Scripting tool", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }

        }

        private bool ScriptValidator(int rowindex, int colindex)
        {
            if (dgChosenObjects.Rows[rowindex].Cells[colindex].Value != null && Convert.ToBoolean(dgChosenObjects.Rows[rowindex].Cells[colindex].Value) == true)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region ScriptEngineCore

        #region TableandTablerelatedobjectsSCriptgeneration

        private void GenerateTableScript(string CurrdbName, int counter)
        {
            int cnt = 0;
            string filename = String.Empty;

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            string workingdir = Path.Combine(rootdirectory, "Tables");
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            StringBuilder strFKCons = new StringBuilder();
            //ScriptingOptions scriptOptions = new ScriptingOptions();
            //scriptOptions.ScriptData = true;
            //scriptOptions.Indexes = false;
            //scriptOptions.Triggers = false;
            //scriptOptions.IncludeIfNotExists = true;
            //scriptOptions.ScriptSchema = false;
            //scriptOptions.AnsiFile = true;
            //scriptOptions.AppendToFile = false;
            //scriptOptions.ContinueScriptingOnError = true;
            //scriptOptions.ExtendedProperties = true;
            //scriptOptions.IncludeHeaders = true;
            //scriptOptions.PrimaryObject = true;
            //scriptOptions.SchemaQualify = true;
            //scriptOptions.ToFileOnly = true;
            //scriptOptions.ConvertUserDefinedDataTypesToBaseType = true;
            //scriptOptions.DriAll = false;

            //scriptOptions.NoFileGroup = false;
            //scriptOptions.DriForeignKeys = false;  
            //scriptOptions.NoTablePartitioningSchemes = false;
            StringBuilder strfkcons = new StringBuilder();

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);

            progressBar1.Maximum = CurrDatabase.Tables.Count;
            try
            {
                foreach (Table myTable in CurrDatabase.Tables)
                {
                    string chkconstr = String.Empty;
                    string defcon = String.Empty;
                    string fkcon = String.Empty;
                    string inxstr = string.Empty;
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.Tables.Count - cnt).ToString();
                    StringBuilder fulltablescript = new StringBuilder();
                    progressBar1.Value = cnt;
                    Application.DoEvents();
                    if (myTable.IsSystemObject == false)
                    {
                        try
                        {
                            lblcurrObject.Text = "TABLE";
                            Application.DoEvents();
                            if (ScriptValidator(counter, 2))
                            {
                                inxstr = GenerateIndexScripts(myTable).Replace("\nGO\n", string.Empty);

                                chkconstr = GenerateCheckConstraintsScripts(myTable).Replace("\nGO\n", string.Empty);
                                defcon = GenerateDefaultConstraintsScripts(myTable).Replace("\nGO\n", string.Empty);
                                fkcon = GenerateForeignKeyScripts(myTable);
                            }


                            //actaul scripting the table
                            StringCollection tableScripts = myTable.Script();
                            filename = Path.Combine(workingdir, "dbo." + myTable.Name + ".Table.sql");
                            String str = ConvertStringArrayToString(tableScripts).Replace("\nGO\n", string.Empty).Replace("SET ANSI_NULLS ON", string.Empty).Replace("SET ANSI_NULLS OFF", string.Empty);
                            fulltablescript.Append("IF  NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + myTable.Name + "]') AND type in (N'U'))\nBEGIN\n\n");
                            fulltablescript.Append(str.Replace("SET QUOTED_IDENTIFIER ON", string.Empty).Replace("SET QUOTED_IDENTIFIER OFF", string.Empty));
                            fulltablescript.Append(defcon);
                            fulltablescript.Append(chkconstr);

                            if (!chktoSingleFile.Checked)
                                fulltablescript.Append(fkcon.Replace("\nGO\n", string.Empty));
                            else
                                strfkcons.Append(fkcon);

                            fulltablescript.Append(inxstr);
                            fulltablescript.Append("\nEnd\nGO\n\n");
                            if (!chktoSingleFile.Checked)
                            {
                                File.WriteAllText(filename, fulltablescript.ToString());
                            }
                            else
                            {
                                strCumulativetext.Append(fulltablescript.ToString());
                                strCumulativetext.Append(Environment.NewLine);
                            }
                            if (ScriptValidator(counter, 3))
                            {
                                GenerateTriggersScripts(myTable);
                            }

                        }
                        catch (Exception ex)
                        {
                            createLog(myTable.Name, "Table (Main)", filename, ex);
                        }
                    }
                }
                if (chktoSingleFile.Checked)
                {
                    strCumulativetext.Append(System.Environment.NewLine).Append(strfkcons.ToString());
                    strCumulativetext.Append(Environment.NewLine);
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }


        private string GenerateIndexScripts(Table myTable)
        {
            StringBuilder str = new StringBuilder();
            lblchildcomp.Text = "Table: " + myTable.Name + " Index";
            Application.DoEvents();
            //string filename = String.Empty;
            //string workingdir = Path.Combine(rootdirectory, "Index");
            //if (!Directory.Exists(workingdir))
            //    Directory.CreateDirectory(workingdir);
            foreach (Index ndx in myTable.Indexes)
            {
                try
                {
                    //if (ndx.IndexKeyType.ToString() == "DriUniqueKey")
                    //{
                    //    filename = Path.Combine(workingdir, "dbo." + ndx.Name + ".ukey.sql");
                    //}
                    //else if (ndx.IndexKeyType.ToString() == "DriPrimaryKey")
                    //{
                    //    filename = Path.Combine(workingdir, "dbo." + ndx.Name + ".pkey.sql");
                    //}
                    //else
                    //{
                    //    filename = Path.Combine(workingdir, "dbo." + ndx.Name + ".Index.sql");
                    //}
                    StringCollection ndxScripts = ndx.Script();
                    str.Append(Environment.NewLine);
                    str.Append(ConvertStringArrayToString(ndxScripts));
                    str.Append(Environment.NewLine);
                    //File.WriteAllText(filename, str);
                }

                catch (Exception ex)
                {
                    createLog(ndx.Name, "Table Index", myTable.Name, ex);
                }
            }
            return str.ToString();
        }

        private string GenerateCheckConstraintsScripts(Table myTable)
        {
            StringBuilder str = new StringBuilder();
            lblchildcomp.Text = "Table: " + myTable.Name + " CheckConstraints";
            Application.DoEvents();
            //string filename = String.Empty;
            //string workingdir = Path.Combine(rootdirectory, "Constraints");
            //if (!Directory.Exists(workingdir))
            //    Directory.CreateDirectory(workingdir);
            foreach (Check chkcon in myTable.Checks)
            {
                try
                {
                    //filename = Path.Combine(workingdir, "dbo." + myTable.Name + "." + chkcon.Name + ".chkconst.sql");

                    StringCollection ndxScripts = chkcon.Script();
                    str.Append(Environment.NewLine);
                    str.Append(ConvertStringArrayToString(ndxScripts));
                    str.Append(Environment.NewLine);
                }

                catch (Exception ex)
                {
                    createLog(chkcon.Name, "Table CheckConstraints", myTable.Name, ex);
                }
            }
            return str.ToString();
        }

        private string GenerateDefaultConstraintsScripts(Table myTable)
        {
            StringBuilder str = new StringBuilder();
            lblchildcomp.Text = "Table: " + myTable.Name + " DefaultConstraints";
            Application.DoEvents();
            //string filename = String.Empty;
            //string workingdir = Path.Combine(rootdirectory, "Constraints");
            //if (!Directory.Exists(workingdir))
            //    Directory.CreateDirectory(workingdir);
            foreach (Column col in myTable.Columns)
            {
                try
                {
                    if (col.DefaultConstraint != null)
                    {
                        //filename = Path.Combine(workingdir, "dbo." + myTable.Name + "." + col.Name + ".defconst.sql");

                        StringCollection ndxScripts = col.DefaultConstraint.Script();
                        str.Append(Environment.NewLine);
                        str.Append(ConvertStringArrayToString(ndxScripts));
                        str.Append(Environment.NewLine);
                    }

                }
                catch (Exception ex)
                {
                    createLog(col.DefaultConstraint.Name, "Table DefaultConstraint", myTable.Name, ex);
                }
            }
            return str.ToString();

        }

        private string GenerateForeignKeyScripts(Table myTable)
        {
            StringBuilder str = new StringBuilder();
            lblchildcomp.Text = "Table: " + myTable.Name + " ForeignKey";
            Application.DoEvents();
            //string filename = String.Empty;
            //string workingdir = Path.Combine(rootdirectory, "Constraints");
            //if (!Directory.Exists(workingdir))
            //    Directory.CreateDirectory(workingdir);
            foreach (ForeignKey col in myTable.ForeignKeys)
            {
                try
                {
                    //filename = Path.Combine(workingdir, "dbo." + myTable.Name + "." + col.Name + ".fkey.sql");
                    
                    StringCollection ndxScripts = col.Script();
                    string drop = string.Format("IF NOT EXISTS(SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID('{0}') AND parent_object_id = OBJECT_ID('{1}') AND referenced_object_id = OBJECT_ID('{2}')){3}BEGIN{3}", col.Name, myTable.Name, col.ReferencedTable, System.Environment.NewLine);
                    str.Append(drop);
                    str.Append(ConvertStringArrayToString(ndxScripts).Replace("\nGO\n",""));
                    str.Append(string.Format("{0}END{0}GO{0}", System.Environment.NewLine));
                }
                catch (Exception ex)
                {
                    createLog(col.Name, "Table ForeignKey", myTable.Name, ex);
                }
            }
            return str.ToString();
        }

        private void GenerateTriggersScripts(Table myTable)
        {
            lblchildcomp.Text = "Table: " + myTable.Name + " Triggers";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Tables", "Triggers");
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            foreach (Trigger trg in myTable.Triggers)
            {
                try
                {
                    StringCollection ndxScripts = trg.Script();
                    filename = Path.Combine(workingdir, "dbo." + myTable.Name + "." + trg.Name + ".trig.sql");
                    String str = ConvertStringArrayToString(ndxScripts);
                    if (!chktoSingleFile.Checked)
                    {
                        File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                    }
                    else
                    {
                        strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                        strCumulativetext.Append(Environment.NewLine);
                    }

                }
                catch (Exception ex)
                {
                    createLog(trg.Name, "Table Triggers", filename, ex);
                }
            }
        }

        #endregion

        #region storedprocedure

        private void GenerateStoredprocedureScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "STORED PROCEDURES";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "StoredProcedures");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            progressBar1.Maximum = CurrDatabase.StoredProcedures.Count;
            try
            {

                foreach (StoredProcedure mysp in CurrDatabase.StoredProcedures)
                {
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.StoredProcedures.Count - cnt).ToString();

                    progressBar1.Value = cnt;
                    Application.DoEvents();
                    try
                    {
                        if (mysp.IsSystemObject == false)
                        {
                            lblchildcomp.Text = mysp.Name;
                            string drop = string.Format("IF OBJECTPROPERTY(object_id('dbo.{0}'), N'IsProcedure') = 1{1} DROP PROCEDURE [dbo].[{0}]{1}GO{1}", mysp.Name, System.Environment.NewLine);
                            StringCollection tableScripts = mysp.Script();
                            filename = Path.Combine(workingdir, "dbo." + mysp.Name + ".proc.sql");
                            String str = ConvertStringArrayToString(tableScripts);
                            if (!chktoSingleFile.Checked)
                            {
                                File.WriteAllText(filename, drop + str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", "").Replace("SET ANSI_NULLS OFF", string.Empty).Replace("SET QUOTED_IDENTIFIER OFF", string.Empty));
                            }
                            else
                            {
                                strCumulativetext.Append(drop + str.Replace("SET ANSI_NULLS OFF", "").Replace("SET QUOTED_IDENTIFIER OFF", "").Replace("SET ANSI_NULLS ON", string.Empty).Replace("SET QUOTED_IDENTIFIER ON", string.Empty));
                                strCumulativetext.Append(Environment.NewLine);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        createLog(mysp.Name, "Storedprocedure", filename, ex);
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        #endregion

        #region Function

        private void GenerateUserDefinedFunctionsScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "FUNCTION";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Functions");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

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
            progressBar1.Maximum = CurrDatabase.UserDefinedFunctions.Count;
            try
            {
                foreach (UserDefinedFunction myfns in CurrDatabase.UserDefinedFunctions)
                {
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.UserDefinedFunctions.Count - cnt).ToString();

                    progressBar1.Value = cnt;
                    Application.DoEvents();
                    try
                    {
                        if (myfns.IsSystemObject == false)
                        {
                            lblchildcomp.Text = myfns.Name;
                            string drop = string.Format("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')){1}DROP FUNCTION [dbo].[{0}]{1}GO{1}", myfns.Name, System.Environment.NewLine);
                            StringCollection tableScripts = myfns.Script();
                            filename = Path.Combine(workingdir, "dbo." + myfns.Name + ".function.sql");
                            String str = ConvertStringArrayToString(tableScripts);
                            if (!chktoSingleFile.Checked)
                            {
                                File.WriteAllText(filename, drop + str.Replace("SET ANSI_NULLS ON", string.Empty).Replace("SET QUOTED_IDENTIFIER ON", string.Empty).Replace("SET ANSI_NULLS OFF", string.Empty).Replace("SET QUOTED_IDENTIFIER OFF", string.Empty));
                            }
                            else
                            {
                                strCumulativetext.Append(drop + str.Replace("SET ANSI_NULLS OFF", string.Empty).Replace("SET QUOTED_IDENTIFIER OFF", string.Empty).Replace("SET ANSI_NULLS ON", string.Empty).Replace("SET QUOTED_IDENTIFIER ON", string.Empty));
                                strCumulativetext.Append(Environment.NewLine);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        createLog(myfns.Name, "Function", filename, ex);
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        #endregion

        #region Security

        private void GenerateApplicationRolesScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "APPLICATION ROLES";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Security\Roles\Application Roles");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;
            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            progressBar1.Maximum = CurrDatabase.ApplicationRoles.Count;
            try
            {
                foreach (ApplicationRole myApproles in CurrDatabase.ApplicationRoles)
                {
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.ApplicationRoles.Count - cnt).ToString();

                    progressBar1.Value = cnt;
                    Application.DoEvents();
                    try
                    {
                        lblchildcomp.Text = myApproles.Name;
                        StringCollection tableScripts = myApproles.Script();
                        filename = Path.Combine(workingdir, "dbo." + myApproles.Name + ".approle.sql");
                        String str = ConvertStringArrayToString(tableScripts);
                        if (!chktoSingleFile.Checked)
                        {
                            File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                        }
                        else
                        {
                            strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                            strCumulativetext.Append(Environment.NewLine);
                        }

                    }
                    catch (Exception ex)
                    {
                        createLog(myApproles.Name, "ApplicationRoles", filename, ex);
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }

        }

        private void GenerateDatabaseRolesScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "DATABASE ROLES";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Security\Roles\Database Roles");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;
            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            //ScriptingOptions scriptOptions = new ScriptingOptions();
            //scriptOptions.ScriptDrops = false;
            //scriptOptions.WithDependencies = true;
            //scriptOptions.Indexes = true;   // To include indexes
            //scriptOptions.DriAllConstraints = true;   // to include r
            //scriptOptions.Permissions = true;
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            progressBar1.Maximum = CurrDatabase.Roles.Count;
            try
            {
                foreach (DatabaseRole dbrole in CurrDatabase.Roles)
                {
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.Roles.Count - cnt).ToString();

                    progressBar1.Value = cnt;
                    Application.DoEvents();
                    if (dbrole.IsFixedRole || dbrole.Name == "public")
                    {
                        continue;
                    }
                    try
                    {
                        lblchildcomp.Text = dbrole.Name;
                        Application.DoEvents();
                        StringCollection tableScripts = dbrole.Script();
                        filename = Path.Combine(workingdir, "dbo." + dbrole.Name + ".role.sql");
                        String str = ConvertStringArrayToString(tableScripts);
                        if (!chktoSingleFile.Checked)
                        {

                            File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                        }
                        else
                        {
                            strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                            strCumulativetext.Append(Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {
                        createLog(dbrole.Name, "DatabaseRole", filename, ex);
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        private void GenerateSecuritySchemaScript(string CurrdbName)
        {
            //int cnt = 0;
            //lblcurrObject.Text = "SECURITY SCHEMA";
            //Application.DoEvents();
            //string filename = String.Empty;
            //string workingdir = Path.Combine(rootdirectory, @"Security\Schemas");

            //ServerConnection svrconn = new ServerConnection(serverName);
            //svrconn.LoginSecure = false;
            //svrconn.Login = userName;
            //svrconn.Password = password;
            //Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            //Scripter scripter = new Scripter(CurrServer);
            //Database CurrDatabase = CurrServer.Databases[CurrdbName];

            ////ScriptingOptions scriptOptions = new ScriptingOptions();
            ////scriptOptions.ScriptDrops = false;
            ////scriptOptions.WithDependencies = true;
            ////scriptOptions.Indexes = true;   // To include indexes
            ////scriptOptions.DriAllConstraints = true;   // to include r
            ////scriptOptions.Permissions = true;
            //if (!Directory.Exists(workingdir))
            //    Directory.CreateDirectory(workingdir);
            //progressBar1.Maximum = CurrDatabase.Schemas.Count;
            //try
            //{
            //    foreach (Schema mysecschema in CurrDatabase.Schemas)
            //    {
            //        cnt++;
            //        lbldone.Text = cnt.ToString();
            //        lbltbremaining.Text = (CurrDatabase.Schemas.Count - cnt).ToString();

            //        progressBar1.Value = cnt;
            //        Application.DoEvents();
            //        if (mysecschema.IsSystemObject == false)
            //        {
            //            try
            //            {
            //                lblchildcomp.Text = mysecschema.Name;
            //                Application.DoEvents();
            //                StringCollection tableScripts = mysecschema.Script();
            //                filename = Path.Combine(workingdir, "dbo." + mysecschema.Name + ".schema.sql");
            //                String str = ConvertStringArrayToString(tableScripts);
            //                if (!chktoSingleFile.Checked)
            //                {
            //                    File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
            //                }
            //                else
            //                {
            //                    strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
            //                    strCumulativetext.Append(Environment.NewLine);
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                createLog(mysecschema.Name, "SecuritySchema", filename, ex);
            //            }
            //        }
            //    }
            //}
            //finally
            //{
            //    if (svrconn != null)
            //        svrconn.Disconnect();
            //}
        }

        private void GenerateUsersScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "USERS";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Security\Schemas");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;
            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            progressBar1.Maximum = CurrDatabase.Users.Count;
            try
            {
                foreach (User user in CurrDatabase.Users)
                {
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.Users.Count - cnt).ToString();

                    progressBar1.Value = cnt;
                    Application.DoEvents();
                    //Skips system users
                    if (user.Name == "dbo" || user.Name == "guest" || user.Name == "INFORMATION_SCHEMA" || user.Name == "sys" || user.Name == @"BUILTIN\Administrators" || user.IsSystemObject)
                    {
                        continue;
                    }
                    if (user.IsSystemObject == false)
                    {
                        try
                        {
                            lblchildcomp.Text = user.Name;
                            Application.DoEvents();
                            StringCollection tableScripts = user.Script();
                            filename = Path.Combine(workingdir, "dbo." + user.Name + ".user.sql");
                            String str = ConvertStringArrayToString(tableScripts);
                            if (!chktoSingleFile.Checked)
                            {
                                File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                            }
                            else
                            {
                                strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                                strCumulativetext.Append(Environment.NewLine);
                            }
                        }
                        catch (Exception ex)
                        {
                            createLog(user.Name, "Users", filename, ex);
                        }
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }

        }

        #endregion

        #region views

        private void GenerateViewsScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "VIEWS";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, "Views");
            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);
            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            //MessageBox.Show(CurrDatabase.Views.Count.ToString ());
            progressBar1.Maximum = CurrDatabase.Views.Count + 1;
            try
            {
                foreach (Microsoft.SqlServer.Management.Smo.View myvws in CurrDatabase.Views)
                {
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.Views.Count - cnt).ToString();

                    progressBar1.Value = cnt;
                    Application.DoEvents();

                    try
                    {
                        if (myvws.IsSystemObject == false)
                        {
                            lblchildcomp.Text = myvws.Name;
                            Application.DoEvents();
                            GenerateViewIndexScripts(workingdir, myvws);
                            GenerateViewTriggersScripts(workingdir, myvws);
                            StringCollection tableScripts = myvws.Script();
                            filename = Path.Combine(workingdir, "dbo." + myvws.Name + ".views.sql");
                            String str = ConvertStringArrayToString(tableScripts);
                            string drop = string.Format("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = '{0}'){1} DROP vIEW [dbo].[{0}]{1}GO{1}", myvws.Name, System.Environment.NewLine);

                            if (!chktoSingleFile.Checked)
                            {
                                File.WriteAllText(filename, drop + str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", "").Replace("SET ANSI_NULLS OFF", "").Replace("SET QUOTED_IDENTIFIER OFF", ""));
                            }
                            else
                            {
                                strCumulativetext.Append(drop + str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", "").Replace("SET ANSI_NULLS OFF", "").Replace("SET QUOTED_IDENTIFIER OFF", ""));
                                strCumulativetext.Append(Environment.NewLine);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        createLog(myvws.Name, "Views", filename, ex);
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        private void GenerateViewIndexScripts(string workingdir, Microsoft.SqlServer.Management.Smo.View myView)
        {
            int cnt = 0;
            string filename = String.Empty;
            workingdir = Path.Combine(workingdir, "Index");
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            //progressBar1.Maximum = myView.Indexes.Count;
            foreach (Index ndx in myView.Indexes)
            {
                cnt++;
                lbldone.Text = cnt.ToString();
                lbltbremaining.Text = (myView.Indexes.Count - cnt).ToString();

                //progressBar1.Value = cnt;
                Application.DoEvents();
                try
                {
                    lblchildcomp.Text = ndx.Name;
                    Application.DoEvents();
                    StringCollection ndxScripts = ndx.Script();
                    String str = ConvertStringArrayToString(ndxScripts);
                    if (!chktoSingleFile.Checked)
                    {
                        File.WriteAllText(filename, str);
                    }
                    else
                    {
                        strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                        strCumulativetext.Append(Environment.NewLine);
                    }
                }

                catch (Exception ex)
                {
                    createLog(ndx.Name, "ViewIndex", filename, ex);
                }
            }
        }

        private void GenerateViewTriggersScripts(string workingdir, Microsoft.SqlServer.Management.Smo.View myView)
        {
            int cnt = 0;
            string filename = String.Empty;
            workingdir = Path.Combine(workingdir, "Triggers");
            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            //progressBar1.Maximum = myView.Triggers.Count;
            foreach (Trigger trg in myView.Triggers)
            {
                cnt++;
                lbldone.Text = cnt.ToString();
                lbltbremaining.Text = (myView.Triggers.Count - cnt).ToString();

                //progressBar1.Value = cnt;
                Application.DoEvents();
                try
                {
                    lblchildcomp.Text = trg.Name;
                    Application.DoEvents();
                    StringCollection ndxScripts = trg.Script();
                    filename = Path.Combine(workingdir, "dbo." + myView.Name + "." + trg.Name + ".trig.sql");
                    String str = ConvertStringArrayToString(ndxScripts);
                    if (!chktoSingleFile.Checked)
                    {
                        File.WriteAllText(filename, str);
                    }
                    else
                    {
                        strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                        strCumulativetext.Append(Environment.NewLine);
                    }


                }
                catch (Exception ex)
                {
                    createLog(trg.Name, "ViewTriggers", filename, ex);
                }
            }

        }

        #endregion

        #region DatabaseTriggers

        private void GenerateDatabaseTriggersScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "DATABASE TRIGGERS";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Database\Database Triggers");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            progressBar1.Maximum = CurrDatabase.Triggers.Count;
            try
            {
                foreach (DatabaseDdlTrigger mydbtrig in CurrDatabase.Triggers)
                {
                    try
                    {
                        cnt++;
                        lbldone.Text = cnt.ToString();
                        lbltbremaining.Text = (CurrDatabase.Triggers.Count - cnt).ToString();

                        progressBar1.Value = cnt;
                        Application.DoEvents();
                        if (mydbtrig.IsSystemObject == false)
                        {
                            lblchildcomp.Name = mydbtrig.Name;
                            Application.DoEvents();
                            StringCollection tableScripts = mydbtrig.Script();
                            filename = Path.Combine(workingdir, "dbo." + mydbtrig.Name + ".ddltrigger.sql");
                            String str = ConvertStringArrayToString(tableScripts);
                            if (!chktoSingleFile.Checked)
                            {
                                File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                            }
                            else
                            {
                                strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                                strCumulativetext.Append(Environment.NewLine);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        createLog(mydbtrig.Name, "DataBase Triggers", filename, ex);
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        #endregion

        #region Storage - FullTextCatalogs

        private void GenerateStorageFullTextCatalogsScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "STORAGE - FULLTEXTCATALOGS";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Storage\Full Text Catalog");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            progressBar1.Maximum = CurrDatabase.UserDefinedFunctions.Count;
            try
            {
                foreach (FullTextCatalog myftCat in CurrDatabase.FullTextCatalogs)
                {
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.UserDefinedFunctions.Count - cnt).ToString();

                    progressBar1.Value = cnt;
                    Application.DoEvents();
                    try
                    {

                        lblchildcomp.Text = myftCat.Name;
                        StringCollection tableScripts = myftCat.Script();
                        filename = Path.Combine(workingdir, "dbo." + myftCat.Name + ".fulltext.sql");
                        String str = ConvertStringArrayToString(tableScripts);
                        if (!chktoSingleFile.Checked)
                        {
                            File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                        }
                        else
                        {
                            strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                            strCumulativetext.Append(Environment.NewLine);
                        }

                    }
                    catch (Exception ex)
                    {
                        createLog(myftCat.Name, "Storage - FullTextCatalogs", filename, ex);
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        #endregion

        #region Synonyms

        private void GenerateSynonymsScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "SYNONYMS";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Synonyms");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            progressBar1.Maximum = CurrDatabase.UserDefinedFunctions.Count;
            try
            {
                foreach (Synonym mySynonym in CurrDatabase.Synonyms)
                {
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.UserDefinedFunctions.Count - cnt).ToString();

                    progressBar1.Value = cnt;
                    Application.DoEvents();
                    try
                    {

                        lblchildcomp.Text = mySynonym.Name;
                        StringCollection tableScripts = mySynonym.Script();
                        filename = Path.Combine(workingdir, "dbo." + mySynonym.Name + ".synonym.sql");
                        String str = ConvertStringArrayToString(tableScripts);
                        if (!chktoSingleFile.Checked)
                        {
                            File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                        }
                        else
                        {
                            strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                            strCumulativetext.Append(Environment.NewLine);
                        }

                    }
                    catch (Exception ex)
                    {
                        createLog(mySynonym.Name, "Synonym", filename, ex);
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        #endregion

        #region User-defined Types

        private void GenerateUserdefinedTypesScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "USER-DEFINED TYPES";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Types\User-defined Data Types");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            progressBar1.Maximum = CurrDatabase.UserDefinedFunctions.Count;
            try
            {
                foreach (UserDefinedDataType myuserdeftype in CurrDatabase.UserDefinedDataTypes)
                {
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.UserDefinedFunctions.Count - cnt).ToString();

                    progressBar1.Value = cnt;
                    Application.DoEvents();
                    try
                    {

                        lblchildcomp.Text = myuserdeftype.Name;
                        StringCollection tableScripts = myuserdeftype.Script();
                        filename = Path.Combine(workingdir, "dbo." + myuserdeftype.Name + ".uddt.sql");
                        String str = ConvertStringArrayToString(tableScripts);
                        if (!chktoSingleFile.Checked)
                        {
                            File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                        }
                        else
                        {
                            strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                            strCumulativetext.Append(Environment.NewLine);
                        }

                    }
                    catch (Exception ex)
                    {
                        createLog(myuserdeftype.Name, "User-defined Types", filename, ex);
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        #endregion

        #region  XML Schema Collections

        private void GenerateXMLSchemasScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "XML SCHEMA COLLECTIONS";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Types\XML Schema Collections");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);
            progressBar1.Maximum = CurrDatabase.UserDefinedFunctions.Count;
            try
            {
                foreach (XmlSchemaCollection xmlSchema in CurrDatabase.XmlSchemaCollections)
                {
                    cnt++;
                    lbldone.Text = cnt.ToString();
                    lbltbremaining.Text = (CurrDatabase.UserDefinedFunctions.Count - cnt).ToString();

                    progressBar1.Value = cnt;
                    Application.DoEvents();
                    try
                    {

                        lblchildcomp.Text = xmlSchema.Name;
                        StringCollection tableScripts = xmlSchema.Script();
                        filename = Path.Combine(workingdir, "dbo." + xmlSchema.Name + ".xmlschema.sql");
                        String str = ConvertStringArrayToString(tableScripts);
                        if (!chktoSingleFile.Checked)
                        {
                            File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                        }
                        else
                        {
                            strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                            strCumulativetext.Append(Environment.NewLine);
                        }

                    }
                    catch (Exception ex)
                    {
                        createLog(xmlSchema.Name, "XmlSchemaCollection", filename, ex);
                    }
                }
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        #endregion

        #region Database

        private void GenerateDatabaseScript(string CurrdbName)
        {
            int cnt = 0;
            lblcurrObject.Text = "DATABASE";
            Application.DoEvents();
            string filename = String.Empty;
            string workingdir = Path.Combine(rootdirectory, @"Database");

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Database CurrDatabase = CurrServer.Databases[CurrdbName];

            if (!Directory.Exists(workingdir))
                Directory.CreateDirectory(workingdir);

            try
            {
                lblchildcomp.Text = CurrDatabase.Name;
                StringCollection tableScripts = CurrDatabase.Script();
                filename = Path.Combine(workingdir, "dbo." + CurrDatabase.Name + ".database.sql");
                String str = ConvertStringArrayToString(tableScripts);
                if (!chktoSingleFile.Checked)
                {
                    File.WriteAllText(filename, str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                }
                else
                {
                    strCumulativetext.Append(str.Replace("SET ANSI_NULLS ON", "").Replace("SET QUOTED_IDENTIFIER ON", ""));
                    strCumulativetext.Append(Environment.NewLine);
                }

            }
            catch (Exception ex)
            {
                createLog(CurrDatabase.Name, "Database", filename, ex);
            }

            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        #endregion



        #endregion//ScriptEngineCore

        #region DataScript

        private void GenerateData(string DBName, ArrayList Tablelist)
        {

            timer1.Enabled = true;
            timer1.Start();
            timer1.Interval = 1000;
            starttime = DateTime.Now;
            timer1.Tick += new EventHandler(timer_Tick);
            string filename = String.Empty;
            int counter = 0;
            progressBar1.Visible = false;
            rootdirectory = Path.Combine(maindir, DBName, "Datapump");
            if (!Directory.Exists(rootdirectory))
                Directory.CreateDirectory(rootdirectory);
            foreach (string tabnam in Tablelist)
            {
                StringBuilder strfinished = new StringBuilder();

                String strpending = GetSummaryLabelText(0);

                Logfilename = Path.Combine(maindir, DBName, tabnam + "_Log.txt");
                File.WriteAllText(Logfilename, "Log File for " + tabnam + Environment.NewLine + "Start Time :" + DateTime.Now + Environment.NewLine);
                lblcurrentdb.Text = tabnam;
                lblTables.Text = strpending;
                Application.DoEvents();
                GenerateTableDataScript(DBName, tabnam, counter);
                createFinishLog();
            }
            timer1.Stop();
            timer1.Dispose();
            MessageBox.Show("All Data Scripted Successfully ", "RBT Database Scripting tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void GenerateTableDataScript(string CurrdbName, string currTableName, int counter)
        {
            string filename = String.Empty;

            ServerConnection svrconn = new ServerConnection(serverName);
            svrconn.LoginSecure = false;

            svrconn.Login = userName;
            svrconn.Password = password;
            Server CurrServer = new Microsoft.SqlServer.Management.Smo.Server(svrconn);

            Scripter scripter = new Scripter(CurrServer);
            Table currTable = CurrServer.Databases[CurrdbName].Tables[currTableName];
            if (!Directory.Exists(rootdirectory))
                Directory.CreateDirectory(rootdirectory);

            try
            {
                lblcurrObject.Text = "TABLE Datapump";
                Application.DoEvents();

                filename = Path.Combine(rootdirectory, "dbo." + currTable.Name + ".DataPump.sql");
                string datapump = GenerateDataScriptforaTable(currTable);
                File.WriteAllText(filename, datapump);

            }
            catch (Exception ex)
            {
                createLog(currTable.Name, "Table (Main)", filename, ex);
            }
            finally
            {
                if (svrconn != null)
                    svrconn.Disconnect();
            }
        }

        private string GenerateDataScriptforaTable(Table myTable)
        {
            int cntr = 0;
            ScriptingOptions scriptoptions = new ScriptingOptions();
            scriptoptions.ScriptData = true;
            scriptoptions.IncludeIfNotExists = true;
            scriptoptions.ScriptSchema = false;
            StringBuilder strTabData = new StringBuilder();
            List<string> myList = myTable.EnumScript(scriptoptions).ToList();
            int total = myList.Count;
            foreach (string tabData in myTable.EnumScript(scriptoptions))
            {
                lbldone.Text = cntr.ToString();
                lbltbremaining.Text = (total - cntr).ToString();
                Application.DoEvents();
                strTabData.Append(tabData).Append("\nGO\n");
                cntr++;
            }

            return strTabData.ToString();
        }

        #endregion

        #endregion//ScriptingEngine

        #region common

        private string ConvertStringArrayToString(StringCollection strcoll)
        {
            // Concatenate all the elements into a StringBuilder.
            StringBuilder builder = new StringBuilder();
            foreach (string value in strcoll)
            {
                builder.Append(value);
                builder.Append('\n');
            }
            return builder.ToString() + "\nGO\n";
        }

        private string GetSummaryLabelText(int selector)
        {
            string strpending = string.Empty;
            switch (selector)
            {
                case 0:
                    strpending = "  Tables \n \n  Views\n \n  Stored Procedures\n \n  Function\n \n  ApplicationRoles\n \n  DatabaseRoles\n \n  SecuritySchema\n \n  Users\n \n  StorageFullTextCatalogs\n \n  DatabaseTriggers\n \n  Synonyms\n \n  User-Defined Datatypes\n \n  XML Schemas\n \n  Database";
                    break;
                case 1:
                    strpending = " \u2713 Tables\n \n     Views\n \n     Stored Procedures\n \n     Function\n \n     ApplicationRoles\n \n     DatabaseRoles\n \n     SecuritySchema\n \n     Users\n \n     StorageFullTextCatalogs\n \n     DatabaseTriggers\n \n     Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 2:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n     Stored Procedures\n \n     Function\n \n     ApplicationRoles\n \n     DatabaseRoles\n \n     SecuritySchema\n \n     Users\n \n     StorageFullTextCatalogs\n \n     DatabaseTriggers\n \n     Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 3:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n     Function\n \n     ApplicationRoles\n \n     DatabaseRoles\n \n     SecuritySchema\n \n     Users\n \n     StorageFullTextCatalogs\n \n     DatabaseTriggers\n \n     Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 4:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n     ApplicationRoles\n \n     DatabaseRoles\n \n     SecuritySchema\n \n     Users\n \n     StorageFullTextCatalogs\n \n     DatabaseTriggers\n \n     Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 5:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n\u2713 ApplicationRoles\n \n     DatabaseRoles\n \n     SecuritySchema\n \n     Users\n \n     StorageFullTextCatalogs\n \n     DatabaseTriggers\n \n     Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 6:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n\u2713 ApplicationRoles\n \n\u2713 DatabaseRoles\n \n     SecuritySchema\n \n     Users\n \n     StorageFullTextCatalogs\n \n     DatabaseTriggers\n \n     Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 7:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n\u2713 ApplicationRoles\n \n\u2713 DatabaseRoles\n \n\u2713 SecuritySchema\n \n     Users\n \n     StorageFullTextCatalogs\n \n     DatabaseTriggers\n \n     Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 8:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n\u2713 ApplicationRoles\n \n\u2713 DatabaseRoles\n \n\u2713 SecuritySchema\n \n\u2713 Users\n \n     StorageFullTextCatalogs\n \n     DatabaseTriggers\n \n     Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 9:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n\u2713 ApplicationRoles\n \n\u2713 DatabaseRoles\n \n\u2713 SecuritySchema\n \n\u2713 Users\n \n\u2713 StorageFullTextCatalogs\n \n     DatabaseTriggers\n \n     Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 10:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n\u2713 ApplicationRoles\n \n\u2713 DatabaseRoles\n \n\u2713 SecuritySchema\n \n\u2713 Users\n \n\u2713 StorageFullTextCatalogs\n \n\u2713 DatabaseTriggers\n \n     Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 11:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n\u2713 ApplicationRoles\n \n\u2713 DatabaseRoles\n \n\u2713 SecuritySchema\n \n\u2713 Users\n \n\u2713 StorageFullTextCatalogs\n \n\u2713 DatabaseTriggers\n \n\u2713 Synonyms\n \n     User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 12:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n\u2713 ApplicationRoles\n \n\u2713 DatabaseRoles\n \n\u2713 SecuritySchema\n \n\u2713 Users\n \n\u2713 StorageFullTextCatalogs\n \n\u2713 DatabaseTriggers\n \n\u2713 Synonyms\n \n\u2713 User-Defined Datatypes\n \n     XML Schemas\n \n     Database";
                    break;
                case 13:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n\u2713 ApplicationRoles\n \n\u2713 DatabaseRoles\n \n\u2713 SecuritySchema\n \n\u2713 Users\n \n\u2713 StorageFullTextCatalogs\n \n\u2713 DatabaseTriggers\n \n\u2713 Synonyms\n \n\u2713 User-Defined Datatypes\n \n\u2713 XML Schemas\n \n     Database";
                    break;
                case 14:
                    strpending = " \u2713 Tables\n \n\u2713 Views\n \n\u2713 Stored Procedures\n \n\u2713 Function\n \n\u2713 ApplicationRoles\n \n\u2713 DatabaseRoles\n \n\u2713 SecuritySchema\n \n\u2713 Users\n \n\u2713 StorageFullTextCatalogs\n \n\u2713 DatabaseTriggers\n \n\u2713 Synonyms\n \n\u2713 User-Defined Datatypes\n \n\u2713 XML Schemas\n \n\u2713 Database";
                    break;
            }
            return strpending;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lblelapsedtime.Text = String.Format("{0:hh\\:mm\\:ss}", (DateTime.Now - starttime)).ToString();
            Application.DoEvents();
        }

        private void CreateSingleFile(string dbname, string Scripttype)
        {
            String cumulativeScriptfilename = Path.Combine(maindir, dbname, dbname + "_" + Scripttype + "_CumulativeScripts.sql");

            File.WriteAllText(cumulativeScriptfilename, strCumulativetext.ToString());
            strCumulativetext.Clear();
        }

        #endregion

        #region LOG

        private void createLog(string filename, string filetype, string path, Exception ex)
        {
            StringBuilder logstr = new StringBuilder();
            logstr.Append("OBJECT NAME : " + filename);
            logstr.Append(Environment.NewLine);
            logstr.Append("OBJECT TYPE : " + filetype);
            logstr.Append(Environment.NewLine);
            logstr.Append("FILE PATH : " + path);
            logstr.Append(Environment.NewLine);
            logstr.Append("EXCEPTION : " + ex.Message + "INNER EXCEPTION : " + ex.InnerException);
            logstr.Append(Environment.NewLine);
            logstr.Append("TIME : " + DateTime.Now);
            logstr.Append(Environment.NewLine);
            logstr.Append("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            logstr.Append(Environment.NewLine);
            File.AppendAllText(Logfilename, logstr.ToString());
        }

        private void createFinishLog()
        {
            StringBuilder logstr = new StringBuilder();
            logstr.Append("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            logstr.Append(Environment.NewLine);
            logstr.Append("All Objects Scripted Successfully");
            logstr.Append(Environment.NewLine);
            logstr.Append("TIME : " + DateTime.Now);
            logstr.Append(Environment.NewLine);
            logstr.Append("Bye regards and cheers");
            logstr.Append(Environment.NewLine);
            File.AppendAllText(Logfilename, logstr.ToString());
            try
            {
                Process.Start(Logfilename);

            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region formevents

        private void Form1_Load(object sender, EventArgs e)
        {
            txtServername.Focus();
            dgDatabasesForScriptObj.Top = (groupBox2.Height / 2) - (dgDatabasesForScriptObj.Height / 2);
            dgDatabasesForScriptObj.Left = (groupBox2.Width / 2) - (dgDatabasesForScriptObj.Width / 2);

            SetPOS(groupBox1);
            SetPOS(groupBox2);
            SetPOS(grpsummary);
            SetPOS(grpSettings);
            SetPOS(grpSelector);
            int y = this.Width - 150;
            int x = this.Left + 10;
            //pictureBox1.Location = new System.Drawing.Point(y, x);
            Application.DoEvents();
        }

        private void SetPOS(Control ctrl)
        {
            ctrl.Top = (this.Height / 2) - (ctrl.Height / 2);
            ctrl.Left = (this.Width / 2) - (ctrl.Width / 2);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            dgDatabasesForScriptObj.Top = (groupBox2.Height / 2) - (dgDatabasesForScriptObj.Height / 2);
            dgDatabasesForScriptObj.Left = (groupBox2.Width / 2) - (dgDatabasesForScriptObj.Width / 2);
            SetPOS(groupBox1);
            SetPOS(groupBox2);
            SetPOS(grpsummary);
            SetPOS(grpSettings);
            SetPOS(grpSelector);
            label1.Left = ((this.Left + this.Right) / 2) - (label1.Width / 2);
            int y = this.Width - 150;
            int x = this.Left + 10;
            //pictureBox1.Location = new System.Drawing.Point(y, x);
            Application.DoEvents();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            dgDatabasesForScriptObj.Top = (groupBox2.Height / 2) - (dgDatabasesForScriptObj.Height / 2);
            dgDatabasesForScriptObj.Left = (groupBox2.Width / 2) - (dgDatabasesForScriptObj.Width / 2);
            dgTableNames.Top = (groupBox2.Height / 2) - (dgTableNames.Height / 2);
            dgTableNames.Left = (groupBox2.Width / 2) - (dgTableNames.Width / 2);
            SetPOS(groupBox1);
            SetPOS(groupBox2);
            SetPOS(grpsummary);
            SetPOS(grpSettings);
            SetPOS(grpSelector);
            label1.Left = ((this.Left + this.Right) / 2) - (label1.Width / 2);
            int y = this.Width - 150;
            int x = this.Left + 10;
            //pictureBox1.Location = new System.Drawing.Point(y, x);
            Application.DoEvents();
        }

        private void dgDatabasesForScriptObj_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (blnScriptData)
            {
                foreach (DataGridViewRow dgvr in dgDatabasesForScriptObj.Rows)
                {
                    ((DataGridViewCheckBoxCell)dgvr.Cells[e.ColumnIndex]).Value = false;
                }

                ((DataGridViewCheckBoxCell)dgDatabasesForScriptObj.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value = true;
            }
        }

        #endregion

        #region buttonevents

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        //first button click
        private void Next_Click(object sender, EventArgs e)
        {
            if (txtServername.Text != string.Empty && txtUsrname.Text != string.Empty && txtpass.Text != string.Empty)
            {
                if (ChkDataBase())
                {
                    serverName = txtServername.Text;
                    userName = txtUsrname.Text;
                    password = txtpass.Text;
                    groupBox1.Visible = false;
                    grpSelector.Visible = true;
                    btnScriptObj.Visible = true;
                    Next.Visible = false;
                }
                else
                {
                    MessageBox.Show("Please check the user name and password or check your network connectivity", "RBT Database Scripting tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ChkDataBase()
        {
            bool result = true;
            string sqlconnstr = @"Server=" + txtServername.Text + ";Database=master;" + "User id=" + txtUsrname.Text + ";password=" + txtpass.Text;
            SqlConnection sqlConx = new SqlConnection(sqlconnstr);
            try
            {
                sqlConx.Open();
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (sqlConx != null)
                    sqlConx.Dispose();
            }
            return result;
        }
        //2nd button click
        private void btnnext2_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < dgDatabasesForScriptObj.Rows.Count; i++)
            {
                if (dgDatabasesForScriptObj.Rows[i].Cells[0].Value != null && Convert.ToBoolean(dgDatabasesForScriptObj.Rows[i].Cells[0].Value) == true)
                {
                    selvalues.Add((string)dgDatabasesForScriptObj.Rows[i].Cells[1].Value.ToString());
                }
            }
            int j = 0;
            foreach (string dbnam in selvalues)
            {
                dgChosenObjects.Rows.Add(1);
                dgChosenObjects.Rows[j].Cells["DBName"].Value = dbnam;
                for (int z = 1; z < dgChosenObjects.ColumnCount; z++)
                {
                    dgChosenObjects.Rows[j].Cells[z].Value = true;
                }
                j++;
            }
            groupBox2.Visible = false;

            if (blnScriptData)
            {

                grpSettings.Visible = true;
                GetTablesinaDatabase(selvalues[0].ToString());
                dgTableNames.Visible = true;
                dgChosenObjects.Visible = false;
                chktoSingleFile.Visible = false;
                chkScriptDB.Visible = false;

            }
            else
            {
                dgTableNames.Visible = false;
                grpSettings.Visible = true;
            }

        }

        //3 rd button click
        private void btnsettings_Click(object sender, EventArgs e)
        {
            if (txtoutputdir.Text.Trim() != string.Empty)
                maindir = txtoutputdir.Text.Trim();
            else
                maindir = @"C:\DBScripter";
            grpSettings.Visible = false;
            grpsummary.Visible = true;

            lblcurrentdb.Text = "";
            lbldone.Text = "";
            lbltbremaining.Text = "";
            lblchildcomp.Text = "";
            Application.DoEvents();
            if (blnScriptData)
            {
                ArrayList tabValues = new ArrayList();
                for (int i = 0; i < dgTableNames.Rows.Count; i++)
                {
                    if (dgTableNames.Rows[i].Cells[0].Value != null && Convert.ToBoolean(dgTableNames.Rows[i].Cells[0].Value) == true)
                    {
                        tabValues.Add((string)dgTableNames.Rows[i].Cells[1].Value.ToString());
                    }
                }
                GenerateData(selvalues[0].ToString(), tabValues);

            }
            else
            {
                GenerateScripts(selvalues);
            }

        }

        private void btnScriptData_Click(object sender, EventArgs e)
        {
            blnScriptData = true;
            groupBox2.Visible = true;
            btnnext2.Visible = true;
            dgDatabasesForScriptObj.Visible = true;
            GetDatabaselist();
            grpSelector.Visible = false;
            btnScriptObj.Visible = false;

        }

        private void HideControl(Control ctrl, Control ctrl2, bool btnHide)
        {
            ctrl.Visible = false;
            ctrl2.Visible = false;

        }

        private void btnScriptObj_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            btnnext2.Visible = true;
            dgDatabasesForScriptObj.Visible = true;
            GetDatabaselist();
            grpSelector.Visible = false;
            btnScriptObj.Visible = false;
        }

    }


        #endregion
}
