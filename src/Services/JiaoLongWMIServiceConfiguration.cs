using System;
using Topshelf;
using JiaoLongWMI.Utils;

namespace JiaoLongWMI.Services;

/// <summary>
/// JiaoLongWMI 服务配置类
/// </summary>
public class JiaoLongWMIServiceConfiguration
{
    /// <summary>
    /// 配置并运行服务，根据参数支持安装、卸载、运行等操作
    /// </summary>
    /// <param name="args">传入的命令行参数，如 "install"、"uninstall" 等</param>
    public static void ConfigureService(string args)
    {
        HostFactory.Run(x =>
        {
            x.Service<JiaoLongWMIService>(s =>
            {
                s.ConstructUsing(name =>
                {
                    Logger.Info("正在构造服务对象...");
                    try
                    {
                        return new JiaoLongWMIService();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("服务构造失败: " + ex);
                        throw;
                    }
                });

                s.WhenStarted((tc, host) =>
                {
                    Logger.Info("服务正在启动...");
                    try
                    {
                        return tc.Start(host);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("服务启动失败: " + ex);
                        throw;
                    }
                });

                s.WhenStopped((tc, host) =>
                {
                    Logger.Info("服务正在停止...");
                    try
                    {
                        return tc.Stop(host);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("服务停止失败: " + ex);
                        throw;
                    }
                });
            });

            // 设置运行身份
            x.RunAsLocalSystem();

            // 设置服务信息
            x.SetDescription("JiaoLongWMI 调度服务");
            x.SetDisplayName("JiaoLongWMI");
            x.SetServiceName("JiaoLongWMI");

            // 应用命令行参数（如 install/uninstall/start/stop）
            if (args.Length > 0)
            {
                x.ApplyCommandLine(args);
            }

            // 自动启动
            x.StartAutomatically();

            // 服务失败恢复策略
            x.EnableServiceRecovery(r =>
            {
                r.RestartService(1); // 1 分钟后重启服务
            });

            // 错误捕获
            x.OnException(ex =>
            {
                Logger.Error("服务发生未处理异常: " + ex);
            });

            // 安装/卸载钩子
            x.AfterInstall(() => Logger.Info("✅ 服务已成功安装！"));
            x.AfterUninstall(() => Logger.Info("❌ 服务已成功卸载！"));
        });
    }
}
