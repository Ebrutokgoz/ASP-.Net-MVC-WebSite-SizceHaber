using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules_FluentValidation;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizceHaber.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        // GET: Register

        readonly WriterManager wm = new WriterManager(new EfWriterDal());
        

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Writer p)
        {
            Context c = new Context();
            var writer = wm.GetByID(c.Writers.Where(x => x.WriterMail == p.WriterMail).Select(y => y.WriterID).FirstOrDefault());
            if(writer == null)
            {
                WriterValidator writerValidator = new WriterValidator();
                ValidationResult results = writerValidator.Validate(p);

                if (results.IsValid)
                {
                    p.WriterPassword = EncryptionController.CreateMD5(p.WriterPassword);
                    p.WriterStatus = true;
                    p.WriterTitle = "Writer";
                    wm.WriterAdd(p);
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    foreach (var item in results.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                    return View();
                }
                
            }
            else
            {
                TempData["Error2"] = "True";
                return RedirectToAction("Index");
            }
            
        

        }
    }
}