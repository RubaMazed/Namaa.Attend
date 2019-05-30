using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Namaa.BioMertics.UI.Models;
using Namaa.BioMetrics.Data;
using Namaa.BioMetrics.Model;
using Namaa.BioMetrics.Utilities;
using Namaa.BioMetrics.Utilities.Enums;
using PagedList;
using System.Net;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;

namespace Namaa.BioMertics.UI.Controllers
{
    [Authorize]
    public class LogDataInfoesController : Controller
    {
        // GET: LogDataInfoes
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LogDataInfoes
        public ActionResult Index(string sortOrder, string currentFilter, string fdtFilter, string tdFilter
            , string searchString, string fromDate, string toDate, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumSortParm = String.IsNullOrEmpty(sortOrder) ? "num_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.DaySortParm = sortOrder == "Day" ? "day_desc" : "Day";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LogInSortParm = sortOrder == "LogIn" ? "login_desc" : "LogIn";
            ViewBag.LogOutSortParm = sortOrder == "LogOut" ? "logout_desc" : "LogOut";
            ViewBag.CommunityCenterSortParm = sortOrder == "CommunityCenterName" ? "cname_desc" : "CommunityCenterName";
            ViewBag.DepartmentNameSortParm = sortOrder == "DepartmentName" ? "dname_desc" : "DepartmentName";

            List<LogDataInfo> logs = db.LogDataInfos.Where(c => c.IsActive).Include("UserInfo")
                .Include("UserInfo.CommunityCenter").Include("UserInfo.Department").ToList();
            List<LogDataInfoViewModel> logsViewModel = new List<LogDataInfoViewModel>();
            foreach (var log in logs)
            {
                LogDataInfoViewModel LVM = log;
                logsViewModel.Add(LVM);
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (fromDate != null)
            {
                page = 1;
            }
            else
            {
                fromDate = fdtFilter;
            }
            if (toDate != null)
            {
                page = 1;
            }
            else
            {
                toDate = tdFilter;
            }
            ViewBag.CurrentFilter = searchString;
            ViewBag.FDtFilter = fromDate;
            ViewBag.TDFilter = toDate;
            if (!String.IsNullOrEmpty(searchString))
            {
                logsViewModel = logsViewModel.Where(u => u.FullName.ToLower().Contains(searchString.ToLower()) ||
               u.EnrollNum.ToString() == searchString).ToList();

            }
            if (!String.IsNullOrEmpty(fromDate) && !String.IsNullOrEmpty(toDate))
            {
                logsViewModel = logsViewModel.Where(u => u.LogDate.AsDateTime().Date >= fromDate.AsDateTime().Date && u.LogDate.AsDateTime().Date <= toDate.AsDateTime().Date).ToList();
            }
            switch (sortOrder)
            {
                case "num_desc":
                    logsViewModel = logsViewModel.OrderByDescending(c => c.EnrollNum).ToList();
                    break;
                case "Name":
                    logsViewModel = logsViewModel.OrderBy(c => c.FullName).ToList();
                    break;
                case "name_desc":
                    logsViewModel = logsViewModel.OrderByDescending(c => c.FullName).ToList();
                    break;
                case "date_desc":
                    logsViewModel = logsViewModel.OrderByDescending(c => c.LogDate).ToList();
                    break;
                case "Date":
                    logsViewModel = logsViewModel.OrderBy(c => c.LogDate).ToList();
                    break;
                case "Day":
                    logsViewModel = logsViewModel.OrderBy(c => c.LogDate.AsDateTime().DayOfWeek).ToList();
                    break;
                case "day_desc":
                    logsViewModel = logsViewModel.OrderByDescending(c => c.LogDate.AsDateTime().DayOfWeek).ToList();
                    break;

                case "cname_desc":
                    logsViewModel = logsViewModel.OrderByDescending(c => c.CommunityCenterName).ToList();
                    break;
                case "CommunityCenterName":
                    logsViewModel = logsViewModel.OrderBy(c => c.CommunityCenterName).ToList();
                    break;
                case "LogIn":
                    logsViewModel = logsViewModel.OrderBy(c => c.LogInTime).ToList();
                    break;
                case "login_desc":
                    logsViewModel = logsViewModel.OrderByDescending(c => c.LogInTime).ToList();
                    break;
                case "LogOut":
                    logsViewModel = logsViewModel.OrderBy(c => c.LogOutTime).ToList();
                    break;
                case "logout_desc":
                    logsViewModel = logsViewModel.OrderByDescending(c => c.LogOutTime).ToList();
                    break;

                case "dname_desc":
                    logsViewModel = logsViewModel.OrderByDescending(c => c.DepartmentName).ToList();
                    break;
                case "DepartmentName":
                    logsViewModel = logsViewModel.OrderBy(c => c.DepartmentName).ToList();
                    break;
                default:
                    logsViewModel = logsViewModel.OrderBy(c => c.EnrollNum).ToList();
                    break;

            }
            int pageSize = (int)Math.Ceiling(logsViewModel.Count / 10.0);
            int pageNumber = (page ?? 1);
            return View(logsViewModel.ToPagedList(pageNumber, pageSize));

            #region Comments
            //DeviceManipulator mainpulator = new DeviceManipulator();
            //if (mainpulator.ConnectDevice("192.168.1.201", 4370) == ConnectStatus.Connected)
            //{
            //    ICollection<LogDataInfo> logs = mainpulator.GetAllLogInfo(1);
            //    foreach (var log in logs)
            //    {
            //        log.CreatedBy = "r.mazed";
            //        var user = db.UserInfos.Where(c => c.EnrollNumber == log.EnrollNum).FirstOrDefault();
            //        if (user != null)
            //        {
            //            log.UserInfoId = user != null ? user.Id : 0;
            //            db.LogDataInfos.Add(log);
            //            db.SaveChanges();
            //        }

            //    }
            //    return View(new List<LogDataInfoViewModel>());

            //}
            //else
            //    return View(new List<LogDataInfoViewModel>());

            #endregion

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogDataInfo logInfo = db.LogDataInfos.Where(c => c.Id == id).Include("UserInfo.Department").Include("UserInfo.CommunityCenter").Include("UserInfo").FirstOrDefault();
            LogDataInfoViewModel LogViewModel = logInfo;
            if (logInfo == null)
            {
                return HttpNotFound();
            }
            return View(LogViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LogInTime,LogOutTime")] LogDataInfoViewModel logInfo)
        {
            if (ModelState.IsValid)
            {
                LogDataInfo updatedInfo = db.LogDataInfos.Where(c => c.Id == logInfo.Id).FirstOrDefault();
                updatedInfo.LogTime = logInfo.LogInTime;
                updatedInfo.LogOutTime = logInfo.LogOutTime;
                updatedInfo.UpdatedDate = DateTime.Now;
                updatedInfo.UpdatedBy = User.Identity.GetUserName();
                db.Entry(updatedInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(logInfo);
        }

    }
}