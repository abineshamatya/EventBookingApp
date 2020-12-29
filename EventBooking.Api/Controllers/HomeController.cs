using Microsoft.AspNetCore.Mvc;




namespace EventBooking.Api.Controllers
{
    //[Route("[controller])]
    //[ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public int Get() {
            return 1;
        }
    }
}
