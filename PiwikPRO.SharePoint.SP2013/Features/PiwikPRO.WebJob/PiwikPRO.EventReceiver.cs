using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using PiwikPRO.SharePoint.Shared;

namespace PiwikPRO.SharePoint.SP2013.Features.PiwikPRO.WebJob
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("7652f168-f6be-4775-bcd9-8ec5bb5ea3f4")]
    public class PiwikPROEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                //SPSite site = properties.Feature.Parent as SPSite;
                SPSecurity.RunWithElevatedPrivileges(delegate () {
                    SPWebApplication parentWebApp = (SPWebApplication)properties.Feature.Parent;
                    //SPWebApplication parentWebApp = parentSite.WebApplication;
                    // make sure the job isn't already registered   
                    foreach (SPJobDefinition job in parentWebApp.JobDefinitions)
                    {
                        if (job.Name == ConfigValues.PiwikPro_JobName)
                        {
                            job.Delete();
                            break;
                        }
                    }
                    // install the job   
                    PiwikPROJSProvisioningJob corpProfileJob = new PiwikPROJSProvisioningJob(ConfigValues.PiwikPro_JobName, parentWebApp);
                    corpProfileJob.Id = new Guid("5367FEE9-CFBC-468C-9A6A-C3367E6322C6");
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
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik FeatureActivated timer job", ex.Message);
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                //SPSite site = properties.Feature.Parent as SPSite;
                SPSecurity.RunWithElevatedPrivileges(delegate () {
                    // delete the job   
                    SPSite parentSite = (SPSite)properties.Feature.Parent;
                    SPWebApplication parentWebApp = parentSite.WebApplication;

                    foreach (SPJobDefinition lockjob in parentWebApp.JobDefinitions)
                    {
                        if (lockjob.Name == ConfigValues.PiwikPro_JobName)
                        {
                            lockjob.Delete();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik FeatureDeactivating timer job", ex.Message);
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
