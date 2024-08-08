using JiaoLong16Pro.BLD.WMIOperation;

namespace JiaoLong16Pro.Models;

public class CPU
{
    /**
     * 低负载电压
     */
    public static bool SetCpuShortPower(byte LongPower)
    {
        return WMIMethodServices.SetValue(WMIMethodName.CPUPower, new byte[2]
        {
            (byte)WMICPUPower.SPLState,
            LongPower
        });

    }

    /**
     * 全核心满载电压
     */
    public static bool SetCpuLongPower(byte ShortPower)
    {
        return WMIMethodServices.SetValue(WMIMethodName.CPUPower, new byte[2]
        {
            (byte)WMICPUPower.SPPTState,
            ShortPower
        });
    }

    public static bool SetCPUTempWall(byte tempwall)
    {

        return WMIMethodServices.SetValue(WMIMethodName.CPUPower, new byte[2]
        {
            (byte)WMICPUPower.CPUTempWallState,
            tempwall
        });
    }
}