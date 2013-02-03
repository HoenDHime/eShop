using E_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_shop.Service
{
    public class ViewOrders
    {
        public IEnumerable<Order> Period(string Filters, IEnumerable<Order> dateProduct)
        {
            DateTime today = DateTime.Today;
            List<Order> result = new List<Order>();

            switch (Filters)
            {
                case "1": //периоду - прошедший месяц
                    var TodayMonth = today.Month - 1;
                    var TodayYear = today.Year;

                    if (TodayMonth == 0)
                    {
                        TodayMonth = 12;
                        TodayYear = today.Year - 1;
                    }
                    var LeftBorder = new DateTime(TodayYear, TodayMonth, 1);
                    var RightBorder = new DateTime(TodayYear, TodayMonth, DateTime.DaysInMonth(TodayYear, TodayMonth));

                    foreach (var i in dateProduct)
                    {
                        var LB = DateTime.Compare(LeftBorder, (DateTime)i.Date);
                        var RB = DateTime.Compare(RightBorder, (DateTime)i.Date);
                        if (LB == -1 && RB == 1) result.Add(i);
                    }
                    break;

                case "2": //периоду - текущий месяц
                    foreach (var i in dateProduct)
                    {
                        var LeftBorderToday = new DateTime(today.Year, today.Month, 1);
                        var LBT = DateTime.Compare(LeftBorderToday, (DateTime)i.Date);
                        var RightBorderToday = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
                        var RBT = DateTime.Compare(RightBorderToday, (DateTime)i.Date);

                        if (LBT == -1 && RBT == 1) result.Add(i);
                    }
                    break;

                case "3": //периоду - текущий год
                    foreach (var i in dateProduct)
                    {
                        var LeftBorderYear = new DateTime(today.Year, 1, 1);
                        var LBY = DateTime.Compare(LeftBorderYear, (DateTime)i.Date);
                        var RightBorderYear = new DateTime(today.Year, 12, DateTime.DaysInMonth(today.Year, 12));
                        var RBY = DateTime.Compare(RightBorderYear, (DateTime)i.Date);

                        if (LBY == -1 && RBY == 1) result.Add(i);
                    }
                    break;
            }

            return result;
        }
    }
}