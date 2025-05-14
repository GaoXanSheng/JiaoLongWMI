namespace JiaoLongWMI.Constants
{
    /// <summary>
    /// 定义 CPU 功耗状态的枚举。
    /// </summary>
    public enum CPUPower : byte
    {
        /// <summary>
        /// 关闭状态。
        /// </summary>
        CloseState = 0,
        /// <summary>
        /// 开启状态。
        /// </summary>
        OpenState,
        /// <summary>
        /// SPL 状态。
        /// </summary>
        SPLState,
        /// <summary>
        /// SPPT 状态。
        /// </summary>
        SPPTState,
        /// <summary>
        /// CPU 温度墙状态。
        /// </summary>
        CPUTempWallState,
        /// <summary>
        /// 未知状态。
        /// </summary>
        Unknow = 255
    }
}
