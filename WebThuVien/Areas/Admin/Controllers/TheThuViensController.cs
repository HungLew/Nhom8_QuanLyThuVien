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
    public class TheThuViensController : BaseController
    {
        private ThuVien1Entities3 db = new ThuVien1Entities3();

        // GET: Admin/TheThuViens
        public ActionResult Index()
        {
            return View(db.TheThuViens.ToList());
        }

        // GET: Admin/TheThuViens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheThuVien theThuVien = db.TheThuViens.Find(id);
            if (theThuVien == null)
            {
                return HttpNotFound();
            }
            return View(theThuVien);
        }

        // GET: Admin/TheThuViens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TheThuViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoThe,HoTenSinhVien,TenDN,MatKhau,NgayHetHan,TinhTrangSuDung")] TheThuVien theThuVien)
        {
            if (ModelState.IsValid)
            {
                DateTime ngayTaoThe = DateTime.Now;
                theThuVien.NgayHetHan = ngayTaoThe.AddYears(3);
                theThuVien.TinhTrangSuDung = "Còn hạn";
                db.TheThuViens.Add(theThuVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(theThuVien);
        }

        // GET: Admin/TheThuViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheThuVien theThuVien = db.TheThuViens.Find(id);
            if (theThuVien == null)
            {
                return HttpNotFound();
            }
            return View(theThuVien);
        }

        // POST: Admin/TheThuViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoThe,HoTenSinhVien,TenDN,MatKhau,NgayHetHan,TinhTrangSuDung")] TheThuVien theThuVien)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra ngày thực tế
                if (DateTime.Now <= theThuVien.NgayHetHan)
                {
                    theThuVien.TinhTrangSuDung = "Còn hạn";
                }
                else
                {
                    theThuVien.TinhTrangSuDung = "Hết hạn";
                }

                db.Entry(theThuVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(theThuVien);
        }

        // GET: Admin/TheThuViens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheThuVien theThuVien = db.TheThuViens.Find(id);
            if (theThuVien == null)
            {
                return HttpNotFound();
            }
            return View(theThuVien);
        }

        // POST: Admin/TheThuViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TheThuVien theThuVien = db.TheThuViens.Find(id);
            db.TheThuViens.Remove(theThuVien);
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
