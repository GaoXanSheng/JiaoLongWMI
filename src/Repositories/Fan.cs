using System.Text.Json.Nodes;
using JiaoLongWMI.Constants;
using JiaoLongWMI.Controllers;
using JiaoLongWMI.Services;


namespace JiaoLongWMI.Repositories;

public class Fan
{
	public static string SetFanSpeed(string inputSpeed)
	{
		bool success = byte.TryParse(inputSpeed, out byte outSpeed);
		if (!success)
		{
			return "Input Speed Error";
		}

		using (ECController ec = new ECController())
		{
			if (ec.WinIoState)
			{
				ec.Fan1SetSpeed(outSpeed);
				ec.Fan2SetSpeed(outSpeed);
				return "Fan Speed Set OK";
			}
		}

		return "Fan Speed Set Error";
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
