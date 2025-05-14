using System.Diagnostics.CodeAnalysis;

namespace JiaoLongWMI.Utils;

/// <summary>
/// 日志记录器类，用于记录应用程序的日志信息。
/// </summary>
public class Logger
{
    /// <summary>
    /// 记录信息级别的日志。
    /// </summary>
    /// <typeparam name="T">消息类型。</typeparam>
    /// <param name="msg">要记录的消息。</param>
    public static void Info<T>(T msg)
    {
        Console.WriteLine(""+msg);
    }

    /// <summary>
    /// 记录信息级别的日志（字节类型）。
    /// </summary>
    /// <param name="msg">要记录的消息。</param>
    public static void Info(byte msg)
    {
        Console.WriteLine(""+msg.ToString("X2"));
    }

    /// <summary>
    /// 记录信息级别的日志（格式化字符串）。
    /// </summary>
    /// <param name="format">格式化字符串。</param>
    /// <param name="arg0">参数。</param>
    public static void Info([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
    {
     Console.WriteLine(format,""+arg0);
    }
}
