using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using FluentValidation.Results;
using BusinessLayer.ValidationRules_FluentValidation;
using System.IO;

namespace SizceHaber.Controllers
{
    public class WriterPanelController : Controller
    {
        // GET: WriterPanel

       
        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator writerValidator = new WriterValidator();
        
        Context c = new Context();

        [HttpGet]
        public ActionResult WriterProfile(int id = 0)
        {
            string p = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(p))
            {
                return RedirectToAction("Index", "Login");
            }
            id = c.Writers.Where(x => x.WriterMail == p).Select(y => y.WriterID).FirstOrDefault();
            var writerValue = wm.GetByID(id);
            
            ViewBag.writerName = writerValue.WriterName + " " + writerValue.WriterSurname;
            return View(writerValue);
        }
        

        [HttpPost]
        public ActionResult WriterProfile(Writer p)
        {
            ValidationResult results = writerValidator.Validate(p);
            if (results.IsValid)
            {
                p.WriterPassword = EncryptionController.CreateMD5(p.WriterPassword);
                wm.WriterUpdate(p);
                return RedirectToAction("WriterProfile");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }

       
    }
}