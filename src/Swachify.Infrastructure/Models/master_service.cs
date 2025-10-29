using System;
using System.Collections.Generic;

namespace Swachify.Infrastructure.Models;

public partial class master_service
{
    public long id { get; set; }

    public string service_name { get; set; } = null!;

    public bool? is_active { get; set; }

    public long? dept_id { get; set; }

    public virtual master_department? dept { get; set; }

    public virtual ICollection<service_tracking> service_trackings { get; set; } = new List<service_tracking>();

    public virtual ICollection<user_registration> user_registrations { get; set; } = new List<user_registration>();
}
