#region Descripción
/*
    Implementa informacin de terminación de tareas.
*/
#endregion

#region Using

#endregion

namespace Monitor.Shared.Utilities
{
    public class TaskResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
