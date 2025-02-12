

using JiaoLongWMI.WMIOperation;
using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLongWMI.Models;

public class Fan
{
    /**
     * 切换BIOS接管运行
     */
    public static bool SwitchMaxFanSpeed(ResultState m)
    {
        return  MethodServices.SetValue(MethodName.MaxFanSpeedSwitch, m);
    }
    public static bool CLISetSwitchMaxFanSpeed(byte b)
    {
        if (b == 1)
        {
           return SwitchMaxFanSpeed(ResultState.ON);
        }
        else if (b == 0)
        {
            return SwitchMaxFanSpeed(ResultState.OFF);
        }
        else
        {
            return SwitchMaxFanSpeed(ResultState.OFF);
        }

    }
    public static byte GetSwitchMaxFanSpeed()
    {
        return MethodServices.GetValue<byte>(MethodName.MaxFanSpeedSwitch);
    }
    public static bool SetFanSpeed(byte speed)
    {
       return MethodServices.SetValue(MethodName.MaxFanSpeed, speed);
    }

    public static string GetFanSpeed()
    {
        Tuple<int, int> CPUGPUFanSpeed = MethodServices.GetValue<Tuple<int, int>>(MethodName.CPUGPUFanSpeed);
        return $"CPU-{CPUGPUFanSpeed.Item1}-GPU-{CPUGPUFanSpeed.Item2}";
    }
}
