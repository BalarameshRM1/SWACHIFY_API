using Swachify.Application.DTOs;
using Swachify.Infrastructure.Models;


namespace Swachify.Application.Interfaces
{
    public interface IBookingService
    {
        Task<List<AllBookingsOutputDtos>> GetAllBookingsAsync(CancellationToken ct = default);
        Task<List<AllBookingsOutputDtos>> GetAllBookingByUserIDAsync(long userid, long empid);    
        Task<long> CreateAsync(service_booking booking, CancellationToken ct = default);
        Task<bool> UpdateAsync(long id, service_booking updatedBooking, CancellationToken ct = default);
        Task<bool> DeleteAsync(long id, CancellationToken ct = default);
        Task<bool> UpdateTicketByEmployeeCompleted(long id);
        Task<bool> UpdateTicketByEmployeeInprogress(long id);

        Task<bool> AssignEmployee(long id, long user_id);
    }
}
