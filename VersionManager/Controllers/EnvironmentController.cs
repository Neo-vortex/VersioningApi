using Microsoft.AspNetCore.Mvc;
using VersionManager.Dtos;
using VersionManager.Interfaces;

namespace VersionManager.Controllers;

    [ApiController]
    [Route("api/projects/{projectId}/environments")]
    public class EnvironmentController(IEnvironmentService environmentService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEnvironments(int projectId)
        {
            var environments = await environmentService.GetEnvironmentsByProjectAsync(projectId);
            if (!environments.Any())
                return NotFound(new { message = "No environments found for the specified project." });

            return Ok(environments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnvironment(int projectId, [FromBody] CreateEnvironmentRequest request)
        {
            var environmentId = await environmentService.CreateEnvironmentAsync(projectId, request.Name);
            if (environmentId == 0)
                return BadRequest(new { message = "Failed to create environment. Ensure the project exists." });

            return CreatedAtAction(nameof(GetEnvironments), new { projectId }, new { environmentId, request.Name });
        }

        [HttpPut("{environmentId:int}")]
        public async Task<IActionResult> UpdateEnvironment(int projectId, int environmentId, [FromBody] UpdateEnvironmentRequest request)
        {
            var updated = await environmentService.UpdateEnvironmentAsync(projectId, environmentId, request.Name);
            if (!updated)
                return NotFound(new { message = "Environment not found or could not be updated." });

            return Ok(new { message = "Environment updated successfully." });
        }

        [HttpDelete("{environmentId:int}")]
        public async Task<IActionResult> DeleteEnvironment(int projectId, int environmentId)
        {
            var deleted = await environmentService.DeleteEnvironmentAsync(projectId, environmentId);
            if (!deleted)
                return NotFound(new { message = "Environment not found or could not be deleted." });

            return Ok(new { message = "Environment deleted successfully." });
        }
    }
