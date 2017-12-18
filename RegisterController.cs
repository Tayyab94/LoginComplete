using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoginComplete.Models;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Text;
using System.Net.Mail;

namespace LoginComplete.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult SaveData(SiteUser model)
        {
            using (DemoContext db = new DemoContext())
            {
               
                model.IsValid = false;
             
                db.tblSiteUser.Add(model);
                db.SaveChanges();
                BuildEmailTemplate(model.ID);

            }
            return Json("Registration Successfull..", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Confirm(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }

        public JsonResult RegistreConfirm(int regId)  //This Method is work for Active usr account that mean it save isValid= true
        {                                                      //in the database and return a Msg Account is activated Now 
            DemoContext _contect = new DemoContext();
            SiteUser Data = _contect.tblSiteUser.Where(x => x.ID == regId).FirstOrDefault();
            Data.IsValid = true;
            _contect.SaveChanges();
            var msg = "Your Message is Verified!";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public void BuildEmailTemplate(int regID)
        { DemoContext _Context = new DemoContext();
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = _Context.tblSiteUser.Where(x => x.ID == regID).FirstOrDefault();

            //Now Create a Confirm link for send with Email
            //ControllNer /ViewName?Parameter
            var url = "http://localhost:57226/" + "Register/Confirm?regId=" + regID;  //Now create a New VIew According to "Confirm Link
                                                                                      //because you create a confirm link with Email when user see and 
                                                                                      //Click the confirm link the user has to move to a View 
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Your Account is Successfully Created", body, regInfo.Email);
        }

        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "tayyab.bhatti30@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if(!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }

            if (!string.IsNullOrEmpty(cc))
            {
               
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);
        }

        private static void SendEmail(MailMessage mail)  //This Methos work for Send Built Email
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("tayyab.bhatti30@gmail.com", "@@@123456789T");
            try
            {
                client.Send(mail);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        //This Method received the data from the login View and Check in the data base , This usre is register or not in the database 

        public JsonResult CheckValidUser(SiteUser model)
        {
            DemoContext _context = new DemoContext();
         
            string result = "fial";
            var DataItem = _context.tblSiteUser.Where(x => x.Email.Equals(model.Email) && x.Password.Equals(model.Password)).SingleOrDefault(); // This qUery Check in the database table weither the use is exist in the database or not
            if(DataItem!=null)
            {
                Session["UserID"] = DataItem.ID.ToString();
                Session["UserNamae"] = DataItem.UserName.ToString();
                result = "Success";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AfterLogin()  // This Method Usre Profile Page where the user will direct after the login 
        {
            if(Session["UserID"]!=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult Logout() //This Method for the usre's Logout
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index");
        }
    }

  
}