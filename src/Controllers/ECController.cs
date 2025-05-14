using JiaoLongWMI.Constants;
using JiaoLongWMI.Utils;

namespace JiaoLongWMI.Controllers;

/// <summary>
/// EC 控制器，用于控制 EC 相关的操作，如风扇转速等。
/// </summary>
public class ECController : WinIo
{
    /// <summary>
    /// 构造函数，初始化 EC。
    /// </summary>
    public ECController()
    {
        EC_init();
    }

    /// <summary>
    /// 初始化 EC。
    /// </summary>
    public void EC_init()
    {
        byte EC_CHIP_ID1 = EC_RAM_READ(0x2000);
        if (EC_CHIP_ID1 == 0x55)
        {
            byte val = EC_RAM_READ(0x1060);
            val = (byte)(val | 0x80);
            EC_RAM_WRITE(0x1060, val); // enable EC RAM
        }
    }

    /// <summary>
    /// 设置风扇 1 的转速。
    /// </summary>
    /// <param name="speed">风扇转速。</param>
    public void Fan1SetSpeed(byte speed)
    {
        EC_RAM_WRITE((ushort)ECMemoryTable.Fan1_RPM_SET, speed);
        var mask = EC_RAM_READ(0xB20) | 0x02;
        EC_RAM_WRITE(0xB20, (byte)mask);
    }

    /// <summary>
    /// 设置风扇 2 的转速。
    /// </summary>
    /// <param name="speed">风扇转速。</param>
    public void Fan2SetSpeed(byte speed)
    {
        EC_RAM_WRITE((ushort)ECMemoryTable.Fan2_RPM_SET, speed);
        var mask = EC_RAM_READ(0xB20) | 0x08;
        EC_RAM_WRITE(0xB20, (byte)mask);
    }
}
