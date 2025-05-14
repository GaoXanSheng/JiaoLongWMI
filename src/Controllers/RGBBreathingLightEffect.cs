using System.Drawing;
using JiaoLongWMI.Repositories;

namespace JiaoLongWMI.Controllers;

/// <summary>
/// RGB 呼吸灯效果控制器。
/// </summary>
class RGBBreathingLightEffect
{
    // 创建线程以同步运行效果
    // private Thread breathingEffectThread;
    private Thread gradientEffectThread ;
    // 用法示例:
    public RGBBreathingLightEffect()
    {
        // breathingEffectThread = new Thread(ApplyBreathingLightEffect);
        gradientEffectThread = new Thread(ApplyLoopingGradientColor);
    }

    /// <summary>
    /// 启动效果。
    /// </summary>
    public void Start()
    {
        // 启动线程
        // breathingEffectThread.Start();
        gradientEffectThread.Start();
    }
    /// <summary>
    /// 停止效果。
    /// </summary>
    public void Stop()
    {
        running = false;
    }
    private bool running = true;

    /// <summary>
    /// 模拟 LightBrightnessSet 函数。
    /// </summary>
    /// <param name="brightness">亮度值。</param>
    private void LightBrightnessSet(byte brightness)
    {
        // 在此处添加设置灯光亮度的逻辑
        Keyboard.LightBrightness.Set(brightness);
        Thread.Sleep(50); // 模拟延迟
    }

    /// <summary>
    /// 模拟 RGB_Set 函数。
    /// </summary>
    /// <param name="r">红色分量。</param>
    /// <param name="g">绿色分量。</param>
    /// <param name="b">蓝色分量。</param>
    private void RGB_Set(byte r, byte g, byte b)
    {
        // 在此处添加设置 RGB 颜色的逻辑
        Keyboard.Color.Set(r, g, b);
        Thread.Sleep(50); // 模拟延迟
    }

    /// <summary>
    /// 应用呼吸灯效果。
    /// </summary>
    private void ApplyBreathingLightEffect()
    {
        const byte minBrightness = 0; // 最小亮度级别
        const byte maxBrightness = 3; // 最大亮度级别
        const int delay = 500; // 每个亮度步长之间的延迟（毫秒）

        while (running)
        {
            // 增加亮度
            for (byte brightness = minBrightness; brightness < maxBrightness; brightness++)
            {
                 LightBrightnessSet(brightness);
                 Thread.Sleep(delay); // 等待指定的延迟
            }

            // 降低亮度
            for (byte brightness = maxBrightness; brightness > minBrightness; brightness--)
            {
                 LightBrightnessSet(brightness);
                 Thread.Sleep(delay); // 等待指定的延迟
            }
        }
    }

    /// <summary>
    /// 获取渐变颜色。
    /// </summary>
    /// <param name="startColor">起始颜色。</param>
    /// <param name="endColor">结束颜色。</param>
    /// <param name="percent">百分比。</param>
    /// <returns>渐变颜色。</returns>
    private Color GetGradientColor(Color startColor, Color endColor, float percent)
    {
        byte r = (byte)(startColor.R + (endColor.R - startColor.R) * percent);
        byte g = (byte)(startColor.G + (endColor.G - startColor.G) * percent);
        byte b = (byte)(startColor.B + (endColor.B - startColor.B) * percent);
        return Color.FromArgb(r, g, b);
    }
    /// <summary>
    /// 应用循环渐变颜色。
    /// </summary>
    private void ApplyLoopingGradientColor()
    {
        const int steps = 100;
        const int delay = 50; // 毫秒
        var colors = new[] {
            Color.Red, // 红色
            Color.Green, // 绿色
            Color.Blue  // 蓝色
        };
        while (running)
        {
            for (int i = 0; i < colors.Length && running; i++)
            {
                var startColor = colors[i];
                var endColor = colors[(i + 1) % colors.Length];

                for (int j = 0; j <= steps && running; j++)
                {
                    float percent = (float)j / steps;
                    var color = GetGradientColor(startColor, endColor, percent);
                    RGB_Set(color.R, color.G, color.B);
                    Thread.Sleep(delay); // 等待指定的延迟
                }
            }
        }
    }
}
