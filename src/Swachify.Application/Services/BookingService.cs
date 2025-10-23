using Microsoft.EntityFrameworkCore;
using Swachify.Application.DTOs;
using Swachify.Application.Interfaces;
using Swachify.Infrastructure.Data;
using Swachify.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

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

    public async Task<List<AllBookingsDtos>> GetAllAsync(CancellationToken ct = default)
    {
      var allBookings = await (
          from b in _db.service_bookings.AsNoTracking()
          join d in _db.master_departments.AsNoTracking()
              on b.dept_id equals d.id into deptJoin
          from department in deptJoin.DefaultIfEmpty()

          join s in _db.master_statuses.AsNoTracking()
              on b.status_id equals s.id into statusJoin
          from status in statusJoin.DefaultIfEmpty()

          join u in _db.user_registrations.AsNoTracking()
              on b.created_by equals u.id into userJoin
          from user in userJoin.DefaultIfEmpty()

          join assign in _db.user_registrations.AsNoTracking()
                  on b.assign_to equals assign.id into AssignuserJoin
          from assignUser in AssignuserJoin.DefaultIfEmpty()
          select new AllBookingsDtos
          {
            id = b.id,
            address = b.address,
            assign_to = b.assign_to,
            assign_to_name = assignUser != null ? assignUser.first_name + " " + assignUser.last_name : null,
            created_by = b.created_by,
            created_by_name = user != null ? user.first_name + " " + user.last_name : null,
            created_date = b.created_date,
            dept_id = b.dept_id,
            email = b.email,
            full_name = b.full_name,
            is_active = b.is_active,
            modified_by = b.modified_by,
            phone = b.phone,
            status_id = b.status_id,
            service_id = b.service_id,
            slot_id = b.slot_id,
            modified_date = b.modified_date,
            preferred_date = b.preferred_date,

            department = department == null ? null : new DepartmentDtos
            {
              id = department.id,
              department_name = department.department_name,
              is_active = department.is_active
            },

            Status = status == null ? null : new StatusesDtos
            {
              id = status.id,
              status = status.status,
              is_active = status.is_active
            }
          }
      ).ToListAsync(ct);

      return allBookings;
    }



    public async Task<service_booking?> GetByIdAsync(long id, CancellationToken ct = default)
    {
      return await _db.service_bookings
          .Include(b => b.dept)
          .Include(b => b.service)
          .Include(b => b.slot)
          .FirstOrDefaultAsync(b => b.id == id, ct);
    }

    public async Task<long> CreateAsync(service_booking booking, CancellationToken ct = default)
    {
      booking.created_date = DateTime.Now;
      booking.modified_date = DateTime.Now;
      booking.is_active = true;
      booking.booking_id ??= Guid.NewGuid().ToString();
      booking.full_name = booking.full_name;
      booking.address = booking.address;
      booking.phone = booking.phone;
      booking.email = booking.email;
      booking.service_id=booking.service_id;
      booking.status_id = 1;
      _db.service_bookings.Add(booking);
      await _db.SaveChangesAsync(ct);

      if (!string.IsNullOrEmpty(booking.email))
      {
        var serviceName = await _db.master_departments.FirstOrDefaultAsync(d => d.id == booking.service_id);
        var subject = $"Thank You for Choosing Swachify Cleaning Service!";
        var mailtemplate = await _db.booking_templates.FirstOrDefaultAsync(b => b.title == AppConstants.ServiceBookingMail);
        string emailBody = mailtemplate.description
        .Replace("{0}", booking.full_name)
        .Replace("{1}", serviceName?.department_name+" Service");
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

      existing.dept_id = updatedBooking.dept_id;
      existing.service_id = updatedBooking.service_id;
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

      _db.service_bookings.Remove(booking);
      await _db.SaveChangesAsync(ct);
      return true;
    }

    public async Task<List<AllBookingsDtos>> GetAllBookingByUserIDAsync(long userid)
    {
      var allBookings = await (
 from b in _db.service_bookings.AsNoTracking()
 join d in _db.master_departments.AsNoTracking()
     on b.dept_id equals d.id into deptJoin
 from department in deptJoin.DefaultIfEmpty()

 join s in _db.master_statuses.AsNoTracking()
     on b.status_id equals s.id into statusJoin
 from status in statusJoin.DefaultIfEmpty()

 join u in _db.user_registrations.AsNoTracking()
     on b.created_by equals u.id into userJoin
 from user in userJoin.DefaultIfEmpty()

 join assign in _db.user_registrations.AsNoTracking()
     on b.assign_to equals assign.id into AssignuserJoin
 from assignUser in AssignuserJoin.DefaultIfEmpty()

 select new AllBookingsDtos
 {
   id = b.id,
   address = b.address,
   assign_to = b.assign_to,
   assign_to_name = assignUser != null ? assignUser.first_name + " " + assignUser.last_name : null,
   created_by = b.created_by,
   created_by_name = user != null ? user.first_name + " " + user.last_name : null,
   created_date = b.created_date,
   dept_id = b.dept_id,
   email = b.email,
   full_name = b.full_name,
   is_active = b.is_active,
   modified_by = b.modified_by,
   phone = b.phone,
   status_id = b.status_id,
   service_id = b.service_id,
   slot_id = b.slot_id,
   modified_date = b.modified_date,
   preferred_date = b.preferred_date,
   department = department == null ? null : new DepartmentDtos
   {
     id = department.id,
     department_name = department.department_name,
     is_active = department.is_active
   },

   Status = status == null ? null : new StatusesDtos
   {
     id = status.id,
     status = status.status,
     is_active = status.is_active
   }
 }
).Where(d => d.assign_to == userid).ToListAsync();

      return allBookings;
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
      var customer = await _db.user_registrations.FirstOrDefaultAsync(db => db.id == existing.created_by);

      string emailBody = mailtemplate.description
      .Replace("{0}", customer?.first_name + " " + customer?.last_name);

      if (mailtemplate != null)
      {
        await _emailService.SendEmailAsync(existing.email, subject, emailBody);
      }

      return true;
    }
  }
}
