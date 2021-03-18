#region Descripción
/*
    Implementa el gestor del sistema.
    Almacena referencias, crea instancias y enlaza notificaciones con controladores.
*/
#endregion

#region Using
using AMGS.Application.Utils.Log;
using AMGS.Application.Utils.Serialization.Bin;
using System;
using System.IO;
using System.Diagnostics;
#endregion

namespace Monitor.Supervisor
{
    internal class DbHandler
    {

        #region Declaraciones
        // Para uso de Log
        private const string ClassName = "DbHandler";

        private const string DatabaseName = "Monitor.Supervisor.Data.bsr";
        private SupervisorData _dbRoot;

        #endregion

        #region Propiedades

        internal string ServerUrl
        {
            get { return _dbRoot.ServerUrl; }

            set { _dbRoot.ServerUrl = value; }
        }

        public string[] ServerUrlList
        {
            get { return _dbRoot.ServerUrlList ; }
        }
        #endregion
           

        #region Database
        internal bool OpenDatabase()
        {
            try
            {
                SupervisorData tmpDB;

                if (File.Exists(DatabaseName))
                {
                    Log.WriteEntry(TraceEventType.Verbose, "Database existente");
                    tmpDB = DeSerializarDeDisco(DatabaseName);
                }
                else
                {
                    Log.WriteEntry(TraceEventType.Verbose, "Database creada");
                    tmpDB = new SupervisorData();

                    SerializarADisco(tmpDB, DatabaseName);
                }
            
                _dbRoot = tmpDB;
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ClassName, "OpenDatabase", TraceEventType.Error, ex.Message);
                return false;
            }
        }

        private void CommitDatabase()
        {
            SerializarADisco(_dbRoot, DatabaseName);
            Log.WriteEntry(ClassName, "CommitDatabase", TraceEventType.Verbose, "Commit Database");
        }
        internal void CloseDatabase()
        {
            CommitDatabase();
        }

        #endregion

        #region Serializacion
        // Emplea funcionalidad del componente AppUtils
        private void SerializarADisco(SupervisorData obj, string file)
        {
            if (obj == null)
            {
                Log.WriteEntry(ClassName, "SerializarADisco", TraceEventType.Warning, "Objeto a serializar es Null");
                return;
            }
            
            try
            {
                Serializer.Serialize<SupervisorData>(obj, file);
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ClassName, "SerializarADisco", TraceEventType.Error, ex.Message);
                throw;
            }
        }

        private SupervisorData DeSerializarDeDisco(string file)
        {
            try
            {
                return Serializer.Deserialize<SupervisorData>(file);
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ClassName, "DeSerializarDeDisco", TraceEventType.Error, ex.Message);
                throw;
            }
        }


        #endregion

    }
}
