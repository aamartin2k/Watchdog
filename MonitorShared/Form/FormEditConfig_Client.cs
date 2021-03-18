#region Descripción
/*
    Implementa la interfase compartida por el servicio monitor y el cliente supervisor.
    Parcial Modo Configuración de Clientes.
*/
#endregion

#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
#endregion


namespace Monitor.Shared
{
    public partial class FormEditConfig : Form
    {
        #region Declaraciones
        private ClientData _currentClient;
        private ClientDataManager _clientList;
        #endregion


        #region Propiedades

        #region Propiedades Públicas

        #endregion

        #region Propiedades Privadas

        private bool CambiosPendientes
        {
            get { return _pending; }

            set
            {
                _pending = value;
                 
                btApplyEdit.Enabled = value;
                lbClients.Enabled = !value;
                btOk.Enabled = !value;
               
            }
        }

        private void TabControl1_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            // Si se han modificado los controles, impedir cambiar de tab
            // hast que se apliquen los cambios
            if (_pending)
                e.Cancel = true;
        }

        // Recibe referencia a un client para actualizar controles 
        private ClientData ClientData
        {
            get { return _currentClient; }
            set
            {
                _currentClient = value;

               
                // pasar valores a controles form
                tbName.Text = _currentClient.Name;
                tbAppPath.Text = _currentClient.AppFilePath;
                tbLogPath.Text = _currentClient.LogFilePath;
                
                tbTimeout.Text = _currentClient.Timeout.ToString();
                chkEmail.Checked = _currentClient.MailEnabled;
                chkAttachLog.Checked = _currentClient.LogAttachEnabled;
                tbQueueSize.Text = _currentClient.QueueSize.ToString();

                if (_currentClient.TransportType == TransportType.TransportUdp)
                {
                    rbTrnUdp.Checked = true;
                    tbKeyPort.Text = _currentClient.Port.ToString();
                }
                else
                {
                    rbTrnPipe.Checked = true;
                    tbKeyPipe.Text = _currentClient.Pipe;
                }

                
                switch (_currentClient.IdType)
                {
                    case ClientIdType.KeyByUdpPort:
                        rbKeyPort.Checked = true;
                        break;

                    case ClientIdType.KeyByIdString:
                        rbKeyId.Checked = true;
                        tbKeyID.Text = _currentClient.Id;
                        break;

                    case ClientIdType.KeyByPipe:
                        rbKeyPipe.Checked = true;
                        break;
                    default:
                        break;
                }

                chkRestart.Enabled = _currentClient.RestartEnabled;

                CambiosPendientes = false;

                btStatus.BackColor = _currentClient.Color;

                lbRestCount.Text = _currentClient.RestartCount.ToString();
                lbIni.Text = _currentClient.StartTime.ToString();
                lbUptime.Text = _currentClient.Uptime;


                try
                {
                    lbxHB.SuspendLayout();
                    lbxHB.Items.Clear();
                    string[] data = _currentClient.HeartBeatList;

                    for (int i = data.Length - 1; i > 0; i--)
                    {
                        lbxHB.Items.Add(data[i]);
                    }

                    lbxHB.PerformLayout();
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }


        private ClientDataManager ClientList
        {
            get { return _clientList; }
            set
            {
                _clientList = value;

                int actSel = lbClients.SelectedIndex;
                int idxSel = (actSel > 0) ? actSel : 0  ;

                lbClients.SuspendLayout();
                lbClients.Items.Clear();

                // Llenar lista de nombre           
                
                foreach (var item in _clientList.List)
                {
                    lbClients.Items.Add(item);
                }
                
                lbClients.PerformLayout();
                // chequear valor menor cuando se elimina un cliente
                if (idxSel == lbClients.Items.Count)
                    idxSel--;

                lbClients.SelectedIndex = idxSel;
            }
        }
        #endregion

        #endregion


        #region Métodos

        #region Métodos Públicos

        #region Métodos Request handlers
        public void ReceiveClientConfig(SendClientConfig req)
        {
            ClientList = req.Data;
        }
        #endregion

        #endregion

        #region Métodos Privados

        #region Manejo de datos y controles

        private void SetCreateClientMode()
        {

            SetSimpleMode();

            Text = "Registrar nuevo cliente";
            btOk.Click += CheckAndCreateClient;

            _ignoreNameCheck = false;
           
            // asi se oculta la lista y los botones
            splitContainer1.Panel1Collapsed = true;
            lbWizard.Visible = false;
            btDeleteClient.Visible = false;
            btNewClient.Visible = false;

            btApplyEdit.Visible = false;
            btCancelEdit.Visible = false;

            // reducir size form
            Width = Width - 135;

            // Moviendo botones
            btOk.Left = btOk.Left - 135;
            btCancel.Left = btCancel.Left - 135;
            // Reduciendo Tab control

            tabControl1.Controls.Remove(tbpQueues);
            tabControl1.Controls.Remove(tbpCltStatus);
            tabControl1.Controls.Remove(tbpServer);
            tabControl1.Controls.Remove(tbpEmail);
            tabControl1.Controls.Remove(tbpLogFile);
            tabControl1.Controls.Remove(tbpConsole);
            tabControl1.Controls.Remove(tbpService);
        }

        private void SetEditClientMode()
        {
            SetSimpleMode();

            Text = "Editar datos de cliente";
            btOk.Click += SaveAcceptClient;
            btApplyEdit.Click += ApplyUpdateInteractive;
            btCancelEdit.Click += CancelUpdateInteractive;

            SetUpdateHandlers();

            _ignoreNameCheck = true;

            lbWizard.Visible = false;
            btDeleteClient.Visible = false;
            btNewClient.Visible = false;

            tabControl1.Controls.Remove(tbpServer);
            tabControl1.Controls.Remove(tbpEmail);
            tabControl1.Controls.Remove(tbpQueues);
            tabControl1.Controls.Remove(tbpLogFile);
            tabControl1.Controls.Remove(tbpConsole);
            tabControl1.Controls.Remove(tbpService);

        }

        private void SetDeleteClientMode()
        {
            SetSimpleMode();

            Text = "Eliminar cliente";
            btOk.Click += SaveAcceptClient;
            btDeleteClient.Click += DeleteClient;

            SetUpdateHandlers();

            lbWizard.Visible = false;
            btApplyEdit.Visible = false;
            btNewClient.Visible = false;
            btCancelEdit.Visible = false;

            tabControl1.Controls.Remove(tbpQueues);
            tabControl1.Controls.Remove(tbpServer);
            tabControl1.Controls.Remove(tbpEmail);
            tabControl1.Controls.Remove(tbpLogFile);
            tabControl1.Controls.Remove(tbpConsole);
            tabControl1.Controls.Remove(tbpService);

            tbAppPath.ReadOnly = true;
            tbLogPath.ReadOnly = true;
            tbKeyPort.ReadOnly = true;
            tbQueueSize.ReadOnly = true;
            tbTimeout.ReadOnly = true;
            chkEmail.Enabled = false;
            chkAttachLog.Enabled = false;
            btLogPath.Enabled = false;
            btAppPath.Enabled = false;
            tbName.ReadOnly = true;
            rbKeyId.Enabled = false;
            rbKeyPort.Enabled = false;

        }

        private void DoUpdateClient()
        {

            //type, Id,  Port
            if (rbKeyId.Checked)
                _currentClient.IdType = ClientIdType.KeyByIdString;
            else
                _currentClient.IdType = ClientIdType.KeyByUdpPort;

            if (tbKeyID.Text.Length == 0)
                _currentClient.Id = null;
            else
                _currentClient.Id = tbKeyID.Text;

            if (tbKeyPort.Text.Length == 0)
                _currentClient.Port = 0;
            else
                _currentClient.Port = int.Parse(tbKeyPort.Text);

            // pasar controles a props de clase config
            _currentClient.Name = tbName.Text;
            _currentClient.AppFilePath = tbAppPath.Text;
            _currentClient.LogFilePath = tbLogPath.Text;

            _currentClient.Timeout = int.Parse(tbTimeout.Text);
            _currentClient.MailEnabled = chkEmail.Checked;
            _currentClient.LogAttachEnabled = chkAttachLog.Checked;
            _currentClient.QueueSize = int.Parse(tbQueueSize.Text);

            _clientList.UpdateKeys();

            CambiosPendientes = false;
        }

        private void DoCreateClient()
        {
            ClientIdType type;
            if (rbKeyId.Checked)
                type = ClientIdType.KeyByIdString;

            if (rbKeyPort.Checked)
                type = ClientIdType.KeyByUdpPort;

            type = ClientIdType.KeyByPipe;


            TransportType trnType;
            if (rbTrnUdp.Checked)
                trnType = TransportType.TransportUdp;
            else
                trnType = TransportType.TransportPipe;

            int port;
            if (tbKeyPort.Text.Length == 0)
                port = 0;
            else
                port = int.Parse(tbKeyPort.Text);

            // Creando cliente con metodo factory
            ClientList.CreateClient(type, trnType, 
                                    tbKeyID.Text,
                                    port, 
                                    tbName.Text, tbKeyPipe.Text,
                                    tbAppPath.Text, tbLogPath.Text,
                                    int.Parse(tbTimeout.Text),
                                    chkEmail.Checked, chkAttachLog.Checked,
                                    chkRestart.Enabled,
                                    int.Parse(tbQueueSize.Text)  );



        }

        private void DeleteClient(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show("Se eliminará el cliente. Esta acción es irreversible, aún empleando Cancelar.\n Desea continuar?",
                                                 "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.Yes)
            {
                ClientData clt = lbClients.SelectedItem as ClientData;

                ClientList.Delete(clt.ClientGuId);
                ClientList = _clientList;
            }
        }


        /// <summary>
        /// Chequeo de datos para creacion de nuevo cliente.
        /// </summary>
        private bool DoChecksBeforeCreate()
        {
            if (tbName.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un nombre.");
                tbName.Focus();
                return false;
            }

            if (!_ignoreNameCheck && ClientList.ContainsName(tbName.Text))
            {
                MessageBox.Show("El nombre ya está registrado. Se requiere un nombre diferente.");
                tbName.Focus();
                return false;
            }

            // Chequear texto Id si esta seleccionado como clave
            if (rbKeyId.Checked)
            {
                if (tbKeyID.Text.Length == 0)
                {
                    MessageBox.Show("Se requiere un ID.");
                    tbKeyID.Focus();
                    return false;
                }

                bool flag = ClientList.ContainsId(tbKeyID.Text);
                if (flag)
                {
                    MessageBox.Show("El ID ya está registrado. Se requiere un ID diferente.");
                    tbKeyID.Focus();
                    return false;
                }
            }

            // Chequear texto Puerto si esta seleccionado como transporte
            if (rbTrnUdp.Checked)  
            {
                if (tbKeyPort.Text.Length == 0)
                {
                    MessageBox.Show("Se requiere un puerto.");
                    tbKeyPort.Focus();
                    return false;
                }

                bool flag = ClientList.ContainsPort(int.Parse(tbKeyPort.Text));
                if (flag)
                {
                    MessageBox.Show("El puerto ya está registrado. Se requiere un puerto diferente.");
                    tbKeyPort.Focus();
                    return false;
                }
            }
            // Chequear texto Pipe si esta seleccionado como transporte
            if (rbTrnPipe.Checked)
            {
                if (tbKeyPipe.Text.Length == 0)
                {
                    MessageBox.Show("Se requiere un nombre de Pipe.");
                    tbKeyPipe.Focus();
                    return false;
                }

                bool flag = ClientList.ContainsPipe(tbKeyPipe.Text);
                if (flag)
                {
                    MessageBox.Show("El Pipe ya está registrado. Se requiere un Pipe diferente.");
                    tbKeyPipe.Focus();
                    return false;
                }
            }

            // Chequear texto Ruta a la aplicacion
            if ((tbAppPath.Text.Length == 0) || (!System.IO.File.Exists(tbAppPath.Text)))
            {
                MessageBox.Show("La ruta al archivo de aplicación no es válida.");
                tbAppPath.Focus();
                return false;
            }
            // Chequear texto Ruta al log
            if ((tbLogPath.Text.Length == 0) || !System.IO.File.Exists(tbLogPath.Text))
            {
                MessageBox.Show("La ruta al archivo de log no es válida.");
                tbLogPath.Focus();
                return false;
            }

            if (tbTimeout.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un valor de timeout.");
                tbTimeout.Focus();
                return false;
            }

            if (tbQueueSize.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un valor de longitud.");
                tbQueueSize.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Chequeo de datos para edicion de cliente existente.
        /// </summary>
        private bool DoChecksBeforeUpdate()
        {
            bool flag;

            // Chequear Nombre NameInUseByOthers
            flag = ClientList.NameInUseByOthers(tbName.Text, _currentClient.Name);
            if (flag)
            {
                MessageBox.Show("El nombre ya está registrado. Se requiere un nombre diferente.");
                tbName.Focus();
                return false;
            }

            // Chequear texto Id si esta seleccionado rbutton ID
            if (rbKeyId.Checked)
            {
                if (tbKeyID.Text.Length == 0)
                {
                    MessageBox.Show("Se requiere un ID.");
                    tbKeyID.Focus();
                    return false;
                }

                flag = ClientList.IdInUseByOthers(tbKeyID.Text, _currentClient.Name);
                if (flag)
                {
                    MessageBox.Show("El ID ya está registrado. Se requiere un ID diferente.");
                    tbKeyID.Focus();
                    return false;
                }
            }
            else  // Chequear puerto
            {
                if (tbKeyPort.Text.Length == 0)
                {
                    MessageBox.Show("Se requiere un puerto.");
                    tbKeyPort.Focus();
                    return false;
                }

                flag = ClientList.PortInUseByOthers(int.Parse(tbKeyPort.Text), _currentClient.Name);
                if (flag)
                {
                    MessageBox.Show("El puerto ya está registrado. Se requiere un puerto diferente.");
                    tbKeyPort.Focus();
                    return false;
                }

            }

            if (tbTimeout.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un valor de timeout.");
                tbTimeout.Focus();
                return false;
            }

            if (tbQueueSize.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un valor de longitud.");
                tbQueueSize.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Chequeo de datos y creacion de nuevo cliente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckAndCreateClient(object sender, EventArgs e)
        {
            if (DoChecksBeforeCreate())
            {
                DoCreateClient();
                OnSaveData();
                OnAcceptEdit();
            }

        }

        private void SaveAcceptClient(object sender, EventArgs e)
        {
            OnSaveData();
            OnAcceptEdit();
        }


        #endregion

        #region Controladores de Eventos


        //Establecer handlers para seguimiento de cambio de campos
        // Se usa en modo Update interactivo y remoto
        private void SetUpdateHandlers()
        {
            tbName.KeyUp += AllControls_Change;
            tbKeyID.KeyUp += AllControls_Change;
            tbAppPath.KeyUp += AllControls_Change;
            tbLogPath.KeyUp += AllControls_Change;
            tbTimeout.KeyUp += AllControls_Change;
            tbKeyPort.KeyUp += AllControls_Change;
            tbQueueSize.KeyUp += AllControls_Change;

            chkEmail.CheckedChanged += Control_Changed;
            chkAttachLog.CheckedChanged += Control_Changed;

            lbClients.SelectedIndexChanged += lbClients_SelectedIndexChanged;

            tabControl1.Deselecting += TabControl1_Deselecting;
        }

        private void AllControls_Change(object sender, KeyEventArgs e)
        {
            CambiosPendientes = true;
        }

        private void ApplyUpdateInteractive(object sender, EventArgs e)
        {
            if (DoChecksBeforeUpdate())
                DoUpdateClient();
        }

        private void CancelUpdateInteractive(object sender, EventArgs e)
        {
            ClientData = lbClients.SelectedItem as ClientData;
        }

        #endregion


        #endregion

        #endregion

    }
}
