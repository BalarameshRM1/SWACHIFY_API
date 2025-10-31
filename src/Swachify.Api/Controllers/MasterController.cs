

using Microsoft.AspNetCore.Mvc;
using Swachify.Application;

[ApiController]
[Route("api/[controller]")]
public class MasterController(IMasterService masterService) : ControllerBase
{
    [HttpGet("getallmasterData")]
    public async Task<IActionResult> GetAllMasterData()
    {
        return Ok(await masterService.GetAllMasterDatasAsync());
    }

    [HttpPost("createmasterData")]
    public async Task<IActionResult> GetAllMasterData(MaserServiceDto cmd)
    {
        var result = await masterService.CreateMasterService(cmd);
        return Ok(result);
    }
    
}
