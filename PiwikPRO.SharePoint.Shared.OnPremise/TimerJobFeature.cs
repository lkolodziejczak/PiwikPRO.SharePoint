using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace PiwikPRO.SharePoint.Shared.OnPremise
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("b4f0cc02-b45b-4e8f-aa92-93ed6d3014a6")]
    public class TimerJobFeature : SPFeatureReceiver
    {
        private const string JobName = "PiwikPRO TimerJob";

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                //SPSite site = properties.Feature.Parent as SPSite;
                SPSecurity.RunWithElevatedPrivileges(delegate () {
                    SPWebApplication parentWebApp = (SPWebApplication)properties.Feature.Parent;
                    // make sure the job isn't already registered   
                    foreach (SPJobDefinition job in parentWebApp.JobDefinitions)
                    {
                        if (job.Name == "Piwik PRO Analytics Job")
                        {
                            job.Delete();
                            break;
                        }
                    }
                    // install the job   
                    PiwikPROJSProvisioningJob corpProfileJob = new PiwikPROJSProvisioningJob("Piwik PRO Analytics Job", parentWebApp);
                    // Updates the timer schedule values   
                    SPHourlySchedule schedule = new SPHourlySchedule();
                    schedule.BeginMinute = 0;
                    schedule.EndMinute = 5;
                    corpProfileJob.Schedule = schedule;
                    parentWebApp.JobDefinitions.Add(corpProfileJob);
                });
            }
            catch (Exception ex)
            {
                //Logger.WriteLog(Logger.Category.Unexpected, "Piwik FeatureActivated timer job", ex.Message);
            }

            //SPSite site = properties.Feature.Parent as SPSite;

            //// make sure the job isn't already registered
            //foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            //{
            //    if (job.Name == JobName)
            //        job.Delete();
            //}

            //// install the job
            //TimerJob timerJob = new TimerJob(JobName, site.WebApplication);

            //SPMinuteSchedule schedule = new SPMinuteSchedule
            //{
            //    BeginSecond = 0,
            //    EndSecond = 59,
            //    Interval = 5
            //};
            //timerJob.Schedule = schedule;

            //timerJob.Update();
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                //SPSite site = properties.Feature.Parent as SPSite;
                SPSecurity.RunWithElevatedPrivileges(delegate () {
                    // delete the job   
                    SPWebApplication parentWebApp = (SPWebApplication)properties.Feature.Parent;

                    foreach (SPJobDefinition lockjob in parentWebApp.JobDefinitions)
                    {
                        if (lockjob.Name == "Piwik PRO Analytics Job")
                        {
                            lockjob.Delete();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
               // Logger.WriteLog(Logger.Category.Unexpected, "Piwik FeatureDeactivating timer job", ex.Message);
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
