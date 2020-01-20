using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace PiwikPRO.SharePoint.SP2013
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class PiwikPRO_MasterPageER : SPItemEventReceiver
    {
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            try
            {
                SPListItem item = properties.ListItem;
                if (item != null)
                {
                    //check if item is master file 
                    if (properties.ListItem.File.Name.ToLower().EndsWith(".master"))
                    {
                        //in case of publish 
                        if (Convert.ToString(properties.AfterProperties["vti_doclibmodstat"]) == Convert.ToString((int)SPModerationStatusType.Approved))
                        {
                            //in case if the list requires check out of document 
                            if (properties.List.ForceCheckout)
                            {
                                AddJSToMasterFileAndCheckinAndPublish(properties, true, true);
                            }
                            else
                            {
                                AddJSToMasterFileAndCheckinAndPublish(properties, false, true);
                            }
                        }
                        //in case of upload new doc 
                        else
                        {
                            //in case if the list requires check out of document 
                            if (properties.List.ForceCheckout)
                            {
                                if (properties.AfterProperties["vti_sourcecontrolcheckedoutby"] == null
                                     && properties.BeforeProperties["vti_sourcecontrolcheckedoutby"] != null)
                                {
                                    AddJSToMasterFileAndCheckinAndPublish(properties, true, false);
                                }
                            }
                            else
                            {
                                SPPropertyBag currentBag = item.Web.Properties;
                                string docID = item.UniqueId.ToString();
                                if ((currentBag.ContainsKey(docID) && currentBag[docID] != null) || properties.ListItem.File.Versions.Count == 0)
                                {
                                    currentBag[docID] = null;
                                    currentBag.Update();
                                    item.Web.Update();
                                    AddJSToMasterFileAndCheckinAndPublish(properties, false, false);
                                }
                                else
                                {
                                    currentBag.Add(docID, "added");
                                    currentBag.Update();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik ItemUpdated", ex.Message);
            }

            base.ItemUpdated(properties);
        }

        private void AddJSToMasterFileAndCheckinAndPublish(SPItemEventProperties properties, bool isCheckOut, bool publishOrNot)
        {
            bool allowunsafeUpdates = properties.Web.AllowUnsafeUpdates;
            //File.Versions[properties.ListItem.File.Versions.Count - 1] 
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            EventDisabler eventDisabler = new EventDisabler();
            SPFile file = properties.ListItem.File;
            byte[] byteArrayOfFile = file.OpenBinary();
            if (byteArrayOfFile.Length > 0)
            {
                PropertyBagOperations pbo = new PropertyBagOperations();
                string trackerJSScriptUrl = pbo.GetPropertyValueFromListByKey("piwik_trackerjsscripturl");
                string strFileContentsBefore = enc.GetString(byteArrayOfFile);
                //check if javascript exists 
                if (!strFileContentsBefore.ToLower().Contains(trackerJSScriptUrl.ToLower()))
                {
                    string newStr = AddJSRefToFile(strFileContentsBefore, trackerJSScriptUrl);
                    byte[] byteArrayFileContentsAfter = null;
                    if (!newStr.Equals(""))
                    {
                        //after binary to string there are ??? chars at the first line, this is the action to replace it: 
                        if (newStr.Substring(0, 3).Equals("???"))
                        {
                            newStr = newStr.Replace("???", "");
                        }
                        byteArrayFileContentsAfter = enc.GetBytes(newStr);
                        eventDisabler.DisableEvents();
                        this.EventFiringEnabled = false;
                        if (isCheckOut)
                        {
                            properties.ListItem.File.CheckOut();
                        }
                        properties.ListItem.File.SaveBinary(byteArrayFileContentsAfter);

                        properties.ListItem.File.Update();

                        try
                        {
                            properties.Web.AllowUnsafeUpdates = true;
                            // Checks if the File is checked out by a user 
                            if (properties.ListItem.File.Level == SPFileLevel.Checkout)
                                // Check in if its checked out 
                                properties.ListItem.File.CheckIn("", SPCheckinType.MajorCheckIn);
                            // Publish / Approve the file... 
                            if (publishOrNot && properties.ListItem.File.Level == SPFileLevel.Draft)
                            {
                                if (properties.ListItem.File.DocumentLibrary.EnableModeration)
                                    properties.ListItem.File.Approve("");
                                else
                                    properties.ListItem.File.Publish("");
                            }
                            properties.ListItem.File.Update();


                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog(Logger.Category.Unexpected, "Piwik AddJSToMasterFileAndCheckinAndPublish", ex.Message);
                        }
                        finally
                        {
                            this.EventFiringEnabled = true;
                        }
                    }
                }
            }
            properties.Web.AllowUnsafeUpdates = allowunsafeUpdates;
        }

        private string AddJSRefToFile(string strFileContentsBefore, string trackerJSScriptUrl)
        {
            string newStr = string.Empty;

            //string newStr = ReplaceFirst(strFileContentsBefore, "\r\n\t<SharePoint:ScriptLink", "\r\n\t<script language=\"javascript\" src=\"https://audi.madeinpoint.com/sites/piwikadmin/Style%20Library/js/piwikJS.js\"></script>\r\n\t<SharePoint:ScriptLink"); 
            if (strFileContentsBefore.Contains("<head runat=\"server\">\r\n\t"))
            {
                newStr = ReplaceFirst(strFileContentsBefore, "<head runat=\"server\">\r\n\t", "<head runat=\"server\">\r\n\t<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>\r\n\t");
            }
            else
            {
                if (strFileContentsBefore.Contains("<head runat=\"server\">\r\n"))
                {
                    newStr = ReplaceFirst(strFileContentsBefore, "<head runat=\"server\">\r\n", "<head runat=\"server\">\r\n<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>\r\n");
                }
                if (strFileContentsBefore.Contains("<head>"))
                {
                    newStr = ReplaceFirst(strFileContentsBefore, "<head>", "<head>\r\n<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>\r\n");
                }
            }

            return newStr;
        }

        private string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }

    public class EventDisabler : SPEventReceiverBase
    {
        public void DisableEvents()
        {
            DisableEventFiring();
        }
        public void EnableEvents()
        {
            EnableEventFiring();
        }
    }
}