using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using NetNet.Gateway.Dtos.ServiceRoutes.Responses;
using System.Diagnostics.CodeAnalysis;

namespace NetNet.Gateway.Admin.Pages.ServiceRoutes;

public partial class ServiceRouteList
{
    [Inject] [NotNull] private NavigationManager? NavigationManager { get; set; }

    private async Task<QueryData<QueryServiceRouteRes>> OnQueryAsync(QueryPageOptions arg)
    {
        var count = 100;
        return new()
        {
            TotalCount = count,
            Items = Enumerable.Range(0, arg.PageItems)
                .Select(x => new QueryServiceRouteRes()
                {
                    Id = Guid.NewGuid(), Name = "Route_" + x, ServiceClusterName = "Service_" + x % 2, Path = "/api/{**remind}"
                })
        };
    }

    private void RedirectToEdit(QueryServiceRouteRes? row)
    {
        NavigationManager.NavigateTo($"/ServiceRoutes/Edit/{row?.Id}");
    }

    private Task DeleteAsync(QueryServiceRouteRes row)
    {
        return Task.CompletedTask;
    }
}
