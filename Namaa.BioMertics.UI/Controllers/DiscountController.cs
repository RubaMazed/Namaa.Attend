using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Namaa.BioMertics.UI.Models;
using Namaa.BioMetrics.Data;
using Namaa.BioMetrics.Model;
using Namaa.BioMetrics.Model.Enums;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Xsl;
using NPOI.SS.Formula.Functions;

//using OfficeOpenXml;
namespace Namaa.BioMertics.UI.Controllers
{
    [Authorize]
    public class DiscountController : Controller
    {
        // private ApplicationDbContext ;
        // GET: Discount
        public ActionResult Index()
        {
            TimeSpan ts = new TimeSpan(8, 35, 30);
            TimeSpan ts2 = new TimeSpan(3, 15, 20);
            TimeSpan diff = ts2.Subtract(ts);
            return View();
        }
        private int CountWeekEnd(DateTime startDate, DateTime endDate)
        {
            var totalDays = (endDate - startDate).TotalDays;
            var count = (int)(totalDays / 7) * 2;
            var remainder = totalDays % 7;
            if (remainder > 0)
            {
                DateTime date = endDate.AddDays(-remainder);
                while (date <= endDate)
                {
                    count = (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                        ? count + 1
                        : count;
                    date = date.AddDays(1);
                }
            }

            //var fridayDiff = (endDate.DayOfWeek - DayOfWeek.Friday) < 0 ? (endDate.DayOfWeek - DayOfWeek.Friday) + 7 : (endDate.DayOfWeek - DayOfWeek.Friday);
            //var saturdayDiff = (endDate.DayOfWeek - DayOfWeek.Saturday) < 0 ? (endDate.DayOfWeek - DayOfWeek.Saturday) + 7 : (endDate.DayOfWeek - DayOfWeek.Saturday);
            //if (remainder >= fridayDiff)
            //    count += fridayDiff;
            //if (remainder >= saturdayDiff)
            //    count += saturdayDiff;
            return count;
        }

        private void CalcSumHourVacation(DiscountViewModel DVM, int CommunityCenter, DateTime fromDate, DateTime toDate)
        {
            // var hours = new[] { new { Num = default(string), Name = default(string), Center = default(string), Department = default(string), HourSum = default(double) } }.Skip(1).ToList();
            using (var db = new ApplicationDbContext())
            {
                var hours = (from h in db.HourlyVacations
                             where h.UserInfo.CommunityCenter.Id == CommunityCenter
                                   && h.VacationDate >= fromDate
                                   && h.VacationDate <= toDate
                             group h by new
                             {
                                 Num = h.UserInfo.EnrollNumber,
                                 Name = h.UserInfo.FullName,
                                 CName = h.UserInfo.CommunityCenter.Name,
                                 DName = h.UserInfo.Department.Name
                             } into g
                             select new { Num = g.Key.Num, Name = g.Key.Name, Center = g.Key.CName, Department = g.Key.DName, HourSum = g.Sum(c => c.Duration.Hours * 60 + c.Duration.Minutes) / 60.0 }).ToList();
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
            }
        }

        private void CalcSumDailyVacation(DiscountViewModel DVM, int CommunityCenter, DateTime fromDate, DateTime toDate)
        {
            using (var db = new ApplicationDbContext())
            {
                var dailys = (from d in db.DailyVacations
                              where d.UserInfo.CommunityCenterId == CommunityCenter
                                    && d.FromDate >= fromDate && d.FromDate <= toDate
                              group d by new
                              {
                                  Num = d.UserInfo.EnrollNumber,
                                  Name = d.UserInfo.FullName,
                                  CName = d.UserInfo.CommunityCenter.Name,
                                  DName = d.UserInfo.Department.Name
                              } into g
                              select new { Num = g.Key.Num, Name = g.Key.Name, Center = g.Key.CName, Department = g.Key.DName, DaySum = g.Sum(c => c.Duration) }).ToList();
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
            }
        }

        private void CalculateTotalHour(DiscountViewModel DVM, int CommunityCenter, DateTime fromDate, DateTime toDate)
        {
            List<string> userNum;
            using (var db = new ApplicationDbContext())
            {
                userNum = (from u in db.UserInfos
                           where u.CommunityCenterId == CommunityCenter
                           select u.EnrollNumber).ToList();

                var logs = db.LogDataInfos
                                       .Where(c => c.LogDate >= fromDate && c.LogDate <= toDate)
                                       .Include("UserInfo").Include("UserInfo.CommunityCenter")
                                       .Include("UserInfo.Department").ToList();

                var results = from l in logs
                              group l by new { l.EnrollNum } into g
                              where userNum.Contains(g.Key.EnrollNum)
                              select new { Num = g.Key.EnrollNum, Times = g.OrderBy(c => c.LogDate).ThenBy(c => c.LogTime).ToList() };

                int totalHour = 0;
                int totalMin = 0;

                foreach (var r in results)
                {
                    totalHour = 0;
                    foreach (var t in r.Times)
                    {
                        if (t.LogOutTime != TimeSpan.Zero)
                        {
                            TimeSpan dif = t.LogOutTime.Subtract(t.LogTime);
                            totalHour += dif.Hours;
                            totalMin += dif.Minutes;
                        }
                    }
                    totalHour += totalMin / 60;
                    totalMin = totalMin % 60;

                    var hd = DVM.Details.Where(c => c.EnrollNum == r.Num).FirstOrDefault();
                    if (hd == null)
                    {
                        DiscountDetailViewModel newDetail = new DiscountDetailViewModel();
                        newDetail.Name = logs.FirstOrDefault(c => c.EnrollNum == r.Num).UserInfo.FullName;
                        newDetail.EnrollNum = r.Num;
                        newDetail.CommunityName = logs.FirstOrDefault(c => c.EnrollNum == r.Num).UserInfo.CommunityCenter.Name;
                        newDetail.DepartmentName = logs.FirstOrDefault(c => c.EnrollNum == r.Num).UserInfo.Department.Name;
                        newDetail.TotalHours = totalHour;
                        newDetail.TotalMins = totalMin;
                        newDetail.TotalTime = String.Format("{0}:{1}", totalHour, totalMin);
                        DVM.Details.Add(newDetail);
                    }
                    else
                    {
                        hd.TotalHours = totalHour;
                        hd.TotalMins = totalMin;
                        hd.TotalTime = String.Format("{0}:{1}", totalHour, totalMin);
                    }
                }

            }
        }

        private void CalculateTotalDelay(DiscountViewModel DVM, int CommunityCenter, DateTime fromDate, DateTime toDate)
        {

            using (var db = new ApplicationDbContext())
            {
                List<string> userNum = (from u in db.UserInfos
                                        where u.CommunityCenterId == CommunityCenter
                                        select u.EnrollNumber).ToList();
                List<LogDataInfo> logs = db.LogDataInfos
                                   .Where(c => c.LogDate >= fromDate && c.LogDate <= toDate)
                                   .Include("UserInfo").Include("UserInfo.CommunityCenter")
                                   .Include("UserInfo.Department").ToList();
                List<CommunityCenter> communityCenters = db.CommunityCenters.ToList();
                var delayresults =
                         from l in logs
                         group l by new
                         {
                             l.EnrollNum,
                             l.UserInfo.FullName,
                             CenterN = l.UserInfo.CommunityCenter.Name,
                             Dept = l.UserInfo.Department.Name,
                             l.LogDate
                         } into g
                         where userNum.Contains(g.Key.EnrollNum)
                         select new
                         {
                             Num = g.Key.EnrollNum,
                             Name = g.Key.FullName,
                             Center = g.Key.CenterN,
                             Department = g.Key.Dept,
                             Date = g.Key.LogDate,
                             Times = g.OrderBy(c => c.LogTime).ToList()
                         };


                foreach (var dr in delayresults)
                {
                    var maxTimeout = dr.Times.Max(c => c.LogOutTime);
                    var minTimeIn = dr.Times.Min(c => c.LogTime);
                    var maxCenterLog = (communityCenters.FirstOrDefault(d => d.Id == dr.Times.FirstOrDefault(c => c.LogOutTime == maxTimeout)?.CommunityCenterId)?.BeginingCOut).Value;
                    var minCenterLog = (communityCenters.FirstOrDefault(d => d.Id == dr.Times.FirstOrDefault(c => c.LogTime == minTimeIn)?.CommunityCenterId)?.EndingCIn).Value;

                    TimeSpan diffIn = TimeSpan.Zero;
                    TimeSpan diffOut = TimeSpan.Zero;
                    if (maxCenterLog != null && maxTimeout != TimeSpan.Zero && maxTimeout < maxCenterLog)
                    {
                        diffOut = maxCenterLog.Subtract(maxTimeout);
                    }

                    if (minTimeIn != TimeSpan.Zero && minTimeIn > minCenterLog)
                    {
                        diffIn = minTimeIn.Subtract(minCenterLog);
                    }
                    var hd = DVM.Details.FirstOrDefault(c => c.EnrollNum == dr.Num);
                    if (hd == null)
                    {
                        DiscountDetailViewModel newDetail = new DiscountDetailViewModel();
                        newDetail.Name = dr.Name;
                        newDetail.EnrollNum = dr.Num;
                        newDetail.CommunityName = dr.Center;
                        newDetail.DepartmentName = dr.Department;
                        newDetail.DelayHour = diffIn.Hours + diffOut.Hours;
                        newDetail.DelayMins = diffIn.Minutes + diffOut.Minutes;
                        DVM.Details.Add(newDetail);
                    }
                    else
                    {
                        hd.DelayHour += diffIn.Hours + diffOut.Hours;
                        hd.DelayMins += diffIn.Minutes + diffOut.Hours;
                    }
                }
                foreach (var detail in DVM.Details)
                {
                    detail.DelayHour += detail.DelayMins / 60;
                    detail.DelayMins = detail.DelayMins % 60;
                    detail.TotalDelay = $"{detail.DelayHour}: {detail.DelayMins}";
                }

            }
        }

        private void CalculateDiscountDay(DiscountViewModel DVM, int CommunityCenter, DateTime fromDate, DateTime toDate)
        {
            int totalPrevVacation = (int)fromDate.Month;
            double daydiscount = 0;
            double restHour = 0.0;
            using (var db = new ApplicationDbContext())
            {
                List<DailyVacation> takenVacation = db.DailyVacations.Where(c => c.IsActive && c.VacationType.IsDiscount && c.VacationType.IsAdministrative && c.FromDate < fromDate)
                    .Include("UserInfo")
                    .ToList();

                var users = (from u in db.UserInfos
                             where u.CommunityCenterId == CommunityCenter
                             && u.EnrollNumber == "258"
                             select u).Include("CommunityCenter")
                    .Include("Department")
                    .ToList();

                var dailys = (from d in db.DailyVacations
                              where d.UserInfo.CommunityCenterId == CommunityCenter
                                    && d.IsActive
                                    && d.FromDate >= fromDate && d.FromDate <= toDate
                                    && d.VacationType.IsDiscount
                                    && d.VacationType.IsAdministrative
                              group d by new
                              {
                                  Num = d.UserInfo.EnrollNumber,
                                  Name = d.UserInfo.FullName,
                                  CName = d.UserInfo.CommunityCenter.Name,
                                  DName = d.UserInfo.Department.Name
                              }
                    into g
                              select new
                              {
                                  Num = g.Key.Num,
                                  Name = g.Key.Name,
                                  Center = g.Key.CName,
                                  Department = g.Key.DName,
                                  DaySum = g.Sum(c => c.Duration)
                              }).ToList();


                var hours = (from h in db.HourlyVacations
                             where h.UserInfo.CommunityCenter.Id == CommunityCenter
                                   && h.VacationDate >= fromDate
                                   && h.VacationDate <= toDate
                                   && h.IsActive
                             group h by new
                             {
                                 Num = h.UserInfo.EnrollNumber,
                                 Name = h.UserInfo.FullName,
                                 CName = h.UserInfo.CommunityCenter.Name,
                                 DName = h.UserInfo.Department.Name
                             }
                    into g
                             select new
                             {
                                 Num = g.Key.Num,
                                 Name = g.Key.Name,
                                 Center = g.Key.CName,
                                 Department = g.Key.DName,
                                 HourSum = g.Sum(c => c.Duration.Hours * 60 + c.Duration.Minutes) / 60.0
                             }).ToList();



                foreach (var u in users)
                {
                    //*************** حساب الحسم من الاجازات اليومية 
                    //حساب مجموعة الاجازات في الأشهر السابقة 
                    var sumtakenVacation = takenVacation?.Where(c => c.UserInfo.EnrollNumber == u.EnrollNumber)
                                               ?.Sum(c => (int?)c.Duration > 1 ? 1 : (int?)c.Duration) ?? 0;

                    //حساب مجموع الاجازات الساعية في هذا الشهر 
                    var sumHourvacation = hours.Where(c => c.Num == u.EnrollNumber).FirstOrDefault()?.HourSum ?? 0;

                    var daySum = dailys.FirstOrDefault(c => c.Num == u.EnrollNumber)?.DaySum ?? 0;
                    //الاجازات الكلية =مجموع الاجازات في هذا الشهر +ناتج قسمة ا
                    var totalVacatin = daySum + (int)(sumHourvacation / 8);
                    restHour = (int)sumHourvacation % 8;

                    #region حساب الساعات السابقة المتراكمة

                    var tt = 0.0;
                    for (int i = 1; i < fromDate.Month; i++)
                    {
                        double? totalHour;

                        var i1 = i;
                        totalHour = db.HourlyVacations
                                        .Where(c => c.IsActive && c.VacationDate.Month == i1 &&
                                                    c.UserInfo.EnrollNumber == u.EnrollNumber)
                                        ?
                                        .Sum(c => (double?)c.Duration.Hours + (double)(c.Duration.Minutes) / 60.0) ??
                                    0;
                        tt += totalHour.Value % 8;

                    }

                    if (fromDate.Day > 1)
                    {
                        var da = new DateTime(fromDate.Year, fromDate.Month, 1);
                        var totalHour = db.HourlyVacations
                                            .Where(c => c.IsActive
                                                        && c.VacationDate >= da
                                                        && c.VacationDate < new DateTime(2019, 4, 17)
                                                        && c.UserInfo.EnrollNumber == u.EnrollNumber)
                                            ?.Sum(c => (double?)c.Duration.Hours +
                                                          (double)(c.Duration.Minutes / 60.0)) ?? 0;
                        tt += totalHour % 8;

                    }

                    #endregion

                    totalVacatin += (int)(tt % 8 + restHour) / 8;
                    sumtakenVacation += (int)tt / 8;
                    var vacationWithoutSalary = db.DailyVacations.Where(
                            c => c.IsActive && c.VacationType.IsDiscount && c.VacationType.IsAdministrative == false &&
                                                 c.FromDate >= fromDate && c.FromDate <= toDate && c.UserInfo.EnrollNumber == u.EnrollNumber).ToList();
                    int sumVacWithoutSal = 0;
                    if (vacationWithoutSalary.Count != 0)
                    {
                        sumVacWithoutSal = vacationWithoutSalary.Sum(c => c.Duration);
                    }
                    //DayDiscount > 0 يوجد حسم 
                    //DayDiscount =0 لايوجد حسم 
                    //DayDiscount <0 الموظف لديه اجازات لم تؤخذ بعد 
                    if (sumtakenVacation < totalPrevVacation)
                    {
                        daydiscount = totalVacatin - ((totalPrevVacation - sumtakenVacation) + 1);
                    }
                    else
                        daydiscount = totalVacatin - (+1);
                    daydiscount += sumVacWithoutSal;

                    var hd = DVM.Details.Where(c => c.EnrollNum == u.EnrollNumber).FirstOrDefault();
                    if (hd == null)
                    {
                        DiscountDetailViewModel newDetail = new DiscountDetailViewModel();
                        newDetail.Name = u.FullName;
                        newDetail.EnrollNum = u.EnrollNumber;
                        newDetail.CommunityName = u.CommunityCenter.Name;
                        newDetail.DepartmentName = u.Department.Name;
                        var DelayHour = Math.Abs(newDetail.DelayHour - sumHourvacation);
                        newDetail.DiscountDay = Math.Floor(daydiscount + (DelayHour / 8));
                        DVM.Details.Add(newDetail);
                    }
                    else
                    {
                        var DelayHour = Math.Abs(hd.DelayHour - sumHourvacation);
                        hd.DiscountDay = Math.Floor(daydiscount + (DelayHour / 8));
                    }

                }
            }
        }

        private DiscountViewModel CalcDiscount(DateTime fromDate, DateTime toDate, int holidaysNum, int CommunityCenter)
        {
            //int Year = 0;
            //int Month = 0;
            DiscountViewModel DVM = new DiscountViewModel();
            using (var db = new ApplicationDbContext())
            {
                DVM.CommunityCenters = db.CommunityCenters.Where(c => c.IsActive).ToList();

            }

            DVM.Details = new List<DiscountDetailViewModel>();
            if (CommunityCenter != 0 && fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
            {
                DVM.TotalOffDay = CountWeekEnd(fromDate, toDate) + holidaysNum;
                DVM.WorkingDay = (int)(toDate - fromDate).TotalDays - DVM.TotalOffDay;
                CommunityCenter cc;
                using (var db = new ApplicationDbContext())
                {
                    cc = db.CommunityCenters.Include("Schedules").FirstOrDefault(c => c.Id == CommunityCenter);
                }
                DVM.CenterWorkingHour = cc.BeginingCOut.Subtract(cc.EndingCIn).Hours;
                DVM.TotlalCenterHours = DVM.CenterWorkingHour * DVM.WorkingDay;

                //مجموع الاجازات الساعية في المدة المحدد
                #region HourlyVacation
                var hours = new[] { new { Num = default(string), Name = default(string), Center = default(string), Department = default(string), HourSum = default(double) } }.Skip(1).ToList();
                CalcSumHourVacation(DVM, CommunityCenter, fromDate, toDate);
                #endregion

                //مجموع الاجازات اليومية في الشهر المحدد
                #region DailyVacation
                var dailys = new[] { new { Num = default(string), Name = default(string), Center = default(string), Department = default(string), DaySum = default(int) } }.Skip(1).ToList();
                CalcSumDailyVacation(DVM, CommunityCenter, fromDate, toDate);
                #endregion
                //اجمالي ساعات الدوام 
                #region Calculate TotlaHour
                CalculateTotalHour(DVM, CommunityCenter, fromDate, toDate);
                #endregion
                //حساب ساعات التأخير 
                #region Calculate TotalDelay
                CalculateTotalDelay(DVM, CommunityCenter, fromDate, toDate);
                #endregion
                //حساب أيام الحسم
                #region CalculateDiscountDay
                CalculateDiscountDay(DVM, CommunityCenter, fromDate, toDate);
                #endregion

            }
            return DVM;
        }
        public ActionResult CalculateDiscount([Bind(Include = "FromDate,ToDate,CommunityCenter,HolidaysNum")] DiscountViewModel dvm)
        {
            try
            {
                DiscountViewModel DVM = CalcDiscount(dvm.FromDate, dvm.ToDate, dvm.HolidaysNum, dvm.CommunityCenter);
                DVM.Details.OrderBy(c => c.EnrollNum);
                ViewBag.fromDate = dvm.FromDate;
                ViewBag.toDate = dvm.ToDate;
                return View(DVM);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Export([Bind(Include = "FromDate,ToDate,CommunityCenter,HolidaysNum")] DiscountViewModel dvm)
        {
            DiscountViewModel DVM = CalcDiscount(dvm.FromDate, dvm.ToDate, dvm.HolidaysNum, dvm.CommunityCenter);

            if (System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/DiscountTemplate.xlsx")))
            {
                var fileBytes = System.IO.File.ReadAllBytes(HostingEnvironment.MapPath("~/App_Data/DiscountTemplate.xlsx"));
                using (var ms = new MemoryStream(fileBytes))
                {
                    var workbook = new XSSFWorkbook(ms);
                    var mainSheet = workbook.GetSheetAt(0);
                    foreach (var detail in DVM.Details)
                    {
                        var rowIndex = mainSheet.LastRowNum + 1;
                        var row = mainSheet.CreateRow(rowIndex);
                        var cellNum = row.GetCell(0) ?? row.CreateCell(0);
                        cellNum.SetCellValue(detail.EnrollNum);

                        var cellName = row.GetCell(1) ?? row.CreateCell(1);
                        cellName.SetCellValue(detail.Name);

                        var cellCenter = row.GetCell(2) ?? row.CreateCell(2);
                        cellCenter.SetCellValue(detail.CommunityName);

                        var cellDept = row.GetCell(3) ?? row.CreateCell(3);
                        cellDept.SetCellValue(detail.DepartmentName);

                        var cellTotaHours = row.GetCell(4) ?? row.CreateCell(4);
                        cellTotaHours.SetCellValue(detail.TotalTime);

                        var cellTotaDV = row.GetCell(5) ?? row.CreateCell(5);
                        cellTotaDV.SetCellValue(detail.TotalDailyVacation);

                        var cellTotaHV = row.GetCell(6) ?? row.CreateCell(6);
                        cellTotaHV.SetCellValue(detail.TotalDailyVacation);

                        var cellTotaDiscount = row.GetCell(7) ?? row.CreateCell(7);
                        cellTotaDiscount.SetCellValue(detail.DiscountDay);
                    }
                    byte[] downloadfileBytes = null;
                    using (var dms = new MemoryStream())
                    {
                        workbook.Write(dms);
                        workbook.Close();
                        downloadfileBytes = dms.ToArray();
                    }
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
                    Response.BinaryWrite(downloadfileBytes);
                    Response.End();
                    //var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(downloadfileBytes) };
                    //response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                    //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    //response.Content.Headers.ContentDisposition.FileName = "DiscountType.xlsx";
                    //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                    //return response;

                }

            }

        }
        public ActionResult GetEmpDisDetails(int num, DateTime fromDate, DateTime toDate)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            EmpDiscountDetails empDetails = new EmpDiscountDetails();
            empDetails.EnrollNumber = num;
            empDetails.Name = _db.UserInfos.FirstOrDefault(c => c.EnrollNumber == num.ToString())?.FullName;
            empDetails.Logs = new List<LogDataInfoViewModel>();
            empDetails.DailyVacations = new List<DailyVacationViewModel>();
            empDetails.HourVacations = new List<HourVacationViewModel>();
            List<LogDataInfo> logs = _db.LogDataInfos.Where(c => c.EnrollNum == num.ToString() &&
            (c.LogDate >= fromDate && c.LogDate <= toDate))
            .Include("UserInfo").Include("UserInfo.CommunityCenter").Include("UserInfo.Department").OrderBy(c => c.LogDate).ThenBy(c => c.LogTime)
            .ThenBy(c => c.LogOutTime).ToList();
            foreach (var log in logs)
            {
                LogDataInfoViewModel lvm = log;
                empDetails.Logs.Add(lvm);
            }
            List<DailyVacation> dVacations = _db.DailyVacations.Include("VacationType")
                .Where(c => c.UserInfo.EnrollNumber == num.ToString()
                && (c.IsActive)
                && (c.FromDate <= toDate)).OrderBy(c => c.FromDate).ToList();
            foreach (var dv in dVacations)
            {
                DailyVacationViewModel d = dv;
                empDetails.DailyVacations.Add(d);
            }
            List<HourlyVacation> hvacations = _db.HourlyVacations.Where(c => c.UserInfo.EnrollNumber == num.ToString()
                                                                          && (c.IsActive) && (c.VacationDate <= toDate)).OrderBy(c => c.VacationDate).ToList();
            foreach (var hv in hvacations)
            {
                HourVacationViewModel hvm = hv;
                empDetails.HourVacations.Add(hvm);
            }

            return View(empDetails);
        }
    }
}