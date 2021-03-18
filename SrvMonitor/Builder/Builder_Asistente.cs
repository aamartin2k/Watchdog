#region Descripción
/*
    Implementa el gestor del sistema.
    Parcial Asistente.
    Implementa el asistente de configuracion inicial.
*/
#endregion

#region Using
using Monitor.Shared;
using System;
using System.Threading;
using System.Windows.Forms;

#endregion

namespace Monitor.Service
{
    static partial class Builder
    {
        #region Modo Asistente

        static private void Wizard_System()
        {
            Builder.Output("Ejecutando asistente de configuración.");

            // configurando formulario
            EditFormMode efm = EditFormMode.Wizard;

            _editForm = new FormEditConfig(efm);
            _editForm.StartPosition = FormStartPosition.CenterScreen;
            // Config system
            MessageBus.Register<SendSystemConfig>(_editForm.ReceiveSystemConfig);
            MessageBus.Send(new SendSystemConfig(_dbHandler.SystemData));
            // Config client
            MessageBus.Register<SendClientConfig>(_editForm.ReceiveClientConfig);
            MessageBus.Send(new SendClientConfig(_dbHandler.ClientList));

            MessageBus.Register<FormEndEdit>(EndWizard);
            MessageBus.Register<RequestSaveConfig>(_dbHandler.Save);

            Application.Run(_editForm);

        }


        static private void EndWizard(FormEndEdit req)
        {
            if (req.AcceptChanges)
            {
                InstallService();
                Builder.Output("Ejecutando InstallService.");
            }


            _editForm.Close();
            _editForm = null;
            Builder.Output("Asistente de configuración terminado.");
        }

       

        #endregion
    }
}