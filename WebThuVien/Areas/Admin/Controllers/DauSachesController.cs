using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebThuVien.Models;

namespace WebThuVien.Areas.Admin.Controllers
{
    public class DauSachesController : BaseController
    {
        private ThuVien1Entities3 db = new ThuVien1Entities3();

        // GET: Admin/DauSaches
        public ActionResult Index()
        {
            var dauSaches = db.DauSaches.Include(d => d.Sach);
            return View(dauSaches.ToList());
        }

        // GET: Admin/DauSaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DauSach dauSach = db.DauSaches.Find(id);
            if (dauSach == null)
            {
                return HttpNotFound();
            }
            return View(dauSach);
        }

        // GET: Admin/DauSaches/Create
        public ActionResult Create()
        {
            ViewBag.ID_Sach = new SelectList(db.Saches, "ID_Sach", "TenSach");
            return View();
        }

        // POST: Admin/DauSaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDauSach,ID_Sach,SoKiemSoat,TinhTrang,GhiChu")] DauSach dauSach)
        {
            if (ModelState.IsValid)
            {
                dauSach.TinhTrang = "Còn";
                db.DauSaches.Add(dauSach);
                db.SaveChanges();

                int? idSach = dauSach.ID_Sach; // Lấy ID_Sach sau khi tạo
                return RedirectToAction("Edit", "SachAdmin11s", new { id = idSach });
            }

            ViewBag.ID_Sach = new SelectList(db.Saches, "ID_Sach", "TenSach", dauSach.ID_Sach);
            return View(dauSach);
        }

        // GET: Admin/DauSaches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DauSach dauSach = db.DauSaches.Find(id);
            if (dauSach == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Sach = new SelectList(db.Saches, "ID_Sach", "TenSach", dauSach.ID_Sach);
            return View(dauSach);
        }

        // POST: Admin/DauSaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDauSach,ID_Sach,SoKiemSoat,TinhTrang,GhiChu")] DauSach dauSach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dauSach).State = EntityState.Modified;
                db.SaveChanges();

                int? idSach = dauSach.ID_Sach; // Lấy ID_Sach sau khi chỉnh sửa
                return RedirectToAction("Edit", "SachAdmin11s", new { id = idSach });
            }

            ViewBag.ID_Sach = new SelectList(db.Saches, "ID_Sach", "TenSach", dauSach.ID_Sach);
            return View(dauSach);
        }

        // GET: Admin/DauSaches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DauSach dauSach = db.DauSaches.Find(id);
            if (dauSach == null)
            {
                return HttpNotFound();
            }
            return View(dauSach);
        }

        // POST: Admin/DauSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DauSach dauSach = db.DauSaches.Find(id);
            int? idSach = dauSach.ID_Sach; // Lấy ID_Sach trước khi xóa
            db.DauSaches.Remove(dauSach);
            db.SaveChanges();

            // Chuyển hướng đến trang chỉnh sửa trong SachAdmin11sController với ID_Sach đã lấy
            return RedirectToAction("Edit", "SachAdmin11s", new { id = idSach });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
