using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Notes_Market_Place.Models
{
    public class SendMail
    {
        public static void SendEmail(MailMessage mail)
        {
            var fromEmail = new MailAddress("*********", "Notes MarketPlace");
            var fromEmailPassword = "********"; // Replace with actual password
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            smtp.Send(mail);
        }
    }
}