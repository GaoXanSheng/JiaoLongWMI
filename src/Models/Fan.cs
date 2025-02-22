using System.Text.Json.Nodes;
using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLongWMI.Models;

public class Fan
{
    [Obsolete("This method is obsolete. Please use EC_Controller instead.")]
    public static bool SetFanSpeed(byte speed)
    {
        return MethodServices.SetValue(MethodName.MaxFanSpeed, speed);
    }

    public static JsonObject GetFanSpeed()
    {
        var res = new JsonObject();
        Tuple<int, int> CPUGPUFanSpeed = MethodServices.GetValue<Tuple<int, int>>(MethodName.CPUGPUFanSpeed);
        res["CPUFanSpeed"] = CPUGPUFanSpeed.Item1;
        res["GPUFanSpeed"] = CPUGPUFanSpeed.Item2;
        return res;
    }
}