using Notes_Market_Place.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Notes_Market_Place.Controllers
{
    public class AdminDashBoardController : Controller
    {
        NotesMarketPLaceEntities db = new NotesMarketPLaceEntities();
        // GET: AdminDashBoard
        [HttpGet]
        public ActionResult AdminDashBoard(string Sortorder, string SortBy, string Search, string month,int PageNumber = 1)
        {
            ViewBag.Admindashboard = "active";
            var now = DateTime.Now;
            ViewBag.MonthList = Enumerable.Range(1, 6).Select(x => new { Value = now.AddMonths(-x + 1).ToString("MM").ToString(), Text = now.AddMonths(-x + 1).ToString("MMM").ToString() }).ToList();

            var last7days = DateTime.Now.AddDays(-7);
            var notesinreview = db.SellerNotes.Where(x => x.Status == 7 || x.Status == 8).Count();
            ViewBag.publishnote = notesinreview;
            var notedownloaded = db.Downloads.Where(x => x.AttechmentDownloadDate > last7days).Count();
            ViewBag.downloaded = notedownloaded;
            var registration = db.Users.Where(x =>x.RoleID == 1 && x.CreatedDate > last7days).Count();
            ViewBag.newregistration = registration;
            var progressnote = db.SellerNotes.Where(x => x.Status == 9).Count();
            if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.SellerNotes.Where(x => x.Status == 9).ToList();
                if (!String.IsNullOrEmpty(Search))
                {
                    if (Search == "Free")
                    {
                        uv = db.SellerNotes.Where(x => x.IsPaid.ToString().Contains("false")).ToList();
                    }
                    else if (Search == "Paid")
                    {
                        uv = db.SellerNotes.Where(x => x.IsPaid.ToString().Contains("true")).ToList();
                    }
                    else
                    {
                         uv = db.SellerNotes.Where(x => x.Title.Contains(Search) || x.NoteCategory.Name.Contains(Search) || x.SellingPrice.ToString().Contains(Search) || x.PublishedDate.ToString().Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search)).ToList();
                    }
                }
                else if (!String.IsNullOrEmpty(month))
                {
                    //var uv1 = db.SellerNotes.Where(x => x.PublishedDate.Value.ToString("MM").ToString);
                     uv = db.SellerNotes.Where(x => x.PublishedDate.Value.ToString().Contains(month)).ToList();
                }
                var uv1 = db.SellerNotes.Where(x => x.Status == 9);
                foreach (var item in uv1)
                {
                    var attachment1 = db.SellerNotesAttechments.Where(x => x.NoteID == item.ID);
                    decimal filesize = 0;
                    decimal sizekb = 0;
                    foreach (var files in attachment1)
                    {
                        string filepath = Server.MapPath(files.FilePath + files.FileName);
                        FileInfo file = new FileInfo(filepath);
                        filesize += file.Length;
                    }
                    sizekb = Math.Ceiling(filesize / 1024);
                }
                
                ViewBag.totalpages = Math.Ceiling(uv.Count() / 5.0);
                ViewBag.PageNumber = PageNumber;

                uv = uv.Skip((PageNumber - 1) * 5).Take(5).ToList();

                switch (SortBy)
                {
                    case "CATEGORY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.NoteCategory.Name).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteCategory.Name).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteCategory.Name).ToList();
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
                                        uv = uv.OrderBy(x => x.Title).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "PUBLISHED DATE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.PublishedDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.PublishedDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.PublishedDate).ToList();
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
                                        uv = uv.OrderBy(x => x.SellingPrice.ToString()).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.SellingPrice.ToString()).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.SellingPrice.ToString()).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "SELLER":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
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
                                        uv = uv.OrderBy(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            uv = uv.OrderByDescending(x => x.PublishedDate).ToList();
                            break;
                        }
                }

                return View(uv);
            }
            else
            {
                var list = db.SellerNotes.Where(x => x.Status == 9).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }

        public ActionResult AdminDashBoardDownload(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var seller = db.SellerNotes.Where(X => X.ID == id).SingleOrDefault();
            var sellerNote = db.SellerNotesAttechments.Where(X => X.NoteID == id).SingleOrDefault();
            var Admindashdownload = db.Downloads.Where(x => x.NoteID == id && x.AttechmentPath != null).FirstOrDefault();
            if (Admindashdownload != null)
            {
                string path = Admindashdownload.AttechmentPath;
                path = Server.MapPath(path);
                DirectoryInfo dir = new DirectoryInfo(path);
                using (var memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in dir.GetFiles())
                        {
                            string filepath = path + item.ToString();
                            ziparchive.CreateEntryFromFile(filepath, item.ToString());
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
                }
            }
            else { return RedirectToAction("AdminDashBoard", "AdminDashBoard"); }

        }

        [HttpGet]
        public ActionResult UnpublishedModel(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.id = progressnote.ID;
            ViewBag.notetitle = progressnote.Title;
            ViewBag.category = progressnote.NoteCategory.Name;
            return View();
        }

        [HttpPost]
        public ActionResult UnpublishedModel(int id, SellerNote note)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();
            progressnote.AdminRemarks = note.AdminRemarks;
            progressnote.ActionBy = user.ID;
            progressnote.ModifiedDate = DateTime.Now;
            progressnote.MidifiedBy = user.ID;
            progressnote.Status = 11;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            SendUnpublishEmail(progressnote.User.EmailID,progressnote.User.FirstName,progressnote.Title,progressnote.AdminRemarks);
            return RedirectToAction("AdminDashBoard", "AdminDashBoard");
        }

        private void SendUnpublishEmail(string emailID, string firstName, string title, string adminRemarks)
        {
            var fromEmail = new MailAddress("k.harshil2000@gmail.com", "Notes Market Place");
            var toEmail = new MailAddress(emailID);
            string subject = "Sorry! We need to Remove your notes from our portal.";

            string body = " Hello " + firstName + "<br/><br/>We Want to inform you that, your note " + title + " has been removed from the portal." +

                "<br/>Please find our remarks as below- " + "<br/>"+ adminRemarks + "<br/><br/>Regards,<br/>Notes MarketPlace";

            using (var mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                SendMail.SendEmail(mail);
        }



        //Notes Under Review Note 
        [HttpGet]
        public ActionResult NotesUnderReview(string Sortorder, string SortBy, string Search, int? Seller, int PageNumber = 1)
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();
            ViewBag.Sellerlist = mylist.SellerNotes.Select(x => new { Value = x.SellerID, Text = x.User.FirstName + x.User.LastName }).Distinct().ToList();
            var progressnote = db.SellerNotes.Where(x => x.Status == 7 || x.Status == 8).Count();
          
            if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.SellerNotes.Where(x => x.Status == 7 || x.Status == 8).ToList();
                if (!String.IsNullOrEmpty(Search))
                {
                     uv = db.SellerNotes.Where(x => x.Title.Contains(Search) || x.NoteCategory.Name.Contains(Search) || x.Status.ToString().Contains(Search) || x.CreatedDate.ToString().Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search)).ToList();
                }
                if (Seller != null)
                {
                    uv = db.SellerNotes.Where(x => x.SellerID == Seller).ToList();
                }

                ViewBag.totalpages = Math.Ceiling(uv.Count() / 5.0);
                ViewBag.PageNumber = PageNumber;

                uv = uv.Skip((PageNumber - 1) * 5).Take(5).ToList();

                switch (SortBy)
                {
                    case "CATEGORY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.NoteCategory.Name).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteCategory.Name).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteCategory.Name).ToList();
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
                                        uv = uv.OrderBy(x => x.Title).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "DATE ADDED":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.CreatedDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.CreatedDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.CreatedDate).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "STATUS":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.ReferenceData.value).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.ReferenceData.value).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.ReferenceData.value).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "SELLER":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            uv = uv.OrderByDescending(x => x.CreatedDate).ToList();
                            break;
                        }
                }

                return View(uv);
            }
            else
            {
                var list = db.SellerNotes.Where(x => x.Status == 7 || x.Status == 8).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }

        public ActionResult Approved(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();
            progressnote.Status = 9;
            progressnote.ModifiedDate = DateTime.Now;
            progressnote.MidifiedBy = user.ID;
            progressnote.PublishedDate = DateTime.Now;
            progressnote.ActionBy = user.ID;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("NotesUnderReview", "AdminDashBoard");

        }

        public ActionResult InReview(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();
            progressnote.Status = 8;
            progressnote.ModifiedDate = DateTime.Now;
            progressnote.MidifiedBy = user.ID;
            progressnote.ActionBy = user.ID;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("NotesUnderReview","AdminDashBoard");
        }

        [HttpGet]
        public ActionResult RejectedModel(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.id = progressnote.ID;
            ViewBag.notetitle = progressnote.Title;
            ViewBag.category = progressnote.NoteCategory.Name;
            return View();
        }

        [HttpPost]
        public ActionResult RejectedModel(int id,SellerNote note)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();
           
            progressnote.AdminRemarks = note.AdminRemarks;
            progressnote.ActionBy = user.ID;
            progressnote.ModifiedDate = DateTime.Now;
            progressnote.MidifiedBy = user.ID;
            progressnote.Status = 10;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("NotesUnderReview", "AdminDashBoard");
        }



        //Published Note
        [HttpGet]
        public ActionResult PublishedNotes(string Sortorder, string SortBy, string Search,int? Seller, int PageNumber = 1)
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();

            ViewBag.Sellerlist = mylist.SellerNotes.Select(x => new { Value = x.SellerID, Text = x.User.FirstName + x.User.LastName}).Distinct().ToList();
            var progressnote = db.SellerNotes.Where(x => x.Status == 9).Count();
          
             if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.SellerNotes.Where(x => x.Status == 9).ToList();
                if (!String.IsNullOrEmpty(Search))
                {
                    if (Search == "Free")
                    {
                         uv = db.SellerNotes.Where(x => x.IsPaid.ToString().Contains("false")).ToList();
                    }
                    else if (Search == "Paid")
                    {
                         uv = db.SellerNotes.Where(x => x.IsPaid.ToString().Contains("true")).ToList();
                    }
                    else
                    {
                         uv = db.SellerNotes.Where(x => x.Title.Contains(Search) || x.NoteCategory.Name.Contains(Search) || x.SellingPrice.ToString().Contains(Search) || x.PublishedDate.ToString().Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search)).ToList();
                    }

                }
                if (Seller != null)
                {
                    uv = db.SellerNotes.Where(x => x.SellerID == Seller).ToList();
                }

                ViewBag.totalpages = Math.Ceiling(uv.Count() / 5.0);
                ViewBag.PageNumber = PageNumber;

                uv = uv.Skip((PageNumber - 1) * 5).Take(5).ToList();

                switch (SortBy)
                {
                    case "CATEGORY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.NoteCategory.Name).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteCategory.Name).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteCategory.Name).ToList();
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
                                        uv = uv.OrderBy(x => x.Title).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "PUBLISHED DATE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.PublishedDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.PublishedDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.PublishedDate).ToList();
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
                                        uv = uv.OrderBy(x => x.SellingPrice.ToString()).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.SellingPrice.ToString()).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.SellingPrice.ToString()).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "SELLER":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
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
                                        uv = uv.OrderBy(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            uv = uv.OrderByDescending(x => x.PublishedDate).ToList();
                            break;
                        }
                }

                return View(uv);
            }
            else
            {
                var list = db.SellerNotes.Where(x => x.Status == 7 || x.Status == 8).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }

        [HttpGet]
        public ActionResult Unpublished(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.id = progressnote.ID;
            ViewBag.notetitle = progressnote.Title;
            ViewBag.category = progressnote.NoteCategory.Name;
            return View();
        }

        [HttpPost]
        public ActionResult Unpublished(int id, SellerNote note)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();
            progressnote.AdminRemarks = note.AdminRemarks;
            progressnote.ActionBy = user.ID;
            progressnote.ModifiedDate = DateTime.Now;
            progressnote.MidifiedBy = user.ID;
            progressnote.Status = 11;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            SendUnpublishEmail(progressnote.User.EmailID, progressnote.User.FirstName, progressnote.Title, progressnote.AdminRemarks);
            return RedirectToAction("AdminDashBoard", "AdminDashBoard");
        }



        //Downloaded Note
        [HttpGet]
        public ActionResult DownloadNotes(string Sortorder, string SortBy, string Search, int? Seller, string Note, int? Buyer, int PageNumber = 1)
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();
            ViewBag.Sellerlist = mylist.SellerNotes.Select(x => new { Value = x.SellerID, Text = x.User.FirstName + x.User.LastName }).Distinct().ToList();
            var note = mylist.Downloads.Where(x => x.IsAttechmentDownloads == true).Select(x => x.NoteTitle).Distinct().ToList();
            ViewBag.note = new SelectList(note, "Note");
            ViewBag.Buyerlist = mylist.Downloads.Where(x => x.IsAttechmentDownloads == true).Select(x => new { Value = x.ID, Text = x.User.FirstName + x.User.LastName }).Distinct().ToList();
            var progressnote = db.Downloads.Where(x => x.IsAttechmentDownloads == true).Count();
            if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.Downloads.Where(x => x.IsAttechmentDownloads == true).ToList();
                if (!String.IsNullOrEmpty(Search))
                {
                    if (Search == "Free")
                    {
                         uv = db.Downloads.Where(x => x.IsPaid.ToString().Contains("false")).ToList();

                    }
                    else if (Search == "Paid")
                    {
                         uv = db.Downloads.Where(x => x.IsPaid.ToString().Contains("true")).ToList();
                    }
                    else
                    {
                        uv = db.Downloads.Where(x => x.NoteTitle.Contains(Search) || x.NoteCategory.Contains(Search) || x.PurchasedPrice.ToString().Contains(Search) || x.AttechmentDownloadDate.ToString().Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search)).ToList();
                    }

                }
                if (Seller != null)
                {
                     uv = db.Downloads.Where(x => x.Seller == Seller && x.IsAttechmentDownloads == true).ToList();
                }
                if (!String.IsNullOrEmpty(Note))
                {
                     uv = db.Downloads.Where(x => x.NoteTitle.Contains(Note) && x.IsAttechmentDownloads == true).ToList();
                }
                if (Buyer != null)
                {
                     uv = db.Downloads.Where(x => x.ID == Buyer && x.IsAttechmentDownloads == true).ToList();
                }

                ViewBag.totalpages = Math.Ceiling(uv.Count() / 5.0);
                ViewBag.PageNumber = PageNumber;

                uv = uv.Skip((PageNumber - 1) * 5).Take(5).ToList();

                switch (SortBy)
                {
                    case "CATEGORY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.NoteCategory).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteCategory).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteCategory).ToList();
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
                                        uv = uv.OrderBy(x => x.NoteTitle).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteTitle).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteTitle).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "DOWNLOADED DATE/TIME":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.AttechmentDownloadDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.AttechmentDownloadDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.AttechmentDownloadDate).ToList();
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
                                        uv = uv.OrderBy(x => x.PurchasedPrice.ToString()).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.PurchasedPrice.ToString()).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.PurchasedPrice.ToString()).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "BUYER":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "SELLER":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.SellerNote.User.FirstName).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.SellerNote.User.FirstName).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.SellerNote.User.FirstName).ToList();
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
                                        uv = uv.OrderBy(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            uv = uv.OrderByDescending(x => x.AttechmentDownloadDate).ToList();
                            break;
                        }
                }

                return View(uv);
            }
            else
            {
                var list = db.Downloads.Where(x => x.IsAttechmentDownloads == true).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }



        //Rejected Note
        [HttpGet]
        public ActionResult RejectedNotes(string Sortorder, string SortBy, string Search, int? Seller, int PageNumber = 1)
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();
            ViewBag.Sellerlist = mylist.SellerNotes.Select(x => new { Value = x.SellerID, Text = x.User.FirstName + x.User.LastName }).Distinct().ToList();
            var progressnote = db.SellerNotes.Where(x => x.Status == 10).Count();
            if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.SellerNotes.Where(x => x.Status == 10).ToList();
                if (!String.IsNullOrEmpty(Search))
                {
                     uv = db.SellerNotes.Where(x => x.Title.Contains(Search) || x.NoteCategory.Name.Contains(Search) || x.SellingPrice.ToString().Contains(Search) || x.PublishedDate.ToString().Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search)).ToList();
                }
                if (Seller != null)
                {
                     uv = db.SellerNotes.Where(x => x.SellerID == Seller && x.Status == 10).ToList();
                }

                ViewBag.totalpages = Math.Ceiling(uv.Count() / 5.0);
                ViewBag.PageNumber = PageNumber;

                uv = uv.Skip((PageNumber - 1) * 5).Take(5).ToList();

                switch (SortBy)
                {
                    case "CATEGORY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.NoteCategory.Name).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteCategory.Name).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.NoteCategory.Name).ToList();
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
                                        uv = uv.OrderBy(x => x.Title).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "DATE ADDED":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.ModifiedDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.ModifiedDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.ModifiedDate).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "SELLER":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "REJECTED BY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.IsPaid.ToString()).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            uv = uv.OrderByDescending(x => x.ModifiedDate).ToList();
                            break;
                        }
                }

                return View(uv);
            }
            else
            {
                var list = db.SellerNotes.Where(x => x.Status == 7 || x.Status == 8).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }


        //Spam Report
        [HttpGet]
        public ActionResult SpamReport(string Sortorder, string SortBy, string Search, int PageNumber = 1)
        {
            var spamnote = db.SellerNotesReportedIssues.Where(x => x.ID != 0).Count();
            if (spamnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.SellerNotesReportedIssues.ToList();
                if (Search != null)
                {
                    uv = db.SellerNotesReportedIssues.Where(x => x.SellerNote.Title.Contains(Search) || x.SellerNote.NoteCategory.Name.Contains(Search) || x.CreatedDate.ToString().Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search)).ToList();
                }

                ViewBag.totalpages = Math.Ceiling(uv.Count() / 5.0);
                ViewBag.PageNumber = PageNumber;

                uv = uv.Skip((PageNumber - 1) * 5).Take(5).ToList();

                switch (SortBy)
                {
                    case "CATEGORY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.SellerNote.NoteCategory.Name).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.SellerNote.NoteCategory.Name).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.SellerNote.NoteCategory.Name).ToList();
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
                                        uv = uv.OrderBy(x => x.SellerNote.Title).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.SellerNote.Title).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.SellerNote.Title).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "DATE EDITED":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.CreatedDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.CreatedDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.CreatedDate).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "REPORTED BY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.SellerNote.User).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.User.FirstName).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "REMARK":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.Remarks).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.Remarks).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.Remarks).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            uv = uv.OrderByDescending(x => x.CreatedDate).ToList();
                            break;
                        }
                }

                return View(uv);
            }
            else
            {
                var list = db.SellerNotesReportedIssues.Where(x => x.ID != 0).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }

        public ActionResult DeleteSpamReport(int id)
        {
            var deletespam = db.SellerNotesReportedIssues.Where(x => x.ID == id).FirstOrDefault();
            db.SellerNotesReportedIssues.Remove(deletespam);
            db.SaveChanges();
            return RedirectToAction("SpamReport","AdminDashBoard");
        }
    }
}