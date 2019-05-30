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
    public class HourVacationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: HourVacation
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumSortParm = String.IsNullOrEmpty(sortOrder) ? "num_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.CommunityCenterNameSortParm = sortOrder == "CenterName" ? "center_desc" : "CenterName";
            ViewBag.DeptSortParm = sortOrder == "DepartmentName" ? "dname_desc" : "DepartmentName";
            ViewBag.UserPositionSortParm = sortOrder == "Position" ? "pos_desc" : "Position";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.UserPositionSortParm = sortOrder == "Position" ? "pos_desc" : "Position";
            ViewBag.VacationDateSortParm = sortOrder == "VactionDate" ? "vdate_desc" : "VactionDate";
            ViewBag.FromHourSortParm = sortOrder == "FromHour" ? "fhour_desc" : "FromHour";
            ViewBag.ToHourSortParm = sortOrder == "ToHour" ? "thour_desc" : "ToHour";
            ViewBag.DurationSortParm = sortOrder == "Duration" ? "dur_desc" : "Duration";
            List<HourlyVacation> vacations = db.HourlyVacations.Where(c => c.IsActive).Include("UserInfo")
                .Include("UserInfo.CommunityCenter").Include("UserInfo.Department").ToList();
            List<HourVacationViewModel> vactionViewModel = new List<HourVacationViewModel>();
            foreach (var vac in vacations)
            {
                //HourVacationViewModel HVM = vac;
                vactionViewModel.Add(vac);
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                vactionViewModel = vactionViewModel.Where(u => u.UserName.ToLower().Contains(searchString.ToLower())).ToList();
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

                case "center_desc":
                    vactionViewModel = vactionViewModel.OrderByDescending(c => c.CommunityCenterName).ToList();
                    break;
                case "CenterName":
                    vactionViewModel = vactionViewModel.OrderBy(c => c.CommunityCenterName).ToList();
                    break;

                case "DepartmentName":
                    vactionViewModel = vactionViewModel.OrderBy(c => c.DepartmentName).ToList();
                    break;
                case "dname_desc":
                    vactionViewModel = vactionViewModel.OrderByDescending(c => c.DepartmentName).ToList();
                    break;

                case "Position":
                    vactionViewModel = vactionViewModel.OrderBy(c => c.UserPosition).ToList();
                    break;
                case "pos_desc":
                    vactionViewModel = vactionViewModel.OrderByDescending(c => c.UserPosition).ToList();
                    break;

                case "FromHour":
                    vactionViewModel = vactionViewModel.OrderBy(c => c.FromHour).ToList();
                    break;
                case "fhour_desc":
                    vactionViewModel = vactionViewModel.OrderByDescending(c => c.FromHour).ToList();
                    break;

                case "ToHour":
                    vactionViewModel = vactionViewModel.OrderBy(c => c.ToHour).ToList();
                    break;
                case "thour_desc":
                    vactionViewModel = vactionViewModel.OrderByDescending(c => c.ToHour).ToList();
                    break;

                case "VacationDate":
                    vactionViewModel = vactionViewModel.OrderBy(c => c.VacationDate).ToList();
                    break;
                case "vdate_desc":
                    vactionViewModel = vactionViewModel.OrderByDescending(c => c.VacationDate).ToList();
                    break;

                case "Duration":
                    vactionViewModel = vactionViewModel.OrderBy(c => c.Duration).ToList();
                    break;
                case "dur_desc":
                    vactionViewModel = vactionViewModel.OrderByDescending(c => c.Duration).ToList();
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
            HourVacationViewModel HVM = new HourVacationViewModel();
            HVM.UserId = user.Id;
            HVM.UserName = user.FullName;
            HVM.DepartmentName = user.Department.Name;
            HVM.CommunityCenterName = user.CommunityCenter.Name;
            HVM.UserPosition = user.Position;
            HVM.VacationTypes = new List<VacationType>();
            HVM.VacationTypes = db.VacationTypes.Where(v => v.IsActive).ToList();
            return View(HVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "UserId,VacationType,Duration,ApplicationDate,VacationDate,FromHour,ToHour,VacationReason")]HourVacationViewModel vacation)
        {
            if (ModelState.IsValid)
            {
                HourlyVacation HV = vacation;
                HV.CreationDate = DateTime.Now;
                HV.CreatedBy = User.Identity.GetUserName();
                db.HourlyVacations.Add(HV);
                db.SaveChanges();
                // return View();
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
            HourlyVacation hv = db.HourlyVacations.Where(c => c.Id == id).Include("UserInfo").Include("UserInfo.Department").Include("UserInfo.CommunityCenter").FirstOrDefault();
            if (hv == null)
            {
                return HttpNotFound();
            }
            HourVacationViewModel HVM = hv;
            HVM.VacationTypes = db.VacationTypes.Where(c => c.IsActive).ToList();
            return View(HVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VacationType,Duration,ApplicationDate,VacationDate,FromHour,ToHour,VacationReason")] HourVacationViewModel HVM)
        {
            if (ModelState.IsValid)
            {
                HourlyVacation hv = db.HourlyVacations.Where(c => c.Id == HVM.Id).FirstOrDefault();
                hv.VacationTypeId = HVM.VacationType;
                hv.ApplicationDate = Convert.ToDateTime(HVM.ApplicationDate);
                hv.Duration = HVM.Duration;
                hv.FromHour = HVM.FromHour;
                hv.ToHour = HVM.ToHour;
                hv.VacationReason = HVM.VacationReason;
                hv.UpdatedDate = DateTime.Now;
                hv.UpdatedBy = User.Identity.GetUserName();
                db.Entry(hv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(HVM);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HourlyVacation hv = db.HourlyVacations.Where(c => c.Id == id && c.IsActive).Include("UserInfo")
                .Include("UserInfo.CommunityCenter").Include("UserInfo.Department").FirstOrDefault();
            HourVacationViewModel HVM = hv;
            if (hv == null)
            {
                return HttpNotFound();
            }
            return View(HVM);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HourlyVacation dv = db.HourlyVacations.Find(id);
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
