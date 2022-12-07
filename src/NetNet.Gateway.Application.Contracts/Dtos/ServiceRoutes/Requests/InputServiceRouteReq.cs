using System.ComponentModel.DataAnnotations;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.Dtos.ServiceRoutes.Requests;

public class InputServiceRouteReq
{
    /// <summary>
    /// 路由名称
    /// </summary>
    [Required] public string Name { get; set; }

    /// <summary>
    /// 服务id
    /// </summary>
    [Required]
    public Guid ServiceClusterId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? Order { get; set; }

    /// <summary>
    /// 授权策略 Default/Anonymous
    /// </summary>
    public string? AuthorizationPolicy { get; set; }

    /// <summary>
    /// 跨域策略 Default/Disable
    /// </summary>
    public string? CorsPolicy { get; set; }

    /// <summary>
    /// 匹配请求主机
    /// </summary>
    public List<string>? MatchHosts { get; set; }

    /// <summary>
    /// 匹配请求方法
    /// </summary>
    public List<string>? MatchMethods { get; set; }

    /// <summary>
    /// 匹配请求路径
    /// </summary>
    public string? MatchPath { get; set; }

    /// <summary>
    /// 匹配请求头
    /// </summary>
    public IEnumerable<ServiceRouteMatchBase<HeaderMatchMode>>? MatchHeaders { get; set; }
        = new List<ServiceRouteMatchBase<HeaderMatchMode>>();

    /// <summary>
    /// 匹配请求参数
    /// </summary>
    public IEnumerable<ServiceRouteMatchBase<QueryParameterMatchMode>>? MatchQueryParameters { get; set; }
        = new List<ServiceRouteMatchBase<QueryParameterMatchMode>>();

    /// <summary>
    /// 请求转换
    /// </summary>
    public IEnumerable<Dictionary<string, string>> Transforms { get; set; } = new List<Dictionary<string, string>>();
}
