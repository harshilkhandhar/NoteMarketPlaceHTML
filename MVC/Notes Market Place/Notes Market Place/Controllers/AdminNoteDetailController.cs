using Notes_Market_Place.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Notes_Market_Place.Controllers
{
    public class AdminNoteDetailController : Controller
    {
        NotesMarketPLaceEntities db = new NotesMarketPLaceEntities();
        // GET: AdminNoteDetail
        public ActionResult NotesDeatil(int id)
        {
            var data = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();
            var review = db.SellerNotesReviews.Where(x => x.ID == id && x.IsActive == true).Select(x => x.Ratings);
            var totalreview = review.Count();
            var avgreview = totalreview > 0 ? Math.Ceiling(review.Average()) : 0;
            var spam = db.SellerNotesReportedIssues.Where(x => x.NoteID == id).Count();
            IEnumerable<Review> notereview1 = from Review in db.SellerNotesReviews
                                              join users in db.Users on Review.ReviewByID equals users.ID
                                              join userprofile in db.UserProfiles on Review.ReviewByID equals userprofile.UserID
                                              where Review.NoteID == id && Review.IsActive == true
                                              orderby Review.Ratings descending
                                              select new Review { review = Review, TblUser = users, TblUserProfile = userprofile };
            var notesDeatil = new NotesDetail();
            notesDeatil.notereview = notereview1;
            if (data != null)
            {
                ViewBag.id = data.ID;
                ViewBag.notetitle = data.Title;
                ViewBag.Category = data.NoteCategory.Name;
                ViewBag.Description = data.Description;
                ViewBag.Institute = data.University;
                ViewBag.Country = data.Country1.Name;
                ViewBag.Course = data.Course;
                ViewBag.CourseCode = data.CourseCode;
                ViewBag.Proffesor = data.Professor;
                ViewBag.NumberofPages = data.NumberofPages;
                ViewBag.ApporvedDate = data.CreatedDate.Value.ToString("MMM dd yyyy");
                ViewBag.SellingPrice = data.SellingPrice;
                ViewBag.NotePreview = data.NotesPreview;
                ViewBag.Display = data.DisplayPicture;
                ViewBag.IsPaid = data.IsPaid;
                ViewBag.Avgreview = avgreview;
                ViewBag.TotalReview = totalreview;
                ViewBag.Spam = spam;
            }
            return View(notesDeatil);
        }
    }
}