using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Notes_Market_Place.Models
{
    [Table("User")]
    public class Signupvalidation
    {
        [Required(ErrorMessage ="FirstName is Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public  string EmailID { get; set; }

        [Required(ErrorMessage = "This is required field")]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Password must be between 8 to 6 character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="password Mismatch")]
        public string ConformPassword { get; set; }

    }
}