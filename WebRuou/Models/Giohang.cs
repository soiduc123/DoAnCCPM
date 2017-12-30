using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRuou.Models;

namespace WebRuou.Models
{
    public class Giohang
    {
        dbQLBanruouDataContext data = new dbQLBanruouDataContext();
        public int iMaruou { set; get; }
        public string sTenruou { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        public Giohang(int Maruou)
        {
            iMaruou = Maruou;
            ruou sach = data.ruous.Single(n => n.Maruou == iMaruou);
            sTenruou = sach.Tenruou;
            sAnhbia = sach.Anhbia;
            dDongia = double.Parse(sach.Giaban.ToString());
            iSoluong = 1;
        }
    }
}