using Dapper;
using EventBooking.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EventBooking.Service
{
    public class EventService : IEventService
    {
        readonly IBaseService bs;
        IDataAccessService das;

        public EventService(IDataAccessService dataAccessService, IBaseService baseService)
        {
            das = dataAccessService;
            bs = baseService;
        }

        public async Task<IEnumerable<MvEvent>> GetEvent(string json)
        {
            var res = await das.RetrievalProcedureSingle<string>("SpEventSel", new { json });
            return JsonConvert.DeserializeObject<List<MvEvent>>(res);
        }

        public async Task<MvEvent> InsertUpdateEvent(MvEvent mvEvent)
        {
            if (mvEvent.EventId == 0)
            {
                var dp = new DynamicParameters();
                dp.Add("@Json", JsonConvert.SerializeObject(mvEvent), direction: ParameterDirection.InputOutput);
                await das.ActionProcedure("SpEventIns", dp);
                string newEvent = dp.Get<string>("@Json");
                return JsonConvert.DeserializeObject<MvEvent>(newEvent);
            }
            else {
                var dp = new DynamicParameters();
                dp.Add("@Json", JsonConvert.SerializeObject(mvEvent), direction: ParameterDirection.Input);
                await das.ActionProcedure("SpEventUpd", dp);
                string newEvent = dp.Get<string>("@Json");
                return JsonConvert.DeserializeObject<MvEvent>(newEvent);
            }
        }

        public async Task<IEnumerable<MvEventCategory>> GetEventCategory(string json)
        {
            var res = await das.RetrievalProcedureSingle<string>("SpEventCategorySel", new { json });
            return JsonConvert.DeserializeObject<List<MvEventCategory>>(res);
        }


        public async Task<MvEvent> InsertUpdateBooking(MvEvent booking)
        {
            
                var dp = new DynamicParameters();
                dp.Add("@Json", JsonConvert.SerializeObject(booking), direction: ParameterDirection.InputOutput);
                await das.ActionProcedure("SpBookingIns", dp);
                string newBooking = dp.Get<string>("@Json");
                return JsonConvert.DeserializeObject<MvEvent>(newBooking);
        }
    }
}
