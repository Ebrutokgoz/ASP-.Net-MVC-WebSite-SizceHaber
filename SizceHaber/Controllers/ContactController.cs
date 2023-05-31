using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules_FluentValidation;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizceHaber.Controllers
{
    public class ContactController : Controller
    {
        ContactManager cm = new ContactManager(new EfContactDal());
        ContactValidator cv = new ContactValidator();

        public ActionResult Index()
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var contactValues = cm.GetList().OrderByDescending(d => d.ContactDate);
            return View(contactValues);
        }

        public ActionResult GetContactDetails(int id)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var contactValues = cm.GetByID(id);
            return View(contactValues);
        }
        public PartialViewResult MessageListMenu()
        {
            return PartialView();
        }
    }
}