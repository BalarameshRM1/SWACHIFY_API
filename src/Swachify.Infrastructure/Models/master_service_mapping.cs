using System;
using System.Collections.Generic;

namespace Swachify.Infrastructure.Models;

public partial class master_service_mapping
{
    public long id { get; set; }

    public long service_id { get; set; }

    public long service_type_id { get; set; }

    public bool? is_active { get; set; }

    public virtual master_service service { get; set; } = null!;

    public virtual master_service_type service_type { get; set; } = null!;
}
