using JiaoLong16Pro.BLD.WMIOperation;

namespace JiaoLong16Pro.Models;

public class LogoLight
{
    public static bool SetLogoLight(WMIResultState m)
    {
        return WMIMethodServices.SetValue(WMIMethodName.Ambientlight, m);
    }
    public static WMIResultState GetLogoLight()
    {
        return WMIMethodServices.GetValue<WMIResultState>(WMIMethodName.Ambientlight);
    }
    public static bool CLISetLogoLight(byte b)
    {
        if (b == 1)
        {
            return WMIMethodServices.SetValue(WMIMethodName.Ambientlight, WMIResultState.ON);
        }
        else if (b == 0)
        {
            return WMIMethodServices.SetValue(WMIMethodName.Ambientlight, WMIResultState.OFF);
        }
        else
        {
            return WMIMethodServices.SetValue(WMIMethodName.Ambientlight, WMIResultState.Unknow);
        }
    }
}