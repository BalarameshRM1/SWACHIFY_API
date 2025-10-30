using Swachify.Infrastructure.Models;

namespace Swachify.Application;

public interface IMasterService
{
    Task<AllMasterDataDtos> GetAllMasterDatasAsync();
}