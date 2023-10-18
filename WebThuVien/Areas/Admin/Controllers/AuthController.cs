using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebThuVien.Controllers;
using WebThuVien.Models;

namespace WebThuVien.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        ThuVien1Entities3 db = new ThuVien1Entities3();
        // GET: Admin/Auth
        public ActionResult Login()
        {
            if (Session["Admin"] != null && !Session["Admin"].ToString().Equals(""))
            {
                return RedirectToAction("index", "homeadmin");
            }
            ViewBag.Error = "";
            return View();
        }

        //nơi nhận giá trị
        [HttpPost]
        public ActionResult Login(WebThuVien.Models.Admin acc, FormCollection form)
        {
            // hứng dữ liệu từ form
            acc.UserAdmin = form["username"];
            acc.PassAdmin = form["password"];

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(acc.UserAdmin))
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không tồn tại!");
                }
                if (string.IsNullOrEmpty(acc.PassAdmin))
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu không đúng!");
                }
                // check dữ liệu trong database và dữ liệu được hứng
                var checkAdmin = db.Admins.FirstOrDefault(s => s.UserAdmin == acc.UserAdmin && s.PassAdmin == acc.PassAdmin);
                if (checkAdmin != null)
                {
                    Session["Admin"] = checkAdmin.UserAdmin;
                    Session["RoleUser"] = checkAdmin.VaiTro;
                    return RedirectToAction("index", "SachAdmin11s");

                }

                // sai dữ liệu sẽ hiện thông báo
                else
                {
                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session["Admin"] = "";
            Session["RoleUser"] = "";
            return RedirectToAction("Index", "Saches");
        }
    }
}