namespace VersionManager.Dtos;

public class SetVersionRequest
{
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }
}