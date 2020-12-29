using RestSharp;
using System.Threading.Tasks;

namespace EventBooking.Service
{
    public interface IUtilityService
    {
        Task<T> Post<T>(string url, object obj);

        Task Post(string url, object obj);

        Task<T> Get<T>(string url);

        IRestRequest GetApiRequest(Method method, string resource);

        Task<IRestResponse> GetApiResponseAsync(string uri, IRestRequest rq);

        IRestResponse GetApiResponse(string uri, IRestRequest rq);

        T GetApiResponseObject<T>(IRestResponse rp);

        Task<T> GetApiResponseObject<T>(string uri, IRestRequest rq);

        Task GetApiResponseObject(string uri, IRestRequest rq);

    }
}