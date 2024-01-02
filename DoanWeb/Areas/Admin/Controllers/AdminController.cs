using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoanWeb.Models;
using System.IO;
using BCrypt;


namespace DoanWeb.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        private data database = new data();
        public ActionResult Index()
        {
            string acc = Session["role"] as string;
            if (acc!=null)
            {
                if (acc == "admin" || acc == "management") 
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("login", "Admin");
                }
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
            
        }
        

        public ActionResult login()
        {
            Session.Add("role", "");
            return View();
        }
        [HttpPost]
        public ActionResult login(string taikhoan, string matkhau)
        {

            bool isPasswordMatched = false;
            var role = database.User_role.Where(p => p.tk == taikhoan).FirstOrDefault();
            if (role != null)
            {
                isPasswordMatched = BCrypt.Net.BCrypt.Verify(matkhau, role.mk);
            }
            if (role != null&& isPasswordMatched&& role._lock!="false")
            {
                if (role.role == "admin" || role.role=="management")
                {
                    Session.Add("role", role.role);
                    Session.Add("acc", role.id_user);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.error = "Bạn không có quyền truy cập";
                    return View();
                }
               
            }
            else
            {
                ViewBag.error = "Tài khoản hoặc mật khẩu sai";
                return View();
            }
            
        }
        public ActionResult account_management()
        {

            string acc = Session["role"] as string;
            if (acc != null)
            {
                if (acc == "admin")
                {
                    List<User_role> dulieu = database.User_role.ToList();
                    return View(dulieu);
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
        }

       [HttpPost]
        public ActionResult sua_nguoidung(string taikhoan, string name,string address,string phone,string email,string ngaysinh,string khoa,string role)
        {
            string acc = Session["role"] as string;
            if (acc != null)
            {
                if (acc == "admin")
                {
                    var timkiem = database.User_role.Where(u => u.tk == taikhoan).FirstOrDefault();
                    if (timkiem != null)
                    {
                        timkiem.name = name;
                        timkiem.address = address;
                        timkiem.phone=phone;
                        timkiem.email = email;
                        timkiem.ngaysinh = ngaysinh;
                        timkiem._lock = khoa;
                        timkiem.role = role;
                    }
                    database.SaveChanges();
                    ViewBag.thanhcong = "Sửa thành công";
                    List<User_role> dulieu = database.User_role.ToList();

                    return View("account_management", dulieu);
                }
                else
                {
                    return RedirectToAction("login", "Admin");
                }
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
        }
        
        public ActionResult xoa_nguoidung(string taikhoan)
        {

            var timkiem = database.User_role.Where(u => u.tk == taikhoan).FirstOrDefault();
            if (timkiem != null)
            {
                database.User_role.Remove(timkiem);
                database.SaveChanges();
                ViewBag.thanhcong = "Xóa thành công";
            }
            
            List<User_role> dulieu = database.User_role.ToList();

            return View("account_management", dulieu);
        }


            public ActionResult add_account()
        {

            string acc = Session["role"] as string;
            if (acc != null)
            {
                if (acc == "admin" || acc == "management")
                {
                    
                    return View();
                }
                else
                {
                    return RedirectToAction("login", "Admin");
                }
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
        }
       
        [HttpPost]
        public ActionResult add_account(string tk,string mk, string role,string username, string phone, string address,string email,string ngaysinh, HttpPostedFileBase file)
        {

            string acc = Session["role"] as string;
            if (acc != null)
            {
                if (acc == "admin" || acc == "management")
                {
                    var id = database.User_role.Select(u => u.id_user).ToList();
                            int dem = 1;
                            var check = database.User_role.Where(u=>u.tk==tk).Select(u => u.tk).FirstOrDefault();
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
                
                        var tao = new User_role
                                {
                                    id_user = dem.ToString(),
                                    tk = tk,
                                    mk = matkhau,
                                    role = role,
                                    name = username,
                                    address = address,
                                    phone = phone,
                                    _lock = "true",
                                    email = email,
                                    ngaysinh = ngaysinh,
                                    img = filename
                                };
                                database.User_role.Add(tao);
                                database.SaveChanges();
                                ViewBag.thanhcong = "Tạo tài khoản thành công";
                                return View();
                            }
                }
                else
                {
                    return RedirectToAction("login", "Admin");
                }
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
        }


        public ActionResult error()
        {

            return View();
        }
    }
}