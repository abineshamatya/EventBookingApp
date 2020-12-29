using EventBooking.Model.UserAccount;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using EventBooking.Model;

namespace EventBooking.Service
{
    public class AccountService : IAccountService
    {
        readonly IJwtFactory jwtFactory;
        readonly IPersonService personService;
        public AccountService(
              IJwtFactory jwtFactory,
              IPersonService personService
              )
        {
            this.jwtFactory = jwtFactory;
            this.personService = personService;
        }

        public async Task<object> Login(MvLogin login)
        {
            var user = await personService.GetPerson(login.Username, login.Password);
            var jwt = await jwtFactory.GenerateJwt(user);
            return jwt;
        }

    }
}
