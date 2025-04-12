using System.Text.Json.Nodes;
using LibreHardwareMonitor.Hardware;

namespace JiaoLongWMI.Repositories;

public class ComputerInformation
{
	private JsonObject _res = new JsonObject();
	private Computer _computer =  new Computer {
		IsCpuEnabled = true,
		IsGpuEnabled = true,
		IsMemoryEnabled = true,
		IsMotherboardEnabled = true,
		IsControllerEnabled = true,
		IsNetworkEnabled = true,
		IsStorageEnabled = true,
		IsBatteryEnabled = true,
	};
	private bool _isUpdate;
	public ComputerInformation()
	{
		Open();
	}
	private void Update()
	{
		if (_isUpdate)
		{
			return;
		}

		_isUpdate = true;
		foreach (var hardwareItem in _computer.Hardware)
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
						data = hardwareJson[sensorType]?.AsArray();
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
				_res[hardwareItem.HardwareType.ToString()] = hardwareJson;
			}
		}
		_isUpdate = false;
	}
	public JsonObject GetHardwareMonitorInfo()
	{
		if (_isUpdate)
		{
			return _res;
		}
		Update();
		return _res;
	}

	private void Open()
	{
		_computer.Open();
	}
	public void Close()
	{
		_computer.Close();
	}
}
