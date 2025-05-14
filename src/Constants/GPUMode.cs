namespace JiaoLongWMI.Constants
{
    /// <summary>
    /// 定义 GPU 模式的枚举。
    /// </summary>
    public enum GPUMode : byte
    {
        /// <summary>
        /// 混合模式。
        /// </summary>
        HybridMode = 0,
        /// <summary>
        /// 独立显卡模式。
        /// </summary>
        DiscreteMode = 1,
        /// <summary>
        /// 未知模式。
        /// </summary>
        Unknow = 255
    }
}
