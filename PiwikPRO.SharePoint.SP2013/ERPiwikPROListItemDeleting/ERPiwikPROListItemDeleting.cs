using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using PiwikPRO.SharePoint.Shared;

namespace PiwikPRO.SharePoint.SP2013.ERPiwikPROListItemDeleting
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class ERPiwikPROListItemDeleting : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being deleted.
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            if (properties.List.Title.Equals(ConfigValues.PiwikPro_SiteDirectoryListName))
            {
                if (properties.ListItem[ConfigValues.PiwikPro_SiteDirectory_Column_Status].Equals(ConfigValues.PiwikPro_SiteDirectory_Column_Status_Active) ||
                    properties.ListItem[ConfigValues.PiwikPro_SiteDirectory_Column_Status].Equals(ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating))
                {
                    properties.Status = SPEventReceiverStatus.CancelWithError;
                    properties.ErrorMessage = "This site cannot be deleted because the tracking is still active.";
                }
                else
                {
                    base.ItemDeleting(properties);
                }
            }
            else
            {
                base.ItemDeleting(properties);
            }
        }


    }
}