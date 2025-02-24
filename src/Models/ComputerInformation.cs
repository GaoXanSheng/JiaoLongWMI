using System.Management;
using System.Text.Json.Nodes;

namespace JiaoLongWMI.Models;

public class ComputerInformation
{
    public static JsonObject GetBiosInfo()
    {
        var res = new JsonObject();
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
        foreach (ManagementObject obj in searcher.Get())
        {
            // 获取 BIOS 版本
            res["Version"] = obj["Caption"].ToString();
        }
        return res;
    }
    public static JsonObject GetCpuTemperature()
    {
        var res = new JsonObject();
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_Counters_ThermalZoneInformation");
        foreach (ManagementObject obj in searcher.Get())
        {
            if (Byte.TryParse(obj["Temperature"].ToString(),out byte Temperature))
            {
                // 转为摄氏度
                res["Temperature"] = (Temperature-273.15);
            }
        }
        return res;
    }
    public static JsonObject GetWindowsInfo()
    {
        var res = new JsonObject();
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
        foreach (ManagementObject obj in searcher.Get())
        {
            // 获取 BIOS 版本
            res["Caption"] = obj["Caption"].ToString();
            res["Version"] = obj["BuildNumber"].ToString();
        }
        return res;
    }
}