using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWasmApp;

public partial class App
{
    private static RenderFragment ShowFound(RouteData routeData)
    {
        return Build;

        void Build(RenderTreeBuilder builder)
        {
            builder.OpenComponent<FoundView>(0);
            builder.AddAttribute(1, "RouteData", routeData);
            builder.CloseComponent();
        }
    }
}
