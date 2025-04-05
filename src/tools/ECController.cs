namespace JiaoLongWMI.tools;

public class ECController
{

    private Ols _ols;

    public ECController()
    {
       _ols = new Ols();
    }
    public void Dispose()
    {
        _ols.Dispose();
    }
    // copy https://github.com/GermanAizek/WinRing0/blob/master/samples/Cs/WinRing0Sample.cs
    public string LibrarySutatus()
    {
        // Check support library sutatus
        switch (_ols.GetStatus())
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
        switch (_ols.GetDllStatus())
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

    public bool Fan1SetSpeed(byte speed)
    {
	    var librarySutatus = LibrarySutatus();
	    if (librarySutatus != "DLL Status OK")
	    {
		    return false;
	    }
	    WriteByte((ushort)ECMemoryTable.Fan1_RPM_SET, speed);
	    uint mask = ReadUint((char)0xB20);
	    mask |= 0x08;
	    WriteUint((char)0xB20, mask);
	    return true;
    }
    public bool Fan2SetSpeed(byte speed)
    {
	    var librarySutatus = LibrarySutatus();
	    if (librarySutatus != "DLL Status OK")
	    {
		    return false;
	    }
	    WriteByte((ushort)ECMemoryTable.Fan2_RPM_SET,speed);
	    uint mask = ReadUint((char)0xB20);
	    mask |= 0x08;
	    WriteUint((char)0xB20, mask);
	    return true;
    }

    private void ECRamDataStaging(ushort iIndex)
    {
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_ADDR_PORT, 0x2E);
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_ADDR_PORT, 0x11);
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_ADDR_PORT, 0x2F);
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_DATA_PORT, (uint)(iIndex >> 8)); // 高字节
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_ADDR_PORT, 0x2E);
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_DATA_PORT, 0x10);
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_ADDR_PORT, 0x2F);
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_DATA_PORT, (uint)(iIndex & 0xFF)); // 低字节
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_ADDR_PORT, 0x2E);
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_DATA_PORT, 0x12);
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_ADDR_PORT, 0x2F);
    }
    public void WriteUint(ushort iIndex, uint data)
    {
	    ECRamDataStaging(iIndex);
	    _ols.WriteIoPortDword((ushort)ECMemoryTable.EC_DATA_PORT, data);
    }
    public void WriteByte(ushort iIndex, byte data)
    {
	    ECRamDataStaging(iIndex);
	    _ols.WriteIoPortByte((ushort)ECMemoryTable.EC_DATA_PORT, data);
    }
    public byte ReadByte(ushort iIndex)
    {
	    ECRamDataStaging(iIndex);
	    return _ols.ReadIoPortByte((ushort)ECMemoryTable.EC_DATA_PORT);
    }
    public ushort ReadUshort(ushort iIndex)
    {
	    ECRamDataStaging(iIndex);
	    return _ols.ReadIoPortWord((ushort)ECMemoryTable.EC_DATA_PORT);
    }
    public uint ReadUint(ushort iIndex)
    {
				ECRamDataStaging(iIndex);
				return _ols.ReadIoPortDword((ushort)ECMemoryTable.EC_DATA_PORT);
    }
}
