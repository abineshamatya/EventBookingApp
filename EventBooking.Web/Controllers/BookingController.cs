using EventBooking.Model;
using EventBooking.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventBooking.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IWebService _webService;
        readonly IConfiguration _configuration;
        MvPersonResult personResult;
        public BookingController(IWebService webService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _webService = webService;
            _configuration = configuration;
            personResult = JsonConvert.DeserializeObject<MvPersonResult>(httpContextAccessor.HttpContext.Session.GetString("personResult") ?? string.Empty);

        }

        public async Task<IActionResult> Index()
        {
            try
            {
                intializeSessionValue();
                var res = await _webService.Get<IEnumerable<MvBooking>>($"{ _configuration["ApiUrl"]}booking/booking?json={{}}");
                // just show the valid events to the client              
                return View(res);
            }
            catch (Exception ex)
            {
                return Redirect("~/notfound");
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
    }
}
