﻿using BusinessLayer.Concrete;
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
        WriterManager wm = new WriterManager(new EfWriterDal());
        Context c = new Context();
        public string GetName(String mail)
        {
            var writer = wm.GetByID(c.Writers.Where(x => x.WriterMail == mail).Select(y => y.WriterID).FirstOrDefault());
            return writer.WriterName + " " + writer.WriterSurname;
        }

        public ActionResult Inbox(string p)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = GetName(mail);
            var messageList = mm.GetListInbox(p);
            return View(messageList);
        }
       
        public ActionResult Sendbox(string p)
        {
            string mail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.writerName = GetName(mail);
            var messageList = mm.GetListSendbox(p);
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
            ViewBag.writerName = GetName(mail);
            return View();
        }

        [HttpPost]
        public ActionResult NewMessage(Message p)
        {
            ValidationResult results = messageValidator.Validate(p);
            if (results.IsValid)
            {
                p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
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
            ViewBag.writerName = GetName(mail);
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
            ViewBag.writerName = GetName(mail);
            var values = mm.GetByID(id);
            return View(values);
        }

    }
}