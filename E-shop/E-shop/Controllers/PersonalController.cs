using E_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace E_shop.Controllers
{
    [Authorize]    
    public class PersonalController : BaseController
    {
        public ActionResult Index()
        {
            UserProfile profile = new UserProfile(User.Identity.Name);
            return View(profile);
        }

        public ActionResult Edit()
        {
            UserProfile profile = new UserProfile(User.Identity.Name);
            return View(profile);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            UserProfile profile = new UserProfile(User.Identity.Name);
            UpdateModel(profile, collection);
            profile.Save();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult MoreReg(string Phone)
        {
            UserProfile profile = new UserProfile(User.Identity.Name);
            profile.Phone = Phone;
            profile.Save();

            return RedirectToAction("Register", "Account");
        }


        
    }
}
