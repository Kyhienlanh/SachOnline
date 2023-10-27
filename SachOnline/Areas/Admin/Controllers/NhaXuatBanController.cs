using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Linq;
using System;

namespace SachOnline.Areas.Admin.Controllers
{
    public class NhaXuatBanController : Controller
    {
        
        BookOnlineEntities db = new BookOnlineEntities();
        // GET: Admin/ChuDe
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB).ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            var data = db.NHAXUATBANs;
            return View();
        }

        [HttpPost]
        public ActionResult Create(NHAXUATBAN nxb)
        {
            if (ModelState.IsValid)
            {

                db.NHAXUATBANs.Add(nxb);
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(nxb);
        }

        public ActionResult Edit(int id)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", nxb.MaNXB);
            return View(nxb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Sử dụng ValidateAntiForgeryToken để bảo mật ứng dụng
        public ActionResult Edit(NHAXUATBAN model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BookOnlineEntities()) // Thay thế YourDbContext bằng đối tượng DbContext thực tế của bạn
                {
                    // Lấy nhà xuất bản từ cơ sở dữ liệu bằng mã
                    var nxb = db.NHAXUATBANs.Find(model.MaNXB);

                    if (nxb == null)
                    {
                        return HttpNotFound();
                    }

                    // Cập nhật thông tin nhà xuất bản với dữ liệu mới từ model
                    nxb.TenNXB = model.TenNXB;
                    nxb.DiaChi = model.DiaChi;
                    nxb.DienThoai = model.DienThoai;

                    db.SaveChanges(); // Lưu các thay đổi vào cơ sở dữ liệu
                }

                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ, trả về View với model để hiển thị lỗi
            return View(model);
        }


        public ActionResult Details(int id)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var nHAXUATBAN = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nHAXUATBAN == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nHAXUATBAN);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var nHAXUATBAN = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nHAXUATBAN == null)
            {
                Response.StatusCode = 404;
                return null; // Handle the case when the record is not found
            }

            try
            {
                db.NHAXUATBANs.Remove(nHAXUATBAN);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle any potential exceptions (e.g., database update errors) here.
                // You can log the exception or perform other error-handling tasks.
                // Example: Log.Error(ex, "Error deleting CHUDE record");
                // You can also return an error view or redirect to an error page.
                // Example: return View("Error");
                // Or redirect to a custom error page: return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index");
        }

    }
}