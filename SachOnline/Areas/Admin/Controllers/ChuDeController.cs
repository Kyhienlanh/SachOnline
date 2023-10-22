using Microsoft.Ajax.Utilities;
using PagedList;
using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SachOnline.Areas.Admin.Controllers
{
    public class ChuDeController : Controller
    {
        // GET: Admin/ChuDe
        BookOnlineEntities db=new BookOnlineEntities();
        public ActionResult Index(int ? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;     
            return View(db.CHUDEs.ToList().OrderBy(n=> n.MaCD).ToPagedList(iPageNum, iPageSize));
        }

       
        [HttpGet]
        public ActionResult Create()
            {
                return View();
            }
        [HttpPost]
        public ActionResult Create(String Tenchude)
        {
                var s = new CHUDE();
                s.TenChuDe = Tenchude;
                db.CHUDEs.Add(s);
                db.SaveChanges();
                return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var Chude=db.CHUDEs.SingleOrDefault(n=>n.MaCD==id);
            if (Chude == null)
            {
                Response.Status = "404";
                return null;
            }

            return View(Chude);
        }
    }
}