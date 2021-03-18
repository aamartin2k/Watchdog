namespace Monitor.Shared
{

    public enum EMessageAction { SysStart, SysEnd, Timeout, Restart, Dead, Operational, Pause, Resume }

    public enum EMessageStyle { Info, Alert, Alarm }

    public enum ClientStatus { Default, Inicial, Operacional, Pausado, Reiniciado, Atrasado, Muerto }

    public enum EditFormMode { Default, CreateClient, EditClient, DeleteClient, EditSystem, Wizard, Supervisor }

    public enum SupervisorLoginType : short
    {
        Logon = 1,
        Logoff = 2
    }

    public enum ClientIdType { KeyByUdpPort, KeyByIdString, KeyByPipe }

    public enum HeartbeatSenderType { SenderUdp, SenderPipe }
}