using JiaoLongWMI.Constants;
using JiaoLongWMI.Services;

namespace JiaoLongWMI.Repositories;

/// <summary>
/// 性能模式相关操作的仓库类。
/// </summary>
public class PerformaceMode
{
    /// <summary>
    /// 设置系统性能模式。
    /// </summary>
    /// <param name="mode">系统性能模式。</param>
    /// <returns>是否设置成功。</returns>
    public static bool Set(SystemPerMode mode)
    {
       return MethodServices.SetValue(MethodName.SystemPerMode, mode);
    }

    /// <summary>
    /// 获取系统性能模式。
    /// </summary>
    /// <returns>系统性能模式。</returns>
    public static SystemPerMode Get()
    {
        return MethodServices.GetValue<SystemPerMode>(MethodName.SystemPerMode);
    }
}
