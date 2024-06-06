using JiaoLong16Pro.server;

namespace JiaoLong16Pro
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
                string[] ProgramArgs = args[0].Split("-");
                string ProgramTitle = ProgramArgs[0];
                string[] function = new string[99];
                for (int i = 1; i < ProgramArgs.Length; i++)
                {
                    function.SetValue(ProgramArgs[i], i - 1);
                }

                if (ProgramTitle=="Socket")
                {
                    socketServer socket = new socketServer(function);
                }
                else
                {
                    Console.WriteLine(new CLI_Program_Enumeration_Type().eumType(ProgramTitle, function));
                }
            }
        }
    }
}