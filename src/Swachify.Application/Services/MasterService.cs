using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swachify.Application;
using Swachify.Infrastructure.Data;

public class MasterService(MyDbContext db) : IMasterService
{
    public async Task<AllMasterDataDtos> GetAllMasterDatasAsync()
    {
        await using var conn = db.Database.GetDbConnection();
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = DbConstants.fn_get_all_masters_data;
        var result = await cmd.ExecuteScalarAsync(); // gets JSON result as string
        if (result == null || result == DBNull.Value)
            return null;
        var json = result.ToString();
        var data = JsonConvert.DeserializeObject<AllMasterDataDtos>(json);
        return data;
    }
}

