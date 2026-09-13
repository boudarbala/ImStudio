namespace ImStudio.Web.Models;

public sealed class Layout
{
    public string Name { get; set; } = "My Layout";
    public int Width { get; set; } = 1000;
    public int Height { get; set; } = 700;
    public List<UiObject> Objects { get; set; } = new();
}
