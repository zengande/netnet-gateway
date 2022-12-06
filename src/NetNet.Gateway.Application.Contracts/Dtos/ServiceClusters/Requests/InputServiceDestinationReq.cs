using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters.Requests;

public class InputServiceDestinationReq : EntityDto<Guid>
{
    /// <summary>
    /// key
    /// </summary>
    [Required]
    public string Key { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [Required]
    public string Address { get; set; }

    /// <summary>
    /// 健康检测地址
    /// </summary>
    public string Health { get; set; } = string.Empty;

    /// <summary>
    /// 元数据
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();
}
