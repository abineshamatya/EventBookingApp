using EventBooking.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EventBooking.Api.Controllers
{
    public class BookingController : BaseController
    {
        readonly IBookingService _bookingService;

        public BookingController(
          IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        [HttpGet]
        public async Task<IActionResult> Booking(string json)
        {
            try
            {
                var res = await _bookingService.GetBooking(json);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
