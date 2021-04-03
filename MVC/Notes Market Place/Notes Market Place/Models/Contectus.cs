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
        
        [Required(ErrorMessage ="This is the required field")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "This is the required field")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "This is the required field")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "This is the required field")]
        public string Comments { get; set; }
    }
}