using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Monitor.Shared;

namespace ConMonitor
{
    public partial class EditConfig : Form
    {
        private bool nonNumberEntered = false;
        private ClientConfigData _editClient;
        private SystemConfigData _editConfig;

        #region Eventos
        internal event ClientConfigEventHandler CreateConfig;
        internal event UniversalHandler CancelConfig;
        internal event UniversalHandler AcceptEdit;
        internal event UniversalHandler SystemConfigEdit;

        #endregion

        public EditConfig()
        {
            InitializeComponent();
        }

        internal List<string> ClientNames { get; set; }

        internal ClientConfigData ClientData 
        {
            get { return _editClient; }
            set 
            {
                _editClient = value;

                // pasar valores a controles form
                this.tbName.Text = _editClient.Name;
                this.tbName.ReadOnly = true;
                this.tbAppPath.Text = _editClient.AppFilePath;
                this.tbLogPath.Text = _editClient.LogFilePath;
                this.tbPort.Text = _editClient.Port.ToString();
                this.tbTimeout.Text = _editClient.Timeout.ToString();
                this.chkEmail.Checked = _editClient.MailEnabled;
            }
        }

        internal SystemConfigData SystemData
        {
            get { return _editConfig; }
            set 
            {
                _editConfig = value;

                // pasar valores a controles form
               
                this.tbUdpPort.Text = _editConfig.UdpServerPort.ToString();
                this.tbZyanComponent.Text = _editConfig.ZyanServerName;

                this.tbTcpPort.Text = _editConfig.ZyanServerPort.ToString();
             
                this.tbSmtpServer.Text = _editConfig.SMtpServer;
                this.tbSender.Text = _editConfig.Source;
                this.tbPassword.Text = _editConfig.Password;
                this.tbRecipients.Text = _editConfig.Destination;
                this.tbSubject.Text = _editConfig.Subject;
                this.tbBody.Text = _editConfig.Body;
            }
        }

        internal void SetClientConfig()
        {
            this.tabControl1.Controls.Remove(this.tbpEmail);
            this.tabControl1.Controls.Remove(this.tbpSystem);

            this.btOk.Click += CheckAndCreateClient ;
        }

        internal void SetClientEdit()
        {
            this.tabControl1.Controls.Remove(this.tbpEmail);
            this.tabControl1.Controls.Remove(this.tbpSystem);

            this.btOk.Click += CheckAndUpdateClient;
        }

        internal void SetSystemConfig()
        {
            //this.tabControl1.Controls.Add(this.tbpEmail);
            //this.tabControl1.Controls.Add(this.tbpSystem);
            this.tabControl1.Controls.Remove(this.tbpClient);

            this.btOk.Click += CheckAndUpdateSystem;
        }

        
        private void btAppPath_Click(object sender, EventArgs e)
        {
            dlgOpenFile.FileName = string.Empty;
            dlgOpenFile.Filter = "Ejecutables (*.exe; *.bat; *.cmd) |*.exe;*.bat;*.cmd|Todos los archivos (*.*)|*.*";
            dlgOpenFile.CheckFileExists = true;
            dlgOpenFile.Multiselect = false;
            dlgOpenFile.ShowDialog();

            tbAppPath.Text = dlgOpenFile.FileName;
        }

        private void btLogPath_Click(object sender, EventArgs e)
        {
            dlgOpenFile.FileName = string.Empty;
            dlgOpenFile.Filter = "Todos los archivos (*)|*.*|Texto (*.txt)|*.txt";
            dlgOpenFile.Multiselect = false;
            dlgOpenFile.ShowDialog();

            tbLogPath.Text = dlgOpenFile.FileName;
        }

        private void tbNumeric_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad.
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace.
                    if (e.KeyCode != Keys.Back)
                    {
                        // A non-numerical keystroke was pressed.
                        // Set the flag to true and evaluate in KeyPress event.
                        nonNumberEntered = true;
                    }
                }
            }

        }

        private void tbNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nonNumberEntered == true)
            {
                // Impedir entrada del caracter no numerico, determinado en tbNumeric_KeyDown
                e.Handled = true;
            }

        }

        private void CheckAndUpdateSystem(object sender, EventArgs e)
        {
            if (DoChecksSystem())
                DoUpdateConfigSystem();
        }

        private bool DoChecksSystem()
        {


            return true;
        }

        private void DoUpdateConfigSystem()
        {
            // pasar valores de controles a clase
            _editConfig.UdpServerPort = int.Parse(this.tbUdpPort.Text) ;
            _editConfig.ZyanServerName = this.tbZyanComponent.Text;
            _editConfig.ZyanServerPort = int.Parse(this.tbTcpPort.Text);

            _editConfig.SMtpServer = this.tbSmtpServer.Text;
            _editConfig.Source = this.tbSender.Text;
            _editConfig.Password = this.tbPassword.Text;
            _editConfig.Destination = this.tbRecipients.Text;
            _editConfig.Subject = this.tbSubject.Text;
            _editConfig.Body = this.tbBody.Text;

            // enviar notificacion
            if (SystemConfigEdit != null)
                SystemConfigEdit();
        }


        private void CheckAndCreateClient(object sender, EventArgs e)
        {
            if (DoChecks())
                DoCreateConfig();
        }

        private void CheckAndUpdateClient(object sender, EventArgs e)
        {
            if (DoChecks())
                DoUpdateConfig();
        }

        private void DoUpdateConfig()
        { 
            // pasar controles a props de clase config
            _editClient.Name = this.tbName.Text;
            _editClient.AppName = System.IO.Path.GetFileNameWithoutExtension(tbAppPath.Text);
            _editClient.AppFilePath = this.tbAppPath.Text;

            _editClient.LogFilePath = this.tbLogPath.Text;
            _editClient.Port = int.Parse(tbPort.Text);
            _editClient.Timeout = double.Parse(tbTimeout.Text);
            _editClient.MailEnabled = this.chkEmail.Checked;

            // enviar notificacion
            if (AcceptEdit != null)
                AcceptEdit();
        }

        private void DoCreateConfig()
        {
            if (CreateConfig != null)
            {
                ClientConfigData data = new ClientConfigData();

                data.Name = tbName.Text;
                data.AppName = System.IO.Path.GetFileNameWithoutExtension(tbAppPath.Text);
                data.AppFilePath = tbAppPath.Text;
                data.LogFilePath = tbLogPath.Text;
                data.Port = int.Parse(tbPort.Text);
                data.Timeout = double.Parse(tbTimeout.Text);
                data.MailEnabled = chkEmail.Checked;

                ClientConfigEventArgs ev = new ClientConfigEventArgs(data);

                CreateConfig(this, ev);
            }
        }

        private bool DoChecks()
        {
            if (tbName.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un nombre.");
                tbName.Focus();
                return false;
            }

            //  Da null en modo edicion porque ClientNames no se asigna.
            if (ClientNames != null)
            {
                if (ClientNames.Contains(tbName.Text))
                {
                    MessageBox.Show("El nombre ya está registrado. Se requiere un nombre diferente.");
                    tbName.Focus();
                    return false;
                }
            }

            if (tbAppPath.Text.Length == 0)
            {
                MessageBox.Show("Se requiere una aplicación.");
                tbAppPath.Focus();
                return false;
            }

            if (tbPort.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un puerto.");
                tbPort.Focus();
                return false;
            }

            if (tbTimeout.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un valor de timeout.");
                tbTimeout.Focus();
                return false;
            }

            return true;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (CancelConfig != null)
                CancelConfig();
        }

        

        
    }
}
