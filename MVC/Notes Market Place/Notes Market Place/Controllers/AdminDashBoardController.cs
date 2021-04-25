using Notes_Market_Place.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Notes_Market_Place.Controllers
{
    public class AdminDashBoardController : Controller
    {
        NotesMarketPLaceEntities db = new NotesMarketPLaceEntities();
        // GET: AdminDashBoard
        [HttpGet]
        public ActionResult AdminDashBoard(string Sortorder, string SortBy, int PageNumber = 1)
        {
            var progressnote = db.SellerNotes.Where(x => x.Status == 9).Count();
            if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.SellerNotes.Where(x => x.Status == 9).ToList();

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


        [HttpPost]
        public ActionResult AdminDashBoard(string Search)
        {
            var uv = db.SellerNotes.ToList();
            if (Search != null)
            {
                uv = db.SellerNotes.Where(x => x.Title.Contains(Search) || x.NoteCategory.Name.Contains(Search) || x.SellingPrice.ToString().Contains(Search)).ToList();
                return View(uv);
            }
            return View();
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
        public ActionResult NotesUnderReview(string Sortorder, string SortBy, int PageNumber = 1)
        {
            var progressnote = db.SellerNotes.Where(x => x.Status == 7 || x.Status == 8).Count();
            if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.SellerNotes.Where(x => x.Status == 7 || x.Status == 8).ToList();

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



        [HttpPost]
        public ActionResult NotesUnderReview(string Search)
        {
            var uv = db.SellerNotes.ToList();
            if (Search != null)
            {
                uv = db.SellerNotes.Where(x => x.Title.Contains(Search) || x.NoteCategory.Name.Contains(Search) || x.SellingPrice.ToString().Contains(Search)).ToList();
                return View(uv);
            }
            return View();
        }




        [HttpGet]
        public ActionResult PublishedNotes(string Sortorder, string SortBy, int PageNumber = 1)
        {
            var progressnote = db.SellerNotes.Where(x => x.Status == 9).Count();
            if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.SellerNotes.Where(x => x.Status == 9).ToList();

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
    }
}