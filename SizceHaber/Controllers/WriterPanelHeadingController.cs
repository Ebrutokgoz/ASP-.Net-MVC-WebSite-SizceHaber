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

namespace SizceHaber.Controllers
{
    public class WriterPanelHeadingController : Controller
    {
        // GET: WriterPanelHeading
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        Context c = new Context();
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult CategoryListMenu()
        {
            var categoryList = cm.GetList();
            return PartialView(categoryList);
        }

        public ActionResult MyHeading(string p, int k = 1)
        {
            p = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(p))
            {
                return RedirectToAction("Index", "Login");
            }
            var writerId = c.Writers.Where(x => x.WriterMail == p).Select(y => y.WriterID).FirstOrDefault();
            var values = hm.GetListByWriter(writerId).OrderByDescending(d => d.HeadingDate).ToPagedList(k, 8);
            var writerValue = wm.GetByID(writerId);
            ViewBag.writerName = writerValue.WriterName + " " + writerValue.WriterSurname;
            return View(values);
        }
        public ActionResult NewHeading(Heading p)
        {
            string mail = (string)Session["WriterMail"];
            var writerId = c.Writers.Where(x => x.WriterMail == mail).Select(y => y.WriterID).FirstOrDefault();
            p.HeadingDate = (DateTime.Now);
            p.WriterID = writerId;
            p.HeadingStatus = true;
            hm.HeadingAdd(p);
            return RedirectToAction("MyHeading");
        }
        public PartialViewResult AddHeading()
        {
            List<SelectListItem> valueCategory = (from x in cm.GetList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryID.ToString()
                                                  }).ToList();
            ViewBag.vlc = valueCategory;
            return PartialView();
        }
        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
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
            hm.HeadingUpdate(p);
            return RedirectToAction("MyHeading");
        }
        public ActionResult DeleteHeading(int id)
        {
            var headingValue = hm.GetByID(id);
            headingValue.HeadingStatus = false;
            hm.HeadingDelete(headingValue);
            return RedirectToAction("MyHeading");
        }
        public ActionResult AllHeadings(int p = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var headings = hm.GetList().OrderByDescending(d => d.HeadingDate).ToPagedList(p, 10);

            return View(headings);
        }

        public ActionResult GetListByCategory(int id, int p = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var headingsValues = hm.GetListByCategoryID(id).OrderByDescending(d => d.HeadingDate).ToPagedList(p, 8);
            return View(headingsValues);
        }
    }
}