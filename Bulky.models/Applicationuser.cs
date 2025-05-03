using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.models
{
    public class Applicationuser: IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        //public string PhoneNumber { get; set; }
        //public string? ImageUrl { get; set; }
        //public DateTime? DateOfBirth { get; set; }
        // 1 to many relationship
        

    }
}
