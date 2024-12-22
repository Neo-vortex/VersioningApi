using Microsoft.AspNetCore.Mvc;
using VersionManager.Dtos;
using VersionManager.Interfaces;

namespace VersionManager.Controllers;

[ApiController]
[Route("api/projects/{projectId:int}/{environment}/version")]
public class VersionController(IVersionService versionService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCurrentVersion(int projectId, string environment)
    {
        var version = await versionService.GetCurrentVersionAsync(projectId, environment);
        if (version == null)
            return NotFound(new { message = "Version not found for the specified project and environment." });

        return Ok(new
        {
            projectId,
            environment,
            version = $"{version.Major}.{version.Minor}.{version.Patch}"
        });
    }

    [HttpPut]
    public async Task<IActionResult> SetVersion(int projectId, string environment, [FromBody] SetVersionRequest request)
    {
        var updated =
            await versionService.SetVersionAsync(projectId, environment, request.Major, request.Minor, request.Patch);
        if (!updated)
            return NotFound(new { message = "Project or environment not found." });

        return Ok(new { message = "Version updated successfully." });
    }

    [HttpPost("increment")]
    public async Task<IActionResult> IncrementVersion(int projectId, string environment,
        [FromBody] IncrementVersionRequest request)
    {
        var updatedVersion = await versionService.IncrementVersionAsync(projectId, environment, string.IsNullOrWhiteSpace( request.Part) ? "patch" : request.Part );
        if (updatedVersion == null)
            return NotFound(new { message = "Project or environment not found." });

        return Ok(new
        {
            message = "Version incremented successfully.",
            version = $"{updatedVersion.Major}.{updatedVersion.Minor}.{updatedVersion.Patch}"
        });
    }

    [HttpPost("decrement")]
    public async Task<IActionResult> DecrementVersion(int projectId, string environment,
        [FromBody] DecrementVersionRequest request)
    {
        var updatedVersion = await versionService.DecrementVersionAsync(projectId, environment, request.Part);
        if (updatedVersion == null)
            return NotFound(new { message = "Project or environment not found." });

        return Ok(new
        {
            message = "Version decremented successfully.",
            version = $"{updatedVersion.Major}.{updatedVersion.Minor}.{updatedVersion.Patch}"
        });
    }
}