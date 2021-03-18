#region Descripción
/*
     Implementa la salida por consola del sistema escritura de mensajes en consola 
    manteniendo el mensaje base.
*/
#endregion

#region Using
using System;
using Monitor.Shared;
using System.Diagnostics;
using Monitor.Service.Interfaces;
#endregion

namespace Monitor.Service.Output
{
    /// <summary>
    /// Implementa escritura de mensajes en consola manteniendo el mensaje base.
    /// Parcial.
    /// </summary>
    internal class ConsoleOutput : IMessageOutput
    {
        #region Declaraciones
        // Bool flag para mostrar mensajes tipo verbose por consola.
        private bool _outputVerbose;

        //Utilitarios para escritura de mensajes en consola manteniendo el mensaje base
        private int _origRow;
        private int _origCol;
        private int _baseRow;
        private object _refLock = new object();

        // Diferenciar mensajes de servicio en modo consola de los demas: install, uninstall, interactive etc.
        private bool _writeBaseLine = false;

        private const string _baseMsg = "<presione Enter para terminar el servicio...>";
        private const string _baseClr = "                                                                                                                ";

        #endregion

        // Constructor
        public ConsoleOutput(bool verbose, bool baseLine)
        {

            _outputVerbose = verbose;
            _writeBaseLine = baseLine;

            Console.Clear();
            _origRow = Console.CursorTop;
            _origCol = Console.CursorLeft;
            _baseRow = 0;
        }

        #region Metodos Publicos 

        public void Write(string newMess, TraceEventType type)
        {
            
                if (!_outputVerbose && (type == TraceEventType.Verbose))
                    return;

                WriteAtBase(newMess, type);    
        }

        #endregion

        #region Metodos Privados
        private void WriteAtBase(string newMess, TraceEventType type)
        {

            ConsoleColor ccolor;
            lock (_refLock)
            {

                ccolor = Functions.GetConsoleColor(type);

                if (_baseRow > Console.BufferHeight)
                {
                    Console.Clear();
                    _baseRow = 1;
                }

                if (_writeBaseLine)
                {
                    WriteAt(_baseClr, 0, _baseRow);
                    Console.ForegroundColor = ccolor;
                    WriteAt(newMess, 1, _baseRow);
                    Console.ResetColor();
                    _baseRow++;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    WriteAt(_baseClr, 0, _baseRow);
                    WriteAt(_baseMsg, 0, _baseRow);
                    Console.ResetColor();
                }
                else
                {
                    WriteAt(_baseClr, 0, _baseRow);
                    Console.ForegroundColor = ccolor;  
                    WriteAt(newMess, 1, _baseRow);
                    Console.ResetColor();    
                    _baseRow++;
                }
            }
        }

        private void WriteAt(string s, int col, int row)
        {
            try
            {

                Console.SetCursorPosition(_origCol + col, _origRow + row);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        #endregion
    }
}
