using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceRoutes.Responses;

public class QueryServiceRouteRes : EntityDto<Guid>
{
    /// <summary>
    /// 路由名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 服务名称
    /// </summary>
    public string ServiceClusterName { get; set; }

    /// <summary>
    /// 匹配路径
    /// </summary>
    public string Path { get; set; }
}
