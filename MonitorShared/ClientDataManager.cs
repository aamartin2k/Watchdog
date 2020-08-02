#region Descripción
/*
    Gestiona la coleccion de datos de cliente y los metodos para creacion y edicion.
 */
#endregion


#region Using
using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
#endregion

namespace Monitor.Shared
{
    [Serializable]
    public class ClientDataManager
    {
        // Declaraciones

        // Almacen para clientes
        // Recuperacion de todos los clientes y enumeracion
        private Dictionary<Guid, ClientData> _clientDict;
        // Recuperacion de clientes por ID
        private Dictionary<string, Guid> _listById;
        // Recuperacion de clientes por Puerto
        private Dictionary<int, Guid> _listByPort;
        // Recuperacion de clientes por Nombre
        private Dictionary<string, Guid> _listByName;

            

        // Constructor
        public ClientDataManager()
        {
            _clientDict = new Dictionary<Guid, ClientData>();
            _listById = new Dictionary<string, Guid>();
            _listByName = new Dictionary<string, Guid>();
            _listByPort = new Dictionary<int, Guid>();

        }

        // Propiedades
        // Lista de recuperacion e iteracion
        public ClientData[] List
        {
            get
            {
                ClientData[] list = new ClientData[_clientDict.Values.Count];
                 _clientDict.Values.CopyTo(list, 0) ;

                return list;
            }
        }

        // Cantidad de Clientes
        public int Count
        {
            get { return _clientDict.Values.Count; }
        }
        // Metodos
        // Recuperacion de clientes

        public bool ContainsId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            return _listById.ContainsKey(id);
        }

        public bool ContainsPort(int port)
        { return _listByPort.ContainsKey(port); }

        public bool ContainsName(string name)
        { return _listByName.ContainsKey(name); }

        public bool ContainsGuid(Guid key)
        { return _clientDict.ContainsKey(key); }

        // Recuperacion de un cliente por claves
        public ClientData GetClient(Guid guid)
        {
            if (_clientDict.ContainsKey(guid))
                return _clientDict[guid];
            else
                throw new Exception("Clave Guid desconocida!");
        }

        public ClientData GetClient(string id)
        {
            Guid key;

            if (_listById.ContainsKey(id))
                key = _listById[id];
            else
                throw new Exception("Clave Id desconocida!");

            return _clientDict[key];
        }

        public ClientData GetClient(int port)
        {
            Guid key;

            if (_listByPort.ContainsKey(port))
                key = _listByPort[port];
            else
                throw new Exception("Clave port desconocida!");

            return _clientDict[key];
        }

        public ClientData GetClientByName(string name)
        {
            Guid key;

            if (_listByName.ContainsKey(name))
                key = _listByName[name];
            else
                throw new Exception("Clave name desconocida!");

            return _clientDict[key];
        }

        public Guid GetClientIdByName(string name)
        {
            Guid key;

            if (_listByName.ContainsKey(name))
            { 
                key = _listByName[name];
                return key;
            }
            else
                throw new Exception("Clave name desconocida!");
        }

        public bool PortInUseByOthers(int port, string name)
        {
            
            if (_listByPort.ContainsKey(port))
            {    // lo contiene, comprobar que no es el mismo
                if (GetClient(port).Name == name)
                    return false;
                else
                    return true;
            }

            return false;
        }

        public bool IdInUseByOthers(string id, string name)
        {

            if (_listById.ContainsKey(id))
            {    // lo contiene, comprobar que no es el mismo
                if (GetClient(id).Name == name)
                    return false;
                else
                    return true;
            }

            return false;
        }

        public bool NameInUseByOthers(string newName, string name)
        {

            if (_listByName.ContainsKey(newName))
            {    // lo contiene, comprobar que no es el mismo
                if (GetClientByName(newName).Name == name)
                    return false;
                else
                    return true;
            }

            return false;
        }

        // Gestion de clientes
        private void Add(ClientData client)
        {
            // generar UId
            Guid key = Guid.NewGuid();

            client.ClientId = key;
            _clientDict.Add(key, client);

            _listByName.Add(client.Name, key);

            if (client.IdType == ClientIdType.KeyByIdString)
                _listById.Add(client.Id, key);
            else
                _listByPort.Add(client.Port, key);
        }

        public void Delete(Guid key)
        {
            if (_clientDict.ContainsKey(key))
                _clientDict.Remove(key);

            UpdateKeys();
        }

        public void CreateClientRemote(ClientRemoteUpdate newClt)
        {
            CreateClient(newClt.IdType,
                        newClt.Id,
                        newClt.Port,
                        newClt.Name,
                        newClt.AppFilePath, newClt.LogFilePath,
                        newClt.Timeout, newClt.MailEnabled, newClt.LogAttachEnabled, newClt.QueueSize);
                        
        }

        public void UpdateClient(ClientRemoteUpdate updatedClient)
        {
            ClientData oldClient = GetClient(updatedClient.ClientId);

            oldClient.IdType = updatedClient.IdType;
            oldClient.Id = updatedClient.Id;
            oldClient.Port = updatedClient.Port;
            oldClient.Name = updatedClient.Name;

            oldClient.AppFilePath = updatedClient.AppFilePath;
            oldClient.LogFilePath = updatedClient.LogFilePath;
            oldClient.Timeout = updatedClient.Timeout;

            oldClient.MailEnabled = updatedClient.MailEnabled;
            oldClient.LogAttachEnabled = updatedClient.LogAttachEnabled;
            oldClient.QueueSize = updatedClient.QueueSize;
           
            UpdateKeys();
        }

        public void UpdateKeys()
        {
            // recreando las listas de claves
            _listById = new Dictionary<string, Guid>();
            _listByName = new Dictionary<string, Guid>();
            _listByPort = new Dictionary<int, Guid>();

            Guid key;
            ClientData client;

            foreach (var item in _clientDict)
            {
                client = item.Value;
                key = client.ClientId;

                _listByName.Add(client.Name, key);

                if (client.IdType == ClientIdType.KeyByIdString)
                    _listById.Add(client.Id, key);
                else
                    _listByPort.Add(client.Port, key);
            }
        }


        // Creacion de cliente
        // Nuevo creador de clientes
        public ClientData CreateClient(ClientIdType type, 
                                        string id, 
                                        int port, 
                                        string name, 
                                        string appPath, string logPath,
                                        int timeout, bool mail, bool logAttach, int queueSize)
        {
            // Validacion de parametros
            // Claves
            if (type == ClientIdType.KeyByIdString)
            {
                // Clave ID
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentException("El valor no puede ser nulo.", "id");

                if (ContainsId(id))
                    throw new ArgumentException("El valor ya está registrado.", "id");

                // Si el puerto NO es clave, puede ser cero. Si es distinto de cero, debe
                // estar en el rango libre
                if (port != 0  && (port < 1024 || port > 65535))
                {
                    throw new ArgumentOutOfRangeException("port", "El  valor debe estar entre 1024 y 65535.");
                }

            }
            else
            {
                // Clave Puerto
                if (port == 0)
                    throw new ArgumentException("El valor no puede ser cero.", "port");

                if (port < 1024 || port > 65535)
                {
                    throw new ArgumentOutOfRangeException("port", "El  valor debe estar entre 1024 y 65535.");
                }

                if (ContainsPort(port))
                    throw new ArgumentException("El valor ya está registrado.", "port");
            }
            // name
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "El valor no puede ser nulo.");
            if (ContainsName(name))
                throw new ArgumentException("El valor ya está registrado.", "name");

            // Resto de los campos 
            // appPath
            if (string.IsNullOrEmpty(appPath))
                throw new ArgumentNullException("appPath", "El valor no puede ser nulo.");
            // logPath
            if (string.IsNullOrEmpty(logPath))
                throw new ArgumentNullException("logPath", "El valor no puede ser nulo.");
            // timeout
            if (timeout == 0)
                throw new ArgumentException("El valor no puede ser cero.", "timeout");
            // queueSize
            if (queueSize == 0)
                throw new ArgumentException("El valor no puede ser cero.", "queueSize");

            // Creacion del objeto
            ClientData newClt = new ClientData();

            newClt.IdType = type;
            newClt.Id = id;
            newClt.Port = port;
            newClt.Name = name;
            newClt.AppFilePath = appPath;
            newClt.LogFilePath = logPath;
            newClt.Timeout = timeout;
            newClt.MailEnabled = mail;
            newClt.LogAttachEnabled = logAttach;
            newClt.QueueSize = queueSize;

            Add(newClt);

            return newClt;


        }

        /*
        // Clave Id
        public ClientData CreateClient(string id, string name, string appPath, string logPath,
                                       int timeout, bool mail, bool logAttach, int queueSize)
        {
            // Validando datos
            // Clave ID
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("El valor no puede ser nulo.", "id");

            if (ContainsId(id))
                throw new ArgumentException("El valor ya está registrado.", "id");

            // name
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "El valor no puede ser nulo.");
            if (ContainsName(name))
                throw new ArgumentException("El valor ya está registrado.", "name");

            // appPath
            if (string.IsNullOrEmpty(appPath))
                throw new ArgumentNullException("appPath", "El valor no puede ser nulo.");
            // logPath
            if (string.IsNullOrEmpty(logPath))
                throw new ArgumentNullException("logPath", "El valor no puede ser nulo.");
            // timeout
            if (timeout == 0)
                throw new ArgumentException("El valor no puede ser cero.", "timeout");
            // queueSize
            if (queueSize == 0)
                throw new ArgumentException("El valor no puede ser cero.", "queueSize");


            ClientData newClt = new ClientData();

            newClt.IdType = ClientIdType.KeyByIdString;
            newClt.Id = id;
            newClt.Name = name;
            newClt.AppFilePath = appPath;
            newClt.LogFilePath = logPath;
            newClt.Timeout = timeout;
            newClt.MailEnabled = mail;
            newClt.LogAttachEnabled = logAttach;
            newClt.QueueSize = queueSize;

            Add(newClt);

            return newClt;
        }

        // Clave PuertoUdp
        public ClientData CreateClient(int port, string name, string appPath, string logPath,
                                       int timeout, bool mail, bool logAttach, int queueSize)
        {
           // Validando datos
           // Clave Puerto
           if (port == 0)
                throw new ArgumentException("El valor no puede ser cero.", "port");

            if (port < 1024 || port > 65535)
            {
                throw new ArgumentOutOfRangeException("port", "El  valor debe estar entre 1024 y 65535.");
            }

            if (ContainsPort(port))
                throw new ArgumentException("El valor ya está registrado.", "port");
            // name
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "El valor no puede ser nulo.");
            if (ContainsName(name))
                throw new ArgumentException("El valor ya está registrado.", "name");

            // appPath
            if (string.IsNullOrEmpty(appPath))
                throw new ArgumentNullException("appPath", "El valor no puede ser nulo.");
            // logPath
            if (string.IsNullOrEmpty(logPath))
                throw new ArgumentNullException("logPath", "El valor no puede ser nulo.");
            // timeout
            if (timeout == 0)
                throw new ArgumentException("El valor no puede ser cero.", "timeout");
            // queueSize
            if (queueSize == 0)
                throw new ArgumentException("El valor no puede ser cero.", "queueSize");

            // Creando objeto
            ClientData newClt = new ClientData();


            newClt.IdType = ClientIdType.KeyByUdpPort;
            newClt.Port = port;
            newClt.Name = name;
            newClt.AppFilePath = appPath;
            newClt.LogFilePath = logPath;
            newClt.Timeout = timeout;
            newClt.MailEnabled = mail;
            newClt.LogAttachEnabled = logAttach;
            newClt.QueueSize = queueSize;

            Add(newClt);

            return newClt;
        }

        */
       
    }
}
