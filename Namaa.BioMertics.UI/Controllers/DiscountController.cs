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
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Discount
        public ActionResult Index()
        {
            TimeSpan TS = new TimeSpan(8, 35, 30);
            TimeSpan Ts2 = new TimeSpan(3, 15, 20);
            TimeSpan diff = Ts2.Subtract(TS);
            return View();
        }

        public ActionResult CalculateDiscount([Bind(Include = "Month,Year,CommunityCenter")] DiscountViewModel dvm)
        {

            DiscountViewModel DVM = new DiscountViewModel();
            DVM.CommunityCenters = db.CommunityCenters.Where(c => c.IsActive).ToList();
            DVM.Details = new List<DiscountDetailViewModel>();
            if (dvm.CommunityCenter != 0 && dvm.Year != 0)
            {
                //الاجازات الطويلة المقسمة على أكثر من شهر لم تحل بعد

                var Dailys =
                    (from d in db.DailyVacations
                     where d.UserInfo.CommunityCenter.Id == dvm.CommunityCenter
                           && d.FromDate.Month == (int)dvm.Month && d.FromDate.Year == dvm.Year
                           && d.ToDate.Month == (int)dvm.Month && d.ToDate.Year == dvm.Year
                     select d).Include("UserInfo").Include("UserInfo.CommunityCenter").Include("UserInfo.Department").ToList();

                var Hours = (from h in db.HourlyVacations
                             where h.UserInfo.CommunityCenter.Id == dvm.CommunityCenter
                                   && h.VacationDate.Month == (int)dvm.Month && h.VacationDate.Year == dvm.Year
                             select h).Include("UserInfo").Include("UserInfo.CommunityCenter").Include("UserInfo.Department").ToList();

                #region CalculateDailyVacation

                foreach (var daily in Dailys)
                {
                    var Detail = DVM.Details.Where(d => d.EnrollNum == daily.UserInfo.EnrollNumber).FirstOrDefault();
                    if (Detail == null)
                    {
                        DiscountDetailViewModel newDetail = new DiscountDetailViewModel();
                        newDetail.Name = daily.UserInfo.FullName;
                        newDetail.EnrollNum = daily.UserInfo.EnrollNumber;
                        newDetail.CommunityName = daily.UserInfo.CommunityCenter.Name;
                        newDetail.DepartmentName = daily.UserInfo.Department.Name;
                        newDetail.TotalDailyVacation = daily.Duration;
                        DVM.Details.Add(newDetail);
                    }
                    else
                    {
                        Detail.TotalDailyVacation += daily.Duration;

                    }
                }
                #endregion

                #region CalculateHourVacation
                foreach (var hour in Hours)
                {
                    var hd = DVM.Details.Where(d => d.EnrollNum == hour.UserInfo.EnrollNumber).FirstOrDefault();
                    if (hd == null)
                    {
                        DiscountDetailViewModel newDetail = new DiscountDetailViewModel();
                        newDetail.Name = hour.UserInfo.FullName;
                        newDetail.EnrollNum = hour.UserInfo.EnrollNumber;
                        newDetail.CommunityName = hour.UserInfo.CommunityCenter.Name;
                        newDetail.DepartmentName = hour.UserInfo.Department.Name;
                        newDetail.TotalHouVacatin = hour.Duration;
                        DVM.Details.Add(newDetail);
                    }
                    else
                    {
                        hd.TotalHouVacatin += hour.Duration;

                    }
                }


                #endregion
            }
            return View(DVM);

        }

    }
}