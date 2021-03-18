#region Descripción
/*
    Implementa el gestor del sistema.
    Parcial Modo Servicio.
    Inicia el funcionamiento en modo servicio.
*/
#endregion

#region Using
using AMGS.Application.Utils.Log;
using Monitor.Shared;
using System;
using System.Diagnostics;
#endregion

namespace Monitor.Service
{
    static partial class Builder
    {
        #region Modo Servicio

        static internal void ModoServicio()
        {
            
            try
            {
                // crear instancia de ServiceHandler
                _serviceHandler = new ServiceHandler();

                _serviceHandler.AutoLog = true;

                System.ServiceProcess.ServiceBase.Run(_serviceHandler);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                Log.WriteEntry(TraceEventType.Error, ex.Message);
                throw;
           
            }

        }

        static internal bool IniciarServicio(string[] args)
        {
            // Estableciendo directorio activo a la ubicación del ejecutable. 
            // Los servicios inician el directorio activo en Windows/system32
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            // Comprobando presencia de parametro verbose
            if (args != null && args.Length == 1 && args[0].Length > 1
                   && (args[0][0] == '-' || args[0][0] == '/'))
            {
                string primero = args[0].Substring(1).ToLower();

                if (primero == cla_v || primero == cla_verbose)
                    OutputVerbose = true;
                else
                    OutputVerbose = false;
            }

            if (!CargarConfiguracion())
                return false;

            if (!ConfigurarComponentes())
                return false;

            if (!IniciarComponentes())
                return false;

            if (!RegistrarMensajes())
                return false;

            NotificarInicioSistema();

            // End OK 
            return true;
        }

        static internal void DetenerServicio()
        {
            DetenerComponentes();
        }

        #endregion
    }
}
