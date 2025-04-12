using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using JiaoLongWMI.Utils;

namespace JiaoLongWMI.Controllers;

public class SocketControllers
{
	public string Port { get; private set; }
	public string Host { get; private set; }
	public string Url { get; private set; }
	public SocketControllers(string port,string host)
	{
		Port = port;
		Host = host;
		Url = "http://" + Host + ":" + Port + "/";
		HttpListener listener = new HttpListener();
		listener.Prefixes.Add(Url);
		try
		{
			listener.Start();

			// 接受请求并处理
			while (true)
			{
				HttpListenerContext context = listener.GetContext(); // 接受请求
				ThreadPool.QueueUserWorkItem((_) =>
				{
					// 处理请求
					HttpListenerRequest request = context.Request;
					string body;
					using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
					{
						body = reader.ReadToEnd();
					}


					// 返回响应
					HttpListenerResponse response = context.Response;

					// Add CORS headers
					response.Headers.Add("Access-Control-Allow-Origin", "*");
					response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
					response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

					// Handle preflight requests (OPTIONS method)
					if (request.HttpMethod == "OPTIONS")
					{
						response.StatusCode = (int)HttpStatusCode.NoContent;
						response.Close();
						return;
					}

					string responseString = Decode(body); // Ensure 'decode' method exists and is functional
					byte[] buffer = Encoding.UTF8.GetBytes(responseString);

					response.ContentLength64 = buffer.Length;
					response.ContentType = "text/plain";
					response.StatusCode = (int)HttpStatusCode.OK;

					Stream output = response.OutputStream;
					try
					{
						output.Write(buffer, 0, buffer.Length);
					}
					catch (HttpListenerException ex)
					{
						Logger.Info($"[Warn] Client disconnected or network issue: {ex.Message}");
					}
					catch (Exception ex)
					{
						Logger.Info($"[Error] Unexpected error: {ex.Message}");
					}

					output.Close();
				});
			}
		}
		catch (Exception ex)
		{
			Logger.Info(ex.Message);
		}
		finally
		{
			// 停止监听
			listener.Close();
		}
	}

	// 需要子类方法重写，并返回JsonString
	public virtual string Parse(JsonNode typeName, JsonNode methodName, JsonNode args)
	{
		return new JsonObject().ToString();
	}

	// 想重写Decode方法也可以，但是不推荐
	public virtual string Decode(string msg)
	{
		// msg是个json 提取msg
		// 解析 JSON 字符串
		JsonObject json = JsonObject.Parse(msg).AsObject();
		// 提取 "message" 字段
		json.TryGetPropertyValue("type", out JsonNode typeName);
		json.TryGetPropertyValue("method", out JsonNode methodName);
		json.TryGetPropertyValue("args", out JsonNode args);
		return Parse(typeName,methodName,args);
	}
}
