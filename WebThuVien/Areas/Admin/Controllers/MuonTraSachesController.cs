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
    public class MuonTraSachesController : BaseController
    {
        private ThuVien1Entities3 db = new ThuVien1Entities3();

        // GET: Admin/MuonTraSaches
        public ActionResult Index(string filter)
        {
            var muonTraSaches = db.MuonTraSaches.Include(m => m.DauSach).Include(m => m.TheThuVien);

            if (!string.IsNullOrEmpty(filter))
            {
                muonTraSaches = muonTraSaches.Where(m => m.TinhTrang == filter);
            }

            return View(muonTraSaches.ToList());
        }

        // GET: Admin/MuonTraSaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MuonTraSach muonTraSach = db.MuonTraSaches.Find(id);
            if (muonTraSach == null)
            {
                return HttpNotFound();
            }
            return View(muonTraSach);
        }

        // GET: Admin/MuonTraSaches/Create
        public ActionResult Create()
        {
            ViewBag.SoThe = new SelectList(db.TheThuViens, "SoThe", "SoThe");
            var sachCoSan = db.DauSaches.Where(p => p.TinhTrang == "Còn").ToList();
            ViewBag.MaDauSach = new SelectList(sachCoSan, "MaDauSach", "SoKiemSoat");
            
            return View();
        }

        // POST: Admin/MuonTraSaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaMuonTra,SoThe,MaDauSach,NgayMuon,NgayTraDuKien,TinhTrang")] MuonTraSach muonTraSach)
        {
            if (ModelState.IsValid)
            {
                muonTraSach.NgayMuon = DateTime.Now;
                muonTraSach.NgayTraDuKien = DateTime.Now.AddDays(15);

                // Kiểm tra số lượng sách mượn của người đó
                int soLuongSachMuon = db.MuonTraSaches
                    .Count(m => m.SoThe == muonTraSach.SoThe && m.TinhTrang == "Đang mượn");

                // Kiểm tra trạng thái mượn trả của người đó
                bool coSachQuaHan = db.MuonTraSaches
                    .Any(m => m.SoThe == muonTraSach.SoThe && m.TinhTrang == "Quá hạn");

                if (soLuongSachMuon < 3 && !coSachQuaHan)
                {
                    // Lọc danh sách đầu sách có tình trạng là "Còn"
                    var danhSachDauSach = db.DauSaches.Where(ds => ds.TinhTrang == "Còn").ToList();

                    // Đưa danh sách đầu sách vào ViewBag để hiển thị trong dropdown
                    ViewBag.MaDauSach = new SelectList(danhSachDauSach, "MaDauSach", "SoKiemSoat", muonTraSach.MaDauSach);

                    // Cập nhật tình trạng mượn trả thành "Đang mượn"
                    muonTraSach.TinhTrang = "Đang mượn";

                    // Thêm mượn trả sách vào cơ sở dữ liệu
                    db.MuonTraSaches.Add(muonTraSach);

                    // Cập nhật trạng thái của đầu sách thành "Đang được mượn"
                    DauSach dauSach = db.DauSaches.Find(muonTraSach.MaDauSach);
                    if (dauSach != null)
                    {
                        dauSach.TinhTrang = "Đang được mượn";
                        db.Entry(dauSach).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (soLuongSachMuon >= 3)
                {
                    ModelState.AddModelError(string.Empty, "Mỗi người chỉ được mượn tối đa 3 sách.");
                }
                else if (coSachQuaHan)
                {
                    ModelState.AddModelError(string.Empty, "Người mượn này hiện chưa trả sách quá hạn.");
                }
            }

            ViewBag.SoThe = new SelectList(db.TheThuViens, "SoThe", "HoTenSinhVien", muonTraSach.SoThe);

            // Lọc danh sách đầu sách có tình trạng là "Còn"
            var sachCoSan = db.DauSaches.Where(p => p.TinhTrang == "Còn").ToList();
            ViewBag.MaDauSach = new SelectList(sachCoSan, "MaDauSach", "SoKiemSoat");

            return View(muonTraSach);
        }


        // GET: Admin/MuonTraSaches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MuonTraSach muonTraSach = db.MuonTraSaches.Find(id);
            if (muonTraSach == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDauSach = new SelectList(db.DauSaches, "MaDauSach", "SoKiemSoat", muonTraSach.MaDauSach);
            ViewBag.SoThe = new SelectList(db.TheThuViens, "SoThe", "SoThe", muonTraSach.SoThe);
            return View(muonTraSach);
        }

        // POST: Admin/MuonTraSaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaMuonTra,SoThe,MaDauSach,NgayMuon,NgayTraDuKien,TinhTrang")] MuonTraSach muonTraSach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(muonTraSach).State = EntityState.Modified;

                DateTime ngayHienTai = DateTime.Now;

                // Kiểm tra nếu ngày thực tế vượt qua ngày trả dự kiến
                if (muonTraSach.NgayTraDuKien < ngayHienTai && muonTraSach.TinhTrang == "Đang mượn")
                {
                    muonTraSach.TinhTrang = "Quá hạn";
                }

                // Cập nhật trạng thái đầu sách tương ứng
                DauSach dauSach = db.DauSaches.Find(muonTraSach.MaDauSach);
                if (dauSach != null)
                {
                    if (muonTraSach.TinhTrang == "Đang mượn" || muonTraSach.TinhTrang == "Quá hạn")
                    {
                        dauSach.TinhTrang = "Đang được mượn";
                    }
                    else if (muonTraSach.TinhTrang == "Đã trả sách")
                    {
                        dauSach.TinhTrang = "Còn";
                    }
                    db.Entry(dauSach).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDauSach = new SelectList(db.DauSaches, "MaDauSach", "SoKiemSoat", muonTraSach.MaDauSach);
            ViewBag.SoThe = new SelectList(db.TheThuViens, "SoThe", "HoTenSinhVien", muonTraSach.SoThe);
            return View(muonTraSach);
        }

        // GET: Admin/MuonTraSaches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MuonTraSach muonTraSach = db.MuonTraSaches.Find(id);
            if (muonTraSach == null)
            {
                return HttpNotFound();
            }
            return View(muonTraSach);
        }

        // POST: Admin/MuonTraSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MuonTraSach muonTraSach = db.MuonTraSaches.Find(id);
            db.MuonTraSaches.Remove(muonTraSach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult XacNhanTraSach(int id)
        {
            MuonTraSach muonTraSach = db.MuonTraSaches.Find(id);
            if (muonTraSach != null && muonTraSach.TinhTrang != "Đã trả sách")
            {
                muonTraSach.TinhTrang = "Đã trả sách";
                db.Entry(muonTraSach).State = EntityState.Modified;
                db.SaveChanges();

                // Cập nhật trạng thái của đầu sách thành "Còn"
                DauSach dauSach = db.DauSaches.Find(muonTraSach.MaDauSach);
                if (dauSach != null)
                {
                    dauSach.TinhTrang = "Còn";
                    db.Entry(dauSach).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
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
