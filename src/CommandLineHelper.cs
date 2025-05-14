using System;
using System.Collections.Generic;
using System.Text;

namespace JiaoLongWMI
{
    /// <summary>
    /// 命令行帮助程序类，用于显示命令行参数的提示信息。
    /// </summary>
    public static class CommandLineHelper
    {
        /// <summary>
        /// 显示命令行参数的提示信息。
        /// </summary>
        public static void ShowUsage()
        {
            Console.WriteLine("JiaoLongWMI 命令行工具");
            Console.WriteLine("用法：JiaoLongWMI <typeName> <methodName> [--parameter <参数列表>]");
            Console.WriteLine();
            Console.WriteLine("  typeName: 类型名称");
            Console.WriteLine("  methodName: 方法名称");
            Console.WriteLine("  --parameter: 可选参数列表");
            Console.WriteLine();
            Console.WriteLine("参数详细说明：");
            Console.WriteLine("  CPU:");
            Console.WriteLine("    SetCpuShortPower: 设置 CPU 短时间功耗限制，参数类型：byte，范围：0-255，单位：瓦");
            Console.WriteLine("      示例：JiaoLongWMI CPU SetCpuShortPower --parameter 100");
            Console.WriteLine("    SetCpuLongPower: 设置 CPU 长时间功耗限制，参数类型：byte，范围：0-255，单位：瓦");
            Console.WriteLine("      示例：JiaoLongWMI CPU SetCpuLongPower --parameter 100");
            Console.WriteLine("    OpenCustomMode: 开启/关闭自定义模式，参数类型：bool，true 表示开启，false 表示关闭");
            Console.WriteLine("      示例：JiaoLongWMI CPU OpenCustomMode --parameter true");
            Console.WriteLine("    GetCustomMode: 获取当前自定义模式的状态，无参数");
            Console.WriteLine("      示例：JiaoLongWMI CPU GetCustomMode");
            Console.WriteLine("    SetCPUTempWall: 设置 CPU 温度墙，参数类型：byte，范围：0-100，单位：摄氏度");
            Console.WriteLine("      示例：JiaoLongWMI CPU SetCPUTempWall --parameter 80");
            Console.WriteLine();
            Console.WriteLine("  Fan:");
            Console.WriteLine("    GetFanSpeed: 获取风扇转速，无参数");
            Console.WriteLine("      示例：JiaoLongWMI Fan GetFanSpeed");
            Console.WriteLine("    SetFanSpeed: 设置风扇转速，参数类型：string，范围：0-255 风扇转速会自动乘以100");
            Console.WriteLine("      示例：JiaoLongWMI Fan SetFanSpeed --parameter 20");
            Console.WriteLine("    SetMaxFanSpeedSwitch: 设置最大风扇转速开关，参数类型：string，true 表示开启，false 表示关闭");
            Console.WriteLine("      示例：JiaoLongWMI Fan SetMaxFanSpeedSwitch --parameter true");
            Console.WriteLine("    GetMaxFanSpeedSwitch: 获取最大风扇转速开关状态，无参数");
            Console.WriteLine("      示例：JiaoLongWMI Fan GetMaxFanSpeedSwitch");
            Console.WriteLine();
            Console.WriteLine("  Keyboard:");
            Console.WriteLine("    ColorSet: 设置键盘颜色，参数类型：byte byte byte，分别表示红、绿、蓝颜色分量，范围：0-255");
            Console.WriteLine("      示例：JiaoLongWMI Keyboard ColorSet --parameter 255 0 0");
            Console.WriteLine("    ColorGet: 获取当前键盘颜色，无参数");
            Console.WriteLine("      示例：JiaoLongWMI Keyboard ColorGet");
            Console.WriteLine("    ModeSet: 设置键盘模式，无参数");
            Console.WriteLine("      示例：JiaoLongWMI Keyboard ModeSet");
            Console.WriteLine("    ModeGet: 获取当前键盘模式，无参数");
            Console.WriteLine("      示例：JiaoLongWMI Keyboard ModeGet");
            Console.WriteLine("    LightBrightnessGet: 获取当前键盘亮度，无参数");
            Console.WriteLine("      示例：JiaoLongWMI Keyboard LightBrightnessGet");
            Console.WriteLine("    LightBrightnessSet: 设置键盘亮度，参数类型：byte，范围：0-4");
            Console.WriteLine("      示例：JiaoLongWMI Keyboard LightBrightnessSet --parameter 1");
            Console.WriteLine();
            Console.WriteLine("  GPUMode:");
            Console.WriteLine("    Get: 获取当前 GPU 模式，无参数");
            Console.WriteLine("      示例：JiaoLongWMI GPUMode Get");
            Console.WriteLine();
            Console.WriteLine("  LogoLight:");
            Console.WriteLine("    Set: 设置 Logo 灯状态，参数类型：ResultState (枚举类型)，可选值：On, Off");
            Console.WriteLine("      示例：JiaoLongWMI LogoLight Set --parameter On");
            Console.WriteLine("    Get: 获取当前 Logo 灯状态，无参数");
            Console.WriteLine("      示例：JiaoLongWMI LogoLight Get");
            Console.WriteLine();
            Console.WriteLine("  PerformaceMode:");
            Console.WriteLine("    Get: 获取当前性能模式，无参数");
            Console.WriteLine("      示例：JiaoLongWMI PerformaceMode Get");
            Console.WriteLine();
            Console.WriteLine("  RGBBreathingLightEffect:");
            Console.WriteLine("    Start: 启动 RGB 呼吸灯效果，无参数");
            Console.WriteLine("      示例：JiaoLongWMI RGBBreathingLightEffect Start");
            Console.WriteLine("    Stop: 停止 RGB 呼吸灯效果，无参数");
            Console.WriteLine("      示例：JiaoLongWMI RGBBreathingLightEffect Stop");
            Console.WriteLine();
        }
    }
}
