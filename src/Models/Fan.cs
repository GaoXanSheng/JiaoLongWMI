using System.Text.Json.Nodes;
using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLongWMI.Models;

public class Fan
{
    [Obsolete("This method is obsolete.")]
    public static bool SetFanSpeed(byte speed)
    {
        return MethodServices.SetValue(MethodName.MaxFanSpeed, speed);
    }
    [Obsolete("This method is obsolete.")]
    public static bool SetMaxFanSpeedSwitch(bool speed)
    {
        if (speed)
        {
            return MethodServices.SetValue(MethodName.MaxFanSpeedSwitch, 1);
        }
        else
        {
            return MethodServices.SetValue(MethodName.MaxFanSpeedSwitch, 0);
        }
    }
    [Obsolete("This method is obsolete.")]
    public static JsonObject GetFanSpeed()
    {
        var res = new JsonObject();
        Tuple<int, int> CPUGPUFanSpeed = MethodServices.GetValue<Tuple<int, int>>(MethodName.CPUGPUFanSpeed);
        res["CPUFanSpeed"] = CPUGPUFanSpeed.Item1;
        res["GPUFanSpeed"] = CPUGPUFanSpeed.Item2;
        return res;
    }
}