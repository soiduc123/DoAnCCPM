using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRuou.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity;

namespace WebRuou.Controllers
{
    public class AdminController : Controller
    {
        dbQLBanruouDataContext db = new dbQLBanruouDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ruou(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.ruous.ToList());
            return View(db.ruous.ToList().OrderBy(n => n.Maruou).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {

                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiRuou()
        {
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.Maloai = new SelectList(db.loais.ToList().OrderBy(n => n.Tenloai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.nhacungcaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiRuou(ruou ruou, HttpPostedFileBase fileUpload)
        {
            {
                //Dua du lieu vao dropdownload
                ViewBag.Maloai = new SelectList(db.loais.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");
                ViewBag.MaNCC = new SelectList(db.nhacungcaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/image"), fileName);
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);
                        }
                        ruou.Anhbia = fileName;
                        //Luu vao CSDL
                        db.ruous.InsertOnSubmit(ruou);
                        db.SubmitChanges();
                    }
                    return RedirectToAction("ruou");
                }
            }
        }
        [HttpGet]
        public ActionResult Suaruou(int id)
        {
            //Lay ra doi tuong sach theo ma
            ruou ruou = db.ruous.SingleOrDefault(n => n.Maruou == id);
            ViewBag.Maruou = ruou.Maruou;
            if (ruou == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.Maloai = new SelectList(db.loais.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai", ruou.Maloai);
            ViewBag.MaNCC = new SelectList(db.nhacungcaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", ruou.MaNCC);
            return View(ruou);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suaruou(ruou ruou, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.Maloai = new SelectList(db.loais.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");
            ViewBag.MaNCC = new SelectList(db.nhacungcaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    ruou.Anhbia = fileName;
                    //Luu vao CSDL   
                    ruou Ruou = db.ruous.Single(p => p.Maruou == ruou.Maruou);
                    Ruou.Tenruou = ruou.Tenruou;
                    Ruou.Giaban = ruou.Giaban;
                    Ruou.Mota = ruou.Mota;
                    Ruou.Anhbia = ruou.Anhbia;
                    Ruou.Ngaycapnhat = ruou.Ngaycapnhat;
                    Ruou.Soluongton = ruou.Soluongton;
                    Ruou.Maloai = ruou.Maloai;
                    Ruou.MaNCC = ruou.MaNCC;
                    db.SubmitChanges();

                 //   UpdateModel(ruou);
                 //   db.SubmitChanges();

                }
                return RedirectToAction("ruou");
            }
        }
        
        public ActionResult Chitietruou(int id)
        {
            //Lay ra doi tuong sach theo ma
            ruou sach = db.ruous.SingleOrDefault(n => n.Maruou == id);
            ViewBag.Maruou = sach.Maruou;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        
        [HttpGet]
        public ActionResult Xoaruou(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            ruou ruou = db.ruous.SingleOrDefault(n => n.Maruou == id);
            ViewBag.Maruou = ruou.Maruou;
            if (ruou == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ruou);
        }
        [HttpPost, ActionName("Xoaruou")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            ruou ruou = db.ruous.SingleOrDefault(n => n.Maruou == id);
            ViewBag.Maruou = ruou.Maruou;
            if (ruou == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.ruous.DeleteOnSubmit(ruou);
            db.SubmitChanges();
            return RedirectToAction("ruou");
        }

        public ActionResult Nhacungcap(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.nhacungcaps.ToList().OrderBy(n => n.MaNCC).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Themmoinhacungcap()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoinhacungcap(nhacungcap nhacungcap)
        {
            db.nhacungcaps.InsertOnSubmit(nhacungcap);
            db.SubmitChanges();
            return RedirectToAction("Nhacungcap");

        }
        [HttpGet]
        public ActionResult Xoanhacungcap(int id)
        {
            nhacungcap nhacungcap = db.nhacungcaps.SingleOrDefault(n => n.MaNCC == id);
            ViewBag.MaNCC = nhacungcap.MaNCC;
            if (nhacungcap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nhacungcap);
        }
        [HttpPost, ActionName("Xoanhacungcap")]
        public ActionResult Xacnhanxoanhacungcap(int id)
        {
            nhacungcap nhacungcap = db.nhacungcaps.SingleOrDefault(n => n.MaNCC == id);
            ViewBag.MaTH = nhacungcap.MaNCC;
            if (nhacungcap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.nhacungcaps.DeleteOnSubmit(nhacungcap);
            db.SubmitChanges();
            return RedirectToAction("Nhacungcap");
        }

        [HttpGet]
        public ActionResult Suanhacungcap(int id)
        {
            nhacungcap thuonghieu = db.nhacungcaps.SingleOrDefault(n => n.MaNCC == id);
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thuonghieu);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suanhacungcap(nhacungcap thuonghieu, HttpPostedFileBase anhSanPham)
        {
            //var filename = Path.GetFileName(fileupload.FileName);
            //var path = Path.Combine(Server.MapPath("~/images/"), filename);

            //db.SubmitChanges();
            //db.THUONGHIEUs.InsertOnSubmit(thuonghieu);

            nhacungcap Thuonghieu = db.nhacungcaps.Single(p => p.MaNCC == thuonghieu.MaNCC);
            Thuonghieu.TenNCC = thuonghieu.TenNCC;
            Thuonghieu.Diachi = thuonghieu.Diachi;
            Thuonghieu.DienThoai = thuonghieu.DienThoai;
            db.SubmitChanges();
            return RedirectToAction("Nhacungcap");


        }
        public ActionResult Loairuou(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.loais.ToList().OrderBy(n => n.Maloai).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Themmoiloairuou()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoiloairuou(loai loai)
        {
            db.loais.InsertOnSubmit(loai);
            db.SubmitChanges();
            return RedirectToAction("Loairuou");

        }
        [HttpGet]
        public ActionResult Xoaloairuou(int id)
        {
            loai loairuou = db.loais.SingleOrDefault(n => n.Maloai == id);
            ViewBag.MaTH = loairuou.Maloai;
            if (loairuou == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loairuou);
        }
        [HttpPost, ActionName("Xoaloairuou")]
        public ActionResult Xacnhanxoaloairuou(int id)
        {
            loai loai = db.loais.SingleOrDefault(n => n.Maloai == id);
            ViewBag.Maloai = loai.Maloai;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.loais.DeleteOnSubmit(loai);
            db.SubmitChanges();
            return RedirectToAction("Loairuou");
        }
        [HttpGet]
        public ActionResult Sualoairuou(int id)
        {
            loai loai = db.loais.SingleOrDefault(n => n.Maloai == id);
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sualoairuou(loai thuonghieu)
        {
            //  UpdateModel(thuonghieu);
            //   db.SubmitChanges();
            //   return RedirectToAction("thuonghieu");

            loai Thuonghieu = db.loais.Single(p => p.Maloai == thuonghieu.Maloai);
            Thuonghieu.Tenloai = thuonghieu.Tenloai;
            db.SubmitChanges();
            return RedirectToAction("Loairuou");
        }
        public ActionResult Khachhang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.Khachhangs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Themmoikhachhang()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoikhachhang(Khachhang khachhang)
        {
            db.Khachhangs.InsertOnSubmit(khachhang);
            db.SubmitChanges();
            return RedirectToAction("Khachhang");

        }
        [HttpGet]
        public ActionResult Xoakhachhang(int id)
        {
            Khachhang khachhang = db.Khachhangs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaTH = khachhang.MaKH;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }
        [HttpPost, ActionName("Xoakhachhang")]
        public ActionResult Xacnhanxoakhachhang(int id)
        {
            Khachhang khachhang = db.Khachhangs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.Maloai = khachhang.MaKH;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Khachhangs.DeleteOnSubmit(khachhang);
            db.SubmitChanges();
            return RedirectToAction("Khachhang");
        }
        [HttpGet]
        public ActionResult Suakhachhang(int id)
        {
            Khachhang khachhang = db.Khachhangs.SingleOrDefault(n => n.MaKH == id);
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suakhachhang(Khachhang thuonghieu)
        {
            //  UpdateModel(thuonghieu);
            //   db.SubmitChanges();
            //   return RedirectToAction("thuonghieu");

            Khachhang Thuonghieu = db.Khachhangs.Single(p => p.MaKH == thuonghieu.MaKH);
            Thuonghieu.HoTen = thuonghieu.HoTen;
            Thuonghieu.Taikhoan = thuonghieu.Taikhoan;
            Thuonghieu.Matkhau = thuonghieu.Matkhau;
            Thuonghieu.Email = thuonghieu.Email;
            Thuonghieu.DiachiKH = thuonghieu.DiachiKH;
            Thuonghieu.DienthoaiKH = thuonghieu.DienthoaiKH;
            Thuonghieu.Ngaysinh = thuonghieu.Ngaysinh;
            db.SubmitChanges();
            return RedirectToAction("Khachhang");
        }
    }
}
    
