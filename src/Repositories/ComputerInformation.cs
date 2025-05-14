using System;
using System.Text.Json.Nodes;
using LibreHardwareMonitor.Hardware;

namespace JiaoLongWMI.Repositories;

/// <summary>
/// 计算机信息类，用于获取计算机硬件信息。
/// </summary>
public class ComputerInformation: IDisposable
{
    // 用于存储硬件信息的 JsonObject。
    private JsonObject _res = new JsonObject();
    // 用于获取硬件信息的 Computer 对象。
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

    /// <summary>
    /// 构造函数，初始化 Computer 对象并打开硬件监控。
    /// </summary>
    public ComputerInformation()
    {
        Open();
    }

    // 用于标记是否已更新硬件信息。
    private bool _isUpdate;

    /// <summary>
    /// 更新硬件信息。
    /// </summary>
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

    /// <summary>
    /// 获取硬件监控信息。
    /// </summary>
    /// <returns>包含硬件信息的 JsonObject。</returns>
    public JsonObject GetHardwareMonitorInfo()
    {
        if (_isUpdate)
        {
            return _res;
        }
        Update();
        return _res;
    }

    /// <summary>
    /// 打开硬件监控。
    /// </summary>
    public void Open()
    {
        _computer.Open();
    }

    /// <summary>
    /// 释放资源，关闭硬件监控。
    /// </summary>
    public void Dispose()
    {
        _computer.Close();
    }
}
