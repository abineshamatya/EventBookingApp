using EventBooking.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventBooking
{
    public interface IBookingService
    {
        Task<IEnumerable<MvBooking>> GetBooking(string json);     
    }
}
