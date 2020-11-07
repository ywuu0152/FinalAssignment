using FIT5032_Week08A.Models;
using FIT5032_Week08A.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalAssignment.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        public ActionResult Send_Email(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;

                    var attachment = Request.Files["attachment"];

                    if (attachment.ContentLength > 0)
                    {
                        String filePath = Path.Combine(Server.MapPath("~/Content/Attachment/"), attachment.FileName);
                        attachment.SaveAs(filePath);

                        EmailSender es = new EmailSender();
                        //es.Send(toEmail, subject, contents);
                        es.SendWithAttachment(toEmail, subject, contents, filePath, attachment.FileName);
                    }
                    else
                    {
                        EmailSender es = new EmailSender();
                        es.Send(toEmail, subject, contents);
                    }

                    ViewBag.Result = "Email has been send.";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

        public ActionResult loader()
        {
            return View();
        }

    }

}