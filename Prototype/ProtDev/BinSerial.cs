using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;



namespace ProtDev
{
    public static class Serializer
    {

        #region Metodos Publicos


        public static T Deserialize<T>(string fileName)
        {
            T newObject = (T)DeserializeObject(fileName);

            return newObject;
        }

        public static void Serialize<T>(T objToSerialize, string fileName)
        {
            SerializeObject(objToSerialize, fileName);
        }

        // Ser/Deser a  Memory stream
        public static T Deserialize<T>(Stream serialStream)
        {
            T newObject = (T)DeserializeObject(serialStream);

            return newObject;
        }

        public static void Serialize<T>(T objToSerialize, Stream serialStream)
        {
            SerializeObject(objToSerialize, serialStream);
        }

        #endregion


        #region Metodos Privados

        private static object DeserializeObject(string fileName)
        {
            FileStream fsrem = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            try
            {
                return DeserializeObject(fsrem);
            }
            finally
            {
                fsrem.Close();
            }

        }

        private static object DeserializeObject(Stream serialStream)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(serialStream);
            }
            catch (SerializationException e)
            {
                throw e;
            }

        }

        private static void SerializeObject(object objToSerialize, string fileName)
        {
            FileStream fstrm = new FileStream(fileName, FileMode.Create, FileAccess.Write);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fstrm, objToSerialize);

                SerializeObject(objToSerialize, fstrm);
            }
            finally
            {
                fstrm.Close();
            }

        }

        private static void SerializeObject(object objToSerialize, Stream serialStream)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(serialStream, objToSerialize);
            }
            catch (SerializationException e)
            {
                throw e;
            }
        }


        #endregion 
    }

}
