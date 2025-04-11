using System.Text.Json.Nodes;
using JiaoLongWMI.tools;
using LibreHardwareMonitor.Hardware;

namespace JiaoLongWMI.Models;

public class ComputerInformation
{
	public static JsonObject GetHardwareMonitorInfo()
	{
		return Task.Run(() =>
		{
			var res = new JsonObject();
			var computer = new Computer
			{
				IsCpuEnabled = true,
				IsGpuEnabled = true,
				IsMemoryEnabled = true,
				IsMotherboardEnabled = true,
				IsControllerEnabled = true,
				IsNetworkEnabled = true,
				IsStorageEnabled = true,
				IsBatteryEnabled = true,
			};
			try
			{
				computer.Open();
			}
			catch (Exception e)
			{
				Logger.Info("[Error] Unexpected error: " + e.Message);
			}

			foreach (var hardwareItem in computer.Hardware)
			{
				var hardwareJson = new JsonObject();
				if (hardwareItem == null) continue;

				hardwareItem.Update();

				foreach (var sensor in hardwareItem.Sensors)
				{
					if (sensor.Value.HasValue && sensor.Name != null)
					{
						string sensorType = sensor.SensorType.ToString();

						// 如果该类型已存在，就使用已有的 JsonArray，否则新建一个
						JsonArray data;
						if (hardwareJson.ContainsKey(sensorType))
						{
							data = hardwareJson[sensorType].AsArray();
						}
						else
						{
							data = new JsonArray();
							hardwareJson[sensorType] = data;
						}

						var sensors = new JsonObject
						{
							["Name"] = sensor.Name,
							["Value"] = sensor.Value.Value
						};

						data.Add(sensors);
					}
				}
				if (hardwareJson.Count > 0)
				{
					hardwareJson["Name"] = hardwareItem.Name;
					res[hardwareItem.HardwareType.ToString()] = hardwareJson;
				}
			}

			computer.Close();
			return res;
		}).Result;
	}
}
