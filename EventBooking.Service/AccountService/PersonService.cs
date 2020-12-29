using EventBooking.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Service
{
    public class PersonService : IPersonService
    {
        IDataAccessService das;
        readonly IBaseService bs;

        public PersonService(IDataAccessService dataAccessService, IBaseService baseService)
        {
            bs = baseService;
            das = dataAccessService;
        }


        public async Task<MvPersonResult> GetPerson(string username, string password)
        {
            try
            {
                var res = await das.RetrievalProcedureSingle<string>("SpPersonSel", new { username, password });
                return JsonConvert.DeserializeObject<MvPersonResult>(res);
            }
            catch (Exception)
            {
                //Log.Error(ex, ex.Message);
                return null;
            }
        }
    }
}
