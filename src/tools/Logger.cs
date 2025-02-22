using System.Diagnostics.CodeAnalysis;

namespace JiaoLongWMI.tools;

public class Logger
{
    public static void Info<T>(T msg)
    {
        Console.WriteLine(msg);
    }
    
    public static void Info([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
    {
        Console.WriteLine(format,arg0);
    }
}