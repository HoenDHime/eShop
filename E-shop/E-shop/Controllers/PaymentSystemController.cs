using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_shop.Models;
using E_shop.Models.Repository;
using System.Net.Mail;
using System.Web.Security;
using System.Net;

namespace E_shop.Controllers
{
    public class PaymentSystemController : BaseController
    {
        Entities entities;
        private readonly IPaymentSystemRepository paymentsystemRepository;

        public PaymentSystemController(IPaymentSystemRepository paymentsystemRepository, Entities entities)
        {
            this.paymentsystemRepository = paymentsystemRepository;
            this.entities = entities;
        }

        public ViewResult Index()
        {
            return View();
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                paymentsystemRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Successful()
        {
            return View();
        }

        private Guid PaymentS { get; set; }

        // Генерирование администратором ключ-кода для пополнение внутренней системы оплаты
        //Generating the administrator key code for the completion of the internal system of payment
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpPost]
        public ActionResult Generate(int Nominal, string UserName)
        {
            try
            {
                PaymentSystem obj = new PaymentSystem();

                Guid g;
                g = Guid.NewGuid();

                var x = entities.PaymentSystems;

                foreach (var i in x)
                {
                    if (!i.KeyCode.Equals(g))
                    {
                        obj.Nominal = Nominal;
                        obj.KeyCode = g.ToString();
                        obj.IsActive = true;
                        obj.UserName = UserName;
                        PaymentS = g;
                    }
                }

                Mail(UserName);

                if (ModelState.IsValid)
                {
                    paymentsystemRepository.Insert(obj);
                    paymentsystemRepository.Save();
                }
                return RedirectToAction("Successful");
            }
            catch (Exception) { return RedirectToAction("Index"); }
        }

        //Отсылка пользователю сгенерированного ключа
        // Sending user generated key
        [HttpPost]
        public void Mail(string UserName)
        {
            MailMessage message = new MailMessage();
            var m = Membership.GetUser(UserName);
            message.To.Add(m.Email); //почта пользователя
            message.Subject = Resources.GlobalString.KeyCodeToUserRu + " " + Resources.GlobalString.KeyCodeToUserEn;
            message.From = new MailAddress(Resources.GlobalString.userSmtp);
            message.Body = Resources.GlobalString.KeyCodeToUserRu + ":<br /><b>" + PaymentS + "</b><br />" + Resources.GlobalString.DoNotRespondRu + "<br /><br />"
                + Resources.GlobalString.KeyCodeToUserEn + ":<br /><b>" + PaymentS + "</b><br />" + Resources.GlobalString.DoNotRespondEn;
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(Resources.GlobalString.hostSmtp);
            if (!String.IsNullOrEmpty(Resources.GlobalString.userSmtp))
            {
                smtp.Credentials = new NetworkCredential(Resources.GlobalString.userSmtp, Resources.GlobalString.passSmtp);
            }
            try
            {
                smtp.Send(message);
            }
            catch (Exception)
            {
                //Email не найден
            }
            message.Dispose();
        }

        //Пополнение пользователем своего счета
        // Addition to the user of the account
        [HttpPost]
        public ActionResult Deposits(string KeyCode)
        {
            try
            {
                UserProfile profile = new UserProfile(User.Identity.Name);
                var x = entities.PaymentSystems;

                foreach (var i in x)
                {
                    if (i.KeyCode.Equals(KeyCode) && i.IsActive == true && i.UserName == User.Identity.Name)
                    {
                        profile.Money += i.Nominal;
                        i.IsActive = false;
                        profile.Save();
                    }
                    else { return RedirectToAction("Index"); }
                }

                if (ModelState.IsValid)
                {
                    entities.SaveChanges();
                }

                return RedirectToAction("Successful");
            }
            catch (Exception) { return RedirectToAction("Index"); }
        }      
    }
}

