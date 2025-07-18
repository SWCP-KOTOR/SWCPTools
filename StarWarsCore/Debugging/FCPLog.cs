namespace SWCP.Core;

public static class SWCPLog
{
    private const string ErrorPrefix = "<color=#7F66FFFF>[SWCP Core/Tools] </color>";
    private const string WarnPrefix = "<color=#B266FFFF>[SWCP Core/Tools] </color>";
    private const string MsgPrefix = "<color=#66ff7fFF>[SWCP Core/Tools] </color>";
    
    public static void Error(string msg)
    {
        Log.Error(ErrorPrefix + msg);
    }

    public static void Warning(string msg)
    {
        Log.Warning(WarnPrefix + msg);
    }

    public static void Message(string msg)
    {
        Log.Message(MsgPrefix + msg);
    }
}