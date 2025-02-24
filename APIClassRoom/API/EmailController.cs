using APIClassRoom.Model;
using Microsoft.AspNetCore.Mvc;

namespace APIClassRoom.API;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        if (request == null)
            return BadRequest("Invalid request");

        // Implement your email sending logic here...
        return Ok("Email sent successfully");
    }
}
