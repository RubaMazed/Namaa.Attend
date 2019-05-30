using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Namaa.BioMetrics.Utilities;
using Namaa.BioMetrics.Data;
using Namaa.BioMetrics.Model;
using zkemkeeper;

namespace Namaa.BioMetrics.AdminUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            string ip = textBox1.Text;
            int port = Convert.ToInt32(textBox2.Text);
            DeviceManipulator.DeviceCzkem = new CZKEM();
            bool? isConnect = DeviceManipulator.DeviceCzkem.Connect_Net(ip, port);
            if (isConnect.HasValue && isConnect.Value)
            {
                MessageBox.Show("Successfully...");
            }
            else
            {
                MessageBox.Show("Error...");
            }


        }

        private void btn_LoadUser_Click(object sender, EventArgs e)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<UserInfo> Users = new List<UserInfo>();
            //  Users = db.UserInfos.ToList();
            List<UserInfo> UserFromDevice = DeviceManipulator.GetAllUserInfo(1).ToList();
            foreach (var usr in UserFromDevice)
            {
                UserInfo user = db.UserInfos.Where(c => c.EnrollNumber == usr.EnrollNumber).FirstOrDefault();
                if (user == null)
                {
                    user = new UserInfo();
                    user.EnrollNumber = usr.EnrollNumber;
                    user.FullName = usr.FullName;
                    user.FingerIndex = usr.FingerIndex;
                    user.Enabled = usr.Enabled;
                    user.Password = usr.Password;
                    user.CreatedBy = "admin";
                    user.CreationDate = DateTime.Now;
                    user.BirthDate = DateTime.Now;
                    user.StartDate = DateTime.Now;
                    user.DepartmentId = 3;
                    //المارتيني
                    user.CommunityCenterId = 1;
                    db.UserInfos.Add(user);
                    db.SaveChanges();
                }
                else
                {
                    user.EnrollNumber = usr.EnrollNumber;
                    user.FullName = usr.FullName;
                    user.FingerIndex = usr.FingerIndex;
                    user.Enabled = usr.Enabled;
                    user.Password = usr.Password;
                    db.Entry(user).State = EntityState.Modified;
                    user.UpdatedDate = DateTime.Now;
                    user.UpdatedBy = "admin";
                    db.SaveChanges();

                }


            }
            MessageBox.Show("All Users Loaded Successfully...");
        }

        private List<LogDataInfo> logs = new List<LogDataInfo>();
        private void btn_LoadLog_Click(object sender, EventArgs e)
        {

            logs.AddRange(DeviceManipulator.GetAllLogInfo(1, 1, dtp_from.Value.Date, dtp_to.Value.Date).ToList());
            //dataGridView1.DataSource = null;
            logs = logs.OrderBy(c => c.LogDate).ToList();
            DGVLogs.DataSource = logs;
            label3.Text = logs.Count.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<UserInfo> Users = db.UserInfos.Where(c => c.IsActive).ToList();
            foreach (var usr in Users)
            {
                if ((usr.EnrollNumber == "1" || usr.EnrollNumber == "2" || usr.EnrollNumber == "7"))
                {
                    DeviceManipulator.DeviceCzkem.SSR_SetUserInfo(1, usr.EnrollNumber, usr.FullName, null, 3, true);
                }
                else
                {
                    DeviceManipulator.DeviceCzkem.SSR_SetUserInfo(1, usr.EnrollNumber, usr.FullName, null, 1, true);
                }

            }
            MessageBox.Show("All User Updated Successfully...");

        }

        private void SaveLog()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var results =
                from l in logs
                group l by new { l.EnrollNum, l.LogDate } into g
                select new { Num = g.Key.EnrollNum, Date = g.Key.LogDate, Times = g.OrderBy(c => c.LogTime).ToList() };
            List<LogDataInfo> lastLogs = new List<LogDataInfo>();
            LogDataInfo log = new LogDataInfo();
            foreach (var r in results)
            {
                for (int i = 0; i <= r.Times.Count - 1;)
                {
                    log = new LogDataInfo();
                    log.EnrollNum = r.Num;
                    log.LogDate = r.Date;
                    log.LogTime = r.Times[i].LogTime;
                    if (i + 1 < r.Times.Count)
                    {
                        log.LogOutTime = r.Times[i + 1].LogTime;
                        i += 2;

                    }
                    else
                    {
                        log.LogOutTime = TimeSpan.Zero;
                        i++;
                    }
                    log.CreatedBy = "admin";
                    //المارتيني
                    log.CommunityCenterId = 1;
                    var user = db.UserInfos.Where(c => c.EnrollNumber == log.EnrollNum).FirstOrDefault();
                    if (user != null)
                    {
                        log.UserInfoId = user.Id;
                        db.LogDataInfos.Add(log);
                        db.SaveChanges();
                        lastLogs.Add(log);
                    }

                }

            }
            lastLogs = lastLogs.OrderBy(c => c.EnrollNum).OrderBy(c => c.LogDate).ToList();
            DGVLogs.DataSource = lastLogs;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //DeviceManipulator.DeviceCzkem.SSR_SetUserInfo(1, "286", "Mohmad Helal Alsheban", null, 1, true);
            //SSR_SetUserInfo(1, 160.ToString(), "Noha Hykal", null, 3, true);
            //DeviceManipulator.DeviceCzkem.SSR_SetUserInfo(1, "7", "Ruba Mazed", null, 3, true);
            //DeviceManipulator.DeviceCzkem.SSR_SetUserInfo(1, "2", "Ahmad Gazal", null, 3, true);
            //DeviceManipulator.DeviceCzkem.SSR_SetUserInfo(1, "1", "Radwan Sorani", null, 3, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region MyRegion

            //int totalPrevVacation = (int)new DateTime(2019, 4, 17).Month;
            //double daydiscount = 0;
            //int restHour = 0;
            //using (var db = new ApplicationDbContext())
            //{
            //    List<DailyVacation> takenVacation = db.DailyVacations.Where(c => c.IsActive && c.FromDate < new DateTime(2019, 4, 17)).Include("UserInfo").ToList();


            //    var dailys = (from d in db.DailyVacations
            //                  where d.UserInfo.CommunityCenterId == 1
            //                        && d.FromDate >= new DateTime(2019, 4, 17) && d.FromDate <= new DateTime(2019, 4, 30)
            //                        && d.UserInfo.EnrollNumber == "10"
            //                  group d by new
            //                  {
            //                      Num = d.UserInfo.EnrollNumber,
            //                      Name = d.UserInfo.FullName,
            //                      CName = d.UserInfo.CommunityCenter.Name,
            //                      DName = d.UserInfo.Department.Name
            //                  } into g
            //                  select new { Num = g.Key.Num, Name = g.Key.Name, Center = g.Key.CName, Department = g.Key.DName, DaySum = g.Sum(c => c.Duration) }).ToList();

            //    var hours = (from h in db.HourlyVacations
            //                 where h.UserInfo.CommunityCenter.Id == 1
            //                       && h.VacationDate >= new DateTime(2019, 4, 17)
            //                       && h.VacationDate <= new DateTime(2019, 4, 30)
            //                        && h.UserInfo.EnrollNumber == "10"
            //                 group h by new
            //                 {
            //                     Num = h.UserInfo.EnrollNumber,
            //                     Name = h.UserInfo.FullName,
            //                     CName = h.UserInfo.CommunityCenter.Name,
            //                     DName = h.UserInfo.Department.Name
            //                 } into g
            //                 select new { Num = g.Key.Num, Name = g.Key.Name, Center = g.Key.CName, Department = g.Key.DName, HourSum = g.Sum(c => c.Duration.Hours * 60 + c.Duration.Minutes) / 60.0 }).ToList();

            //    foreach (var dr in dailys)
            //    {
            //        //*************** حساب الحسم من الاجازات اليومية 
            //        //حساب مجموعة الاجازات في الأشهر السابقة 
            //        var sumtakenVacation = takenVacation?.Where(c => c.UserInfo.EnrollNumber == "10")?.Sum(c => (int?)c.Duration) ?? 0;

            //        //حساب مجموع الاجازات الساعية في هذا الشهر 
            //        var sumHourvacation = hours.Where(c => c.Num == "10").FirstOrDefault()?.HourSum ?? 0;

            //        //الاجازات الكلية =مجموع الاجازات في هذا الشهر +ناتج قسمة ا
            //        var totalVacatin = dr.DaySum + (int)(sumHourvacation / 8);
            //        restHour = (int)sumHourvacation % 8;

            //        var tt = 0.0;
            //        for (int i = 1; i < new DateTime(2019, 4, 17).Month; i++)
            //        {
            //            double? totalHour;

            //            var i1 = i;
            //            totalHour = db.HourlyVacations
            //                               .Where(c => c.IsActive && c.VacationDate.Month == i1 &&
            //                                           c.UserInfo.EnrollNumber == "10")?
            //                               .Sum(c => (double?)c.Duration.Hours + (double)(c.Duration.Minutes) / 60.0) ?? 0;
            //            tt += totalHour.Value % 8;

            //        }

            //        if (new DateTime(2019, 4, 17).Day > 1)
            //        {
            //            var da = new DateTime(new DateTime(2019, 4, 17).Year, new DateTime(2019, 4, 17).Month, 1);
            //            var totalHour = db.HourlyVacations
            //                               .Where(c => c.IsActive
            //                               && c.VacationDate >= da
            //                               && c.VacationDate < new DateTime(2019, 4, 17)
            //                               && c.UserInfo.EnrollNumber == "10")?
            //                               .Sum(c => (double?)c.Duration.Hours + (double)(c.Duration.Minutes / 60.0)) ?? 0;
            //            tt += totalHour % 8;

            //        }
            //        totalVacatin += (int)(tt % 8 + restHour) / 8;
            //        //DayDiscount > 0 يوجد حسم 
            //        //DayDiscount =0 لايوجد حسم 
            //        //DayDiscount <0 الموظف لديه اجازات لم تؤخذ بعد 
            //        if (sumtakenVacation < totalPrevVacation)
            //        {
            //            daydiscount = totalVacatin - ((totalPrevVacation - sumtakenVacation) + 1);
            //        }
            //        else
            //            daydiscount = totalVacatin - (+1);
            //    }
            //} 

            #endregion

            DateTime fromDate = new DateTime(2019, 4, 17);
            DateTime toDate = new DateTime(2019, 5, 17);
            int CommunityCenter = 1;
            int totalPrevVacation = (int)fromDate.Month;
            double daydiscount = 0;
            double restHour = 0.0;
            using (var db = new ApplicationDbContext())
            {
                List<DailyVacation> takenVacation = db.DailyVacations.Where(c => c.IsActive && c.VacationType.IsDiscount && c.FromDate < fromDate)
                    .Include("UserInfo")
                    .ToList();

                var users = (from u in db.UserInfos
                             where u.CommunityCenterId == CommunityCenter
                             select u).Include("CommunityCenter")
                    .Include("Department")
                    .ToList();

                var dailys = (from d in db.DailyVacations
                              where d.UserInfo.CommunityCenterId == CommunityCenter
                                    && d.IsActive
                                    && d.FromDate >= fromDate && d.FromDate <= toDate
                                    && d.VacationType.IsDiscount
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
                                               ?.Sum(c => (int?)c.Duration) ?? 0;

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
                                            ?
                                            .Sum(c => (double?)c.Duration.Hours +
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
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeviceManipulator.DeviceCzkem.SSR_SetUserInfo(1, "1111", "Shahed Tarakji", null, 1, true);
            DeviceManipulator.DeviceCzkem.SSR_SetUserInfo(1, "175", "Joudi Jamali", null, 1, true);
            DeviceManipulator.DeviceCzkem.SSR_SetUserInfo(1, "380", "Rama Jaber", null, 1, true);
            MessageBox.Show("Yes");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //DeviceManipulator.DeviceCzkem.BackupData("D:\\test.xlsx");
            MessageBox.Show((8.5 % 8).ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveLog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TimeSpan ts1 = TimeSpan.Parse(tbFrom.Text);
            TimeSpan ts2 = TimeSpan.Parse(tbTo.Text);
            TimeSpan diff = ts2.Subtract(ts1);
            lbResult.Text = diff.ToString();
        }
    }
}
