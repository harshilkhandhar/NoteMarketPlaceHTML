using Notes_Market_Place.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Notes_Market_Place.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Loginvalidation obj)
        {
            if (ModelState.IsValid)
            {
                using (var databaseContext = new NotesMarketPLaceEntities())
                {
                    User userModal = databaseContext.Users.Where(query => query.EmailID == obj.EmailID && query.Password == obj.Password).SingleOrDefault();
                    if (userModal != null)
                    {
                        Session["ID"] = userModal.ID;
                        if (userModal.RoleID == 1 && userModal.IsEmailVerified==true)
                        {
                            FormsAuthentication.SetAuthCookie(obj.EmailID, false);
                            return RedirectToAction("Search", "Home");
                        }
                        else if (userModal.RoleID==2)
                        {
                            return RedirectToAction("AdminDashBoard", "Home");
                        }
                        else
                        {
                            ViewBag.Message = "Please Verify Your Email First.";
                            return View();
                        }  
                    }
                    else
                    {
                        ViewBag.Message = "Please Enter the Correct Email and Password .";
                        return View();
                    }
                } 
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult Forgotpass()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forgotpass(string EmailID)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                using (var databaseContext = new NotesMarketPLaceEntities())
                { 
                    var EmailExist = databaseContext.Users.Where(query => query.EmailID == EmailID).FirstOrDefault();
                    if (EmailExist != null)
                    {
                        databaseContext.Configuration.ValidateOnSaveEnabled = false;
                        EmailExist.Password = "12345678";
                        EmailExist.ModifiedDate = DateTime.Now;
                        databaseContext.SaveChanges();
                        SendForgotpassLinkEmail(EmailID,EmailExist.Password);
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ViewBag.message = "This Email Is Not Exist. Try Another Email";
                        return View("Forgotpass");
                    }
                }
            }
            else
            {
                return View();
            }

        }

        private void SendForgotpassLinkEmail(string emailID,string password)
        { 
            var fromEmail = new MailAddress("k.harshil2000@gmail.com","Notes Market Place");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "********"; // Replace with actual password
            string subject = "New Temporary Password has been created for you";

            string body = "Hello" + "<br/><br/>We have generated a new passsword for you" +

                "<br/><br/>Password:" + password + "<br/><br/>Regards,<br/>Notes MarketPlace";

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
        public ActionResult Signup()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(User user)
        {
           
            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ViewBag.message = "This Email Is Already Exist. Try Another Email";
                    return View("Signup");
                }
                user.CreatedDate = DateTime.Now;
                user.RoleID = 1;
                user.IsActive = true;
                user.ActivationCode = Guid.NewGuid();
                using (var databaseContext = new NotesMarketPLaceEntities())
                {
                    databaseContext.Users.Add(user);
                        databaseContext.SaveChanges();
                        SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString(),user.FirstName);
                }
                ViewBag.Message1 = "Your account has been successfilly created.Plase Verify your Email.";
                return View("Signup");
            }
            else
            {
                return View("Signup");
            }
        }


        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            NotesMarketPLaceEntities databasecontext = new NotesMarketPLaceEntities();
            var EmailVerified = databasecontext.Users.Where(c => c.ActivationCode == new Guid(id)).FirstOrDefault();
            if (EmailVerified != null)
            {
                databasecontext.Configuration.ValidateOnSaveEnabled = false;
                EmailVerified.IsEmailVerified = true;
                databasecontext.SaveChanges();
            }
            return RedirectToAction("EmailVerification", "Home");
        }


        [NonAction]
        private void SendVerificationLinkEmail(string emailID, string activationCode, string Name)
        {
            var verifyUrl = "/Login/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var name = Name;

            var fromEmail = new MailAddress("k.harshil2000@gmail.com","Notes MarketPlace");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "********"; // Replace with actual password
            string subject = "Notes MarketPlace - Email Verification";

            string body = "Hello, "+name+"<br/><br/>Thank you for signing up with us.Please click below link to verify your email address and to do login." +
                
                " <br/><br/><a href='" + link + "'>" + link + "</a> " + "<br/><br/>Regards,<br/>Notes MarketPlace";

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


        private bool IsEmailExist(string emailID)
        {
            using ( var databaseContext = new NotesMarketPLaceEntities())
            {
                var v = databaseContext.Users.Where(query => query.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }


        [HttpGet]
        public ActionResult Changepass()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Changepass(Changepassword changepassword)
        {
            NotesMarketPLaceEntities databaseContext = new NotesMarketPLaceEntities();
            var user = databaseContext.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
           
                if (user != null && user.Password == changepassword.Password)
                {
                    databaseContext.Configuration.ValidateOnSaveEnabled = false;
                    user.Password = changepassword.NewPassword;
                    user.ModifiedDate = DateTime.Now;
                    ViewBag.Message = "Your Password Change Sessuful.";
                    databaseContext.SaveChanges();
                    return RedirectToAction("Index", "Login"); 
                }
                else
                {
                    ViewBag.Error = "Please Enter the Correct Old Password";
                    return View();
                }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //Session.RemoveAll();
            //Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }

}