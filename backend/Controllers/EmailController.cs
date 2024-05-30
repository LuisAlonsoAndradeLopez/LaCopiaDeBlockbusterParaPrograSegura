using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backendnet.Services;

namespace backendnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController(EmailService emailService) : ControllerBase
    {

        [HttpPost] 
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (ModelState.IsValid)
            {
                await emailService.SendEmailAsync(emailRequest.Email);
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost("verify")]
        public IActionResult VerifyCode([FromBody] VerifyRequest verifyRequest)
        {
            if (ModelState.IsValid)
            {
                bool isValid = emailService.VerifyCode(verifyRequest.Email, verifyRequest.Code);
                if (isValid)
                {
                    return Ok();
                }
                return BadRequest("Código de verificación incorrecto.");
            }
            return BadRequest(ModelState);
        }

    }
    

    public class EmailRequest
    {
        public string? Email { get; set; }
    }

    public class VerifyRequest
    {
        public string? Email { get; set; }
        public string? Code { get; set; }
    }

}