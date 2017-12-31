using System;
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

        public ActionResult Index(int ? page)
        {
            int pageSize =5;
            int pageNum = (page ?? 1);
            var ruoumoi = Layruoumoi(9);
            return View(ruoumoi.ToPagedList(pageNum,pageSize));
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

        
    }
}