using System.Text.Json;
using System.Text.Json.Nodes;
using JiaoLongWMI.Controllers;
using JiaoLongWMI.Utils;

namespace JiaoLongWMI.server;

public class ServiceModeSocketServer : SocketServer
{
	public ServiceModeSocketServer(string[] args) : base(args)
	{
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
		if (typeName.ToString() == "SetFanCurve")
		{
			var callBack = new JsonObject();
			callBack["typeName"] = typeName.DeepClone();
			callBack["methodName"] = methodName.DeepClone();

			// 组合文件完整路径
			string filePath = Path.Combine(Logger.GetLogDirectory(), "fanCurve.json");

			try
			{
				// 写文件，覆盖已有内容
				File.WriteAllText(filePath, args.ToString());
				callBack["result"] = "保存 配置文件 成功";
			}
			catch (Exception ex)
			{
				callBack["msg"] = $"保存 配置文件 失败 {ex.Message}";
			}

			return callBack.ToString();
		}

		return base.Parse(typeName, methodName, args);
	}
}
