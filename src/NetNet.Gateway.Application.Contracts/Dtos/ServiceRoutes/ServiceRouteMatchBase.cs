using System.ComponentModel.DataAnnotations;

namespace NetNet.Gateway.Dtos.ServiceRoutes;

public class ServiceRouteMatchBase<TMatchMode> where TMatchMode : Enum
{
    /// <summary>
    /// 模式
    /// </summary>
    [Required]
    public TMatchMode Mode { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 值（当 Mode = Exists 不可为空）
    /// </summary>
    public List<string>? Values { get; set; }

    /// <summary>
    /// 大小写是否敏感
    /// </summary>
    public bool IsCaseSensitive { get; set; }
}
