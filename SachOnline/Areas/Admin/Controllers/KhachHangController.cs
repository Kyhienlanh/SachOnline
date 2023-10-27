using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Linq;
using System;
using System.Data.Entity.Infrastructure;

namespace SachOnline.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: Admin/KhachHang
        BookOnlineEntities db = new BookOnlineEntities();
        // GET: Admin/ChuDe
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(iPageNum, iPageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            var data = db.KHACHHANGs;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KHACHHANG newKhachHang)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Add the new customer to the database
                    db.KHACHHANGs.Add(newKhachHang);
                    db.SaveChanges();

                    return RedirectToAction("Index"); // Redirect to the list of customers
                }
                catch (DbUpdateException ex)
                {
                    // Handle the exception or log it as needed
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the customer.");
                }
            }

            // If the ModelState is not valid or there is an error, return the view with validation errors
            return View(newKhachHang);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(kh);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, KHACHHANG updatedKhachHang)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Find the existing customer by ID
                var existingKhachHang = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);

                if (existingKhachHang == null)
                {
                    return HttpNotFound();
                }

                // Update the existing customer's properties
                existingKhachHang.HoTen = updatedKhachHang.HoTen;
                existingKhachHang.TaiKhoan = updatedKhachHang.TaiKhoan;
                existingKhachHang.MatKhau = updatedKhachHang.MatKhau;
                existingKhachHang.Email = updatedKhachHang.Email;
                existingKhachHang.DiaChi = updatedKhachHang.DiaChi;
                existingKhachHang.DienThoai = updatedKhachHang.DienThoai;
                existingKhachHang.NgaySinh = updatedKhachHang.NgaySinh;

                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                // If the model state is not valid, return the view with validation errors
                return View(updatedKhachHang);
            }
        }


        public ActionResult Delete(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            
            db.KHACHHANGs.Remove(kh);
            db.SaveChanges();

            return RedirectToAction("Index");


        }
        public ActionResult Details(int id)
        {
            var sach = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        




    }
}

