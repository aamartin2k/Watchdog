using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.Shared;


namespace ConMonitor
{
    public partial class EditSysConfig : Form
    {
        private bool nonNumberEntered = false;
        private SystemConfigData _editConfig;

        internal event UniversalHandler RequestSystemConfig;
        internal event UniversalHandler CancelConfig;
        internal event UniversalHandler AcceptEdit;
        internal event SystemConfigEventHandler SendSystemConfig;

        public EditSysConfig()
        {
            InitializeComponent();
        }

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
            OnRequestSystemConfig();
        }

        #endregion 

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

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (CancelConfig != null)
                CancelConfig();
        }


    }
}
