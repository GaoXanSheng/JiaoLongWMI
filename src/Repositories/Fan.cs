using System;
using System.Text.Json.Nodes;
using JiaoLongWMI.Constants;
using JiaoLongWMI.Controllers;
using JiaoLongWMI.Services;

namespace JiaoLongWMI.Repositories;

/// <summary>
/// 风扇相关操作的仓库类。
/// </summary>
public class Fan
{
    /// <summary>
    /// 设置风扇转速。
    /// </summary>
    /// <param name="inputSpeed">输入的转速值（字符串类型）。</param>
    /// <returns>设置结果信息。</returns>
    public static string SetFanSpeed(string inputSpeed)
    {
	    if (string.IsNullOrWhiteSpace(inputSpeed))
	    {
		    return "Input Speed Error";
	    }

	    // 仅保留前两位字符
	    string trimmedInput = inputSpeed.Length >= 2 ? inputSpeed.Substring(0, 2) : inputSpeed;

	    bool success = byte.TryParse(trimmedInput, out byte outSpeed);
	    if (!success)
	    {
		    return "Input Speed Error";
	    }

	    using (ECController ec = new ECController())
	    {
		    if (ec.State)
		    {
			    ec.Fan1SetSpeed(outSpeed);
			    ec.Fan2SetSpeed(outSpeed);
			    return "Fan Speed Set OK";
		    }
	    }

	    return "Fan Speed Set Error";
    }


    /// <summary>
    /// 设置最大风扇转速开关。
    /// </summary>
    /// <param name="set">开关状态（"1" 表示开启，其他值表示关闭）。</param>
    /// <returns>是否设置成功。</returns>
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

    /// <summary>
    /// 获取最大风扇转速开关状态。
    /// </summary>
    /// <returns>最大风扇转速开关状态（0 或 1）。</returns>
    public static byte GetMaxFanSpeedSwitch()
    {
        return MethodServices.GetValue<byte>(MethodName.MaxFanSpeedSwitch);
    }

    /// <summary>
    /// 获取风扇转速。
    /// </summary>
    /// <returns>包含 CPU 和 GPU 风扇转速的 JsonObject。</returns>
    public static JsonObject GetFanSpeed()
    {
        var res = new JsonObject();
        Tuple<int, int> CPUGPUFanSpeed = MethodServices.GetValue<Tuple<int, int>>(MethodName.CPUGPUFanSpeed);
        res["CPUFanSpeed"] = CPUGPUFanSpeed.Item1;
        res["GPUFanSpeed"] = CPUGPUFanSpeed.Item2;
        return res;
    }
}
