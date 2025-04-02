namespace JiaoLongWMI.tools;

public class WinRing0
{
    static readonly ushort EC_ADDR_PORT = 0x4E;
    static readonly ushort EC_DATA_PORT = 0x4F;

    private static Ols ols = new Ols();
    // copy https://github.com/GermanAizek/WinRing0/blob/master/samples/Cs/WinRing0Sample.cs
    public string librarySutatus()
    {
        // Check support library sutatus
        switch (ols.GetStatus())
        {
            case (uint)Ols.Status.NO_ERROR:
                break;
            case (uint)Ols.Status.DLL_NOT_FOUND:
                return ("Status Error!! DLL_NOT_FOUND");
            case (uint)Ols.Status.DLL_INCORRECT_VERSION:
                return ("Status Error!! DLL_INCORRECT_VERSION");
            case (uint)Ols.Status.DLL_INITIALIZE_ERROR:
                return ("Status Error!! DLL_INITIALIZE_ERROR");
        }
        
        // Check WinRing0 status
        switch (ols.GetDllStatus())
        {
            case (uint)Ols.OlsDllStatus.OLS_DLL_NO_ERROR:
                break;
            case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_LOADED:
                return ("DLL Status Error!! OLS_DRIVER_NOT_LOADED");
            case (uint)Ols.OlsDllStatus.OLS_DLL_UNSUPPORTED_PLATFORM:
                return ("DLL Status Error!! OLS_UNSUPPORTED_PLATFORM");
            case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_FOUND:
                return ("DLL Status Error!! OLS_DLL_DRIVER_NOT_FOUND");
            case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_UNLOADED:
                return("DLL Status Error!! OLS_DLL_DRIVER_UNLOADED");
            case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_LOADED_ON_NETWORK:
                return("DLL Status Error!! DRIVER_NOT_LOADED_ON_NETWORK");
            case (uint)Ols.OlsDllStatus.OLS_DLL_UNKNOWN_ERROR:
                return("DLL Status Error!! OLS_DLL_UNKNOWN_ERROR");
        }

        return ("DLL Status OK");
    }
    public static void WriteIoPortByte(ushort port, byte value)
    {
        ols.WriteIoPortByte(port, value);
    }

    public byte ReadIoPortByte(ushort port)
    {
        return ols.ReadIoPortByte(port);
    }

    public void ECRamWriteExt_Direct(ushort iIndex, byte data)
    {
        WriteIoPortByte(EC_ADDR_PORT, 0x2E);
        WriteIoPortByte(EC_DATA_PORT, 0x11);
        WriteIoPortByte(EC_ADDR_PORT, 0x2F);
        WriteIoPortByte(EC_DATA_PORT, (byte)(iIndex >> 8)); // 高字节

        WriteIoPortByte(EC_ADDR_PORT, 0x2E);
        WriteIoPortByte(EC_DATA_PORT, 0x10);
        WriteIoPortByte(EC_ADDR_PORT, 0x2F);
        WriteIoPortByte(EC_DATA_PORT, (byte)(iIndex & 0xFF)); // 低字节

        WriteIoPortByte(EC_ADDR_PORT, 0x2E);
        WriteIoPortByte(EC_DATA_PORT, 0x12);
        WriteIoPortByte(EC_ADDR_PORT, 0x2F);
        WriteIoPortByte(EC_DATA_PORT, data);
    }

    public byte ECRamReadExt_Direct(ushort iIndex)
    {
        WriteIoPortByte(EC_ADDR_PORT, 0x2E);
        WriteIoPortByte(EC_DATA_PORT, 0x11);
        WriteIoPortByte(EC_ADDR_PORT, 0x2F);
        WriteIoPortByte(EC_DATA_PORT, (byte)(iIndex >> 8)); // 高字节

        WriteIoPortByte(EC_ADDR_PORT, 0x2E);
        WriteIoPortByte(EC_DATA_PORT, 0x10);
        WriteIoPortByte(EC_ADDR_PORT, 0x2F);
        WriteIoPortByte(EC_DATA_PORT, (byte)(iIndex & 0xFF)); // 低字节

        WriteIoPortByte(EC_ADDR_PORT, 0x2E);
        WriteIoPortByte(EC_DATA_PORT, 0x12);
        WriteIoPortByte(EC_ADDR_PORT, 0x2F);

        // 读取 EC 数据
        return ReadIoPortByte(EC_DATA_PORT);
    }
}