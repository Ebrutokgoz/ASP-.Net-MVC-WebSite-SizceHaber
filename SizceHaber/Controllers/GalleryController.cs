using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizceHaber.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery

        ImageFileManager ifm = new ImageFileManager(new EfImageFileDal());
      
        public ActionResult Index(int k = 1)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var files = ifm.GetList().ToPagedList(k, 30);
            return View(files);
        }
    }
}