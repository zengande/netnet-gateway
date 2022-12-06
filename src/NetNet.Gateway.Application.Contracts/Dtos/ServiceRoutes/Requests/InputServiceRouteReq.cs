using System.ComponentModel.DataAnnotations;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.Dtos.ServiceRoutes.Requests;

public class InputServiceRouteReq
{
    [Required]
    public string Name { get; set; }

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

    public IEnumerable<ServiceRouteTransformDto> Transforms { get; set; } = new List<ServiceRouteTransformDto>();
}
