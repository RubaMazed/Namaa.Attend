using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Namaa.BioMertics.UI.Models;
using Namaa.BioMetrics.Data;
using Namaa.BioMetrics.Model;
using PagedList;

namespace Namaa.BioMertics.UI.Controllers
{
    [Authorize]
    public class DailyVacationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: DailyVacation
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumSortParm = String.IsNullOrEmpty(sortOrder) ? "num_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.BirthSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.BirthSortParm = sortOrder == "Time" ? "time_desc" : "Time";
            ViewBag.CenterSortParm = sortOrder == "CommunityCenterName" ? "cname_desc" : "CenterName";
            ViewBag.DeptSortParm = sortOrder == "DepartmentName" ? "dname_desc" : "DepartmentName";

            List<DailyVacation> vacations = db.DailyVacations.Where(c => c.IsActive).Include("UserInfo")
                .Include("UserInfo.CommunityCenter").Include("UserInfo.Department").ToList();
            List<DailyVacationViewModel> vactionViewModel = new List<DailyVacationViewModel>();
            foreach (var vac in vacations)
            {
                DailyVacationViewModel DVM = vac;
                vactionViewModel.Add(vac);
            }
            switch (sortOrder)
            {

                case "Name":
                    vactionViewModel = vactionViewModel.OrderBy(c => c.UserName).ToList();
                    break;
                case "name_desc":
                    vactionViewModel = vactionViewModel.OrderByDescending(c => c.UserName).ToList();
                    break;
                case "date_desc":
                    vactionViewModel = vactionViewModel.OrderByDescending(c => c.ApplicationDate).ToList();
                    break;
                case "Date":
                    vactionViewModel = vactionViewModel.OrderBy(c => c.ApplicationDate).ToList();
                    break;
                case "cname_desc":
                    vactionViewModel = vactionViewModel.OrderByDescending(c => c.CommunityCenterName).ToList();
                    break;

                case "DepartmentName":
                    vactionViewModel = vactionViewModel.OrderBy(c => c.DepartmentName).ToList();
                    break;
                default:
                    vactionViewModel = vactionViewModel.OrderBy(c => c.UserName).ToList();
                    break;

            }
            int pageSize = (int)Math.Ceiling(vactionViewModel.Count / 10.0) != 0 ? (int)Math.Ceiling(vactionViewModel.Count / 10.0) : 1;
            int pageNumber = (page ?? 1);
            return View(vactionViewModel.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Create(int? num)
        {
            var user = db.UserInfos.Where(c => c.EnrollNumber == num.ToString() && c.IsActive).Include("Department").Include("CommunityCenter").FirstOrDefault();
            DailyVacationViewModel DVM = new DailyVacationViewModel();
            DVM.UserId = user.Id;
            DVM.UserName = user.FullName;
            DVM.DepartmentName = user.Department.Name;
            DVM.CommunityCenterName = user.CommunityCenter.Name;
            DVM.UserPosition = user.Position;
            DVM.VacationTypes = new List<VacationType>();
            DVM.VacationTypes = db.VacationTypes.Where(v => v.IsActive).ToList();
            return View(DVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,VacationType,Duration,ApplicationDate,VacationDate,FromDate,ToDate,IsDelegacy,DeputationPerson,Reason")] DailyVacationViewModel vacation)
        {
            if (ModelState.IsValid)
            {
                DailyVacation DV = vacation;
                DV.CreationDate = DateTime.Now;
                DV.CreatedBy = User.Identity.GetUserName();
                db.DailyVacations.Add(DV);
                db.SaveChanges();
                //return View();
                return RedirectToAction("Index");
            }

            return View(vacation);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyVacation dv = db.DailyVacations.Where(c => c.Id == id).Include("UserInfo").Include("UserInfo.Department").Include("UserInfo.CommunityCenter").FirstOrDefault();
            if (dv == null)
            {
                return HttpNotFound();
            }
            DailyVacationViewModel DVM = dv;
            DVM.VacationTypes = db.VacationTypes.Where(c => c.IsActive).ToList();
            return View(DVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VacationType,Duration,ApplicationDate,VacationDate,FromDate,ToDate,IsDelegacy,DeputationPerson,Reason")] DailyVacationViewModel DVM)
        {
            if (ModelState.IsValid)
            {
                DailyVacation dv = db.DailyVacations.Where(c => c.Id == DVM.Id).FirstOrDefault();


                dv.VacationTypeId = DVM.VacationType;
                dv.ApplicationDate = Convert.ToDateTime(DVM.ApplicationDate);
                dv.Duration = DVM.Duration;
                dv.FromDate = Convert.ToDateTime(DVM.FromDate);
                dv.ToDate = Convert.ToDateTime(DVM.ToDate);
                dv.IsDelegacy = DVM.IsDelegacy;
                dv.DeputationPerson = DVM.DeputationPerson;
                dv.Reason = DVM.Reason;
                dv.UpdatedDate = DateTime.Now;
                dv.UpdatedBy = User.Identity.GetUserName();
                db.Entry(dv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(DVM);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyVacation dv = db.DailyVacations.Where(c => c.Id == id && c.IsActive).Include("UserInfo")
                .Include("UserInfo.CommunityCenter").Include("UserInfo.Department").FirstOrDefault();
            DailyVacationViewModel DVM = dv;
            if (dv == null)
            {
                return HttpNotFound();
            }
            return View(DVM);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DailyVacation dv = db.DailyVacations.Find(id);
            dv.DeletedBy = User.Identity.GetUserName();
            dv.IsActive = false;
            dv.DeletedDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}