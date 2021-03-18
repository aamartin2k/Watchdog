using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Monitor.Shared;

namespace ConMonitor
{
    public partial class EditClientConfig : Form
    {
        #region Declaraciones

        private bool _pending;
        private bool _populate;
        private bool _nonNumberEntered = false;
        private bool _ignoreNameCheck;

        private ClientData _editClient;
        private SystemConfigData _editConfig;

        private Dictionary<string, ClientData> _clientConfigList;
        private List<string> _clientNames;
        private Hashtable _portCheck;
        #endregion

        #region Eventos
        internal event SystemConfigEventHandler SendSystemConfig;
        internal event ClientConfigListEventHandler SaveClientData;

        internal event UniversalHandler CancelConfig;
        internal event UniversalHandler AcceptEdit;

        internal event UniversalHandler RequestClientConfig;
        internal event UniversalHandler RequestSystemConfig;
        
        #endregion

        public EditClientConfig()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        #region Edicion de Cliente
        
        #region Propiedades

        private ClientData ClientData 
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

                CambiosPendientes = false;
                
                if (_editClient.Status == ClientStatus.Detenido)
                    this.btStatus.BackColor = Color.Red;

               Color btColor;

                switch (_editClient.Status)
	                {
		                case ClientStatus.Inicial:
                        btColor = Color.Yellow;
                        break;

                        case ClientStatus.Operacional:
                        btColor = Color.Green;
                         break;

                        case ClientStatus.Reiniciado:
                        btColor = Color.Gold;
                         break;

                        case ClientStatus.Detenido:
                        btColor = Color.Red;
                         break;

                        case ClientStatus.Muerto:
                        default:
                        btColor = Color.Black;
                        break;
	                }
                this.btStatus.BackColor = btColor;

                this.lbRestCount.Text = _editClient.RestartCount.ToString();
                this.lbIni.Text = _editClient.StartTime.ToString();
                this.lbUptime.Text= _editClient.Uptime.ToString();
                this.lbxHB.Items.Clear();
                this.lbxHB.Items.AddRange(_editClient.HeartBeatList);
	
            }
        }

        private bool CambiosPendientes
        {
            get { return _pending; }

            set 
            {
                _pending = value;

                this.btApply.Enabled = value;
                this.cmbClientes.Enabled = !value;
                this.btOk.Enabled = !value;
            }
        }
        
        #endregion
       
        #region Envio de notificaciones --> Out

        private void OnSaveClientData(ClientConfigListEventArgs e)
        {
            if (SaveClientData != null)
                SaveClientData(this, e);
        }

        private void OnRequestClientConfig()
        {
            if (RequestClientConfig != null)
                RequestClientConfig();
        }

        private void OnCancelConfig()
        {
            if (CancelConfig != null)
                CancelConfig();
        }

        private void OnAcceptEdit()
        {
            if (AcceptEdit != null)
                AcceptEdit();
        }

        #endregion

        #region Manejo de notificaciones <-- In

        internal void ReceiveClientConfig(object sender, ClientConfigListEventArgs e)
        {
            _clientConfigList = e.List;

            this.cmbClientes.Items.Clear();

            // create names/ports list
            _clientNames = new List<string>();
            _portCheck = new Hashtable();

            foreach (var item in _clientConfigList)
            {
                _clientNames.Add(item.Value.Name);
                // create port list
                cmbClientes.Items.Add(item.Value);
                _portCheck.Add(item.Value.Port, item.Value.Name);
            }

            if (_populate)
            {
                // seguimiento a cambios Text_Changed
                tbAppPath.TextChanged += Text_Changed;
                tbLogPath.TextChanged += Text_Changed;
                tbTimeout.TextChanged += Text_Changed;
                tbPort.TextChanged += Text_Changed;
                chkEmail.CheckedChanged += Text_Changed;

            
                // Excluir el llenado de controles cuando se crea un nuevo cliente
                cmbClientes.SelectedItem = cmbClientes.Items[0];
                ClientData = cmbClientes.SelectedItem as ClientData;
                cmbClientes.SelectedIndexChanged += cmbClientes_SelectedIndexChanged;
            }
        }

        internal void DoSetCreateClientMode()
        {
            this.Text = "Registrar nuevo cliente";
            this.btOk.Click += CheckAndCreateClient;
            _ignoreNameCheck = false;
            _populate = false;
            this.cmbClientes.Visible = false;
            this.btEliminar.Visible = false;
            this.btApply.Visible = false;
            //this.tabControl1.Top = 11;
            this.tabControl1.Controls.Remove(this.tbpCltStatus);
            this.tabControl1.Controls.Remove(this.tbpServer);
            this.tabControl1.Controls.Remove(this.tbpEmail);

            // Notify request for data
            OnRequestClientConfig();
        }

        internal void DoSetEditClientMode()
        {
            this.Text = "Editar datos de cliente";
            this.btOk.Click += CheckAndUpdateClient;
            _ignoreNameCheck = true;
            _populate = true;
            this.btEliminar.Visible = false;

            this.tabControl1.Controls.Remove(this.tbpServer);
            this.tabControl1.Controls.Remove(this.tbpEmail);

            // Notify request for data
            OnRequestClientConfig();
        }
       
        internal void DoSetDeleteClientMode()
        {
            this.Text = "Eliminar cliente";
            this.btOk.Click += SaveDeleteClient;

            _populate = true;
            this.btApply.Visible = false;
            this.btEliminar.Top = this.btApply.Top;
            this.btEliminar.Left = this.btApply.Left;

            this.tabControl1.Controls.Remove(this.tbpServer);
            this.tabControl1.Controls.Remove(this.tbpEmail);

            this.tbAppPath.ReadOnly = true;
            this.tbLogPath.ReadOnly = true;
            this.tbPort.ReadOnly = true;
            this.tbTimeout.ReadOnly = true; 
            this.chkEmail.Enabled = false;  
            this.btLogPath.Enabled = false;
            this.btAppPath.Enabled = false;

            // Notify request for data
            OnRequestClientConfig();
        }
   
        #endregion

        #region Manejo de datos y controles

        private void DoUpdateConfig()
        {
            // pasar controles a props de clase config
            _editClient.Name = this.tbName.Text;
            _editClient.AppFilePath = this.tbAppPath.Text;

            _editClient.LogFilePath = this.tbLogPath.Text;
            _editClient.Port = int.Parse(tbPort.Text);
            _editClient.Timeout = double.Parse(tbTimeout.Text);
            _editClient.MailEnabled = this.chkEmail.Checked;

            CambiosPendientes = false;
  
        }

        private void DoCreateConfig()
        {
            ClientData data = new ClientData();

            data.Name = tbName.Text;
            data.AppFilePath = tbAppPath.Text;
            data.LogFilePath = tbLogPath.Text;
            data.Port = int.Parse(tbPort.Text);
            data.Timeout = double.Parse(tbTimeout.Text);
            data.MailEnabled = chkEmail.Checked;

            _clientConfigList.Add(data.Name, data);

            ClientConfigListEventArgs ev = new ClientConfigListEventArgs(_clientConfigList);
            OnSaveClientData(ev);

        }

        private void DeleteClient()
        {
            ClientData clt = cmbClientes.SelectedItem as ClientData;
            _clientConfigList.Remove(clt.Name);

            ClientConfigListEventArgs ev = new ClientConfigListEventArgs(_clientConfigList);
            ReceiveClientConfig(this, ev);
        }

        private bool DoChecks()
        {
            if (tbName.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un nombre.");
                tbName.Focus();
                return false;
            }

            if (!_ignoreNameCheck)
            {
                if (_clientNames.Contains(tbName.Text))
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
            else if (!System.IO.File.Exists( tbAppPath.Text))
            {
                MessageBox.Show("La ruta al archivo de aplicación no es válida.");
                tbAppPath.Focus();
                return false;
            }

            if ((tbLogPath.Text.Length != 0) && !System.IO.File.Exists(tbLogPath.Text))
            {
                MessageBox.Show("La ruta al archivo de log no es válida.");
                tbLogPath.Focus();
                return false;
            }
            if (tbPort.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un puerto.");
                tbPort.Focus();
                return false;
            }

            bool flag = PortInUse(tbPort.Text, tbName.Text);
            if (flag)
            {
                MessageBox.Show("El puerto ya está registrado. Se requiere un puerto diferente.");
                tbTimeout.Focus();
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

        private bool PortInUse(string port, string name)
        {
            int iPort = int.Parse(port);

                try 
	            {
                    if (_portCheck.ContainsKey(iPort))
                    {    // lo contiene, comprobar que no es el mismo
                        string savedName = _portCheck[iPort] as string;
                        if (savedName == name)
                            return false;
                        else
                            return true;
                    }
                    else
                    { 
                        // no lo contiene, 
                        return false;
                    }

	            }
	            catch (Exception)
	            {
            	    return true;
	            }
        }

        #endregion

        #region Controladores de eventos de controles

        private void btAppPath_Click(object sender, EventArgs e)
        {
            dlgOpenFile.FileName = string.Empty;
            dlgOpenFile.Filter = "Ejecutables (*.exe; *.bat; *.cmd) |*.exe;*.bat;*.cmd|Todos los archivos (*.*)|*.*";
            dlgOpenFile.CheckFileExists = true;
            dlgOpenFile.Multiselect = false;
            var ret = dlgOpenFile.ShowDialog();

            if (ret == DialogResult.OK)
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

       

        private void CheckAndCreateClient(object sender, EventArgs e)
        {
            if (DoChecks())
            {
                DoCreateConfig();
                OnAcceptEdit();
            }
        }

        private void CheckAndUpdateClient(object sender, EventArgs e)
        {
            if (DoChecks())
            {
                DoUpdateConfig();

                ClientConfigListEventArgs ev = new ClientConfigListEventArgs(_clientConfigList);
                OnSaveClientData(ev);

                OnAcceptEdit();
            }
        }

        private void SaveDeleteClient(object sender, EventArgs e)
        {
            ClientConfigListEventArgs ev = new ClientConfigListEventArgs(_clientConfigList);
            OnSaveClientData(ev);

            OnAcceptEdit();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            OnCancelConfig();
        }

        private void Text_Changed(object sender, EventArgs e)
        {
            CambiosPendientes = true;
        }

        private void cmbClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClientData = cmbClientes.SelectedItem as ClientData;
        }

        private void btApply_Click(object sender, EventArgs e)
        {
            if (DoChecks())
                DoUpdateConfig();
        }

        private void btEliminar_Click(object sender, EventArgs e)
        {

             DialogResult ret=   MessageBox.Show("Se eliminará el cliente. Desea continuar?", 
                                                 "Advertencia", MessageBoxButtons.YesNo);
             if (ret == DialogResult.Yes)
                 DeleteClient();
        }

        #endregion
        #endregion

        #region Edicion de Sistema

        private SystemConfigData SystemData
        {
            get { return _editConfig; }
            set
            {
                _editConfig = value;

                // pasar valores a controles form

                this.tbUdpPort.Text = _editConfig.UdpServerPort.ToString();
                this.tbZyanComponent.Text = _editConfig.ZyanServerName;
                this.tbTcpPort.Text = _editConfig.ZyanServerPort.ToString();
                this.tbTimeMult.Text = _editConfig.TimeoutFactor.ToString();
                this.tbHbLong.Text = _editConfig.HeartBeatHistory.ToString();

                this.tbSmtpServer.Text = _editConfig.SMtpServer;
                this.tbSender.Text = _editConfig.Source;
                this.tbPassword.Text = _editConfig.Password;
                this.tbRecipients.Text = _editConfig.Destination;
                this.tbSubject.Text = _editConfig.Subject;
                this.tbBody.Text = _editConfig.Body;
            }

        }

        private void CheckAndUpdateSystem(object sender, EventArgs e)
        {
            if (DoChecksSystem())
            {
                DoUpdateConfigSystem();

                // Enviar ev a dbHandler
                SystemConfigEventArgs ev = new SystemConfigEventArgs(_editConfig);
                OnSendSystemConfig(ev);

                // enviar notificacion
                if (AcceptEdit != null)
                    AcceptEdit();
            }
        }

        private bool DoChecksSystem()
        {
            if (tbZyanComponent.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un nombre.");
                tbZyanComponent.Focus();
                return false;
            }

            if (tbUdpPort.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un puerto.");
                tbUdpPort.Focus();
                return false;
            }
            //tbTcpPort
            if (tbTcpPort.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un puerto.");
                tbTcpPort.Focus();
                return false;
            }
            if (tbTimeMult.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un valor de timeout.");
                tbTimeMult.Focus();
                return false;
            }
            //tbHbLong
            if (tbHbLong.Text.Length == 0)
            {
                MessageBox.Show("Se requiere una cantidad.");
                tbHbLong.Focus();
                return false;
            }
            return true;
        }

        private void DoUpdateConfigSystem()
        {
            // pasar valores de controles a clase
            _editConfig.UdpServerPort = int.Parse(this.tbUdpPort.Text);
            _editConfig.ZyanServerName = this.tbZyanComponent.Text;
            _editConfig.ZyanServerPort = int.Parse(this.tbTcpPort.Text);
            _editConfig.TimeoutFactor = int.Parse(this.tbTimeMult.Text);
            _editConfig.HeartBeatHistory = int.Parse(this.tbHbLong.Text);

            _editConfig.SMtpServer = this.tbSmtpServer.Text;
            _editConfig.Source = this.tbSender.Text;
            _editConfig.Password = this.tbPassword.Text;
            _editConfig.Destination = this.tbRecipients.Text;
            _editConfig.Subject = this.tbSubject.Text;
            _editConfig.Body = this.tbBody.Text;


        }

        #region Envio de notificaciones --> Out

        private void OnSendSystemConfig(SystemConfigEventArgs e)
        {
            if (SendSystemConfig != null)
                SendSystemConfig(this, e);
        }

        private void OnRequestSystemConfig()
        {
            if (RequestSystemConfig != null)
                RequestSystemConfig();
        }

        #endregion

        #region Manejo de notificaciones <-- In

        internal void ReceiveSystemConfig(object sender, SystemConfigEventArgs e)
        {
            SystemData = e.Data;
        }

        internal void DoSetEditSystemMode()
        {
            this.Text = "Configuración del sistema";
            this.btOk.Click += CheckAndUpdateSystem;

            this.cmbClientes.Visible = false;
            this.btEliminar.Visible = false;
            this.btApply.Visible = false;
            this.tabControl1.Controls.Remove(this.tbpClient);
            this.tabControl1.Controls.Remove(this.tbpCltStatus);

            OnRequestSystemConfig();
        }

        #endregion

      
        #endregion


        #region Validacion de entrada numerica

        private void tbNumeric_KeyDown(object sender, KeyEventArgs e)
        {
            _nonNumberEntered = false;

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
                        _nonNumberEntered = true;
                    }
                }
            }

        }

        private void tbNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_nonNumberEntered == true)
            {
                // Impedir entrada del caracter no numerico, determinado en tbNumeric_KeyDown
                e.Handled = true;
            }

        }

        #endregion



    }
}
