using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace EventBooking.Service
{
    public class BaseService : IBaseService
    {

        public BaseService(IHttpContextAccessor httpContextAccessor, IDataAccessService das)
        {
            var accessor = httpContextAccessor;
            int personId = 0;
            string role = string.Empty;
            string token = "";
            if (accessor.HttpContext != null)
                token = accessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (token != null && token != "")
            {
                var jwt = new JwtSecurityToken(token.Substring(7));
                int.TryParse(jwt.Claims.First(claim => claim.Type == "personId").Value, out personId);
                role = jwt.Claims.First(claim => claim.Type == "role").Value;
            }

            das.CurrentPersonId = personId;
            das.Role = role;
            das.UserContext = JsonConvert.SerializeObject(new { personId });

        }
    }
}
