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

namespace Namaa.BioMertics.UI.Controllers
{
    [Authorize]
    public class LogDataInfoesController : Controller
    {
        // GET: LogDataInfoes
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LogDataInfoes
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumSortParm = String.IsNullOrEmpty(sortOrder) ? "num_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.BirthSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.BirthSortParm = sortOrder == "Time" ? "time_desc" : "Time";
            ViewBag.CenterSortParm = sortOrder == "CommunityCenterName" ? "cname_desc" : "CenterName";
            ViewBag.DeptSortParm = sortOrder == "DepartmentName" ? "dname_desc" : "DepartmentName";

            List<LogDataInfo> logs = db.LogDataInfos.Where(c => c.IsActive).Include("UserInfo")
                .Include("UserInfo.CommunityCenter").Include("UserInfo.Department").ToList();
            List<LogDataInfoViewModel> logsViewModel = new List<LogDataInfoViewModel>();
            foreach (var log in logs)
            {
                LogDataInfoViewModel LVM = log;
                logsViewModel.Add(LVM);
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
                case "cname_desc":
                    logsViewModel = logsViewModel.OrderByDescending(c => c.CommunityCenterName).ToList();
                    break;
                case "Time":
                    logsViewModel = logsViewModel.OrderBy(c => c.LogInTime).ToList();
                    break;
                case "time_desc":
                    logsViewModel = logsViewModel.OrderByDescending(c => c.LogInTime).ToList();
                    break;
                case "CenterName":
                    logsViewModel = logsViewModel.OrderBy(c => c.CommunityCenterName).ToList();
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
    }
}