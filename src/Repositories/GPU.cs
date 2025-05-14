using JiaoLongWMI.Constants;
using JiaoLongWMI.Services;

namespace JiaoLongWMI.Repositories;

/// <summary>
/// GPU 相关操作的仓库类。
/// </summary>
public class GPU
{
    /// <summary>
    /// 设定是否为独显直连。
    /// </summary>
    /// <param name="mode">GPU 模式。</param>
    /// <returns>是否设置成功。</returns>
    public static bool Set(GPUMode mode)
    {
     return MethodServices.SetValue(MethodName.GPUMode, mode);
    }

    /// <summary>
    /// 获取 GPU 模式。
    /// </summary>
    /// <returns>GPU 模式。</returns>
    public static GPUMode Get()
    {
        return MethodServices.GetValue<GPUMode>(MethodName.GPUMode);
    }
}
