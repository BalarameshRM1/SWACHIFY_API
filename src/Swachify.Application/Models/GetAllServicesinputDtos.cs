namespace Swachify.Application.Models;

public record GetAllServicesinputDtos(long status_id,int limit,int offset);
public record GetAllBookingByUserIDDtos(long ticketid, long user_id,long emp_id,int limit,int offset);
