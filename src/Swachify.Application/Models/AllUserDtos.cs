using Swachify.Infrastructure.Models;

namespace Swachify.Application.DTOs;
public class AllUserDtos
{
    public long id { get; set; }

    public string first_name { get; set; } = null!;

    public string last_name { get; set; } = null!;

    public long? role_id { get; set; }

    public string email { get; set; } = null!;

    public long? dept_id { get; set; }

    public string mobile { get; set; } = null!;

    public long? age { get; set; }

    public long? gender_id { get; set; }

    public bool? is_active { get; set; }
    public bool? is_assigned { get; set; }
    public long? location_id { get; set; }

    public List<string> depts { get; set; }
}
