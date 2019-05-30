using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using zkemkeeper;
using Namaa.BioMetrics.Utilities.Enums;
using Namaa.BioMetrics.Model;
namespace Namaa.BioMetrics.Utilities
{
    public static class DeviceManipulator
    {
        public static CZKEM DeviceCzkem { get; set; }
        /// <summary>
        /// Connect To Device 
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="portNum"></param>
        /// <returns></returns>
        public static ConnectStatus ConnectDevice(string ipAddress, int portNum)
        {
            if (DeviceCzkem == null)
            {
                DeviceCzkem = new CZKEM();
            }
            if (!ValidateIP(ipAddress))
            {
                return ConnectStatus.IPNotValid;
            }
            else
            {
                bool? isConnect = DeviceCzkem.Connect_Net(ipAddress, portNum);
                return isConnect.Value ? ConnectStatus.Connected : ConnectStatus.NotConnected;
            }

        }
        private static bool ValidateIP(string addrString)
        {
            IPAddress address;
            if (IPAddress.TryParse(addrString, out address))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Read Users From Devices
        /// </summary>
        /// <param name="machineNumber"></param>
        /// <returns></returns>
        public static ICollection<UserInfo> GetAllUserInfo(int machineNumber)
        {
            string sdwEnrollNumber = string.Empty,
                sName = string.Empty,
                sPassword = string.Empty,
                sTmpData = string.Empty;
            int iPrivilege = 0, iTmpLength = 0, iFlag = 0, idwFingerIndex;
            bool bEnabled = false;
            ICollection<UserInfo> uSers = new List<UserInfo>();
            DeviceCzkem.ReadAllUserID(machineNumber);
            DeviceCzkem.ReadAllTemplate(machineNumber);
            while (DeviceCzkem.SSR_GetAllUserInfo(machineNumber, out sdwEnrollNumber, out sName, out sPassword,
                out iPrivilege, out bEnabled))
            {
                for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    if (DeviceCzkem.GetUserTmpExStr(machineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))
                    {
                        UserInfo userinfo = new UserInfo()
                        {
                            EnrollNumber = sdwEnrollNumber,
                            FullName = sName,
                            FingerIndex = idwFingerIndex,
                            Enabled = bEnabled,
                            Password = sPassword
                        };
                        uSers.Add(userinfo);
                    }
                }
            }
            return uSers;

        }
        /// <summary>
        ///Get All Log Data For All User
        /// </summary>
        /// <param name="machineNumber"></param>
        /// <returns></returns>
        public static ICollection<LogDataInfo> GetAllLogInfo(int machineNumber, int centerId, DateTime fromDate, DateTime toDate)
        {
            DeviceCzkem.ReadAllGLogData(machineNumber);
            ICollection<LogDataInfo> LogInfoList = new List<LogDataInfo>();
            string num;
            int vmode;
            int outMode;
            int year;
            int month;
            int day;
            int Hour, Min, Second;
            int workCode = 0;

            while (DeviceCzkem.SSR_GetGeneralLogData(machineNumber, out num, out vmode, out outMode,
                out year, out month, out day, out Hour, out Min, out Second, ref workCode))
            {
                var date = new DateTime(year, month, day);
                if (date >= fromDate && date <= toDate)
                {
                    LogDataInfo log = new LogDataInfo()
                    {
                        EnrollNum = num,
                        LogDate = new DateTime(year, month, day),
                        LogTime = new TimeSpan(Hour, Min, Second),
                        CommunityCenterId = centerId
                    };
                    LogInfoList.Add(log);
                }

            }
            LogInfoList = LogInfoList.OrderBy(c => c.EnrollNum).ThenBy(c => c.LogDate).ThenBy(c => c.LogTime).ToList();

            var result =
                from l in LogInfoList
                group l by new { l.EnrollNum, l.LogDate } into g
                select new { Num = g.Key.EnrollNum, Date = g.Key.LogDate, Times = g.ToList() };

            foreach (var r in result)
            {

            }
            return LogInfoList;
        }

    }
}

