using Swachify.Infrastructure.Models;

namespace Swachify.Application.DTOs;

public class AllBookingsOutputDtos
{
    public long id { get; set; }

    public string? booking_id { get; set; }
    public long slot_id { get; set; }
    public string? slot_time { get; set; }
    public string? full_name { get; set; }

    public string? phone { get; set; }

    public string? email { get; set; }

    public string? address { get; set; }

    public long? status_id { get; set; }

    public long? assign_to { get; set; }
    public string? employee_name { get; set; }
    public string? status { get; set; }

    public long? dept_id { get; set; }
    public string? department_name { get; set; }

    public long? service_id { get; set; }


    public string? service_name { get; set; }

    public long? service_type_id { get; set; }

    public decimal? total { get; set; }
    public decimal? subtotal { get; set; }
    public decimal? customer_requested_amount { get; set; }
    public decimal? discount_amount { get; set; }
    public decimal? discount_percentage { get; set; }
    public decimal? discount_total { get; set; }

    public long? created_by { get; set; }

    public string? customer_name { get; set; }

    public DateTime? created_date { get; set; }

    public List<BookingServiceDto> services { get; set; } = new();

}

public class BookingServiceDto
{
    public long? dept_id { get; set; }
    public string department_name { get; set; }
    public long? service_id { get; set; }
    public string service_name { get; set; }
    public long? service_type_id { get; set; }
}



