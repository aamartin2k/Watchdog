namespace WinClienteUdp
{
    partial class Form1
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tbInterval = new System.Windows.Forms.TextBox();
            this.tbLocalPort = new System.Windows.Forms.TextBox();
            this.tbSPort = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tbIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.chkSerial = new System.Windows.Forms.CheckBox();
            this.tbFormat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbKeyId = new System.Windows.Forms.RadioButton();
            this.rbKeyPort = new System.Windows.Forms.RadioButton();
            this.lbMsg = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel3.Controls.Add(this.tbInterval, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.tbLocalPort, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.tbSPort, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label13, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label14, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label15, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label16, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.tbIp, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.tbID, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.chkSerial, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.tbFormat, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.rbKeyId, 2, 4);
            this.tableLayoutPanel3.Controls.Add(this.rbKeyPort, 2, 2);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 7;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(285, 221);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // tbInterval
            // 
            this.tbInterval.Location = new System.Drawing.Point(85, 93);
            this.tbInterval.Name = "tbInterval";
            this.tbInterval.Size = new System.Drawing.Size(46, 20);
            this.tbInterval.TabIndex = 9;
            this.tbInterval.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbNumeric_KeyDown);
            this.tbInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNumeric_KeyPress);
            // 
            // tbLocalPort
            // 
            this.tbLocalPort.Location = new System.Drawing.Point(85, 63);
            this.tbLocalPort.Name = "tbLocalPort";
            this.tbLocalPort.Size = new System.Drawing.Size(46, 20);
            this.tbLocalPort.TabIndex = 8;
            this.tbLocalPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbNumeric_KeyDown);
            this.tbLocalPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNumeric_KeyPress);
            // 
            // tbSPort
            // 
            this.tbSPort.Location = new System.Drawing.Point(85, 33);
            this.tbSPort.Name = "tbSPort";
            this.tbSPort.Size = new System.Drawing.Size(46, 20);
            this.tbSPort.TabIndex = 7;
            this.tbSPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbNumeric_KeyDown);
            this.tbSPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNumeric_KeyPress);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Ip Servidor:";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(30, 30);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 26);
            this.label14.TabIndex = 1;
            this.label14.Text = "Puerto Servidor:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 60);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Puerto Local:";
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 90);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(63, 26);
            this.label16.TabIndex = 3;
            this.label16.Text = "Intervalo en segundos:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbIp
            // 
            this.tbIp.Location = new System.Drawing.Point(85, 3);
            this.tbIp.Name = "tbIp";
            this.tbIp.Size = new System.Drawing.Size(134, 20);
            this.tbIp.TabIndex = 5;
            this.tbIp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbNumeric_KeyDown);
            this.tbIp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNumeric_KeyPress);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "ID Cliente:";
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(85, 123);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(65, 20);
            this.tbID.TabIndex = 11;
            this.tbID.Text = "123456";
            this.tbID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbID_KeyPress);
            // 
            // chkSerial
            // 
            this.chkSerial.AutoSize = true;
            this.chkSerial.Location = new System.Drawing.Point(85, 153);
            this.chkSerial.Name = "chkSerial";
            this.chkSerial.Size = new System.Drawing.Size(110, 17);
            this.chkSerial.TabIndex = 12;
            this.chkSerial.Text = "Usar Consecutivo";
            this.chkSerial.UseVisualStyleBackColor = true;
            this.chkSerial.CheckedChanged += new System.EventHandler(this.chkSerial_CheckedChanged);
            // 
            // tbFormat
            // 
            this.tbFormat.Location = new System.Drawing.Point(85, 180);
            this.tbFormat.Name = "tbFormat";
            this.tbFormat.Size = new System.Drawing.Size(134, 20);
            this.tbFormat.TabIndex = 14;
            this.tbFormat.Text = "dd/MMM/yyyy HH:mm:ss";
            this.tbFormat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbID_KeyPress);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 26);
            this.label2.TabIndex = 13;
            this.label2.Text = "Timestamp format:";
            // 
            // rbKeyId
            // 
            this.rbKeyId.AutoSize = true;
            this.rbKeyId.Location = new System.Drawing.Point(227, 123);
            this.rbKeyId.Name = "rbKeyId";
            this.rbKeyId.Size = new System.Drawing.Size(43, 17);
            this.rbKeyId.TabIndex = 15;
            this.rbKeyId.TabStop = true;
            this.rbKeyId.Text = "Key";
            this.rbKeyId.UseVisualStyleBackColor = true;
            this.rbKeyId.CheckedChanged += new System.EventHandler(this.rbKey_CheckedChanged);
            // 
            // rbKeyPort
            // 
            this.rbKeyPort.AutoSize = true;
            this.rbKeyPort.Location = new System.Drawing.Point(227, 63);
            this.rbKeyPort.Name = "rbKeyPort";
            this.rbKeyPort.Size = new System.Drawing.Size(43, 17);
            this.rbKeyPort.TabIndex = 16;
            this.rbKeyPort.TabStop = true;
            this.rbKeyPort.Text = "Key";
            this.rbKeyPort.UseVisualStyleBackColor = true;
            this.rbKeyPort.CheckedChanged += new System.EventHandler(this.rbKey_CheckedChanged);
            // 
            // lbMsg
            // 
            this.lbMsg.AutoSize = true;
            this.lbMsg.Location = new System.Drawing.Point(10, 236);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(25, 13);
            this.lbMsg.TabIndex = 10;
            this.lbMsg.Text = ". . . ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 261);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 27);
            this.button1.TabIndex = 3;
            this.button1.Text = "Iniciar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(150, 260);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 27);
            this.button2.TabIndex = 4;
            this.button2.Text = "Detener";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 300);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.lbMsg);
            this.Name = "Form1";
            this.Text = "Cliente Udp HB";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox tbInterval;
        private System.Windows.Forms.TextBox tbLocalPort;
        private System.Windows.Forms.TextBox tbSPort;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbIp;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.CheckBox chkSerial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFormat;
        private System.Windows.Forms.RadioButton rbKeyId;
        private System.Windows.Forms.RadioButton rbKeyPort;
    }
}

