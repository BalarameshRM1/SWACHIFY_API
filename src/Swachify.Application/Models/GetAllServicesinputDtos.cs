namespace Swachify.Application.Models;

public record GetAllServicesinputDtos(int limit,int offset);
public record GetAllBookingByUserIDDtos(long user_id,long emp_id,int limit,int offset);
