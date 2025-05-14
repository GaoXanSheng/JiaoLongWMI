using JiaoLongWMI.Constants;
using JiaoLongWMI.Services;

namespace JiaoLongWMI.Repositories;

/// <summary>
/// Logo 灯相关操作的仓库类。
/// </summary>
public class LogoLight
{
    /// <summary>
    /// 设置 Logo 灯状态。
    /// </summary>
    /// <param name="m">Logo 灯状态。</param>
    /// <returns>是否设置成功。</returns>
    public static bool Set(ResultState m)
    {
        return MethodServices.SetValue(MethodName.Ambientlight, m);
    }

    /// <summary>
    /// 获取 Logo 灯状态。
    /// </summary>
    /// <returns>Logo 灯状态。</returns>
    public static ResultState Get()
    {
        return MethodServices.GetValue<ResultState>(MethodName.Ambientlight);
    }
}
