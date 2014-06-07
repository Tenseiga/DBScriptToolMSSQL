namespace DBScripter
{
    partial class DbScpritForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbScpritForm));
            this.label1 = new System.Windows.Forms.Label();
            this.Next = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServername = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgDatabasesForScriptObj = new System.Windows.Forms.DataGridView();
            this.btnnext2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtUsrname = new System.Windows.Forms.TextBox();
            this.txtpass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grpsummary = new System.Windows.Forms.GroupBox();
            this.lblelapsedtime = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblTables = new System.Windows.Forms.Label();
            this.lblcurrObject = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Statusbar = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbltbremaining = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbldone = new System.Windows.Forms.Label();
            this.lblchildcomp = new System.Windows.Forms.Label();
            this.lblcurrentdb = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.dgTableNames = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chkScriptDB = new System.Windows.Forms.CheckBox();
            this.chktoSingleFile = new System.Windows.Forms.CheckBox();
            this.txtoutputdir = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lblScriptType = new System.Windows.Forms.Label();
            this.btnsettings = new System.Windows.Forms.Button();
            this.dgChosenObjects = new System.Windows.Forms.DataGridView();
            this.DBName = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Table = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TableTriggers = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Constraints = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Views = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SPs = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Functions = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Roles = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SecuritySchema = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Users = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Storage = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DBTriggers = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Synonyms = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UserDefinedandXml = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.grpSelector = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnScriptData = new System.Windows.Forms.Button();
            this.btnScriptObj = new System.Windows.Forms.Button();
            this.dbselector = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDatabasesForScriptObj)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpsummary.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTableNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgChosenObjects)).BeginInit();
            this.grpSelector.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("Calibri", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Brown;
            this.label1.Location = new System.Drawing.Point(500, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(378, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database Scripting Tool";
            // 
            // Next
            // 
            this.Next.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Next.Location = new System.Drawing.Point(328, 205);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(75, 23);
            this.Next.TabIndex = 4;
            this.Next.Text = "Proceed";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Brown;
            this.label2.Location = new System.Drawing.Point(119, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server Name";
            // 
            // txtServername
            // 
            this.txtServername.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServername.ForeColor = System.Drawing.Color.Black;
            this.txtServername.Location = new System.Drawing.Point(215, 62);
            this.txtServername.Name = "txtServername";
            this.txtServername.Size = new System.Drawing.Size(188, 22);
            this.txtServername.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.dgDatabasesForScriptObj);
            this.groupBox2.Controls.Add(this.btnnext2);
            this.groupBox2.Location = new System.Drawing.Point(40, 250);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(839, 474);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Visible = false;
            // 
            // dgDatabasesForScriptObj
            // 
            this.dgDatabasesForScriptObj.AllowUserToAddRows = false;
            this.dgDatabasesForScriptObj.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgDatabasesForScriptObj.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgDatabasesForScriptObj.BackgroundColor = System.Drawing.Color.LightBlue;
            this.dgDatabasesForScriptObj.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDatabasesForScriptObj.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDatabasesForScriptObj.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDatabasesForScriptObj.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDatabasesForScriptObj.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dbselector});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgDatabasesForScriptObj.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgDatabasesForScriptObj.Location = new System.Drawing.Point(125, 25);
            this.dgDatabasesForScriptObj.Name = "dgDatabasesForScriptObj";
            this.dgDatabasesForScriptObj.RowHeadersVisible = false;
            this.dgDatabasesForScriptObj.Size = new System.Drawing.Size(491, 404);
            this.dgDatabasesForScriptObj.TabIndex = 1;
            this.dgDatabasesForScriptObj.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDatabasesForScriptObj_CellContentClick);
            // 
            // btnnext2
            // 
            this.btnnext2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnnext2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnnext2.Location = new System.Drawing.Point(546, 446);
            this.btnnext2.Name = "btnnext2";
            this.btnnext2.Size = new System.Drawing.Size(75, 23);
            this.btnnext2.TabIndex = 2;
            this.btnnext2.Text = "Next";
            this.btnnext2.UseVisualStyleBackColor = true;
            this.btnnext2.Click += new System.EventHandler(this.btnnext2_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.txtServername);
            this.groupBox1.Controls.Add(this.txtUsrname);
            this.groupBox1.Controls.Add(this.txtpass);
            this.groupBox1.Controls.Add(this.Next);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(387, 174);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(531, 270);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // txtUsrname
            // 
            this.txtUsrname.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsrname.ForeColor = System.Drawing.Color.Black;
            this.txtUsrname.Location = new System.Drawing.Point(215, 101);
            this.txtUsrname.Name = "txtUsrname";
            this.txtUsrname.Size = new System.Drawing.Size(188, 23);
            this.txtUsrname.TabIndex = 2;
            // 
            // txtpass
            // 
            this.txtpass.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpass.ForeColor = System.Drawing.Color.Black;
            this.txtpass.Location = new System.Drawing.Point(215, 148);
            this.txtpass.Name = "txtpass";
            this.txtpass.PasswordChar = '.';
            this.txtpass.Size = new System.Drawing.Size(188, 23);
            this.txtpass.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Brown;
            this.label4.Location = new System.Drawing.Point(119, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Brown;
            this.label3.Location = new System.Drawing.Point(119, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "User Name";
            // 
            // grpsummary
            // 
            this.grpsummary.AutoSize = true;
            this.grpsummary.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpsummary.BackColor = System.Drawing.Color.Transparent;
            this.grpsummary.Controls.Add(this.lblelapsedtime);
            this.grpsummary.Controls.Add(this.label10);
            this.grpsummary.Controls.Add(this.lblTables);
            this.grpsummary.Controls.Add(this.lblcurrObject);
            this.grpsummary.Controls.Add(this.label6);
            this.grpsummary.Controls.Add(this.Statusbar);
            this.grpsummary.Controls.Add(this.label11);
            this.grpsummary.Controls.Add(this.label9);
            this.grpsummary.Controls.Add(this.lbltbremaining);
            this.grpsummary.Controls.Add(this.label8);
            this.grpsummary.Controls.Add(this.label7);
            this.grpsummary.Controls.Add(this.lbldone);
            this.grpsummary.Controls.Add(this.lblchildcomp);
            this.grpsummary.Controls.Add(this.lblcurrentdb);
            this.grpsummary.Controls.Add(this.progressBar1);
            this.grpsummary.Controls.Add(this.label5);
            this.grpsummary.Location = new System.Drawing.Point(181, 95);
            this.grpsummary.Name = "grpsummary";
            this.grpsummary.Size = new System.Drawing.Size(804, 441);
            this.grpsummary.TabIndex = 4;
            this.grpsummary.TabStop = false;
            this.grpsummary.Visible = false;
            // 
            // lblelapsedtime
            // 
            this.lblelapsedtime.AutoSize = true;
            this.lblelapsedtime.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblelapsedtime.Location = new System.Drawing.Point(277, 284);
            this.lblelapsedtime.Name = "lblelapsedtime";
            this.lblelapsedtime.Size = new System.Drawing.Size(0, 15);
            this.lblelapsedtime.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Maroon;
            this.label10.Location = new System.Drawing.Point(541, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(257, 19);
            this.label10.TabIndex = 15;
            this.label10.Text = "List of Objects Scripted Successsfully";
            // 
            // lblTables
            // 
            this.lblTables.AutoSize = true;
            this.lblTables.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTables.ForeColor = System.Drawing.Color.Green;
            this.lblTables.Location = new System.Drawing.Point(630, 81);
            this.lblTables.Name = "lblTables";
            this.lblTables.Size = new System.Drawing.Size(0, 15);
            this.lblTables.TabIndex = 14;
            // 
            // lblcurrObject
            // 
            this.lblcurrObject.AutoSize = true;
            this.lblcurrObject.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcurrObject.ForeColor = System.Drawing.Color.DarkRed;
            this.lblcurrObject.Location = new System.Drawing.Point(276, 46);
            this.lblcurrObject.Name = "lblcurrObject";
            this.lblcurrObject.Size = new System.Drawing.Size(0, 23);
            this.lblcurrObject.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Maroon;
            this.label6.Location = new System.Drawing.Point(45, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(174, 19);
            this.label6.TabIndex = 12;
            this.label6.Text = "Current Object Scripting";
            // 
            // Statusbar
            // 
            this.Statusbar.AutoSize = true;
            this.Statusbar.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Statusbar.ForeColor = System.Drawing.Color.Maroon;
            this.Statusbar.Location = new System.Drawing.Point(25, 403);
            this.Statusbar.Name = "Statusbar";
            this.Statusbar.Size = new System.Drawing.Size(123, 19);
            this.Statusbar.TabIndex = 11;
            this.Statusbar.Text = "Current Progress";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.Maroon;
            this.label11.Location = new System.Drawing.Point(83, 200);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 15);
            this.label11.TabIndex = 7;
            this.label11.Text = "Objects Remaining";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.Maroon;
            this.label9.Location = new System.Drawing.Point(83, 167);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 15);
            this.label9.TabIndex = 5;
            this.label9.Text = "Objects Done";
            // 
            // lbltbremaining
            // 
            this.lbltbremaining.AutoSize = true;
            this.lbltbremaining.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbltbremaining.Location = new System.Drawing.Point(277, 200);
            this.lbltbremaining.Name = "lbltbremaining";
            this.lbltbremaining.Size = new System.Drawing.Size(0, 15);
            this.lbltbremaining.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.Maroon;
            this.label8.Location = new System.Drawing.Point(83, 284);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 15);
            this.label8.TabIndex = 3;
            this.label8.Text = "Elapsed Time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.Maroon;
            this.label7.Location = new System.Drawing.Point(83, 240);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(148, 15);
            this.label7.TabIndex = 3;
            this.label7.Text = "Current Child Component";
            // 
            // lbldone
            // 
            this.lbldone.AutoSize = true;
            this.lbldone.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbldone.Location = new System.Drawing.Point(277, 167);
            this.lbldone.Name = "lbldone";
            this.lbldone.Size = new System.Drawing.Size(0, 15);
            this.lbldone.TabIndex = 6;
            // 
            // lblchildcomp
            // 
            this.lblchildcomp.AutoSize = true;
            this.lblchildcomp.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblchildcomp.Location = new System.Drawing.Point(277, 240);
            this.lblchildcomp.Name = "lblchildcomp";
            this.lblchildcomp.Size = new System.Drawing.Size(0, 15);
            this.lblchildcomp.TabIndex = 4;
            // 
            // lblcurrentdb
            // 
            this.lblcurrentdb.AutoSize = true;
            this.lblcurrentdb.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblcurrentdb.Location = new System.Drawing.Point(277, 131);
            this.lblcurrentdb.Name = "lblcurrentdb";
            this.lblcurrentdb.Size = new System.Drawing.Size(0, 15);
            this.lblcurrentdb.TabIndex = 2;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.progressBar1.Location = new System.Drawing.Point(156, 399);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(430, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Maroon;
            this.label5.Location = new System.Drawing.Point(83, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Current Database";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1212, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // grpSettings
            // 
            this.grpSettings.BackColor = System.Drawing.Color.Transparent;
            this.grpSettings.Controls.Add(this.dgTableNames);
            this.grpSettings.Controls.Add(this.chkScriptDB);
            this.grpSettings.Controls.Add(this.chktoSingleFile);
            this.grpSettings.Controls.Add(this.txtoutputdir);
            this.grpSettings.Controls.Add(this.label13);
            this.grpSettings.Controls.Add(this.lblScriptType);
            this.grpSettings.Controls.Add(this.btnsettings);
            this.grpSettings.Controls.Add(this.dgChosenObjects);
            this.grpSettings.Location = new System.Drawing.Point(40, 62);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(920, 468);
            this.grpSettings.TabIndex = 13;
            this.grpSettings.TabStop = false;
            this.grpSettings.Visible = false;
            // 
            // dgTableNames
            // 
            this.dgTableNames.AllowUserToAddRows = false;
            this.dgTableNames.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgTableNames.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgTableNames.BackgroundColor = System.Drawing.Color.LightBlue;
            this.dgTableNames.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgTableNames.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTableNames.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgTableNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTableNames.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgTableNames.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgTableNames.Location = new System.Drawing.Point(170, 85);
            this.dgTableNames.Name = "dgTableNames";
            this.dgTableNames.RowHeadersVisible = false;
            this.dgTableNames.Size = new System.Drawing.Size(595, 298);
            this.dgTableNames.TabIndex = 7;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "TableName";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 68;
            // 
            // chkScriptDB
            // 
            this.chkScriptDB.AutoSize = true;
            this.chkScriptDB.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkScriptDB.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkScriptDB.ForeColor = System.Drawing.Color.Maroon;
            this.chkScriptDB.Location = new System.Drawing.Point(484, 429);
            this.chkScriptDB.Name = "chkScriptDB";
            this.chkScriptDB.Size = new System.Drawing.Size(111, 19);
            this.chkScriptDB.TabIndex = 6;
            this.chkScriptDB.Text = "Script Database";
            this.chkScriptDB.UseVisualStyleBackColor = true;
            // 
            // chktoSingleFile
            // 
            this.chktoSingleFile.AutoSize = true;
            this.chktoSingleFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chktoSingleFile.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chktoSingleFile.ForeColor = System.Drawing.Color.Maroon;
            this.chktoSingleFile.Location = new System.Drawing.Point(484, 401);
            this.chktoSingleFile.Name = "chktoSingleFile";
            this.chktoSingleFile.Size = new System.Drawing.Size(139, 19);
            this.chktoSingleFile.TabIndex = 5;
            this.chktoSingleFile.Text = "Script to a single File";
            this.chktoSingleFile.UseVisualStyleBackColor = true;
            // 
            // txtoutputdir
            // 
            this.txtoutputdir.Location = new System.Drawing.Point(202, 400);
            this.txtoutputdir.Name = "txtoutputdir";
            this.txtoutputdir.Size = new System.Drawing.Size(221, 20);
            this.txtoutputdir.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.Color.Maroon;
            this.label13.Location = new System.Drawing.Point(48, 401);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 15);
            this.label13.TabIndex = 3;
            this.label13.Text = "Output Directory";
            // 
            // lblScriptType
            // 
            this.lblScriptType.AutoSize = true;
            this.lblScriptType.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScriptType.ForeColor = System.Drawing.Color.Maroon;
            this.lblScriptType.Location = new System.Drawing.Point(45, 49);
            this.lblScriptType.Name = "lblScriptType";
            this.lblScriptType.Size = new System.Drawing.Size(144, 18);
            this.lblScriptType.TabIndex = 1;
            this.lblScriptType.Text = "Objects to be Scripted";
            // 
            // btnsettings
            // 
            this.btnsettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnsettings.Location = new System.Drawing.Point(716, 427);
            this.btnsettings.Name = "btnsettings";
            this.btnsettings.Size = new System.Drawing.Size(75, 23);
            this.btnsettings.TabIndex = 0;
            this.btnsettings.Text = "Next";
            this.btnsettings.UseVisualStyleBackColor = true;
            this.btnsettings.Click += new System.EventHandler(this.btnsettings_Click);
            // 
            // dgChosenObjects
            // 
            this.dgChosenObjects.AllowUserToAddRows = false;
            this.dgChosenObjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgChosenObjects.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgChosenObjects.BackgroundColor = System.Drawing.Color.LightBlue;
            this.dgChosenObjects.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgChosenObjects.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgChosenObjects.ColumnHeadersHeight = 32;
            this.dgChosenObjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DBName,
            this.Table,
            this.TableTriggers,
            this.Constraints,
            this.Views,
            this.SPs,
            this.Functions,
            this.Roles,
            this.SecuritySchema,
            this.Users,
            this.Storage,
            this.DBTriggers,
            this.Synonyms,
            this.UserDefinedandXml});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.NullValue = "True";
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgChosenObjects.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgChosenObjects.Location = new System.Drawing.Point(23, 78);
            this.dgChosenObjects.Name = "dgChosenObjects";
            this.dgChosenObjects.RowHeadersVisible = false;
            dataGridViewCellStyle7.NullValue = "True";
            this.dgChosenObjects.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgChosenObjects.Size = new System.Drawing.Size(857, 288);
            this.dgChosenObjects.TabIndex = 2;
            // 
            // DBName
            // 
            this.DBName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DBName.HeaderText = "DBName";
            this.DBName.Name = "DBName";
            this.DBName.Width = 56;
            // 
            // Table
            // 
            this.Table.HeaderText = "Table";
            this.Table.Name = "Table";
            this.Table.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Table.Width = 40;
            // 
            // TableTriggers
            // 
            this.TableTriggers.HeaderText = "TableTriggers";
            this.TableTriggers.Name = "TableTriggers";
            this.TableTriggers.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TableTriggers.Width = 78;
            // 
            // Constraints
            // 
            this.Constraints.HeaderText = "Constraints";
            this.Constraints.Name = "Constraints";
            this.Constraints.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Constraints.Width = 65;
            // 
            // Views
            // 
            this.Views.HeaderText = "Views";
            this.Views.Name = "Views";
            this.Views.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Views.Width = 41;
            // 
            // SPs
            // 
            this.SPs.HeaderText = "Sps";
            this.SPs.Name = "SPs";
            this.SPs.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SPs.Width = 31;
            // 
            // Functions
            // 
            this.Functions.HeaderText = "Functions";
            this.Functions.Name = "Functions";
            this.Functions.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Functions.Width = 59;
            // 
            // Roles
            // 
            this.Roles.HeaderText = "Roles";
            this.Roles.Name = "Roles";
            this.Roles.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Roles.Width = 40;
            // 
            // SecuritySchema
            // 
            this.SecuritySchema.HeaderText = "Security Schema";
            this.SecuritySchema.Name = "SecuritySchema";
            this.SecuritySchema.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SecuritySchema.Width = 84;
            // 
            // Users
            // 
            this.Users.HeaderText = "Users";
            this.Users.Name = "Users";
            this.Users.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Users.Width = 40;
            // 
            // Storage
            // 
            this.Storage.HeaderText = "Storage";
            this.Storage.Name = "Storage";
            this.Storage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Storage.Width = 50;
            // 
            // DBTriggers
            // 
            this.DBTriggers.HeaderText = "DBTriggers";
            this.DBTriggers.Name = "DBTriggers";
            this.DBTriggers.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DBTriggers.Width = 66;
            // 
            // Synonyms
            // 
            this.Synonyms.HeaderText = "Synonyms";
            this.Synonyms.Name = "Synonyms";
            this.Synonyms.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Synonyms.Width = 61;
            // 
            // UserDefinedandXml
            // 
            this.UserDefinedandXml.HeaderText = "User Defined Types & XML Schema";
            this.UserDefinedandXml.Name = "UserDefinedandXml";
            this.UserDefinedandXml.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.UserDefinedandXml.Width = 107;
            // 
            // grpSelector
            // 
            this.grpSelector.BackColor = System.Drawing.Color.Transparent;
            this.grpSelector.Controls.Add(this.label12);
            this.grpSelector.Controls.Add(this.btnScriptData);
            this.grpSelector.Controls.Add(this.btnScriptObj);
            this.grpSelector.Location = new System.Drawing.Point(181, 14);
            this.grpSelector.Name = "grpSelector";
            this.grpSelector.Size = new System.Drawing.Size(839, 474);
            this.grpSelector.TabIndex = 11;
            this.grpSelector.TabStop = false;
            this.grpSelector.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Calibri", 12.25F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.Maroon;
            this.label12.Location = new System.Drawing.Point(333, 58);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(173, 21);
            this.label12.TabIndex = 11;
            this.label12.Text = "Select Script Operation";
            // 
            // btnScriptData
            // 
            this.btnScriptData.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnScriptData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnScriptData.ForeColor = System.Drawing.Color.Maroon;
            this.btnScriptData.Location = new System.Drawing.Point(226, 199);
            this.btnScriptData.Name = "btnScriptData";
            this.btnScriptData.Size = new System.Drawing.Size(93, 124);
            this.btnScriptData.TabIndex = 10;
            this.btnScriptData.Text = "Script Data";
            this.btnScriptData.UseVisualStyleBackColor = true;
            this.btnScriptData.Click += new System.EventHandler(this.btnScriptData_Click);
            // 
            // btnScriptObj
            // 
            this.btnScriptObj.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnScriptObj.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnScriptObj.ForeColor = System.Drawing.Color.Maroon;
            this.btnScriptObj.Location = new System.Drawing.Point(550, 199);
            this.btnScriptObj.Name = "btnScriptObj";
            this.btnScriptObj.Size = new System.Drawing.Size(99, 124);
            this.btnScriptObj.TabIndex = 2;
            this.btnScriptObj.Text = "Script Objects";
            this.btnScriptObj.UseVisualStyleBackColor = true;
            this.btnScriptObj.Click += new System.EventHandler(this.btnScriptObj_Click);
            // 
            // dbselector
            // 
            this.dbselector.HeaderText = "DBSelector";
            this.dbselector.Name = "dbselector";
            this.dbselector.Width = 67;
            // 
            // DbScpritForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1334, 602);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpsummary);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpSelector);
            this.Controls.Add(this.grpSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DbScpritForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Script Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDatabasesForScriptObj)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpsummary.ResumeLayout(false);
            this.grpsummary.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTableNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgChosenObjects)).EndInit();
            this.grpSelector.ResumeLayout(false);
            this.grpSelector.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsrname;
        private System.Windows.Forms.TextBox txtpass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnnext2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgDatabasesForScriptObj;
        private System.Windows.Forms.GroupBox grpsummary;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbltbremaining;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbldone;
        private System.Windows.Forms.Label lblchildcomp;
        private System.Windows.Forms.Label lblcurrentdb;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label Statusbar;
        private System.Windows.Forms.Label lblcurrObject;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblelapsedtime;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.CheckBox chktoSingleFile;
        private System.Windows.Forms.TextBox txtoutputdir;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DataGridView dgChosenObjects;
        private System.Windows.Forms.Label lblScriptType;
        private System.Windows.Forms.Button btnsettings;
        private System.Windows.Forms.DataGridViewButtonColumn DBName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Table;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TableTriggers;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Constraints;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Views;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SPs;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Functions;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Roles;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SecuritySchema;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Users;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Storage;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DBTriggers;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Synonyms;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UserDefinedandXml;
        private System.Windows.Forms.CheckBox chkScriptDB;
        private System.Windows.Forms.GroupBox grpSelector;
        private System.Windows.Forms.Button btnScriptObj;
        private System.Windows.Forms.Button btnScriptData;
        private System.Windows.Forms.DataGridView dgTableNames;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTables;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dbselector;
    }
}

