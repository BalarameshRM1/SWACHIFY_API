using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swachify.Application;
using Swachify.Application.Interfaces;

namespace Swachify.Api;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, IBookingService bookingService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> UserRegister(UserCommandDto userCommandDto)
    {
        try
        {
            var result = await userService.CreateUserAsync(userCommandDto);
            if (result == null)
                return Forbid();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("getallusers")]
    public async Task<IActionResult> GetAllUsers(AllusersDto cmd)
    {
        return Ok(await userService.GetAllUsersAsync(cmd));
    }

    [HttpGet("getuserbyid")]
    public async Task<IActionResult> GetUserByID(long id)
    {
        return Ok(await userService.GetUserByID(id));
    }
    [HttpPost("createemployee")]
    public async Task<IActionResult> CreateEmployee(EmpCommandDto empCommandDto)
    {
        try
        {
            var result = await userService.CreateEmployeAsync(empCommandDto);
            if (result == null)
                return Forbid();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("updateuser/{id}")]
    public async Task<IActionResult> UpdateUser(long id,EmpCommandDto empCommandDto)
    {
        try
        {
            var result = await userService.UpdateUserAsync(id,empCommandDto);
            if (result == null)
                return Forbid();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("assignemployee")]
    public async Task<IActionResult> AssignEmployee(AssignEmpDto commandDto)
    {
        var result = await bookingService.AssignEmployee(commandDto.id, commandDto.user_id);
        if (result == null) return Forbid();
        return Ok(result);
    }

    [HttpDelete("deleteuser")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        var result = await userService.DeleteUserAsync(id);
        if (result == null) return Forbid();
        return Ok(result);
    }

}