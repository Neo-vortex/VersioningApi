namespace VersionManager.Entities;

public class VersionTag
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }
    public DateTime UpdatedAt { get; set; }
}