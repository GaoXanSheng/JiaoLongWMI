using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using JiaoLongWMI.Utils;

namespace JiaoLongWMI.Controllers;

/// <summary>
/// Socket 控制器，用于处理 Socket 请求。
/// </summary>
public class SocketControllers
{
    /// <summary>
    /// 端口号。
    /// </summary>
    public string Port { get; private set; }
    /// <summary>
    /// 主机地址。
    /// </summary>
    public string Host { get; private set; }
    /// <summary>
    /// URL 地址。
    /// </summary>
    public string Url { get; private set; }
		public HttpListener listener { get; private set; }
    /// <summary>
    /// 构造函数，初始化 Socket 控制器。
    /// </summary>
    /// <param name="port">端口号。</param>
    /// <param name="host">主机地址。</param>
    public SocketControllers(string port, string host)
    {
        Port = port;
        Host = host;
        Url = "http://" + Host + ":" + Port + "/";
				listener = new HttpListener();
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

    /// <summary>
    /// 需要子类方法重写，并返回 JsonString。
    /// </summary>
    /// <param name="typeName">类型名称。</param>
    /// <param name="methodName">方法名称。</param>
    /// <param name="args">参数。</param>
    /// <returns>JsonString。</returns>
    public virtual string Parse(JsonNode typeName, JsonNode methodName, JsonNode args)
    {
        return new JsonObject().ToString();
    }

    /// <summary>
    /// 想重写 Decode 方法也可以，但是不推荐。
    /// </summary>
    /// <param name="msg">消息。</param>
    /// <returns>解析后的字符串。</returns>
    public virtual string Decode(string msg)
    {
        // msg是个json 提取msg
        // 解析 JSON 字符串
        JsonObject json = JsonObject.Parse(msg).AsObject();
        // 提取 "message" 字段
        json.TryGetPropertyValue("type", out JsonNode typeName);
        json.TryGetPropertyValue("method", out JsonNode methodName);
        json.TryGetPropertyValue("args", out JsonNode args);
        return Parse(typeName, methodName, args);
    }
}
