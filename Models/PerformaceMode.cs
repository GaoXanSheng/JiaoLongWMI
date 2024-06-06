using JiaoLong16Pro.BLD.WMIOperation;

namespace JiaoLong16Pro.Models;

public class PerformaceMode
{
    public static bool SetPerformaceMode(WMISystemPerMode mode)
    {
       return WMIMethodServices.SetValue(WMIMethodName.SystemPerMode, mode);
    }
    public static WMISystemPerMode GetPerformaceMode()
    {
        return WMIMethodServices.GetValue<WMISystemPerMode>(WMIMethodName.SystemPerMode);
    }
    public static bool CLISetPerformaceMode(byte b)
    {
        if (b == 0)
        {
           return SetPerformaceMode(WMISystemPerMode.BalanceMode);
        }
        else if (b == 1)
        {
            return SetPerformaceMode(WMISystemPerMode.PerformanceMode);
        }
        else if (b == 2)
        {
            return SetPerformaceMode(WMISystemPerMode.QuietMode);
        }
        else
        {
            return SetPerformaceMode(WMISystemPerMode.Unknow);
        }

    }
}