using Monitor.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Monitor.Service.Output
{

    internal class FileLogOutput : IMessageOutput
    {

        #region Declaraciones

        #endregion

        #region Metodos Publicos 

        public void Write(string newMess, TraceEventType type)
        {
            WriteToLog( newMess,  type);
        }

        #endregion

        #region Metodos Privados
        private void WriteToLog(string newMess, TraceEventType type)
        {

        }

        #endregion

    }
}
