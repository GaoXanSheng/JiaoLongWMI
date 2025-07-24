using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using JiaoLongWMI.Constants;
using JiaoLongWMI.Repositories;
using JiaoLongWMI.server;
using JiaoLongWMI.Utils;
using Microsoft.Extensions.Hosting;

namespace JiaoLongWMI.Services;

public class Worker : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;

	public Worker(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		Logger.Info("JiaoLongWMI Worker 启动成功");
		// 注册取消回调：杀死当前进程
		stoppingToken.Register(() =>
		{
			Logger.Info("收到取消信号，准备终止当前进程...");
			try
			{
				Process.GetCurrentProcess().Kill();
			}
			catch (Exception ex)
			{
				Logger.Info($"Kill 当前进程失败: {ex}");
			}
		});
		var socketTask = Task.Run(() => StartSocketServer(), stoppingToken);
		var fanControlTask = Task.Run(() => StartFanControlLoop(stoppingToken), stoppingToken);

		await Task.WhenAll(socketTask, fanControlTask);
	}


	private void StartSocketServer()
	{
		try
		{
			new ServiceModeSocketServer(new[] { "9871", "127.0.0.1" });
		}
		catch (Exception ex)
		{
			Logger.Info("SocketServer 启动失败: " + ex);
		}
	}

	private static List<FanCurvePoint> fanCurve;

	private void StartFanControlLoop(CancellationToken token)
	{
		string path = Path.Combine(Logger.GetLogDirectory(), "fanCurve.json");

		if (!File.Exists(path))
		{
			Logger.Info("fanCurve配置文件不存在。");
			return;
		}

		fanCurve = LoadFanCurve(path);

		if (fanCurve.Count == 0)
		{
			Logger.Info("fanCurve 为空，不启用温控");
			return;
		}

		Logger.Info($"成功加载 {fanCurve.Count} 条风扇曲线");

		while (!token.IsCancellationRequested)
		{
			try
			{
				if (fanCurve.Count == 0)
				{
					Logger.Info("fanCurve 为空，不启用温控");
					return;
				}

				int temp = GetCpuTemperature();
				int rpm = GetFanRpmForTemp(fanCurve, temp);
				string result = Fan.SetFanSpeed(rpm.ToString());
				Logger.Info($"温度: {temp}°C -> 转速: {rpm} RPM，结果: {result}");
			}
			catch (Exception ex)
			{
				Logger.Info("[FanControlLoop] 异常: " + ex);
			}

			Thread.Sleep(5000);
		}
	}


	public static void ReloadFanCurve(string list)
	{
		var curve = JsonSerializer.Deserialize<List<FanCurvePoint>>(list);
		fanCurve = curve ?? new();
		Logger.Info($"重新加载 fanCurve 成功，共 {fanCurve.Count} 条");
	}

	private static int GetCpuTemperature()
	{
		try
		{
			JsonObject? root = SocketServer._computer.GetHardwareMonitorInfo();
			if (root == null)
				return 0;

			// 逐级安全地访问嵌套结构
			var valueNode = root["Cpu"]?["Temperature"]?[0]?["Value"];
			if (valueNode == null)
				return 0;
			// 读取为 float 或 double，再手动转换
			float tempFloat = valueNode.GetValue<float>();
			return (int)Math.Round(tempFloat); // 四舍五入转 int
		}
		catch (Exception ex)
		{
			// 可选：记录日志
			Logger.Info($"[GetCpuTemperature] Error: {ex.Message}");
			return 0;
		}
	}

	private List<FanCurvePoint> LoadFanCurve(string filePath)
	{
		try
		{
			string json = File.ReadAllText(filePath);
			var curve = JsonSerializer.Deserialize<List<FanCurvePoint>>(json);
			return curve ?? new();
		}
		catch (Exception ex)
		{
			Logger.Info("[LoadFanCurve] 异常: " + ex);
			return new();
		}
	}

	public static int GetFanRpmForTemp(List<FanCurvePoint> curve, int temperature)
	{
		if (curve == null || curve.Count == 0)
		{
			Logger.Info("GetFanRpmForTemp: 曲线为空，返回2000rpm");
			return 2000;
		}

		curve = curve.OrderBy(p => p.temp).ToList();

		Logger.Info($"GetFanRpmForTemp: 输入温度 {temperature}°C，风扇曲线点数 {curve.Count}");

		if (temperature <= curve[0].temp)
		{
			int rpm = curve[0].speed;
			Logger.Info($"温度低于曲线最低点 {curve[0].temp}°C，返回值 {rpm}（rpm）");
			return rpm;
		}

		if (temperature >= curve[^1].temp)
		{
			int rpm = curve[^1].speed;
			Logger.Info($"温度高于曲线最高点 {curve[^1].temp}°C，返回值 {rpm}（rpm）");
			return rpm;
		}

		for (int i = 0; i < curve.Count - 1; i++)
		{
			var lower = curve[i];
			var upper = curve[i + 1];

			if (temperature >= lower.temp && temperature <= upper.temp)
			{
				double ratio = (temperature - lower.temp) / (double)(upper.temp - lower.temp);
				int interpolated = (int)(lower.speed + ratio * (upper.speed - lower.speed));
				Logger.Info($"温度位于区间 [{lower.temp}°C, {upper.temp}°C], 得到转速: {interpolated}（rpm）");
				return interpolated;
			}
		}

		Logger.Info("未命中任何区间，返回2000rpm");
		return 2000;
	}
}
