
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Notes_Market_Place.Models;


namespace Notes_Market_Place.Controllers
{
    public class HomeController : Controller
    {
        NotesMarketPLaceEntities db = new NotesMarketPLaceEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string Sortorder, string SortBy, int PageNumber = 1)
        {
            using(var mylist = new NotesMarketPLaceEntities())
            {
                var category = mylist.NoteCategories.ToList();
                var type = mylist.NoteTypes.ToList();
                var country = mylist.Countries.ToList();
                var course = mylist.SellerNotes.Where(x => /*x.IsActive == true && */x.Course != null).Select(x => x.Course).Distinct().ToList() ;
                var university = mylist.SellerNotes.Where(x => /*x.IsActive ==true &&*/ x.University != null ).Select(x => x.University).Distinct().ToList();
                SelectList list = new SelectList(category, "Name", "Name");
                ViewBag.categoryname = list;
                ViewBag.typename = new SelectList(type, "Name", "Name");
                ViewBag.countryname = new SelectList(country, "Name", "Name");
                ViewBag.course = new SelectList(course, "Course");
                ViewBag.university = new SelectList(university, "University");
                ViewBag.rating = new List<SelectListItem> { new SelectListItem { Text = "1+", Value = "1" }, new SelectListItem { Text = "2+", Value = "2" }, new SelectListItem { Text = "3+", Value = "3" }, new SelectListItem { Text = "4+", Value = "4" }, new SelectListItem { Text = "5+", Value = "5" } };
            }
            ViewBag.SEARCHNOTE = "active";
            var seller = db.SellerNotes.Where(x => x.Status == 9).ToList();
            ViewBag.Sortorder = Sortorder;
            ViewBag.SortBy = SortBy;
            ViewBag.totalpages = Math.Ceiling(seller.Count() / 9.0);
            ViewBag.PageNumber = PageNumber;
            seller = seller.Skip((PageNumber - 1) * 9).Take(9).ToList();
            return View(seller);
        }


        [HttpPost]
        public ActionResult Search(string Rating, string University,string Category, string Course, string Country,string Search,string Type)
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();
            var category = mylist.NoteCategories.ToList();
            var type = mylist.NoteTypes.ToList();
            var country = mylist.Countries.ToList();
            var course = mylist.SellerNotes.Where(x => /*x.IsActive == true && */x.Course != null).Select(x => x.Course).Distinct().ToList();
            var university = mylist.SellerNotes.Where(x => /*x.IsActive ==true &&*/ x.University != null).Select(x => x.University).Distinct().ToList();
            SelectList list = new SelectList(category, "Name", "Name");
            ViewBag.categoryname = list;
            ViewBag.typename = new SelectList(type, "Name", "Name");
            ViewBag.countryname = new SelectList(country, "Name", "Name");
            ViewBag.course = new SelectList(course,"Course");
            ViewBag.university = new SelectList(university,"University");
            ViewBag.rating = new List<SelectListItem> { new SelectListItem { Text = "1+", Value = "1" },  new SelectListItem { Text = "2+", Value = "2" }, new SelectListItem { Text = "3+", Value = "3" }, new SelectListItem { Text = "4+", Value = "4" }, new SelectListItem { Text = "5+", Value = "5" } };
            //var uv = db.SellerNotes.ToList(); 
            if (Search != "")
            {
                var uv1 = db.SellerNotes.Where(x => x.NoteCategory.Name.Contains(Search) || x.NoteType1.Name.Contains(Search) || x.University.Contains(Search)).ToList();
                ModelState.Clear();
                return View(uv1);
            }
            if (Category != "")
            {
                var uv1 = db.SellerNotes.Where(x => x.NoteCategory.Name.Contains(Category)).ToList();
                ModelState.Clear();
                return View(uv1);
            }
            if (Type != "")
            {
                var uv1 = db.SellerNotes.Where(x => x.NoteType1.Name.Contains(Type)).ToList();
                ModelState.Clear();
                return View(uv1);
            }
            if (University != "")
            {
                var uv1 = db.SellerNotes.Where(x => x.University.Contains(University)).ToList();
                ModelState.Clear();
                return View(uv1);
            }
            if (Course != "")
            {
                var uv1 = db.SellerNotes.Where(x => x.Course.Contains(Course)).ToList();
                ModelState.Clear();
                return View(uv1);
            }
            if (Country != "")
            {
                var uv1 = db.SellerNotes.Where(x => x.Country1.Name.Contains(Country)).ToList();
                ModelState.Clear();
                return View(uv1);
            }
            if (Rating != "")
            {
                List<SellerNote> sellerNotesReviews = new List<SellerNote>();
                foreach (var item in sellerNotesReviews)
                {
                    var review = db.SellerNotesReviews.Where(x => x.ID == item.ID && x.IsActive == true).Select(x => x.Ratings);
                    var totalreview = review.Count();
                    var avgreview = totalreview > 0 ? Math.Ceiling(review.Average()) : 0;
                    var spam = db.SellerNotesReportedIssues.Where(x => x.NoteID == item.ID).Count();
                }
                //var uv1 = db.SellerNotes.Where(x => x.SellerNotesReviews.Contains(Rating)).ToList();
                ModelState.Clear();
                //return View(uv1);
            }

            ModelState.Clear();
            return View();
           
        }



        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Contect = "active";
            var EmailExist = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (EmailExist != null)
            {
                Contectus con = new Contectus
                {
                    FullName = EmailExist.FirstName +" "+ EmailExist.LastName,
                    EmailID = EmailExist.EmailID,
                };
                return View(con);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contectus contectus)
        {
           if (ModelState.IsValid)
           {
                db.Configuration.ValidateOnSaveEnabled = false;
                SendqueryEmail(contectus.FullName,contectus.EmailID,contectus.Subject,contectus.Comments);
                return View();
            }
            else
            {
                return View("Index");
            }
           
        }

        private void SendqueryEmail(string fullName, string emailID, string subject1, string comments)
        {
            //var verifyUrl = "/Login/VerifyAccount/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
           
            var fromEmail = new MailAddress("k.harshil2000@gmail.com", "Notes MarketPlace");
            var toEmail = new MailAddress(emailID);
            string subject = subject1;

            string body = "Hello,<br/><br/> " + comments + "<br/><br/>Regards,<br/>"+ fullName;

            //var smtp = new SmtpClient
            //{
            //    Host = "smtp.gmail.com",
            //    Port = 587,
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            //};
            using (var mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                SendMail.SendEmail(mail);
        }


        public ActionResult NotesDeatil(int id)
        {
            var data = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();
            var review = db.SellerNotesReviews.Where(x => x.ID == id && x.IsActive ==true).Select(x => x.Ratings);
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


        public ActionResult FAQ()
        {
            ViewBag.Active = "active";
            return View();
        }


        [HttpGet]
        public ActionResult EmailVerification(string email)
        {
            var user = db.Users.Where(x => x.EmailID == email).FirstOrDefault();
            ViewBag.Name = user.FirstName;
            return View();
        }


        [HttpPost]
        public ActionResult EmailVerification(User user)
        {
            return RedirectToAction("Index","Login");
        }
        

        [HttpGet]
        public ActionResult BuyerRequest(string Sortorder, string SortBy, int PageNumber = 1)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.Downloads.Where(x => x.IsPaid == true && x.IsSellerHasAllowedDownloads == false && x.Seller == user.ID).FirstOrDefault();
            if (progressnote != null)
            {
                //var buyer = db.Downloads.ToList();
                var buyer = db.Downloads.Where(x => x.IsPaid == true && x.IsSellerHasAllowedDownloads == false && x.Seller == user.ID).ToList();
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;
                ViewBag.totalpages = Math.Ceiling(buyer.Count() / 10.0);
                ViewBag.PageNumber = PageNumber;
                buyer = buyer.Skip((PageNumber - 1) * 10).Take(10).ToList();

                switch (SortBy)
                {
                    case "CATEGORY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        buyer = buyer.OrderBy(x => x.NoteCategory).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        buyer = buyer.OrderByDescending(x => x.NoteCategory).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        buyer = buyer.OrderByDescending(x => x.NoteCategory).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "NOTE TITLE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        buyer = buyer.OrderBy(x => x.NoteTitle).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        buyer = buyer.OrderByDescending(x => x.NoteTitle).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        buyer = buyer.OrderByDescending(x => x.NoteTitle).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "SELL TYPE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        buyer = buyer.OrderBy(x => x.IsPaid).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        buyer = buyer.OrderByDescending(x => x.IsPaid).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        buyer = buyer.OrderByDescending(x => x.IsPaid).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "PRICE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        buyer = buyer.OrderBy(x => x.PurchasedPrice).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        buyer = buyer.OrderByDescending(x => x.PurchasedPrice).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        buyer = buyer.OrderByDescending(x => x.PurchasedPrice).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "DOWNLOAD DATE/TIME":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        buyer = buyer.OrderBy(x => x.CreatedDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        buyer = buyer.OrderByDescending(x => x.CreatedDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        buyer = buyer.OrderByDescending(x => x.CreatedDate).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            buyer = buyer.OrderByDescending(x => x.CreatedDate).ToList();
                            break;
                        }
                }
                return View(buyer);
            }
            else
            {
                var buyer = db.Downloads.Where(x => x.IsPaid==true && x.IsSellerHasAllowedDownloads==false).ToList();
                ViewBag.Message = "No Record Found";
                return View(buyer);
            }
        }


        [HttpPost]
        public ActionResult BuyerRequest(string Search)
        {
            var uv = db.Downloads.ToList();
            if (Search != null)
            {
                uv = db.Downloads.Where(x => x.NoteTitle.Contains(Search) || x.NoteCategory.Contains(Search) || x.PurchasedPrice.ToString().Contains(Search)).ToList();
                return View(uv);
            }
            return View();
        }
      

    }
}