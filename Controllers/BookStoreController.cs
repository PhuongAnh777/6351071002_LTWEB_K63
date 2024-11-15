using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MvcBookStore.Models;
using PagedList;
using PagedList.Mvc;

namespace MvcBookStore.Controllers
{
    public class BookStoreController : Controller
    {
        // GET: BookStore
        private QLBANSACHEntities db = new QLBANSACHEntities();
        private List<SACH> Laysachmoi(int count)
        {
            return db.SACH.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            //var sachmoi = db.SACH
            //.OrderByDescending(b => b.Ngaycapnhat)
            //.ToPagedList(pageNum, pageSize);

            var sachmoi = Laysachmoi(30);
            var categories = db.CHUDE.ToList();
            var publishers = db.NHAXUATBAN.ToList();

            // Kiểm tra null trước khi gán giá trị
            //ViewBag.Books = sachmoi ?? new List<SACH>();
            ViewBag.Categories = categories ?? new List<CHUDE>();
            ViewBag.Publishers = publishers ?? new List<NHAXUATBAN>();

            return View(sachmoi.ToPagedList(pageNum, pageSize));
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
        public ActionResult SPTheochude(int id, int? page)
        {
            int pageSize = 2;
            int pageNum = (page ?? 1);

            var books = db.SACH.Where(s => s.MaCD == id).OrderByDescending(a => a.Ngaycapnhat).ToList(); // Lấy sách theo chủ đề
            ViewBag.CategoryName = db.CHUDE.Find(id).TenChuDe; // Tên chủ đề
            return View(books.ToPagedList(pageNum, pageSize));
        }

        // Action để lấy sản phẩm theo nhà xuất bản
        public ActionResult SPTheoNXB(int id, int? page)
        {
            int pageSize = 2;
            int pageNum = (page ?? 1);

            var books = db.SACH.Where(s => s.MaNXB == id).OrderByDescending(a => a.Ngaycapnhat).ToList(); // Lấy sách theo nhà xuất bản
            ViewBag.PublisherName = db.NHAXUATBAN.Find(id).TenNXB; // Tên nhà xuất bản
            return View(books.ToPagedList(pageNum, pageSize));
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