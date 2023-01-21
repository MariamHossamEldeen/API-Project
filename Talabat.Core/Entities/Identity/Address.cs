﻿namespace Talabat.Core.Entities.Identity
{
    public class Address : BaseEntity
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string AppUserId { get; set; } // Foreign Key
        public AppUser User { get; set; } // Navigation Property [One]
    }
}