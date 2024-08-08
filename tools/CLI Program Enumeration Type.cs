using JiaoLong16Pro.BLD.WMIOperation;
using JiaoLong16Pro.Models;
using JiaoLong16Pro.server;

namespace JiaoLong16Pro;

public class CLI_Program_Enumeration_Type
{
    public string eumType(string type, string[] args)
    {
        switch (type)
        {
            case "CPU": return CpuType(args);
            case "Fan": return FanType(args);
            case "GPU": return GPUType(args);
            case "Keyboard": return KeyboardType(args);
            case "LogoLight": return LogoLightType(args);
            case "PerformaceMode": return PerformaceModeType(args);
            case "System": return SystemType(args);
            default: return "false";
        }
    }

    private string CpuType(string[] args)
    {
        switch (args[0])
        {
            case "SetCpuShortPower": return CPU.SetCpuShortPower(byte.Parse(args[1])).ToString();
            case "SetCpuLongPower": return CPU.SetCpuLongPower(byte.Parse(args[1])).ToString();
            case "SetCPUTempWall": return CPU.SetCPUTempWall(byte.Parse(args[1])).ToString();
            default: return "false";
        }
    }

    private string FanType(string[] args)
    {
        switch (args[0])
        {
            case "SwitchMaxFanSpeed": return Fan.CLISetSwitchMaxFanSpeed(byte.Parse(args[1])).ToString();
            case "GetFanSpeed": return Fan.GetFanSpeed().ToString();
            case "SetFanSpeed": return Fan.SetFanSpeed(byte.Parse(args[1])).ToString();
            case "GetSwitchMaxFanSpeed": return Fan.GetSwitchMaxFanSpeed().ToString();
            default: return "false";
        }
    }

    private string GPUType(string[] args)
    {
        switch (args[0])
        {
            case "SetGpuMode": return GPU.CLISetGpuMode(byte.Parse(args[1])).ToString();
            case "GetGpuMode": return GPU.GetGpuMode().ToString();
            default: return "false";
        }
    }

    private string KeyboardType(string[] args)
    {
        switch (args[0])
        {
            case "GetRGBKeyboardColor":
                return Keyboard.GetRGBKeyboardColor();
                break;
            case "GetkeyboardLightBrightness":
                return Keyboard.GetkeyboardLightBrightness().ToString();
                break;
            case "GetKeyboardMode":
                return Keyboard.GetKeyboardMode().ToString();
                break;
            case "SetKeyboardMode":
                return Keyboard.SetKeyboardMode().ToString();
                break;
            case "SetRGBKeyboardColor":
                return Keyboard.SetRGBKeyboardColor(byte.Parse(args[1]), byte.Parse(args[2]), byte.Parse(args[3]))
                    .ToString();
            case "SetkeyboardLightBrightness":
                return Keyboard.SetkeyboardLightBrightness(byte.Parse(args[1])).ToString();
            default: return "false";
        }
    }

    private string LogoLightType(string[] args)
    {
        switch (args[0])
        {
            case "SetLogoLight":
                return LogoLight.CLISetLogoLight(byte.Parse(args[1])).ToString();
            case "GetLogoLight":
                return LogoLight.GetLogoLight().ToString();
            default: return "false";
        }
    }

    private string PerformaceModeType(string[] args)
    {
        switch (args[0])
        {
            case "SetPerformaceMode": return PerformaceMode.CLISetPerformaceMode(byte.Parse(args[1])).ToString();
            case "GetPerformaceMode": return PerformaceMode.GetPerformaceMode().ToString();
            default: return "false";
        }
    }

    private string SystemType(string[] args)
    {
        switch (args[0])
        {
            case "OpenCustomMode": return (SystemModels.OpenCustomMode()).ToString();
            case "GetACType": return (SystemModels.GetACType()).ToString();
            case "GetInfo": return (SystemModels.GetInfo());
            default: return "false";
        }
    }
}