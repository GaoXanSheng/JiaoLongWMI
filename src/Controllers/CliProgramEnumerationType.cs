using System.Text.Json.Nodes;
using JiaoLongWMI.Models;
using JiaoLongWMI.WMIOperation;
using JiaoLongWMI.WMIOperation.System;
using GPUMode = JiaoLong16Pro.Models.GPUMode;

namespace JiaoLongWMI.tools;

public class CliProgramEnumerationType
{
    private JsonObject callBack = new JsonObject();
    public static string ErrMag = null;
    private RGBBreathingLightEffect RgbBreathingLightEffect = new RGBBreathingLightEffect();

    private void CLI_Cpu(string methodName, string[] args)
    {
        if (methodName == "SetCpuShortPower")
        {
            callBack["result"] = CPU.SetCpuShortPower(Convert.ToByte(args[0])).ToString();
        }

        if (methodName == "SetCpuLongPower")
        {
            callBack["result"] = CPU.SetCpuLongPower(Convert.ToByte(args[0])).ToString();
        }

        if (methodName == "OpenCustomMode")
        {
            callBack["result"] = CPU.OpenCustomMode(Convert.ToBoolean(args[0])).ToString();
        }

        if (methodName == "SetCPUTempWall")
        {
            callBack["result"] = CPU.SetCPUTempWall(Convert.ToByte(args[0])).ToString();
        }
    }

    private void CLI_RGBBreathingLightEffect(string methodName, string[] args)
    {
        if (methodName == "Start")
        {
            RgbBreathingLightEffect.Start();
        }

        if (methodName == "Stop")
        {
            RgbBreathingLightEffect.Stop();
        }
    }

    private void CLI_Keyboard(string methodName, string[] args)
    {
        if (methodName == "ColorSet")
        {
            byte RedParseResult;
            byte GreenParseResult;
            byte BlueParseResult;
            if (byte.TryParse(args[0], out RedParseResult) && byte.TryParse(args[1], out GreenParseResult) &&
                byte.TryParse(args[2], out BlueParseResult))
            {
                callBack["result"] = Keyboard.Color.Set(RedParseResult, GreenParseResult, BlueParseResult);
            }
        }

        if (methodName == "ColorGet")
        {
            callBack["result"] = Keyboard.Color.Get();
        }

        if (methodName == "ModeSet")
        {
            callBack["result"] = Keyboard.Mode.Set();
        }

        if (methodName == "ModeGet")
        {
            callBack["result"] = Keyboard.Mode.Get().ToString();
        }

        if (methodName == "LightBrightnessGet")
        {
            callBack["result"] = Keyboard.LightBrightness.Get().ToString();
        }

        if (methodName == "LightBrightnessSet")
        {
            byte LightBrightness;
            bool success = byte.TryParse(args[0], out LightBrightness);
            if (success)
            {
                callBack["result"] = Keyboard.LightBrightness.Set(LightBrightness);
            }
        }
    }

    private void CLI_LogoLight(string methodName, string[] args)
    {
        if (methodName == "Set")
        {
            ResultState boolValue;
            bool success = Enum.TryParse(args[0], out boolValue);
            if (success)
            {
                callBack["result"] = LogoLight.Set(boolValue);
            }
        }

        if (methodName == "Get")
        {
            callBack["result"] = LogoLight.Get().ToString();
        }
    }

    private void CLI_GPUMode(string methodName, string[] args)
    {
        if (methodName == "Set")
        {
            JiaoLongWMI.WMIOperation.GPUMode GPUModeEnum;
            bool success = Enum.TryParse(args[0], out GPUModeEnum);
            if (success)
            {
                callBack["result"] = GPUMode.Set(GPUModeEnum).ToString();
            }
            else
            {
                callBack["result"] = false;
            }
        }

        if (methodName == "Get")
        {
            callBack["result"] = GPUMode.Get().ToString();
        }
    }

    private void CLI_PerformaceMode(string methodName, string[] args)
    {
        if (methodName == "Set")
        {
            bool success = Enum.TryParse(args[0], out SystemPerMode PerformaceModeEnum);
            if (success)
            {
                callBack["result"] = PerformaceMode.Set(PerformaceModeEnum);
            }
        }

        if (methodName == "Get")
        {
            callBack["result"] = PerformaceMode.Get().ToString();
        }
    }

    public string EumType(string typeName, string methodName, string[] args)
    {
        callBack["typeName"] = typeName;
        callBack["methodName"] = methodName;
        callBack["result"] = false;
        if (typeName == "CPU")
        {
            CLI_Cpu(methodName, args);
        }
        if (typeName=="GetHardwareMonitorInfo")
        {
	        callBack["result"] = ComputerInformation.GetHardwareMonitorInfo();
        }
        if (typeName == "Fan")
        {
            if (methodName == "GetFanSpeed")
            {
                callBack["result"] = Fan.GetFanSpeed();
            }

            if (methodName == "SetFanSpeed")
            {
                callBack["result"] = Fan.SetFanSpeed(args[0]);
            }

            if (methodName == "SetMaxFanSpeedSwitch")
            {
                callBack["result"] = Fan.SetMaxFanSpeedSwitch(args[0]);
            }

            if (methodName == "GetMaxFanSpeedSwitch")
            {
                callBack["result"] = Fan.GetMaxFanSpeedSwitch();
            }
        }

        if (typeName == "GPUMode")
        {
            CLI_GPUMode(methodName, args);
        }

        if (typeName == "LogoLight")
        {
            CLI_LogoLight(methodName, args);
        }

        if (typeName == "Keyboard")
        {
            CLI_Keyboard(methodName, args);
        }

        if (typeName == "PerformaceMode")
        {
            CLI_PerformaceMode(methodName, args);
        }

        if (typeName == "RGBBreathingLightEffect")
        {
            CLI_RGBBreathingLightEffect(methodName, args);
        }

        if (ErrMag != null)
        {
            callBack["msg"] = ErrMag;
            ErrMag = null;
        }

        return callBack.ToJsonString();
    }
}
