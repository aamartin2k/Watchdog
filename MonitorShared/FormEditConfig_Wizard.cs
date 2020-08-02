#region Descripción
/*
    Implementa la interfase compartida por el servicio monitor y el cliente supervisor.
    Parcial Modo Asistente de Configuración.
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
        private void SetWizardMode()
        {
            SetSimpleMode();

            Text = "Asistente de Configuración";

            lbWizard.Visible = true;
            lbWizard.Text = "Configuración inicial del sistema.";
            btOk.Click += CheckAndSaveSystem_Wizard;

            splitContainer1.Panel1Collapsed = true;
            btApplyEdit.Visible = false;
            btCancelEdit.Visible = false;

            // reducir size form
            Width = Width - 135;

            // Moviendo botones
            btOk.Left = btOk.Left - 135;
            btCancel.Left = btCancel.Left - 135;
           
            tabControl1.Controls.Remove(tbpClient);
            tabControl1.Controls.Remove(tbpCltStatus);
            tabControl1.Controls.Remove(tbpQueues);
            tabControl1.Controls.Remove(tbpLogFile);
            tabControl1.Controls.Remove(tbpConsole);
            tabControl1.Controls.Remove(tbpService);

        }


        private void CheckAndSaveSystem_Wizard(object sender, EventArgs e)
        {
            if (DoChecksSystem())
            {
                DoUpdateConfigSystem();
                OnSaveData();

                SetWizardClient();
            }
        }

        // bool flag para habilitar controles
        private bool EnableControls;
        private void SetWizardClient()
        {
            lbWizard.Text = "Configuración inicial de clientes.";

            btOk.Click -= CheckAndSaveSystem_Wizard;
            btOk.Click += SetWizardService; 

            btDeleteClient.Visible = false;
            btNewClient.Click += CheckAndSaveClient_Wizard;

            tabControl1.Controls.Remove(tbpServer);
            tabControl1.Controls.Remove(tbpEmail);

            // Deshabilitando entrada
            tbKeyID.Enabled = false;
            tbName.Enabled = false;
            tbAppPath.Enabled = false;
            tbLogPath.Enabled = false;
            btAppPath.Enabled = false;
            btLogPath.Enabled = false;
            tbTimeout.Enabled = false;
            chkEmail.Enabled = false;
            chkAttachLog.Enabled = false;
            tbQueueSize.Enabled = false;
            rbKeyId.Enabled = false;
            rbKeyPort.Enabled = false;
            tbKeyPort.Enabled = false;
            EnableControls = true;

            tabControl1.Controls.Add(tbpClient);

            
        }

        private void CheckAndSaveClient_Wizard(object sender, EventArgs e)
        {
            // Habilitando entrada
            if (EnableControls)
            {
                tbKeyID.Enabled = true;
                tbName.Enabled = true;
                tbAppPath.Enabled = true;
                tbLogPath.Enabled = true;
                btAppPath.Enabled = true;
                btLogPath.Enabled = true;
                tbTimeout.Enabled = true;
                chkEmail.Enabled = true;
                chkAttachLog.Enabled = true;
                tbQueueSize.Enabled = true;
                rbKeyId.Enabled = true;
                rbKeyPort.Enabled = true;
                tbKeyPort.Enabled = true;

                EnableControls = false;
            }
            else if (DoChecksBeforeCreate())
            {
                DoCreateClient();

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

            }

        }

        private void SetWizardService(object sender, EventArgs e)
        {
            lbWizard.Text = "Configuración del servicio.";
           
            OnSaveData();

            btOk.Click -= SetWizardService;
            btOk.Click += CheckInstallService;

            tabControl1.Controls.Add(tbpService);
            tabControl1.Controls.Remove(tbpClient);

        }

        private void CheckInstallService(object sender, EventArgs e)
        {
            if (chkInstallService.Checked)
                OnAcceptEdit();

            OnSaveData();
        }

    }
}
