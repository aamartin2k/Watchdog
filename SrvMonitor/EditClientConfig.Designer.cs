namespace ConMonitor
{
    partial class EditClientConfig
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
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbpClient = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbTimeout = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbLogPath = new System.Windows.Forms.TextBox();
            this.btLogPath = new System.Windows.Forms.Button();
            this.btAppPath = new System.Windows.Forms.Button();
            this.tbAppPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkEmail = new System.Windows.Forms.CheckBox();
            this.tbpCltStatus = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label19 = new System.Windows.Forms.Label();
            this.btStatus = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.lbRestCount = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.lbxHB = new System.Windows.Forms.ListBox();
            this.label22 = new System.Windows.Forms.Label();
            this.lbUptime = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.lbIni = new System.Windows.Forms.Label();
            this.tbpServer = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tbHbLong = new System.Windows.Forms.TextBox();
            this.tbTimeMult = new System.Windows.Forms.TextBox();
            this.tbTcpPort = new System.Windows.Forms.TextBox();
            this.tbZyanComponent = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.tbUdpPort = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tbpEmail = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tbBody = new System.Windows.Forms.TextBox();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.tbRecipients = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbSender = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbSmtpServer = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dlgFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.cmbClientes = new System.Windows.Forms.ComboBox();
            this.btApply = new System.Windows.Forms.Button();
            this.btEliminar = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tbpClient.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tbpCltStatus.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tbpServer.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tbpEmail.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbpClient);
            this.tabControl1.Controls.Add(this.tbpCltStatus);
            this.tabControl1.Controls.Add(this.tbpServer);
            this.tabControl1.Controls.Add(this.tbpEmail);
            this.tabControl1.Location = new System.Drawing.Point(12, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(473, 267);
            this.tabControl1.TabIndex = 2;
            // 
            // tbpClient
            // 
            this.tbpClient.Controls.Add(this.tableLayoutPanel1);
            this.tbpClient.Location = new System.Drawing.Point(4, 22);
            this.tbpClient.Name = "tbpClient";
            this.tbpClient.Padding = new System.Windows.Forms.Padding(3);
            this.tbpClient.Size = new System.Drawing.Size(465, 241);
            this.tbpClient.TabIndex = 0;
            this.tbpClient.Text = "Cliente";
            this.tbpClient.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.38368F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.61631F));
            this.tableLayoutPanel1.Controls.Add(this.tbTimeout, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbLogPath, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btLogPath, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btAppPath, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbAppPath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbPort, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.chkEmail, 1, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(452, 201);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tbTimeout
            // 
            this.tbTimeout.Location = new System.Drawing.Point(113, 102);
            this.tbTimeout.Name = "tbTimeout";
            this.tbTimeout.Size = new System.Drawing.Size(50, 20);
            this.tbTimeout.TabIndex = 13;
            this.tbTimeout.Text = "30";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Timeout:";
            // 
            // tbLogPath
            // 
            this.tbLogPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLogPath.Location = new System.Drawing.Point(113, 69);
            this.tbLogPath.Name = "tbLogPath";
            this.tbLogPath.Size = new System.Drawing.Size(279, 20);
            this.tbLogPath.TabIndex = 10;
            // 
            // btLogPath
            // 
            this.btLogPath.Location = new System.Drawing.Point(398, 69);
            this.btLogPath.Name = "btLogPath";
            this.btLogPath.Size = new System.Drawing.Size(50, 23);
            this.btLogPath.TabIndex = 9;
            this.btLogPath.Text = "...";
            this.btLogPath.UseVisualStyleBackColor = true;
            this.btLogPath.Click += new System.EventHandler(this.btLogPath_Click);
            // 
            // btAppPath
            // 
            this.btAppPath.Location = new System.Drawing.Point(398, 36);
            this.btAppPath.Name = "btAppPath";
            this.btAppPath.Size = new System.Drawing.Size(50, 23);
            this.btAppPath.TabIndex = 6;
            this.btAppPath.Text = "...";
            this.btAppPath.UseVisualStyleBackColor = true;
            this.btAppPath.Click += new System.EventHandler(this.btAppPath_Click);
            // 
            // tbAppPath
            // 
            this.tbAppPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbAppPath.Location = new System.Drawing.Point(113, 36);
            this.tbAppPath.Name = "tbAppPath";
            this.tbAppPath.Size = new System.Drawing.Size(279, 20);
            this.tbAppPath.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nombre descriptivo:";
            // 
            // tbName
            // 
            this.tbName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbName.Location = new System.Drawing.Point(113, 3);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(279, 20);
            this.tbName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ruta a la aplicacion:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ruta al log:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Puerto:";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(113, 135);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(50, 20);
            this.tbPort.TabIndex = 15;
            this.tbPort.Text = "8000";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Enviar correo:";
            // 
            // chkEmail
            // 
            this.chkEmail.AutoSize = true;
            this.chkEmail.Location = new System.Drawing.Point(113, 168);
            this.chkEmail.Name = "chkEmail";
            this.chkEmail.Size = new System.Drawing.Size(35, 17);
            this.chkEmail.TabIndex = 17;
            this.chkEmail.Text = "   ";
            this.chkEmail.UseVisualStyleBackColor = true;
            // 
            // tbpCltStatus
            // 
            this.tbpCltStatus.Controls.Add(this.tableLayoutPanel4);
            this.tbpCltStatus.Location = new System.Drawing.Point(4, 22);
            this.tbpCltStatus.Name = "tbpCltStatus";
            this.tbpCltStatus.Size = new System.Drawing.Size(465, 241);
            this.tbpCltStatus.TabIndex = 3;
            this.tbpCltStatus.Text = "Estado del cliente";
            this.tbpCltStatus.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btStatus, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label20, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lbRestCount, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label24, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.lbxHB, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.label22, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.lbUptime, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.label21, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.lbIni, 1, 2);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 12);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 5;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(384, 226);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(60, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(43, 13);
            this.label19.TabIndex = 1;
            this.label19.Text = "Estado:";
            // 
            // btStatus
            // 
            this.btStatus.BackColor = System.Drawing.Color.Transparent;
            this.btStatus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btStatus.Location = new System.Drawing.Point(109, 3);
            this.btStatus.Name = "btStatus";
            this.btStatus.Size = new System.Drawing.Size(58, 19);
            this.btStatus.TabIndex = 0;
            this.btStatus.UseVisualStyleBackColor = false;
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(3, 25);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(100, 13);
            this.label20.TabIndex = 2;
            this.label20.Text = "Conteo de reinicios:";
            // 
            // lbRestCount
            // 
            this.lbRestCount.AutoSize = true;
            this.lbRestCount.Location = new System.Drawing.Point(109, 25);
            this.lbRestCount.Name = "lbRestCount";
            this.lbRestCount.Size = new System.Drawing.Size(31, 13);
            this.lbRestCount.TabIndex = 3;
            this.lbRestCount.Text = " ..   ..";
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(53, 100);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(50, 13);
            this.label24.TabIndex = 7;
            this.label24.Text = "Lista HB:";
            // 
            // lbxHB
            // 
            this.lbxHB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxHB.FormattingEnabled = true;
            this.lbxHB.Location = new System.Drawing.Point(109, 103);
            this.lbxHB.Name = "lbxHB";
            this.lbxHB.Size = new System.Drawing.Size(272, 108);
            this.lbxHB.TabIndex = 6;
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(26, 75);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(77, 13);
            this.label22.TabIndex = 4;
            this.label22.Text = "Tiempo activo:";
            // 
            // lbUptime
            // 
            this.lbUptime.AutoSize = true;
            this.lbUptime.Location = new System.Drawing.Point(109, 75);
            this.lbUptime.Name = "lbUptime";
            this.lbUptime.Size = new System.Drawing.Size(31, 13);
            this.lbUptime.TabIndex = 5;
            this.lbUptime.Text = " ..   ..";
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(68, 50);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(35, 13);
            this.label21.TabIndex = 8;
            this.label21.Text = "Inicio:";
            // 
            // lbIni
            // 
            this.lbIni.AutoSize = true;
            this.lbIni.Location = new System.Drawing.Point(109, 50);
            this.lbIni.Name = "lbIni";
            this.lbIni.Size = new System.Drawing.Size(31, 13);
            this.lbIni.TabIndex = 9;
            this.lbIni.Text = " ..   ..";
            // 
            // tbpServer
            // 
            this.tbpServer.Controls.Add(this.tableLayoutPanel3);
            this.tbpServer.Location = new System.Drawing.Point(4, 22);
            this.tbpServer.Name = "tbpServer";
            this.tbpServer.Size = new System.Drawing.Size(465, 241);
            this.tbpServer.TabIndex = 4;
            this.tbpServer.Text = "Servidor";
            this.tbpServer.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.textBox1, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.tbHbLong, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.tbTimeMult, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.tbTcpPort, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.tbZyanComponent, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label13, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label14, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label15, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label16, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label17, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.tbUdpPort, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label18, 0, 5);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 12);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(453, 204);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(160, 153);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(290, 48);
            this.textBox1.TabIndex = 11;
            this.textBox1.Visible = false;
            // 
            // tbHbLong
            // 
            this.tbHbLong.Location = new System.Drawing.Point(160, 123);
            this.tbHbLong.Name = "tbHbLong";
            this.tbHbLong.Size = new System.Drawing.Size(100, 20);
            this.tbHbLong.TabIndex = 10;
            // 
            // tbTimeMult
            // 
            this.tbTimeMult.Location = new System.Drawing.Point(160, 93);
            this.tbTimeMult.Name = "tbTimeMult";
            this.tbTimeMult.Size = new System.Drawing.Size(100, 20);
            this.tbTimeMult.TabIndex = 9;
            // 
            // tbTcpPort
            // 
            this.tbTcpPort.Location = new System.Drawing.Point(160, 63);
            this.tbTcpPort.Name = "tbTcpPort";
            this.tbTcpPort.Size = new System.Drawing.Size(100, 20);
            this.tbTcpPort.TabIndex = 8;
            // 
            // tbZyanComponent
            // 
            this.tbZyanComponent.Location = new System.Drawing.Point(160, 33);
            this.tbZyanComponent.Name = "tbZyanComponent";
            this.tbZyanComponent.Size = new System.Drawing.Size(262, 20);
            this.tbZyanComponent.TabIndex = 7;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(87, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Puerto UDP:";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 30);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(151, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Nombre de componente Zyan:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(89, 60);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Puerto TCP:";
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(58, 90);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(96, 13);
            this.label16.TabIndex = 3;
            this.label16.Text = "Factor de Timeout:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(47, 120);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(107, 13);
            this.label17.TabIndex = 4;
            this.label17.Text = "Longitud de cola HB:";
            // 
            // tbUdpPort
            // 
            this.tbUdpPort.Location = new System.Drawing.Point(160, 3);
            this.tbUdpPort.Name = "tbUdpPort";
            this.tbUdpPort.Size = new System.Drawing.Size(100, 20);
            this.tbUdpPort.TabIndex = 5;
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(117, 150);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(37, 13);
            this.label18.TabIndex = 6;
            this.label18.Text = "Texto:";
            this.label18.Visible = false;
            // 
            // tbpEmail
            // 
            this.tbpEmail.Controls.Add(this.tableLayoutPanel2);
            this.tbpEmail.Location = new System.Drawing.Point(4, 22);
            this.tbpEmail.Name = "tbpEmail";
            this.tbpEmail.Size = new System.Drawing.Size(465, 241);
            this.tbpEmail.TabIndex = 5;
            this.tbpEmail.Text = "Correo electrónico";
            this.tbpEmail.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tbBody, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.tbSubject, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.tbRecipients, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.tbPassword, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbSender, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.tbSmtpServer, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 5);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 12);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(453, 204);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // tbBody
            // 
            this.tbBody.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBody.Location = new System.Drawing.Point(127, 153);
            this.tbBody.Multiline = true;
            this.tbBody.Name = "tbBody";
            this.tbBody.Size = new System.Drawing.Size(323, 48);
            this.tbBody.TabIndex = 11;
            // 
            // tbSubject
            // 
            this.tbSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSubject.Location = new System.Drawing.Point(127, 123);
            this.tbSubject.Name = "tbSubject";
            this.tbSubject.Size = new System.Drawing.Size(323, 20);
            this.tbSubject.TabIndex = 10;
            // 
            // tbRecipients
            // 
            this.tbRecipients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRecipients.Location = new System.Drawing.Point(127, 93);
            this.tbRecipients.Name = "tbRecipients";
            this.tbRecipients.Size = new System.Drawing.Size(323, 20);
            this.tbRecipients.TabIndex = 9;
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Location = new System.Drawing.Point(127, 63);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(323, 20);
            this.tbPassword.TabIndex = 8;
            // 
            // tbSender
            // 
            this.tbSender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSender.Location = new System.Drawing.Point(127, 33);
            this.tbSender.Name = "tbSender";
            this.tbSender.Size = new System.Drawing.Size(323, 20);
            this.tbSender.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Servidor SMTP:";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Buzón de Origen:";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(57, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Contraseña:";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Direcciones de destino:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 120);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(102, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Asunto del mensaje:";
            // 
            // tbSmtpServer
            // 
            this.tbSmtpServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSmtpServer.Location = new System.Drawing.Point(127, 3);
            this.tbSmtpServer.Name = "tbSmtpServer";
            this.tbSmtpServer.Size = new System.Drawing.Size(323, 20);
            this.tbSmtpServer.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(84, 150);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Texto:";
            // 
            // dlgFolderBrowser
            // 
            this.dlgFolderBrowser.ShowNewFolderButton = false;
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(121, 312);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 26);
            this.btOk.TabIndex = 3;
            this.btOk.Text = "Aceptar";
            this.btOk.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(281, 312);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 26);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Cancelar";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // cmbClientes
            // 
            this.cmbClientes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClientes.FormattingEnabled = true;
            this.cmbClientes.Location = new System.Drawing.Point(12, 12);
            this.cmbClientes.Name = "cmbClientes";
            this.cmbClientes.Size = new System.Drawing.Size(125, 21);
            this.cmbClientes.TabIndex = 5;
            // 
            // btApply
            // 
            this.btApply.Location = new System.Drawing.Point(169, 12);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(125, 23);
            this.btApply.TabIndex = 6;
            this.btApply.Text = "Aplicar cambios";
            this.btApply.UseVisualStyleBackColor = true;
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // btEliminar
            // 
            this.btEliminar.Location = new System.Drawing.Point(281, 12);
            this.btEliminar.Name = "btEliminar";
            this.btEliminar.Size = new System.Drawing.Size(125, 23);
            this.btEliminar.TabIndex = 7;
            this.btEliminar.Text = "Eliminar cliente";
            this.btEliminar.UseVisualStyleBackColor = true;
            this.btEliminar.Click += new System.EventHandler(this.btEliminar_Click);
            // 
            // EditClientConfig
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(491, 346);
            this.ControlBox = false;
            this.Controls.Add(this.btEliminar);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.cmbClientes);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditClientConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Configuracion de clientes";
            this.tabControl1.ResumeLayout(false);
            this.tbpClient.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tbpCltStatus.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tbpServer.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tbpEmail.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbpClient;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbLogPath;
        private System.Windows.Forms.Button btLogPath;
        private System.Windows.Forms.Button btAppPath;
        private System.Windows.Forms.TextBox tbAppPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbTimeout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkEmail;
        private System.Windows.Forms.FolderBrowserDialog dlgFolderBrowser;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TabPage tbpCltStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btStatus;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lbRestCount;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lbUptime;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ListBox lbxHB;
        private System.Windows.Forms.ComboBox cmbClientes;
        private System.Windows.Forms.Button btApply;
        private System.Windows.Forms.Button btEliminar;
        private System.Windows.Forms.TabPage tbpServer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox tbHbLong;
        private System.Windows.Forms.TextBox tbTimeMult;
        private System.Windows.Forms.TextBox tbTcpPort;
        private System.Windows.Forms.TextBox tbZyanComponent;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbUdpPort;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabPage tbpEmail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox tbBody;
        private System.Windows.Forms.TextBox tbSubject;
        private System.Windows.Forms.TextBox tbRecipients;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbSender;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbSmtpServer;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label lbIni;
    }
}