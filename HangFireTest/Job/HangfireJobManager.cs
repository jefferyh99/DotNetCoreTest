using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFireTest.Job
{
    public class HangfireJobManager
    {
        public static void ConfigJob()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring!" + "Time:" + DateTime.Now), Cron.Minutely);
        }
    }
}
