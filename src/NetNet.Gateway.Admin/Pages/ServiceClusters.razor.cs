using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using NetNet.Gateway.Dtos.ServiceClusters.Requests;
using NetNet.Gateway.Dtos.ServiceClusters.Responses;

namespace NetNet.Gateway.Admin.Pages;

public partial class ServiceClusters
{
    [Inject] private IServiceClusterAppService ServiceClusterAppService { get; set; }

    private Table<QueryServiceClusterRes>? Table { get; set; }

    private List<QueryServiceClusterRes>? Items { get; set; }

    protected override void OnInitialized()
    {
        Items = Enumerable.Range(1, 100)
            .Select(x => new QueryServiceClusterRes()
            {
                Name = $"Service Cluster {x}",
                Source = "dashboard",
                Services = new() { new() { Version = "version1", Address = "http://localhost:5000" } }
            }).ToList();
    }


    private async Task<QueryData<QueryServiceClusterRes>> OnQueryAsync(QueryPageOptions options)
    {
        var req = new QueryServiceClusterReq { MaxResultCount = options.PageItems, SkipCount = (options.PageIndex - 1) * options.PageItems };
        var result = await ServiceClusterAppService.QueryAsync(req);

        return new() { TotalCount = (int)result.TotalCount, Items = result.Items };
    }

    private Func<QueryServiceClusterRes, Task>? OnDoubleClickRowCallback()
    {
        return foo =>
        {
            Table!.ExpandDetailRow(foo);
            return Task.CompletedTask;
        };
    }

    private IEnumerable<QueryServiceInfoRes> GetDetailDataSource(QueryServiceClusterRes row) => row.Services;
}
