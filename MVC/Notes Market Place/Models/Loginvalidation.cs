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
    public class Loginvalidation
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Required()]
        [StringLength(8,MinimumLength =6, ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}