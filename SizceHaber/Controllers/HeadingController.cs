using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizceHaber.Controllers
{
    public class HeadingController : Controller
    {
        // GET: Heading

        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        Context c = new Context();
        
        public ActionResult Index(string searchedString, int k = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);

            //if (string.IsNullOrEmpty(searchedString))
            //{
            //    searchedString = "";
            //}
            var headingValues = hm.GetList(searchedString).ToPagedList(k, 8);
            return View(headingValues);
        }

        public ActionResult HeadingReport()
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var headingValues = hm.GetList();
            return View(headingValues);
        }
        public ActionResult MyHeadings(string p, int k = 1)
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
            var contentValues = hm.GetListByWriter(writerId).ToPagedList(k, 10);
            return View(contentValues);
        }

        [HttpGet]
        public PartialViewResult AddHeading()
        {
            List<SelectListItem> valueCategory = (from x in cm.GetList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryID.ToString()
                                                  }).ToList();
            List<SelectListItem> valueWriter = (from x in wm.GetList()
                                                select new SelectListItem
                                                {
                                                    Text = x.WriterName + " " + x.WriterSurname,
                                                    Value = x.WriterID.ToString()
                                                }).ToList();
            ViewBag.vlc = valueCategory;
            ViewBag.vlw = valueWriter;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddHeading(Heading p)
        {
            p.HeadingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            hm.HeadingAdd(p);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            List<SelectListItem> valueCategory = (from x in cm.GetList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryID.ToString()
                                                  }).ToList();
            ViewBag.vlc = valueCategory;
            var headingValue = hm.GetByID(id);
            return View(headingValue);
        }

        [HttpPost]
        public ActionResult EditHeading(Heading p)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            hm.HeadingUpdate(p);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteHeading(int id)
        {
            var headingValue = hm.GetByID(id);
            headingValue.HeadingStatus = false;
            hm.HeadingDelete(headingValue);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult GetListByCategory(int id, int k = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var headingValues = hm.GetListByCategoryID(id).ToPagedList(k, 8);
            return View(headingValues);
        }
        public ActionResult GetListByWriter(int id, int k = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var contentValues = hm.GetListByWriter(id).ToPagedList(k, 10);
            return View(contentValues);
        }

        public PartialViewResult CategoryListMenu()
        {
            var categoryList = cm.GetList();
            return PartialView(categoryList);
        }
    }
}