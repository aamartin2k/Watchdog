#region Descripción
//Implementa los datos de configuración y estado del Monitor.
#endregion

#region Using
using System;
#endregion

namespace Monitor.Shared
{
    /// <summary>
    /// Implementa los datos de configuración y estado del Monitor.
    /// El atributo  [Serializable] es necesario para Zyan Frx y serializacion a disco.
    /// </summary>
    [Serializable]
    public class SystemConfigData
    {
        // Datos de correo electronico.
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Password { get; set; }
        public string SMtpServer { get; set; }

        // Datos del serv Zyan.
        public string ZyanServerName { get; set; }
        public int ZyanServerPort { get; set; }

        // Datos del serv Udp.
        public int UdpServerPort { get; set; }
        public string ServerIpAdr { get; set; }

        // Timeout inicial. Tiempo que se espera por un cliente que se comienza a monitorear
        // o que ha sido reiniciado.  Valor en segundos. Rango 1.5 a 2.5 minutos.
        // debe ser mayor que el timeout de operacion (30 - 45 segs). Default 90 segundos.
        public int TimeoutStartRestart { get; set; }

        // Limite de reinicios. Cantidad de veces que se reinicia un cliente que no envia HB
        // antes de declararlo Fuera de control. Default 2.
        public int RestartAttemps { get; set; }
    }


}
