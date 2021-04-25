using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Notes_Market_Place.Models;

namespace Notes_Market_Place.Controllers
{
    public class AdminController : Controller
    {
        NotesMarketPLaceEntities db = new NotesMarketPLaceEntities();
        
        [HttpGet]
        public ActionResult ManageCategory(string Sortorder, string SortBy, int PageNumber = 1)
        {
            //var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.NoteCategories.FirstOrDefault();
            if (progressnote != null)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.NoteCategories.ToList();

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
                                        uv = uv.OrderBy(x => x.Name).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.Name).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.Name).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "DESCRIPTION":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.Description).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.Description).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.Description).ToList();
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
                    case "ACTIVE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.IsActive).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.IsActive).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.IsActive).ToList();
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
            return View();
        }


        [HttpPost]
        public ActionResult ManageCategory(string Search)
        {
            var uv = db.NoteCategories.ToList();
            if (Search != null)
            {
                uv = db.NoteCategories.Where(x => x.Name.Contains(Search) || x.Description.Contains(Search)).ToList();
                return View(uv);
            }
            return View(uv);
        }

        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            var data = db.NoteCategories.Where(query => query.ID == id).FirstOrDefault();
            return View(data);
        }


        [HttpPost]
        public ActionResult EditCategory(NoteCategory category)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            category.ModifiedDate = DateTime.Now;
            category.MidifiedBy = user.ID;
            db.SaveChanges();
            return RedirectToAction("ManageCategory", "Admin");
        }


        public ActionResult DeleteCategory(int id)
        {
            var data = db.NoteCategories.Where(query => query.ID == id).First();
            if (data != null)
            {
                db.NoteCategories.Remove(data);
                db.SaveChanges();
                var list = db.NoteCategories.ToList();
                return RedirectToAction("ManageCategory", "Admin");
            }
            return View();
        }


            [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddCategory(NoteCategory category)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                category.CreatedDate = DateTime.Now;
                category.CreatedBy = user.ID;
                category.ModifiedDate = DateTime.Now;
                category.MidifiedBy = user.ID;
                category.IsActive = true;
                db.NoteCategories.Add(category);
                db.SaveChanges();
                return RedirectToAction("ManageCategory", "Admin");
            }
            else
            {
                return View();
            }
        }



        [HttpGet]
        public ActionResult ManageType(string Sortorder, string SortBy, int PageNumber = 1)
        {
            //var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.NoteTypes.FirstOrDefault();
            if (progressnote != null)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.NoteTypes.ToList();

                ViewBag.totalpages = Math.Ceiling(uv.Count() / 5.0);
                ViewBag.PageNumber = PageNumber;

                uv = uv.Skip((PageNumber - 1) * 5).Take(5).ToList();

                switch (SortBy)
                {
                    case "TYPE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.Name).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.Name).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.Name).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "DESCRIPTION":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.Description).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.Description).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.Description).ToList();
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
                    case "ACTIVE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.IsActive).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.IsActive).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.IsActive).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "ADDED BY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.IsActive).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.IsActive).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.IsActive).ToList();
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
            return View();
        }


        [HttpPost]
        public ActionResult ManageType(string Search)
        {
            var uv = db.NoteTypes.ToList();
            if (Search != null)
            {
                uv = db.NoteTypes.Where(x => x.Name.Contains(Search) || x.Description.Contains(Search)).ToList();
                return View(uv);
            }
            return View(uv);
        }


        [HttpGet]
        public ActionResult AddType()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddType(NoteType type)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                type.CreatedDate = DateTime.Now;
                type.CreatedBy = user.ID;
                type.ModifiedDate = DateTime.Now;
                type.MidifiedBy = user.ID;
                type.IsActive = true;
                db.NoteTypes.Add(type);
                db.SaveChanges();
                return RedirectToAction("ManageType", "Admin");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditType(int id)
        {
            var data = db.NoteTypes.Where(query => query.ID == id).FirstOrDefault();
            return View(data);
        }


        [HttpPost]
        public ActionResult EditType(NoteType noteType)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            noteType.ModifiedDate = DateTime.Now;
            noteType.MidifiedBy = user.ID;
            db.SaveChanges();
            return RedirectToAction("ManageType","Admin");
        }


        public ActionResult DeleteType(int id)
        {
            var data = db.NoteTypes.Where(query => query.ID == id).First();
            if (data != null)
            {
                db.NoteTypes.Remove(data);
                db.SaveChanges();
                var list = db.NoteCategories.ToList();
                return RedirectToAction("ManageType", "Admin");
            }
            return View();
        }


        [HttpGet]
        public ActionResult ManageCountry(string Sortorder, string SortBy, int PageNumber = 1)
        {
            //var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var progressnote = db.Countries.FirstOrDefault();
            if (progressnote != null)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.Countries.ToList();

                ViewBag.totalpages = Math.Ceiling(uv.Count() / 5.0);
                ViewBag.PageNumber = PageNumber;

                uv = uv.Skip((PageNumber - 1) * 5).Take(5).ToList();

                switch (SortBy)
                {
                    case "COUNTRY":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                {
                                    uv = uv.OrderBy(x => x.Name).ToList();
                                    break;
                                }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.Name).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.Name).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "COUNTRY CODE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.CountryCode).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.CountryCode).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.CountryCode).ToList();
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
                    case "ACTIVE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.IsActive).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.IsActive).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.IsActive).ToList();
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
            return View();
        }


        [HttpPost]
        public ActionResult ManageCountry(string Search)
        {
            var uv = db.Countries.ToList();
            if (Search != null)
            {
                uv = db.Countries.Where(x => x.Name.Contains(Search) || x.CountryCode.Contains(Search)).ToList();
                return View(uv);
            }
            return View(uv);
        }

        [HttpGet]
        public ActionResult EditCountry(int id)
        {
            var data = db.Countries.Where(query => query.ID == id).FirstOrDefault();
            return View(data);
        }


        [HttpPost]
        public ActionResult EditCountry(Country country)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            country.ModifiedDate = DateTime.Now;
            country.MidifiedBy = user.ID;
            db.SaveChanges();
            return RedirectToAction("ManageCountry", "Admin");
        }


        public ActionResult DeleteCountry(int id)
        {
            var data = db.Countries.Where(query => query.ID == id).First();
            if (data != null)
            {
                db.Countries.Remove(data);
                db.SaveChanges();
                var list = db.Countries.ToList();
                return RedirectToAction("ManageCountry", "Admin");
            }
            return View();
        }


        [HttpGet]
        public ActionResult AddCountry()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddCountry(Country country)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                country.CreatedDate = DateTime.Now;
                country.CreatedBy = user.ID;
                country.ModifiedDate = DateTime.Now;
                country.MidifiedBy = user.ID;
                country.IsActive = true;
                db.Countries.Add(country);
                db.SaveChanges();
                return RedirectToAction("ManageCountry", "Admin");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult ManageAdministrator(string Sortorder, string SortBy, int PageNumber = 1)
        {
            //var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            //var progressnote = db.Users.Where(x => x.RoleID == 2).Count();
            var progressnote = db.UserProfiles.Where(x => x.User.RoleID == 2).Count();
            if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                //var uv = db.Users.Where(x => x.RoleID == 2).ToList();
                var uv = db.UserProfiles.Where(x => x.User.RoleID == 2).ToList();

                ViewBag.totalpages = Math.Ceiling(uv.Count() / 5.0);
                ViewBag.PageNumber = PageNumber;

                uv = uv.Skip((PageNumber - 1) * 5).Take(5).ToList();

                switch (SortBy)
                {
                    case "FIRST NAME":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.FirstName).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.FirstName).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.FirstName).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "LAST NAME":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.LastName).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.LastName).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.LastName).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "EMAIL":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.User.EmailID).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.User.EmailID).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.User.EmailID).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    //case "PHONE NO.":
                    //    {
                    //        switch (Sortorder)
                    //        {

                    //            case "ASC":
                    //                {
                    //                    uv = uv.OrderBy(x => x.UserProfiles.).ToList();
                    //                    break;
                    //                }
                    //            case "DESC":
                    //                {
                    //                    uv = uv.OrderByDescending(x => x.LastName).ToList();
                    //                    break;
                    //                }
                    //            default:
                    //                {
                    //                    uv = uv.OrderByDescending(x => x.LastName).ToList();
                    //                    break;
                    //                }
                    //        }
                    //        break;
                    //    }
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
                    case "ACTIVE":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.User.IsActive).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.User.IsActive).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.User.IsActive).ToList();
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
                var norecord = db.UserProfiles.ToList().Where(x => x.User.RoleID == 2);
                //var norecord = db.Users.ToList().Where(x => x.RoleID == 2);
                ViewBag.Message = "No Record Found";
                return View(norecord);
            }
        }



        [HttpPost]
        public ActionResult ManageAdministrator(string Search)
        {
            var uv = db.Users.ToList();
            if (Search != null)
            {
                uv = db.Users.Where(x => x.FirstName.Contains(Search) || x.LastName.Contains(Search)).ToList();
                return View(uv);
            }
            return View(uv);
        }



        public ActionResult DeleteAdministrator(int id)
        {
            var data = db.Users.Where(query => query.ID == id).First();
            if (data != null)
            {
                db.Users.Remove(data);
                db.SaveChanges();
                var list = db.Users.Where(x => x.RoleID == 2).ToList();
                return RedirectToAction("ManageAdministrator", "Admin");
            }
            return View();
        }

        public ActionResult AddAdministrator()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddAdministrator(User user)
        {
            var user1 = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = user.ID;
                user.ModifiedDate = DateTime.Now;
                user.MidifiedBy = user.ID;
                user.IsActive = true;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("ManageAdministrator", "Admin");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult ManageSyatemConfiguration()
        {
            return View();
        }
    }
}