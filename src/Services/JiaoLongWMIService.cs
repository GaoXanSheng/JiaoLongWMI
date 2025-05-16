using JiaoLongWMI.server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Topshelf;

namespace JiaoLongWMI.Services
{
	/// <summary>
	/// JiaoLongWMI 服务类
	/// </summary>
	public class JiaoLongWMIService : ServiceControl
	{
		private readonly ILogger _logger;
		private SocketServer _socketServer = new SocketServer(["9871", "127.0.0.1"]);

		/// <summary>
		/// 构造函数
		/// </summary>
		public JiaoLongWMIService()
		{
			_logger = InitializeLogger();
			_logger.LogInformation("JiaoLongWMI 服务正在初始化...");
		}

		/// <summary>
		/// 初始化日志系统
		/// </summary>
		private ILogger InitializeLogger()
		{
			var serviceProvider = new ServiceCollection()
				.AddLogging(builder => { builder.AddConsole(); })
				.BuildServiceProvider();
			return serviceProvider.GetRequiredService<ILogger<JiaoLongWMIService>>();
		}

		/// <summary>
		/// 启动服务
		/// </summary>
		public bool Start(HostControl hostControl)
		{
			_logger.LogInformation("JiaoLongWMI 服务已启动。");
			return true;
		}

		/// <summary>
		/// 停止服务
		/// </summary>
		public bool Stop(HostControl hostControl)
		{
			_logger.LogInformation("JiaoLongWMI 服务已停止。");
			_socketServer.Close();
			return true;
		}
	}
}
