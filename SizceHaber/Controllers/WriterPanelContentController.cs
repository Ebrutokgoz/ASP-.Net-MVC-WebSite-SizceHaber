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
using System.Web.Mvc;

namespace SizceHaber.Controllers
{
    public class WriterPanelContentController : Controller
    {
        // GET: WriterPanelContent

        ContentManager cm = new ContentManager(new EfContentDal());
        CategoryManager cam = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        Context c = new Context();

        public PartialViewResult CategoryListMenu()
        {
            var categoryList = cam.GetList();
            return PartialView(categoryList);
        }

        [Route("paylasimlarim")]
        public ActionResult MyContent(string p, int k = 1)
        { 
            p = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(p))
            {
                return RedirectToAction("Index", "Login");
            }
            var writerId = c.Writers.Where(x => x.WriterMail == p).
                Select(y => y.WriterID).FirstOrDefault();
            var writer = wm.GetByID(writerId);
            ViewBag.writerName = writer.WriterName + " " + writer.WriterSurname;
            var contentValues = cm.GetListByWriter(writerId).OrderByDescending(d => d.ContentDate).ToPagedList(k, 20);
            return View(contentValues);
        }

        public ActionResult AllContents(int p = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var contentValues = cm.GetList().OrderByDescending(d => d.ContentDate).ToPagedList(p, 10);
            return View(contentValues);
        }

        public ActionResult ContentByHeading(int id, int k = 1 )
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var heading = hm.GetByID(id);
            if(heading.HeadingStatus)
            {
                ViewBag.headingID = heading.HeadingID;
                ViewBag.headingName = heading.HeadingName;

                ViewBag.headingWriterInfo = heading.Writer.WriterName + " " + heading.Writer.WriterSurname + " - " + (((DateTime)heading.HeadingDate).ToString("dd.MM.yyyy"));

                var contentValues = cm.GetListByHeadingID(id).OrderByDescending(d => d.ContentDate).ToPagedList(k, 8);
                return View(contentValues);
            }
            else
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            
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

        public PartialViewResult AddContent()
        {
            return PartialView();
        }

        //[HttpGet]
        //public ActionResult AddContent(int id)
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult AddContent(Content p)
        //{
        //    p.ContentDate = DateTime.Parse(DateTime.Now.ToShortDateString());
        //    string mail = (string)Session["WriterMail"];
        //    var writerId = c.Writers.Where(x => x.WriterMail == mail).
        //        Select(y => y.WriterID).FirstOrDefault();
        //    p.WriterID = writerId;
        //    p.ContentStatus = true;
        //    cm.ContentAdd(p);
        //    return RedirectToAction("MyContent");
        //}
        public ActionResult GetListByCategory(int id, int p = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var contentValues = cm.GetListByCategoryID(id).OrderByDescending(d => d.ContentDate).ToPagedList(p, 8);
            return View(contentValues);
        }
      

        
        [HttpGet]
        [Route("duzenle/{id}/{baslik}")]
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
        [Route("duzenle/{id}/{baslik}")]
        public ActionResult EditContent(Content p)
        {
            cm.ContentUpdate(p);
            return RedirectToAction("MyContent");
        }

        [Route("sil/{id}/{baslik}")]
        public ActionResult DeleteContent(int id)
        {
            var contentValue = cm.GetByID(id);
            contentValue.ContentStatus = false;
            cm.ContentDelete(contentValue);
            return RedirectToAction("MyContent");
        }

    }
}