﻿nayusing System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRuou.Models;
using PagedList;
using PagedList.Mvc;


namespace WebRuou.Controllers
{
    public class WineController : Controller
    {
        dbQLBanruouDataContext data = new dbQLBanruouDataContext();
        private List<ruou> Layruoumoi(int count)
        {
            return data.ruous.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var ruoumoi = Layruoumoi(9);
            return View(ruoumoi.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Loai()
        {
            var loai = from lo in data.loais select lo;
            return PartialView(loai);
        }
        public ActionResult NhaCungCap()
        {
            var nhacungcap = from cd in data.nhacungcaps select cd;
            return PartialView(nhacungcap);
        }

        public ActionResult SPTheoLoai(int id)
        {
            var ruou = from s in data.ruous where s.Maloai == id select s;
            return View(ruou);
        }

        public ActionResult SPTheoNCC(int id)
        {
            var ruou = from s in data.ruous where s.MaNCC == id select s;
            return View(ruou);
        }

        public ActionResult Details(int id)
        {
            var ruou = from s in data.ruous
                       where s.Maruou == id
                       select s;
            return View(ruou.Single());
        }

        public ActionResult Lienhe()
        {
            return View();
        }
    }
}
