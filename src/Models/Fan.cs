using System.Text.Json.Nodes;
using JiaoLongWMI.tools;
using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLongWMI.Models;

public class Fan
{
    public static string SetFanSpeed(string hexSpeed)
    {
        if (!byte.TryParse(hexSpeed, System.Globalization.NumberStyles.HexNumber, null, out byte speed))
        {
            return "Invalid Hex Input";
        }
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
        winRing0.Dispose();
        return "Fan Speed Set OK";
    }
    public static JsonObject GetFanLevel()
    {
        var res = new JsonObject();
        var winRing0 = new WinRing0();
        var librarySutatus = winRing0.librarySutatus();
        if (librarySutatus != "DLL Status OK")
        {
            res["msg"] =  librarySutatus;
            return res;
        }
        ushort fan1RpmLevel = 0xC836;
        ushort fan2RpmLevel = 0xC837;
        res["fan1RpmLevel"] =  winRing0.ECRamReadExt_Direct(fan1RpmLevel);
        res["fan2RpmLevel"] =  winRing0.ECRamReadExt_Direct(fan2RpmLevel);
        winRing0.Dispose();
        res["msg"] =  "Fan Speed Set OK";
        return res;
    }
    public static string SetFanLevel(string hexSpeed)
    {
        if (!byte.TryParse(hexSpeed, System.Globalization.NumberStyles.HexNumber, null, out byte level))
        {
            return "Invalid Hex Input";
        }
        if (level>10)
        {
            return "Fan Level Error";
        }
        var winRing0 = new WinRing0();
        var librarySutatus = winRing0.librarySutatus();
        if (librarySutatus != "DLL Status OK")
        {
            return librarySutatus;
        }
        ushort fan1RpmLevel = 0xC836;
        ushort fan2RpmLevel = 0xC837;
        winRing0.ECRamWriteExt_Direct(fan1RpmLevel, level);
        winRing0.ECRamWriteExt_Direct(fan2RpmLevel, level);
        winRing0.Dispose();
        return "Fan Level Set OK";
    }
    public static bool SetMaxFanSpeedSwitch(string set)
    {
        if (set == "1")
        {
            return MethodServices.SetValue(MethodName.MaxFanSpeedSwitch, (byte)1);
        }
        else
        {
            return MethodServices.SetValue(MethodName.MaxFanSpeedSwitch, (byte)0);
        }
    }
    public static bool GetMaxFanSpeedSwitch()
    {
            return MethodServices.GetValue<bool>(MethodName.MaxFanSpeedSwitch);
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