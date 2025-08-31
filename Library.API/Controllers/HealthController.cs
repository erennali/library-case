using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var report = await _healthCheckService.CheckHealthAsync();

        return report.Status == HealthStatus.Healthy
            ? Ok(new { status = "Healthy", timestamp = DateTime.UtcNow })
            : StatusCode(503, new { status = "Unhealthy", timestamp = DateTime.UtcNow });
    }

    [HttpGet("detailed")]
    public async Task<IActionResult> GetDetailed()
    {
        var report = await _healthCheckService.CheckHealthAsync();

        return Ok(new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            })
        });
    }

    [HttpGet("ready")]
    public async Task<IActionResult> GetReadiness()
    {
        var report = await _healthCheckService.CheckHealthAsync(reg => reg.Tags.Contains("ready"));

        return report.Status == HealthStatus.Healthy
            ? Ok(new { status = "Ready", timestamp = DateTime.UtcNow })
            : StatusCode(503, new { status = "Not Ready", timestamp = DateTime.UtcNow });
    }

    [HttpGet("live")]
    public async Task<IActionResult> GetLiveness()
    {
        var report = await _healthCheckService.CheckHealthAsync(reg => reg.Tags.Contains("live"));

        return report.Status == HealthStatus.Healthy
            ? Ok(new { status = "Alive", timestamp = DateTime.UtcNow })
            : StatusCode(503, new { status = "Not Alive", timestamp = DateTime.UtcNow });
    }

    [HttpGet("database")]
    public async Task<IActionResult> GetDatabaseHealth()
    {
        var report = await _healthCheckService.CheckHealthAsync(reg => reg.Tags.Contains("database"));

        return Ok(new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            database = report.Entries.TryGetValue("database", out var dbEntry) ? dbEntry.Status.ToString() : "Unknown"
        });
    }

    [HttpGet("storage")]
    public async Task<IActionResult> GetStorageHealth()
    {
        var report = await _healthCheckService.CheckHealthAsync(reg => reg.Tags.Contains("storage"));

        return Ok(new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            storage = report.Entries.TryGetValue("storage", out var storageEntry) ? storageEntry.Status.ToString() : "Unknown"
        });
    }
}

