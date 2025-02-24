/*using APIClassRoom.Storage;
using Microsoft.AspNetCore.Mvc;

namespace APIClassRoom.API;

[Route("api/ClassReservations")]
[ApiController]
public class CompensationReservation : ControllerBase
{
    private readonly ClassCompensationStorage _classCompensationStorage;

    public CompensationReservation(ClassCompensationStorage classCompensationStorage)
    {
        _classCompensationStorage = classCompensationStorage ?? throw new ArgumentNullException(nameof(classCompensationStorage));
    }

    [HttpGet("{levelId}")]
    public async Task<IActionResult> GetReservationByLevelId(int levelId)
    {
        var reservation = await _classCompensationStorage.GetReservationByIdAsync(levelId);
        if (reservation == null)
        {
            return NotFound(); // Causes a 404 error if no reservation is found
        }
        return Ok(reservation);
    }
}*/