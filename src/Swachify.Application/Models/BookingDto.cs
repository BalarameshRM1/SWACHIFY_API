namespace Swachify.Application.DTOs
{
    public record BookingDto(
        long Id,
        string? BookingId,
        long SlotId,
        long? CreatedBy,
        DateTime? CreatedDate,
        long? ModifiedBy,
        DateTime? ModifiedDate,
        bool? IsActive,
        DateOnly? PreferredDate,
        string? full_name,
        string? phone,
        string? email,
        string? address,
        long? status_id,
decimal? total,
decimal? subtotal,
decimal? customer_requested_amount,
decimal? discount_amount,
decimal? discount_percentage,
decimal? discount_total,
    int? hours,
     int? add_on_hours,
List<Service> Services
    );

    public record Service
    (
        long deptId,
        long serviceId,
        long serviceTypeId
    );
}
