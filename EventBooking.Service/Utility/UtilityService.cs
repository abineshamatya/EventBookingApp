using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using Serilog;
using System;
using System.Threading.Tasks;

namespace EventBooking.Service
{
    public class UtilityService : IUtilityService
    {
        private readonly IHostingEnvironment env;

        //UtilityService doesnot call other userdefined service. Not even base. Other service call Utility service. Never call utility from Controller.
        public UtilityService(IHostingEnvironment env)
        {
            this.env = env;
        }


        public async Task<T> Post<T>(string url, object obj)
        {
            var rq = GetApiRequest(Method.POST, url); //get token from session
            rq.AddJsonBody(obj);
            var res = await GetApiResponseObject<T>(url, rq);
            return res;
        }

        public async Task Post(string url, object obj)
        {
            var rq = GetApiRequest(Method.POST, url); //get token from session
            rq.AddJsonBody(obj);
            //Debug.WriteLine("post method url: " + url);
            //Debug.WriteLine("post method obj: " + JsonConvert.SerializeObject(obj));
            await GetApiResponseObject(url, rq);
        }

        public async Task<T> Get<T>(string url)
        {
            var rq = GetApiRequest(Method.GET, url);
            var res = await GetApiResponseObject<T>(url, rq);
            //Debug.WriteLine("get method url: " + url);
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

        public async Task<IRestResponse> GetApiResponseAsync(string uri, IRestRequest rq)
        {
            var rc = new RestClient(uri);
            IRestResponse rp = new RestResponse();
            rp.ContentType = "application/json";
            rp = await rc.ExecuteAsync(rq);
            return rp;
        }

        public IRestResponse GetApiResponse(string uri, IRestRequest rq)
        {
            var rc = new RestClient(uri);
            IRestResponse rp = new RestResponse();
            rp.ContentType = "application/json";
            rp = rc.Execute(rq);
            return rp;
        }

        public T GetApiResponseObject<T>(IRestResponse rp)
        {
            return JsonConvert.DeserializeObject<T>(rp.Content);
        }

        public async Task<T> GetApiResponseObject<T>(string uri, IRestRequest rq)
        {
            IRestResponse rp;
            try
            {
                rp = await GetApiResponseAsync(uri, rq);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
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
                rp = await GetApiResponseAsync(uri, rq);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                rp = new RestResponse();
            }
        }

        public static string SerializeObject<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, CustomSerializerSettings());
        }

        public static JsonSerializerSettings CustomSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy/MM/dd hh:mm:ss"
            };
        }

     
    }
}
