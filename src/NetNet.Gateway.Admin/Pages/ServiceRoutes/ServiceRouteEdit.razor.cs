using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using NetNet.Gateway.Dtos.ServiceRoutes.Requests;
using System.Diagnostics.CodeAnalysis;
using Yarp.ReverseProxy.Transforms.Builder;

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

    private IEnumerable<BreadcrumbItem> BreadcrumbItems { get; set; } = Enumerable.Empty<BreadcrumbItem>();
    private InputServiceRouteReq _input = new();

    [Parameter] public Guid? Id { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var currentText = "新建路由";
        if (Id.HasValue)
        {
            currentText = "编辑路由";
        }

        BreadcrumbItems = new[] { new BreadcrumbItem("路由管理", "/ServiceRoutes"), new BreadcrumbItem(currentText) };
    }
}
