using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetNet.Gateway.Dtos.ServiceClusters.Requests;
using System.Diagnostics.CodeAnalysis;

namespace NetNet.Gateway.Admin.Pages.ServiceClusters;

public partial class ServiceClusterEdit
{
    [Inject] [NotNull] private IServiceClusterAppService? ServiceClusterAppService { get; set; }
    [Inject] [NotNull] private NavigationManager? NavigationManager { get; set; }
    [NotNull] private IEnumerable<BreadcrumbItem>? BreadcrumbItems { get; set; } = Enumerable.Empty<BreadcrumbItem>();
    [Parameter] public long? Id { get; set; }

    private InputServiceClusterReq _input = new();

    protected override async Task OnParametersSetAsync()
    {
        var currentText = "新建服务";
        if (Id.HasValue)
        {
            currentText = "编辑服务";

            await FetchServiceClusterEditDataAsync(Id.Value);
        }

        BreadcrumbItems = new[] { new BreadcrumbItem("服务管理", "/ServiceClusters"), new BreadcrumbItem(currentText) };
    }

    private async Task FetchServiceClusterEditDataAsync(long id)
    {
        var serviceCluster = await ServiceClusterAppService.GetAsync(id);
        _input = new() { Name = serviceCluster.Name, Description = serviceCluster.Description, };
    }

    private async Task OnValidSubmit(EditContext context)
    {
        if (context.Model is InputServiceClusterReq req)
        {
            if (Id.HasValue)
            {
                await ServiceClusterAppService.UpdateAsync(Id.Value, req);
            }
            else
            {
                await ServiceClusterAppService.CreateAsync(req);
            }

            NavigationManager.NavigateTo("/ServiceClusters");
        }
    }
}
