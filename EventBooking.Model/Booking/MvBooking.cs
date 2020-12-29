using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EventBooking.Model
{
    public class MvBooking
    {
        [Key]
        public int BookingId { get; set; }
        
        [Required]
        [DisplayName("Client Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Event Id")]
        public int EventId { get; set; }

        [DisplayName("Event Category")]
        public string EventCategory { get; set; }
        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? Price { get; set; }

        [DisplayName("Is Ticket Confirmed")]
        public bool IsTicketConfirmed { get; set; }

        [DisplayName("Event")]
        public string Event { get; set; }

        [DisplayName("Seat Capacity")]
        public int SeatCapacity { get; set; }

        [DisplayName("Reserved Seat")]
        public int ReservedSeat { get; set; }

        [DisplayName("Available Seat")]
        public int AvailableSeat { get; set; }

        [DisplayName("Is Expired")]
        public int IsExpired { get; set; }



    }
}
