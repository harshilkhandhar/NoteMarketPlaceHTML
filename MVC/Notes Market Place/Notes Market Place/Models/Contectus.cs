using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Notes_Market_Place.Models
{
   
    public class Contectus
    {
        [Required]
        [RegularExpression("[A-za-z]*", ErrorMessage = "Invalid Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "This is Required Field")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "This is Required Field")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "This is Required Field")]
        public string Comments { get; set; }
    }
}