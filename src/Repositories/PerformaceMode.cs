
using JiaoLongWMI.Constants;
using JiaoLongWMI.Services;

namespace JiaoLongWMI.Repositories;

public class PerformaceMode
{
    public static bool Set(SystemPerMode mode)
    {
       return MethodServices.SetValue(MethodName.SystemPerMode, mode);
    }
    public static SystemPerMode Get()
    {
        return MethodServices.GetValue<SystemPerMode>(MethodName.SystemPerMode);
    }
}
