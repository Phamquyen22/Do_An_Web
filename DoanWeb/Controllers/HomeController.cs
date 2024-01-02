using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DoanWeb.Models;
namespace DoanWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home



        public ActionResult Index()
        {
            Models.data data_db = new Models.data();
            List<Products> products = data_db.Products.Where(p => p.proDiscount > 6).ToList();

            return View(products);
        }
        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }
        [HttpPost]
        public ActionResult Login(string tk , string mk)
        {
            Models.data data_db = new Models.data();
            bool isPasswordMatched = false;
            var role = data_db.User_role.Where(p => p.tk == tk).FirstOrDefault();
            if (role != null)
            {
                isPasswordMatched = BCrypt.Net.BCrypt.Verify(mk, role.mk);
            }
            if (role != null && isPasswordMatched && role._lock!="false")
            {
                    Session.Add("role", tk);
                    Session.Add("acc", role.id_user);
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu !";

                return View();
            }
            
            
        }
        public ActionResult CreateAcc()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAcc(string tk,string mk,string name , string address, string phone,string email,HttpPostedFileBase file)
        {
            data data_db = new data();
            
            int dem = 1;
            var id = data_db.User_role.Select(u => u.id_user).ToList();
            var check = data_db.User_role.Where(u => u.tk == tk).Select(u => u.tk).FirstOrDefault();
            if (check != null)
            {
                ViewBag.error = "Đã tồn tại tài khoản";
                return View();
            }
            else
            {
                foreach (var i in id)
                {
                    if (dem == int.Parse(i)) dem = dem + 1;
                }
                string filename = "avatar-1.png";
                if (file != null && file.ContentLength > 0)
                {
                    filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    string filepath = Path.Combine(Server.MapPath("~/public/img/user/"), filename);
                }
                string matkhau = BCrypt.Net.BCrypt.HashPassword(mk);
                var acccout = new User_role
                {
                    id_user = dem.ToString(),
                    tk = tk,
                    mk = matkhau,
                    name = name,
                    address = address,
                    role = "user",
                    img = filename,
                    phone = phone,
                    _lock = "true",
                    email = email,

                };
                data_db.User_role.Add(acccout);
                data_db.SaveChanges();
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult error()
        {
            return View();
        }
        
        public ActionResult addcart(string proID)
        {
            var dk = Session["role"];
            string acc = Session["acc"] as string;
            string size = Session["size"] as string;
            if (dk != null)
            {
                Models.data data_db = new Models.data();

                // Lấy session hàng từ cơ sở dữ liệu
                var ses_hang = data_db.Sessions.SingleOrDefault(s => s.id_user == acc && s.ses == proID);

                if (ses_hang != null)
                {
                    ses_hang.soluong += 1;
                    data_db.SaveChanges();
                }
                else
                {
                    var latestSession = data_db.Sessions.Select(p => p.id_ses).ToList();
                    var dem = 1;
                    if (latestSession != null)
                    {
                        foreach (var i in latestSession)
                        {

                            if (int.Parse(i) == dem)
                            {
                                dem = dem + 1;
                            }
                            
                        }
                        var capnhat = new Sessions
                        {
                            id_ses = dem.ToString(),
                            id_user = acc,
                            ses = proID,
                            soluong = 1,
                            size_sp=size
                        };

                        data_db.Sessions.Add(capnhat);
                        data_db.SaveChanges();
                    }
                    else
                    {
                        var capnhat = new Sessions
                        {
                            id_ses = "1",
                            id_user = acc,
                            ses = proID,
                            soluong = 1,
                            size_sp = size
                        };

                        data_db.Sessions.Add(capnhat);
                        data_db.SaveChanges();
                    }

                    
                }

                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult contact()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string subject, string message, string name, string email, string pass)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
                {
                    ViewBag.ErrorMessage = "Please fill in all fields.";
                    return View();
                }

                // Use secure App Password if two-factor authentication is enabled
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(email, pass);
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(email);
                        mailMessage.To.Add("nguyhiem46@gmail.com");
                        mailMessage.Subject = subject;
                        mailMessage.Body = message;
                        mailMessage.IsBodyHtml = true;

                        smtpClient.Send(mailMessage);
                    }

                    ViewBag.thanhcong = "Email sent successfully!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = "Error sending email: " + ex.Message;
            }

            return View();
        }



    }

}