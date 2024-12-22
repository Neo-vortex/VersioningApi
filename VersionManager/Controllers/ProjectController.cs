using Microsoft.AspNetCore.Mvc;
using VersionManager.Interfaces;

namespace VersionManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController(IProjectService projectService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] string projectName)
    {
        if (string.IsNullOrWhiteSpace(projectName))
        {
            return BadRequest("Project name is required.");
        }

        try
        {
            var projectId = await projectService.CreateProjectAsync(projectName);
            return CreatedAtAction(nameof(GetProjectById), new { id = projectId }, new { projectId, projectName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var project = await projectService.GetProjectByIdAsync(id);

        if (project == null)
        {
            return NotFound($"Project with ID {id} not found.");
        }

        return Ok(project);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var deleted = await projectService.DeleteProjectAsync(id);

        if (!deleted)
        {
            return NotFound($"Project with ID {id} not found or could not be deleted.");
        }

        return NoContent();
    }

    [HttpPost("{projectId:int}/Environment")]
    public async Task<IActionResult> CreateEnvironment(int projectId, [FromBody] string environmentName)
    {
        if (string.IsNullOrWhiteSpace(environmentName))
        {
            return BadRequest("Environment name is required.");
        }

        try
        {
            var environmentId = await projectService.CreateEnvironmentAsync(projectId, environmentName);
            return CreatedAtAction(nameof(GetEnvironments), new { projectId = projectId }, new { environmentId, environmentName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{projectId:int}/Environment")]
    public async Task<IActionResult> GetEnvironments(int projectId)
    {
        var environments = await projectService.GetEnvironmentsAsync(projectId);

        if (!environments.Any())
        {
            return NotFound($"No environments found for project with ID {projectId}.");
        }

        return Ok(environments);
    }
}