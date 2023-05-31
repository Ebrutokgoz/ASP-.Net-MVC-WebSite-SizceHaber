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
    public class AdminCategoryController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
      
        

        [Authorize(Roles = "A")]
        public ActionResult Index()
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            ViewBag.writerMail = (string)Session["WriterMail"];
            var categoryValues = cm.GetList();
            return View(categoryValues);
        }

        public PartialViewResult CategoryListMenu()
        {
            var categoryList = cm.GetList();
            return PartialView(categoryList);
        }

        //[HttpGet]
        //public ActionResult AddCategory()
        //{
        //    return View();
        //}

        //[HttpPost]
        public ActionResult NewCategory(Category p)
        {
            CategoryValidator categoryValidator = new CategoryValidator();
            ValidationResult results = categoryValidator.Validate(p);

            if (results.IsValid)
            {
                p.CategoryStatus = true;
                cm.CategoryAdd(p);
               
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return RedirectToAction("Index");
        }
        public PartialViewResult AddCategory()
        {
            return PartialView();
        }

        public ActionResult DeleteCategory(int id)
        {
            var categoryValue = cm.GetByID(id);
            cm.CategoryDelete(categoryValue);
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public ActionResult EditCategory(int id)
        //{
        //    var categoryValue = cm.GetByID(id);
        //    return View(categoryValue);
        //}

        //[HttpPost]
        //public ActionResult EditCategory(Category p)
        //{
        //    cm.CategoryUpdate(p);
        //    return RedirectToAction("Index");
        //}


    }
}