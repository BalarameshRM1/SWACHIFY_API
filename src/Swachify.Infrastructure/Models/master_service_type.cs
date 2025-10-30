using System;
using System.Collections.Generic;

namespace Swachify.Infrastructure.Models;

public partial class master_service_type
{
    public long id { get; set; }

    public string service_type { get; set; } = null!;

    public decimal? price { get; set; }

    public int? hours { get; set; }

    public bool? is_active { get; set; }

    public virtual ICollection<master_service_mapping> master_service_mappings { get; set; } = new List<master_service_mapping>();

    public virtual ICollection<service_booking> service_bookings { get; set; } = new List<service_booking>();

    public virtual ICollection<service_tracking> service_trackings { get; set; } = new List<service_tracking>();
}
