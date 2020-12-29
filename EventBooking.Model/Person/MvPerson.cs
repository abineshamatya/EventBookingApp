using System;
using System.Collections.Generic;

namespace EventBooking.Model
{
    public class MvPerson
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string UsernameType { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }


    public class MvPersonResult
    {
        public string accessToken { get; set; }
        public List<MvPerson> person { get; set; }
        public List<MvNavigation> navigation { get; set; }
        public string? error { get; set; }

    }
}
