using Microsoft.AspNetCore.Components;
using NetNet.Gateway.Admin.Shared;

namespace NetNet.Gateway.Admin.Components;

public class PageHeader : ComponentBase, IDisposable
{
    [CascadingParameter] public MainLayout MainLayout { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override void OnInitialized()
    {
        MainLayout.SetPageHeader(this);
        base.OnInitialized();
    }


    public void Dispose()
    {
        MainLayout?.SetPageHeader(null);
    }
}
