using JiaoLong16Pro.BLD.WMIOperation;

namespace JiaoLong16Pro.Models;

public class Keyboard
{
    /**
     * return float[] R G B
     */
    public static string GetRGBKeyboardColor()
    {
        Tuple<int, int, int> tuple = WMIMethodServices.GetValue<Tuple<int, int, int>>(WMIMethodName.RGBKeyboardColor);
        return $"{tuple.Item1}-{tuple.Item2}-{tuple.Item3}";
    }

    public static WMIRGBKeyboardBrightnessLevel GetkeyboardLightBrightness()
    {
        return WMIMethodServices.GetValue<WMIRGBKeyboardBrightnessLevel>(WMIMethodName.RGBKeyboardBrightness);
    }

    public static WMIRGBKeyboardMode GetKeyboardMode()
    {
        return WMIMethodServices.GetValue<WMIRGBKeyboardMode>(WMIMethodName.RGBKeyboardMode);
    }

    public static bool SetKeyboardMode()
    {
      return WMIMethodServices.SetValue(WMIMethodName.RGBKeyboardMode, WMIRGBKeyboardMode.Mode_RGBFixedMode);
    }
    public static bool SetRGBKeyboardColor(byte red, byte green, byte blue)
    {
        return WMIMethodServices.SetValue(WMIMethodName.RGBKeyboardColor, new byte[3]
        {
            red,
            green,
            blue
        });
    }

    public static bool SetkeyboardLightBrightness(byte b)
    {
        if (b > 4)
        {
            return WMIMethodServices.SetValue(WMIMethodName.RGBKeyboardBrightness, 4);
        }
        else if (b < 1)
        {
            return WMIMethodServices.SetValue(WMIMethodName.RGBKeyboardBrightness, 0);
        }
        else
        {
            return WMIMethodServices.SetValue(WMIMethodName.RGBKeyboardBrightness, b);
        }
    }
}