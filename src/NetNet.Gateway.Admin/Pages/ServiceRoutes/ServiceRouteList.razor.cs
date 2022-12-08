using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using NetNet.Gateway.Dtos.ServiceRoutes.Requests;
using NetNet.Gateway.Dtos.ServiceRoutes.Responses;
using System.Diagnostics.CodeAnalysis;

namespace NetNet.Gateway.Admin.Pages.ServiceRoutes;

public partial class ServiceRouteList
{
    [Inject, NotNull] private NavigationManager? NavigationManager { get; set; }
    [Inject, NotNull] private IServiceRouteAppService? ServiceRouteAppService { get; set; }

    [NotNull] private Table<QueryServiceRouteRes>? RouteTable { get; set; }

    private async Task<QueryData<QueryServiceRouteRes>> OnQueryAsync(QueryPageOptions arg)
    {
        var req = new QueryServiceRouteReq
        {
            MaxResultCount = arg.PageItems, SkipCount = (arg.PageIndex - 1) * arg.PageItems, SearchKey = arg.SearchText
        };

        var res = await ServiceRouteAppService.QueryAsync(req);

        return new() { TotalCount = (int)res.TotalCount, Items = res.Items };
    }

    private void RedirectToEdit(QueryServiceRouteRes? row)
    {
        NavigationManager.NavigateTo($"/ServiceRoutes/Edit/{row?.Id}");
    }

    private async Task DeleteAsync(QueryServiceRouteRes row)
    {
        await ServiceRouteAppService.DeleteAsync(row.Id);

        await RouteTable.QueryAsync();
    }
}
