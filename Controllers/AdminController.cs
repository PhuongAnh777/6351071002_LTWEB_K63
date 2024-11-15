using MvcBookStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MvcBookStore.Controllers
{
    public class AdminController : Controller
    {
        private QLBANSACHEntities db = new QLBANSACHEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sach(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACH.ToList());
            return View(db.SACH.ToList().OrderBy(n => n.Masach).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = db.ADMIN.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["AdminLoggedIn"] = true;
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSach()
        {
            if (Session["AdminLoggedIn"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe),"MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB),"MaNXB", "TenNXB");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSach(SACH sach, HttpPostedFileBase fileupload)
        {
            if (Session["AdminLoggedIn"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
           
            if(fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/Hinhdulieu"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sach.Hinhminhhoa = fileName;

                    db.SACH.Add(sach);
                    db.SaveChanges();
                }
                return RedirectToAction("Sach");
            }
        }
        public ActionResult Chitietsach (int id)
        {
            if (Session["AdminLoggedIn"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            SACH sach = db.SACH.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpGet]
        public ActionResult Xoasach (int id)
        {
            if (Session["AdminLoggedIn"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            SACH sach = db.SACH.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);  
        }
        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoa (int id)
        {
            if (Session["AdminLoggedIn"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            SACH sach = db.SACH.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACH.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Sach");
        }
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            if (Session["AdminLoggedIn"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            SACH sach = db.SACH.SingleOrDefault(n => n.Masach == id);
            
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        {
            if (Session["AdminLoggedIn"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaCD = new SelectList(db.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View(sach);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/Hinhdulieu"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        return View(sach);
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    sach.Hinhminhhoa = fileName;

                    db.Entry(sach).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                }
                return RedirectToAction("Sach");
            }
        }
        public ActionResult Thongke()
        {
            if (Session["AdminLoggedIn"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var booksByCategory = db.SACH
                                    .GroupBy(s => s.CHUDE.TenChuDe)  // Assuming `Tenchude` is the category name in CHUDE
                                    .Select(g => new { Category = g.Key, Count = g.Count() })
                                    .ToList();

            ViewBag.ChartLabels = booksByCategory.Select(b => b.Category).ToArray();
            ViewBag.ChartData = booksByCategory.Select(b => b.Count).ToArray();

            return View();
        }
    }
}