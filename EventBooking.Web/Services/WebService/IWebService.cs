using RestSharp;
using System.Threading.Tasks;

namespace EventBooking.Web.Services
{
    public interface IWebService
    {
        Task<T> Get<T>(string url);
        IRestRequest GetApiRequest(Method method, string resource);
        Task<IRestResponse> GetApiResponse(string uri, IRestRequest rq);
        Task GetApiResponseObject(string uri, IRestRequest rq);
        Task<T> GetApiResponseObject<T>(string uri, IRestRequest rq);
        Task Post(string url, object obj);
        Task<T> Post<T>(string url, object obj);
    }
}