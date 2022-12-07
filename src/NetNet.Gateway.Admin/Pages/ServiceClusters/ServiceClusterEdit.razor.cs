using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetNet.Gateway.Dtos.ServiceClusters.Requests;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Guids;

namespace NetNet.Gateway.Admin.Pages.ServiceClusters;

public partial class ServiceClusterEdit
{
    [Inject] [NotNull] private IServiceClusterAppService? ServiceClusterAppService { get; set; }
    [Inject] [NotNull] private NavigationManager? NavigationManager { get; set; }
    [Inject] [NotNull] private IGuidGenerator? GuidGenerator { get; set; }
    [NotNull] private IEnumerable<BreadcrumbItem>? BreadcrumbItems { get; set; } = Enumerable.Empty<BreadcrumbItem>();
    [NotNull] private Table<InputServiceDestinationReq>? DestinationTable { get; set; }
    [Parameter] public Guid? Id { get; set; }

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

    private async Task FetchServiceClusterEditDataAsync(Guid id)
    {
        var serviceCluster = await ServiceClusterAppService.GetAsync(id);
        _input = new()
        {
            Name = serviceCluster.Name,
            Description = serviceCluster.Description,
            LoadBalancingPolicy = serviceCluster.LoadBalancingPolicy,
            Destinations = serviceCluster.Destinations.Select(x => new InputServiceDestinationReq
            {
                Id = x.Id,
                Key = x.Key,
                Address = x.Address,
                Health = x.Health,
                Metadata = x.Metadata
            })
        };
    }

    private async Task OnValidSubmit(EditContext context)
    {
        var req = (InputServiceClusterReq)context.Model;
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
