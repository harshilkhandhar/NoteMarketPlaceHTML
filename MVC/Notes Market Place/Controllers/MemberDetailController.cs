using Notes_Market_Place.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Notes_Market_Place.Controllers
{
    public class MemberDetailController : Controller
    {
        NotesMarketPLaceEntities db = new NotesMarketPLaceEntities();
        // GET: MemberDetail
        [HttpGet]
        public ActionResult MemberDetails(int id, string Sortorder, string SortBy, int PageNumber = 1)
        {
            var Detail = db.UserProfiles.Where(x => x.UserID == id).FirstOrDefault();
            ViewBag.firstname = Detail.User.FirstName;
            ViewBag.lastname = Detail.User.LastName;
            ViewBag.email = Detail.User.EmailID;
            ViewBag.collage = Detail.Collage;
            ViewBag.phonenumber = Detail.phonenumber;
            ViewBag.profilepicture = Detail.ProfilePicture;
            ViewBag.dateofbirth = Detail.DOB.Value.ToString("dd-MM-yyyy");
            ViewBag.zipcode = Detail.ZipCode;
            ViewBag.Address1 = Detail.AddressLine1;
            ViewBag.Address2 = Detail.AddressLine2;
            ViewBag.city = Detail.City;
            var country = db.Countries.Where(x => x.ID.ToString() == Detail.Country).FirstOrDefault();
            ViewBag.country = country.Name;
            ViewBag.state = Detail.State;
            ViewBag.id = id;
            
            var progressnote = db.SellerNotes.Where(x => x.SellerID == id && x.Status != 6).Count();
            if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.SellerNotes.Where(x => x.SellerID == id && x.Status != 6).ToList();

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
                var list = db.SellerNotes.Where(x => x.SellerID == id && x.Status != 6).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }
    }
}