using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway;

public delegate void ConfigChangedHandler();

public interface IReverseProxyStore
{
    public event ConfigChangedHandler OnConfigChanged;

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    IProxyConfig GetConfig();

    /// <summary>
    /// 配置已变更 invoke ChangeConfig
    /// </summary>
    void RaiseConfigChanged();

    /// <summary>
    /// 重新加载配置
    /// </summary>
    void ReloadConfig();

    /// <summary>
    /// 获取 ChangeToken
    /// </summary>
    /// <returns></returns>
    IChangeToken GetReloadToken();
}
