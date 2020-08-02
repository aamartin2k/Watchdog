#region Descripción
/*
    Implementa la interfase compartida por el servicio monitor y el cliente supervisor.
    Parcial Modo Configuración de Sistema.
*/
#endregion

#region Using
using System;
using System.Windows.Forms;
#endregion


namespace Monitor.Shared
{
    public partial class FormEditConfig : Form
    {
        #region Declaraciones
        private SystemConfigData _editSystemConfig;
        #endregion


        #region Propiedades

        #region Propiedades Públicas

        #endregion

        #region Propiedades Privadas

        private SystemConfigData SystemData
        {
            get { return _editSystemConfig; }
            set
            {
                _editSystemConfig = value;

                // pasar valores a controles form
                tbIpAdr.Text = _editSystemConfig.ServerIpAdr;
                tbUdpPort.Text = _editSystemConfig.UdpServerPort.ToString();
                tbZyanComponent.Text = _editSystemConfig.ZyanServerName;
                tbTcpPort.Text = _editSystemConfig.ZyanServerPort.ToString();
                tbTimeoutMult.Text = _editSystemConfig.TimeoutStartRestart.ToString();
                tbRestAtp.Text = _editSystemConfig.RestartAttemps.ToString();

                tbSmtpServer.Text = _editSystemConfig.SMtpServer;
                tbSender.Text = _editSystemConfig.Source;
                tbPassword.Text = _editSystemConfig.Password;
                tbRecipients.Text = _editSystemConfig.Destination;

                tbIpAdr_Leave(tbIpAdr, new EventArgs());

                CambiosPendientes = false;
            }

        }

        #endregion

        #endregion


        #region Métodos

        #region Métodos Públicos

        #region Métodos Request handlers
        // Recibe datos de configuración de sistema desde el MessageBus, esto ocurre cuando
        // el formulario es utilizado en modo interactivo en Monitor.Service.
        public void ReceiveSystemConfig(SendSystemConfig req)
        {
            SystemData = req.Data;
        }
        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Establece la configuración del formulario para funcionar en modo Configuración de Sistema.
        /// </summary>
        private void SetEditSystemMode()
        {
            SetSimpleMode();

            Text = "Configuración del sistema";
            btOk.Click += CheckAndUpdateSystem;

            splitContainer1.Panel1Collapsed = true;
            lbWizard.Visible = false;
            btApplyEdit.Visible = false;
            btCancelEdit.Visible = false;

            // reducir size form
            Width = Width - 135;
            
            // Moviendo botones
            btOk.Left = btOk.Left - 135;
            btCancel.Left = btCancel.Left - 135;
            //
            //tabControl1.Width = tabControl1.Width - 135;

            tabControl1.Controls.Remove(tbpClient);
            tabControl1.Controls.Remove(tbpCltStatus);
            tabControl1.Controls.Remove(tbpQueues);
            tabControl1.Controls.Remove(tbpLogFile);
            tabControl1.Controls.Remove(tbpConsole);
            tabControl1.Controls.Remove(tbpService);

        }

        private void CheckAndUpdateSystem(object sender, EventArgs e)
        {
            if (DoChecksSystem())
            {
                DoUpdateConfigSystem();
                OnSaveData();
                OnAcceptEdit();
            }
        }


        private bool DoChecksSystem()
        {
            TabPage actPage;

            if (tbIpAdr.Text.Length == 0)
            {
                MessageBox.Show("Se requiere una  dirección IP.");
                tbIpAdr.Focus();
                actPage = tbpServer;
                goto Return_False;
            }

            if (tbZyanComponent.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un nombre.");
                tbZyanComponent.Focus();
                actPage = tbpServer;
                goto Return_False;
            }

            if (tbUdpPort.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un puerto UDP.");
                tbUdpPort.Focus();
                actPage = tbpServer;
                goto Return_False;
            }
            //tbTcpPort
            if (tbTcpPort.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un puerto TCP.");
                tbTcpPort.Focus();
                actPage = tbpServer;
                goto Return_False;
            }
            if (tbTimeoutMult.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un valor de timeout.");
                tbTimeoutMult.Focus();
                actPage = tbpServer;
                goto Return_False; 
            }

            if (tbRestAtp.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un valor de limite.");
                tbRestAtp.Focus();
                actPage = tbpServer;
                goto Return_False;
            }
            
            if (tbSmtpServer.Text.Length == 0)
            {
                MessageBox.Show("Se requiere un nombre de servidor.");
                tbSmtpServer.Focus();
                actPage = tbpEmail;
                goto Return_False;
            }

            
            if (tbSender.Text.Length == 0)
            {
                MessageBox.Show("Se requiere una dirección de correo.");
                tbSender.Focus();
                actPage = tbpEmail;
                goto Return_False;
            }

            if (tbPassword.Text.Length == 0)
            {
                MessageBox.Show("Se requiere una contraseña.");
                tbPassword.Focus();
                actPage = tbpEmail;
                goto Return_False;
            }

            if (tbRecipients.Text.Length == 0)
            {
                MessageBox.Show("Se requiere una(s) dirección(es) de correo.");
                tbRecipients.Focus();
                actPage = tbpEmail;
                goto Return_False;
            }
            // Todo OK.
            return true;

            Return_False:
            // Hay problemas, seleccionar Tab y dar foco a control
            tabControl1.SelectedTab = actPage;
            return false;
        }

        private void DoUpdateConfigSystem()
        {
            // pasar valores de controles a clase
            _editSystemConfig.ServerIpAdr = tbIpAdr.Text;
            _editSystemConfig.UdpServerPort = int.Parse(tbUdpPort.Text);
            _editSystemConfig.ZyanServerName = tbZyanComponent.Text;
            _editSystemConfig.ZyanServerPort = int.Parse(tbTcpPort.Text);
            _editSystemConfig.TimeoutStartRestart = int.Parse(tbTimeoutMult.Text);
            _editSystemConfig.RestartAttemps = int.Parse(tbRestAtp.Text);

            _editSystemConfig.SMtpServer = tbSmtpServer.Text;
            _editSystemConfig.Source = tbSender.Text;
            _editSystemConfig.Password = tbPassword.Text;
            _editSystemConfig.Destination = tbRecipients.Text;

        }

       

        #endregion

        #endregion

    }
}
