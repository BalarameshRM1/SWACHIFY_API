

using Microsoft.EntityFrameworkCore;
using Swachify.Application.DTOs;
using Swachify.Application.Interfaces;
using Swachify.Infrastructure.Data;
using Swachify.Infrastructure.Models;

namespace Swachify.Application.Services
{
  public class BookingService : IBookingService
  {
    private readonly MyDbContext _db;
    private readonly IEmailService _emailService;

    public BookingService(MyDbContext db, IEmailService emailService)
    {
      _db = db;
      _emailService = emailService;
    }

    public async Task<List<AllBookingsOutputDtos>> GetAllBookingsAsync(CancellationToken ct = default)
    {

      var rawData = await _db.Database.SqlQueryRaw<AllBookingsDtos>(DbConstants.fn_service_booking_list).ToListAsync();

      var groupedResult = rawData
        .GroupBy(x => x.id)
        .Select(g => new AllBookingsOutputDtos
        {
          id = g.Key,
          booking_id = g.First().booking_id,
          slot_id = g.First().slot_id,
          slot_time = g.First().slot_time,
          full_name = g.First().full_name,
          phone = g.First().phone,
          email = g.First().email,
          address = g.First().address,
          status_id = g.First().status_id,
          assign_to = g.First().assign_to,
          employee_name = g.First().employee_name,
          status = g.First().status,
          total = g.First().total,
          subtotal = g.First().subtotal,
          customer_requested_amount = g.First().customer_requested_amount,
          discount_amount = g.First().discount_amount,
          discount_percentage = g.First().discount_percentage,
          discount_total = g.First().discount_total,
          created_by = g.First().created_by,
          customer_name = g.First().customer_name,
          created_date = g.First().created_date,

          // Merge distinct department/service details
          services = g.Select(x => new BookingServiceDto
          {
            dept_id = x.dept_id,
            department_name = x.department_name,
            service_id = x.service_id,
            service_name = x.service_name,
            service_type_id = x.service_type_id
          })
            .DistinctBy(s => new { s.dept_id, s.service_id }) // requires .NET 6+
            .ToList()
        })
        .ToList();

      return groupedResult;
    }
    
    public async Task<List<AllBookingsOutputDtos>> GetAllBookingByUserIDAsync(long userid, long empid)
    {
      string query = string.Empty;
      if (userid > 0 && empid > 0)
      {
        query = string.Format(DbConstants.fn_service_booking_list_by_Userid_Empid, userid, empid);
      }
      else if (userid > 0)
      {
        query = string.Format(DbConstants.fn_service_booking_list_by_Userid, userid);

      }
      else if (empid > 0)
      {
        query = string.Format(DbConstants.fn_service_booking_list_by_Empid, empid);

      }

      var rawData =  await _db.Database.SqlQueryRaw<AllBookingsDtos>(query).ToListAsync();

      return rawData
        .GroupBy(x => x.id)
        .Select(g => new AllBookingsOutputDtos
        {
          id = g.Key,
          booking_id = g.First().booking_id,
          slot_id = g.First().slot_id,
          slot_time = g.First().slot_time,
          full_name = g.First().full_name,
          phone = g.First().phone,
          email = g.First().email,
          address = g.First().address,
          status_id = g.First().status_id,
          assign_to = g.First().assign_to,
          employee_name = g.First().employee_name,
          status = g.First().status,
          total = g.First().total,
          subtotal = g.First().subtotal,
          customer_requested_amount = g.First().customer_requested_amount,
          discount_amount = g.First().discount_amount,
          discount_percentage = g.First().discount_percentage,
          discount_total = g.First().discount_total,
          created_by = g.First().created_by,
          customer_name = g.First().customer_name,
          created_date = g.First().created_date,

          // Merge distinct department/service details
          services = g.Select(x => new BookingServiceDto
          {
            dept_id = x.dept_id,
            department_name = x.department_name,
            service_id = x.service_id,
            service_name = x.service_name,
            service_type_id = x.service_type_id
          })
            .DistinctBy(s => new { s.dept_id, s.service_id }) // requires .NET 6+
            .ToList()
        })
        .ToList();
    }



    public async Task<long> CreateAsync(service_booking booking, CancellationToken ct = default)
    {
      var bookingID = Guid.NewGuid().ToString();
      booking.created_date = DateTime.Now;
      booking.is_active = true;
      booking.booking_id ??= bookingID;
      booking.full_name = booking.full_name;
      booking.address = booking.address;
      booking.phone = booking.phone;
      booking.email = booking.email;
      booking.status_id = 1;
      booking.total = booking.total;
      booking.subtotal = booking.subtotal;
      booking.customer_requested_amount = booking.customer_requested_amount;
      booking.discount_amount = booking.discount_amount;
      booking.discount_percentage = booking.discount_percentage;
      booking.discount_total = booking.discount_total;

      _db.service_bookings.Add(booking);

      await _db.SaveChangesAsync(ct);

      var newTrackings = booking.service_trackings
    .Select(item => new service_tracking
    {
      service_booking_id = booking.id,
      booking_id = bookingID,
      dept_id = item.dept_id,
      service_id = item.service_id,
      service_type_id = item.service_type_id,
    })
    .ToList();
      _db.service_trackings.AddRange(newTrackings);
      await _db.SaveChangesAsync(ct);

      if (!string.IsNullOrEmpty(booking.email))
      {
        var serviceName = await _db.master_departments.FirstOrDefaultAsync(d => d.id == booking.id);
        var subject = $"Thank You for Choosing Swachify Cleaning Service!";
        var mailtemplate = await _db.booking_templates.FirstOrDefaultAsync(b => b.title == AppConstants.ServiceBookingMail);
        string emailBody = mailtemplate.description
        .Replace("{0}", booking.full_name)
        .Replace("{1}", serviceName?.department_name + " Service");
        if (mailtemplate != null)
        {
          await _emailService.SendEmailAsync(booking.email, subject, emailBody);
        }
      }
      return booking.id;
    }

    public async Task<bool> UpdateAsync(long id, service_booking updatedBooking, CancellationToken ct = default)
    {
      var existing = await _db.service_bookings.FirstOrDefaultAsync(b => b.id == id, ct);
      if (existing == null) return false;

      existing.slot_id = updatedBooking.slot_id;
      existing.modified_by = updatedBooking.modified_by;
      existing.modified_date = DateTime.UtcNow;
      existing.preferred_date = updatedBooking.preferred_date;
      existing.is_active = updatedBooking.is_active;
      existing.full_name = updatedBooking.full_name;
      existing.address = updatedBooking.address;
      existing.phone = updatedBooking.phone;
      existing.email = updatedBooking.email;
      existing.status_id = updatedBooking.status_id;
      await _db.SaveChangesAsync(ct);
      return true;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
      var booking = await _db.service_bookings.FirstOrDefaultAsync(b => b.id == id, ct);
      if (booking == null) return false;

      // _db.service_bookings.Remove(booking);
      booking.is_active = false;
      await _db.SaveChangesAsync(ct);
      return true;
    }



    public async Task<bool> UpdateTicketByEmployeeInprogress(long id)
    {
      var existing = await _db.service_bookings.FirstOrDefaultAsync(b => b.id == id);
      if (existing == null) return false;
      existing.status_id = 3;
      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> UpdateTicketByEmployeeCompleted(long id)
    {
      var existing = await _db.service_bookings.FirstOrDefaultAsync(b => b.id == id);
      if (existing == null) return false;
      existing.status_id = 4;
      await _db.SaveChangesAsync();
      var subject = $"Your Cleaning Service Is Completed!";
      var mailtemplate = await _db.booking_templates.FirstOrDefaultAsync(b => b.title == AppConstants.CustomerAssignMail);
      string emailBody = mailtemplate.description
      .Replace("{0}", existing?.full_name);

      if (mailtemplate != null)
      {
        await _emailService.SendEmailAsync(existing.email, subject, emailBody);
      }

      return true;
    }
  }
}
