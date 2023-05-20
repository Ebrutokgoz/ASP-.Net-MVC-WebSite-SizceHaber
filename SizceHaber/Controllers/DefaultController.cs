using BusinessLayer.Concrete;
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
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        // GET: Default
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        ContentManager cm = new ContentManager(new EfContentDal());
        CategoryManager cam = new CategoryManager(new EfCategoryDal());

        public ActionResult Headings(int p = 1)
        {
            var headingList = hm.GetList().ToPagedList(p, 20);
            return View(headingList);
        }
        public PartialViewResult Sidebar()
        {
            var headingList = hm.GetList();
            return PartialView(headingList);
        }

        public PartialViewResult HeadingList(int id = 2, int p = 1)
        {
            var headingList = cm.GetListByHeadingID(id).ToPagedList(p, 8);
            ViewBag.heading = hm.GetByID(id).HeadingName;
            ViewBag.writerName = hm.GetByID(id).Writer.WriterName;
            ViewBag.writerSurname = hm.GetByID(id).Writer.WriterSurname;
            ViewBag.date = (((DateTime)hm.GetByID(id).HeadingDate).ToString("dd.MM.yyyy"));
            return PartialView(headingList);
        }
        public PartialViewResult ContentListByCategory(int id = 2 , int p = 1)
        {
            var contentList = cm.GetListByCategoryID(id).ToPagedList(p, 8);
            return PartialView(contentList);
        }
        public ActionResult HeadingListByCategory(int id)
        {
            var headingValues = hm.GetListByCategoryID(id);
            return View(headingValues);
        }
        public PartialViewResult ContentList(int p = 1)
        {
            var contentList = cm.GetList().ToPagedList(p, 8);
            return PartialView(contentList);
        }
        public ActionResult Navbar()
        {
            var categoryList = cam.GetList();
            return View(categoryList);
        }
    }
}