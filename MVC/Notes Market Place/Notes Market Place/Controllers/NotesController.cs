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
            var category = mylist.NoteCategories.ToList();
            var type = mylist.NoteTypes.ToList();
            var country = mylist.Countries.ToList();
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
            var category = mylist.NoteCategories.ToList();
            var type = mylist.NoteTypes.ToList();
            var country = mylist.Countries.ToList();
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
            var fromEmailPassword = "*********"; // Replace with actual password
            string subject = username + "Send his note for review";

            string body = "Hello Admins," + "<br/><br/>We Want to inform you that," + username + "sent his note"

               + notetitle + "for review.Please look at the note and take required actions." + "<br/><br/>Regards,<br/>Notes MarketPlace";

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



        [HttpGet]
        [Authorize]
        public ActionResult DashBoard( string Sortorder, string SortBy, int PageNumber = 1)
        {
            //var uv = db.SellerNotes.ToList();
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.Where(quere => quere.SellerID == user.ID).FirstOrDefault();
            if (progressnote != null)
            {
                if (progressnote.Status != 9)
                {
                    ViewBag.Sortorder = Sortorder;
                    ViewBag.SortBy = SortBy;

                    var uv = db.SellerNotes.ToList().Where(quere => quere.Status != 9);

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
                        default:
                            {
                                uv = uv.OrderByDescending(x => x.Title).ToList();
                                break;
                            }
                    }

                    return View(uv);
                }
                return View();
            }
            else
            {
                var norecord = db.SellerNotes.ToList();
                ViewBag.Message = "No Record Found";
                return View(norecord);
            }
        }


        public ActionResult DashboardPartial(string SortorderPublish, string SortByPublish, int PageNumberPublish = 1)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.SellerNotes.ToList();
            ViewBag.SortorderPublish = SortorderPublish;
            ViewBag.SortByPublish = SortByPublish;

            var publishtable = db.SellerNotes.ToList().Where(quere => quere.Status == 9);

            ViewBag.totalpagesPublish = Math.Ceiling(publishtable.Count() / 5.0);
            ViewBag.PageNumberPublish = PageNumberPublish;

            publishtable = publishtable.Skip((PageNumberPublish - 1) * 5).Take(5).ToList();

            switch (SortByPublish)
            {
                case "CATEGORY":
                    {
                        switch (SortorderPublish)
                        {

                            case "ASC":
                                {
                                    publishtable = publishtable.OrderBy(x => x.NoteCategory.Name).ToList();
                                    break;
                                }
                            case "DESC":
                                {
                                    publishtable = publishtable.OrderByDescending(x => x.NoteCategory.Name).ToList();
                                    break;
                                }
                            default:
                                {
                                    publishtable = publishtable.OrderByDescending(x => x.NoteCategory.Name).ToList();
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
                                    publishtable = publishtable.OrderBy(x => x.Title).ToList();
                                    break;
                                }
                            case "DESC":
                                {
                                    publishtable = publishtable.OrderByDescending(x => x.Title).ToList();
                                    break;
                                }
                            default:
                                {
                                    publishtable = publishtable.OrderByDescending(x => x.Title).ToList();
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
                                    publishtable = publishtable.OrderBy(x => x.CreatedDate).ToList();
                                    break;
                                }
                            case "DESC":
                                {
                                    publishtable = publishtable.OrderByDescending(x => x.CreatedDate).ToList();
                                    break;
                                }
                            default:
                                {
                                    publishtable = publishtable.OrderByDescending(x => x.CreatedDate).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        publishtable = publishtable.OrderByDescending(x => x.Title).ToList();
                        break;
                    }
            }
            //return View(publishtable);
            return PartialView(publishtable);
        }


        [HttpPost]
        public ActionResult DashBoard(string Search, string SearchPublished)
        {
            var uv = db.SellerNotes.ToList();
            var uvpublished = db.SellerNotes.ToList();
            if (Search != null)
            {
                uv = db.SellerNotes.Where(x => x.NoteCategory.Name.Contains(Search) || x.Title.Contains(Search) || x.ReferenceData.value.Contains(Search)).ToList();
                return View(uv);
            }
            if (SearchPublished != null)
            {
                uvpublished = db.SellerNotes.Where(x => x.NoteCategory.Name.Contains(SearchPublished) || x.Title.Contains(SearchPublished) || x.ReferenceData.value.Contains(SearchPublished)).ToList();
                return View(uvpublished);
            }
            return View();

        }

        public ActionResult DeleteNote(int id, string title)
        {
            var data = db.SellerNotes.Where(query => query.ID == id).First();
            if (data != null)
            {
                var abc = db.SellerNotesAttechments.Where(query => query.NoteID == data.ID).SingleOrDefault();
                db.SellerNotes.Remove(data);
                db.SellerNotesAttechments.Remove(abc);
                db.SaveChanges();
                var list = db.SellerNotes.ToList();
                return RedirectToAction("DashBoard","Notes",new {id = data.SellerID });
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditNote(int id)
        {
            NotesMarketPLaceEntities mylist = new NotesMarketPLaceEntities();
            var category = mylist.NoteCategories.ToList();
            var type = mylist.NoteTypes.ToList();
            var country = mylist.Countries.ToList();
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
            var category = mylist.NoteCategories.ToList();
            var type = mylist.NoteTypes.ToList();
            var country = mylist.Countries.ToList();
            SelectList list = new SelectList(category, "ID", "Name");
            ViewBag.categoryname = list;
            ViewBag.typename = new SelectList(type, "ID", "Name");
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            db.Configuration.ValidateOnSaveEnabled = false;
            seller.ModifiedDate = DateTime.Now;
            db.Entry(seller).State = EntityState.Modified;
            db.SaveChanges();
            var list1 = db.SellerNotes.ToList();
            return RedirectToAction("DashBoard","Notes",new {id = seller.SellerID });
            //return View();
        }
      

        public ActionResult Download(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var seller = db.SellerNotes.Where(X => X.ID == id).SingleOrDefault();
            var sellerNote = db.SellerNotesAttechments.Where(X => X.NoteID == id).SingleOrDefault();
            if (sellerNote != null)
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
                        foreach(var item in dir.GetFiles())
                        {
                            string filepath = path + item.ToString();
                            ziparchive.CreateEntryFromFile(filepath, item.ToString());
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
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
            return View();
        }

        private void SendAllowDownloadEmail(string emailID, string firstName1, string firstName2)
        {
            //var verifyUrl = "Home/EmailVerification/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            //var Password = password;

            var fromEmail = new MailAddress("k.harshil2000@gmail.com", "Notes Market Place");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "*********"; // Replace with actual password
            string subject = firstName1 + "Allows you to download a note";

            string body = "Hello," + firstName2 + "<br/><br/>We would like to inform you that," + firstName1 +

                "Allows you to download a note.Please login and see My Download tabs to download particular note." + " <br/><br/>Regards,<br/>Notes MarketPlace";

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

        [HttpGet]
        public ActionResult PaidConformation(int id)
        {
            var data = db.SellerNotes.Where(x => x.ID == id).SingleOrDefault();
            if (data != null)
            {
                ViewBag.id = data.ID;
                ViewBag.SellingPrice = data.SellingPrice;
            }
            return View();
        }
       

        public ActionResult PaidNote(int id, decimal Sellingprice)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var seller = db.SellerNotes.Where(x => x.ID == id).SingleOrDefault();
            //var download = db.Downloads.Where(x => x.NoteID==id && x.IsSellerHasAllowedDownloads == true && x.AttechmentPath != null).SingleOrDefault();
            //if(download != null)
            //{
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
                download.Downloader = 1;
                download.IsAttechmentDownloads = false;
                db.Downloads.Add(download);
                db.SaveChanges();
                SendpaidnoteEmail(seller.User.EmailID, seller.User.FirstName,user.FirstName);
            //}
            return View("MessageModal");
        }

        private void SendpaidnoteEmail(string emailID, string firstName,string name)
        {
            //var verifyUrl = "Home/EmailVerification/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            //var Password = password;

            var fromEmail = new MailAddress("k.harshil2000@gmail.com", "Notes Market Place");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "********"; // Replace with actual password
            string subject = name + "wants to purchase your notes";

            string body = "Hello," + firstName + "<br/><br/>We would like to inform you that," + name +

                "wants to purchase your notes. Please see Buyer Requests tab and allow download access to Buyer if you have received the payment from him." + "<br/><br/>Regards,<br/>Notes MarketPlace";

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

        public ActionResult MessageModal()
        {
            return View();
        }

       
    }
}