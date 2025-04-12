using JiaoLongWMI.Constants;
using JiaoLongWMI.Services;

namespace JiaoLongWMI.Repositories;

public class GPU
{
    /**
     * 设定是否为独显直连
     */
    public static bool Set(GPUMode mode)
    {
     return MethodServices.SetValue(MethodName.GPUMode, mode);
    }
    public static GPUMode Get()
    {
        return MethodServices.GetValue<GPUMode>(MethodName.GPUMode);
    }
}
