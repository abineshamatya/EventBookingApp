using EventBooking.Model;
using EventBooking.Model.UserAccount;
using EventBooking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EventBooking.Api.Controllers
{
    public class AccountController : BaseController
    {
        readonly IAccountService accountService;

        public AccountController(
           IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] MvLogin login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new MvError("Login Error", "Invalid username or password"));
                }
                var jwt = await accountService.Login(login);

                if (jwt == null)
                {
                    return BadRequest(new MvError("Login error", "Invalid username or password"));
                    //return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
                }
                return new OkObjectResult(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest(new MvError("Login Error", ex.Message));
            }
        }        
    }
}
