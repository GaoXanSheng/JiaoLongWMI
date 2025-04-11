using System.Text;
using JiaoLongWMI.server;
using JiaoLongWMI.tools;

namespace JiaoLongWMI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            if (args.Length != 0)
            {
                string typeName = args[0];
                string methodName = args[1];
                List<string> parameter = new List<string>();
                for (int i = 2; i < args.Length; i++)
                {
                    parameter.Add(args[i]);
                }
                if (typeName == "SocketServer")
                {
                     new SocketServer(parameter.ToArray());
                }
                else
                {
                    Logger.Info(new CliProgramEnumerationType().EumType(typeName, methodName, parameter.ToArray()));
                }
            }
        }
    }
}
