using SizceHaber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizceHaber.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoryChart()
        {
            return Json(HeadingList(), JsonRequestBehavior.AllowGet);
        }

        public List<CategoryClass> HeadingList()
        {
            List<CategoryClass> ct = new List<CategoryClass>();
            ct.Add(new CategoryClass()
            {
                CategoryName = "Spor",
                CategoryCount = 8
            });
            ct.Add(new CategoryClass()
            {
                CategoryName = "Ekonomi",
                CategoryCount = 4
            });
            ct.Add(new CategoryClass()
            {
                CategoryName = "Bilim ve Teknoloji",
                CategoryCount = 7
            });
            ct.Add(new CategoryClass()
            {
                CategoryName = "Dünya",
                CategoryCount = 1
            });

            return ct;
        }
    }
}