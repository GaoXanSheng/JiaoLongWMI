namespace JiaoLongWMI.Constants
{
    /// <summary>
    /// 定义系统性能模式的枚举。
    /// </summary>
    public enum SystemPerMode : byte
    {
        BalanceMode,
        PerformanceMode,
        QuietMode,
        /// <summary>
        /// 未知模式。
        /// </summary>
        Unknow = 255
    }
}
