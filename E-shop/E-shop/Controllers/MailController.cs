using E_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.Security;

namespace E_shop.Controllers
{
    public class MailController : BaseController
    {
        MailMessage message = new MailMessage();

        SmtpClient smtp;

        public MailController(SmtpClient smtp)
        {
            this.smtp = smtp;
        }

        public ActionResult Index() 
        {
            return View();
        }

        // Отправка письма администратором, выбранному пользователю
        // Sending a letter to the manager, the selected user
        [HttpPost]
        public ActionResult Mail(string to, string subject, string body)
        {
            //enter email
            //message.To.Add(to);

            //enter username - nick
            message.To.Add(Membership.GetUser(to).Email);
            message.Subject = subject;
            message.From = new MailAddress(Resources.GlobalString.userSmtp);
            message.Body = body;
            message.IsBodyHtml = true;

            smtp.Host = Resources.GlobalString.hostSmtp;

            if (!String.IsNullOrEmpty(Resources.GlobalString.userSmtp))
            {
                smtp.Credentials = new System.Net.NetworkCredential(Resources.GlobalString.userSmtp, Resources.GlobalString.passSmtp);
            }
            try
            {
                smtp.Send(message);
            }
            catch(Exception)
            {
                //Email не найден
                //Email don't found
                return RedirectToAction("ErrorPage", "Home");            
            }
            message.Dispose();

            return RedirectToAction("MailWasSent");
        }

        //Отсылание письма администратору - пользователем
        //Letter send to the administrator - the user
        [HttpPost]
        public object MailToAdmin(string subject, string body)
        {
            try
            {
                UserProfile profile = new UserProfile(User.Identity.Name);

                message.To.Add(Resources.GlobalString.userSmtp);

                message.Subject = subject;
                message.From = new MailAddress(Resources.GlobalString.userSmtp);
                message.Body = body + "<br /><b>" + User.Identity.Name + "</b><br />" + profile.LastName + "   " + profile.FirstName + "   " + profile.Money + "<br />" + profile.Address + "   " + profile.Phone;
                message.IsBodyHtml = true;

                smtp.Host = Resources.GlobalString.hostSmtp;

                if (!String.IsNullOrEmpty(Resources.GlobalString.userSmtp))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(Resources.GlobalString.userSmtp, Resources.GlobalString.passSmtp);
                }
                smtp.Send(message);

                message.Dispose();

                return RedirectToAction("MailWasSent");
            }
            catch (Exception) { return RedirectToAction("Index"); }
        }

        public ViewResult MailWasSent()
        {
            return View();
        }
    }
}
