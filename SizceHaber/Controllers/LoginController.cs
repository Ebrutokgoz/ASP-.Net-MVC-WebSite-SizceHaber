using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SizceHaber.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
        WriterLoginManager wm = new WriterLoginManager(new EfWriterDal());

        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Writer p)
        {
            Context c = new Context();

            //var writerUserInfo = c.Writers.FirstOrDefault(x => x.WriterMail == p.WriterMail
            //&& x.WriterPassword == p.WriterPassword);
            string encPassword = EncryptionController.CreateMD5(p.WriterPassword);
            var writerUserInfo = wm.GetWriter(p.WriterMail, encPassword);
            
            if (writerUserInfo != null)
            {
                FormsAuthentication.SetAuthCookie(writerUserInfo.WriterMail, writerUserInfo.RememberMe);
                Session["WriterMail"] = writerUserInfo.WriterMail;
                if (writerUserInfo.WriterTitle == "Writer")
                {
                    
                    return RedirectToAction("AllContents", "WriterPanelContent");
                }
                else
                {
                    
                    return RedirectToAction("Index", "Heading");
                }
                
            }
            else 
            {

                TempData["Error"] = "True";
                return RedirectToAction("Index");
            }


        }

        //[HttpGet]
        //public ActionResult AdminLogin()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult AdminLogin(Admin p)
        //{
        //    Context c = new Context();
        //    var adminUserInfo = c.Admins.FirstOrDefault(x=> x.AdminUsername == p.AdminUsername
        //    && x.AdminPassword == p.AdminPassword);
        //    if(adminUserInfo != null)
        //    {
        //        FormsAuthentication.SetAuthCookie(adminUserInfo.AdminUsername, false);
        //        Session["AdminUsername"] = adminUserInfo.AdminUsername;
        //        return RedirectToAction("Index", "AdminCategory");
        //    }
        //    else
        //    {
        //        TempData["Error"] = "True";
        //        return RedirectToAction("AdminLogin");
                
        //    }
        //}
      


        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("HomePage", "Home");
        }

    }
}