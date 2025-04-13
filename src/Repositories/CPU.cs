using JiaoLongWMI.Constants;
using JiaoLongWMI.Services;

namespace JiaoLongWMI.Repositories;

public class CPU
{
    /**
     * 低负载电压
     */
    public static bool SetCpuShortPower(byte LongPower)
    {
        return MethodServices.SetValue(MethodName.CPUPower, new byte[2]
        {
            (byte)CPUPower.SPLState,
            LongPower
        });
    }

    /**
     * 自定义模式
     */
    public static bool OpenCustomMode(Boolean Open)
    {
        if (Open) {
            return MethodServices.SetValue(MethodName.CPUPower, CPUPower.OpenState);
        }
        return MethodServices.SetValue(MethodName.CPUPower, CPUPower.CloseState);
    }
    public static bool GetCustomMode()
    {
		    var res =  MethodServices.GetValue<CPUPower>(MethodName.CPUPower);
		    if (res == CPUPower.OpenState)
		    {
			    return true;
		    }

		    return false;
    }

    /**
     * 全核心满载电压
     */
    public static bool SetCpuLongPower(byte ShortPower)
    {
        return MethodServices.SetValue(MethodName.CPUPower, new byte[2]
        {
            (byte)CPUPower.SPPTState,
            ShortPower
        });
    }

    /**
     * CPU温控
     */
    public static bool SetCPUTempWall(byte tempwall)
    {
        return MethodServices.SetValue(MethodName.CPUPower, new byte[2]
        {
            (byte)CPUPower.CPUTempWallState,
            tempwall
        });
    }
}
