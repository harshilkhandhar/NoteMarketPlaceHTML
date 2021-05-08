using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notes_Market_Place.Models
{
    public class AddAdminstrator
    {
        public int RoleID { get; set; }

        [Required]
        
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailID { get; set; }

        public string Phone_number_Country_Code { get; set; }
        public string phonenumber { get; set; }

    }
}