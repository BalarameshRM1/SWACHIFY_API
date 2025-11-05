using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1;
using Swachify.Application;
using Swachify.Application.DTOs;
using Swachify.Application.Interfaces;
using Swachify.Infrastructure.Data;

public class MasterService(MyDbContext db, IBookingService bookingService) : IMasterService
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

    public async Task<DashboardDtos> GetDashboardData(long id=0,CancellationToken cancellationToken = default)
    {
        var result = new DashboardDtos();
        result.servicesount = await db.master_departments.CountAsync();
        result.availableEmployeeCount = await db.user_registrations.CountAsync(u => u.is_active == true);

        if (id > 0)
        {
            result.pendingActivebookingcount = await db.service_bookings.CountAsync(d => d.assign_to == id && d.status_id == 2 && d.is_active == true);
        }
        else
        {
            result.pendingActivebookingcount = await db.service_bookings.CountAsync(d => d.status_id == 2 && d.is_active == true);
        }

        if (id > 0)
        {
            result.inprogressopenticketscount = await db.service_bookings.CountAsync(d => d.assign_to == id && d.status_id == 3 && d.is_active == true);
        }
        else
        {
            result.inprogressopenticketscount = await db.service_bookings.CountAsync(d => d.status_id == 3 && d.is_active == true);
        }
        if (id > 0)
        {
            result.Allbookings = await bookingService.GetAllBookingByUserIDAsync(0, id, 300, 0);
        }
        else
        {
            result.Allbookings = await bookingService.GetAllBookingsAsync(300, 0);
        }

        return result;
    }

    
}



