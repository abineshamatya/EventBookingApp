using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventBooking.Web.Services;
using Microsoft.Extensions.Configuration;
using EventBooking.Model;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using EventBooking.Web.Models;

namespace EventBooking.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly IWebService _webService;
        readonly IConfiguration _configuration;
        MvPersonResult personResult;
        public EventController(IWebService webService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _webService = webService;
            _configuration = configuration;
            personResult = JsonConvert.DeserializeObject<MvPersonResult>(httpContextAccessor.HttpContext.Session.GetString("personResult") ?? string.Empty);

        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            try
            {
                intializeSessionValue();
                var res = await _webService.Get<IEnumerable<MvEvent>>($"{ _configuration["ApiUrl"]}event/event?json={{}}");
                return View(res);
            }
            catch (Exception ex)
            {
                return Redirect("~/notfound");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EventView(MvEvent mvEvent)
        {
            var res = await _webService.Get<IEnumerable<MvEventCategory>>($"{ _configuration["ApiUrl"]}event/eventcategory?json={{}}");
            mvEvent.EventCategoryList = res != null ? res : Enumerable.Empty<MvEventCategory>();
            if (mvEvent.EventId == 0)
            {
                DateTime todayDate = DateTime.Now;
                var model = new MvEvent
                {
                    EventCategoryList = res,
                    StartDate = todayDate.AddDays(-1),
                    EndDate = todayDate,

                };
                mvEvent = model;
            }
            return PartialView("_AddEditEvent", mvEvent);
        }

        // POST: Event/Create     
        [HttpPost]
        public async Task<IActionResult> AddEditEvent([FromBody] MvEvent mvEvent)
        {
            if (ModelState.IsValid)
            {
                var res = await _webService.Post<MvEvent>($"{ _configuration["ApiUrl"]}event/event", mvEvent);
                if (res == null) { return BadRequest(ModelState); }
                return Ok(res);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        public void intializeSessionValue()
        {
            ViewData["navigation"] = personResult.navigation;
            foreach (var person in personResult.person)
            {
                ViewData["role"] = person.Role;
            }
        }



        [HttpGet]
        public IActionResult BookEventView(MvEvent mvEvent)
        {
            return PartialView("_BookEvent", mvEvent);
        }

        [HttpPost]
        public async Task<IActionResult> BookEvent([FromBody]MvEvent mvEvent)
        {
            if (ModelState.IsValid)
            {
                var res = await _webService.Post<MvEvent>($"{ _configuration["ApiUrl"]}Event/BookingTask", mvEvent);
                if (res == null) { return BadRequest(ModelState); }
                return Ok(res);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }
}
