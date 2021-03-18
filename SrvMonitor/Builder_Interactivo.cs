#region Descripción
/*
    Implementa el gestor del sistema.
    Parcial Interactivo.
    Implementa las acciones a realizar desde la linea de comando.
*/
#endregion

#region Using
using Monitor.Shared;
using System;
using System.Threading;

#endregion

namespace Monitor.Service
{
    static partial class Builder
    {
        #region Modo Interactivo 

        static private string _errMsg;
        static internal void ModoInteractivo(string[] args)
        {
          
            string primero = string.Empty;
            string segundo = string.Empty;

            // Interpretando argumentos por cantidad.
            // Sin argumentos en modo interactivo...
            if (args != null && args.Length == 0)
            {
                // Si no existe archivo de configuracion, ejecutar asistente
                //  de configuracion, despues de cargar DbHandler.
                if (!DbHandler.DatabaseExists())
                    ContinuarModoInteractivo(cla_assist, null);
                else
                    ShowHelp();
            }
            else    // Dos argumentos, Para Import/Export El segundo arg es filename.
                    // Para Console, el posible segundo argumento es  v verbose con - /
            if (args != null && args.Length == 2 && args[0].Length > 1 && args[1].Length > 1
                   && (args[0][0] == '-' || args[0][0] == '/'))
            {
                // Eliminar separador arg '-' or '/'
                primero = args[0].Substring(1).ToLower();

                switch (primero)
                {
                    case cla_im:
                    case cla_import:
                    case cla_ex:
                    case cla_export:
                        ContinuarModoInteractivo(primero, args[1]);
                        break;

                    case cla_c:
                    case cla_console:
                        segundo = args[1].Substring(1).ToLower();

                        if (segundo == cla_v || segundo == cla_verbose)
                            ContinuarModoInteractivo(primero, segundo);
                        break;
                }
            }
            else    // UN argumento, varias opciones
            if (args != null && args.Length == 1 && args[0].Length > 1
                    && (args[0][0] == '-' || args[0][0] == '/'))
            {
                // Replace args
                primero = args[0].Substring(1).ToLower();

                switch (primero)
                {
                    case cla_add:
                    case cla_a:
                    case cla_edit:
                    case cla_e:
                    case cla_del:
                    case cla_d:
                    case cla_sys:
                    case cla_s:
                    case cla_console:
                    case cla_c:
                    case cla_cc:
                    case cla_create:
                    case cla_as:
                    case cla_assist:
                        ContinuarModoInteractivo(primero, null);
                        break;

                    case cla_install:
                    case cla_i:
                        InstallService();
                        break;

                    case cla_uninstall:
                    case cla_u:
                        UninstallService();
                        break;

                    case cla_h:
                    case cla_help:
                    default:
                        ShowHelp();
                        break;
                }
            }
            // Salida de regreso a Main().
            
        }

        

        static private void ContinuarModoInteractivo(string primero, string segundo)
        {
            // Comprobando cmd arg verbose
            if (segundo != null && (segundo == cla_v || segundo == cla_verbose))
                OutputVerbose = true;
            else
                OutputVerbose = false;

            // Preparando cache de texto para consola remota
            // Es imprescindible ejecutar este metodo antes de llamar a Builder.Output.
            ClientLogonEvent(new SupervisorClientLogEvent(SupervisorLoginType.Logoff));

            // Preparando Consola
            Console.Clear();
            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;
            baseRow = 0;

            Builder.Output("Iniciando Monitor.Service en modo interactivo.");

            if (!CargarConfiguracion())
                return;

            Builder.Output("Ejecutando accion interactiva.");

            switch (primero)
            {
                case cla_im:
                case cla_import:
                case cla_ex:
                case cla_export:
                    ImportExport(primero, segundo);
                    break;

                case cla_add:
                case cla_a:
                case cla_edit:
                case cla_e:
                case cla_del:
                case cla_d:
                case cla_sys:
                case cla_s:
                    CargarFormulario(primero);
                    break;

                case cla_console:
                case cla_c:
                    CargarTodosEjecutarConsole();
                    break;

                case cla_cc:
                case cla_create:
                    CreateConfig();
                    break;

                case cla_as:
                case cla_assist:
                    Wizard_System();
                    break;
            }

            // Salida de regreso a ModoInteractivo()
            // Terminar DbHandler
            _dbHandler = null;

            Builder.Output("Accion interactiva terminada.");
            Builder.Output("Terminado Monitor.Service en modo interactivo.");
        }

       
        #region Controladores de Respuestas

        // Receptor ReplyStart
        // Continuar carga configuracion y ejecucion
        static private void DbHandlerStartOK(ReplyOK reply)
        {
            Builder.Output("DbHandler Start OK");

            // Se elimina para que no ejecute al enviar este mensaje 
            // para iniciar los otros modulos
            MessageBus.Remove<RequestStart>(_dbHandler.Start);
            MessageBus.Remove<RequestStop>(_dbHandler.Stop);
            MessageBus.Remove<ReplyOK>(DbHandlerStartOK);

            Detenerse = false;
            _areEsperaOperacion.Set();
      
        }

        // Receptor ConfigError
        // Ocurrio un error iniciando DbHandler o cargando configuracion, informar y cerrar.
        static private void ModuleStartError(ReplyError reply)
        {
            //Builder.Output("Error: " + reply.Message, System.Diagnostics.TraceEventType.Error);
            Detenerse = true;
            _errMsg = reply.Message;
            _areEsperaOperacion.Set();
        }



        #endregion

        static internal void CreateConfig()
        {
            Console.WriteLine("Creando configuracion de prueba.");

            // Ejecutando metodo en DbHandler
            _dbHandler.CreateTestConfig();

            Console.WriteLine("Configuracion de prueba creada.");
        }

        static private bool CargarConfiguracion()
        {
            Builder.Output("Cargando configuracion.");
            // crear instancia
            _dbHandler = new DbHandler();

            bool ret = _dbHandler.OpenDatabase();
            if (!ret)
            {
                // log error
                Builder.Output("Error: " + _dbHandler.ErrorMsg, System.Diagnostics.TraceEventType.Error);
                _dbHandler = null;
                return false;
            }

            Builder.Output("Configuracion cargada.");
            return true;
        }

       

        #endregion
    }
}
