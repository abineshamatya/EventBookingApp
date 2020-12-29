using RestSharp;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using EventBooking.Model;
using Microsoft.AspNetCore.Http;

namespace EventBooking.Web.Services
{
    public class WebService : IWebService
    {
        private MvPersonResult personResult;
        readonly IRestClient restClient;
        readonly IConfiguration config;
        readonly IHttpContextAccessor _httpContextAccessor;
        public WebService(IRestClient restClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.restClient = restClient;
            config = configuration;
            this.restClient.BaseUrl = new Uri(config["ApiUrl"]);
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<T> Post<T>(string url, object obj)
        {
            var rq = GetApiRequest(Method.POST, url); //get token from session
            personResult = JsonConvert.DeserializeObject<MvPersonResult>(_httpContextAccessor.HttpContext.Session.GetString("personResult") ?? string.Empty);
            if (personResult != null && !String.IsNullOrEmpty(personResult.accessToken))
            {
                rq.AddHeader("Authorization", "Bearer " + personResult.accessToken);
                //rq.AddParameter("Authorization", string.Format("Bearer {0}", accessToken), ParameterType.HttpHeader);
            }
            rq.AddJsonBody(obj);
            var res = await GetApiResponseObject<T>(url, rq);
            return res;
        }

        public async Task Post(string url, object obj)
        {
            var rq = GetApiRequest(Method.POST, url); //get token from session
            personResult = JsonConvert.DeserializeObject<MvPersonResult>(_httpContextAccessor.HttpContext.Session.GetString("personResult") ?? string.Empty);
            if (personResult != null && !String.IsNullOrEmpty(personResult.accessToken))
            {
                rq.AddHeader("Authorization", "Bearer " + personResult.accessToken);
                //rq.AddParameter("Authorization", string.Format("Bearer {0}", accessToken), ParameterType.HttpHeader);
            }
            rq.AddJsonBody(obj);
            await GetApiResponseObject(url, rq);
        }

        public async Task<T> Get<T>(string url)
        {
            var rq = GetApiRequest(Method.GET, url);
            personResult = JsonConvert.DeserializeObject<MvPersonResult>(_httpContextAccessor.HttpContext.Session.GetString("personResult") ?? string.Empty);
            if (personResult != null && !String.IsNullOrEmpty(personResult.accessToken))
            {
                rq.AddHeader("Authorization", "Bearer " + personResult.accessToken);
                //rq.AddParameter("Authorization", string.Format("Bearer {0}", accessToken), ParameterType.HttpHeader);
            }
            var res = await GetApiResponseObject<T>(url, rq);
            return res;
        }

        public IRestRequest GetApiRequest(Method method, string resource/*, string token*/)
        {
            var rq = new RestRequest();
            rq.Method = method;
            rq.Resource = resource;
            rq.RequestFormat = DataFormat.Json;
            // // rq.AddHeader("Authorization", "Bearer " + token);
            //if (token != null && token != "")
            //    rq.AddParameter("Authorization", string.Format("Bearer {0}", token), ParameterType.HttpHeader);
            rq.AddParameter("Access-Control-Allow-Origin", "*", ParameterType.HttpHeader);
            return rq;
        }

        public async Task<T> GetApiResponseObject<T>(string uri, IRestRequest rq)
        {
            IRestResponse rp;
            try
            {
                rp = await GetApiResponse(uri, rq);
            }
            catch (Exception ex)
            {
                //Log.Error(ex, ex.Message);
                rp = new RestResponse();
            }
            if (rp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(rp.Content);
            }
            return default(T);
        }

        public async Task GetApiResponseObject(string uri, IRestRequest rq)
        {
            IRestResponse rp;
            try
            {
                rp = await GetApiResponse(uri, rq);
            }
            catch (Exception ex)
            {
                //Log.Error(ex, ex.Message);
                rp = new RestResponse();
            }
        }

        public async Task<IRestResponse> GetApiResponse(string uri, IRestRequest rq)
        {
            IRestResponse rp = new RestResponse();
            rp.ContentType = "application/json";
            rp = await restClient.ExecuteAsync(rq);
            return rp;
        }

    }
}
