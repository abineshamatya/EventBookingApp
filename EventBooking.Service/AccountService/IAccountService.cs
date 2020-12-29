using EventBooking.Model.UserAccount;
using System.Threading.Tasks;

namespace EventBooking.Service
{
    public interface IAccountService
    {
        Task<object> Login(MvLogin login);
    }
}