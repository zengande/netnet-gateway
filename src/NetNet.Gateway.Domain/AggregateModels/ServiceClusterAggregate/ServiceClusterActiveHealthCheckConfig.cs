using Volo.Abp.Domain.Values;

namespace NetNet.Gateway.AggregateModels.ServiceClusterAggregate;

/// <summary>
/// 主动健康检查
/// </summary>
public class ServiceClusterActiveHealthCheckConfig : ValueObject
{
    /// <summary>
    /// 是否启用主动健康检查
    /// </summary>
    public bool? Enabled { get; set; }

    /// <summary>
    /// 间隔秒数
    /// </summary>
    public int? IntervalSeconds { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 健康检测策略
    /// </summary>
    public string? Policy { get; set; }

    /// <summary>
    /// 超时秒数
    /// </summary>
    public int? TimeoutSeconds { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Enabled;
        yield return IntervalSeconds;
        yield return Path;
        yield return Policy;
        yield return TimeoutSeconds;
    }
}
