using JiaoLong16Pro.BLD.WMIOperation;

namespace JiaoLong16Pro.Models;

public class Fan
{
    /**
     * 切换BIOS接管运行
     */
    public static bool SwitchMaxFanSpeed(WMIResultState m)
    {
        return  WMIMethodServices.SetValue(WMIMethodName.MaxFanSpeedSwitch, m);
    }
    public static bool CLISetSwitchMaxFanSpeed(byte b)
    {
        if (b == 1)
        {
           return SwitchMaxFanSpeed(WMIResultState.ON);
        }
        else if (b == 0)
        {
            return SwitchMaxFanSpeed(WMIResultState.OFF);
        }
        else
        {
            return SwitchMaxFanSpeed(WMIResultState.OFF);
        }

    }
    public static byte GetSwitchMaxFanSpeed()
    {
        return WMIMethodServices.GetValue<byte>(WMIMethodName.MaxFanSpeedSwitch);
    }
    public static bool SetFanSpeed(byte speed)
    {
       return WMIMethodServices.SetValue(WMIMethodName.MaxFanSpeed, speed);
    }

    public static string GetFanSpeed()
    {
        Tuple<int, int> CPUGPUFanSpeed = WMIMethodServices.GetValue<Tuple<int, int>>(WMIMethodName.CPUGPUFanSpeed);
        return $"CPU-{CPUGPUFanSpeed.Item1}-GPU-{CPUGPUFanSpeed.Item2}";
    }
}
