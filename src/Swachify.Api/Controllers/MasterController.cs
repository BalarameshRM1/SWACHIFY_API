using Microsoft.AspNetCore.Mvc;
using Swachify.Application;

namespace Swachify.Api;

[ApiController]
[Route("api/[controller]")]
public class MasterController(IMasterService masterService) : ControllerBase
{
    [HttpGet("getallmasterData")]
    public async Task<IActionResult> GetAllMasterData()
    {
        return Ok(await masterService.GetAllMasterDatasAsync());
    }
    
}
