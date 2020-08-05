#region Descripción
/*
    Implementa el gestor del servicio de Windows.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace Monitor.Service
{
    internal class ServiceHandler : ServiceBase
    {

        #region  Metodos para Implementación de Servicio de Windows

        /// <summary>
        /// Inicia el servicio.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
           
            if (!Builder.IniciarServicio(args))
                Builder.DetenerServicio();
        }

        /// <summary>
        /// Detiene el servicio.
        /// </summary>
        protected override void OnStop()
        {
            Builder.DetenerServicio();
        }

        #endregion

    }
}
