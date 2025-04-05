using System.Text.Json.Nodes;
using JiaoLongWMI.tools;
using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLongWMI.Models;

public class Fan
{
	public static string SetFanSpeed(string inputSpeed)
	{
		ECController ecController = new ECController();
		bool success = byte.TryParse(inputSpeed, out byte outSpeed);
		if (!success)
		{
			return "Input Speed Error";
		}

		var librarySutatus = ecController.LibrarySutatus();
		if (librarySutatus != "DLL Status OK")
		{
			return librarySutatus;
		}

		if (ecController.Fan1SetSpeed(outSpeed) && ecController.Fan2SetSpeed(outSpeed))
		{
			ecController.ReadUint((ushort)ECMemoryTable.Fan1_RPM);
			ecController.ReadUint((ushort)ECMemoryTable.Fan2_RPM);
			ecController.Dispose();
			return "Fan Speed Set OK";
		}
		else
		{
			ecController.Dispose();
			return "Fan Speed Set Error";
		}
	}

	public static JsonObject GetFanLevel()
	{
		var res = new JsonObject();
		var winRing0 = new ECController();
		var librarySutatus = winRing0.LibrarySutatus();
		if (librarySutatus != "DLL Status OK")
		{
			res["msg"] = librarySutatus;
			return res;
		}

		res["fan1RpmLevel"] = winRing0.ReadUint((char)ECMemoryTable.Fan1_RPM_Level);
		res["fan2RpmLevel"] = winRing0.ReadUint((char)ECMemoryTable.Fan2_RPM_Level);
		winRing0.Dispose();
		res["msg"] = "Fan Speed Set OK";
		return res;
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
