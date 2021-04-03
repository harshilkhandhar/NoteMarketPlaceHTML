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
        // GET: Profile
        [HttpGet]
        public ActionResult UserProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserProfile(UserProfile userProfile)
        {

            return View();
        }

    }
}