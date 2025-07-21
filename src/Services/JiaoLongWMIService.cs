using System.Diagnostics;
using System.Management;
using System.Text.Json;
using JiaoLongWMI.Constants;
using JiaoLongWMI.Repositories;
using JiaoLongWMI.server;
using JiaoLongWMI.Services;
using JiaoLongWMI.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JiaoLongWMI.Services
{
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

			var socketTask = Task.Run(() => StartSocketServer(), stoppingToken);
			var fanControlTask = Task.Run(() => StartFanControlLoop(stoppingToken), stoppingToken);
			var watchTask = Task.Run(() => StartFanCurveWatcher(stoppingToken), stoppingToken);

			await Task.WhenAll(socketTask, fanControlTask, watchTask);
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
				Logger.Info("fanCurve.json 不存在。");
				return;
			}

			fanCurve = LoadFanCurve(path);

			if (fanCurve.Count == 0)
			{
				Logger.Info("fanCurve.json 为空，不启用温控");
				return;
			}

			Logger.Info($"成功加载 {fanCurve.Count} 条风扇曲线");

			while (!token.IsCancellationRequested)
			{
				try
				{
					int temp = GetCpuTemperature();
					int rpm = GetFanRpmForTemp(fanCurve, temp);
					string result = Fan.SetFanSpeed(rpm.ToString());
					Logger.Info($"温度: {temp}°C -> 转速: {rpm * 100} RPM，结果: {result}");
				}
				catch (Exception ex)
				{
					Logger.Info("[FanControlLoop] 异常: " + ex);
				}

				Thread.Sleep(5000);
			}
		}

		private void StartFanCurveWatcher(CancellationToken token)
		{
			string path = Path.Combine(Logger.GetLogDirectory(), "fanCurve.json");

			var watcher = new FileSystemWatcher
			{
				Path = Path.GetDirectoryName(path)!,
				Filter = Path.GetFileName(path),
				NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
			};

			DateTime lastRead = DateTime.MinValue;
			watcher.Changed += (_, _) => ReloadFanCurve(path, ref lastRead);
			watcher.Created += (_, _) => ReloadFanCurve(path, ref lastRead);
			watcher.Renamed += (_, _) => ReloadFanCurve(path, ref lastRead);
			watcher.EnableRaisingEvents = true;

			Logger.Info("已启动 fanCurve.json 文件监控");

			while (!token.IsCancellationRequested)
			{
				Thread.Sleep(Timeout.Infinite);
			}
		}

		private void ReloadFanCurve(string path, ref DateTime lastRead)
		{
			if ((DateTime.Now - lastRead).TotalMilliseconds < 500) return;
			lastRead = DateTime.Now;

			try
			{
				Logger.Info("检测到 fanCurve.json 变更，重新加载...");
				fanCurve = LoadFanCurve(path);
				Logger.Info($"重新加载 fanCurve 成功，共 {fanCurve.Count} 条");
			}
			catch (Exception ex)
			{
				Logger.Info("重新加载 fanCurve 失败: " + ex);
			}
		}

		private static int GetCpuTemperature()
		{
			using var searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
			foreach (var obj in searcher.Get())
			{
				double tempK = Convert.ToDouble(obj["CurrentTemperature"]);
				return (int)((tempK - 2732) / 10.0);
			}

			return 0;
		}

		private static List<FanCurvePoint> LoadFanCurve(string filePath)
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
				return 20; // 返回20，表示转速“20”，后续乘以100才是2000rpm
			}

			curve = curve.OrderBy(p => p.temp).ToList();

			Logger.Info($"GetFanRpmForTemp: 输入温度 {temperature}°C，风扇曲线点数 {curve.Count}");

			int ExtractFirstTwoDigits(int rpm)
			{
				if (rpm <= 0) return 20; // 最低20，表示2000rpm

				var rpmStr = rpm.ToString();
				var firstTwoDigits = rpmStr.Length >= 2 ? rpmStr.Substring(0, 2) : rpmStr.PadRight(2, '0');
				int val = int.Parse(firstTwoDigits);
				return val < 15 ? 15 : val; // 最低15（1500rpm）
			}

			if (temperature <= curve[0].temp)
			{
				int val = ExtractFirstTwoDigits(curve[0].speed);
				Logger.Info($"温度低于曲线最低点 {curve[0].temp}°C，返回值 {val * 100}（rpm）");
				return val;
			}

			if (temperature >= curve[^1].temp)
			{
				int val = ExtractFirstTwoDigits(curve[^1].speed);
				Logger.Info($"温度高于曲线最高点 {curve[^1].temp}°C，返回值 {val * 100}（rpm）");
				return val;
			}

			for (int i = 0; i < curve.Count - 1; i++)
			{
				var lower = curve[i];
				var upper = curve[i + 1];

				if (temperature >= lower.temp && temperature <= upper.temp)
				{
					double ratio = (temperature - lower.temp) / (double)(upper.temp - lower.temp);
					double interpolated = lower.speed + ratio * (upper.speed - lower.speed);
					int rpmRaw = (int)interpolated;
					int val = ExtractFirstTwoDigits(rpmRaw);
					Logger.Info($"温度位于区间 [{lower.temp}°C, {upper.temp}°C], 原始转速: {rpmRaw}，返回值: {val * 100}（rpm）");
					return val;
				}
			}

			Logger.Info("未命中任何区间，返回20");
			return 20;
		}
	}
}

public class JiaoLongWMIService
{
	// ----------------------------
	// Windows 服务入口
	// ----------------------------
	public static void Run(string[] args)
	{
		var builder = Host.CreateDefaultBuilder(args)
			.UseWindowsService(options => { options.ServiceName = "JiaoLongWMI"; })
			.ConfigureServices((hostContext, services) =>
			{
				services.AddSingleton<JiaoLongWMIService>();
				services.AddHostedService<Worker>();
			});

		builder.Build().Run();
	}

	// ----------------------------
	// Windows 服务控制命令
	// ----------------------------
	public static void Install()
	{
		var path = Process.GetCurrentProcess().MainModule!.FileName;
		string args = $"create JiaoLongWMI binPath= \"{path}\" start= auto";
		ExecuteSCCommand(args, "安装");
	}

	public static void Uninstall()
	{
		string args = "delete JiaoLongWMI";
		ExecuteSCCommand(args, "卸载");
	}

	public static void StartService()
	{
		ExecuteSCCommand("start JiaoLongWMI", "启动");
	}

	public static void StopService()
	{
		ExecuteSCCommand("stop JiaoLongWMI", "停止");
	}

	private static void ExecuteSCCommand(string arguments, string action)
	{
		try
		{
			var psi = new ProcessStartInfo("sc.exe", arguments)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using var process = Process.Start(psi);
			process!.WaitForExit();

			var output = process.StandardOutput.ReadToEnd();
			var error = process.StandardError.ReadToEnd();

			if (process.ExitCode == 0)
			{
				Console.WriteLine($"{action}服务成功。");
				Console.WriteLine(output);
			}
			else
			{
				Console.WriteLine($"{action}服务失败：{error}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"{action}服务异常：{ex.Message}");
		}
	}
}
