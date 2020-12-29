using Dapper;
using EventBooking.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EventBooking.Service
{
    public class BookingService : IBookingService
    {
        readonly IBaseService bs;
        IDataAccessService das;

        public BookingService(IDataAccessService dataAccessService, IBaseService baseService)
        {
            das = dataAccessService;
            bs = baseService;
        }

        public async Task<IEnumerable<MvBooking>> GetBooking(string json)
        {
            var res = await das.RetrievalProcedureSingle<string>("SpEventBookingSel", new { json });
            return JsonConvert.DeserializeObject<List<MvBooking>>(res);
        }       

      
    }

}
