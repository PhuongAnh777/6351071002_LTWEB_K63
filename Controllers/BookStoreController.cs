using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;

namespace MvcBookStore.Controllers
{
        public class BookStoreController : Controller
        {
            // GET: BookStore
            private QLBANSACHEntities db = new QLBANSACHEntities();
            public ActionResult Index()
            {
            var books = db.SACH
                .OrderByDescending(b => b.Ngaycapnhat)
                .Take(5)
                .ToList();
            var categories = db.CHUDE.ToList();
            var publishers = db.NHAXUATBAN.ToList();

            // Kiểm tra null trước khi gán giá trị
            ViewBag.Books = books ?? new List<SACH>();
            ViewBag.Categories = categories ?? new List<CHUDE>();
            ViewBag.Publishers = publishers ?? new List<NHAXUATBAN>();

            return View();

            }
        // Action cho PartialView Chude
        public ActionResult Chude()
        {
            var categories = db.CHUDE.ToList();
            return PartialView(categories);
        }

        // Action cho PartialView Nhaxuatban
        public ActionResult Nhaxuatban()
        {
            var publishers = db.NHAXUATBAN.ToList();
            return PartialView(publishers);
        }
        // Action để lấy sản phẩm theo chủ đề
        public ActionResult SPTheochude(int id)
        {
            var books = db.SACH.Where(s => s.MaCD == id).ToList(); // Lấy sách theo chủ đề
            ViewBag.CategoryName = db.CHUDE.Find(id).TenChuDe; // Tên chủ đề
            return View(books);
        }

        // Action để lấy sản phẩm theo nhà xuất bản
        public ActionResult SPTheoNXB(int id)
        {
            var books = db.SACH.Where(s => s.MaNXB == id).ToList(); // Lấy sách theo nhà xuất bản
            ViewBag.PublisherName = db.NHAXUATBAN.Find(id).TenNXB; // Tên nhà xuất bản
            return View(books);
        }

        // Action để xem chi tiết sản phẩm
        public ActionResult Details(int id)
        {
            var book = db.SACH.Find(id); // Lấy thông tin sách
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
    }
}