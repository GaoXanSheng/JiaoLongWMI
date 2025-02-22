using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLong16Pro.Models;

public class GPUMode
{
    /**
     * 设定是否为独显直连
     */
    public static bool Set(JiaoLongWMI.WMIOperation.GPUMode mode)
    {
     return MethodServices.SetValue(MethodName.GPUMode, mode);
    }
    public static JiaoLongWMI.WMIOperation.GPUMode Get()
    {
        return MethodServices.GetValue<JiaoLongWMI.WMIOperation.GPUMode>(MethodName.GPUMode);
    }
}