using EventBooking.Model;
using EventBooking.Model.UserAccount;
using EventBooking.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EventBooking.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebService _webService;
        readonly IConfiguration _configuration;
        public AccountController(IWebService webService, IConfiguration configuration)
        {
            _webService = webService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] MvLogin login)
        {
            try
            {
                var res = await _webService.Post<MvPersonResult>($"{ _configuration["ApiUrl"]}account/login", login);
                // success cas
                if (String.IsNullOrEmpty(res.error))
                {
                    HttpContext.Session.SetString("personResult", JsonConvert.SerializeObject(res));                            
                    return Ok(res);
                   
                }
                else
                {
                    return Redirect("~/notfound"); // error 
                }
            }
            catch (Exception ex)
            {
                return Redirect("~/notfound");
            }


        }

    }
}
