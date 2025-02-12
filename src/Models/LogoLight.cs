

using JiaoLongWMI.WMIOperation;
using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLongWMI.Models;

public class LogoLight
{
    public static bool SetLogoLight(ResultState m)
    {
        return MethodServices.SetValue(MethodName.Ambientlight, m);
    }
    public static ResultState GetLogoLight()
    {
        return MethodServices.GetValue<ResultState>(MethodName.Ambientlight);
    }
    public static bool CLISetLogoLight(byte b)
    {
        if (b == 1)
        {
            return MethodServices.SetValue(MethodName.Ambientlight, ResultState.ON);
        }
        else if (b == 0)
        {
            return MethodServices.SetValue(MethodName.Ambientlight, ResultState.OFF);
        }
        else
        {
            return MethodServices.SetValue(MethodName.Ambientlight, ResultState.Unknow);
        }
    }
}