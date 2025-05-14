using System;
using System.Collections.Generic;
using System.Text;
using JiaoLongWMI.server;
using JiaoLongWMI.Controllers;
using JiaoLongWMI.Services;
using JiaoLongWMI.Utils;
using System.CommandLine;
using JiaoLongWMI;

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
            // 设置控制台输出编码为 UTF8，以支持显示中文等字符。
            Console.OutputEncoding = Encoding.UTF8;

            if (args.Length == 0)
            {
                CommandLineHelper.ShowUsage();
                return;
            }

            // 创建根命令。
            var rootCommand = new RootCommand("JiaoLongWMI 命令行工具");

            // 创建类型参数。
            var typeNameArgument = new Argument<string>("typeName", "类型名称 (例如：CPU, Fan, Keyboard)");
            rootCommand.AddArgument(typeNameArgument);

            // 创建方法参数。
            var methodNameArgument = new Argument<string>("methodName", "方法名称 (例如：GetFanSpeed, SetCpuLongPower)");
            rootCommand.AddArgument(methodNameArgument);

            // 创建参数选项。
            var parameterOption = new Option<string>("--parameter", "参数列表 (例如：9871 127.0.0.1)");
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
                }
                else
                {
                    // 否则，调用 CliProgramEnumerationType 类的 EumType 方法，并将结果记录到日志中。
                    Logger.Info(new CliProgramEnumerationType().EumType(typeName, methodName, parameter));
                }
            }, typeNameArgument, methodNameArgument, parameterOption);


            // 执行命令。
            await rootCommand.InvokeAsync(args);
        }
    }
}
