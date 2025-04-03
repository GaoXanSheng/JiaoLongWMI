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
        var librarySutatus = winRing0.LibrarySutatus();
        if (librarySutatus != "DLL Status OK")
        {
            return librarySutatus;
        }

        winRing0.ECRamWriteExt_Direct((ushort)ECMemoryTable.Fan1_RPM_SET, speed);
        winRing0.ECRamWriteExt_Direct((ushort)ECMemoryTable.Fan2_RPM_SET, speed);
        winRing0.Dispose();
        return "Fan Speed Set OK";
    }
    public static JsonObject GetFanLevel()
    {
        var res = new JsonObject();
        var winRing0 = new WinRing0();
        var librarySutatus = winRing0.LibrarySutatus();
        if (librarySutatus != "DLL Status OK")
        {
            res["msg"] =  librarySutatus;
            return res;
        }

        res["fan1RpmLevel"] =  winRing0.ECRamReadExt_Direct((ushort)ECMemoryTable.Fan1_RPM_Level);
        res["fan2RpmLevel"] =  winRing0.ECRamReadExt_Direct((ushort)ECMemoryTable.Fan2_RPM_Level);
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
        var librarySutatus = winRing0.LibrarySutatus();
        if (librarySutatus != "DLL Status OK")
        {
            return librarySutatus;
        }
        winRing0.ECRamWriteExt_Direct((ushort)ECMemoryTable.Fan1_RPM_Level, level);
        winRing0.ECRamWriteExt_Direct((ushort)ECMemoryTable.Fan2_RPM_Level, level);
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
    public static byte GetMaxFanSpeedSwitch()
    {
        return MethodServices.GetValue<byte>(MethodName.MaxFanSpeedSwitch);
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