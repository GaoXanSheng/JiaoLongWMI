namespace JiaoLong16Pro.BLD.WMIOperation
{
    public enum WMICPUPower : byte
    {
        CloseState = 0,
        OpenState,
        SPLState,
        SPPTState,
        CPUTempWallState,
        Unknow = 255
    }
}
