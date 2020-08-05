#region Descripción
/*
    Implementa la interfase compartida por el servicio monitor y el cliente supervisor.
*/
#endregion

#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
#endregion


/// <summary>
/// Implementa la interfase compartida por el servicio monitor y el cliente supervisor.
/// </summary>
namespace Monitor.Shared
{
    public partial class FormEditConfig : Form
    {
        #region Declaraciones

        private bool _pending;
        private bool _nonNumberEntered = false;
        private bool _ignoreNameCheck;
        private string _serverUrl;
        private EditFormMode _mode;

        #endregion

        // Constructor 
       
        /// <summary>
        /// Inicializa el formulario de acuerdo al modo requerido.
        /// </summary>
        /// <param name="mode">Enumeración de los modos del formulario.</param>
        public FormEditConfig(EditFormMode mode)
        {
            InitializeComponent();

            _mode = mode;

            switch (mode)
            {
                case EditFormMode.CreateClient:
                    SetCreateClientMode();
                    break;
                case EditFormMode.EditClient:
                    SetEditClientMode();
                    break;
                case EditFormMode.DeleteClient:
                    SetDeleteClientMode();
                    break;
                case EditFormMode.EditSystem:
                    SetEditSystemMode();
                    break;
                case EditFormMode.Supervisor:
                    SetSupervisorMode();
                    break;
                case EditFormMode.Wizard:
                    SetWizardMode();
                    break;

                default:
                    break;
            }
        }

        #region Envio de notificaciones --> Out

        private void OnSaveData()
        {
            MessageBus.Send(new RequestSaveConfig()); 
        }

      
        private void OnCancelEdit()
        {
            MessageBus.Send(new FormEndEdit(false));
        }

        private void OnAcceptEdit()
        {
            MessageBus.Send(new FormEndEdit(true));
        }

        #endregion


        #region Manejo del form
        /// <summary>
        /// Configura el form para su uso como editor simple.
        /// Add, Delete, Edit configuracion clientes
        /// Edit configuracion sistema
        /// </summary>
        private void SetSimpleMode()
        {
            ControlBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            gbServer.Visible = false;
      
        }
       

       

        #endregion

        #region Controladores de eventos de controles


        private void rbKey_CheckedChanged(object sender, EventArgs e)
        {
            // obteniendo estado de rbKeyPort
            bool check = rbKeyPort.Checked;

            // activar 
            tbKeyPort.Enabled = check;
            // desativar
            tbKeyID.Enabled = !check;
        }

        private bool ValidChar(char test)
        {
            // Direccion IP valida contiene numeros y puntos
            if (test == '.')
                return true;

            if (char.IsDigit(test))
                return true;

            return false;
        }

        private bool TestString(string data)
        {
            foreach (char item in data)
            {
                if (!ValidChar(item))
                    return false;
            }

            return true;
        }

        // Genera una URL para uso del cliente al cambiar los datos de configuracion de
        // Direccion IP, Puerto TCP y Nombre del componente remoto.
        // "tcpex://dc003cfg:9090/WDMonitor"
        // "tcpex://--------:----/---------"
        //          host/ip   port  component
        // format base:
        // "tcpex://{0}:{1}/{2}"
        private void tbIpAdr_Leave(object sender, EventArgs e)
        {
            string hostip, port, component;

            hostip = tbIpAdr.Text.Trim(new Char[] { '\t', '\0', ' ' });

            // Si es una direccion IP se comprueba con Parse.
            // Si es un hostname se ignora, el frx lo resolvera mediante DNS.
            if (TestString(hostip))
            {
                System.Net.IPAddress tmpIp;
                if (!System.Net.IPAddress.TryParse(hostip, out tmpIp))
                {
                    // Ip no valida!
                    return;
                }
            }

            if (hostip == "127.0.0.1")
                hostip = "localhost";

            port = tbTcpPort.Text;
            int tmpInt;
            if (!Int32.TryParse(port, out tmpInt))
            {
                // valor int no valido!
                return;
            }

            component = tbZyanComponent.Text;

            tbUrl.Text = string.Format("tcpex://{0}:{1}/{2}", hostip, port, component);
        }

        private void FormEditConfig_Resize(object sender, EventArgs e)
        {
            if (_mode == EditFormMode.Supervisor)
            {
                if (Height < 400)
                    Height = 400;

                if (Width < 640)
                    Width = 640;
            }
        }


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
       
        private void btCancel_Click(object sender, EventArgs e)
        {
            OnCancelEdit();
        }

        private void Control_Changed(object sender, EventArgs e)
        {
            CambiosPendientes = true;
        }

        private void lbClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClientData = lbClients.SelectedItem as ClientData;
        }

       

        #endregion
       
        #region Validacion de entrada numérica en controles texto

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
