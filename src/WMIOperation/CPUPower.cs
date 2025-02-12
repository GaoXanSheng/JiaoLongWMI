namespace JiaoLongWMI.WMIOperation
{
    public enum CPUPower : byte
    {
        CloseState = 0,
        OpenState,
        SPLState,
        SPPTState,
        CPUTempWallState,
        Unknow = 255
    }
}
