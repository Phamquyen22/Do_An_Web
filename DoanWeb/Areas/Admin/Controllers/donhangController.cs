using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoanWeb.Models;

namespace DoanWeb.Areas.Admin.Controllers
{
    public class donhangController : Controller
    {
        // GET: Admin/donhang
        private data database = new data();
        
        public ActionResult donhang()
        {
            string acc = Session["role"] as string;
            if (acc != null && (acc == "admin" || acc == "management"))
            {
                List<Orders> or = database.Orders.ToList();
                return View(or);
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
        }

        public ActionResult Xacnhan(string orderID, string check,string action)
        {

            string acc = Session["role"] as string;
            if (acc != null && (acc == "admin" || acc == "management"))
            {
                if (!string.IsNullOrEmpty(action))
                {
                    if (orderID != null)
                    {
                        var accept = database.Orders.Find(orderID);
                        accept.orderStatus = check;
                        database.SaveChanges();
                        ViewBag.thanhcong = "Đã chuyển sang " + check;
                    }
                    List<Orders> or;
                    if (action == "choxacnhan")
                    {
                        or = database.Orders.Where(o => o.orderStatus == "Chờ xác nhận").ToList();
                    }
                    else if (action == "chovanchuyen")
                    {
                        or = database.Orders.Where(o => o.orderStatus == "Chờ Vận chuyển").ToList();
                    }
                    else
                    {
                        or = database.Orders.ToList();
                        action = "donhang";
                    }

                    return View(action, or);
                }
                else
                {
                    if (orderID != null)
                    {
                        var accept = database.Orders.Find(orderID);
                        accept.orderStatus = check;
                        database.SaveChanges();
                        ViewBag.thanhcong = "Đã chuyển sang " + check;
                    }

                    return RedirectToAction("donhang", "donhang");
                }

            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
           
            
        }
        public ActionResult detail(string orderID)
        {
            string acc = Session["role"] as string;
            if (acc != null && (acc == "admin" || acc == "management"))
            {
                if (orderID != null)
                {
                    List<OrderDetails> order = database.OrderDetails.Where(o => o.orderID == orderID).ToList();

                    return View(order);
                }
                else
                {
                    return RedirectToAction("donhang", "donhang");
                }
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
            
        }
        
        public ActionResult xoa_donhang(string orderID)
        {

            string acc = Session["role"] as string;
            if (acc != null && (acc == "admin" || acc == "management"))
            {
                if (orderID != null)
                {
                    var a = database.OrderDetails.Where(o=>o.orderID==orderID).ToList();
                    if (a != null)
                    {
                        foreach (var i in a)
                        {
                            database.OrderDetails.Remove(i);
                            database.SaveChanges();
                        }
                    }
                    var remove = database.Orders.Where(o=>o.orderID==orderID).FirstOrDefault();
                    if (remove != null)
                    {
                        database.Orders.Remove(remove);
                        database.SaveChanges();
                        ViewBag.thanhcong = "Xóa thành công đơn hàng";
                    }
                    else
                    {
                        ViewBag.error = "Xóa không thành công vì không tồn tại đơn hàng";
                    }
                    List<Orders> or = database.Orders.ToList();
                    return RedirectToAction("donhang", "donhang");
                }
                else
                {
                    return RedirectToAction("error", "Admin");
                }
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
        }



        public ActionResult choxacnhan()
        {
            string acc = Session["role"] as string;
            if (acc != null && (acc == "admin" || acc == "management"))
            {
                List<Orders> or = database.Orders.Where(o => o.orderStatus == "Chờ xác nhận").ToList();

                return View(or);
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
        }
        public ActionResult donhoanthanh()
        {
            string acc = Session["role"] as string;
            if (acc != null && (acc == "admin" || acc == "management"))
            {
                List<Orders> or = database.Orders.Where(o => o.orderStatus == "Hoàn thành").ToList();
                return View(or);
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
        }
        public ActionResult chovanchuyen()
        {
            string acc = Session["role"] as string;
            if (acc != null && (acc == "admin" || acc == "management"))
            {
                List<Orders> or = database.Orders.Where(o => o.orderStatus == "Chờ Vận chuyển").ToList();
                return View(or);
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
        }
    }
}