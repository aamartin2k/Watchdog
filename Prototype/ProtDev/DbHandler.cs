using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Monitor.Shared;



namespace ProtDev
{
    class DbHandler
    {
        private const string DatabaseName = "Prototipo.bsr";
        internal SystemPlusClientData _dbRoot;

        
        internal void OpenDatabase()
        {
            SystemPlusClientData tmpDB;

            if (File.Exists(DatabaseName))
            {
                Console.WriteLine("Db Existente");

                tmpDB = Serializer.Deserialize<SystemPlusClientData>(DatabaseName);
                // implementar uso stream memoria
            }
            else
            {
                Console.WriteLine("Db Nueva creacion");
                tmpDB = new SystemPlusClientData();
                Serializer.Serialize<SystemPlusClientData>(tmpDB, DatabaseName);
            }
            _dbRoot = tmpDB;

        }

        internal void DbCommit()
        {
            Console.WriteLine("db.Commit");
            Serializer.Serialize<SystemPlusClientData>(_dbRoot, DatabaseName);
            // implementar uso stream memoria
        }

        internal void DbClose()
        {
            Console.WriteLine("db.Close");
            // implementar cambio a disco
        }

       

       

    }
}
