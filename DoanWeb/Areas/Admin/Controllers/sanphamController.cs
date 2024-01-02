using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoanWeb.Models;
namespace DoanWeb.Areas.Admin.Controllers
{
    public class sanphamController : Controller
    {
        // GET: Admin/sanpham



        // nhóm sản xuất

        public ActionResult nhomsanpham()
        {

            string acc = Session["role"] as string;
            if (acc != null)
            {
                if (acc == "admin" || acc == "management")
                {
                    data database = new data();
                    List<Categories> cate = database.Categories.ToList();
                    return View(cate);
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
        public ActionResult sua_cate(int cateID, string cateName, HttpPostedFileBase file)
        {
            string fileName = "";

            data database = new data();
            if (fileName == "")
            {
                fileName = database.Categories.Where(c => c.cateID == cateID).Select(c => c.catePhoto).FirstOrDefault();
            }
            if (file != null && file.ContentLength > 0)
            {
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss")+Path.GetFileName(file.FileName);
                string filePath = Path.Combine(Server.MapPath("~/public/img/cate/"), fileName);
                file.SaveAs(filePath);

            }
            if (cateID.ToString() != null)
            {

                var sua = database.Categories.SingleOrDefault(c => c.cateID == cateID);
                if (sua != null)
                {
                    sua.cateName = cateName;
                    sua.catePhoto = fileName;

                    database.SaveChanges();
                }
            }

            return RedirectToAction("nhomsanpham", "sanpham");
        }

        [HttpPost]
        public ActionResult add_cate(string name_cateName, HttpPostedFileBase f_file)
        {
            string fileName = "demo.jpg";

            data database = new data();

            if (f_file != null && f_file.ContentLength > 0)
            {
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(f_file.FileName);
                string filePath = Path.Combine(Server.MapPath("~/public/img/cate/"), fileName);
                f_file.SaveAs(filePath);
            }
            if (name_cateName.ToString() != null)
            {
                var caste_id = database.Categories.Select(p => p.cateID).ToList();
                int dem = 1;
                if (caste_id != null)
                {
                    foreach (var i in caste_id)
                    {
                        if (dem == i) dem = dem + 1;
                    }
                }
                var ad = new Categories
                {
                    cateID = dem,
                    cateName = name_cateName,
                    catePhoto = fileName
                };
                database.Categories.Add(ad);
                database.SaveChanges();
            }
            List<Categories> cate = database.Categories.ToList();
            ViewBag.thanhcong = "Thêm thành công danh mục sản phẩm";
            return View("nhomsanpham", cate);
        }

        public ActionResult xoa_danhmuc(int cateID)
        {
            data database = new data();

            if (cateID.ToString() != null)
            {
                var del = database.Categories.SingleOrDefault(c => c.cateID == cateID);
                var check = database.ProductTypes.Where(p => p.cateID == cateID).FirstOrDefault();
                if (check == null)
                {
                    database.Categories.Remove(del);
                    database.SaveChanges();
                    List<Categories> cate = database.Categories.ToList();
                    ViewBag.thanhcong = "Xóa thành công danh mục sản phẩm";
                    return View("nhomsanpham", cate);
                }
                else
                {
                    ViewBag.error = "Không thể xóa danh mục sản phẩm này vì có liên kết với loại sản phẩm";
                    List<Categories> cate = database.Categories.ToList();
                    return View("nhomsanpham", cate);
                }
            }

            return RedirectToAction("nhomsanpham");
        }

        // loại sản phẩm
        public ActionResult loaisanpham()
        {
            string acc = Session["role"] as string;
            if (acc != null)
            {
                if (acc == "admin" || acc == "management")
                {
                    data database = new data();
                    List<ProductTypes> pro_tp = database.ProductTypes.ToList();
                    return View(pro_tp);
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
        public ActionResult add_type(string cateName, string typeName)
        {


            data database = new data();


            if (cateName.ToString() != null)
            {

                var type_id = database.ProductTypes.Select(p => p.typeID).ToList();
                int dem = 1;
                if (type_id != null)
                {
                    foreach (var i in type_id)
                    {
                        if (dem == i) dem = dem + 1;
                    }
                }

                int cate_id = database.Categories.Where(c => c.cateName == cateName).Select(c => c.cateID).FirstOrDefault();
                var ad = new ProductTypes
                {
                    typeID = dem,
                    cateID = cate_id,
                    typeName = typeName,
                };
                database.ProductTypes.Add(ad);
                database.SaveChanges();
            }
            List<ProductTypes> cate = database.ProductTypes.ToList();
            ViewBag.thanhcong = "Thêm thành công danh mục sản phẩm";
            return View("loaisanpham", cate);
        }
        [HttpPost]
        public ActionResult sua_type(int typeID, string cateName, string typeName)
        {


            data database = new data();

            if (typeID.ToString() != null)
            {

                var sua = database.ProductTypes.SingleOrDefault(c => c.typeID == typeID);
                var id_cate = database.Categories.Where(c => c.cateName == cateName).Select(c => c.cateID).FirstOrDefault();
                if (sua != null)
                {
                    sua.cateID = id_cate;
                    sua.typeName = typeName;

                    database.SaveChanges();
                    ViewBag.thanhcong = "Sửa thành công";
                }
                else
                {
                    ViewBag.error = "lỗi sửa";
                }
            }
            List<ProductTypes> pro_tp = database.ProductTypes.ToList();

            return View("loaisanpham", pro_tp);
        }

        
        public ActionResult xoa_loaisanpham(int typeID)
        {

            data database = new data();

            if (typeID.ToString() != null)
            {
                var xoa_type = database.ProductTypes.SingleOrDefault(c => c.typeID == typeID);
                var check = database.Products.Where(p => p.typeID == typeID).FirstOrDefault();
                if (check == null)
                {
                    database.ProductTypes.Remove(xoa_type);
                    database.SaveChanges();
                    List<ProductTypes> pro_type = database.ProductTypes.ToList();
                    ViewBag.thanhcong = "Xóa thành công loại sản phẩm";
                    return View("loaisanpham", pro_type);
                }
                else
                {
                    ViewBag.error = "Không thể xóa loại sản phẩm này vì có liên kết với sản phẩm";
                    List<ProductTypes> pro_type = database.ProductTypes.ToList();
                    return View("loaisanpham", pro_type);
                }
            }
            return RedirectToAction("loaisanpham");
        }


        // NHÀ SẢN XUẤT
        public ActionResult nhasanxuat()
        {
            data database = new data();
            List<Producers> pdc = database.Producers.ToList();
            return View(pdc);
        }
        [HttpPost]
        public ActionResult add_nhasanxuat(string pdcName, string pdcPhone, string pdcEmail, string pdcAddress, HttpPostedFileBase file, string pdcInfo)
        {
            data database = new data();


            if (pdcName.ToString() != null)
            {
                string fileName = "avatar-1.png";
                var pdc_id = database.Producers.Select(p => p.pdcID).ToList();
                int dem = 1;
                if (pdc_id != null)
                {
                    foreach (var i in pdc_id)
                    {
                        if (dem == i) dem = dem + 1;
                    }
                }

                if (file != null && file.ContentLength > 0)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/public/img/producers/"), fileName);
                    file.SaveAs(filePath);

                }
                var ad = new Producers
                {
                    pdcID = dem,
                    pdcName = pdcName,
                    pdcPhone = pdcPhone,
                    pdcEmail = pdcEmail,
                    pdcAddress = pdcAddress,
                    pdcPhoto = fileName,
                    pdcInfo = pdcInfo
                };
                database.Producers.Add(ad);
                database.SaveChanges();
            }
            List<Producers> prod = database.Producers.ToList();
            ViewBag.thanhcong = "Thêm thành công nhà sản xuất";

            return View("nhasanxuat", prod);
        }
        [HttpPost]
        public ActionResult sua_nhasanxuat(int id_pdcID, string N_pdcName, string P_pdcPhone, string E_pdcEmail, string A_pdcAddress, HttpPostedFileBase f_file, string I_pdcInfo)
        {
            data database = new data();
            string fileName;
            if (N_pdcName.ToString() != null)
            {


                if (f_file != null && f_file.ContentLength > 0)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(f_file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/public/img/producers/"), fileName);
                    f_file.SaveAs(filePath);


                    var sua = database.Producers.SingleOrDefault(pr => pr.pdcID == id_pdcID);

                    if (sua != null)
                    {
                        sua.pdcName = N_pdcName;
                        sua.pdcPhone = P_pdcPhone;
                        sua.pdcEmail = E_pdcEmail;
                        sua.pdcAddress = E_pdcEmail;
                        sua.pdcPhoto = fileName;


                        database.SaveChanges();
                        ViewBag.thanhcong = "Sửa thành công";
                    }
                    else
                    {
                        ViewBag.error = "lỗi sửa";
                    }
                }
                else
                {
                    var sua = database.Producers.SingleOrDefault(pr => pr.pdcID == id_pdcID);

                    if (sua != null)
                    {
                        sua.pdcName = N_pdcName;
                        sua.pdcPhone = P_pdcPhone;
                        sua.pdcEmail = E_pdcEmail;
                        sua.pdcAddress = E_pdcEmail;
                        sua.pdcInfo = I_pdcInfo;


                        database.SaveChanges();
                        ViewBag.thanhcong = "Sửa thành công";
                    }
                    else
                    {
                        ViewBag.error = "lỗi sửa";
                    }
                }

            }
               

                List<Producers> prod = database.Producers.ToList();
                ViewBag.thanhcong = "Sửa thành công nhà sản xuất";

                return View("nhasanxuat", prod);
            
        }
        public ActionResult xoa_nhasanxuat(int pdcID)
        {

            data database = new data();

            if (pdcID.ToString() != null)
            {
                var xoa_nhasx = database.Producers.SingleOrDefault(c => c.pdcID == pdcID);
                var check = database.Products.Where(p => p.typeID == pdcID).FirstOrDefault();
                if (check == null)
                {
                    database.Producers.Remove(xoa_nhasx);
                    database.SaveChanges();
                    List<Producers> prod = database.Producers.ToList();
                    ViewBag.thanhcong = "Xóa thành công nhà sản xuất";
                    return View("nhasanxuat", prod);
                }
                else
                {
                    ViewBag.error = "Không thể xóa nhà sản xuất này vì có liên kết với sản phẩm";
                    List<Producers> prod = database.Producers.ToList();
                    return View("nhasanxuat", prod);
                }
            }
            return RedirectToAction("nhasanxuat");
        }

        // sản phẩm 
        public ActionResult product()
        {
            string acc = Session["role"] as string;
            if (acc != null)
            {
                if (acc == "admin" || acc == "management")
                {
                    data database = new data();
                    List<Products> pro = database.Products.ToList();
                    return View(pro);
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
        public ActionResult add_sanpham(string nameprod,string nametype,string proName,string proSize,string proPrice,string proDiscount,HttpPostedFileBase file,string proUpdateDate,string proDescription,string proQuantity , string an_sp)
        {
            data database = new data();


            if (proName.ToString() != null)
            {
                string fileName = "demo.jpg";
                var pro_id = database.Products.Select(p => p.proID).ToList();
                int dem = 1;
                if (pro_id != null)
                {
                    foreach (var i in pro_id)
                    {
                        if (dem == int.Parse(i)) dem = dem + 1;
                    }
                }

                if (file != null && file.ContentLength > 0)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/public/img/product/product/"), fileName);
                    file.SaveAs(filePath);

                }
                var id_prod = database.Producers.Where(pr => pr.pdcName == nameprod).Select(s => s.pdcID).FirstOrDefault();
                var id_type = database.ProductTypes.Where(t => t.typeName == nametype).Select(s => s.typeID).FirstOrDefault();
                if (id_type.ToString() != null && id_prod.ToString() != null)
                {
                    var ad = new Products
                    {
                        proID = dem.ToString(),
                        pdcID = id_prod,
                        typeID = id_type,
                        proName = proName,
                        proSize = proSize,
                        proPrice = proPrice,
                        proDiscount = int.Parse(proDiscount),
                        proPhoto = fileName,
                        proUpdateDate = proUpdateDate,
                        proDescription = proDescription,
                        proQuantity = int.Parse(proQuantity),
                        hide = an_sp
                    };
                    database.Products.Add(ad);
                    database.SaveChanges();
                    ViewBag.thanhcong = "Thêm thành công sản phẩm";
                }
                
            }
            
            
            List<Products> prod = database.Products.ToList();
            return View("product", prod);
        }
        [HttpPost]
        public ActionResult sua_sanpham(string id_proID, string id_nameprod, string id_nametype, string N_proName, string N_proSize, string N_proPrice, string N_proDiscount, HttpPostedFileBase f_file, string N_proUpdateDate, string N_proDescription, string N_proQuantity , string an_sp)
        {
            data database = new data();
            string fileName;
            if (id_proID.ToString() != null)
            {


                if (f_file != null && f_file.ContentLength > 0)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(f_file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/public/img/product/product/"), fileName);
                    f_file.SaveAs(filePath);


                    var sua = database.Products.SingleOrDefault(p => p.proID == id_proID);
                    var id_prod = database.Producers.Where(pr => pr.pdcName == id_nameprod).Select(s => s.pdcID).FirstOrDefault();
                    var id_type = database.ProductTypes.Where(t => t.typeName == id_nametype).Select(s => s.typeID).FirstOrDefault();
                    if (sua != null)
                    {
                        sua.pdcID = id_prod;
                        sua.typeID = id_type;
                        sua.proName = N_proName;
                        sua.proSize = N_proSize;
                        sua.proPrice = N_proPrice;
                        sua.proDiscount =  int.Parse( N_proDiscount);
                        sua.proPhoto = fileName;
                        sua.proUpdateDate = N_proUpdateDate;
                        sua.proDescription = N_proDescription;
                        sua.proQuantity = int.Parse(  N_proQuantity);
                        sua.hide = an_sp;
                        database.SaveChanges();
                        ViewBag.thanhcong = "Sửa thành công";
                    }
                    else
                    {
                        ViewBag.error = "Lỗi sửa vì sản phẩm không tồn tại";
                    }
                }
                else
                {
                    var sua = database.Products.SingleOrDefault(p => p.proID == id_proID);
                    var id_prod = database.Producers.Where(pr => pr.pdcName == id_nameprod).Select(s => s.pdcID).FirstOrDefault();
                    var id_type = database.ProductTypes.Where(t => t.typeName == id_nametype).Select(s => s.typeID).SingleOrDefault();
                    if (sua != null)
                    {
                        sua.pdcID = id_prod;
                        sua.typeID = id_type;
                        sua.proName = N_proName;
                        sua.proSize = N_proSize;
                        sua.proPrice = N_proPrice;
                        sua.proDiscount = int.Parse(N_proDiscount);
                        sua.proUpdateDate = N_proUpdateDate;
                        sua.proDescription = N_proDescription;
                        sua.proQuantity = int.Parse(N_proQuantity);
                        sua.hide = an_sp;
                        database.SaveChanges();
                        ViewBag.thanhcong = "Sửa thành công";
                    }
                    else
                    {
                        ViewBag.error = "Lỗi sửa vì sản phẩm không tồn tại";
                    }
                }

            }
            else
            {
                ViewBag.error = "Lỗi sửa vì sản phẩm không tồn tại";
            }
            
            List<Products> prod = database.Products.ToList();
            return View("product", prod);

        }
        public ActionResult xoa_sanpham(string proID)
        {

            data database = new data();

            if (proID.ToString() != null)
            {
                var xoa_sp = database.Products.SingleOrDefault(c => c.proID == proID);
                var check = database.OrderDetails.Where(o => o.proID == proID).Select(o=>o.proID).FirstOrDefault();
                if (check == null)
                {
                    var xoa = database.Products.SingleOrDefault(r => r.proID == proID);
                    if (xoa != null)
                    {
                        database.Products.Remove(xoa);
                        database.SaveChanges();
                    }
                    database.Products.Remove(xoa_sp);
                    database.SaveChanges();
                    List<Products> pro = database.Products.ToList();
                    ViewBag.thanhcong = "Xóa thành công nhà sản xuất";
                    return View("product", pro);
                }
                else
                {
                    List<Products> pro = database.Products.ToList();
                    ViewBag.error = "Xóa sảm phẩm không thành công vì có khách đặt đơn";
                    return View("product", pro);
                }
            }else
            {
                ViewBag.error = "Xóa thành công vì sản phẩm không tồn tại";
                List<Products> pro = database.Products.ToList();
                return View("product", pro);
            }
            
        }


    }


}