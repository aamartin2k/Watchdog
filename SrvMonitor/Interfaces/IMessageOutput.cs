

using System.Diagnostics;

namespace Monitor.Service.Interfaces
{
    internal interface IMessageOutput
    {
        void Write(string message, TraceEventType type);
    }
}
