

using Microsoft.EntityFrameworkCore;
using Swachify.Application.DTOs;
using Swachify.Application.Interfaces;
using Swachify.Infrastructure.Data;
using Swachify.Infrastructure.Models;

namespace Swachify.Application.Services
{
  public class BookingService(MyDbContext _db, IEmailService _emailService) : IBookingService
  {
    public async Task<List<AllBookingsOutputDtos>> GetAllBookingsAsync(int limit = 10, int offset = 0)
    {
      var query = string.Format(DbConstants.fn_service_booking_list, limit, offset);
      var rawData = await _db.Database.SqlQueryRaw<AllBookingsDtos>(query).ToListAsync();
      return await MappingBookingData(rawData);
    }

    public async Task<List<AllBookingsOutputDtos>> GetAllBookingByBookingIDAsync(long bookingId)
    {
      string query = string.Format(DbConstants.fn_service_booking_get_list_by_booking_id, bookingId);
      var rawData = await _db.Database.SqlQueryRaw<AllBookingsDtos>(query).ToListAsync();
      return await MappingBookingData(rawData);
    }

    public async Task<List<AllBookingsOutputDtos>> GetAllBookingByUserIDAsync(long userid, long empid, int limit = 10, int offset = 0)
    {
      string query = string.Empty;
      if (userid > 0)
      {
        query = string.Format(DbConstants.fn_service_booking_list_by_Userid, userid, limit, offset);

      }
      else if (empid > 0)
      {
        query = string.Format(DbConstants.fn_service_booking_list_by_Empid, empid, limit, offset);

      }
      var rawData = await _db.Database.SqlQueryRaw<AllBookingsDtos>(query).ToListAsync();

      return await MappingBookingData(rawData);
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
      booking.hours = booking.hours;
      booking.add_on_hours = booking.add_on_hours;

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
      room_sqfts = item.room_sqfts,
      with_basement = item.with_basement,
    })
    .ToList();
      _db.service_trackings.AddRange(newTrackings);
      await _db.SaveChangesAsync(ct);

      if (!string.IsNullOrEmpty(booking.email))
      {
        var serviceName = await _db.master_departments.FirstOrDefaultAsync(d => d.id == booking.id);
        var subject = $"Thank You for Choosing Swachify Cleaning Service!";
        var mailtemplate = await _db.email_templates.FirstOrDefaultAsync(b => b.title == AppConstants.ServiceBookingMail);
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


    private async Task<List<AllBookingsOutputDtos>> MappingBookingData(List<AllBookingsDtos> rawData)
    {
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
    employee_email = g.First().employee_email,
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
    preferred_date = g.First().preferred_date,
    hours = g.First().hours,
    add_on_hours = g.First().add_on_hours,
    services = g.Select(x => new BookingServiceDto
    {
      dept_id = x.dept_id,
      department_name = x.department_name,
      service_id = x.service_id,
      service_name = x.service_name,
      service_type_id = x.service_type_id,
      service_type = x.service_type
    })
      .DistinctBy(s => new { s.dept_id, s.service_id })
      .ToList()
  })
  .ToList();
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
      var mailtemplate = await _db.email_templates.FirstOrDefaultAsync(b => b.title == AppConstants.CustomerAssignMail);
      string emailBody = mailtemplate.description
      .Replace("{0}", existing?.full_name);

      if (mailtemplate != null)
      {
        await _emailService.SendEmailAsync(existing.email, subject, emailBody);
      }

      return true;
    }
    public async Task<bool> AssignEmployee(long id, long user_id)
    {
      var existing = await _db.service_bookings.FirstOrDefaultAsync(b => b.id == id);
      if (existing == null) return false;
      existing.status_id = 2;
      existing.assign_to = user_id;
      await _db.SaveChangesAsync();

      var resultBookings = await GetAllBookingByBookingIDAsync(id);
      var departnames = string.Join(",", resultBookings
       .Where(b => b?.services != null)
       .SelectMany(b => b.services)
       .Select(s => $"[{s.department_name} - {s.service_name} -{s.service_type}]")
       .Where(name => !string.IsNullOrEmpty(name))
       .ToList());
      var mailtemplate = await _db.email_templates.FirstOrDefaultAsync(b => b.title == AppConstants.CustomerAssignedAgent);
      var agentemail = resultBookings.FirstOrDefault().employee_email;
      var agentname = resultBookings.FirstOrDefault().employee_name;

      string emailBody = mailtemplate.description
      .Replace("{0}", existing?.full_name)
      .Replace("{1}", agentname)
      .Replace("{2}", existing.preferred_date.ToString() ?? DateTime.Now.ToString())
      .Replace("{3}", departnames)
      .Replace("{4}", "India");
      if (mailtemplate != null)
      {
        await _emailService.SendEmailAsync(existing.email, AppConstants.CustomerAssignedAgent, emailBody);
      }
      var agentmailtemplate = await _db.email_templates.FirstOrDefaultAsync(b => b.title == AppConstants.EMPAssignmentMail);
      string agentEmailBody = agentmailtemplate?.description.ToString()
       .Replace("{0}", existing?.id.ToString() + departnames)
       .Replace("{1}", agentname)
       .Replace("{2}", existing?.id.ToString())
       .Replace("{3}", existing?.full_name)
      .Replace("{4}", "India")
      .Replace("{5}", existing.preferred_date.ToString());

      var subject = $"New Service Assigned - {existing?.id}";
      if (mailtemplate != null)
      {
        await _emailService.SendEmailAsync(agentemail, subject, agentEmailBody);
      }
      return true;
    }

  }
}
