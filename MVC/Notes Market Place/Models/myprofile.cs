using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notes_Market_Place.Models
{
    public class myprofile
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "This is Required Field")]
        [RegularExpression("[A-za-z]*", ErrorMessage = "Invalid Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This is Required Field")]
        [RegularExpression("[A-za-z]*", ErrorMessage = "Invalid Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This is Required Field")]
        [EmailAddress]
        public string EmailID { get; set; }

        public string SecondaryEmailAddress { get; set; }

        public string Phone_number_Country_Code { get; set; }
        public string phonenumber { get; set; }
        public string ProfilePicture { get; set; }

        public HttpPostedFileBase profilepicutre { get; set; }
    }
}