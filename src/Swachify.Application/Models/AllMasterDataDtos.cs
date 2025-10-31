using Newtonsoft.Json;
using Swachify.Infrastructure.Models;

namespace Swachify.Application;

public class AllMasterDataDtos
{
    [JsonProperty("departments")]
    public List<MasterDepartmentDto> Departments { get; set; } = new();

    [JsonProperty("roles")]
    public List<MasterRoleDto> Roles { get; set; } = new();

    [JsonProperty("slots")]
    public List<MasterSlotDto> Slots { get; set; } = new();

    [JsonProperty("gender")]
    public List<MasterGenderDto> Genders { get; set; } = new();

    [JsonProperty("locations")]
    public List<MasterLocationDto> Locations { get; set; } = new();

    [JsonProperty("status")]
    public List<masterStatusDto> Status { get; set; } = new();
}

public class MasterDepartmentDto
{
    [JsonProperty("department_id")]
    public long? DepartmentId { get; set; }

    [JsonProperty("department_name")]
    public string? DepartmentName { get; set; }

    [JsonProperty("service_id")]
    public long? ServiceId { get; set; }

    [JsonProperty("service_name")]
    public string? ServiceName { get; set; }

    [JsonProperty("service_type_id")]
    public long? ServiceTypeId { get; set; }

    [JsonProperty("service_type")]
    public string? ServiceType { get; set; }

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("hours")]
    public int? Hours { get; set; }
}

public class MasterRoleDto
{
    [JsonProperty("id")]
    public long? RoleId { get; set; }

    [JsonProperty("role_name")]
    public string? RoleName { get; set; }
}

public class MasterSlotDto
{
    [JsonProperty("id")]
    public long? SlotId { get; set; }

    [JsonProperty("slot_time")]
    public string? SlotTime { get; set; }

    [JsonProperty("is_active")]
    public bool? IsActive { get; set; }

}

public class MasterGenderDto
{
    [JsonProperty("id")]
    public long? GenderId { get; set; }

    [JsonProperty("gender_name")]
    public string? GenderName { get; set; }

    [JsonProperty("is_active")]
    public bool? IsActive { get; set; }
}

public class MasterLocationDto
{
    [JsonProperty("id")]
    public long? LocationId { get; set; }

    [JsonProperty("location_name")]
    public string? LocationName { get; set; }
    [JsonProperty("is_active")]
    public bool? IsActive { get; set; }
}

public class masterStatusDto
{
    [JsonProperty("id")]
    public long? StatusId { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }
    [JsonProperty("is_active")]
    public bool? IsActive { get; set; }
}

public record MaserServiceDto(string department_name,string service_name, string cleaning_type_name,decimal price);