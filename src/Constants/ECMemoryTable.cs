namespace JiaoLongWMI.Constants;

/// <summary>
/// 定义 EC 内存表的枚举，包含 EC 地址端口、数据端口以及各种风扇相关的 RPM 值和设置。
/// </summary>
public enum ECMemoryTable : ushort
{
    /// <summary>
    /// EC 地址端口。
    /// </summary>
    EC_ADDR_PORT = 0x4E,
    /// <summary>
    /// EC 数据端口。
    /// </summary>
    EC_DATA_PORT = 0x4F,
    /// <summary>
    /// 风扇 1 的 RPM 等级。
    /// </summary>
    Fan1_RPM_Level = 0xC836,
    /// <summary>
    /// 风扇 2 的 RPM 等级。
    /// </summary>
    Fan2_RPM_Level = 0xC837,
    /// <summary>
    /// 风扇 1 的 RPM 值。
    /// </summary>
    Fan1_RPM = 0XC834,
    /// <summary>
    /// 风扇 2 的 RPM 值。
    /// </summary>
    Fan2_RPM = 0XC835,
    /// <summary>
    /// 风扇 1 的 RPM 设置。
    /// </summary>
    Fan1_RPM_SET = 0xC83C,
    /// <summary>
    /// 风扇 2 的 RPM 设置。
    /// </summary>
    Fan2_RPM_SET = 0xC83D,
    /// <summary>
    /// EC 版本。
    /// </summary>
    EC_Version = 0xC411,
}
