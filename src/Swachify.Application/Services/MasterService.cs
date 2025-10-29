using Microsoft.EntityFrameworkCore;
using Swachify.Infrastructure.Data;
using Swachify.Infrastructure.Models;

namespace Swachify.Application;

public class MasterService(MyDbContext db) : IMasterService
{
    public async Task<List<master_location>> GetAllLocationsAsync()
    {
        return await db.master_locations.ToListAsync();
    }

    public async Task<List<master_service>> GetAllServicesAsync()
    {
        return await db.master_services.ToListAsync();
    }

    public async Task<List<master_department>> GetAllDepartmentsAsync()
    {
        return await db.master_departments.ToListAsync();
    }

    public async Task<List<master_role>> GetAllRolesAsync()
    {
        return await db.master_roles.ToListAsync();
    }

     public async Task<List<master_slot>> GetAllSlots()
    {
        return await db.master_slots.ToListAsync();
    }
    public async Task<List<master_status>> GetAllStatuses()
    {
        return await db.master_statuses.ToListAsync();
    }
    
    public async Task<List<master_service_type>> GetAllServiceTypes()
    {
        return await db.master_service_types.ToListAsync();
    }
}