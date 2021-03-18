#region Descripción
/*
    Implementa el gestor del sistema.
    Acciones de importar y exportar la configuracion del sistema.
    Parcial Import Export
    
*/
#endregion

#region Using
using System.Diagnostics;
#endregion

namespace Monitor.Service
{
    static partial class Builder
    {
        #region Acciones Import Export
        static private void ImportExport(string arg, string file)
        {
           
            switch (arg)
            {
                case cla_im:
                case cla_import:
                    ImportData(file);
                    break;

                case cla_ex:
                case cla_export:
                    ExportData(file);
                    break;

            }
        }

        static private void ImportData(string file)
        {
            Builder.Output("Importando configuracion desde archivo: " + file , TraceEventType.Verbose);

            // file must exists
            if (System.IO.File.Exists(file))
            {
                if (_dbHandler.ImportConfig(file))
                    Builder.Output("Configuracion importada OK.");
                else
                    Builder.Output("Error: " + _dbHandler.ErrorMsg);
            }
            else
            {
                Builder.Output("No se encuentra archivo para importar.", TraceEventType.Error);
            }
        }

        static private void ExportData(string file)
        {
            Builder.Output("Exportando configuracion hacia archivo: " + file , TraceEventType.Verbose);

            if (_dbHandler.ExportConfig(file))
                Builder.Output("Configuracion exportada OK.");
            else
                Builder.Output("Error: " + _dbHandler.ErrorMsg);

        }

        #endregion
    }
}