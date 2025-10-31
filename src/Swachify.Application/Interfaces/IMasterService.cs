using Swachify.Infrastructure.Models;

namespace Swachify.Application;

public interface IMasterService
{
    Task<AllMasterDataDtos> GetAllMasterDatasAsync();

    Task<bool> CreateMasterService(MaserServiceDto cmdinput);
}