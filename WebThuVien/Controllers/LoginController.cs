using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebThuVien.Models;

namespace WebThuVien.Controllers
{
    public class LoginController : Controller
    {
        ThuVien1Entities3 db = new ThuVien1Entities3();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult CheckValidUser(TheThuVien model, Admin admin)
        {
            string result = "Fail";
            var z = model.TenDN;
            var y = model.MatKhau;
            if (model.TenDN == "" || model.MatKhau == "")
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var DataItem = db.TheThuViens.Where(x => x.TenDN == model.TenDN && x.MatKhau == model.MatKhau).SingleOrDefault();
                if (DataItem != null)
                {
                    Session["Name"] = DataItem.HoTenSinhVien.ToString();
                    result = "Success";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            /*var DataItem = db.TheThuViens.Where(x => x.TenDN == model.TenDN && x.MatKhau == model.MatKhau).SingleOrDefault();
            if (DataItem != null)
            {
                Session["Name"] = DataItem.HoTenSinhVien.ToString();
                result = "Thành công";
            }
            return Json(result, JsonRequestBehavior.AllowGet);*/
        }
    }
}