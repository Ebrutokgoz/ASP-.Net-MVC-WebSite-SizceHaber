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
    public class ContentController : Controller
    {
        // GET: Content
        WriterManager wm = new WriterManager(new EfWriterDal());
        ContentManager cm = new ContentManager(new EfContentDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        CategoryManager cam = new CategoryManager(new EfCategoryDal());
        Context c = new Context();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ContentReport()
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var contentValues = cm.GetList();
            return View(contentValues);
        }

        public ActionResult GetAllContent(string p, int k = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            if (string.IsNullOrEmpty(p))
            {
                p = "";
            }
            var values = cm.GetList(p).OrderByDescending(d => d.ContentDate).ToPagedList(k, 8);
            return View(values);

            //var values = from x in c.Contents select x;
            //if (!string.IsNullOrEmpty(p))
            //{
            //    values = values.Where(y => y.ContentValue.Contains(p));
            //}
            //return View(values.ToList());
        }
        public ActionResult NewContent(Content p)
        {
            string mail = (string)Session["WriterMail"];
            var writerId = c.Writers.Where(x => x.WriterMail == mail).Select(y => y.WriterID).FirstOrDefault();

            ContentValidator contentValidator = new ContentValidator();
            ValidationResult results = contentValidator.Validate(p);

            if (results.IsValid)
            {
                p.ContentDate = DateTime.Now;
                p.WriterID = writerId;
                p.ContentStatus = true;
                cm.ContentAdd(p);
                
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
        public ActionResult MyContent(string p, int k = 1)
        {
            p = (string)Session["WriterMail"];
            var writerId = c.Writers.Where(x => x.WriterMail == p).
                Select(y => y.WriterID).FirstOrDefault();
            var writer = wm.GetByID(writerId);
            ViewBag.writerName = writer.WriterName + " " + writer.WriterSurname;
            var contentValues = cm.GetListByWriter(writerId).OrderByDescending(d => d.ContentDate).ToPagedList(k, 10);
            return View(contentValues);
        }
        public ActionResult ContentByHeading(int id, int p = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var heading = hm.GetByID(id);
            if (heading.HeadingStatus)
            {
                ViewBag.headingName = heading.HeadingName;
                ViewBag.headingWriterInfo = heading.Writer.WriterName + " " + heading.Writer.WriterSurname + " - " + (((DateTime)heading.HeadingDate).ToString("dd.MM.yyyy"));
                ViewBag.headingID = heading.HeadingID;
                var contentValues = cm.GetListByHeadingID(id).OrderByDescending(d => d.ContentDate).ToPagedList(p, 8);
                return View(contentValues);
            }
            else
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult GetListByCategory(int id, int p = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var contentValues = cm.GetListByCategoryID(id).OrderByDescending(d => d.ContentDate).ToPagedList(p, 8);
            ViewBag.path = "/Content/GetListByCategory";
            return View(contentValues);
        }
        public ActionResult DeleteContent(int id)
        {
            var contentValue = cm.GetByID(id);
            contentValue.ContentStatus = false;
            cm.ContentDelete(contentValue);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public PartialViewResult CategoryListMenu()
        {
            var categoryList = cam.GetList();
            return PartialView(categoryList);
        }
        [HttpGet]
        public ActionResult EditContent(int id)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var contentValue = cm.GetByID(id);
            return PartialView(contentValue);
        }

        [HttpPost]
        public ActionResult EditContent(Content p)
        {
            cm.ContentUpdate(p);
            return RedirectToAction("MyContent");
        }
    }
}