namespace Pavliks.WAM.ManagementConsole.Domain
{
   
    public enum StatusReasonSMSMessage
    {
        sent = 602300001,
        failed = 602300003,
        Open = 1,
        PendingSend = 602300000,
        Queued = 602300002,
        Unqueued = 602300004
    }

    public enum StateSMSMessage
    {
        Open = 0
    }
}


