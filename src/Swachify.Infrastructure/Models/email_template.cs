using System;
using System.Collections.Generic;

namespace Swachify.Infrastructure.Models;

public partial class email_template
{
    public long id { get; set; }

    public string title { get; set; } = null!;

    public string description { get; set; } = null!;

    public long? created_by { get; set; }

    public DateTime? created_date { get; set; }

    public long? modified_by { get; set; }

    public DateTime? modified_date { get; set; }

    public bool? is_active { get; set; }

    public virtual user_registration? created_byNavigation { get; set; }

    public virtual user_registration? modified_byNavigation { get; set; }
}
