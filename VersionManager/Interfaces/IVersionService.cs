using VersionManager.Entities;

namespace VersionManager.Interfaces;

public interface IVersionService
{
    Task<VersionTag?> GetCurrentVersionAsync(int projectId, string environment);
    Task<bool> SetVersionAsync(int projectId, string environment, int major, int minor, int patch);
    Task<VersionTag?> IncrementVersionAsync(int projectId, string environment, string part);
    Task<VersionTag?> DecrementVersionAsync(int projectId, string environment, string part);
}
