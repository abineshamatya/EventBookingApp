using EventBooking.Model;
using EventBooking.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EventBooking.Api.Controllers
{
    public class EventController : BaseController
    {
        readonly IEventService _eventService;

        public EventController(
          IEventService eventService)
        {
            _eventService = eventService;
        }


        [HttpGet]
        public async Task<IActionResult> Event(string json)
        {
            try
            {
                var res = await _eventService.GetEvent(json);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Event([FromBody] MvEvent mvEvent)
        {
            try
            {
                return Ok(await _eventService.InsertUpdateEvent(mvEvent));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> EventCategory(string json)
        {
            try
            {
                var res = await _eventService.GetEventCategory(json);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> BookingTask([FromBody] MvEvent json)
        {
            try
            {
                var res = await _eventService.InsertUpdateBooking(json);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
