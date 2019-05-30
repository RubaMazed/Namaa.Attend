using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Namaa.BioMetrics.Data;
using Namaa.BioMetrics.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace TestEquation
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int totalPrevVacation = (int)new DateTime(2019, 4, 14).Month - 1;
            double daydiscount = 0;
            double houDiscount = 0;
            double restHour = 0.0;
            using (var db = new ApplicationDbContext())
            {
                List<DailyVacation> takenVacation = db.DailyVacations.Where(c => c.IsActive && c.FromDate < DateTime.Now).Include("UserInfo").ToList();

                var dailys = (from d in db.DailyVacations
                              where d.UserInfo.CommunityCenterId == 1
                                    && d.FromDate >= new DateTime(2019, 4, 14) && d.FromDate <= new DateTime(2019, 4, 30)
                                    && d.UserInfo.EnrollNumber == "10"
                              group d by new
                              {
                                  Num = d.UserInfo.EnrollNumber,
                                  Name = d.UserInfo.FullName,
                                  CName = d.UserInfo.CommunityCenter.Name,
                                  DName = d.UserInfo.Department.Name
                              } into g
                              select new { Num = g.Key.Num, Name = g.Key.Name, Center = g.Key.CName, Department = g.Key.DName, DaySum = g.Sum(c => c.Duration) }).ToList();

                var hours = (from h in db.HourlyVacations
                             where h.UserInfo.CommunityCenter.Id == 1
                                   && h.VacationDate >= new DateTime(2019, 4, 14)
                                   && h.VacationDate <= new DateTime(2019, 4, 30)
                                    && h.UserInfo.EnrollNumber == "10"
                             group h by new
                             {
                                 Num = h.UserInfo.EnrollNumber,
                                 Name = h.UserInfo.FullName,
                                 CName = h.UserInfo.CommunityCenter.Name,
                                 DName = h.UserInfo.Department.Name
                             } into g
                             select new { Num = g.Key.Num, Name = g.Key.Name, Center = g.Key.CName, Department = g.Key.DName, HourSum = g.Sum(c => c.Duration.Hours * 60 + c.Duration.Minutes) / 60.0 }).ToList();
                foreach (var dr in dailys)
                {
                    //*************** حساب الحسم من الاجازات اليومية 
                    //حساب مجموعة الاجازات في الأشهر السابقة 
                    var sumtakenVacation = takenVacation?.Where(c => c.UserInfo.EnrollNumber == "10")?.Sum(c => (int?)c.Duration) ?? 0;

                    //حساب مجموع الاجازات الساعية في هذا الشهر 
                    var sumHourvacation = hours.Where(c => c.Num == "10").FirstOrDefault()?.HourSum ?? 0;

                    //الاجازات الكلية =مجموع الاجازات في هذا الشهر +ناتج قسمة ا
                    var totalVacatin = dr.DaySum + (sumHourvacation / 8);
                    restHour = sumHourvacation % 8;

                    var tt = 0.0;
                    for (int i = 1; i < new DateTime(2019, 4, 14).Month; i++)
                    {
                        int? totalHour;

                        var i1 = i;
                        totalHour = db.HourlyVacations
                                           .Where(c => c.IsActive && c.VacationDate.Month == i1 &&
                                                       c.UserInfo.EnrollNumber == "10")?
                                           .Sum(c => (int?)c.Duration.Hours + c.Duration.Minutes / 60) ?? 0;
                        tt += totalHour.Value % 8;

                    }

                    if (new DateTime(2019, 4, 14).Day > 1)
                    {
                        var da = new DateTime(new DateTime(2019, 4, 14).Year, new DateTime(2019, 4, 14).Month, 1);
                        var totalHour = db.HourlyVacations
                                           .Where(c => c.IsActive
                                           && c.VacationDate >= da
                                           && c.VacationDate < new DateTime(2019, 4, 14)
                                           && c.UserInfo.EnrollNumber == "10")?
                                           .Sum(c => (int?)c.Duration.Hours + c.Duration.Minutes / 60) ?? 0;
                        tt += totalHour % 8;

                    }
                    totalVacatin += (tt % 8 + restHour) / 8;
                    //DayDiscount > 0 يوجد حسم 
                    //DayDiscount =0 لايوجد حسم 
                    //DayDiscount <0 الموظف لديه اجازات لم تؤخذ بعد 
                    daydiscount = totalVacatin - ((totalPrevVacation - sumtakenVacation) + 1);

                }
            }
        }
    }
}
