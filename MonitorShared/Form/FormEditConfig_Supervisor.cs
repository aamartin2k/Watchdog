#region Descripción
/*
    Implementa la interfase compartida por el servicio monitor y el cliente supervisor.
    Parcial Modo Supervisor.
*/
#endregion

#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
#endregion


namespace Monitor.Shared
{
    public partial class FormEditConfig : Form
    {
        #region Declaraciones
        private bool _connected;
        #endregion


        #region Propiedades

        #region Propiedades Públicas

        #region Salida de Solicitudes al servidor
        public Action<RemReqClientData> Out_GetClientConfig { get; set; }
        public Action<RemReqSystemData> Out_GetSystemConfig { get; set; }
        public Action<RemReqQueueData> Out_GetQueueInfo { get; set; }
        public Action<RemReqLogFile> Out_GetLogFile { get; set; }
        public Action<RemReqConsoleText> Out_GetConsoleText { get; set; }
        public Action<RemReqCreateClient> Out_SendCreateClient { get; set; }
        public Action<RemReqUpdateClient> Out_SendClientUpdate { get; set; }
        public Action<RemReqDeleteClient> Out_SendClientDelete { get; set; }
        public Action<RemReqPauseClient> Out_SendClientPause { get; set; }
        public Action<RemReqResumeClient> Out_SendClientResume { get; set; }
        public Action<RemReqUpdateSystem> Out_SendSystemUpdate { get; set; }
       
        
        #endregion
        #endregion

        #region Propiedades Privadas
        private string ServerUrl
        {
            set
            {
                _serverUrl = value;
                cmbUrl.Text = _serverUrl;
                //if (!cmbUrl.Items.Contains(_serverUrl))
                //    cmbUrl.Items.Add(_serverUrl);
            }
            get
            {
                return _serverUrl;
            }
        }

        private bool Connected
        {
            set
            {
               Color color = value ? Color.Green : Color.Red;
                pnlConnect.BackColor = color;

                _connected = value;

                SetUpdateButtons(value);
            }
            get
            {
                return _connected;
            }
        }

        #endregion

        #endregion


        #region Métodos

        #region Métodos Públicos
        #region Entrada de Respuestas del servidor
        public void In_ClientDataList(RemReplyClientData cldat)
        {
            ClientList = cldat.Data;
        }

        public void In_SystemConfigData(RemReplySystemData cldat)
        {
            SystemData = cldat.Data;
        }

        public void In_QueueInfo(RemReplyQueueInfo rpq)
        {
            QueueInfo data = rpq.Data;

            lbInitial.Items.Clear();
            lbInitial.Items.AddRange(data.InitialList.ToArray());

            lbWork.Items.Clear();
            lbWork.Items.AddRange(data.WorkList.ToArray());

            lbInPause.Items.Clear();
            lbInPause.Items.AddRange(data.PausedList.ToArray());

            lbRecover.Items.Clear();
            lbRecover.Items.AddRange(data.RecoverList.ToArray());

            lbDead.Items.Clear();
            lbDead.Items.AddRange(data.DeadList.ToArray());

        }

        public void In_LogFile(RemReplyLogFile req)
        {
            rtbLog.Clear();
            rtbLog.LoadFile(req.File, RichTextBoxStreamType.PlainText);
        }

        public void In_Error(RemReplyConsoleText rpl)
        {
            MessageBox.Show("Se ha recibido un mensaje de error del servicio.");

            In_ConsoleText(rpl);

            tabControl1.SelectedTab = tbpConsole;
        }

        public void In_ConsoleText(RemReplyConsoleText req)
        {
            foreach (var item in req.List)
            {
                AddConsoleText(item);
            }
        }

        private void AddConsoleText(RemoteConsoleText msg)
        {
            //Color ccolor;
            //ccolor = Functions.GetTextColor(msg.Type);
            rtbConsole.SelectionColor = Functions.GetTextColor(msg.Type);
            rtbConsole.AppendText(msg.Text + "\n");
            rtbConsole.ScrollToCaret();
        }

        #endregion
        #endregion

        #region Métodos Privados

        //Request Handler

        /// <summary>
        /// Recibe información sobre la conexión solicitada.
        /// </summary>
        /// <param name="req">Datos de la conexión.</param>
        public void ReceiveConnection(FormConnected req)
        {

            Connected = req.Connected;

            if (req.Connected)
            {
                // solicitar texto de consola
                Out_GetConsoleText(new RemReqConsoleText());

                ServerUrl = req.ServerUrl;

                // Inicia tmrConnect con intervalo corto. Se solicitará envio de datos inmediatamente 
                // despues de conectarse.
                tmrConnect.Interval = 300;
                tmrConnect.Start();

                cmbUrl.Text = req.ServerUrl;
                if (req.ServerUrlList != null)
                {
                    cmbUrl.Items.Clear();
                    cmbUrl.Items.AddRange(req.ServerUrlList);
                }
            }
            else
            {
                tmrConnect.Stop();
            }

            btRefreshAll.Enabled = req.Connected;

        }


        /// <summary>
        /// Establece la configuración del formulario para funcionar en modo supervisor.
        /// </summary>
        private void SetSupervisorMode()
        {
            SetNormalMode();

            btCancel.Visible = false;
            lbWizard.Visible = false;

            tabControl1.Controls.Remove(tbpService);

            btApplyEdit.Click += UpdateRemote;
            btCancelEdit.Click += CancelUpdateRemote;
            rtbLog.DoubleClick += RequestViewLog_Click;
            btNewClient.Click += StartCreateClientRemote;
            btDeleteClient.Click += DeleteClientRemote;
            btOk.Click += Cerrar_Supervisor;
            cmnuPauseClient.Opening += CmnuPauseClient_Opening;
            cmnuResumeClient.Opening += CmnuResumeClient_Opening;
            tsmnuPauseClient.Click += TsmnuPauseClient_Click;
            tsmnuResumeClient.Click += TsmnuResumeClient_Click;
            btOk.Text = "Terminar";
            Text = "Supervisor del servicio Monitor";

            SetUpdateHandlersRemote();

            // Crear Tooltips
            toolTip1.SetToolTip(pnlConnect, "Indicador de conexión");
            toolTip1.SetToolTip(cmbUrl, "Barra de dirección");
            toolTip1.SetToolTip(btRefreshAll, "Refrescar todos los datos");
            toolTip1.SetToolTip(btRefreshData, "Refrescar datos");
            toolTip1.SetToolTip(lbClients, "Lista de clientes");

            btRefreshAll.Click += BtRefreshAll_Click;
            btRefreshData.Click += BtRefresh_Click;
        }

        

        /// <summary>
        /// Configura el form para su uso normal como aplicación.
        /// </summary>
        private void SetNormalMode()
        {
            ControlBox = true;
            ShowIcon = true;
            ShowInTaskbar = true;
            FormBorderStyle = FormBorderStyle.Sizable;

            // moviendo pictureBox del perrito...
            this.gbServer.Controls.Add(this.pictureBox1);
            this.pictureBox1.Location = new Point(lbTimer.Left + 40, pictureBox1.Top);

        }

        private void CheckAndExit(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Implementa la solicitud de realizar conexion al servicio.
        /// </summary>
        private void RequestConnect()
        {
            // Si la conexion es a una url diferente, proceder
            string url = string.Empty;

            if (ServerUrl != cmbUrl.Text)
            {
                url = cmbUrl.Text;

                lbTimer.Text = "Conectando...";
                OnRequestFormConnection(url);
            }
            else
            {
                url = ServerUrl;

                if (Connected)
                {
                    lbTimer.Text = "Solicitando datos...";
                    RequestAllData();
                }
                else
                {
                    lbTimer.Text = "Conectando...";
                    OnRequestFormConnection(url);
                }
            }

            // Activar timer para borrar texto descriptivo en lbTimer al transcurrir el intervalo (1.6 segs aprox.).
            tmrStatus.Start();
        }
        /// <summary>
        /// Envia mensaje solicitando conexion al servicio.
        /// </summary>
        /// <param name="url">Direccion del servidor remoto a conectar.</param>
        private void OnRequestFormConnection(string url)
        {
            MessageBus.Send(new RequestConnect(url));
        }

        /// <summary>
        /// Envia mensaje solicitando datos de configuracion al servicio.
        /// </summary>
        private void RequestAllData()
        {
            lbTimer.Text = "Solicitando todos los datos...";

            try
            {

                Out_GetClientConfig(new RemReqClientData());

                Out_GetSystemConfig(new RemReqSystemData());

                Out_GetQueueInfo(new RemReqQueueData());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
            finally
            {
                tmrStatus.Start();
            }
        }

        private void RequestPartData()
        {
            lbTimer.Text = "Solicitando datos...";

            try
            {

                if (tabControl1.SelectedTab == tbpServer || tabControl1.SelectedTab == tbpEmail)
                    Out_GetSystemConfig(new RemReqSystemData());

                if (tabControl1.SelectedTab == tbpClient || tabControl1.SelectedTab == tbpCltStatus)
                    Out_GetClientConfig(new RemReqClientData());

                if (tabControl1.SelectedTab == tbpQueues)
                    Out_GetQueueInfo(new RemReqQueueData());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
            finally
            {
                tmrStatus.Start();
            }
        }

        private void SetUpdateButtons(bool status)
        {
            btApplyEdit.Enabled = status;
            btCancelEdit.Enabled = status;
            btNewClient.Enabled = status;
            btDeleteClient.Enabled = status;
        }

        #region Controladores de Eventos

        private void TsmnuResumeClient_Click(object sender, EventArgs e)
        {
            string name = lbInPause.SelectedItem as string;

            if (_clientList.ContainsName(name))
            {
                Guid key = _clientList.GetClientIdByName(name);
                Out_SendClientResume(new RemReqResumeClient(key));
            }
        }

        private void TsmnuPauseClient_Click(object sender, EventArgs e)
        {
            string name = lbWork.SelectedItem as string;

            if (_clientList.ContainsName(name))
            {
                Guid key = _clientList.GetClientIdByName(name);
                Out_SendClientPause(new RemReqPauseClient(key));
            }

        }

        private void CmnuPauseClient_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lbWork.SelectedIndex < 0)
                e.Cancel = true; 
        }

        private void CmnuResumeClient_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lbInPause.SelectedIndex < 0)
                e.Cancel = true;
        }

        private void BtRefreshAll_Click(object sender, EventArgs e)
        {
            RequestAllData();
        }

        private void BtRefresh_Click(object sender, EventArgs e)
        {
            RequestPartData();

           

        }


        private void CancelUpdateRemote(object sender, EventArgs e)
        {
            // restableciendo controles con valores de System
            SystemData = _editSystemConfig;
            // Resyableciendo controles con valores del Selected  Client
            CancelUpdateInteractive(sender, e);

        }

        private void SetUpdateHandlersRemote()
        {
            SetUpdateHandlers();

            tbIpAdr.KeyUp += AllControls_Change;
            tbUdpPort.KeyUp += AllControls_Change;
            tbTcpPort.KeyUp += AllControls_Change;
            tbZyanComponent.KeyUp += AllControls_Change;
            tbTimeoutMult.KeyUp += AllControls_Change;
            tbRestAtp.KeyUp += AllControls_Change;
            tbSmtpServer.KeyUp += AllControls_Change;
            tbSender.KeyUp += AllControls_Change;
            tbPassword.KeyUp += AllControls_Change;
            tbRecipients.KeyUp += AllControls_Change;
        }

        private void UpdateRemote(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tbpClient)
            { 
                // Client part
                if (DoChecksBeforeUpdate())
                    DoUpdateClientRemote();
                
            }

            if (tabControl1.SelectedTab == tbpServer || tabControl1.SelectedTab == tbpEmail)
            {
                //   system part
                if (DoChecksSystem())
                {
                    DoUpdateSystemRemote();
                    //OnSaveData();
                    //OnAcceptEdit();
                   
                }
            }
        }

        private void DoUpdateSystemRemote()
        {
            SystemConfigData data = new SystemConfigData();

            // pasar valores de controles a clase
            data.ServerIpAdr = tbIpAdr.Text;
            data.UdpServerPort = int.Parse(tbUdpPort.Text);
            data.ZyanServerName = tbZyanComponent.Text;
            data.ZyanServerPort = int.Parse(tbTcpPort.Text);
            data.TimeoutStartRestart = int.Parse(tbTimeoutMult.Text);
            data.RestartAttemps = int.Parse(tbRestAtp.Text);

            data.SMtpServer = tbSmtpServer.Text;
            data.Source = tbSender.Text;
            data.Password = tbPassword.Text;
            data.Destination = tbRecipients.Text;

            // enviar
            RemReqUpdateSystem msg = new RemReqUpdateSystem(data);
            Out_SendSystemUpdate(msg);
        }


        private void DoUpdateClientRemote()
        {
            // crear clase de update
            ClientRemoteUpdate cltUpd = new ClientRemoteUpdate();

            cltUpd.ClientId = _currentClient.ClientId;
            cltUpd.Name = tbName.Text;
            cltUpd.AppFilePath = tbAppPath.Text;
            cltUpd.LogFilePath = tbLogPath.Text;
            cltUpd.Timeout = int.Parse(tbTimeout.Text);
            cltUpd.MailEnabled = chkEmail.Checked;
            cltUpd.LogAttachEnabled = chkAttachLog.Checked;
            cltUpd.QueueSize = int.Parse(tbQueueSize.Text);

            if (rbKeyId.Checked)
            {
                cltUpd.IdType = ClientIdType.KeyByIdString;
                cltUpd.Id = tbKeyID.Text;
            }
            else
            {
                cltUpd.IdType = ClientIdType.KeyByUdpPort;
                cltUpd.Id = null;
            }

            if (tbKeyPort.Text.Length == 0)
                cltUpd.Port = 0;
            else
                cltUpd.Port = int.Parse(tbKeyPort.Text);

            // enviar
            RemReqUpdateClient msg = new RemReqUpdateClient(cltUpd);
            Out_SendClientUpdate(msg);
        }

        private int _selIndx;
        private void StartCreateClientRemote(object sender, EventArgs e)
        {
            // guardar indice select
            _selIndx = lbClients.SelectedIndex;

            // limpiar controles
            tbKeyID.Text = null;
            tbName.Text = null;
            tbAppPath.Text = null;
            tbLogPath.Text = null;
            tbTimeout.Text = null;
            chkEmail.Checked = false;
            chkAttachLog.Checked = false;
            tbQueueSize.Text = null;
            rbKeyId.Checked = false;
            rbKeyPort.Checked = false;
            tbKeyPort.Text = null;

            // modificar event handlers
            btApplyEdit.Click -= UpdateRemote;
            btCancelEdit.Click -= CancelUpdateRemote;

            btApplyEdit.Click += EndCreateClientRemote;
            btCancelEdit.Click += CancelCreateClientRemote;
        }

        private void EndCreateClientRemote(object sender, EventArgs e)
        {
            // crear clase de update
            ClientRemoteUpdate cltUpd = new ClientRemoteUpdate();

            cltUpd.Name = tbName.Text;
            cltUpd.AppFilePath = tbAppPath.Text;
            cltUpd.LogFilePath = tbLogPath.Text;
            cltUpd.Timeout = int.Parse(tbTimeout.Text);
            cltUpd.MailEnabled = chkEmail.Checked;
            cltUpd.LogAttachEnabled = chkAttachLog.Checked;
            cltUpd.QueueSize = int.Parse(tbQueueSize.Text);

            if (rbKeyId.Checked)
            {
                cltUpd.IdType = ClientIdType.KeyByIdString;
                cltUpd.Id = tbKeyID.Text;
            }
            else
            {
                cltUpd.IdType = ClientIdType.KeyByUdpPort;
                cltUpd.Id = null;
            }

            if (tbKeyPort.Text.Length == 0)
                cltUpd.Port = 0;
            else
                cltUpd.Port = int.Parse(tbKeyPort.Text);

            // enviar
            RemReqCreateClient msg = new RemReqCreateClient(cltUpd);
            Out_SendCreateClient(msg);

            // restablecer event handlers
            btApplyEdit.Click -= EndCreateClientRemote;
            btCancelEdit.Click -= CancelCreateClientRemote;

            btApplyEdit.Click += UpdateRemote;
            btCancelEdit.Click += CancelUpdateRemote;

            // restablecer datos de cliente seleccionado
            lbClients.SelectedIndex = _selIndx;
        }

        private void CancelCreateClientRemote(object sender, EventArgs e)
        {
            // restablecer event handlers
            btApplyEdit.Click -= EndCreateClientRemote;
            btCancelEdit.Click -= CancelCreateClientRemote;

            btApplyEdit.Click += UpdateRemote;
            btCancelEdit.Click += CancelUpdateRemote;

            // restablecer datos de cliente seleccionado
            lbClients.SelectedIndex = _selIndx;
        }

        private void DeleteClientRemote(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show("Se eliminará el cliente. Esta acción es irreversible.\n Desea continuar?",
                                                "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.Yes)
            {
                ClientData clt = lbClients.SelectedItem as ClientData;

                Out_SendClientDelete(new RemReqDeleteClient(clt.ClientId));
            }
        }

      

        private void Cerrar_Supervisor(object sender, EventArgs e)
        {
            Close();
        }
        private void cmbUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RequestConnect();
            }
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            RequestAllData();
        }

        private void tmrConnect_Tick(object sender, EventArgs e)
        {
            // Modifica el intervalo, para solicitar datos en lapsos de un minuto aprox.
            // despues de realizar la solicitud inicial posterior a la conexión.
            if (tmrConnect.Interval == 300)
            {
                tmrConnect.Interval = 60 * 1000;
            }

            RequestAllData();
        }
        
        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            lbTimer.Text = string.Empty;
            tmrStatus.Stop();
        }

        private void RequestViewLog_Click(object sender, EventArgs e)
        {
            Out_GetLogFile(new RemReqLogFile(_currentClient.LogFilePath));
        }

        #endregion

        #endregion

        #endregion


        
    }
}
