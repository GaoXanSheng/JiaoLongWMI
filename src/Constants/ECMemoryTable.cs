namespace JiaoLongWMI.Constants;

public enum ECMemoryTable : UInt16
{
    EC_ADDR_PORT = 0x4E,
    EC_DATA_PORT = 0x4F,
    Fan1_RPM_Level = 0xC836,
    Fan2_RPM_Level = 0xC837,
    Fan1_RPM = 0XC834,
    Fan2_RPM = 0XC835,
    Fan1_RPM_SET = 0xC83C,
    Fan2_RPM_SET = 0xC83D,
    EC_Version = 0xC411,
}
