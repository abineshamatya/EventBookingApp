using EventBooking.Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventBooking.Service
{
    public interface IJwtFactory
    {
        ClaimsIdentity GenerateClaimsIdentity(MvPerson personResult);
        Task<object> GenerateJwt(ClaimsIdentity identity, string username, MvPersonResult personResult);
        Task<object> GenerateJwt(MvPersonResult personResult);
    }
}
