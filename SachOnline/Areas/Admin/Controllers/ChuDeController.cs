using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace SachOnline.Areas.Admin.Controllers
{
    public class ChuDeController : Controller
    {
        BookOnlineEntities db = new BookOnlineEntities();
        // GET: Admin/ChuDe
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.CHUDEs.ToList().OrderBy(n => n.MaCD).ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            var data = db.CHUDEs;
            return View();
        }

        [HttpPost]
        public ActionResult Create(CHUDE chude)
        {
            if(ModelState.IsValid){
              
                db.CHUDEs.Add(chude);
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(chude);
        }



        [HttpGet]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(CHUDE cHUDE, HttpPostedFileBase fFileUpLoad, FormCollection f)
        {
            if (cHUDE == null)
            {
                return HttpNotFound(); // Handle when the record is not found
            }

            // Kiểm tra xem CHUDE có tồn tại trong cơ sở dữ liệu không
            var existingCHUDE = db.CHUDEs.SingleOrDefault(n => n.MaCD == cHUDE.MaCD);
            if (existingCHUDE == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            if (ModelState.IsValid)
            {
                if (fFileUpLoad != null)
                {
                    // Xử lý hình ảnh nếu cần
                    var sFileName = Path.GetFileName(fFileUpLoad.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    fFileUpLoad.SaveAs(path);
                    existingCHUDE.TenChuDe = f["sTenChuDe"];
                    // Cập nhật thông tin hình ảnh nếu cần
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                // Chuyển hướng đến trang Index sau khi cập nhật
                return RedirectToAction("Index");
            }

            return View(existingCHUDE);
        }




        [HttpGet]
        public ActionResult Delete(int id)
        {
            var cHUDE = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (cHUDE == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cHUDE);
        }



        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var cHUDE = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (cHUDE == null)
            {
                Response.StatusCode = 404;
                return null; // Handle the case when the record is not found
            }

            try
            {
                db.CHUDEs.Remove(cHUDE);
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
        public ActionResult Edit(int id)
        {
            var cHUDE = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (cHUDE == null)
            {
                Response.StatusCode = 404;
                return null;
            }


            return View(cHUDE);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, CHUDE updatedChuDe)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Find the existing customer by ID
                var cHUDE = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);

                if (cHUDE == null)
                {
                    return HttpNotFound();
                }

                // Update the existing customer's properties

                cHUDE.TenChuDe = updatedChuDe.TenChuDe;


                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                // If the model state is not valid, return the view with validation errors
                return View(updatedChuDe);
            }
        }


    }
}