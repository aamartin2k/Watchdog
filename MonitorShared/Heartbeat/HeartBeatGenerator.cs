using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Shared.Heartbeat
{
    public class HeartBeatGenerator
    {
        #region Declaraciones

        // Contador para enviar numero consecutivo en el HeartBeat.
        private ulong _serialCount;
        #endregion

        #region Constructor
        // El constructor por defecto inicialia las propiedades a los valores necesarios
        #endregion

        #region Propiedades
        /// <summary>
        /// Controla el envío de número consecutivo en el heartbeat. Si es true se envía el número, 
        /// si es false no se envía. La acción por defecto es no enviar.
        /// </summary>
        public bool UsarSerialHB { get; set; }

        /// <summary>
        /// Establece la identificación del cliente.
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// Establece el formato alternativo para el timestamp. Es opcional, de no establecerse, 
        /// se usa el valor por defecto definido en la clase HearBeat.
        /// </summary>
        public string TimestampFormat { get; set; }

        #endregion
        /*
        static public byte[] GeneraHeartbeat()
        {
            HeartBeat hb;

            // Incorporando numero consecutivo si esta establecida la propiedad UsarSerialHB
            if (UsarSerialHB)
            {
                _serialCount++;
                // Evitar exception por sobrepasar valor maximo del tipo ulong (System.UInt64)
                if (_serialCount == ulong.MaxValue)
                    _serialCount = 1;

                if (string.IsNullOrEmpty(TimestampFormat))
                    hb = HeartBeat.CreateHeartBeat(ClientID, _serialCount);
                else
                    hb = HeartBeat.CreateHeartBeat(ClientID, TimestampFormat, _serialCount);
            }
            else
            {
                if (string.IsNullOrEmpty(TimestampFormat))
                    hb = HeartBeat.CreateHeartBeat(ClientID);
                else
                    hb = HeartBeat.CreateHeartBeat(ClientID, TimestampFormat);
            }

            return hb.ByteSequence;
        }
        */
        public string GeneraHeartbeatString()
        {
            HeartBeat hb;

            // Incorporando numero consecutivo si esta establecida la propiedad UsarSerialHB
            if (UsarSerialHB)
            {
                _serialCount++;
                // Evitar exception por sobrepasar valor maximo del tipo ulong (System.UInt64)
                if (_serialCount == ulong.MaxValue)
                    _serialCount = 1;

                if (string.IsNullOrEmpty(TimestampFormat))
                    hb = HeartBeat.CreateHeartBeat(ClientID, _serialCount);
                else
                    hb = HeartBeat.CreateHeartBeat(ClientID, TimestampFormat, _serialCount);
            }
            else
            {
                if (string.IsNullOrEmpty(TimestampFormat))
                    hb = HeartBeat.CreateHeartBeat(ClientID);
                else
                    hb = HeartBeat.CreateHeartBeat(ClientID, TimestampFormat);
            }

            return hb.CharSequence;
        }

    }
}
