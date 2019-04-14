using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
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

        private void btn_LoadLog_Click(object sender, EventArgs e)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<LogDataInfo> Logs = DeviceManipulator.GetAllLogInfo(1, 1).ToList();
            var results =
                from l in Logs
                group l by new { l.EnrollNum, l.LogDate } into g
                select new { Num = g.Key.EnrollNum, Date = g.Key.LogDate, Times = g.OrderBy(c => c.LogTime).ToList() };
            List<LogDataInfo> LastLogs = new List<LogDataInfo>();
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
                        LastLogs.Add(log);
                    }

                }

            }
            LastLogs = LastLogs.OrderBy(c => c.EnrollNum).OrderBy(c => c.LogDate).ToList();
            dataGridView1.DataSource = LastLogs;

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

            //TimeSpan Ts = new TimeSpan(6, 46, 11);
            //TimeSpan Ts2 = new TimeSpan(7, 03, 29);
            //TimeSpan res = Ts2.Subtract(Ts);

        }
    }
}
