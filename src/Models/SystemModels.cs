
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;
using JiaoLongWMI.WMIOperation.Method;
using JiaoLongWMI.WMIOperation.System;

namespace JiaoLongWMI.Models;

public class SystemModels
{

    public static SystemACType GetACType()
    {
      return MethodServices.GetValue<SystemACType>(MethodName.SystemAcType);
    }
    public static JsonObject GetInfo()
    {
        double usage;
        int gputemp, speed, cputemp;
        double gpufreq, rate;
        SystemUsage.GetNvidiaGpuUsage(out usage, out gputemp, out gpufreq, out rate, out speed, out cputemp);
        JsonObject json = new JsonObject();
        json["usage"] = usage;
        json["gputemp"] = gputemp;
        json["gpufreq"] = gpufreq;
        json["rate"] = rate;
        json["speed"] = speed;
        json["cputemp"] = cputemp;
        return json;
        // Console.WriteLine($"GPU Usage: {usage}%, GPU Temp: {gputemp}°C, GPU Freq: {gpufreq} MHz, Rate: {rate}, Speed: {speed}, CPU Temp: {cputemp}°C");
    }

    private enum ACLineStatus : byte
    {
        Offline = 0,
        Online = 1,
        Unknown = 255,
    }
    private enum BatteryFlag : byte
    {
        High = 1,
        Low = 2,
        Critical = 4,
        Charging = 8,
        NoSystemBattery = 128,
        Unknown = 255,
    }
    private struct SystemPowerStatus
    {
        public ACLineStatus LineStatus;
        public BatteryFlag flgBattery;
        public byte BatteryLifePercent;
        public byte Reserved1;
        public int BatteryLifeTime;
        public int BatteryFullLifeTime;
    }
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetSystemPowerStatus(out SystemPowerStatus sps);

}