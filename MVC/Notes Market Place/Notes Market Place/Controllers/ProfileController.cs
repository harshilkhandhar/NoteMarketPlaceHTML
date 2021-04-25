using Notes_Market_Place.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                var country = db.Countries.ToList();
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
            var country = db.Countries.ToList();
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
                var country = db.Countries.ToList();
                var gender = db.ReferenceDatas.Where(x => x.RefCategory == "Gender").ToList();
                ViewBag.countryname = new SelectList(country, "ID", "Name");
                ViewBag.countrycode = new SelectList(country, "ID", "CountryCode");
                ViewBag.gendrename = new SelectList(gender, "ID", "value");
                //UserProfile use = new UserProfile
                //{
                userprofile.FirstName = user.FirstName;
                userprofile.LastName = user.LastName;
                //};
                return View(userprofile);
            }
            return View();
        }

        [HttpPost]
        public ActionResult UserProfile1(UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                userProfile.ModifiedDate = DateTime.Now;
                userProfile.MidifiedBy = user.ID;
                db.SaveChanges();
                return RedirectToAction("Search", "Home");
            }
            return View();
        }
    }
}