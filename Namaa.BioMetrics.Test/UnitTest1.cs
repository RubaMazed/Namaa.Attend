using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Namaa.BioMetrics.Data;
using Namaa.BioMetrics.Model;
namespace Namaa.BioMetrics.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                List<LogDataInfo> Logs = context.LogDataInfos.ToList();
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
                        LastLogs.Add(log);
                    }

                }
                LastLogs = LastLogs.OrderBy(c => c.EnrollNum).OrderBy(c => c.LogDate).ToList();
            }
        }
    }
}
