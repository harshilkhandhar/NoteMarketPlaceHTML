using Notes_Market_Place.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Notes_Market_Place.Controllers 
{
    public class NotesController : Controller
    {
        NotesMarketPLaceEntities db = new NotesMarketPLaceEntities();
        // GET: Notes
        [HttpGet]
        public ActionResult AddNotes()
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();
            var category = mylist.NoteCategories.Where(x => x.IsActive == true).ToList();
            var type = mylist.NoteTypes.Where(x => x.IsActive == true).ToList();
            var country = mylist.Countries.Where(x => x.IsActive == true).ToList();
            SelectList list = new SelectList(category, "ID", "Name");
            ViewBag.categoryname = list;
            ViewBag.typename = new SelectList(type, "ID", "Name");
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotesPost(SellerNote sellerNote )
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var category = mylist.NoteCategories.Where(x => x.IsActive == true).ToList();
            var type = mylist.NoteTypes.Where(x => x.IsActive == true).ToList();
            var country = mylist.Countries.Where(x => x.IsActive == true).ToList();
            SelectList list = new SelectList(category, "ID", "Name");
            ViewBag.categoryname = list;
            ViewBag.typename = new SelectList(type, "ID", "Name");
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            if (ModelState.IsValid)
            {
                sellerNote.SellerID = user.ID;
                sellerNote.Status = 6;
                sellerNote.CreatedBy = user.ID;
                sellerNote.CreatedDate = DateTime.Now;
                sellerNote.MidifiedBy = user.ID;
                sellerNote.ModifiedDate = DateTime.Now;
                sellerNote.IsActive = true;
                SellerNotesAttechment notesAttechment = new SellerNotesAttechment();
               
                if (sellerNote.Status == 6)
                {
                    ViewBag.Status = "Draft";
                }
                ViewBag.Status = sellerNote.Status;
               
                using (var db = new NotesMarketPLaceEntities())
                {
                    db.SellerNotes.Add(sellerNote);
                    db.SaveChanges();   
                }
                var ab = db.SellerNotes.Where(x => x.ID == sellerNote.ID).FirstOrDefault();
                if (sellerNote.DisplayImage != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(sellerNote.DisplayImage.FileName);

                    string FileExtension = Path.GetExtension(sellerNote.DisplayImage.FileName);

                    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                    var path = Server.MapPath("~/Members/" + sellerNote.SellerID + "/" + sellerNote.ID + "/DisplayPicture/");

                    ab.DisplayPicture = "~/Members/" + sellerNote.SellerID + "/" + sellerNote.ID + "/DisplayPicture/" + FileName;
                    Directory.CreateDirectory(path);
                   
                    FileName = Path.Combine(path, FileName);
                   
                    sellerNote.DisplayImage.SaveAs(FileName);
                }
                if (sellerNote.notepreview != null)
                {
                    string FileName2 = Path.GetFileNameWithoutExtension(sellerNote.notepreview.FileName);

                    string FileExtension2 = Path.GetExtension(sellerNote.notepreview.FileName);

                    FileName2 = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName2.Trim() + FileExtension2;

                    var path = Server.MapPath("~/Members/" + sellerNote.SellerID + "/" + sellerNote.ID + "/NotesPreview/");

                    Directory.CreateDirectory(path);
                    ab.NotesPreview = "~/Members/" + sellerNote.SellerID + "/" + sellerNote.ID + "/NotesPreview/" + FileName2;

                    FileName2 = Path.Combine(path, FileName2);

                    sellerNote.notepreview.SaveAs(FileName2);
                }
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                if (sellerNote.uploadNote != null)
                {
                    foreach (HttpPostedFileBase file in sellerNote.uploadNote)
                    {
                        if (file != null)
                        {
                            var InputFileName = Path.GetFileName(file.FileName);
                            var path = Server.MapPath("~/Members/" + sellerNote.SellerID + "/" + sellerNote.ID + "/Attechment/");
                            Directory.CreateDirectory(path);
                            var ServerSavePath = Path.Combine(path + InputFileName);
                            file.SaveAs(ServerSavePath);
                            notesAttechment.FileName = InputFileName;
                            notesAttechment.FilePath = "~/Members/" + sellerNote.SellerID + "/" + sellerNote.ID + "/Attechment/";
                        }
                    }
                }
                notesAttechment.NoteID = sellerNote.ID;
                notesAttechment.CreatedBy = user.ID;
                notesAttechment.MidifiedBy = user.ID;
                notesAttechment.CreatedDate = DateTime.Now;
                notesAttechment.ModifiedDate = DateTime.Now;
                db.SellerNotesAttechments.Add(notesAttechment);
                db.SaveChanges();
                return RedirectToAction("DashBoard", "Notes", new { id = sellerNote.SellerID });
            }
            return View();
        }

        public ActionResult AddnotePublish(int id)
        {
            var Status = db.SellerNotes.Where(x => x.ID == id).SingleOrDefault();
            Status.Status = 7;
            Status.ModifiedDate = DateTime.Now;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            SendpublishLinkEmail(Status.Title, Status.User.FullName);
            return RedirectToAction("DashBoard", "Notes", new { id = Status.SellerID });
        }

        private void SendpublishLinkEmail(string notetitle, string username)
        {
            //var verifyUrl = "Home/EmailVerification/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("k.harshil2000@gmail.com", "Notes Market Place");
            var toEmail = new MailAddress("harshilkhandhar7@gmail.com");
            string subject = username +" "+ "Send his note for review";

            string body = "Hello Admins," + "<br/><br/>We Want to inform you that, " + username + " sent his note "

               + notetitle + " for review.Please look at the note and take required actions. " + "<br/><br/>Regards,<br/>Notes MarketPlace";

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



        [HttpGet]
        [Authorize]
        public ActionResult DashBoard(string Search, string SearchPublished, string Sortorder, string SortBy, string SortorderPublish, string SortByPublish, int PageNumberPublish = 1, int PageNumber = 1)
        {
            ViewBag.Dash = "active";
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(quere => quere.SellerID == user.ID).FirstOrDefault();
            if (progressnote != null)
            {
                    var buyerrequest = db.Downloads.Where(x => x.IsPaid == true && x.IsSellerHasAllowedDownloads == false && x.Seller == user.ID).Count();
                    var mydownload = db.Downloads.Where(x => x.Downloader == user.ID && x.IsAttechmentDownloads == true).Count();
                    var mysoldnote = db.Downloads.Where(x => x.IsSellerHasAllowedDownloads == true && x.Seller == user.ID).Count();
                    var myearning = db.Downloads.Where(x => x.IsSellerHasAllowedDownloads == true && x.Seller == user.ID).Sum(x => x.PurchasedPrice);
                    var myrejectednote = db.SellerNotes.Where(x => x.Status == 10 && x.SellerID == user.ID).Count();
                    ViewBag.Buyer = buyerrequest;
                    ViewBag.MyDownload = mydownload;
                    ViewBag.MySoldNote = mysoldnote;
                    ViewBag.MyEarning = myearning;
                    ViewBag.Rejected = myrejectednote;
                    ViewBag.Sortorder = Sortorder;
                    ViewBag.SortBy = SortBy;
                    ViewBag.SortorderPublish = SortorderPublish;
                    ViewBag.SortByPublish = SortByPublish;

                SellerNote sellerNote = new SellerNote();

                sellerNote.InProgressNotes = from note in db.SellerNotes
                                             where (note.Status == 6 || note.Status == 7 || note.Status == 8) && note.SellerID == user.ID
                                             select new InProgressNote
                                             {
                                                 NoteId = note.ID,
                                                 Title = note.Title,
                                                 Category = note.NoteCategory.Name,
                                                 Status = note.ReferenceData.value,
                                                 AddedDate = note.CreatedDate.Value
                                             };

                if (!string.IsNullOrEmpty(Search))
                {
                    sellerNote.InProgressNotes = sellerNote.InProgressNotes.Where(x => x.Category.Contains(Search) || x.Title.Contains(Search) || x.Status.Contains(Search)).ToList();
                }

                sellerNote.PublishedNotes = from note in db.SellerNotes
                                            where (note.Status == 9) && note.SellerID == user.ID
                                            select new PublishedNote
                                            {
                                                NoteId = note.ID,
                                                Title = note.Title,
                                                Category = note.NoteCategory.Name,
                                                Price = note.SellingPrice,
                                                Selltype = note.IsPaid,
                                                PublishedDate = note.PublishedDate.Value
                                            };

                    ViewBag.totalpages = Math.Ceiling(sellerNote.InProgressNotes.Count() / 5.0);
                    ViewBag.PageNumber = PageNumber;

                    sellerNote.InProgressNotes = sellerNote.InProgressNotes.Skip((PageNumber - 1) * 5).Take(5).ToList();

                    switch (SortBy)
                    {
                        case "CATEGORY":
                            {
                                switch (Sortorder)
                                {

                                    case "ASC":
                                        {
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderBy(x => x.Category).ToList();
                                            break;
                                        }
                                    case "DESC":
                                        {
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderByDescending(x => x.Category).ToList();
                                            break;
                                        }
                                    default:
                                        {
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderByDescending(x => x.Category).ToList();
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
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderBy(x => x.Title).ToList();
                                            break;
                                        }
                                    case "DESC":
                                        {
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderByDescending(x => x.Title).ToList();
                                            break;
                                        }
                                    default:
                                        {
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderByDescending(x => x.Title).ToList();
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
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderBy(x => x.Status).ToList();
                                            break;
                                        }
                                    case "DESC":
                                        {
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderByDescending(x => x.Status).ToList();
                                            break;
                                        }
                                    default:
                                        {
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderByDescending(x => x.Status).ToList();
                                            break;
                                        }
                                }
                                break;
                            }
                        case "ADDED DATE":
                            {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderBy(x => x.AddedDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderByDescending(x => x.AddedDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderByDescending(x => x.AddedDate).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                        default:
                            {
                            sellerNote.InProgressNotes = sellerNote.InProgressNotes.OrderByDescending(x => x.Title).ToList();
                                break;
                            }
                    }

                if (!string.IsNullOrEmpty(SearchPublished))
                {
                    if (SearchPublished == "Free")
                    {
                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.Where(x => x.Selltype.Equals(false)).ToList();
                    }
                    else if (SearchPublished == "Paid")
                    {
                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.Where(x => x.Selltype.Equals(true)).ToList();
                    }
                    else
                    {
                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.Where(x => x.Category.Contains(SearchPublished) || x.Title.Contains(SearchPublished) || x.Price.ToString().Contains(SearchPublished)).ToList();
                    }
                }

                ViewBag.totalpagesPublish = Math.Ceiling(sellerNote.PublishedNotes.Count() / 5.0);
                ViewBag.PageNumberPublish = PageNumberPublish;

                sellerNote.PublishedNotes = sellerNote.PublishedNotes.Skip((PageNumberPublish - 1) * 5).Take(5).ToList();

                switch (SortByPublish)
                {
                    case "CATEGORY":
                        {
                            switch (SortorderPublish)
                            {

                                case "ASC":
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderBy(x => x.Category).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.Category).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.Category).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "TITLE":
                        {
                            switch (SortorderPublish)
                            {

                                case "ASC":
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderBy(x => x.Title).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "ADDED DATE":
                        {
                            switch (SortorderPublish)
                            {

                                case "ASC":
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderBy(x => x.PublishedDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.PublishedDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.PublishedDate).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "PRICE":
                        {
                            switch (SortorderPublish)
                            {

                                case "ASC":
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderBy(x => x.Price).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.Price).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.Price).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "SELL TYPE":
                        {
                            switch (SortorderPublish)
                            {

                                case "ASC":
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderBy(x => x.Selltype).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.Selltype).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.Selltype).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            sellerNote.PublishedNotes = sellerNote.PublishedNotes.OrderByDescending(x => x.Title).ToList();
                            break;
                        }
                }

                return View(sellerNote);
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditNote(int id)
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();
            var category = mylist.NoteCategories.Where(x => x.IsActive == true).ToList();
            var type = mylist.NoteTypes.Where(x => x.IsActive == true).ToList();
            var country = mylist.Countries.Where(x => x.IsActive == true).ToList();
            SelectList list = new SelectList(category, "ID", "Name");
            ViewBag.categoryname = list;
            ViewBag.typename = new SelectList(type, "ID", "Name");
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            var edit = db.SellerNotes.Where(x => x.ID == id).SingleOrDefault();
            if (edit != null)
            {
                return View(edit);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditNote(SellerNote seller)
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();
            var category = mylist.NoteCategories.Where(x => x.IsActive == true).ToList();
            var type = mylist.NoteTypes.Where(x => x.IsActive == true).ToList();
            var country = mylist.Countries.Where(x => x.IsActive == true).ToList();
            SelectList list = new SelectList(category, "ID", "Name");
            ViewBag.categoryname = list;
            ViewBag.typename = new SelectList(type, "ID", "Name");
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            db.Configuration.ValidateOnSaveEnabled = false;
            seller.ModifiedDate = DateTime.Now;
            db.Entry(seller).State = EntityState.Modified;
            db.SaveChanges();
            var list1 = db.SellerNotes.ToList();
            return RedirectToAction("DashBoard", "Notes", new { id = seller.SellerID });
            //return View();
        }

        public ActionResult DeleteNote(int id)
        {
            var data = db.SellerNotes.Where(query => query.ID == id).First();
            if (data != null)
            {
                var abc = db.SellerNotesAttechments.Where(query => query.NoteID == data.ID).SingleOrDefault();
                string attechmentpath = abc.FilePath;
                string path1 = data.DisplayPicture;
                string path2 = data.NotesPreview;
                path1 = Server.MapPath(path1);
                path2 = Server.MapPath(path2);
                attechmentpath = Server.MapPath(attechmentpath + abc.FileName);
                System.IO.File.Delete(attechmentpath);
                System.IO.File.Delete(path1);
                System.IO.File.Delete(path2);
                db.SellerNotes.Remove(data);
                db.SellerNotesAttechments.Remove(abc);
                db.SaveChanges();
                var list = db.SellerNotes.ToList();
                return RedirectToAction("DashBoard","Notes",new {id = data.SellerID });
            }
            return View();
        }

    
      


        // Notes Detail Page

        [Authorize]
        public ActionResult Download(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var seller = db.SellerNotes.Where(X => X.ID == id).SingleOrDefault();
            var sellerNote = db.SellerNotesAttechments.Where(X => X.NoteID == id).SingleOrDefault();
            var downloadfreenote = db.Downloads.Where(x => x.NoteID == seller.ID && x.Downloader == user.ID && x.IsSellerHasAllowedDownloads == true && x.AttechmentPath != null).FirstOrDefault();
            if (downloadfreenote == null)
            {
                Download download = new Download();
                download.NoteID = seller.ID;
                download.Downloader = user.ID;
                download.Seller = seller.SellerID;
                download.IsSellerHasAllowedDownloads = true;
                download.NoteTitle = seller.Title;
                download.NoteCategory = seller.NoteCategory.Name;
                download.IsPaid = seller.IsPaid;
                download.PurchasedPrice = seller.SellingPrice;
                download.CreatedBy = user.ID;
                download.CreatedDate = DateTime.Now;
                download.ModifiedDate = DateTime.Now;
                download.AttechmentPath = sellerNote.FilePath;
                download.AttechmentDownloadDate = DateTime.Now;
                download.Downloader = 1;
                download.IsAttechmentDownloads = true;
                db.Downloads.Add(download);
                db.SaveChanges();
                string path = sellerNote.FilePath;
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
            else
            {
                var downloadpaidnote = db.Downloads.Where(x => x.NoteID == seller.ID && x.Downloader == user.ID && x.IsSellerHasAllowedDownloads == true && x.AttechmentPath != null).FirstOrDefault();
                if (downloadpaidnote != null)
                {
                    if (downloadpaidnote.IsAttechmentDownloads == false)
                    {
                        downloadpaidnote.IsAttechmentDownloads = true;
                        downloadpaidnote.AttechmentDownloadDate = DateTime.Now;
                        downloadpaidnote.ModifiedDate = DateTime.Now;
                        downloadpaidnote.MidifiedBy = user.ID;
                        db.SaveChanges();
                    }
                    string path = downloadpaidnote.AttechmentPath;
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
            }
            return View();
        }

       
        public ActionResult DownloadPaid(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var seller = db.SellerNotes.Where(X => X.ID == id).SingleOrDefault();
            var sellerNote = db.SellerNotesAttechments.Where(X => X.NoteID == id).SingleOrDefault();
            var Download = db.Downloads.Where(X => X.NoteID == id).SingleOrDefault();
            if(Download.IsAttechmentDownloads== false && Download.IsSellerHasAllowedDownloads == false)
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Download.IsSellerHasAllowedDownloads = true;
                Download.AttechmentPath = sellerNote.FilePath;
                db.SaveChanges();
                SendAllowDownloadEmail(user.EmailID, seller.User.FirstName, user.FirstName);
            }
            return RedirectToAction("BuyerRequest","Home");
        }

        private void SendAllowDownloadEmail(string emailID, string firstName1, string firstName2)
        {

            var fromEmail = new MailAddress("k.harshil2000@gmail.com", "Notes Market Place");
            var toEmail = new MailAddress(emailID);
           
            string subject = firstName1 +" "+ "Allows you to download a note";

            string body = "Hello, " + firstName2 + "<br/><br/>We would like to inform you that, " + firstName1 +

                " Allows you to download a note.Please login and see My Download tabs to download particular note. " + " <br/><br/>Regards,<br/>Notes MarketPlace";

            using (var mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                SendMail.SendEmail(mail);
        }

        public ActionResult PaidNote(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var seller = db.SellerNotes.Where(x => x.ID == id).SingleOrDefault();
            Download download = new Download();
            download.NoteID = seller.ID;
            download.Downloader = user.ID;
            download.Seller = seller.SellerID;
            download.IsSellerHasAllowedDownloads = false;
            download.NoteTitle = seller.Title;
            download.NoteCategory = seller.NoteCategory.Name;
            download.IsPaid = seller.IsPaid;
            download.PurchasedPrice = seller.SellingPrice;
            download.CreatedBy = user.ID;
            download.CreatedDate = DateTime.Now;
            download.ModifiedDate = DateTime.Now;
            download.AttechmentPath = null;
            download.IsAttechmentDownloads = false;
            db.Downloads.Add(download);
            db.SaveChanges();
            SendpaidnoteEmail(seller.User.EmailID, seller.User.FirstName, user.FirstName);
            return View("MessageModal");
        }

        private void SendpaidnoteEmail(string emailID, string firstName,string name)
        {
            var fromEmail = new MailAddress("k.harshil2000@gmail.com", "Notes Market Place");
            var toEmail = new MailAddress(emailID);
            
            string subject = name + " "+ "wants to purchase your notes";

            string body = "Hello, " + firstName + "<br/><br/>We would like to inform you that, " + name +

                " wants to purchase your notes. Please see Buyer Requests tab and allow download access to Buyer if you have received the payment from him." + "<br/><br/>Regards,<br/>Notes MarketPlace";

            using (var mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                SendMail.SendEmail(mail);
        }

        public ActionResult MessageModal()
        {
            return View();
        }



        //My DownloadNote
        [HttpGet]
        public ActionResult MyDownload(string Sortorder, string SortBy, string Search, int PageNumber = 1 )
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.Downloads.Where(x => x.Downloader == user.ID && x.IsSellerHasAllowedDownloads == true).FirstOrDefault();
          
             if (progressnote != null)
            {
                var list = db.Downloads.Where(x => x.Downloader == user.ID && x.IsSellerHasAllowedDownloads == true).ToList();
                if (Search != null)
                {
                    if (Search == "Free")
                    {
                         list = db.Downloads.Where(x => x.IsPaid.ToString().Contains("false")).ToList();
                    }
                    else if (Search == "Paid")
                    {
                         list = db.Downloads.Where(x => x.IsPaid.ToString().Contains("true")).ToList();
                    }
                    else
                    {
                         list = db.Downloads.Where(x => x.NoteTitle.Contains(Search) || x.NoteCategory.Contains(Search) || x.PurchasedPrice.ToString().Contains(Search)).ToList();
                    }
                }
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;
                ViewBag.totalpages = Math.Ceiling(list.Count() / 10.0);
                ViewBag.PageNumber = PageNumber;
                list = list.Skip((PageNumber - 1) * 10).Take(10).ToList();

                switch (SortBy)
                {
                    case "CATEGORY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        list = list.OrderBy(x => x.NoteCategory).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.NoteCategory).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.NoteCategory).ToList();
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
                                        list = list.OrderBy(x => x.NoteTitle).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.NoteTitle).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.NoteTitle).ToList();
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
                                        list = list.OrderBy(x => x.IsPaid).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.IsPaid).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.IsPaid).ToList();
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
                                        list = list.OrderBy(x => x.PurchasedPrice.ToString()).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.PurchasedPrice.ToString()).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.PurchasedPrice.ToString()).ToList();
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
                                        list = list.OrderBy(x => x.CreatedDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.CreatedDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.CreatedDate).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            list = list.OrderByDescending(x => x.CreatedDate).ToList();
                            break;
                        }
                }
                return View(list);
            }
            else
            {
                var list = db.Downloads.Where(x => x.Downloader == user.ID && x.IsSellerHasAllowedDownloads == true).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }

        [HttpGet]
        public ActionResult Addreview(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var data = db.Downloads.Where(x => x.NoteID == id && x.User.ID == user.ID).FirstOrDefault();
            if (data != null)
            {
                ViewBag.title = data.NoteTitle;
                ViewBag.id = id;
            }
            return View();
        } 

        [HttpPost]
        public ActionResult Addreview(int id,SellerNotesReview notesReview)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var seller = db.SellerNotes.Where(x => x.SellerID == user.ID).FirstOrDefault();
            var data = db.Downloads.Where(x => x.NoteID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                notesReview.CreatedBy = user.ID;
                notesReview.CreatedDate = DateTime.Now;
                notesReview.MidifiedBy = user.ID;
                notesReview.ModifiedDate = DateTime.Now;
                notesReview.NoteID = id;
                notesReview.ReviewByID = user.ID;
                notesReview.IsActive = true;
                notesReview.AgainstDownloadsID = data.ID;
                db.SellerNotesReviews.Add(notesReview);
                db.SaveChanges();
                return RedirectToAction("MyDownload", "Notes");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Reportanissue(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var data = db.Downloads.Where(x => x.NoteID == id && x.User.ID == user.ID).FirstOrDefault();
            if (data != null)
            {
                ViewBag.title = data.NoteTitle;
                ViewBag.id = data.NoteID;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Reportanissue(int id , SellerNotesReportedIssue reportedIssue)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var seller = db.SellerNotes.Where(x => x.SellerID == user.ID).FirstOrDefault();
            var data = db.Downloads.Where(x => x.NoteID == id).SingleOrDefault();
            if (ModelState.IsValid)
            {
                reportedIssue.NoteID = data.NoteID;
                reportedIssue.CreatedDate = DateTime.Now;
                reportedIssue.CreatedBy = user.ID;
                reportedIssue.ModifiedDate = DateTime.Now;
                reportedIssue.ReviewByID = user.ID;
                reportedIssue.AgainstDownloadsID = data.ID;
                SendReportIssueEmail(user.FirstName, seller.User.FirstName,data.NoteTitle);
                db.SellerNotesReportedIssues.Add(reportedIssue);
                db.SaveChanges();
                return RedirectToAction("MyDownload", "Notes");
            }
            else
            {
                return View();
            }
           
        }

        private void SendReportIssueEmail(string firstName1, string firstName2, string noteTitle)
        {

            var fromEmail = new MailAddress("k.harshil2000@gmail.com", "Notes MarketPlace");
            var toEmail = new MailAddress("harshilkhandhar7@gmail.com");
            
            string subject = firstName1 +" "+ " Reported an issue for " +" "+ noteTitle ;

            string body = "Hello Admins,<br/><br/> " + "We want to inform you that, "+ firstName1 + " Reported an issue for " +firstName2+"’s Note with title "+ noteTitle+". Please look at the notes and take required actions." + "<br/><br/>Regards,<br/>Notes Marketplace";
            using (var mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                SendMail.SendEmail(mail);
        }



        //My SlodNote
        [HttpGet]
        public ActionResult MySoldNote(string Sortorder, string SortBy, string Search, int PageNumber = 1)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.Downloads.Where(x => x.IsSellerHasAllowedDownloads == true && x.Seller == user.ID).FirstOrDefault();
            if (progressnote != null)
            {
                var list = db.Downloads.Where(x => x.IsSellerHasAllowedDownloads == true && x.Seller == user.ID).ToList();
                if (Search != null)
                {
                    if (Search == "Free")
                    {
                         list = db.Downloads.Where(x => x.IsPaid.ToString().Contains("false")).ToList();
                    }
                    else if (Search == "Paid")
                    {
                         list = db.Downloads.Where(x => x.IsPaid.ToString().Contains("true")).ToList();
                    }
                    else
                    {
                        list = db.Downloads.Where(x => x.NoteTitle.Contains(Search) || x.NoteCategory.Contains(Search) || x.PurchasedPrice.ToString().Contains(Search) || x.User1.EmailID.Contains(Search)).ToList();
                    }
                }
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;
                ViewBag.totalpages = Math.Ceiling(list.Count() / 10.0);
                ViewBag.PageNumber = PageNumber;
                list = list.Skip((PageNumber - 1) * 10).Take(10).ToList();

                switch (SortBy)
                {
                    case "CATEGORY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        list = list.OrderBy(x => x.NoteCategory).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.NoteCategory).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.NoteCategory).ToList();
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
                                        list = list.OrderBy(x => x.NoteTitle).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.NoteTitle).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.NoteTitle).ToList();
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
                                        list = list.OrderBy(x => x.IsPaid).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.IsPaid).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.IsPaid).ToList();
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
                                        list = list.OrderBy(x => x.PurchasedPrice).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.PurchasedPrice).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.PurchasedPrice).ToList();
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
                                        list = list.OrderBy(x => x.CreatedDate).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.CreatedDate).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.CreatedDate).ToList();
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
                                        list = list.OrderBy(x => x.User1.EmailID).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.User.EmailID).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.User.EmailID).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            list = list.OrderByDescending(x => x.CreatedDate).ToList();
                            break;
                        }
                }
                return View(list);
            }
            else
            {
                var list = db.Downloads.Where(x => x.IsSellerHasAllowedDownloads == true && x.Seller == user.ID).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }


        public ActionResult MySoldNoteDownload(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var seller = db.SellerNotes.Where(X => X.ID == id).SingleOrDefault();
            var sellerNote = db.SellerNotesAttechments.Where(X => X.NoteID == id).SingleOrDefault();
            var mysolddownload = db.Downloads.Where(x => x.NoteID == id && x.IsAttechmentDownloads == true).FirstOrDefault();
            if(mysolddownload != null)
            {
                string path = mysolddownload.AttechmentPath;
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
            else { return View(); }
            
        }



        //My RejectedNote
        [HttpGet]
        public ActionResult MyRejectedNote(string Sortorder, string SortBy, string Search, int PageNumber = 1)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(x => x.SellerID == user.ID && x.Status == 10).FirstOrDefault();
            if (progressnote != null)
            {
                var list = db.SellerNotes.Where(x => x.SellerID == user.ID && x.Status == 10).ToList();
                if (Search != null)
                {
                    list = db.SellerNotes.Where(x => x.Title.Contains(Search) || x.NoteCategory.Name.Contains(Search)).ToList();
                }
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;
                ViewBag.totalpages = Math.Ceiling(list.Count() / 10.0);
                ViewBag.PageNumber = PageNumber;
                list = list.Skip((PageNumber - 1) * 10).Take(10).ToList();

                switch (SortBy)
                {
                    case "CATEGORY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        list = list.OrderBy(x => x.NoteCategory.Name).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.NoteCategory.Name).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.NoteCategory.Name).ToList();
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
                                        list = list.OrderBy(x => x.Title).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.Title).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "REMARKS":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        list = list.OrderBy(x => x.AdminRemarks).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        list = list.OrderByDescending(x => x.AdminRemarks).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        list = list.OrderByDescending(x => x.AdminRemarks).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            list = list.OrderByDescending(x => x.Title).ToList();
                            break;
                        }
                }
                return View(list);
            }
            else
            {
                var list = db.SellerNotes.Where(x => x.SellerID == user.ID && x.Status == 11).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }

    }
}