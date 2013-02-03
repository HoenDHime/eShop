using E_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_shop.Service
{
    public class ViewOrderProfit
    {
        private readonly IOrderRepository orderRepository;
        public ViewOrderProfit(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public decimal? ProfitFromThePeriod(string Filters)
        {
            DateTime today = DateTime.Today;
            var orders = orderRepository.List();
            decimal? profit = 0.0m;

            switch (Filters)
            {
                case "4": //прибыли с периода - прошедший месяц
                    var TodayMonth = today.Month - 1;
                    var TodayYear = today.Year;

                    if (TodayMonth == 0)
                    {
                        TodayMonth = 12;
                        TodayYear = today.Year - 1;
                    }
                    var LeftBorder = new DateTime(TodayYear, TodayMonth, 1);
                    var RightBorder = new DateTime(TodayYear, TodayMonth, DateTime.DaysInMonth(TodayYear, TodayMonth));

                    foreach (var i in orders)
                    {
                        var LB = DateTime.Compare(LeftBorder, (DateTime)i.Date);
                        var RB = DateTime.Compare(RightBorder, (DateTime)i.Date);
                        if (LB == -1 && RB == 1)
                        {
                            if (profit == null)
                                profit = i.OrderItems.Select(x => x.Profit.Value).Sum();
                            else
                                profit += i.OrderItems.Select(x => x.Profit.Value).Sum();
                        }
                    }
                    break;

                case "5": //прибыли с периода - текущий месяц
                    foreach (var i in orders)
                    {
                        var LeftBorderToday = new DateTime(today.Year, today.Month, 1);
                        var LBT = DateTime.Compare(LeftBorderToday, (DateTime)i.Date);
                        var RightBorderToday = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
                        var RBT = DateTime.Compare(RightBorderToday, (DateTime)i.Date);

                        if (LBT == -1 && RBT == 1)
                        {
                            if (profit == null)
                                profit = i.OrderItems.Select(x => x.Profit.Value).Sum();
                            else
                                profit += i.OrderItems.Select(x => x.Profit.Value).Sum();
                        }
                    }

                    break;

                case "6": //прибыли с периода - текущий год
                    foreach (var i in orders)
                    {
                        var LeftBorderYear = new DateTime(today.Year, 1, 1);
                        var LBY = DateTime.Compare(LeftBorderYear, (DateTime)i.Date);
                        var RightBorderYear = new DateTime(today.Year, 12, DateTime.DaysInMonth(today.Year, 12));
                        var RBY = DateTime.Compare(RightBorderYear, (DateTime)i.Date);

                        if (LBY == -1 && RBY == 1)
                        {
                            if (profit == null)
                                profit = i.OrderItems.Select(x => x.Profit.Value).Sum();
                            else
                                profit += i.OrderItems.Select(x => x.Profit.Value).Sum();
                        }
                    }
                    break;
            }
            return profit;
        }  
    }
}