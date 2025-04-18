using System.Text.Json.Nodes;
using JiaoLongWMI.Controllers;
using JiaoLongWMI.Repositories;

namespace JiaoLongWMI.server;

public class SocketServer : SocketControllers
{
	private static ComputerInformation _computer = null;
	public SocketServer(string[] args) : base(args[0], args[1])
	{
		_computer = new ComputerInformation();
		Program.ShutdownDispatcher.Subscribe(_computer.Dispose);
	}

	public override string Parse(JsonNode typeName, JsonNode methodName, JsonNode args)
	{
		// 通过socket访问时，分离数据
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
