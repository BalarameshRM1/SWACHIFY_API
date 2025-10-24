namespace Swachify.Application.DTOs
{
    public record BookingDto(
        long Id,
        string? BookingId,
        long DeptId,
        long ServiceId,
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
   bool? is_regular ,

     bool? is_premium ,

     bool? is_ultimate 
    );
}
