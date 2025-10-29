using System;
using System.Collections.Generic;

namespace Swachify.Infrastructure.Models;

public partial class master_department
{
    public long id { get; set; }

    public string department_name { get; set; } = null!;

    public bool? is_active { get; set; }

    public virtual ICollection<master_service> master_services { get; set; } = new List<master_service>();

    public virtual ICollection<service_tracking> service_trackings { get; set; } = new List<service_tracking>();

    public virtual ICollection<user_department> user_departments { get; set; } = new List<user_department>();

    public virtual ICollection<user_registration> user_registrations { get; set; } = new List<user_registration>();
}
