using System.Text.Json.Nodes;
using JiaoLongWMI.Constants;
using JiaoLongWMI.Services;

namespace JiaoLongWMI.Repositories;

/// <summary>
/// 键盘相关操作的仓库类。
/// </summary>
public class Keyboard
{
    /// <summary>
    /// 键盘颜色相关操作。
    /// </summary>
    public class Color
    {
        /// <summary>
        /// 获取键盘颜色。
        /// </summary>
        /// <returns>包含红、绿、蓝分量的 JsonObject。</returns>
        public static JsonObject Get()
        {
            var json = new JsonObject();
            Tuple<int, int, int> tuple = MethodServices.GetValue<Tuple<int, int, int>>(MethodName.RGBKeyboardColor);
            json["red"] = tuple.Item1;
            json["green"] = tuple.Item2;
            json["blue"] = tuple.Item3;
            return json;
        }

        /// <summary>
        /// 设置键盘颜色。
        /// </summary>
        /// <param name="red">红色分量。</param>
        /// <param name="green">绿色分量。</param>
        /// <param name="blue">蓝色分量。</param>
        /// <returns>是否设置成功。</returns>
        public static bool Set(byte red, byte green, byte blue)
        {
            return MethodServices.SetValue(MethodName.RGBKeyboardColor, new byte[3]
            {
                red,
                green,
                blue
            });
        }
    }

    /// <summary>
    /// 键盘模式相关操作。
    /// </summary>
    public class Mode
    {
        /// <summary>
        /// 设置键盘模式为 RGB 固定模式。
        /// </summary>
        /// <returns>是否设置成功。</returns>
        public static bool Set()
        {
            return MethodServices.SetValue(MethodName.RGBKeyboardMode, RGBKeyboardMode.Mode_RGBFixedMode);
        }

        /// <summary>
        /// 获取键盘模式。
        /// </summary>
        /// <returns>键盘模式。</returns>
        public static RGBKeyboardMode Get()
        {
            return MethodServices.GetValue<RGBKeyboardMode>(MethodName.RGBKeyboardMode);
        }
    }

    /// <summary>
    /// 键盘亮度相关操作。
    /// </summary>
    public class LightBrightness
    {
        /// <summary>
        /// 获取键盘亮度。
        /// </summary>
        /// <returns>键盘亮度。</returns>
        public static RGBKeyboardBrightnessLevel Get()
        {
            return MethodServices.GetValue<RGBKeyboardBrightnessLevel>(MethodName.RGBKeyboardBrightness);
        }

        /// <summary>
        /// 设置键盘亮度。
        /// </summary>
        /// <param name="b">亮度值。</param>
        /// <returns>是否设置成功。</returns>
        public static bool Set(byte b)
        {
            if (b > 0 && b <= 4)
            {
                return MethodServices.SetValue(MethodName.RGBKeyboardBrightness, b);
            }
            return false;
        }
    }
}
