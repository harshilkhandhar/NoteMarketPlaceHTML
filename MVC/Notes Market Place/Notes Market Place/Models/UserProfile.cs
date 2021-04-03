//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Notes_Market_Place.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserProfile
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> Gender { get; set; }
        public string SecondaryEmailAddress { get; set; }
        public string Phone_number_Country_Code { get; set; }
        public string phonenumber { get; set; }
        public string ProfilePicture { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string University { get; set; }
        public string Collage { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> MidifiedBy { get; set; }
    
        public virtual ReferenceData ReferenceData { get; set; }
        public virtual User User { get; set; }
    }
}
