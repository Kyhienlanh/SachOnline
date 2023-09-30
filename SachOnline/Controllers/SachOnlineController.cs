using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
      
        /// <summary>
        /// GetChuDe 
        /// </summary>
        /// <returns></returns>
        private BookOnlineEntities db=new BookOnlineEntities();

        private List<SACH> LaySachMoi(int count)
        {
            return db.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

      
        public ActionResult Index()
        {
            var listSachMoi = LaySachMoi(6);
            return View(listSachMoi);

        }

        public ActionResult ChuDePartial()
        {
            var listchude = db.CHUDEs;
            return PartialView(listchude);
        }
        /// <summary>
        /// GetNhaXuatBan
        /// </summary>
        /// <returns></returns>
        public ActionResult NhaXuatBanPartial()
        {
            var listnxb = db.NHAXUATBANs;
            return PartialView(listnxb);
        }
        /// <summary>
        /// GetSlider
        /// </summary>
        /// <returns></returns>
        public ActionResult SliderPartial()
        {
            return PartialView();
        }
        /// <summary>
        /// GetSachBanNhieu
        /// </summary>
        /// <returns></returns>
        /// 

        private List<SACH> LaySachbannhieu(int count)
        {
            return db.SACHes.OrderByDescending(a => a.SoLuongBan).Take(count).ToList();
        }
        public ActionResult SachBanNhieuPartial()
        {
            var listsachbannhieu = LaySachbannhieu(6);
            return PartialView(listsachbannhieu);
           
        }
        public ActionResult FooterPartial()
        {
            return PartialView();
        }
        public ActionResult NavPartial()
        {
            return PartialView();
        }
    }
}