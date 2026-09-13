namespace ImStudio.Web.Models;

public sealed class LegalRelation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SourceNodeId { get; set; }
    public Guid TargetNodeId { get; set; }
    public string Type { get; set; } = "REFERS_TO";
    public string? Label { get; set; }
}
