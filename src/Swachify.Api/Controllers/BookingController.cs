using Microsoft.AspNetCore.Mvc;
using Swachify.Application.Interfaces;
using Swachify.Application.DTOs;
using Swachify.Infrastructure.Models;
using Swachify.Application.Models;

namespace Swachify.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        [HttpPost("getall")]
        public async Task<ActionResult> GetAll(GetAllServicesinputDtos input)
        {
            return Ok(await _bookingService.GetAllBookingsAsync(input.status_id,input.limit, input.offset));
        }

        [HttpPost("getallbookingsbyuserID")]
        public async Task<ActionResult> getallbookingsbyuserID(GetAllBookingByUserIDDtos input)
        {
            return Ok(await _bookingService.GetAllBookingByUserIDAsync(input.user_id, input.emp_id, input.limit, input.offset));
        }

        [HttpPost("getallbookingsbyid")]
        public async Task<ActionResult> getallbookingsbyID(GetAllBookingByUserIDDtos input)
        {
            return Ok(await _bookingService.GetAllBookingByBookingIDAsync(input.ticketid, input.offset, input.offset));
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] BookingDto dto, CancellationToken ct)
        {
            List<service_tracking> service_Trackings = new List<service_tracking>();
            dto.Services.ForEach(s =>
            {
                var service = new service_tracking
                {
                    dept_id = s.deptId,
                    service_id = s.serviceId,
                    service_type_id = s.serviceTypeId,
                };

                service_Trackings.Add(service);
            });

            var booking = new service_booking
            {
                booking_id = dto.BookingId,
                slot_id = dto.SlotId,
                created_by = dto.CreatedBy,
                preferred_date = dto.PreferredDate,
                is_active = dto.IsActive ?? true,
                full_name = dto.full_name,
                address = dto.address,
                phone = dto.phone,
                email = dto.email,
                status_id = dto.status_id,
                total = dto?.total,
                subtotal = dto?.subtotal,
                customer_requested_amount = dto?.customer_requested_amount,
                discount_amount = dto?.discount_amount,
                discount_percentage = dto?.discount_percentage,
                discount_total = dto?.discount_total,
                service_trackings = service_Trackings
            };


            var id = await _bookingService.CreateAsync(booking, ct);
            return Ok(id);
        }


        [HttpPut("{id:long}")]
        public async Task<ActionResult> Update(long id, [FromBody] BookingDto dto, CancellationToken ct)
        {
            var booking = new service_booking
            {
                slot_id = dto.SlotId,
                modified_by = dto.ModifiedBy,
                preferred_date = dto.PreferredDate,
                is_active = dto.IsActive,
                full_name = dto.full_name,
                address = dto.address,
                phone = dto.phone,
                email = dto.email,
                status_id = dto.status_id
            };

            var updated = await _bookingService.UpdateAsync(id, booking, ct);
            if (!updated) return NotFound();

            return NoContent();
        }


        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id, CancellationToken ct)
        {
            var deleted = await _bookingService.DeleteAsync(id, ct);
            if (!deleted) return NotFound();

            return NoContent();
        }

        [HttpPut("UpdateTicketByEmployeeCompleted/{id:long}")]
        public async Task<ActionResult> UpdateTicketByEmployeeCompleted(long id)
        {
            var updated = await _bookingService.UpdateTicketByEmployeeCompleted(id);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpPut("UpdateTicketByEmployeeInprogress/{id:long}")]
        public async Task<ActionResult> UpdateTicketByEmployeeInprogress(long id)
        {
            var updated = await _bookingService.UpdateTicketByEmployeeInprogress(id);

            if (!updated) return NotFound();

            return NoContent();
        }

    }
}
