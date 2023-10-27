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
    public class DonHangController : Controller
    {
        BookOnlineEntities db = new BookOnlineEntities();

        // GET: Admin/DonHang

        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]

        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DONDATHANG dONDATHANG, FormCollection f, HttpPostedFileBase fFileUpLoad)
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen");

            if (fFileUpLoad == null)
            {
                // Nếu không có tệp được tải lên, bạn có thể xử lý ở đây (nếu cần)
                // Ví dụ: Thiết lập các giá trị cho ViewBag và trả về View
                ViewBag.DaThanhToan = false; // Ví dụ: Mặc định là chưa thanh toán
                ViewBag.TinhTrangGiaoHang = 1; // Ví dụ: Mặc định là không có tình trạng
                ViewBag.NgayDat = Convert.ToDateTime(f["dNgayDat"]);
                ViewBag.NgayGiao = Convert.ToDateTime(f["dNgayGiao"]);
                return View(dONDATHANG);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // Đã có tệp được tải lên, xử lý và lưu thông tin
                    dONDATHANG.DaThanhToan = Boolean.Parse(f["iDaThanhToan"]); // Ví dụ: Đánh dấu là đã thanh toán
                    dONDATHANG.TinhTrangGiaoHang = int.Parse(f["nTinhTrangGiaoHang"]); // Ví dụ: Tình trạng giao hàng
                    dONDATHANG.NgayDat = Convert.ToDateTime(f["dNgayDat"]);
                    dONDATHANG.NgayGiao = Convert.ToDateTime(f["dNgayGiao"]);

                    db.DONDATHANGs.Add(dONDATHANG);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                // ModelState không hợp lệ, trả về View với dONDATHANG để hiển thị lỗi
                return View(dONDATHANG);
            }
        }
        public ActionResult Edit(int id)
        {
            var dONDATHANG = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dONDATHANG == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen");

            return View(dONDATHANG);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, DONDATHANG updatedDonDatHang)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Find the existing customer by ID
                var existingDonDatHang = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);

                if (existingDonDatHang == null)
                {
                    return HttpNotFound();
                }

                // Update the existing customer's properties
                existingDonDatHang.MaDonHang = updatedDonDatHang.MaDonHang;
                existingDonDatHang.DaThanhToan = updatedDonDatHang.DaThanhToan;
                existingDonDatHang.TinhTrangGiaoHang = updatedDonDatHang.TinhTrangGiaoHang;
                existingDonDatHang.NgayDat = updatedDonDatHang.NgayDat;
                existingDonDatHang.NgayGiao = updatedDonDatHang.NgayGiao;
                existingDonDatHang.MaKH = updatedDonDatHang.MaKH;


                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                // If the model state is not valid, return the view with validation errors
                return View(updatedDonDatHang);
            }
        }

        public ActionResult Delete(int id)
        {
            var dONDATHANG = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dONDATHANG == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dONDATHANG);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var dONDATHANG = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dONDATHANG == null)
            {
                Response.StatusCode = 404;
                return null;
            }


            db.DONDATHANGs.Remove(dONDATHANG);
            db.SaveChanges();

            return RedirectToAction("Index");


        }
        public ActionResult Details(int id)
        {
            var dONDATHANG = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dONDATHANG == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dONDATHANG);
        }

    }
}
