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
            if (args.Length != 0)
            {
                string[] programArgs = args[0].Split("-");
                string programTitle = programArgs[0];
                string[] function = new string[99];
                for (int i = 1; i < programArgs.Length; i++)
                {
                    function.SetValue(programArgs[i], i - 1);
                }

                if (programTitle=="Socket")
                {
                    SocketServer socket = new SocketServer(function);
                }
                else
                {
                    Console.WriteLine(new CliProgramEnumerationType().EumType(programTitle, function));
                }
            }
        }
    }
}