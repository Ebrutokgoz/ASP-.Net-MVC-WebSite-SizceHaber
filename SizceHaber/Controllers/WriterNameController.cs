using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizceHaber.Controllers
{
    public class WriterNameController : Controller
    {
        
        public  static string GetName(string mail)
        {
            WriterManager wm = new WriterManager(new EfWriterDal());
            Context c = new Context();
           
            
            var writer = wm.GetByID(c.Writers.Where(x => x.WriterMail == mail).Select(y => y.WriterID).FirstOrDefault());

           return writer.WriterName + " " + writer.WriterSurname;
        }
        
    }
}