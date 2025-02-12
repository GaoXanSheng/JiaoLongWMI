
using JiaoLongWMI.WMIOperation.Keyboard;
using JiaoLongWMI.WMIOperation.Method;

namespace JiaoLongWMI.Models;

public class Keyboard
{
    /**
     * return float[] R G B
     */
    public static string GetRGBKeyboardColor()
    {
        Tuple<int, int, int> tuple = MethodServices.GetValue<Tuple<int, int, int>>(MethodName.RGBKeyboardColor);
        return $"{tuple.Item1}-{tuple.Item2}-{tuple.Item3}";
    }

    public static RGBKeyboardBrightnessLevel GetkeyboardLightBrightness()
    {
        return MethodServices.GetValue<RGBKeyboardBrightnessLevel>(MethodName.RGBKeyboardBrightness);
    }

    public static RGBKeyboardMode GetKeyboardMode()
    {
        return MethodServices.GetValue<RGBKeyboardMode>(MethodName.RGBKeyboardMode);
    }

    public static bool SetKeyboardMode()
    {
      return MethodServices.SetValue(MethodName.RGBKeyboardMode, RGBKeyboardMode.Mode_RGBFixedMode);
    }
    public static bool SetRGBKeyboardColor(byte red, byte green, byte blue)
    {
        return MethodServices.SetValue(MethodName.RGBKeyboardColor, new byte[3]
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
            return MethodServices.SetValue(MethodName.RGBKeyboardBrightness, 4);
        }
        else if (b < 1)
        {
            return MethodServices.SetValue(MethodName.RGBKeyboardBrightness, 0);
        }
        else
        {
            return MethodServices.SetValue(MethodName.RGBKeyboardBrightness, b);
        }
    }
}