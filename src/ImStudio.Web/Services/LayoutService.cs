using ImStudio.Web.Models;

namespace ImStudio.Web.Services;

public sealed class LayoutService : ILayoutService
{
    public Layout CreateDefault() => new()
    {
        Objects =
        {
            new UiObject { Type = "Text", Label = "Hello ImStudio", X = 40, Y = 40, Width = 220 },
            new UiObject { Type = "Button", Label = "Click Me", X = 40, Y = 100 }
        }
    };
}
