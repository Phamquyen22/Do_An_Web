using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoanWeb.Models;
namespace DoanWeb.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult product(int page = 1)
        {

            using (var data_db = new data())
            {
                int pageSize = 9;

                var products = data_db.Products.Where(p => p.hide == "Hiện").OrderBy (p=>p.proUpdateDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                int totalProducts = data_db.Products.Where(p => p.hide == "Hiện").Count();
                int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                var viewModel = new productviewpage
                {
                    Products = products,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    Totalproduct = totalProducts
                };
                

                return View(viewModel);
            }

        }

        public ActionResult nhomsanpham(string cateName, int page = 1)
        {
            
                using (var data_db = new Models.data())
                {
                    var cate = data_db.Categories.FirstOrDefault(c => c.cateName == cateName)?.cateID;

                    if (cate != null)
                    {
                        var category = data_db.ProductTypes.FirstOrDefault(c => c.cateID == cate);
                        if (category != null)
                        {
                            int pageSize = 9;

                            var data_searchs = data_db.Products
                                .Where(p => p.typeID == category.typeID && p.hide == "Hiện")
                                .OrderBy(p => p.proUpdateDate)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
                            
                            int totalProducts = data_db.Products.Where(p => p.hide == "Hiện").Count(p => p.typeID == category.typeID);
                            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
                        
                            var viewModel = new productviewpage
                            {
                                Products = data_searchs,
                                CurrentPage = page,
                                TotalPages = totalPages,
                                Totalproduct=totalProducts,
                            };

                            return View("product", viewModel);
                        }
                    }

                    return RedirectToAction("Error");
                }
            
        }

        public ActionResult nhasanxuat(string pdcName, int page = 1)
        {
            try
            {
                using (var data_db = new Models.data())
                {
                    var pcd = data_db.Producers.FirstOrDefault(c => c.pdcName == pdcName).pdcID;

                    if (pcd.ToString() != null)
                    {
                        int pageSize = 9;
                        var data_searchs = data_db.Products
                            .Where(p => p.pdcID == pcd && p.hide == "Hiện")
                            .OrderBy(p => p.proUpdateDate)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                        int totalProducts = data_db.Products.Where(p => p.hide == "Hiện").Count(p => p.pdcID == pcd);
                        int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                        var viewModel = new productviewpage
                        {
                            Products = data_searchs,
                            CurrentPage = page,
                            TotalPages = totalPages,
                            Totalproduct= totalProducts
                        };

                        return View("product", viewModel);
                    }

                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = "Lỗi không có trả về " + ex;
                return View();
            }
        }




        public ActionResult timkiem(string query)
        {


            try
            {

                using (var data_db = new Models.data())
                {
                   
                    List<Products> data = data_db.Products.Where(
                      p => p.proName.Contains(query)&&p.hide=="Hiện").ToList();
                    var dulieu = new productviewpage
                    {
                        Products = data,
                    };
                    return View("product", dulieu);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.error = "một lỗi của hệ thống" + ex;
                return View("Error");
            }

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
                var ses_hang = data_db.Sessions.SingleOrDefault(s => s.id_user == acc && s.ses==proID);

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
                                dem = dem+1;
                            }
                            else
                            {
                                break;
                            }
                        }
                        int nextSessionId = dem;
                        var capnhat = new Sessions
                        {
                            id_ses = nextSessionId.ToString(),
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
                            size_sp=size,
                        };

                        data_db.Sessions.Add(capnhat);
                        data_db.SaveChanges();
                    }
                }
                
                return Redirect(Url.Action("product", "Shop"));
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
    
        }




       
        public ActionResult detail(string proID)
        {

            try
            {

                using (var data_db = new Models.data())
                {

                    var data = data_db.Products.FirstOrDefault(c=>c.proID==proID);
                    
                    return View("detail",data);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.error = "An error occurred while processing your request." + ex;
                return View("Error");
            }
            
        }

        [HttpPost]
        public ActionResult comme(string message, string proID)
        {
            data database = new data();
            var check = database.Comments.Select(p=>p.commentID).ToList();
            string acc = Session["acc"] as string;
            int dem = 1;
            if (check != null)
            {
                foreach (var i in check)
                {
                    if (dem == i) dem += 1;
                }

            }
            var them = new Comments
            {
                commentID = dem,
                commentMessage = message,
                proID=proID,
                id_user = acc,
                datepost= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            };
            database.Comments.Add(them);
            database.SaveChanges();
            var data = database.Products.FirstOrDefault(c => c.proID == proID);

            return View("detail", data);
        }

        [HttpPost]
        public ActionResult SaveSize(string size)
        {
            // Save the size value in the server-side session
            Session.Add("size", size);

            return Json(new { success = true });
        }

        
    }
}