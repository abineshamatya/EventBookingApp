using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EventBooking.Model
{
    public class MvEvent
    {
        [Key]
        public int EventId { get; set; }        

        [Required]
        [DisplayName("Event")]
        public string Event { get; set; }

        [DisplayName("Is Ticket Confirmed")]
        public bool IsTicketConfirmed { get; set; }

        [Required]
        [DisplayName("Event Category")]
        public int EventCategoryListItemId { get; set; }

        [DisplayName("Event Category")]
        public string EventCategory { get; set; }

        [Required]
        [DisplayName("Location")]
        public string Location { get; set; }

        [Required]
        [DisplayName("Start Date Time")]      
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayName("End Date Time")]       
        public DateTime EndDate { get; set; }

        [Required]
        [DisplayName("Seat Capacity")]
        public int SeatCapacity { get; set; }

        [DisplayName("Reserved Seat")]
        public int ReservedSeat { get; set; }

        [DisplayName("Available Seat")]
        public int AvailableSeat { get; set; }

        [DisplayName("Is Expired")]
        public int IsExpired { get; set; }

        [DisplayName("Price")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal? Price { get; set; }
        public int BookingId { get; set; }

        public IEnumerable<MvEventCategory> EventCategoryList { get; set; }
    }

    public class MvEventResult
    {
        public List<MvEvent> eventResult { get; set; }
    }


    public class MvEventCategory
    {
        public int EventCategoryListItemId { get; set; }
        public string Category { get; set; }
    }
}
