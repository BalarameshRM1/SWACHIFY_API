using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swachify.Application;
using Swachify.Infrastructure.Data;
using Swachify.Infrastructure.Models;

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

    public async Task<bool> CreateMasterService(MaserServiceDto maserServiceDto)
    {
        // var departResult = await db.master_departments.FirstOrDefaultAsync(d => d.department_name == maserServiceDto.department_name);
        // if (departResult == null) return false;

        // var depart = new master_department
        // {
        //     department_name = maserServiceDto.department_name,
        //     is_active = true,
        // };
        // await db.SaveChangesAsync();

        // db.master_departments.Add(depart);
        // var serviceResult = await db.master_services.FirstOrDefaultAsync(d => d.service_name == maserServiceDto.service_name);
        // if (serviceResult == null) return false;
        // var services = new master_service
        // {
        //     service_name = maserServiceDto.service_name,
        //     is_active = true,
        //     dept_id = depart.id,
        // };
        // db.master_services.Add(services);
        // await db.SaveChangesAsync();

        return true;
    }
}



