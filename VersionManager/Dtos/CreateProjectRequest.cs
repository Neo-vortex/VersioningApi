namespace VersionManager.Dtos;

public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public List<string> Environments { get; set; } = [];
}