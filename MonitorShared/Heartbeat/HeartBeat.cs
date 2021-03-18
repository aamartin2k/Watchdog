#region Descripción
/*
    Implementa la generacion y parsing de los datos periodicos que envian los clientes a monitorear.

    Para implementar en clientes, esta clase puede emplearse sola o en combinacion con la clases auxiliare HbSenderUdp.
    Para su uso se provee un metodo factory estatico con cuatro sobrecargas para crear un HeartBeat 
    a partir de datos iniciales, con la intencion de transmitirlo por red;
    y una sobrecarga para crear un HeartBeat a partir de un arreglo de bytes, tal y como se recibe
    desde la red.

    Esquema de datos enviados en un HeartBeat:

    Primera version:
    Timestamp
    Solo contiene timestamp. Los clientes se diferencian por el puerto local desde el que envian.

    yyyy-MM-dd HH:mm:ss

    Segunda Version:
    ClientID y Timestamp.
    QWer56 \t yyyy-MM-dd HH:mm:ss

     Client ID    
     Caracter de separacion TAB
     Timestamp   

    Tercera version:
    ClientID,Timestamp y numero consecutivo (serial):
       
      QWer56 \t yyyy-MM-dd HH:mm:ss \t 234 

      Client ID 
      Caracter de separacion TAB
      Timestamp 
      Caracter de separacion TAB
      Numero consecutivo

      Se emplea el caracter TAB como separador de campos. La longitud de los campos es irrelevante a los efectos de parsing.

    
    El campo ClientId es opcional. Debe ser proporcionado por el codigo externo.

    El campo Timestamp es el unico obligatorio. Se genera internamente en la clase. Es simplemente una conversion
    a cadena formteada del valor de la fecha y hora en que se genera el HB. (DateTime.Now.ToString(TimeStampFormat);)
    El formato de conversion empleado por defecto es el siguiente: 
       "yyyy-MM-dd HH:mm:ss"  
       
      Se ofrece la posibilidad de modificar el formato de conversion mediante el argumento format en el metodo factory.

    El campo numero consecutivo es opcional. Debe ser proporcionado por el codigo externo. Es de un 
    tipo de dato ulong (System.UInt64). Se ofrece como base para un control mas estricto de la responsividad del cliente.

    Las formas de empleo prevista son:

    Para transmitir, generar un HB en el cliente y enviarlo por conexion UDP al receptor:

        -Emplear uno de las cuatro sobrecargas del metodo factory y obtener una instancia de la clase con los valores deseados.
        -Obtener el arreglo de bytes mediante la propiedad ByteSequence y transmitirla.

        (Esta funcion esta implementada en la clase auxiliar HbSenderUdp)

    Para recibir, generar un HB en el servidor a partir de los bytes recibidos por conexion UDP:
        -Emplear la sobrecargas del metodo factory que recibe una arreglo de bytes y retorna una instancia de la clase.
        -Continuar el procesamiento de dicha instancia.

        (Esta funcion esta implementada en la clase auxiliar HbReceiverUdp)

*/
#endregion

#region Using
using System;
#endregion


namespace Monitor.Shared.Heartbeat
{

    public class HeartBeat
    {
        #region Declaraciones
                
        private const string Tab = "\t";
        private string _format;
        private const string _defFormat = "yyyy-MM-dd HH:mm:ss";

        #endregion

        #region Propiedades
        /// <summary>
        /// Retorna la cadena de formato por defecto empleada para crear timestamps.
        /// </summary>
        
        public string ClientID { get; private set; }
       
        public string Timestamp { get; private set; }

        public string TimeStampFormat
        { get
            {
                if (_format == null)
                    return _defFormat;
                else
                    return _format;

            }

            private set { _format = value; } }

        public string Serial { get; private set; }
        //public ulong SerialNumber { get; private set; }
        //public byte[] ByteSequence { get; private set; }
        public string CharSequence { get; private set; }


        #endregion

        #region Constructores

        // default
        public HeartBeat()
        {
        }

        // Variante SIN numero Serial:
        // unico argumento ClientID, se genera internamente el timestamp.

        private HeartBeat(string id)
        {
            ClientID = id;
            Timestamp = DateTime.Now.ToString(TimeStampFormat);
            Serial = null;
            //SerialNumber = 0;

            if (ClientID == null)
            {
                // HB V 1
                //ByteSequence = new ASCIIEncoding().GetBytes(Timestamp);
                CharSequence = Timestamp;
            }
            else
            {
                // HB V 2
                //ByteSequence = new ASCIIEncoding().GetBytes(ClientID + Tab + Timestamp);
                CharSequence = ClientID + Tab + Timestamp;
            }
        }

        // 2 argumentos, ClientID y timestamp format.
        private HeartBeat(string id, string format)
        {
            ClientID = id;
            TimeStampFormat = format;
            Timestamp = DateTime.Now.ToString(TimeStampFormat);
            Serial = null;
            //SerialNumber = 0;

            if (ClientID == null)
            {
                // HB V 1
                //ByteSequence = new ASCIIEncoding().GetBytes(Timestamp);
                CharSequence = Timestamp;
            }
            else
            {
                // HB V 2
                //ByteSequence = new ASCIIEncoding().GetBytes(ClientID + Tab + Timestamp);
                CharSequence = ClientID + Tab + Timestamp;
            }
        }

        // Variante CON numero Serial:
        // Se genera internamente el timestamp.
        private HeartBeat(string id, ulong serial)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id", "Se necesita un valor de Id.");

            ClientID = id;
            Timestamp = DateTime.Now.ToString(TimeStampFormat);
            //SerialNumber = serial;
            Serial = serial.ToString();

            // HB V 3
            //ByteSequence = new ASCIIEncoding().GetBytes(ClientID + Tab + Timestamp + Tab + Serial);
            CharSequence = ClientID + Tab + Timestamp + Tab + Serial;
        }

        private HeartBeat(string id, string format, ulong serial)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id", "Se necesita un valor de Id.");

            ClientID = id;
            TimeStampFormat = format;
            Timestamp = DateTime.Now.ToString(TimeStampFormat);
            //SerialNumber = serial;
            Serial = serial.ToString();

            // HB V 3
            //ByteSequence = new ASCIIEncoding().GetBytes(ClientID + Tab + Timestamp + Tab + Serial);
            CharSequence = ClientID + Tab + Timestamp + Tab + Serial;
        }

        /*
        private HeartBeat(byte[] bs)
        {
            ByteSequence = bs;

            string hb = new ASCIIEncoding().GetString(ByteSequence).TrimEnd(new Char[] { '\0' });
            
            string[] split = hb.Split(new Char[] { '\t' });

            switch (split.Length)
            {
                case 1:
                    Timestamp = split[0];
                    ClientID = null;
                    Serial = null;
                    //SerialNumber = 0;
                    break;

                case 2:
                    ClientID = split[0];
                    Timestamp = split[1];
                    Serial = null;
                    //SerialNumber = 0;
                    break;

                case 3:
                default:
                    ClientID = split[0];
                    Timestamp = split[1];
                    Serial = split[2];

                    //ulong number;
                    //ulong.TryParse(Serial, out number);
                    // Contiene el valor parseado o ZERO si ocurre error de parsing.
                    //SerialNumber = number;
                    break;
            }
  
        }
        */
        #endregion

        #region Factory

        /// <summary>
        /// Crea una instancia de la clase con Id de cliente y timestamp formateado por defecto. 
        /// Si la Id es null, se genera una instancia compuesta solo de timestamp.
        /// </summary>
        /// <param name="id">Id de cliente. Cadena que identifica al cliente.</param>
        /// <returns>Nueva instancia de la clase.</returns>
        static public HeartBeat CreateHeartBeat(string id)
        {
            HeartBeat hb = new HeartBeat(id);
            return hb;
        }

        /// <summary>
        /// Crea una instancia de la clase con Id de cliente y timestamp formateado de acuerdo al
        /// argumento presente. Si la Id es null, se genera una instancia compuesta solo de timestamp.
        /// </summary>
        /// <param name="id">Id de cliente. Cadena que identifica al cliente.</param>
        /// <param name="format">Formato a aplicar al timestamp.</param>
        /// <returns>Nueva instancia de la clase.</returns>
        static public HeartBeat CreateHeartBeat(string id, string format)
        {
            HeartBeat hb = new HeartBeat(id, format);

            return hb;
        }

        /// <summary>
        /// Crea una instancia de la clase con Id de cliente y número consecutivo.
        /// </summary>
        /// <param name="id">Id de cliente. Cadena que identifica al cliente.</param>
        /// <param name="serial">Número consecutivo.</param>
        /// <returns>Nueva instancia de la clase.</returns>
        static public HeartBeat CreateHeartBeat(string id, ulong serial)
        {
            return new HeartBeat(id, serial);
        }

        /// <summary>
        /// Crea una instancia de la clase con Id de cliente, timestamp formateado de acuerdo al
        /// argumento presente y número consecutivo.
        /// </summary>
        /// <param name="id">Id de cliente. Cadena que identifica al cliente.</param>
        /// <param name="format">Formato a aplicar al timestamp.</param>
        /// <param name="serial">Número consecutivo.</param>
        /// <returns>Nueva instancia de la clase.</returns>
        static public HeartBeat CreateHeartBeat(string id, string format, ulong serial)
        {
            return new HeartBeat(id, format, serial);
        }

        /// <summary>
        /// Crea una instancia de la clase a partir de una secuencia de bytes.
        /// </summary>
        /// <param name="bs">Secuencia de bytes recibida.</param>
        /// <returns>Nueva instancia de la clase.</returns>
        /*
        static public HeartBeat CreateHeartBeat(byte[] bs)
        {
            return new HeartBeat(bs);
        }
        */

        static public HeartBeat CreateHeartBeatFromString(string  data)
        {
            HeartBeat newHb = new HeartBeat(); 

            string[] split = data.Split(new Char[] { '\t' });

            switch (split.Length)
            {
                case 1:
                    newHb.Timestamp = split[0];
                    newHb.ClientID = null;
                    newHb.Serial = null;
                    //newHb.SerialNumber = 0;       
                    break;

                case 2:
                    newHb.ClientID = split[0];
                    newHb.Timestamp = split[1];
                    newHb.Serial = null;
                    //newHb.SerialNumber = 0;
                    break;

                case 3:
                default:
                    newHb.ClientID = split[0];
                    newHb.Timestamp = split[1];
                    newHb.Serial = split[2];

                    ulong number;
                    ulong.TryParse(split[2], out number);
                    // Contiene el valor parseado o ZERO si ocurre error de parsing.
                    //newHb.SerialNumber = number;
                    break;
            }

            return newHb;

        }
        #endregion

    }

}
