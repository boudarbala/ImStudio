namespace ImStudio.Web.Models;

public sealed class UiObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = "Button";
    public string Label { get; set; } = "Button";
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; } = 120;
    public int Height { get; set; } = 32;
}
