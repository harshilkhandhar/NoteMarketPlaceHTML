using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notes_Market_Place.Models
{
    public class Changepassword
    {
        [Required]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Password must be between 8 to 6 character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Password must be between 8 to 6 character")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "password Mismatch")]
        public string ConformPassword { get; set; }
    }
}