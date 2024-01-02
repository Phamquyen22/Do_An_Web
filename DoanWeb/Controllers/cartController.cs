using DoanWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoanWeb.Controllers
{
    public class cartController : Controller
    {
        // GET: cart
        private Models.data database = new Models.data();
        public ActionResult cart()
        {
            string acc = Session["acc"] as string;
            if (!string.IsNullOrEmpty(acc))
            {
                var dulieu_ses = database.Sessions.Where(p => p.id_user == acc).Select(p => p.ses).ToList();
                if (dulieu_ses!=null)
                {
                    return View(dulieu_ses);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }



        }

        [HttpPost]
        public ActionResult UpdateQuantity(string proID, int quantity)
        {
            upgiohang(proID, quantity);

            return Json(new { success = true });
        }
        public void upgiohang(string proID , int quantity)
        {
            Models.data data_db = new Models.data();
            string acc = Session["acc"] as string;
            // Lấy session hàng từ cơ sở dữ liệu
            string pro = proID;
            var ses_hang = data_db.Sessions.SingleOrDefault(s => s.id_user == acc && s.ses == proID);
            if (ses_hang != null)
            {
                ses_hang.soluong = quantity;

                data_db.SaveChanges();

            }


        }

        
        public ActionResult delete_cart (string proID)
        {
            data data = new data();
            string acc = Session["acc"] as string;
            var ses_del = data.Sessions.SingleOrDefault(s => s.id_user == acc && s.ses == proID);
            if (ses_del != null)
            {
                data.Sessions.Remove(ses_del);
                data.SaveChanges();
            }
            return RedirectToAction("cart","cart");
        }

        public ActionResult hoadon()
        {
            string acc = Session["acc"] as string;

            if (!string.IsNullOrEmpty(acc))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        
        

        public ActionResult thanhtoan()
        {
            string acc = Session["acc"] as string;

            if (!string.IsNullOrEmpty(acc))
            {
                data data = new data();
                List<Sessions> ses = data.Sessions.Where(s => s.id_user == acc).ToList();
                var id = data.Orders.OrderByDescending(o => o.orderID).Select(o => o.orderID).FirstOrDefault();


                if (id ==null)
                {
                    id = "1";   
                }
                foreach (var s in ses)
                {
                    List<Products> dulieu = data.Products.Where(p => p.proID == s.ses).ToList();

                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult thanhtoan(string name, string phone, string email, string address, string message)
        {
            string acc = Session["acc"] as string;

            if (!string.IsNullOrEmpty(acc))
            {
                data data = new data();
                List<Sessions> ses = data.Sessions.Where(s => s.id_user == acc).ToList();
                var id = (data.Orders.ToList());
                
                string size = Session["size"] as string;
                if (size == null)
                {
                    size = "s";
                }
                var dem = 1;
                foreach(var i in id)
                {
                    if (dem == int.Parse(i.orderID)) dem += 1;
                }
                
              
                    DateTime date_time = DateTime.Now;

                    string date_t = date_time.ToString("yyyy-MM-dd HH:mm:ss");
                
                    var add_o = new Orders
                    {
                        orderID = dem.ToString(),
                        o_phone = phone,
                        orderMessage = message,
                        orderDateTime = date_t,
                        orderStatus = "Chờ xác nhận",
                        id_user = acc
                        
                    };
                    data.Orders.Add(add_o);
                    data.SaveChanges();
                    foreach (var s in ses)
                    {
                        List<Products> dulieu = data.Products.Where(p => p.proID == s.ses).ToList();

                        foreach (var i in dulieu)
                        {
                            var add_od = new OrderDetails
                            {
                                orderID = dem.ToString(),
                                proID = s.ses,
                                ordtsQuantity = s.soluong,
                                ordtsThanhTien = ((int.Parse(i.proPrice) - (int.Parse(i.proPrice) * i.proDiscount / 100)) * s.soluong).ToString(),
                                size=size,
                            };
                            data.OrderDetails.Add(add_od);
                            data.SaveChanges();
                        }

                    }
                    var xoa_ses = data.Sessions.Where(s => s.id_user == acc).ToList();

                if (xoa_ses != null)
                {
                    foreach(var i in xoa_ses)
                    {

                        data.Sessions.Remove(i);
                        data.SaveChanges();
                    }
                   
                }

                    return RedirectToAction("hoadon", "cart");
                
               
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

    }
}