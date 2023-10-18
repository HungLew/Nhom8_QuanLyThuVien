using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebThuVien.Models;

namespace WebThuVien.Controllers
{
    public class RegisterController : Controller
    {
        ThuVien1Entities3 db = new ThuVien1Entities3();
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        // Controller Action Method
        public JsonResult SaveData(TheThuVien model)
        {
            try
            {
                // Perform validation and save data to the database here
                // Ensure that the model properties match the form field names

                // Example:
                var x = model.HoTenSinhVien;
                var y = model.TenDN;
                var z = model.MatKhau;

                model.NgayHetHan = DateTime.Now;
                model.TinhTrangSuDung = "Hết";
                db.TheThuViens.Add(model);
                db.SaveChanges();
                return Json("Đăng ký thành công", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Đăng ký không thành công! ", JsonRequestBehavior.AllowGet);
            }
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
                return Json("Success", JsonRequestBehavior.AllowGet);
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