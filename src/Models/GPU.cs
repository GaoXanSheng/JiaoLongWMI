

using JiaoLongWMI.WMIOperation;
using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLong16Pro.Models;

public class GPU
{
    /**
     * 设定是否为独显直连
     */
    public static bool SetGpuMode(GPUMode mode)
    {
     return  MethodServices.SetValue(MethodName.GPUMode, mode);
    }
    public static GPUMode GetGpuMode()
    {
        return MethodServices.GetValue<GPUMode>(MethodName.GPUMode);
    }
    public static bool CLISetGpuMode(byte b)
    {
        if (b == 1)
        {
           return SetGpuMode(GPUMode.HybridMode);
        }
        else if (b == 0)
        {
           return SetGpuMode(GPUMode.DiscreteMode);
        }
        else
        {
           return SetGpuMode(GPUMode.Unknow);
        }

    }
}