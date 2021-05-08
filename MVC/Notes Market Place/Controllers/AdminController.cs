using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Notes_Market_Place.Models;

namespace Notes_Market_Place.Controllers
{
    public class AdminController : Controller
    {
        NotesMarketPLaceEntities db = new NotesMarketPLaceEntities();
        

        //Manage Category
        [HttpGet]
        public ActionResult ManageCategory(string Sortorder, string SortBy, string Search, int PageNumber = 1)
        {
            var progressnote = db.NoteCategories.FirstOrDefault();
            if (progressnote != null)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.NoteCategories.ToList();
                if (Search != null)
                {
                    uv = db.NoteCategories.Where(x => x.Name.Contains(Search) || x.Description.Contains(Search) || x.ModifiedDate.ToString().Contains(Search)).ToList();
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
        public ActionResult EditCategory(int id)
        {
            var data = db.NoteCategories.Where(query => query.ID == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult EditCategory(NoteCategory category)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                category.ModifiedDate = DateTime.Now;
                category.MidifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                return RedirectToAction("ManageCategory", "Admin");
            }
            return View();
        }

        public ActionResult DeleteCategory(int id)
        {
            var data = db.NoteCategories.Where(query => query.ID == id).First();
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (data != null)
            {
                data.IsActive = false;
                data.ModifiedDate = DateTime.Now;
                data.MidifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                var list = db.NoteCategories.ToList();
                return RedirectToAction("ManageCategory", "Admin");
            }
            return View();
        }


      


        //Manage Type
        [HttpGet]
        public ActionResult ManageType(string Sortorder, string SortBy, string Search, int PageNumber = 1)
        {
            var progressnote = db.NoteTypes.FirstOrDefault();
            if (progressnote != null)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.NoteTypes.ToList();
                if (Search != null)
                {
                    uv = db.NoteTypes.Where(x => x.Name.Contains(Search) || x.Description.Contains(Search) || x.ModifiedDate.ToString().Contains(Search)).ToList();
                }

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
            if (ModelState.IsValid)
            {
                noteType.ModifiedDate = DateTime.Now;
                noteType.MidifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                return RedirectToAction("ManageType", "Admin");
            }
            return View();
        
        }

        public ActionResult DeleteType(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var data = db.NoteTypes.Where(query => query.ID == id).First();
            if (data != null)
            {
                data.IsActive = false;
                data.ModifiedDate = DateTime.Now;
                data.MidifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                var list = db.NoteCategories.ToList();
                return RedirectToAction("ManageType", "Admin");
            }
            return View();
        }




        //Manage Country
        [HttpGet]
        public ActionResult ManageCountry(string Sortorder, string SortBy, string Search, int PageNumber = 1)
        {
            var progressnote = db.Countries.FirstOrDefault();
            if (progressnote != null)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.Countries.ToList();
                if (Search != null)
                {
                    uv = db.Countries.Where(x => x.Name.Contains(Search) || x.CountryCode.Contains(Search) || x.ModifiedDate.ToString().Contains(Search)).ToList();
                }

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
        public ActionResult EditCountry(int id)
        {
            var data = db.Countries.Where(query => query.ID == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult EditCountry(Country country)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                country.ModifiedDate = DateTime.Now;
                country.MidifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                return RedirectToAction("ManageCountry", "Admin");
            }
            return View();
        }

        public ActionResult DeleteCountry(int id)
        {
            var data = db.Countries.Where(query => query.ID == id).First();
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (data != null)
            {
                data.IsActive = false;
                data.ModifiedDate = DateTime.Now;
                data.MidifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                db.SaveChanges();
                var list = db.Countries.ToList();
                return RedirectToAction("ManageCountry", "Admin");
            }
            return View();
        }


        


        //Manage Adminstrator
        [HttpGet]
        public ActionResult ManageAdministrator(string Sortorder, string SortBy, string Search, int PageNumber = 1)
        {
            var progressnote = db.UserProfiles.Where(x => x.User.RoleID == 2).Count();
            if (progressnote != 0)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.UserProfiles.Where(x => x.User.RoleID == 2).ToList();
                if (Search != null)
                {
                    uv = db.UserProfiles.Where(x => x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search)).ToList();
                }

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
                    case "PHONE NO.":
                        {
                            switch (Sortorder)
                            {

                                case "ASC":
                                    {
                                        uv = uv.OrderBy(x => x.phonenumber).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.phonenumber).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.phonenumber).ToList();
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
                ViewBag.Message = "No Record Found";
                return View(norecord);
            }
        }

        [HttpGet]
        public ActionResult AddAdministrator()
        {
            var country = db.Countries.ToList();
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
            return View();
        }

        [HttpPost]
        public ActionResult AddAdministrator(AddAdminstrator userProfile)
        {
            var country = db.Countries.ToList();
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
            var user1 = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                User user = new User();
                user.FirstName = userProfile.FirstName;
                user.LastName = userProfile.LastName;
                user.EmailID = userProfile.EmailID;
                user.Password = "Abcd#1";
                user.RoleID = 2;
                user.IsEmailVerified = true;
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = user1.ID;
                user.ModifiedDate = DateTime.Now;
                user.MidifiedBy = user1.ID;
                user.IsActive = true;
                db.Users.Add(user);

                UserProfile profile = new UserProfile();
                profile.UserID = user.ID;
                profile.FirstName = userProfile.FirstName;
                profile.LastName = userProfile.LastName;
                profile.phonenumber = userProfile.phonenumber;
                profile.Phone_number_Country_Code = userProfile.Phone_number_Country_Code;
                profile.CreatedDate = DateTime.Now;
                profile.ModifiedDate = DateTime.Now;
                profile.MidifiedBy = user1.ID;
                profile.CreatedBy = user1.ID;
                profile.AddressLine1 = "NA";
                profile.ZipCode = "NA";
                profile.Country = "NA";
                profile.City = "NA";
                profile.AddressLine2 = "NA";
                profile.State = "NA";
                db.Configuration.ValidateOnSaveEnabled = false;
                db.UserProfiles.Add(profile);
                db.SaveChanges();
                return RedirectToAction("ManageAdministrator", "Admin");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditAdministrator(int id)
        {
            var user = db.Users.Where(x => x.ID == id).FirstOrDefault();
            var country = db.Countries.ToList();
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
            var data = db.UserProfiles.Where(query => query.UserID == id).FirstOrDefault();
            data.FirstName = user.FirstName;
            data.LastName = user.LastName;
            return View(data);
        }

        [HttpPost]
        public ActionResult EditAdministrator(UserProfile userProfile)
        {
            var country = db.Countries.ToList();
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var edituser = db.Users.Where(x => x.ID == userProfile.UserID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                edituser.FirstName = userProfile.FirstName;
                edituser.LastName = userProfile.LastName;
                edituser.MidifiedBy = user.ID;
                edituser.EmailID = userProfile.SecondaryEmailAddress;
                edituser.ModifiedDate = DateTime.Now;
                userProfile.ModifiedDate = DateTime.Now;
                userProfile.MidifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                return RedirectToAction("ManageAdministrator", "Admin");
            }
            return View();
           
        }

        public ActionResult DeleteAdministrator(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var data = db.Users.Where(query => query.ID == id).First();
            if (data != null)
            {
                data.IsActive = false;
                data.MidifiedBy = user.ID;
                data.ModifiedDate = DateTime.Now;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                var list = db.Users.Where(x => x.RoleID == 2).ToList();
                return RedirectToAction("ManageAdministrator", "Admin");
            }
            return View();
        }





        //Manage Syatem Configuration
        [HttpGet]
        public ActionResult ManageSyatemConfiguration()
        {
            ManageSyatemConfiguration systemConfiguration = new ManageSyatemConfiguration();
            var supportemail = db.SystemConfigurations.Where(x => x.Key == "supportemail").FirstOrDefault();
            if (supportemail != null)
            {
                systemConfiguration.Supportemailsaddress = supportemail.Value;
            }

            var supportcontact = db.SystemConfigurations.Where(x => x.Key == "supporcontact").FirstOrDefault();
            if (supportcontact != null)
            {
                systemConfiguration.Supportphonenumber = supportcontact.Value;
            }

            var notifyemail = db.SystemConfigurations.Where(x => x.Key == "notifyemail").FirstOrDefault();
            if (notifyemail != null)
            {
                systemConfiguration.EmailAddress = notifyemail.Value;
            }

            var facebookurl = db.SystemConfigurations.Where(x => x.Key == "facebookurl").FirstOrDefault();
            if (facebookurl != null)
            {
                systemConfiguration.FacebookURL = facebookurl.Value;
            }

            var tweeterurl = db.SystemConfigurations.Where(x => x.Key == "tweeterurl").FirstOrDefault();
            if (tweeterurl != null)
            {
                systemConfiguration.TwitterURL = tweeterurl.Value;
            }

            var linkedinurl = db.SystemConfigurations.Where(x => x.Key == "linkedinurl").FirstOrDefault();
            if (linkedinurl != null)
            {
                systemConfiguration.LinkedinURL = linkedinurl.Value;
            }

            var defaultprofilepicture = db.SystemConfigurations.Where(x => x.Key == "defaultprofle").FirstOrDefault();
            if (defaultprofilepicture != null)
            {
                systemConfiguration.profilepicture = defaultprofilepicture.Value;
            }

            var defaultnote = db.SystemConfigurations.Where(x => x.Key == "defaultnote").FirstOrDefault();
            if (defaultnote != null)
            {
                systemConfiguration.displayimage = defaultnote.Value;
            }

            return View(systemConfiguration);
        }

        [HttpPost]
        public ActionResult ManageSyatemConfiguration(ManageSyatemConfiguration managesyatemConfiguration)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                SystemConfiguration systemConfiguration = new SystemConfiguration();
                var supportemail = db.SystemConfigurations.Where(x => x.Key == "supportemail").FirstOrDefault();
                if (supportemail == null)
                {
                    systemConfiguration.Key = "supportemail";
                    systemConfiguration.Value = managesyatemConfiguration.Supportemailsaddress;
                    systemConfiguration.CreatedDate = DateTime.Now;
                    systemConfiguration.CreatedBy = user.ID;
                    systemConfiguration.IsActive = true;
                    db.SystemConfigurations.Add(systemConfiguration);
                    db.SaveChanges();
                }
                else
                {
                    if (!supportemail.Value.Equals(managesyatemConfiguration.Supportemailsaddress))
                    {
                        supportemail.Value = managesyatemConfiguration.Supportemailsaddress;
                        supportemail.ModifiedDate = DateTime.Now;
                        supportemail.MidifiedBy = user.ID;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                    }
                }


                var supportcontact = db.SystemConfigurations.Where(x => x.Key == "supporcontact").FirstOrDefault();
                if (supportcontact == null)
                {
                    systemConfiguration.Key = "supporcontact";
                    systemConfiguration.Value = managesyatemConfiguration.Supportphonenumber;
                    systemConfiguration.CreatedDate = DateTime.Now;
                    systemConfiguration.CreatedBy = user.ID;
                    systemConfiguration.IsActive = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SystemConfigurations.Add(systemConfiguration);
                    db.SaveChanges();
                }
                else
                {
                    if (!supportcontact.Value.Equals(managesyatemConfiguration.Supportphonenumber))
                    {
                        supportcontact.Value = managesyatemConfiguration.Supportphonenumber;
                        supportcontact.ModifiedDate = DateTime.Now;
                        supportcontact.MidifiedBy = user.ID;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                    }
                }


                var notifyemail = db.SystemConfigurations.Where(x => x.Key == "notifyemail").FirstOrDefault();
                if (notifyemail == null)
                {
                    systemConfiguration.Key = "notifyemail";
                    systemConfiguration.Value = managesyatemConfiguration.EmailAddress;
                    systemConfiguration.CreatedDate = DateTime.Now;
                    systemConfiguration.CreatedBy = user.ID;
                    systemConfiguration.IsActive = true;
                    db.SystemConfigurations.Add(systemConfiguration);
                    db.SaveChanges();
                }
                else
                {
                    if (!notifyemail.Value.Equals(managesyatemConfiguration.EmailAddress))
                    {
                        notifyemail.Value = managesyatemConfiguration.EmailAddress;
                        notifyemail.ModifiedDate = DateTime.Now;
                        notifyemail.MidifiedBy = user.ID;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                    }
                }


                var facebookurl = db.SystemConfigurations.Where(x => x.Key == "facebookurl").FirstOrDefault();
                if (facebookurl == null)
                {
                    systemConfiguration.Key = "facebookurl";
                    systemConfiguration.Value = managesyatemConfiguration.FacebookURL;
                    systemConfiguration.CreatedDate = DateTime.Now;
                    systemConfiguration.CreatedBy = user.ID;
                    systemConfiguration.IsActive = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SystemConfigurations.Add(systemConfiguration);
                    db.SaveChanges();
                }
                else
                {
                    if (!facebookurl.Value.Equals(managesyatemConfiguration.FacebookURL))
                    {
                        facebookurl.Value = managesyatemConfiguration.FacebookURL;
                        facebookurl.ModifiedDate = DateTime.Now;
                        facebookurl.MidifiedBy = user.ID;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                    }
                }


                var tweeterurl = db.SystemConfigurations.Where(x => x.Key == "tweeterurl").FirstOrDefault();
                if (tweeterurl == null)
                {
                    systemConfiguration.Key = "tweeterurl";
                    systemConfiguration.Value = managesyatemConfiguration.TwitterURL;
                    systemConfiguration.CreatedDate = DateTime.Now;
                    systemConfiguration.CreatedBy = user.ID;
                    systemConfiguration.IsActive = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SystemConfigurations.Add(systemConfiguration);
                    db.SaveChanges();
                }
                else
                {
                    if (!tweeterurl.Value.Equals(managesyatemConfiguration.TwitterURL))
                    {
                        tweeterurl.Value = managesyatemConfiguration.TwitterURL;
                        tweeterurl.ModifiedDate = DateTime.Now;
                        tweeterurl.MidifiedBy = user.ID;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                    }
                }


                var linkedinurl = db.SystemConfigurations.Where(x => x.Key == "linkedinurl").FirstOrDefault();
                if (linkedinurl == null)
                {
                    systemConfiguration.Key = "linkedinurl";
                    systemConfiguration.Value = managesyatemConfiguration.LinkedinURL;
                    systemConfiguration.CreatedDate = DateTime.Now;
                    systemConfiguration.CreatedBy = user.ID;
                    systemConfiguration.IsActive = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SystemConfigurations.Add(systemConfiguration);
                    db.SaveChanges();
                }
                else
                {
                    if (!linkedinurl.Value.Equals(managesyatemConfiguration.LinkedinURL))
                    {
                        linkedinurl.Value = managesyatemConfiguration.LinkedinURL;
                        linkedinurl.ModifiedDate = DateTime.Now;
                        linkedinurl.MidifiedBy = user.ID;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                    }
                }


                var defaultprofle = db.SystemConfigurations.Where(x => x.Key == "defaultprofle").FirstOrDefault();
                if (defaultprofle == null)
                {
                    systemConfiguration.Key = "defaultprofle";
                    if(managesyatemConfiguration.ProfilePicture != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(managesyatemConfiguration.ProfilePicture.FileName);

                        string FileExtension = Path.GetExtension(managesyatemConfiguration.ProfilePicture.FileName);

                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                        var path = Server.MapPath("~/Default/");

                        systemConfiguration.Value = "~/Default/" + FileName;
                        Directory.CreateDirectory(path);

                        FileName = Path.Combine(path, FileName);

                        managesyatemConfiguration.ProfilePicture.SaveAs(FileName);
                    }
                    systemConfiguration.CreatedDate = DateTime.Now;
                    systemConfiguration.CreatedBy = user.ID;
                    systemConfiguration.IsActive = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SystemConfigurations.Add(systemConfiguration);
                    db.SaveChanges();
                }
                else
                {
                    if (managesyatemConfiguration.ProfilePicture != null)
                    {
                        if (defaultprofle.Value != null)
                        {
                            string previouspath = Server.MapPath(defaultprofle.Value);
                            FileInfo file = new FileInfo(previouspath);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                        }
                        string FileName = Path.GetFileNameWithoutExtension(managesyatemConfiguration.ProfilePicture.FileName);

                        string FileExtension = Path.GetExtension(managesyatemConfiguration.ProfilePicture.FileName);

                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                        var path = Server.MapPath("~/Default/");

                        defaultprofle.Value = "~/Default/" + FileName;
                        Directory.CreateDirectory(path);

                        FileName = Path.Combine(path, FileName);

                        managesyatemConfiguration.DisplayImage.SaveAs(FileName);

                        defaultprofle.ModifiedDate = DateTime.Now;
                        defaultprofle.MidifiedBy = user.ID;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                    }
                }


                var defaultnote = db.SystemConfigurations.Where(x => x.Key == "defaultnote").FirstOrDefault();
                if (defaultnote == null)
                {
                    systemConfiguration.Key = "defaultnote";
                    if (managesyatemConfiguration.DisplayImage != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(managesyatemConfiguration.DisplayImage.FileName);

                        string FileExtension = Path.GetExtension(managesyatemConfiguration.DisplayImage.FileName);

                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                        var path = Server.MapPath("~/Default/");

                        systemConfiguration.Value = "~/Default/" + FileName;
                        Directory.CreateDirectory(path);

                        FileName = Path.Combine(path, FileName);

                        managesyatemConfiguration.DisplayImage.SaveAs(FileName);
                    }
                    systemConfiguration.CreatedDate = DateTime.Now;
                    systemConfiguration.CreatedBy = user.ID;
                    systemConfiguration.IsActive = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SystemConfigurations.Add(systemConfiguration);
                    db.SaveChanges();
                }
                else
                {
                    if (managesyatemConfiguration.DisplayImage != null)
                    {
                        if (defaultnote.Value != null)
                        {
                            string previouspath = Server.MapPath(defaultnote.Value);
                            FileInfo file = new FileInfo(previouspath);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                        }
                        string FileName = Path.GetFileNameWithoutExtension(managesyatemConfiguration.DisplayImage.FileName);

                        string FileExtension = Path.GetExtension(managesyatemConfiguration.DisplayImage.FileName);

                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                        var path = Server.MapPath("~/Default/");

                        defaultnote.Value = "~/Default/" + FileName;
                        Directory.CreateDirectory(path);

                        FileName = Path.Combine(path, FileName);

                        managesyatemConfiguration.DisplayImage.SaveAs(FileName);

                        defaultnote.ModifiedDate = DateTime.Now;
                        defaultnote.MidifiedBy = user.ID;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                    }
                }
            }
            return View();
        }
    }
}