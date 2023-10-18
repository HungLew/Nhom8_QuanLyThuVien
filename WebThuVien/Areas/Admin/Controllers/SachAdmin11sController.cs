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
    public class SachAdmin11sController : BaseController
    {
        private ThuVien1Entities3 db = new ThuVien1Entities3();

        // GET: Admin/SachAdmin11s
        public ActionResult Index(string searchText)
        {
            var saches = db.Saches.Include(s => s.NhaXuatBan).Include(s => s.TacGia).Include(s => s.TheLoai);

            if (!string.IsNullOrEmpty(searchText))
            {
                saches = saches.Where(s => s.TenSach.Contains(searchText));
            }

            return View(saches.ToList());
        }

        // GET: Admin/SachAdmin11s/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Include(s => s.DauSaches).FirstOrDefault(s => s.ID_Sach == id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // GET: Admin/SachAdmin11s/Create
        public ActionResult Create()
        {
            ViewBag.ID_NXB = new SelectList(db.NhaXuatBans, "ID_NXB", "TenNXB");
            ViewBag.ID_TacGia = new SelectList(db.TacGias, "ID_TacGia", "HotenTG");
            ViewBag.ID_TheLoai = new SelectList(db.TheLoais, "ID_TheLoai", "TenTheLoai");
            return View();
        }

        // POST: Admin/SachAdmin11s/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Sach,TenSach,ImageSach,ID_TheLoai,ID_TacGia,ID_NXB,NamXuatBan,GioiThieu")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Saches.Add(sach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_NXB = new SelectList(db.NhaXuatBans, "ID_NXB", "TenNXB", sach.ID_NXB);
            ViewBag.ID_TacGia = new SelectList(db.TacGias, "ID_TacGia", "HotenTG", sach.ID_TacGia);
            ViewBag.ID_TheLoai = new SelectList(db.TheLoais, "ID_TheLoai", "TenTheLoai", sach.ID_TheLoai);
            return View(sach);
        }

        // GET: Admin/SachAdmin11s/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_NXB = new SelectList(db.NhaXuatBans, "ID_NXB", "TenNXB", sach.ID_NXB);
            ViewBag.ID_TacGia = new SelectList(db.TacGias, "ID_TacGia", "HotenTG", sach.ID_TacGia);
            ViewBag.ID_TheLoai = new SelectList(db.TheLoais, "ID_TheLoai", "TenTheLoai", sach.ID_TheLoai);
            return View(sach);
        }

        // POST: Admin/SachAdmin11s/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Sach,TenSach,ImageSach,ID_TheLoai,ID_TacGia,ID_NXB,NamXuatBan,GioiThieu")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_NXB = new SelectList(db.NhaXuatBans, "ID_NXB", "TenNXB", sach.ID_NXB);
            ViewBag.ID_TacGia = new SelectList(db.TacGias, "ID_TacGia", "HotenTG", sach.ID_TacGia);
            ViewBag.ID_TheLoai = new SelectList(db.TheLoais, "ID_TheLoai", "TenTheLoai", sach.ID_TheLoai);
            return View(sach);
        }

        // GET: Admin/SachAdmin11s/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: Admin/SachAdmin11s/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sach sach = db.Saches.Find(id);
            db.Saches.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Index");
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
