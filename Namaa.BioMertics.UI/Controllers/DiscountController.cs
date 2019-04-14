using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Namaa.BioMertics.UI.Models;
using Namaa.BioMetrics.Data;
using Namaa.BioMetrics.Model;

namespace Namaa.BioMertics.UI.Controllers
{
    [Authorize]
    public class DiscountController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Discount
        public ActionResult Index()
        {
            TimeSpan ts = new TimeSpan(8, 35, 30);
            TimeSpan ts2 = new TimeSpan(3, 15, 20);
            TimeSpan diff = ts2.Subtract(ts);
            return View();
        }

        public ActionResult CalculateDiscount([Bind(Include = "Month,Year,CommunityCenter")] DiscountViewModel dvm)
        {
            DiscountViewModel DVM = new DiscountViewModel();
            DVM.CommunityCenters = _db.CommunityCenters.Where(c => c.IsActive).ToList();
            DVM.Details = new List<DiscountDetailViewModel>();
            if (dvm.CommunityCenter != 0 && dvm.Year != 0)
            {
                var userNum = (from u in _db.UserInfos
                               where u.CommunityCenterId == dvm.CommunityCenter
                               select u.EnrollNumber).ToList();

                //var dailys =
                //    (from d in _db.DailyVacations
                //     where d.UserInfo.CommunityCenter.Id == dvm.CommunityCenter
                //           && d.FromDate.Month == (int)dvm.Month && d.FromDate.Year == dvm.Year
                //     // && d.ToDate.Month == (int)dvm.Month && d.ToDate.Year == dvm.Year
                //     select d).Include("UserInfo").Include("UserInfo.CommunityCenter")
                //     .Include("UserInfo.Department").ToList();


                var hours = (from h in _db.HourlyVacations
                             where h.UserInfo.CommunityCenter.Id == dvm.CommunityCenter
                                   && h.VacationDate.Month == (int)dvm.Month
                                   && h.VacationDate.Year == dvm.Year
                             group h by new
                             {
                                 Num = h.UserInfo.EnrollNumber,
                                 Name = h.UserInfo.FullName,
                                 CName = h.UserInfo.CommunityCenter.Name,
                                 DName = h.UserInfo.Department.Name
                             } into g
                             select new { Num = g.Key.Num, Name = g.Key.Name, Center = g.Key.CName, Department = g.Key.DName, HourSum = g.Sum(c => c.Duration) }).ToList();


                var dailys = (
                    from d in _db.DailyVacations
                    where d.UserInfo.CommunityCenterId == dvm.CommunityCenter
                    && d.FromDate.Month == (int)dvm.Month && d.FromDate.Year == dvm.Year
                    group d by new
                    {
                        Num = d.UserInfo.EnrollNumber,
                        Name = d.UserInfo.FullName,
                        CName = d.UserInfo.CommunityCenter.Name,
                        DName = d.UserInfo.Department.Name
                    } into g
                    select new { Num = g.Key.Num, Name = g.Key.Name, Center = g.Key.CName, Department = g.Key.DName, DaySum = g.Sum(c => c.Duration) });


                #region CalculateDailyVacation
                foreach (var dr in dailys)
                {
                    var detail = DVM.Details.Where(d => d.EnrollNum == dr.Num.ToString()).FirstOrDefault();
                    if (detail == null)
                    {
                        DiscountDetailViewModel newDetail = new DiscountDetailViewModel();
                        newDetail.Name = dr.Name;
                        newDetail.EnrollNum = dr.Num.ToString();
                        newDetail.CommunityName = dr.Center;
                        newDetail.DepartmentName = dr.Department;
                        newDetail.TotalDailyVacation = dr.DaySum;
                        DVM.Details.Add(newDetail);
                    }
                    else
                    {

                        detail.TotalDailyVacation = dr.DaySum;
                    }
                }

                #endregion

                #region CalculateHourVacation
                foreach (var hour in hours)
                {
                    var hd = DVM.Details.Where(d => d.EnrollNum == hour.Num).FirstOrDefault();
                    if (hd == null)
                    {
                        DiscountDetailViewModel newDetail = new DiscountDetailViewModel();
                        newDetail.Name = hour.Name;
                        newDetail.EnrollNum = hour.Num;
                        newDetail.CommunityName = hour.Center;
                        newDetail.DepartmentName = hour.Department;
                        newDetail.TotalHouVacatin = hour.HourSum;
                        DVM.Details.Add(newDetail);
                    }
                    else
                    {
                        hd.TotalHouVacatin = hour.HourSum;

                    }
                }
                #endregion

                #region CalculateDiscountDay
                int totalPrevVacation = (int)dvm.Month - 1;
                int daydiscount = 0;
                int houDiscount = 0;
                int restHour = 0;
                foreach (var dr in dailys)
                {
                    //if (dr.DaySum > 1)
                    //{
                    //*************** حساب الحسم من الاجازات اليومية 
                    //حساب مجموعة الاجازات في الأشهر السابقة 
                    var sumtakenVacation = _db.DailyVacations.Where(c => c.IsActive
                                                                    && c.FromDate.Month < (int)dvm.Month
                                                                    && c.UserInfo.EnrollNumber == dr.Num.ToString())
                        .Sum(c => c.Duration);

                    //حساب مجموع الاجازات الساعية في هذا الشهر 
                    var sumHourvacation = hours.Where(c => c.Num == dr.Num.ToString()).FirstOrDefault().HourSum;

                    //الاجازات الكلية =مجموع الاجازات في هذا الشهر +ناتج قسمة ا
                    var totalVacatin = dr.DaySum + sumHourvacation / 7;
                    restHour = sumHourvacation % 7;

                    if (totalPrevVacation - sumtakenVacation > 0)
                    {
                        if (totalPrevVacation - sumtakenVacation + 1 == totalVacatin)
                        {

                            daydiscount = 0;
                        }
                        else
                        {
                            daydiscount = totalVacatin - (totalPrevVacation - sumtakenVacation + 1);
                        }
                    }
                    else
                    {

                    }

                    var tt = 0;
                    for (int i = 1; i < (int)dvm.Month; i++)
                    {
                        var totalHour = _db.HourlyVacations
                            .Where(c => c.IsActive && c.VacationDate.Month == i &&
                                        c.UserInfo.EnrollNumber == dr.Num.ToString())
                            .Sum(c => c.Duration);
                        tt += totalHour % 7;
                        //معادلات التقسيم على 7

                    }
                    var TotalrestHour = tt % 7 + restHour;
                    daydiscount += TotalrestHour / 7;
                    //}
                }
                #endregion

                #region Calculate TotlaHour
                List<LogDataInfo> logs = _db.LogDataInfos
                    .Where(c => c.LogDate.Month == (int)dvm.Month && c.LogDate.Year == dvm.Year).ToList();


                var results =
                    from l in logs
                    group l by new { l.EnrollNum } into g
                    where userNum.Contains(g.Key.EnrollNum)
                    select new { Num = g.Key.EnrollNum, Times = g.OrderBy(c => c.LogTime).ToList() };


                int TotalHour = 0;
                int totalMin = 0;

                foreach (var r in results)
                {
                    TotalHour = 0;
                    foreach (var t in r.Times)
                    {
                        if (t.LogOutTime != TimeSpan.Zero)
                        {
                            TimeSpan dif = t.LogOutTime.Subtract(t.LogTime);
                            TotalHour += dif.Hours;
                            totalMin += dif.Minutes;
                        }
                    }
                    TotalHour += totalMin / 60;
                    totalMin = totalMin % 60;
                    var hd = DVM.Details.Where(c => c.EnrollNum == r.Num).FirstOrDefault();
                    if (hd == null)
                    {
                        var user = _db.UserInfos.Where(c => c.EnrollNumber == r.Num)
                            .Include("CommunityCenter")
                            .Include("Department")
                            .FirstOrDefault();
                        DiscountDetailViewModel newDetail = new DiscountDetailViewModel();
                        newDetail.Name = user.FullName;
                        newDetail.EnrollNum = r.Num;
                        newDetail.CommunityName = user.CommunityCenter.Name;
                        newDetail.DepartmentName = user.Department.Name;
                        newDetail.TotalHours = TotalHour;
                        newDetail.TotalMins = totalMin;
                        newDetail.TotalTime = String.Format("{0} h : {1} m", TotalHour, totalMin);
                        DVM.Details.Add(newDetail);
                    }
                    else
                    {
                        hd.TotalHours = TotalHour;
                        hd.TotalMins = totalMin;
                        hd.TotalTime = String.Format("{0} h : {1} m", TotalHour, totalMin);
                    }
                }

                #endregion
            }
            return View(DVM);

        }

    }
}