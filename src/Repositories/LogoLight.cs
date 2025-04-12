using JiaoLongWMI.Constants;
using JiaoLongWMI.Services;

namespace JiaoLongWMI.Repositories;

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
