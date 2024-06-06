using System.Net;
using System.Net.Sockets;
using System.Text;

namespace JiaoLong16Pro.server;

public class socketServer
{
    public CLI_Program_Enumeration_Type socket_Program_Enumeration_Type = new CLI_Program_Enumeration_Type();
   public socketServer(string[] args)
   {
        string port = args[0];
        string host = args[1];
        // 设置要监听的URL
        string url = "http://"+host+":"+port+"/";
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(url);
        try
        {
            listener.Start();
            Console.WriteLine(url);

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

                    Console.WriteLine(body);

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

                    string responseString = decode(body);  // Ensure 'decode' method exists and is functional
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
            Console.WriteLine(ex.Message);
        }
        finally
        {
            // 停止监听
            listener.Close();
        }
    }

   public string decode(string msg)
   {
       string[] ProgramArgs = msg.Split("-");
       string ProgramTitle = ProgramArgs[0];
       string[] function = new string[99];
       for (int i = 1; i < ProgramArgs.Length; i++)
       {
           function.SetValue(ProgramArgs[i], i - 1);
       }

       if (ProgramTitle != "Socket")
       {
         return socket_Program_Enumeration_Type.eumType(ProgramTitle, function);
       }
       else
       {
           throw new Exception("Server is Run");
       }
   }
}