
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

        public ActionResult Search()
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();
            var category = mylist.NoteCategories.ToList();
            var type = mylist.NoteTypes.ToList();
            var country = mylist.Countries.ToList();
            SelectList list = new SelectList(category, "ID", "Name");
            ViewBag.categoryname = list;
            ViewBag.typename = new SelectList(type, "ID", "Name");
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            var EmailExist = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (EmailExist != null)
            {
                User con = new User
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
            var fromEmailPassword = "*******"; // Replace with actual password
            string subject = subject1;

            string body = "Hello,<br/><br/> " + comments + "<br/><br/>Regards,<br/>"+ fullName;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }


        public ActionResult NotesDeatil(int id)
        {
            var data = db.SellerNotes.Where(x => x.ID == id).SingleOrDefault();
            if(data != null)
            {
                ViewBag.id = data.ID;
                ViewBag.title = data.Title;
                ViewBag.Category = data.NoteCategory.Name;
                ViewBag.Description = data.Description;
                ViewBag.Institute = data.University;
                ViewBag.Country = data.Country1.Name;
                ViewBag.Course = data.Course;
                ViewBag.CourseCode = data.CourseCode;
                ViewBag.Proffesor = data.Professor;
                ViewBag.NumberofPages = data.NumberofPages;
                ViewBag.ApporvedDate = data.CreatedDate;
                ViewBag.SellingPrice = data.SellingPrice;
                ViewBag.NotePreview = data.NotesPreview;
                ViewBag.Display = data.DisplayPicture;
                ViewBag.IsPaid = data.IsPaid;
            }
            return View();
        }


        public ActionResult FAQ()
        {

            return View();
        }


        [HttpGet]
        public ActionResult EmailVerification()
        {
            return View();
        }


        [HttpPost]
        public ActionResult EmailVerification(User user)
        {
            return RedirectToAction("UserProfile","Profile");
        }
        

        [HttpGet]
        public ActionResult BuyerRequest(string Sortorder, string SortBy, int PageNumber = 1)
        {
            var buyer = db.Downloads.ToList();
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
                case "TITLE":
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


        [HttpPost]
        public ActionResult BuyerRequest(string Search)
        {
            var uv = db.Downloads.ToList();
            if (Search != null)
            {
                uv = db.Downloads.Where(x => x.NoteTitle.Contains(Search) || x.NoteCategory.Contains(Search) || x.PurchasedPrice.Equals(Search)).ToList();
                return View(uv);
            }
            return View();
        }

       
        public ActionResult AdminDashBoard()
        {
            return View();
        }

    }
}