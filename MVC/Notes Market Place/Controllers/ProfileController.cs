using Notes_Market_Place.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;


namespace Notes_Market_Place.Controllers
{
    public class ProfileController : Controller
    {
        NotesMarketPLaceEntities db = new NotesMarketPLaceEntities();
        // GET: Profile
        [HttpGet]
        public ActionResult UserProfile()
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                var country = db.Countries.Where(x => x.IsActive == true).ToList();
                var gender = db.ReferenceDatas.Where(x => x.RefCategory == "Gender").ToList();
                ViewBag.countryname = new SelectList(country, "ID", "Name");
                ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
                ViewBag.gendrename = new SelectList(gender, "ID", "value");
                UserProfile use = new UserProfile
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                return View(use);
            }
            return View();
        }

        [HttpPost]
        public ActionResult UserProfile(UserProfile userProfile)
        {
            var country = db.Countries.Where(x => x.IsActive == true).ToList();
            var gender = db.ReferenceDatas.Where(x => x.RefCategory == "Gender").ToList();
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
            ViewBag.gendrename = new SelectList(gender, "ID", "value");
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                userProfile.CreatedDate = DateTime.Now;
                userProfile.ModifiedDate = DateTime.Now;
                userProfile.MidifiedBy = user.ID;
                userProfile.CreatedBy = user.ID;
                userProfile.UserID = user.ID;
                if (userProfile.profilepicutre != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(userProfile.profilepicutre.FileName);

                    string FileExtension = Path.GetExtension(userProfile.profilepicutre.FileName);

                    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                    var path = Server.MapPath("~/Members/" + user.ID + "/ProfileImage/");

                    userProfile.ProfilePicture = "~/Members/" + user.ID + "/ProfileImage/" + FileName;
                    Directory.CreateDirectory(path);

                    FileName = Path.Combine(path, FileName);

                    userProfile.profilepicutre.SaveAs(FileName);
                }
                db.UserProfiles.Add(userProfile);
                db.SaveChanges();
                Session["Image"] = userProfile.ProfilePicture;
            }
            return RedirectToAction("Search", "Home");
        }


        [HttpGet]
        public ActionResult UserProfile1()
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var userprofile = db.UserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();
            if (user != null && userprofile != null)
            {
                var country = db.Countries.Where(x => x.IsActive == true).ToList();
                var gender = db.ReferenceDatas.Where(x => x.RefCategory == "Gender").ToList();
                ViewBag.countryname = new SelectList(country, "ID", "Name");
                ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
                ViewBag.gendrename = new SelectList(gender, "ID", "value");
               
                userprofile.FirstName = user.FirstName;
                userprofile.LastName = user.LastName;
            
                return View(userprofile);
            }
            return View();
        }

        [HttpPost]
        public ActionResult UserProfile1(UserProfile userProfile)
        {
            var country = db.Countries.Where(x => x.IsActive == true).ToList();
            var gender = db.ReferenceDatas.Where(x => x.RefCategory == "Gender").ToList();
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
            ViewBag.gendrename = new SelectList(gender, "ID", "value");
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                var previoususer = db.UserProfiles.Where(x => x.ID == userProfile.ID).FirstOrDefault();
                if (previoususer.ProfilePicture != null)
                {
                    string previouspath = Server.MapPath(previoususer.ProfilePicture);
                    FileInfo file = new FileInfo(previouspath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                if (userProfile.profilepicutre != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(userProfile.profilepicutre.FileName);

                    string FileExtension = Path.GetExtension(userProfile.profilepicutre.FileName);

                    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                    var path = Server.MapPath("~/Members/" + user.ID + "/ProfileImage/");

                    userProfile.ProfilePicture = "~/Members/" + user.ID + "/ProfileImage/" + FileName;
                    Directory.CreateDirectory(path);

                    FileName = Path.Combine(path, FileName);

                    userProfile.profilepicutre.SaveAs(FileName);
                }
                userProfile.ModifiedDate = DateTime.Now;
                userProfile.MidifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                //db.Entry(userProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Search", "Home");
            }
            return View();
        }



        //Members Page
        [HttpGet]
        public ActionResult MembersPage(string Sortorder, string SortBy, string Search, int PageNumber = 1)
        {
            ViewBag.member = "active";
            var progressnote = db.Users.Where(x => x.RoleID == 1 && x.IsEmailVerified == true && x.IsActive == true).FirstOrDefault();
            if (progressnote != null)
            {
                ViewBag.Sortorder = Sortorder;
                ViewBag.SortBy = SortBy;

                var uv = db.Users.Where(x => x.RoleID == 1 && x.IsEmailVerified == true && x.IsActive == true).ToList();
                if (!String.IsNullOrEmpty(Search))
                {
                    uv = db.Users.Where(x => x.FirstName.Contains(Search) || x.LastName.Contains(Search) || x.EmailID.Contains(Search) || x.CreatedDate.ToString().Contains(Search)).ToList();
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
                                        uv = uv.OrderBy(x => x.EmailID).ToList();
                                        break;
                                    }
                                case "DESC":
                                    {
                                        uv = uv.OrderByDescending(x => x.EmailID).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        uv = uv.OrderByDescending(x => x.EmailID).ToList();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "JOININD DATE":
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
                var list = db.Users.Where(x => x.RoleID == 1 && x.IsActive == true).ToList();
                ViewBag.Message = "No Record Found";
                return View(list);
            }
        }



        //My Profile
        [HttpGet]
        public ActionResult MyProfile()
        {
            var country = db.Countries.Where(x => x.IsActive == true).ToList();
            ViewBag.countryname = new SelectList(country, "ID", "Name");
            ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var userprofile = db.UserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();
            if (userprofile != null)
            {
                myprofile use = new myprofile
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID,
                    ID = user.ID,
                    SecondaryEmailAddress = userprofile.SecondaryEmailAddress,
                    Phone_number_Country_Code = userprofile.Phone_number_Country_Code,
                    phonenumber = userprofile.phonenumber,
                    ProfilePicture = userprofile.ProfilePicture,
                    profilepicutre = userprofile.profilepicutre
                };
                return View(use);
            }
            else
            {
                myprofile use = new myprofile
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID,
                    ID = user.ID,
                };
                return View(use);
            }
        }


        [HttpPost]
        public ActionResult MyProfile(myprofile myprofile)
        {
            var country = db.Countries.Where(x => x.IsActive == true).ToList();
            ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var userProfile = db.UserProfiles.Where(x => x.UserID == myprofile.ID).FirstOrDefault();
                if(userProfile != null) 
                {
                    userProfile.ModifiedDate = DateTime.Now;
                    userProfile.MidifiedBy = user.ID;
                    userProfile.LastName = myprofile.LastName;
                    userProfile.FirstName = myprofile.FirstName;
                    userProfile.Phone_number_Country_Code = myprofile.Phone_number_Country_Code;
                    userProfile.phonenumber = myprofile.phonenumber;
                    userProfile.SecondaryEmailAddress = myprofile.SecondaryEmailAddress;
                    if (userProfile.ProfilePicture != null)
                    {
                        string previouspath = Server.MapPath(userProfile.ProfilePicture);
                        FileInfo file = new FileInfo(previouspath);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    if (myprofile.profilepicutre != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(myprofile.profilepicutre.FileName);

                        string FileExtension = Path.GetExtension(myprofile.profilepicutre.FileName);

                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                        var path = Server.MapPath("~/Members/" + user.ID + "/ProfileImage/");

                        userProfile.ProfilePicture = "~/Members/" + user.ID + "/ProfileImage/" + FileName;
                        Directory.CreateDirectory(path);

                        FileName = Path.Combine(path, FileName);

                        myprofile.profilepicutre.SaveAs(FileName);
                    }
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                } 
                Session["Image"] = userProfile.ProfilePicture;

                return RedirectToAction("AdminDashBoard","AdminDashBoard");
            }
            return View();
        }


        public ActionResult Deactive(int id)
        {
            var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
            var userdeactivate = db.Users.Where(x => x.ID == id).FirstOrDefault();
            userdeactivate.IsActive = false;
            userdeactivate.MidifiedBy = user.ID;
            userdeactivate.ModifiedDate = DateTime.Now;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("MembersPage", "Profile");
        }
    }
}