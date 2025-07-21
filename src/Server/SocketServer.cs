using System.Text.Json.Nodes;
using JiaoLongWMI.Controllers;
using JiaoLongWMI.Repositories;

namespace JiaoLongWMI.server;

/// <summary>
/// Socket 服务器类，用于处理来自 Socket 客户端的请求。
/// </summary>
public class SocketServer : SocketControllers
{
    // 用于获取计算机硬件信息的 ComputerInformation 对象。
    public static ComputerInformation _computer  = new ComputerInformation();

    /// <summary>
    /// 构造函数，初始化 Socket 服务器。
    /// </summary>
    /// <param name="args">命令行参数，包含端口号和主机地址。</param>
    public SocketServer(string[] args) : base(args[0], args[1])
    {

    }

    public void Close()
    {
	    _computer.Dispose();
	    listener.Close();
    }
    /// <summary>
    /// 解析来自 Socket 客户端的请求，并调用相应的方法。
    /// </summary>
    /// <param name="typeName">类型名称。</param>
    /// <param name="methodName">方法名称。</param>
    /// <param name="args">参数。</param>
    /// <returns>包含结果的 JsonObject 字符串。</returns>
    public override string Parse(JsonNode typeName, JsonNode methodName, JsonNode args)
    {
        // 通过 socket 访问时，分离数据
        // GetHardwareMonitorInfo 很耗时，分离线程
        if (typeName.ToString() == "GetHardwareMonitorInfo")
        {
            var callBack = new JsonObject();
            callBack["typeName"] = typeName.DeepClone();
            callBack["methodName"] = methodName.DeepClone();
            callBack["result"] = _computer.GetHardwareMonitorInfo().DeepClone();
            return callBack.ToString();
        }

        return new CliProgramEnumerationType().EumType(typeName.ToString(), methodName.ToString(),
            args.AsArray().Select(item => item.ToString()).ToArray());
    }
}
