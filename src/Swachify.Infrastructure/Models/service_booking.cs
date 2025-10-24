using System;
using System.Collections.Generic;

namespace Swachify.Infrastructure.Models;

public partial class service_booking
{
    public long id { get; set; }

    public string? booking_id { get; set; }

    public long dept_id { get; set; }

    public long service_id { get; set; }

    public long slot_id { get; set; }

    public long? created_by { get; set; }

    public DateTime? created_date { get; set; }

    public long? modified_by { get; set; }

    public DateTime? modified_date { get; set; }

    public bool? is_active { get; set; }

    public DateOnly? preferred_date { get; set; }

    public string? full_name { get; set; }

    public string? phone { get; set; }

    public string? email { get; set; }

    public string? address { get; set; }

    public long? status_id { get; set; }

    public long? assign_to { get; set; }

    public bool? unavailable { get; set; }

    public bool? is_regular { get; set; }

    public bool? is_premium { get; set; }

    public bool? is_ultimate { get; set; }

    public virtual user_registration? created_byNavigation { get; set; }

    public virtual master_department dept { get; set; } = null!;

    public virtual user_registration? modified_byNavigation { get; set; }

    public virtual master_service service { get; set; } = null!;

    public virtual master_slot slot { get; set; } = null!;

    public virtual master_status? status { get; set; }
}
