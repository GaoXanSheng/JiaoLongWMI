using JiaoLongWMI.WMIOperation;
using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLongWMI.Models;

public class LogoLight
{
    public static bool Set(ResultState m)
    {
        return MethodServices.SetValue(MethodName.Ambientlight, m);
    }

    public static ResultState Get()
    {
        return MethodServices.GetValue<ResultState>(MethodName.Ambientlight);
    }
}