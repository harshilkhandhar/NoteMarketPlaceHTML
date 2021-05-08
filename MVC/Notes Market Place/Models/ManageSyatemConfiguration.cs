using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notes_Market_Place.Models
{
    public class ManageSyatemConfiguration
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Supportemailsaddress { get; set; }

        [Required]
        public string Supportphonenumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        public string FacebookURL { get; set; }

        public string TwitterURL { get; set; }

        public string LinkedinURL { get; set; }

        [Required]
        public HttpPostedFileBase DisplayImage { get; set; }

        public string displayimage { get; set; }

        [Required]
        public HttpPostedFileBase ProfilePicture { get; set; }

        public string profilepicture { get; set; }
    }
}