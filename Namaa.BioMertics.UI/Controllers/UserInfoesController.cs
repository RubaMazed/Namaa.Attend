using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Namaa.BioMertics.UI.Models;
using Namaa.BioMetrics.Data;
using Namaa.BioMetrics.Model;
using Namaa.BioMetrics.Utilities;
using Namaa.BioMetrics.Utilities.Enums;
using WebGrease.Css.Extensions;
using PagedList;
using Microsoft.AspNet.Identity;

namespace Namaa.BioMertics.UI.Controllers
{
    [Authorize]
    public class UserInfoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserInfoes
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumSortParm = String.IsNullOrEmpty(sortOrder) ? "num_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.PositionSortParm = sortOrder == "Position" ? "position_desc" : "Position";
            ViewBag.CenterSortParm = sortOrder == "CenterName" ? "cname_desc" : "CenterName";
            ViewBag.DeptSortParm = sortOrder == "DepartmentName" ? "dname_desc" : "DepartmentName";

            var users2 = db.UserInfos.Where(c => c.IsActive).Include("Department").Include("CommunityCenter").ToList();
            List<UserInfoViewModel> UserViews = new List<UserInfoViewModel>();
            foreach (var u in users2)
            {
                UserInfoViewModel uvm = u;
                UserViews.Add(u);
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
                UserViews = UserViews.Where(u => u.FullName.ToLower().Contains(searchString.ToLower()) ||
                                            u.EnrollNumber.ToString().ToLower().Contains(searchString.ToLower()))
                                          .ToList();
            }
            switch (sortOrder)
            {
                case "num_desc":
                    UserViews = UserViews.OrderByDescending(c => c.EnrollNumber).ToList();
                    break;
                case "Name":
                    UserViews = UserViews.OrderBy(c => c.FullName).ToList();
                    break;
                case "name_desc":
                    UserViews = UserViews.OrderByDescending(c => c.FullName).ToList();
                    break;
                case "position_desc":
                    UserViews = UserViews.OrderByDescending(c => c.Position).ToList();
                    break;
                case "Position":
                    UserViews = UserViews.OrderBy(c => c.Position).ToList();
                    break;
                case "cname_desc":
                    UserViews = UserViews.OrderByDescending(c => c.CommunityCentreName).ToList();
                    break;
                case "CenterName":
                    UserViews = UserViews.OrderBy(c => c.CommunityCentreName).ToList();
                    break;
                case "dname_desc":
                    UserViews = UserViews.OrderByDescending(c => c.DepartmentName).ToList();
                    break;
                case "DepartmentName":
                    UserViews = UserViews.OrderBy(c => c.DepartmentName).ToList();
                    break;
                default:
                    UserViews = UserViews.OrderBy(c => c.EnrollNumber).ToList();
                    break;
            }
            int pageSize = (int)Math.Ceiling(UserViews.Count / 10.0);
            int pageNumber = (page ?? 1);
            return View(UserViews.ToPagedList(pageNumber, pageSize));

            #region Comment

            //var users = db.UserInfos.Include("Department").Include("CommunityCenter").ToList();


            //List<UserInfoViewModel> UserViews = new List<UserInfoViewModel>();
            //foreach (var u in users)
            //{
            //    UserInfoViewModel uvm = u;
            //    UserViews.Add(u);
            //}
            //  AutoMapper.Mapper.Map<List<UserInfoViewModel>, List<UserInfo>>(UserViews, users);
            // return View(UserViews);

            // ViewBag.CurrentSort = sortOrder; 

            #endregion


            //else
            //    return View(new List<UserInfoViewModel>());

        }

        // GET: UserInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo userInfo = db.UserInfos.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // GET: UserInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EnrollNumber,Name,FingerIndex,Password,Enabled")] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                db.UserInfos.Add(userInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userInfo);
        }

        // GET: UserInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo userInfo = db.UserInfos.Where(c => c.EnrollNumber == id.ToString()).Include("Department").Include("CommunityCenter").FirstOrDefault();
            UserInfoViewModel UserViewModel = userInfo;
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            UserViewModel.CommunityCenters = db.CommunityCenters.Where(c => c.IsActive).ToList();
            UserViewModel.Departments = db.Departments.Where(c => c.IsActive).ToList();

            return View(UserViewModel);
        }

        // POST: UserInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,EnrollNumber,BirthDate,StartDate,Position,CommunityCentre,Department")] UserInfoViewModel userInfo)
        {
            if (ModelState.IsValid)
            {
                UserInfo updatedInfo = db.UserInfos.Where(c => c.EnrollNumber == userInfo.Id.ToString()).FirstOrDefault();
                // updatedInfo = userInfo;
                updatedInfo.FullName = userInfo.FullName;
                updatedInfo.BirthDate = Convert.ToDateTime(userInfo.BirthDate);
                updatedInfo.StartDate = Convert.ToDateTime(userInfo.StartDate);
                updatedInfo.Position = userInfo.Position;
                updatedInfo.CommunityCenterId = userInfo.CommunityCentre;
                updatedInfo.DepartmentId = userInfo.Department;
                updatedInfo.UpdatedDate = DateTime.Now;
                updatedInfo.UpdatedBy = User.Identity.GetUserName();
                db.Entry(updatedInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userInfo);
        }

        // GET: UserInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo userInfo = db.UserInfos.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserInfo userInfo = db.UserInfos.Find(id);
            db.UserInfos.Remove(userInfo);
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
