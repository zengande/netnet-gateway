using Microsoft.AspNetCore.Components;
using NetNet.Gateway.Distributed;
using NetNet.Gateway.Distributed.Models;
using System.Diagnostics.CodeAnalysis;

namespace NetNet.Gateway.Admin.Pages;

public partial class Index
{
    [Inject, NotNull] public IYarpNodeManager? YarpNodeManager { get; set; }
    [Inject, NotNull] public CurrentNodeInfo? CurrentNodeInfo { get; set; }

    public List<ServerNode> Nodes { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await foreach (var node in YarpNodeManager.GetAllServerNodesAsync())
        {
            Nodes.Add(node);
        }
    }

    private Dictionary<string, string> _dic = new() { { "k1", "v1" }, { "k2", "v2" }, { "k3", "v3" }, };

    private IEnumerable<string> _strings = new List<string>() { "1", "2", "3" };

}
