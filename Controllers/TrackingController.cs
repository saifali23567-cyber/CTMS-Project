using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Models;
using ProjectAPI.Services;


namespace ProjectAPI.Controllers;

[ApiController]
[Route("api/tracking")]
public class TrackingController : ControllerBase
{
    private static List<Tracking> trackingList = new();

    [HttpPost("create")]
    public IActionResult CreateTracking(int trackingId)
    {
        var tracking = new Tracking
        {
            TrackingID = trackingId,
            TrackingNumber = TrackingService.GenerateTrackingNumber(),
            Status = "Created",
            UpdatedAt = DateTime.Now
        };

        trackingList.Add(tracking);
        return Ok(tracking);
    }

    [HttpPost("update")]
    public IActionResult UpdateTracking(string trackingNumber, string location, string status)
    {
        var track = trackingList.FirstOrDefault(t => t.TrackingNumber == trackingNumber);
        if (track == null) return NotFound();

        track.Location = location;
        track.Status = status;
        track.UpdatedAt = DateTime.Now;

        return Ok(track);
    }

    [HttpGet("{trackingNumber}")]
    public IActionResult Track(string trackingNumber)
    {
        var updates = trackingList.Where(t => t.TrackingNumber == trackingNumber);
        return Ok(updates);
    }
}