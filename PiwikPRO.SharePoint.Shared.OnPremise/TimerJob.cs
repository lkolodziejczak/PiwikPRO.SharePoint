using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client;
using System;

namespace PiwikPRO.SharePoint.Shared.OnPremise
{
    class TimerJob : SPJobDefinition
    {
        public TimerJob() : base()
        {
        }

        public TimerJob(string jobName, SPService service, SPServer server, SPJobLockType targetType)
          : base(jobName, service, server, targetType)
        {
        }

        public TimerJob(string jobName, SPWebApplication webApplication)
          : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            this.Title = "PiwikPRO TimerJob";
        }

        public override void Execute(Guid contentDbId)
        {
            using (ClientContext ctx = new ClientContext("siteUrl"))
            {
                ListProcessor listProcessor = new ListProcessor(ctx, new Configuration((SPWebApplication)this.Parent));
                listProcessor.Execute();
            }
        }
    }
}
