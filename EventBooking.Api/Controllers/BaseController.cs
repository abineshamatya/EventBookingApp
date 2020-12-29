using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EventBooking.Api.Controllers
{
    [Produces("application/json")]
    [EnableCors("EventBookingCorsPolicy"), Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    [Authorize]

    public class BaseController : Controller
    { }
}
