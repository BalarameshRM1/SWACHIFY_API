using Swachify.Infrastructure.Models;

namespace Swachify.Application.DTOs;
public class AllUserDtos
{
    public long user_id { get; set; }

    public string user_name { get; set; } = null!;

    public long? role_id { get; set; }
    public string? role_name { get; set; }

    public string email { get; set; } = null!;

    public string mobile { get; set; } = null!;

    public long? age { get; set; }

    public long? gender_id { get; set; }

    public bool? is_assigned { get; set; }
    public long? location_id { get; set; }
    public string? location_name { get; set; }
    public string dept_id { get; set; }
    public string department_name { get; set; }
}
