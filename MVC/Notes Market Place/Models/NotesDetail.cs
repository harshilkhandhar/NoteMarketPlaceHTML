using System;
using Notes_Market_Place.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notes_Market_Place.Models
{
    public class NotesDetail
    {

        public IEnumerable<Review> notereview { get; set; }
    }


    public class Review
    {
        public User TblUser { get; set; }
        public UserProfile TblUserProfile { get; set; }
        public SellerNotesReview review { get; set; }
    }
}
