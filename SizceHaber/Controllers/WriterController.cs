using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules_FluentValidation;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizceHaber.Controllers
{
    public class WriterController : Controller
    {
        string password = "";
        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator writerValidator = new WriterValidator();
        [Authorize(Roles = "A")]
        public ActionResult Index(int k = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var writerValues = wm.GetList().OrderBy(d => d.WriterName).ToPagedList(k, 20);
            return View(writerValues);
        }
        public ActionResult WriterReport()
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var writerValues = wm.GetList();
            return View(writerValues);
        }

        [Authorize(Roles = "A")]
        [HttpGet]
        public PartialViewResult AddWriter()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddWriter(Writer p)
        {
            
            ValidationResult results = writerValidator.Validate(p);
            if (results.IsValid)
            {
                p.WriterStatus = true;
                p.WriterPassword = EncryptionController.CreateMD5(p.WriterPassword);
                wm.WriterAdd(p);
               
            }
            else
            {
                foreach(var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A")]
        [HttpGet]
        public ActionResult EditWriter(int id)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var writerValue = wm.GetByID(id);
            password = writerValue.WriterPassword;
            return View(writerValue);
        }

        [HttpPost]
        public ActionResult EditWriter(Writer p)
        {
            
            ValidationResult results = writerValidator.Validate(p);
            if (results.IsValid)
            {
                p.WriterPassword = EncryptionController.CreateMD5(p.WriterPassword);
                wm.WriterUpdate(p);
                
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        [Authorize(Roles = "A")]
        public ActionResult DeleteWriter(int id)
        {
            var writerValue = wm.GetByID(id);
            wm.WriterDelete(writerValue);
            return RedirectToAction("Index");
        }
    }
}