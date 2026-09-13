namespace ImStudio.Web.Models;

public sealed class LegalNode
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = "Article";
    public string Label { get; set; } = "Article 1";
    public string? Identifier { get; set; }
    public string? Source { get; set; }
    public string? Language { get; set; } = "fr";
    public string? Text { get; set; }
}
