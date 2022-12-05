using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using NetNet.Gateway.Dtos.ServiceClusters.Requests;
using NetNet.Gateway.Dtos.ServiceClusters.Responses;
using System.Diagnostics.CodeAnalysis;

namespace NetNet.Gateway.Admin.Pages.ServiceClusters;

public partial class ServiceClusterList
{
    [Inject] [NotNull] private IServiceClusterAppService? ServiceClusterAppService { get; set; }
    [Inject] [NotNull] private DialogService? DialogService { get; set; }
    [Inject] [NotNull] private NavigationManager? NavigationManager { get; set; }

    private Table<QueryServiceClusterRes>? Table { get; set; }

    private List<QueryServiceClusterRes>? Items { get; set; }

    protected override void OnInitialized()
    {
    }


    private async Task<QueryData<QueryServiceClusterRes>> OnQueryAsync(QueryPageOptions options)
    {
        var req = new QueryServiceClusterReq
        {
            MaxResultCount = options.PageItems, SkipCount = (options.PageIndex - 1) * options.PageItems, SearchKey = options.SearchText
        };
        var result = await ServiceClusterAppService.QueryAsync(req);

        return new() { TotalCount = (int)result.TotalCount, Items = result.Items };
    }

    private Func<QueryServiceClusterRes, Task>? OnDoubleClickRowCallback()
    {
        return row =>
        {
            Table!.ExpandDetailRow(row);
            return Task.CompletedTask;
        };
    }

    private void RedirectToEdit(QueryServiceClusterRes? context)
    {
        NavigationManager.NavigateTo($"/ServiceClusters/Edit/{context?.Id}");
    }

    private async Task<QueryData<QueryServiceDestinationRes>> OnQueryDetailAsync(QueryServiceClusterRes context, QueryPageOptions options)
    {
        return default;
    }

    private Task DeleteClusterAsync(QueryServiceClusterRes row)
    {
        return Task.CompletedTask;
    }
}
