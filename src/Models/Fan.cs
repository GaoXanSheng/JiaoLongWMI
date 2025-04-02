using System.Text.Json.Nodes;
using JiaoLongWMI.tools;
using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLongWMI.Models;

public class Fan
{
    public static string SetFanSpeed(byte speed)
    {
        var winRing0 = new WinRing0();
        var librarySutatus = winRing0.librarySutatus();
        if (librarySutatus != "DLL Status OK")
        {
            return librarySutatus;
        }
        ushort fan1SetRpmSet = 0xC83C;
        ushort fan2SetRpmSet = 0xC83D;
        winRing0.ECRamWriteExt_Direct(fan1SetRpmSet, speed);
        winRing0.ECRamWriteExt_Direct(fan2SetRpmSet, speed);
        return "Fan Speed Set OK";
    }
    
    public static bool SetMaxFanSpeedSwitch(byte set)
    {
        if (set == 1)
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