using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


using Monitor.Shared;


namespace Monitor.Service.Test
{
    [TestClass]
    public class NotifierTest
    {
        public NotifierTest()
        {
            //
            // TODO: Add constructor logic here}

            // access test
            
        }

        /// <summary>
        /// Comprueba el inicio del componente.
        /// </summary>
        [TestMethod]
        public void StartModuleTest()
        {
            Notifier _notif = new Notifier();
            // Eliminar archivo de email creado en prueba anteriores
            if (System.IO.File.Exists(Notifier.EmailFile))
                System.IO.File.Delete(Notifier.EmailFile);

            Assert.IsNull(_notif.SmtpClient);

            // Creando system config
            SystemConfigData syscfg =  new SystemConfigData();
            syscfg.SMtpServer = "smtp.test";
            syscfg.Source = "source@smtp.test";
            syscfg.Destination = "destination@smtp.test";
            syscfg.Password = "smtp.test.pwd";

            _notif.ReceiveSystemConfig(new SendSystemConfig(syscfg));

            _notif.Start();

            Assert.IsNotNull(_notif.SmtpClient);
            Assert.IsTrue(System.IO.File.Exists(Notifier.EmailFile));

            _notif.Stop();

            if (System.IO.File.Exists(Notifier.EmailFile))
                System.IO.File.Delete(Notifier.EmailFile);

        }

        /// <summary>
        /// Comprueba el envio simulado de mensajes.
        /// </summary>
        [TestMethod]
        public void SimulatedMailTest()
        {
            Notifier _notif = new Notifier();
            // Eliminar archivo de email creado en prueba anteriores
            if (System.IO.File.Exists(Notifier.EmailFile))
                System.IO.File.Delete(Notifier.EmailFile);

            // Creando system config
            SystemConfigData syscfg = new SystemConfigData();
            syscfg.SMtpServer = "smtp.test";
            syscfg.Source = "source@smtp.test";
            syscfg.Destination = "destination@smtp.test";
            syscfg.Password = "smtp.test.pwd";

            _notif.ReceiveSystemConfig(new SendSystemConfig(syscfg));

            _notif.Start();

            RequestSendEmail req = new RequestSendEmail(EMessageAction.SysStart, DateTime.Now);

            _notif.SendEmail(req);

            Assert.IsNotNull(_notif.SmtpClient);
            Assert.IsTrue(System.IO.File.Exists(Notifier.EmailFile));

            _notif.Stop();

            if (System.IO.File.Exists(Notifier.EmailFile))
                System.IO.File.Delete(Notifier.EmailFile);
        }


    }

}
