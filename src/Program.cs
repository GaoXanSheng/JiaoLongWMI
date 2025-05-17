using JiaoLongWMI.server;
using JiaoLongWMI.Controllers;
using JiaoLongWMI.Utils;
using System.CommandLine;
using JiaoLongWMI.Services;

namespace JiaoLongWMI
{
	/// <summary>
	/// 应用程序的入口点。
	/// </summary>
	internal static class Program
	{
		/// <summary>
		/// 应用程序的主函数。
		/// </summary>
		/// <param name="args">命令行参数。</param>
		[STAThread]
		static async Task Main(string[] args)
		{
			if (!Environment.UserInteractive)
			{
				// 服务启动
				JiaoLongWMIService.Run(args);
				return;
			}
			if (args.Length == 0)
			{
				CommandLineHelper.ShowUsage();
				return;
			}

			// 创建根命令。
			var rootCommand = new RootCommand("JiaoLongWMI 命令行工具");

			// 创建类型参数。
			var typeNameArgument = new Argument<string>("typeName", "类型名称");
			rootCommand.AddArgument(typeNameArgument);

			// 创建方法参数。
			var methodNameArgument = new Argument<string>("methodName", "方法名称");
			rootCommand.AddArgument(methodNameArgument);

			// 创建参数选项。
			var parameterOption = new Option<string>("--parameter", "参数列表");
			rootCommand.AddOption(parameterOption);

			// 设置命令处理程序。
			rootCommand.SetHandler((typeName, methodName, parameterOptionValue) =>
			{
				string[] parameter = null;
				if (!string.IsNullOrEmpty(parameterOptionValue))
				{
					parameter = parameterOptionValue.Split(' ');
				}

				// 根据类型名称执行不同的操作。
				if (typeName == "SocketServer")
				{
					// 如果类型名称为 "SocketServer"，则创建 SocketServer 实例。
					new SocketServer(parameter);
					return;
				}

				if (typeName == "JiaoLongWMIService" && (methodName == "install" || methodName == "uninstall" ||
				                                         methodName == "start" || methodName == "stop"))
				{
					switch (methodName.ToLower())
					{
						case "install":
							JiaoLongWMIService.Install();
							break;
						case "uninstall":
							JiaoLongWMIService.Uninstall();
							break;
						case "start":
							JiaoLongWMIService.StartService();
							break;
						case "stop":
							JiaoLongWMIService.StopService();
							break;
						default:
							Console.WriteLine("未知命令。支持: install, uninstall, start, stop");
							break;
					}
					return;
				}

				if (typeName != null && methodName != null && parameter != null)
				{
					Logger.Info(new CliProgramEnumerationType().EumType(typeName, methodName, parameter));
					return;
				}
			}, typeNameArgument, methodNameArgument, parameterOption);

			// 执行命令。
			await rootCommand.InvokeAsync(args);
		}
	}
}
