using EventBooking.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventBooking.Service
{
    public interface IEventService
    {
        Task<IEnumerable<MvEvent>> GetEvent(string json);
        Task<MvEvent> InsertUpdateEvent(MvEvent eventJs);
        Task<IEnumerable<MvEventCategory>> GetEventCategory(string json);
        Task<MvEvent> InsertUpdateBooking(MvEvent booking);
    }
}
