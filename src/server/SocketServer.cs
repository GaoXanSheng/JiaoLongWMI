using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using JiaoLongWMI.tools;
using Namotion.Reflection;

namespace JiaoLongWMI.server;

public class SocketServer
{
    private readonly CliProgramEnumerationType _socketProgramEnumerationType = new CliProgramEnumerationType();

    public SocketServer(string[] args)
    {
        string port = args[0];
        string host = args[1];
        // 设置要监听的URL
        string url = "http://" + host + ":" + port + "/";
        Logger.Info(url);
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(url);
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

                    Logger.Info(body);

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
                    output.Write(buffer, 0, buffer.Length);
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

    private string Decode(string msg)
    {
        // msg是个json 提取msg
        // 解析 JSON 字符串
        JsonObject json = JsonObject.Parse(msg).AsObject();
        // 提取 "message" 字段
        json.TryGetPropertyValue("type", out JsonNode typeName);
        json.TryGetPropertyValue("method", out JsonNode methodName);
        json.TryGetPropertyValue("args", out JsonNode args);
        return _socketProgramEnumerationType.EumType(typeName.ToString(), methodName.ToString(),args.AsArray().Select(item => item.ToString()).ToArray());
    }
}