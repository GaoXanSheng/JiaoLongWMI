using JiaoLongWMI.Constants;
using JiaoLongWMI.Services;

namespace JiaoLongWMI.Repositories;

/// <summary>
/// CPU 相关操作的仓库类。
/// </summary>
public class CPU
{
    /// <summary>
    /// 设置 CPU 低负载电压。
    /// </summary>
    /// <param name="LongPower">低负载电压值。</param>
    /// <returns>是否设置成功。</returns>
    public static bool SetCpuShortPower(byte LongPower)
    {
        return MethodServices.SetValue(MethodName.CPUPower, new byte[2]
        {
            (byte)CPUPower.SPLState,
            LongPower
        });
    }

    /// <summary>
    /// 开启或关闭自定义模式。
    /// </summary>
    /// <param name="Open">是否开启自定义模式。</param>
    /// <returns>是否设置成功。</returns>
    public static bool OpenCustomMode(Boolean Open)
    {
        if (Open) {
            return MethodServices.SetValue(MethodName.CPUPower, CPUPower.OpenState);
        }
        return MethodServices.SetValue(MethodName.CPUPower, CPUPower.CloseState);
    }

    /// <summary>
    /// 获取自定义模式状态。
    /// </summary>
    /// <returns>自定义模式是否开启。</returns>
    public static bool GetCustomMode()
    {
        var res =  MethodServices.GetValue<CPUPower>(MethodName.CPUPower);
        if (res == CPUPower.OpenState)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 设置 CPU 全核心满载电压。
    /// </summary>
    /// <param name="ShortPower">全核心满载电压值。</param>
    /// <returns>是否设置成功。</returns>
    public static bool SetCpuLongPower(byte ShortPower)
    {
        return MethodServices.SetValue(MethodName.CPUPower, new byte[2]
        {
            (byte)CPUPower.SPPTState,
            ShortPower
        });
    }

    /// <summary>
    /// 设置 CPU 温控墙。
    /// </summary>
    /// <param name="tempwall">CPU 温控墙温度值。</param>
    /// <returns>是否设置成功。</returns>
    public static bool SetCPUTempWall(byte tempwall)
    {
        return MethodServices.SetValue(MethodName.CPUPower, new byte[2]
        {
            (byte)CPUPower.CPUTempWallState,
            tempwall
        });
    }
}
