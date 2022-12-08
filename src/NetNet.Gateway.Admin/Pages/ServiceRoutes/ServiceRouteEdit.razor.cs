using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetNet.Gateway.Dtos.ServiceRoutes;
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
    private int _selectedTransformGroupIndex = -1;
    private IEnumerable<ServiceRouteTransformDto> _selectedTransforms = Enumerable.Empty<ServiceRouteTransformDto>();
    [Inject, NotNull] public IServiceRouteAppService? ServiceRouteAppService { get; set; }
    [Inject, NotNull] public IServiceClusterAppService? ServiceClusterAppService { get; set; }
    [Inject, NotNull] public NavigationManager? NavigationManager { get; set; }
    [Parameter] public Guid? Id { get; set; }
    [NotNull] private Modal? EditTransformModal { get; set; }
    [NotNull] private Table<ServiceRouteTransformDto>? TransformTable { get; set; }

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

            _input = new InputServiceRouteReq()
            {
                Name = route.Name,
                Order = route.Order,
                AuthorizationPolicy = route.AuthorizationPolicy,
                CorsPolicy = route.CorsPolicy,
                MatchHosts = route.MatchHosts,
                MatchMethods = route.MatchMethods,
                MatchHeaders = route.MatchHeaders,
                MatchPath = route.MatchPath,
                MatchQueryParameters = route.MatchQueryParameters,
                ServiceClusterId = route.ServiceClusterId,
                Transforms = route.Transforms
            };
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

    private Task OnEditTransform(int groupIndex = -1)
    {
        _selectedTransformGroupIndex = groupIndex;
        _selectedTransforms = _input.Transforms.GetValueOrDefault(_selectedTransformGroupIndex)
                              ?? Enumerable.Empty<ServiceRouteTransformDto>();

        return EditTransformModal.Toggle();
    }

    private Task<bool> OnSaveTransform()
    {
        var flag = true;

        if (!_input.Transforms.ContainsKey(_selectedTransformGroupIndex))
        {
            var maxIndex = _input.Transforms.Keys.Any() ? _input.Transforms.Keys.Max() + 1 : 0;

            _input.Transforms.Add(maxIndex, _selectedTransforms.ToList());
        }
        else
        {
            _input.Transforms[_selectedTransformGroupIndex] = _selectedTransforms.ToList();
        }

        return Task.FromResult(flag);
    }

    private Task OnEditTransformModalCloseAsync()
    {
        _selectedTransformGroupIndex = -1;
        _selectedTransforms = Enumerable.Empty<ServiceRouteTransformDto>();


        StateHasChanged();
        return Task.CompletedTask;
    }
}
