using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetNet.Gateway.Dtos.ServiceRoutes.Requests;
using System.Diagnostics.CodeAnalysis;

namespace NetNet.Gateway.Admin.Pages.ServiceRoutes;

public partial class ServiceRouteEdit
{
    private static readonly IEnumerable<SelectedItem> HttpMethodSelectItems = new List<SelectedItem>()
    {
        new(HttpMethods.Get, HttpMethods.Get),
        new(HttpMethods.Post, HttpMethods.Post),
        new(HttpMethods.Put, HttpMethods.Put),
        new(HttpMethods.Delete, HttpMethods.Delete),
        new(HttpMethods.Options, HttpMethods.Options),
        new(HttpMethods.Patch, HttpMethods.Patch),
        new(HttpMethods.Trace, HttpMethods.Trace),
        new(HttpMethods.Connect, HttpMethods.Connect),
        new(HttpMethods.Head, HttpMethods.Head)
    };

    private IEnumerable<BreadcrumbItem> _breadcrumbItems = Enumerable.Empty<BreadcrumbItem>();
    private IEnumerable<SelectedItem> _serviceClusterSelections = Enumerable.Empty<SelectedItem>();
    private InputServiceRouteReq _input = new();
    private Dictionary<string, string> _selectedTransform = new();
    [Inject, NotNull] public IServiceRouteAppService? ServiceRouteAppService { get; set; }
    [Inject, NotNull] public IServiceClusterAppService? ServiceClusterAppService { get; set; }
    [Inject, NotNull] public NavigationManager? NavigationManager { get; set; }
    [Parameter] public Guid? Id { get; set; }
    [NotNull] private Modal? EditTransformModel { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await ServiceClusterAppService.QueryAsync(new() { MaxResultCount = 1000 });
        _serviceClusterSelections = result.Items.Select(x => new SelectedItem(x.Id.ToString(), x.Name));
    }

    protected override async Task OnParametersSetAsync()
    {
        var currentText = "新建路由";
        if (Id.HasValue)
        {
            currentText = "编辑路由";

            var route = await ServiceRouteAppService.GetAsync(Id.Value);

            _input = new InputServiceRouteReq() { Name = route.Name };
        }

        _breadcrumbItems = new[] { new BreadcrumbItem("路由管理", "/ServiceRoutes"), new BreadcrumbItem(currentText) };
    }

    private async Task OnSubmit(EditContext arg)
    {
        var req = (InputServiceRouteReq)arg.Model;

        if (Id.HasValue)
        {
            await ServiceRouteAppService.UpdateAsync(Id.Value, req);
        }
        else
        {
            await ServiceRouteAppService.CreateAsync(req);
        }

        NavigationManager.NavigateTo("/ServiceRoutes");
    }
}
