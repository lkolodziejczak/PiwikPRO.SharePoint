using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;

namespace PiwikPRO.SharePoint.Shared
{
    public class ListProcessor
    {
        private readonly ClientContext context;
        ISPLogger logger;

        public ListProcessor(ClientContext context, ISPLogger _logger)
        {
            this.context = context;
            this.logger = _logger;
        }

        public bool AddOrUpdateElementInList(string title, string status, string url, string errorlog, string relativeUrl, string siteId)
        {
            bool statusNotChangedEarlier = true;
            bool itemToDelete = false;
            try
            {
                List oList = context.Web.Lists.GetByTitle("Piwik Pro Site Directory");
                ListItem itemToAdd = CheckIfElementIsAlreadyOnList(relativeUrl);
                if (itemToAdd != null)
                {
                    if (Convert.ToString(itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_Status]) == ConfigValues.PiwikPro_SiteDirectory_Column_Status_New && status == ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating && string.IsNullOrEmpty(Convert.ToString(itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID])))
                    {
                        itemToDelete = true;
                    }
                    else
                    {
                        if (Convert.ToString(itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_Status]) == ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating && status == ConfigValues.PiwikPro_SiteDirectory_Column_Status_New)
                        {
                            itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Active;
                            statusNotChangedEarlier = false;
                        }

                        if (Convert.ToString(itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_Status]) == ConfigValues.PiwikPro_SiteDirectory_Column_Status_New && status == ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating)
                        {
                            itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_NoActive;
                            statusNotChangedEarlier = false;
                        }

                        if (statusNotChangedEarlier)
                        {
                            itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = status;
                        }

                        itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_Title] = title;

                        itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_Url] = url;
                        itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = errorlog;
                        if (!string.IsNullOrEmpty(siteId))
                        {
                            itemToAdd[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID] = siteId;
                        }
                    }
                    if (!itemToDelete)
                    {
                        itemToAdd.Update();
                        context.ExecuteQueryRetry();
                    }
                    else
                    {
                        itemToAdd.DeleteObject();
                        context.ExecuteQueryRetry();
                    }
                }
                else
                {
                    ListItem newItem = oList.AddItem(new ListItemCreationInformation());
                    newItem[ConfigValues.PiwikPro_SiteDirectory_Column_Title] = title;
                    newItem[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = status;
                    newItem[ConfigValues.PiwikPro_SiteDirectory_Column_Url] = url;
                    newItem[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = errorlog;
                    if (!string.IsNullOrEmpty(siteId))
                    {
                        newItem[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID] = siteId;
                    }
                    newItem.Update();
                    context.ExecuteQueryRetry();
                }
            }
            catch (Exception ex)
            {
                 logger.WriteLog(Category.Unexpected, "Piwik AddOrUpdateElementInList", ex.Message);
            }
            return statusNotChangedEarlier;
        }

        public List<ListItem> GetAllNewSites()
        {
            List<ListItem> listToReturn = new List<ListItem>();
            try
            {
                List oList = context.Web.Lists.GetByTitle(ConfigValues.PiwikPro_SiteDirectoryListName);
                CamlQuery qry = new CamlQuery();
                qry.ViewXml =
                    @"<View><Query><Where>" +
          "<Eq>" +
             "<FieldRef Name='" + ConfigValues.PiwikPro_SiteDirectory_Column_Status + "' />" +
             "<Value Type='Choice'>" + ConfigValues.PiwikPro_SiteDirectory_Column_Status_New + "</Value>" +
          "</Eq>" +
       "</Where></View></Query>";
                ListItemCollection collListItem = oList.GetItems(qry);

                context.Load(
                collListItem,
                items => items.Include(
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Title],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Url],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Status],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]));

                context.ExecuteQueryRetry();
                foreach (ListItem item in collListItem)
                {
                    listToReturn.Add(item);
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik GetAllNewSites", ex.Message);
            }
            return listToReturn;
        }

        public List<ListItem> GetAllNewSettingsUpdatedSites()
        {
            List<ListItem> listToReturn = new List<ListItem>();
            try
            {
                List oList = context.Web.Lists.GetByTitle(ConfigValues.PiwikPro_SiteDirectoryListName);
                CamlQuery qry = new CamlQuery();
                qry.ViewXml =
                    @"<View><Query><Where>" +
          "<Eq>" +
             "<FieldRef Name='" + ConfigValues.PiwikPro_SiteDirectory_Column_Status + "' />" +
             "<Value Type='Choice'>" + ConfigValues.PiwikPro_SiteDirectory_Column_Status_SettingsUpdated + "</Value>" +
          "</Eq>" +
       "</Where></View></Query>";
                ListItemCollection collListItem = oList.GetItems(qry);

                context.Load(
                collListItem,
                items => items.Include(
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Title],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Url],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Status],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]));

                context.ExecuteQueryRetry();
                foreach (ListItem item in collListItem)
                {
                    listToReturn.Add(item);
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik GetAllNewSettingsUpdatedSites", ex.Message);
            }
            return listToReturn;
        }

        public List<ListItem> GetAllDeactivatingSites()
        {
            List<ListItem> listToReturn = new List<ListItem>();
            try
            {
                List oList = context.Web.Lists.GetByTitle("Piwik Pro Site Directory");
                CamlQuery qry = new CamlQuery();
                qry.ViewXml =
                    @"<View><Query><Where>" +
          "<Eq>" +
             "<FieldRef Name='" + ConfigValues.PiwikPro_SiteDirectory_Column_Status + "' />" +
             "<Value Type='Choice'>" + ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating + "</Value>" +
          "</Eq>" +
       "</Where></View></Query>";
                ListItemCollection collListItem = oList.GetItems(qry);

                context.Load(
                collListItem,
                items => items.Include(
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Title],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Url],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Status],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]));

                context.ExecuteQueryRetry();

                foreach (ListItem item in collListItem)
                {
                    listToReturn.Add(item);
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik GetAllDeactivatingSites", ex.Message);
            }
            return listToReturn;
        }

        public ListItem CheckIfElementIsAlreadyOnList(string url)
        {
            try
            {
                List oList = context.Web.Lists.GetByTitle("Piwik Pro Site Directory");
                CamlQuery qry = new CamlQuery();
                qry.ViewXml =
                @"<View><Query><Where>" +
      "<Eq>" +
         "<FieldRef Name='" + ConfigValues.PiwikPro_SiteDirectory_Column_Url + "' />" +
         "<Value Type='URL'>" + new Uri(url).AbsolutePath + "</Value>" +
      "</Eq>" +
   "</Where></View></Query>";

                ListItemCollection collListItem = oList.GetItems(qry);

                context.Load(
                collListItem,
                items => items.Include(
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Title],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Url],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_Status],
                item => item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]));

                context.ExecuteQueryRetry();
                foreach (ListItem item in collListItem)
                {
                    return item;
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik CheckIfElementIsAlreadyOnList", ex.Message);
            }

            return null;
        }
    }
}
