using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.Ingress.Controllers;

[Route("[controller]/[action]")]
public class TestController : AbpController
{
    private readonly IProxyConfigProvider _configProvider;

    public TestController(IProxyConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }

    public IProxyConfig Get()
    {
        return _configProvider.GetConfig();
    }
}
