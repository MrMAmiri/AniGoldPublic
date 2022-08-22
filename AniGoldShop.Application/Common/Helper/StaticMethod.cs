using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application.Common.Helper
{
    public class Pager
    {
        public int Index { get; set; }
        public bool isActive { get; set; }
    }
    public static class StaticMethods
    {

        public static readonly string AdminRole = "EA49ECB3-4D67-4D8F-9A6E-98C1FB9836F3";
        public static readonly string AgentRole = "DFC284EB-381E-40C2-8BE8-3FE880388B36";
        public static readonly string CustomerRole = "84A8B08C-8BC0-4DFF-B5B8-6E60BA96A633";
        public static List<Pager> CreatePager(int cnt, int pgindex, int pgsize)
        {
            try
            {
                double totalpagecount = (double)((decimal)cnt / pgsize);
                int pagecount = (int)Math.Ceiling(totalpagecount);

                List<Pager> pg = new List<Pager>();
                for (int i = 1; i <= pagecount; i++)
                {
                    pg.Add(new Pager() { Index = i });
                }

                if (pg != null && pg.Count > 0)
                    if (pgindex <= pg.Max(s => s.Index))
                        pg.FirstOrDefault(s => s.Index == pgindex).isActive = true;

                return pg;
            }
            catch
            {
                return new List<Pager>();
            }
        }

        public static int DartToNotNull(this int? val, int value = 0)
        {
            if (val == null)
                return value;
            else
                return val.Value;
        }

        public static string GetOrderStatusText(int val)
        {
            switch (val)
            {
                case 0:
                    return "پیش فاکتور";
                case 1:
                    return "منتظر برنامه ریزی";
                case 2:
                    return "منتظر تولید";
                case 3:
                    return "درحال تولید";
                case 4:
                    return "تولید شده";
                case 5:
                    return "ارسال به انبار";
                case 6:
                    return "ارسال به مشتری";
                case 7:
                    return "";
                case 8:
                    return "کنسل شده";
                case 9:
                    return "انبار شده";
            }

            return "";
        }

        public static string[] ImageTypes => new[] { "jpg", "jpeg", "bmp", "gif", "png", "tif", "svg", "webp" };
        public static string[] VideoTypes => new[] { "mp4", "mov", "wmv", "avi", "webm", "mkv", "flv" };

        public static string GetFactorStatus(int? st)
        {
            if (st == null)
                return "";

            switch (st)
            {
                case 0:
                    return "جدید";
                case 1:
                    return "تایید شده";
                case 2:
                    return "ارسال شده";
                case 3:
                    return "تحویل شده";
                case 4:
                    return "مرجوع شده";
                case 5:
                    return "";
                case 100:
                    return "حذف شده";
                default:
                    return "نامشخص";
            }

        }
    }


}
