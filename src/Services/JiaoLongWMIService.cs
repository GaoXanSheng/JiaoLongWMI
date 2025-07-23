using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JiaoLongWMI.Services;
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
