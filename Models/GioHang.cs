using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBookStore.Models
{
    public class GioHang
    {
        private QLBANSACHEntities db = new QLBANSACHEntities();
        public int iMasach { get; set; } 
        public string sTensach { get; set; } 
        public string sAnhbia { get; set; } 
        public Double dDongia { get; set; } 
        public int iSoluong { get; set; } = 1; 
        public double dThanhtien
        {
            get { return iSoluong * dDongia; } 
        }

        public GioHang(int Masach)
        {
            iMasach = Masach;

            SACH sach = db.SACH.Single(n => n.Masach == Masach);

            sTensach = sach.Tensach;
            sAnhbia = sach.Hinhminhhoa;
            dDongia = double.Parse(sach.Dongia.ToString());
            iSoluong = 1;
        }
    }
}