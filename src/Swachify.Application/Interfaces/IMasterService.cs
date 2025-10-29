using Swachify.Infrastructure.Models;

namespace Swachify.Application;

public interface IMasterService
{
    Task<List<master_service>> GetAllServicesAsync();
    Task<List<master_location>> GetAllLocationsAsync();
    Task<List<master_role>> GetAllRolesAsync();
    Task<List<master_department>> GetAllDepartmentsAsync();
    Task<List<master_slot>> GetAllSlots();
    Task<List<master_status>> GetAllStatuses();

    Task<List<master_service_type>> GetAllServiceTypes();
}