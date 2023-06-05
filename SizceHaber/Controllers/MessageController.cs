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
    public class MessageController : Controller
    {
        // GET: Message

        MessageManager mm = new MessageManager(new EfMessageDal());
        MessageValidator messageValidator = new MessageValidator();

        public ActionResult Inbox()
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var messageList = mm.GetListInbox(mail);
            return View(messageList);
        }
       
        public ActionResult Sendbox(string p)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var messageList = mm.GetListSendbox(mail);
            return View(messageList);
        }

      
        [HttpGet]
        public ActionResult NewMessage()
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            return View();
        }

        [HttpPost]
        public ActionResult NewMessage(Message p)
        {
            string mail = (string)Session["WriterMail"];
            ValidationResult results = messageValidator.Validate(p);
            if (results.IsValid)
            {
                p.MessageStatus = true;
                p.SenderMail = mail;
                p.MessageDate = DateTime.Now;
                mm.MessageAdd(p);
                return RedirectToAction("Sendbox");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
        public ActionResult GetInboxMessageDetails(int id)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var values = mm.GetByID(id);
            return View(values);
        }
        public ActionResult GetSendboxMessageDetails(int id)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = WriterNameController.GetName(mail);
            var values = mm.GetByID(id);
            return View(values);
        }

    }
}