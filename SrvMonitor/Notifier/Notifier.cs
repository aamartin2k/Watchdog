#region Descripción
/*
    Implementa el envío de notificaciones mediante varios medios.
    Se notifican cambios en la operacion del sistema, cambios de estado de los clientes
    monitoreados y errores.
*/
#endregion

#region Using

using Monitor.Shared;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;

#endregion

namespace Monitor.Service
{
    public class Notifier
    {
        #region Declaraciones  

        // ref a System Config
        private string _sMtpServer;
        private string _source;
        private string _destination;
        private string _password;

        private SmtpClient _smtpClient;

        // Para uso de Log.
        private const string ClassName = "Notifier";

        #endregion

        #region Propiedades

        #region Propiedades Públicas

        #endregion

        #region Propiedades Privadas

        #endregion

        #endregion

        #region Métodos

        #region Métodos Públicos

        internal void Start(RequestStart req)
        {
            Builder.Output("Iniciando Notifier.");
            StartComponent();
            Builder.Output("Notifier iniciado.");
            
        }

        internal void Stop(RequestStop req)
        {
            Builder.Output("Deteniendo Notifier.");
            StopComponent(); 
            Builder.Output("Notifier detenido.");
        }

        #region Métodos Request Handlers


        public void ReceiveSystemConfig(SendSystemConfig req)
        {
           
            _sMtpServer = req.Data.SMtpServer;
            _source = req.Data.Source;
            _destination = req.Data.Destination;
            _password = req.Data.Password;

            Builder.Output(ClassName + ": recibidos datos de configuracion de sistema.", TraceEventType.Information);
        }

        internal void DoSendEmail(RequestSendEmail req)
        {
            if (req.Client == null)
                SendMail(req.Action, req.Date);
            else
                SendMail(req.Action, req.Date, req.Client);
        }

        #endregion
        
     
        #endregion

        #region Métodos Privados

        private void StartComponent()
        {
            // debug
            //MessageBus.Send(new ReplyError("Debug Error."));
            //return;

            // Creando cliente de correo
            _smtpClient = new SmtpClient(_sMtpServer);
            _smtpClient.Credentials = new NetworkCredential(_source, _password);

            // Intentar conexion con SMTP server
            if (SendTestMail())
                MessageBus.Send(new ReplyOK());
     
        }

        private void StopComponent()
        {
            _smtpClient = null;
            MessageBus.Send(new ReplyStop());
        }


        #region SMTP Email

        private void SendMail(EMessageAction action, DateTime date)
        {
            // Para notificar eventos del sistema de monitoreo y no de los clientes
            if (action == EMessageAction.SysStart | action == EMessageAction.SysEnd)
            {
                SendMail(action, EMessageStyle.Info, "Sistema de Monitoreo", date);
            }
        }


        private void SendMail(EMessageAction action , DateTime date, ClientData client)
        {
            // incorporar el estilo en dependencia de la accion
            EMessageStyle style;

            switch (action)
            {
                case EMessageAction.Timeout:
                case EMessageAction.Restart:
                    style = EMessageStyle.Alert;
                    break;

                case EMessageAction.Dead:
                    style = EMessageStyle.Alarm;
                    break;

                case EMessageAction.SysStart:
                case EMessageAction.SysEnd:
                case EMessageAction.Operational:
                case EMessageAction.Pause:
                case EMessageAction.Resume:
                default:
                    style = EMessageStyle.Info;
                    break;
            }

            SendMail(action, style, client, date);
        }


        private void SendMail(EMessageAction action, EMessageStyle style, ClientData client, DateTime date)
        {
            PlainTextMessage mess = new PlainTextMessage(action, style, client.Name, date);
            HTMLMessage htmmess = new HTMLMessage(action, style, client.Name, date);

            MailMessage mailMessage = new MailMessage();

            // Direciones
            mailMessage.From = new MailAddress(_source);
            mailMessage.To.Add(_destination);

            // Asunto
            mailMessage.Subject = mess.Subject;
            // Cuerpo en texto plano
            mailMessage.Body = mess.Body;

            // Add HTML View
            AlternateView altHtml = AlternateView.CreateAlternateViewFromString(htmmess.Body, null, "text/html");
            mailMessage.AlternateViews.Add(altHtml);

            // Incorporar adjunto si esta configurado en el cliente
            // y es una accion Timeout o Dead
            bool UseAttach = client.LogAttachEnabled && (action == EMessageAction.Timeout) || (action == EMessageAction.Dead);

            FileStream fs = null;

            if (UseAttach && File.Exists(client.LogFilePath))
            {
                fs = new FileStream(client.LogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                mailMessage.Attachments.Add(new Attachment(fs, "Archivo Log", "text/plain"));

            }

            try
            {
                EnviaEmail(mailMessage);
            }
            
            catch (Exception ex)
            {
                Builder.Output(string.Format("{0}.SendMail: Ocurrió una excepción: {1}", ClassName, ex.Message), TraceEventType.Error);
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }


        private void SendMail(EMessageAction action, EMessageStyle style, string name, DateTime date)
        {
            PlainTextMessage mess = new PlainTextMessage(action, style, name, date);
            HTMLMessage htmmess = new HTMLMessage(action, style, name, date);

            MailMessage mailMessage = new MailMessage();

            // Direciones
            mailMessage.From = new MailAddress(_source);
            mailMessage.To.Add( _destination )  ;

            // Asunto
            mailMessage.Subject = mess.Subject;
            // Cuerpo en texto plano
            mailMessage.Body = mess.Body;

            // Add HTML View
            AlternateView altHtml = AlternateView.CreateAlternateViewFromString(htmmess.Body, null, "text/html");
            mailMessage.AlternateViews.Add(altHtml);

            try
            {
                EnviaEmail(mailMessage);
            }

            catch (Exception ex)
            {
                Builder.Output(string.Format("{0}.SendMail: Ocurrió una excepción: {1}", ClassName, ex.Message), TraceEventType.Error);
                throw ex;
            }

        }

        private void EnviaEmail(MailMessage email)
        {
#if DEBUG || TEST
            EnviaEmailaArchivo(email);
#else
            _smtpClient.Send(email);
#endif
        }

        private void EnviaEmail(string from, string recipients, string subject, string body)
        {
#if DEBUG || TEST
            EnviaEmailaArchivo(from, recipients, subject, body);
#else
            _smtpClient.Send(from, recipients, subject, body);
#endif
        }


#if DEBUG || TEST

        static public string EmailFile { get { return "EmailaArchivo.txt"; } }

        // Para depuracion y prueba, en lugar de enviar mensajes a un servidor SMTP se
        // escribe a un archivo de texto.
        private void EnviaEmailaArchivo(MailMessage email)
        {

            using (StreamWriter w = File.AppendText(EmailFile))
            {
                w.WriteLine("From: " + email.From);
                w.WriteLine("To: " + email.To);
                w.WriteLine("Subject: " + email.Subject);
                w.WriteLine("Att count: " + email.Attachments.Count);
                w.WriteLine("Body: \n" + email.Body);
                w.WriteLine("------------------------------------\n");
            }
        }

        private void EnviaEmailaArchivo(string from, string recipients, string subject, string body)
        {

            using (StreamWriter w = File.AppendText(EmailFile))
            {
                w.WriteLine("From: " + from);
                w.WriteLine("To: " + recipients);
                w.WriteLine("Subject: " + subject);
                w.WriteLine("Att count: 0" );
                w.WriteLine("Body: \n" + body);
                w.WriteLine("------------------------------------\n");
            }
        }

#endif

#endregion

        private bool SendTestMail()
        {
            try
            {
                
                string from = _source;
                string recipients = _destination;
                string subject = "Mensaje de prueba.";
                string body = "Este es un mensaje de prueba del sistema de monitoreo.";

                EnviaEmail(from, recipients, subject, body);

                return true;
            }
            catch (Exception ex)
            {
                MessageBus.Send(new ReplyError(ex.Message));
                return false;
            }
        }

#endregion

#endregion

#region Testing
        /*  La realizacion de los test requiere ejecucion directa de los metodos. Esto va 
           en contra de la arquitectura del sistema, basada en intercambio de mensajes entre componentes.
           Para realizar los test se implementan metodos que exponen de modo publico otros metodos 
           privados del componente. Se emplea compilacion condicional para limitar la generación de
           estos metodos.
           Se requiere emplear la configuracion de proyecto denominada Test, donde se define el
           simbolo TEST.
        */
#if TEST
        // Metodos implementados para facilitar las pruebas
        // Gestion del componente
        public void Start()
        { StartComponent(); }

        public void Stop()
        { StopComponent(); }

        public SmtpClient SmtpClient { get { return _smtpClient; } }

        public void SendEmail(RequestSendEmail req)
        { DoSendEmail(req); }

        
      
#endif

#endregion

    }
}
