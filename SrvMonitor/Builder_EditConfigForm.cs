#region Descripción
/*
    Implementa el gestor del sistema.
    Parcial Formulario de Configuración
    Implementa el uso de formulario para realizar acciones de configuracion
*/
#endregion

#region Using
using Monitor.Shared;
using System.Windows.Forms;

#endregion

namespace Monitor.Service
{
    static partial class Builder
    {
        #region Modo Formulario de Configuración

        private static void CargarFormulario(string arg)
        {
            string modo = "Default";

            // determinando modo form
            EditFormMode efm = EditFormMode.Default;

            switch (arg)
            {
                case cla_add:
                case cla_a:
                    efm = EditFormMode.CreateClient;
                    modo = "Crear cliente.";
                    break;
                case cla_edit:
                case cla_e:
                    efm = EditFormMode.EditClient;
                    modo = "Editar cliente.";
                    break;
                case cla_del:
                case cla_d:
                    efm = EditFormMode.DeleteClient;
                    modo = "Eliminar cliente.";
                    break;
                case cla_sys:
                case cla_s:
                    efm = EditFormMode.EditSystem;
                    modo = "Configurar sistema.";
                    break;
                default:
                    break;
            }

            Builder.Output("Cargando formulario de configuracion en modo: " + modo);

            // Chequeando condiciones extremas
            // Si no hay clientes registrados, no ejecutar Delete ni Edit
            if (_dbHandler.ClientList.Count == 0)
            {
                switch (arg)
                {
                    case cla_edit:
                    case cla_e:
                    case cla_del:
                    case cla_d:
                        MessageBox.Show("No existen clientes registrados.");
                        return;
                }
            }

            _editForm = new FormEditConfig(efm);
            _editForm.StartPosition = FormStartPosition.CenterScreen;
            
            // Pasando datos al formulario
            switch (arg)
            {
                case cla_add:
                case cla_a:
                case cla_edit:
                case cla_e:
                case cla_del:
                case cla_d:
                    MessageBus.Register<SendClientConfig>(_editForm.ReceiveClientConfig);
                    MessageBus.Send(new SendClientConfig(_dbHandler.ClientList));
                    break;
                case cla_sys:
                case cla_s:
                    MessageBus.Register<SendSystemConfig>(_editForm.ReceiveSystemConfig);
                    MessageBus.Send(new SendSystemConfig(_dbHandler.SystemData));
                    break;
            }

            MessageBus.Register<FormEndEdit>(DoFormEnd);
            MessageBus.Register<RequestSaveConfig>(_dbHandler.Save);

            Application.Run(_editForm);

            // Fin de accion del formulario
            Builder.Output("Formulario de configuracion terminado.");
        }

        static private void DoFormEnd(FormEndEdit req)
        {

            string msg = req.AcceptChanges ? "Edicion aceptada." : "Edicion cancelada.";
            Builder.Output(msg);

            _editForm.Close();

        }

        #endregion
    }
}
