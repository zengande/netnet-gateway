using Microsoft.AspNetCore.Mvc;
using NetNet.Gateway.Distributed;
using NetNet.Gateway.Distributed.Models;
using Volo.Abp.AspNetCore.Mvc;

namespace NetNet.Gateway.AdminApi.Controllers;

[Route("/api/cluster")]
public class ClusterController : AbpController
{
    private readonly IYarpNodeManager _yarpNodeManager;

    public ClusterController(IYarpNodeManager yarpNodeManager)
    {
        _yarpNodeManager = yarpNodeManager;
    }

    [HttpGet("nodes")]
    public async Task<IEnumerable<ServerNode>> GetAllServerNodesAsync()
    {
        return _yarpNodeManager.GetAllServerNodesAsync().ToBlockingEnumerable();
    }
}
