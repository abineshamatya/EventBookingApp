using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Model
{
    public class MvError
    {
        public MvError(string title, string message)
        {
            Title = title;
            Message = message;
        }
        public string Title { get; set; }
        public string Message { get; set; }

    }    
}
