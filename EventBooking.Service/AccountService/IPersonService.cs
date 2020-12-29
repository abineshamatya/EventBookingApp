using EventBooking.Model;
using System.Threading.Tasks;

namespace EventBooking.Service
{
    public interface IPersonService
    {
        /// <summary>
        /// Get a registered person
        /// </summary>
        /// <param name="username">Email or Phone</param>
        /// <param name="password">Password</param>
        /// <param name="provider">Google, Facebook or Roovi</param>
        /// <param name="providerKey">Reference id given by provider</param>
        /// <param name="refreshToken">refresh token is passed if token need to be refreshed</param>
        /// <returns>Person object</returns>
        Task<MvPersonResult> GetPerson(string username, string password);

    }
}