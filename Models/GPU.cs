using JiaoLong16Pro.BLD.WMIOperation;

namespace JiaoLong16Pro.Models;

public class GPU
{
    /**
     * 设定是否为独显直连
     */
    public static bool SetGpuMode(WMIGPUMode mode)
    {
     return  WMIMethodServices.SetValue(WMIMethodName.GPUMode, mode);
    }
    public static WMIGPUMode GetGpuMode()
    {
        return WMIMethodServices.GetValue<WMIGPUMode>(WMIMethodName.GPUMode);
    }
    public static bool CLISetGpuMode(byte b)
    {
        if (b == 1)
        {
           return SetGpuMode(WMIGPUMode.HybridMode);
        }
        else if (b == 0)
        {
           return SetGpuMode(WMIGPUMode.DiscreteMode);
        }
        else
        {
           return SetGpuMode(WMIGPUMode.Unknow);
        }

    }
}