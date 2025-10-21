﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swachify.Application;

namespace Swachify.Api;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> UserRegister(UserCommandDto userCommandDto)
    {
        var result = await userService.CreateUserAsync(userCommandDto);
        if (result == null) return Forbid();
        return Ok(result);
    }

    [HttpGet("getallusers")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await userService.GetAllUsersAsync());
    }

    [HttpGet("getuserbyid")]
    public async Task<IActionResult> GetUserByID(long id)
    {
        return Ok(await userService.GetUserByID(id));
    }


    [HttpGet("getallusersByDept")]
    public async Task<IActionResult> GetAllUsersByDept(long deptId)
    {
        return Ok(await userService.GetAllUsersByDept(deptId));
    }

    [HttpPost("createemployee")]
    public async Task<IActionResult> CreateEmployee(EmpCommandDto empCommandDto)
    {
        var result = await userService.CreateEmployeAsync(empCommandDto);
        if (result == null) return Forbid();
        return Ok(result);
    }

    [HttpPost("assignemployee")]
    public async Task<IActionResult> AssignEmployee(AssignEmpDto commandDto)
    {
        var result = await userService.AssignEmployee(commandDto.id,commandDto.user_id);
        if (result == null) return Forbid();
        return Ok(result);
    }


}