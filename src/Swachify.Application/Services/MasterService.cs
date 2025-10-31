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

    public async Task<bool> CreateMasterService(MaserServiceDto cmdinput)
    {
        await using var conn = db.Database.GetDbConnection();
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();

        if (cmdinput.department_id > 0 && cmdinput?.service_id > 0 && cmdinput?.service_type_id > 0)
        {
            cmd.CommandText = DbConstants.fn_create_master_departments
     .Replace("{department_id}", cmdinput.department_id.ToString())
     .Replace("{department_name}", cmdinput.department_name)
     .Replace("{service_id}", cmdinput.service_id.ToString())
     .Replace("{service_name}", cmdinput.service_name)
     .Replace("{service_type_id}", cmdinput.service_type_id.ToString())
     .Replace("{service_type}", cmdinput.service_type_name)
     .Replace("{price}", cmdinput.price.ToString());
        }
        else
        {
            cmd.CommandText = DbConstants.fn_create_master_departments
     .Replace("{department_name}", cmdinput.department_name)
     .Replace("{service_name}", cmdinput.service_name)
     .Replace("{service_type}", cmdinput.service_type_name)
     .Replace("{price}", cmdinput.price.ToString());
        }

        var result = await cmd.ExecuteScalarAsync(); // gets JSON result as string
        if (result == null || result == DBNull.Value)
            return false;
        return true;
    }
}



