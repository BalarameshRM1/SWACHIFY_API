using System;
using System.Collections.Generic;

namespace Swachify.Infrastructure.Models;

public partial class service_tracking
{
    public long id { get; set; }

    public long service_booking_id { get; set; }

    public long? status_id { get; set; }

    public long? created_by { get; set; }

    public DateTime? created_date { get; set; }

    public long? modified_by { get; set; }

    public DateTime? modified_date { get; set; }

    public bool? is_active { get; set; }

    public long? dept_id { get; set; }

    public long? service_id { get; set; }

    public long? service_type_id { get; set; }

    public string? booking_id { get; set; }

    public string? room_sqfts { get; set; }

    public bool? with_basement { get; set; }

    public decimal? with_basement_price { get; set; }

    public virtual master_department? dept { get; set; }

    public virtual master_service? service { get; set; }

    public virtual service_booking service_booking { get; set; } = null!;

    public virtual master_service_type? service_type { get; set; }

    public virtual master_status? status { get; set; }
}
