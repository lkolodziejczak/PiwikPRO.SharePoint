using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;

namespace PiwikPRO.SharePoint.SP2013.Features.PiwikPRO.EnableTracking
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("1333f65b-8f6e-477d-9a4e-e4cabf0b0c2d")]
    public class PiwikPROEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite osite = ((SPSite)properties.Feature.Parent);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (SPSite elevatedSite = new SPSite(osite.ID))
                    {
                        ClientContext ctx = new ClientContext(elevatedSite.Url);
                        ctx.ExecuteQuery();
                        if (ctx.ServerVersion.Major == 16 && ctx.ServerVersion.Build > 10000)
                        {
                            //sp2019
                            AddCustomAction("piwikpro-sharepoint-ListTracking-100", ctx, "a0a0acea-cd3c-454b-9376-9cd0e98f5847", "piwikpro-sharepoint-ListTracking-100", "piwikpro-sharepoint-ListTracking-100", "Adds ListTrackingCommandSet100 to the site", "ClientSideExtension.ListViewCommandSet");
                            AddCustomAction("piwikpro-sharepoint-ListTracking-101", ctx, "a0a0acea-cd3c-454b-9376-9cd0e98f5847", "piwikpro-sharepoint-ListTracking-101", "piwikpro-sharepoint-ListTracking-101", "Adds ListTrackingCommandSet101 to the site", "ClientSideExtension.ListViewCommandSet");
                            AddCustomAction("piwikpro-sharepoint-ListTracking-119", ctx, "a0a0acea-cd3c-454b-9376-9cd0e98f5847", "piwikpro-sharepoint-ListTracking-119", "piwikpro-sharepoint-ListTracking-119", "Adds ListTrackingCommandSet119 to the site", "ClientSideExtension.ListViewCommandSet");
                            AddCustomAction("piwikpro-sharepoint-Tracking", ctx, "2ff5e374-69cb-4645-9083-b6317019705b", "piwikpro-sharepoint-Tracking", "piwikpro-sharepoint-Tracking", "Adds TrackingApplicationCustomizer to the site", "ClientSideExtension.ApplicationCustomizer");
                        }
                    }
                }
                catch { }
            });
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite osite = ((SPSite)properties.Feature.Parent);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (SPSite elevatedSite = new SPSite(osite.ID))
                    {
                        ClientContext ctx = new ClientContext(elevatedSite.Url);
                        ctx.ExecuteQuery();
                        if (ctx.ServerVersion.Major == 16 && ctx.ServerVersion.Build > 10000)
                        {
                            //sp2019
                            DeleteCustomAction("piwikpro-sharepoint-ListTracking-100", ctx);
                            DeleteCustomAction("piwikpro-sharepoint-ListTracking-101", ctx);
                            DeleteCustomAction("piwikpro-sharepoint-ListTracking-119", ctx);
                            DeleteCustomAction("piwikpro-sharepoint-Tracking", ctx);
                        }
                    }
                }
                catch { }
            });

        }

        private static void DeleteCustomAction(string customActionName, ClientContext ctx)
        {
            var userCustomActions = ctx.Site.UserCustomActions;
            ctx.Load(userCustomActions);
            ctx.ExecuteQueryRetry();

            for (int i = userCustomActions.Count - 1; i >= 0; i--)
            {
                if (userCustomActions[i].Name == customActionName)
                {
                    userCustomActions[i].DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
            }
        }

        private static void AddCustomAction(string customActionName, ClientContext ctx, string guid, string spfxExtName, string spfxExtTitle, string spfxExtDescription, string spfxExtLocation)
        {
            bool checkIfExists = false;

            var userCustomActions = ctx.Site.UserCustomActions;
            ctx.Load(userCustomActions);
            ctx.ExecuteQueryRetry();

            for (int i = userCustomActions.Count - 1; i >= 0; i--)
            {
                if (userCustomActions[i].Name == customActionName)
                {
                    checkIfExists = true;
                }
            }

            if (!checkIfExists)
            {
                Guid spfxExtension_GlobalHeaderID = new Guid(guid);

                UserCustomAction userCustomAction = ctx.Site.UserCustomActions.Add();
                userCustomAction.Name = spfxExtName;
                userCustomAction.Title = spfxExtTitle;
                userCustomAction.Description = spfxExtDescription;
                userCustomAction.Location = spfxExtLocation;
                userCustomAction.ClientSideComponentId = spfxExtension_GlobalHeaderID;
                userCustomAction.ClientSideComponentProperties = "{}";
                userCustomAction.Update();
                ctx.ExecuteQueryRetry();
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
