
using JiaoLongWMI.WMIOperation.Method;
using JiaoLongWMI.WMIOperation.System;

namespace JiaoLongWMI.Models;

public class PerformaceMode
{
    public static bool SetPerformaceMode(SystemPerMode mode)
    {
       return MethodServices.SetValue(MethodName.SystemPerMode, mode);
    }
    public static SystemPerMode GetPerformaceMode()
    {
        return MethodServices.GetValue<SystemPerMode>(MethodName.SystemPerMode);
    }
    public static bool CLISetPerformaceMode(byte b)
    {
        if (b == 0)
        {
           return SetPerformaceMode(SystemPerMode.BalanceMode);
        }
        else if (b == 1)
        {
            return SetPerformaceMode(SystemPerMode.PerformanceMode);
        }
        else if (b == 2)
        {
            return SetPerformaceMode(SystemPerMode.QuietMode);
        }
        else
        {
            return SetPerformaceMode(SystemPerMode.Unknow);
        }

    }
}