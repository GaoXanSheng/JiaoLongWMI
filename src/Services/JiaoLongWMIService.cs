using System.Diagnostics;
using JiaoLongWMI.server;
using JiaoLongWMI.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JiaoLongWMI.Services
{
    public class Worker : BackgroundService
    {
        private readonly JiaoLongWMIService _jiaoLongWMIService;
        private readonly ILogger<Worker> _logger;

        public Worker(JiaoLongWMIService jiaoLongWMIService, ILogger<Worker> logger)
        {
            _jiaoLongWMIService = jiaoLongWMIService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("JiaoLongWMI Worker 启动成功");
            _jiaoLongWMIService.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("JiaoLongWMI Worker 即将停止");
            _jiaoLongWMIService.Stop();
        }
    }

    public class JiaoLongWMIService
    {
        private SocketServer? _socketServer;

        // 运行服务（启动Host）
        public static void Run(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .UseWindowsService(options =>
                {
                    options.ServiceName = "JiaoLongWMI";
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<JiaoLongWMIService>();
                    services.AddHostedService<Worker>();
                });

            builder.Build().Run();
        }

        // 安装服务
        public static void Install()
        {
            var path = Process.GetCurrentProcess().MainModule.FileName;
            string args = $"create JiaoLongWMI binPath= \"{path}\" start= auto";
            ExecuteSCCommand(args, "安装");
        }

        // 卸载服务
        public static void Uninstall()
        {
            string args = "delete JiaoLongWMI";
            ExecuteSCCommand(args, "卸载");
        }

        // 启动服务
        public static void StartService()
        {
            ExecuteSCCommand("start JiaoLongWMI", "启动");
        }

        // 停止服务
        public static void StopService()
        {
            ExecuteSCCommand("stop JiaoLongWMI", "停止");
        }

        private static void ExecuteSCCommand(string arguments, string action)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("sc.exe", arguments)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using var process = Process.Start(psi);
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"{action}服务成功。");
                    Console.WriteLine(output);
                }
                else
                {
                    Console.WriteLine($"{action}服务失败。错误信息：");
                    Console.WriteLine(error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{action}服务时发生异常：{ex.Message}");
            }
        }

        public void Start()
        {
	        Logger.Info("JiaoLongWMI 服务启动...");
	        Task.Run(() =>
	        {
		        try
		        {
			        _socketServer = new SocketServer(new[] { "9871", "127.0.0.1" });
		        }
		        catch(Exception ex)
		        {
			        Logger.Error("SocketServer运行异常：" + ex);
		        }
	        });
        }


        public void Stop()
        {
            Logger.Info("JiaoLongWMI 服务停止...");
            _socketServer?.Close();
        }
    }
}
