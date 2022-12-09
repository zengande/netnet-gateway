namespace NetNet.Gateway.Dtos.ServiceClusters;

public class ServiceClusterActiveHealthCheckConfigDto
{
    /// <summary>
    /// 是否启用
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
}
