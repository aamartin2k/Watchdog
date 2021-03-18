#region Descripción
/*
    Implementa el gestor del sistema.
    Almacena referencias, crea instancias y enlaza notificaciones con controladores.

    Parcial Declaraciones.
*/
#endregion

#region Using
using Monitor.Shared;
using System.Collections.Generic;
using System.Threading;
#endregion

namespace Monitor.Service
{

    static partial class Builder
    {

        #region Declaraciones

        // Para uso de Log.
        private const string ClassName = "Builder";

        // Componentes
        static private ClientManager _clientManager;
        static private DbHandler _dbHandler;
        static private ZyanServer _zyanServer;
        static private Notifier _notifier;

        // Sub componente de ZyanServer.
        static private RemoteMonitor _remoteMonitor;

        // Service Handler
        static private ServiceHandler _serviceHandler;
        // Formulario
        static private FormEditConfig _editForm;

        // Bool flag para detectar error.
        static private bool Detenerse;
        // Objeto de sincronizacion
        static private AutoResetEvent _areEsperaOperacion = new AutoResetEvent(false);

        // Bool flag para registrar si hay cliente supervisor conectado.
        static private bool SupervisorOnLine;
        // Lista para almacenar los textos de consola que se enviaran
        // al cliente supervisor cuando se conecta.
        static private List<RemoteConsoleText> _texCache;

        // Bool flag para mostrar mensajes tipo verbose por consola.
        static private bool OutputVerbose;

        // Constantes para argumentos de linea de comando.
        private const string cla_im = "im";
        private const string cla_import = "import";
        private const string cla_ex = "ex";
        private const string cla_export = "export";
        private const string cla_c = "c";
        private const string cla_console = "console";
        private const string cla_a = "a";
        private const string cla_add = "add";
        private const string cla_e = "e";
        private const string cla_edit = "edit";
        private const string cla_d = "d";
        private const string cla_del = "del";
        private const string cla_s = "s";
        private const string cla_sys = "sys";
        private const string cla_cc = "cc";
        private const string cla_create = "create";
        private const string cla_as = "as";
        private const string cla_assist = "assist";
        private const string cla_i = "i";
        private const string cla_install = "install";
        private const string cla_u = "u";
        private const string cla_uninstall = "uninstall";
        private const string cla_h = "h";
        private const string cla_help = "help";

        private const string cla_v = "v";
        private const string cla_verbose = "verbose";

        #endregion

    }
}
