using Microsoft.AspNetCore.Mvc;
using Swachify.Application;
using Swachify.Application.DTOs;
namespace Swachify.Api;

[ApiController]
[Route("api/[controller]")]
public class LoginController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult> Login(loginDtos req)
    {
        if (!(string.IsNullOrEmpty(req.email) && string.IsNullOrEmpty(req.password)))
        {
            var data = await authService.ValidateCredentialsAsync(req.email, req.password);
            if (data == null)
            {
                return Unauthorized("Invalid username or password");
            }
            else
            {
                return Ok(data);
            }
        }
        else
        {
            return Unauthorized("Invalid username or password");
        }
    }
    [HttpPost("forgotpassword")]
    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        var result = await authService.ForgotPasswordAsync(
            request.Email,
            request.Password,
            request.ConfirmPassword
        );

        if (result == "Password updated successfully.")
            return Ok(new { message = result });

        return BadRequest(new { message = result });
    }

 [HttpPost("forgotpasswordlink")]
    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordLinkDto request)
    {
        var result = await authService.ForgotpasswordLink(
            request.Email
        );

        if (result)
            return Ok(result);

        return BadRequest(new { message = result });
    }


}
