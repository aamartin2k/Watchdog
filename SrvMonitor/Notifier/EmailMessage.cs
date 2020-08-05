using Monitor.Shared;
using System;
using System.Text;


namespace Monitor.Service
{

    internal abstract class EmailMessageBase
    {
        public abstract string Subject
        { get; }

        public abstract string Title
        { get; }

        public abstract string SubTitle
        { get; }

        public abstract string TimeDate
        { get; }
        public abstract string Application
        { get; }
        public abstract string Details
        { get; }

        public abstract DateTime RealTimeDate
        { get; set; }

        public abstract string ClientName
        { get; set; }
    }

    internal class PlainTextMessage : EmailMessageBase
    {

        // Constructor
        public PlainTextMessage(EMessageAction action, EMessageStyle style, string client, DateTime date)
        {
            Action = action;
            Style = style;
            RealTimeDate = date;
            ClientName = client;
        }

        public virtual string Body
        {
            get {
                    StringBuilder sb = new StringBuilder();
                
                    sb.AppendLine(Title);
                    sb.AppendLine(SubTitle);
                    sb.AppendLine(); 
                    sb.AppendLine();
                    sb.AppendLine(TimeDate);
                    sb.AppendLine(Application);
                    sb.AppendLine(Details);

                    return sb.ToString();
            }
        }

        public EMessageAction Action { get; set; }
        public EMessageStyle Style { get; set; }

        public override string Subject
        {
            get 
            {
                string action = string.Empty;
                switch (Action)
                {
                    case EMessageAction.SysStart:
                        action = "Servicio Iniciado";
                        break;
                    case EMessageAction.SysEnd:
                        action = "Servicio Detenido";
                        break;
                    case EMessageAction.Timeout:
                        action = "Aplicación Detenida";
                        break;
                    case EMessageAction.Restart:
                        action = "Aplicación Reiniciada";
                        break;
                    case EMessageAction.Dead:
                        action = "Aplicación Fuera de Control";
                        break;
                    case EMessageAction.Operational:
                        action = "Aplicación Ejecutándose Normalmente";
                        break;
                    case EMessageAction.Pause:
                        action = "Se ha pausado el monitoreo de la Aplicación";
                        break;
                    case EMessageAction.Resume:
                        action = "Se continua el monitoreo de la Aplicación";
                        break;
                    default:
                        break;
                }

                // Parte constante del subject
                // Aviso del sistema de Monitoreo: <action>
                 
                return string.Format("Aviso del sistema de Monitoreo: {0}",   action) ;

            }
        }

        public override string Title
        {
            get { return "Aviso del sistema de Monitoreo"; }
        }

        public override string SubTitle
        {
            get 
            {
                string style = string.Empty;

                switch (Style)
                {
                    case EMessageStyle.Info:
                        style = "Información";
                        break;
                    case EMessageStyle.Alert:
                        style = "Alerta";
                        break;
                    case EMessageStyle.Alarm:
                        style = "Alarma";
                        break;
                    default:
                        break;
                }

                return style;
            }
        }

        public override string TimeDate
        {
            get {
                return string.Format("Hora y Fecha: {0}", RealTimeDate.ToString("hh:mm tt  dd-MMMM-yyyy") );
            }
        }

        public override string Application
        {
            get { return string.Format("Aplicación: {0}", ClientName)  ; }
        }

        public override string Details
        {
            get 
            {
                string action = string.Empty;
                switch (Action)
                {
                    case EMessageAction.SysStart:
                        action = "El servicio de monitoreo se ha iniciado normalmente.";
                        break;
                    case EMessageAction.SysEnd:
                        action = "El servicio de monitoreo se ha detenido.";
                        break;
                    case EMessageAction.Timeout:
                        action = "La aplicación no reporta en el tiempo especificado. Se intentará reiniciarla.";
                        break;
                    case EMessageAction.Restart:
                        action = "La aplicación se ha reiniciado. Se continuará su monitoreo.";
                        break;
                    case EMessageAction.Dead:
                        action = "La aplicación se ha detenido y no se puede reiniciar. Se requiere intervención humana inmediata.";
                        break;
                    case EMessageAction.Operational:
                        action = "La aplicación se ejecuta normalmente.";
                        break;
                    case EMessageAction.Pause:
                        action = "Se ha pausado el monitoreo de la Aplicación mediante una acción administrativa. La aplicación se ejecuta normalmente.";
                        break;
                    case EMessageAction.Resume:
                        action = "Se continua el monitoreo de la Aplicación mediante una acción administrativa. La aplicación se ejecuta normalmente.";
                        break;
                    default:
                        break;
                }

                // Parte constante del detail
                return string.Format("Detalles: {0}", action);
            }
        }

        public override DateTime RealTimeDate
        { get;  set; }

        public override string ClientName
        { get; set; }
    }

    internal class HTMLMessage : PlainTextMessage
    {
        public HTMLMessage(EMessageAction action, EMessageStyle style, string client, DateTime date)
            :base (action, style, client, date)
        { }


        public override string Body
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                //sb.AppendLine("<body lang=\"es-ES\" dir=\"ltr\">");
                sb.AppendLine(Title);
                sb.AppendLine(SubTitle);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine(TimeDate);
                sb.AppendLine(Application);
                sb.AppendLine(Details);
                //sb.AppendLine("</body>");
                return sb.ToString();
            }
        }

        public override string Title
        {
            get
            {
                string text = "<p style=\"margin-bottom: 0in; page-break-before: always\">" +
                              "{0}<font face=\"Arial, sans-serif\"><font size=\"4\" style=\"font-size: 14pt\">" +
                              "<b>{1}</b></font></font></p>" ; 
                // Insert color tag
                return string.Format(text, FontColor, base.Title);
            }
        }

        public override string SubTitle
        {
            get
            {
                string text = "<p>{0}<font face=\"Arial, sans-serif\"><font size=\"3\" style=\"font-size: 12pt\">{1}</font>" +
                              "</font></p><p style=\"margin-bottom: 0in\"><br/>";

                // Insert color tag
                return string.Format(text, FontColor, base.SubTitle);
            }
        }

        public override string Application
        {
            get
            {
                string text = "<p style=\"margin-bottom: 0in\"><font color=\"black\"><font face=\"Arial, sans-serif\"><font size=\"2\" style=\"font-size: 11pt\">" +
                              "Aplicaci&oacute;n: {0}</font></font></p>";

                return string.Format(text, ClientName);
            }
        }

        public override string TimeDate
        {
            get
            {
                string text = " <p style=\"margin-bottom: 0in\"><font color=\"black\"><font face=\"Arial, sans-serif\"><font size=\"2\" style=\"font-size: 11pt\">" +
                              "{0}</font></font></font></p>";
                //return string.Format(text, base.TimeDate);
                return string.Format(text, base.TimeDate);
            }
        }

        public override string Details
        {
            get
            {
                string text = " <p style=\"margin-bottom: 0in\"><font color=\"black\"><font face=\"Arial, sans-serif\"><font size=\"2\" style=\"font-size: 11pt\">" +
                              "{0}</font></font></p>";
                return string.Format(text, base.Details);
            }
        }

        private string FontColor 
        {
            get 
            {
                string text = string.Empty;

                switch (Style)
                {
                    case EMessageStyle.Info:
                        text = "<font color=\"black\">";
                        break;
                    case EMessageStyle.Alert:
                        //text = "<font color=\"#ffcc00\">";
                        text = "<font color=\"yellow\">";
                        break;
                    case EMessageStyle.Alarm:
                        //text = "<font color=\"#ff3333\">";
                        text = "<font color=\"red\">";
                        break;
                    default:
                        break;

                }
                return text;
            }
        }
    }

}
