using System;
using System.Text.Json.Nodes;
using JiaoLongWMI.Constants;
using JiaoLongWMI.Repositories;
using JiaoLongWMI.server;

namespace JiaoLongWMI.Controllers;

/// <summary>
/// 命令行程序枚举类型，用于根据传入的类型名称和方法名称，调用相应的方法。
/// </summary>
public class CliProgramEnumerationType
{
    // 用于存储回调结果的 JsonObject。
    private JsonObject callBack = new JsonObject();
    // 错误信息。
    public static string ErrMag = null;
    // RGB 呼吸灯效果控制器。
    private RGBBreathingLightEffect RgbBreathingLightEffect = new RGBBreathingLightEffect();

    /// <summary>
    /// 处理 CPU 相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <summary>
    /// 处理 CPU 相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <param name="args">参数。</param>
    private void CLI_Cpu(string methodName, string[] args)
    {
        switch (methodName)
        {
            case "SetCpuShortPower":
                if (args.Length > 0 && byte.TryParse(args[0], out byte shortPower))
                {
                    callBack["result"] = CPU.SetCpuShortPower(shortPower).ToString();
                }
                else
                {
                    callBack["result"] = false;
                    callBack["msg"] = "参数错误：SetCpuShortPower 需要一个 byte 类型的参数。";
                }
                break;
            case "SetCpuLongPower":
                if (args.Length > 0 && byte.TryParse(args[0], out byte longPower))
                {
                    callBack["result"] = CPU.SetCpuLongPower(longPower).ToString();
                }
                else
                {
                    callBack["result"] = false;
                    callBack["msg"] = "参数错误：SetCpuLongPower 需要一个 byte 类型的参数。";
                }
                break;
            case "OpenCustomMode":
                if (args.Length > 0 && bool.TryParse(args[0], out bool openCustomMode))
                {
                    callBack["result"] = CPU.OpenCustomMode(openCustomMode).ToString();
                }
                else
                {
                    callBack["result"] = false;
                    callBack["msg"] = "参数错误：OpenCustomMode 需要一个 bool 类型的参数。";
                }
                break;
            case "GetCustomMode":
                callBack["result"] = CPU.GetCustomMode().ToString();
                break;
            case "SetCPUTempWall":
                if (args.Length > 0 && byte.TryParse(args[0], out byte cpuTempWall))
                {
                    callBack["result"] = CPU.SetCPUTempWall(cpuTempWall).ToString();
                }
                else
                {
                    callBack["result"] = false;
                    callBack["msg"] = "参数错误：SetCPUTempWall 需要一个 byte 类型的参数。";
                }
                break;
        }
    }

    /// <summary>
    /// 处理 RGB 呼吸灯效果相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <summary>
    /// 处理 RGB 呼吸灯效果相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <param name="args">参数。</param>
    private void CLI_RGBBreathingLightEffect(string methodName, string[] args)
    {
        switch (methodName)
        {
            case "Start":
                RgbBreathingLightEffect.Start();
                break;
            case "Stop":
                RgbBreathingLightEffect.Stop();
                break;
        }
    }

    /// <summary>
    /// 处理键盘相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <summary>
    /// 处理键盘相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <param name="args">参数。</param>
    private void CLI_Keyboard(string methodName, string[] args)
    {
        switch (methodName)
        {
            case "ColorSet":
                if (args.Length > 2 && byte.TryParse(args[0], out byte RedParseResult) && byte.TryParse(args[1], out byte GreenParseResult) &&
                    byte.TryParse(args[2], out byte BlueParseResult))
                {
                    callBack["result"] = Keyboard.Color.Set(RedParseResult, GreenParseResult, BlueParseResult);
                }
                else
                {
                    callBack["result"] = false;
                    callBack["msg"] = "参数错误：ColorSet 需要三个 byte 类型的参数。";
                }
                break;
            case "ColorGet":
                callBack["result"] = Keyboard.Color.Get();
                break;
            case "ModeSet":
                callBack["result"] = Keyboard.Mode.Set();
                break;
            case "ModeGet":
                callBack["result"] = Keyboard.Mode.Get().ToString();
                break;
            case "LightBrightnessGet":
                callBack["result"] = Keyboard.LightBrightness.Get().ToString();
                break;
            case "LightBrightnessSet":
                if (args.Length > 0 && byte.TryParse(args[0], out byte LightBrightness))
                {
                    callBack["result"] = Keyboard.LightBrightness.Set(LightBrightness);
                }
                else
                {
                    callBack["result"] = false;
                    callBack["msg"] = "参数错误：LightBrightnessSet 需要一个 byte 类型的参数。";
                }
                break;
        }
    }

    /// <summary>
    /// 处理 Logo 灯相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <summary>
    /// 处理 Logo 灯相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <param name="args">参数。</param>
    private void CLI_LogoLight(string methodName, string[] args)
    {
        switch (methodName)
        {
            case "Set":
                if (args.Length > 0 && Enum.TryParse(args[0], out ResultState boolValue))
                {
                    callBack["result"] = LogoLight.Set(boolValue);
                }
                else
                {
                    callBack["result"] = false;
                    callBack["msg"] = "参数错误：Set 需要一个 ResultState 类型的参数。";
                }
                break;
            case "Get":
                callBack["result"] = LogoLight.Get().ToString();
                break;
        }
    }

    /// <summary>
    /// 处理 GPU 模式相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <summary>
    /// 处理 GPU 模式相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <param name="args">参数。</param>
    private void CLI_GPUMode(string methodName, string[] args)
    {
        switch (methodName)
        {
            case "Set":
                if (args.Length > 0 && Enum.TryParse(args[0], out GPUMode gpuEnum))
                {
                    callBack["result"] = GPU.Set(gpuEnum).ToString();
                }
                else
                {
                    callBack["result"] = false;
                    callBack["msg"] = "参数错误：Set 需要一个 GPUMode 类型的参数。";
                }
                break;
            case "Get":
                callBack["result"] = GPU.Get().ToString();
                break;
        }
    }

    /// <summary>
    /// 处理性能模式相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <summary>
    /// 处理性能模式相关的方法。
    /// </summary>
    /// <param name="methodName">方法名称。</param>
    /// <param name="args">参数。</param>
    private void CLI_PerformaceMode(string methodName, string[] args)
    {
        switch (methodName)
        {
            case "Set":
                if (args.Length > 0 && Enum.TryParse(args[0], out SystemPerMode PerformaceModeEnum))
                {
                    callBack["result"] = PerformaceMode.Set(PerformaceModeEnum);
                }
                else
                {
                    callBack["result"] = false;
                    callBack["msg"] = "参数错误：Set 需要一个 SystemPerMode 类型的参数。";
                }
                break;
            case "Get":
                callBack["result"] = PerformaceMode.Get().ToString();
                break;
        }
    }

    /// <summary>
    /// 根据类型名称和方法名称，调用相应的方法。
    /// </summary>
    /// <param name="typeName">类型名称。</param>
    /// <param name="methodName">方法名称。</param>
    /// <summary>
    /// 根据类型名称和方法名称，调用相应的方法。
    /// </summary>
    /// <param name="typeName">类型名称。</param>
    /// <param name="methodName">方法名称。</param>
    /// <param name="args">参数。</param>
    /// <returns>返回 JSON 格式的字符串。</returns>
    public string EumType(string typeName, string methodName, string[] args)
    {
        callBack["typeName"] = typeName;
        callBack["methodName"] = methodName;
        callBack["result"] = false;

        try
        {
            switch (typeName)
            {
                case "CPU":
                    CLI_Cpu(methodName, args);
                    break;
                case "GetHardwareMonitorInfo":
                    using (ComputerInformation computerInformation = new ComputerInformation())
                    {
                        callBack["result"] = computerInformation.GetHardwareMonitorInfo();
                    }
                    break;
                case "Fan":
                    switch (methodName)
                    {
                        case "GetFanSpeed":
                            callBack["result"] = Fan.GetFanSpeed();
                            break;
                        case "SetFanSpeed":
                            if (args.Length > 0)
                            {
                                callBack["result"] = Fan.SetFanSpeed(args[0]);
                            }
                            else
                            {
                                callBack["result"] = false;
                                callBack["msg"] = "参数错误：SetFanSpeed 需要一个参数。";
                            }
                            break;
                        case "SetMaxFanSpeedSwitch":
                            if (args.Length > 0)
                            {
                                callBack["result"] = Fan.SetMaxFanSpeedSwitch(args[0]);
                            }
                            else
                            {
                                callBack["result"] = false;
                                callBack["msg"] = "参数错误：SetMaxFanSpeedSwitch 需要一个参数。";
                            }
                            break;
                        case "GetMaxFanSpeedSwitch":
                            callBack["result"] = Fan.GetMaxFanSpeedSwitch();
                            break;
                    }
                    break;
                case "GPUMode":
                    CLI_GPUMode(methodName, args);
                    break;
                case "LogoLight":
                    CLI_LogoLight(methodName, args);
                    break;
                case "Keyboard":
                    CLI_Keyboard(methodName, args);
                    break;
                case "PerformaceMode":
                    CLI_PerformaceMode(methodName, args);
                    break;
                case "RGBBreathingLightEffect":
                    CLI_RGBBreathingLightEffect(methodName, args);
                    break;
                default:
                    callBack["result"] = false;
                    callBack["msg"] = "类型错误：未知的类型名称。";
                    break;
            }
        }
        catch (Exception e)
        {
            callBack["result"] = false;
            callBack["msg"] = e.Message;
        }

        if (ErrMag != null)
        {
            callBack["msg"] = ErrMag;
            ErrMag = null;
        }

        return callBack.ToString();
    }
}
