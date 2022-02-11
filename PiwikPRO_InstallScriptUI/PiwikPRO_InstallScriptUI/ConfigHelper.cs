using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace PiwikPRO_InstallScriptUI
{
    public partial class Main : Form
    {
        //string configPath = @"C:\Users\lkolodziejczak\source\repos\PiwikPRO.SharePoint\PiwikPRO.SharePoint.PowerShell\PiwikPRO_InstallScriptUI\PiwikPRO_InstallScriptUI\installationConfig.json";
        string configPath = "";// @"C:\LK\PiwikScriptAll\PiwikPRO.SharePoint.PowerShell\SharePoint\installationConfig.json";
        bool startInstall = false;
        bool notTwiceSave = false;
        bool isOnlySave = true;

        public void LoadConfig()
        {
            string readText = File.ReadAllText(configPath);
            jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<InstallationConfig>(readText);
            FillValuesIntoForm();
        }

        public void SaveConfig()
        {
            startInstall = true;
            FillValuesIntoClass();
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(configPath, output, Encoding.UTF8);
        }

        public void SaveCancelInstall()
        {
            notTwiceSave = true;
            string readText = File.ReadAllText(configPath);
            jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<InstallationConfig>(readText);
            jsonObj.cancelInstallation = true;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(configPath, output, Encoding.UTF8);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((!startInstall && !notTwiceSave) || isOnlySave)
            {
                string readText = File.ReadAllText(configPath);
                jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<InstallationConfig>(readText);
                jsonObj.cancelInstallation = true;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(configPath, output, Encoding.UTF8);
            }
        }

        public void FillValuesIntoClass()
        {
            jsonObj.sharePointVersion = Convert.ToString(cmb_SPVersion.SelectedItem);
            jsonObj.sharePointUrl = txt_SPUrl.Text.Trim();
            jsonObj.cancelInstallation = false;
            if (!Convert.ToString(cmb_SPVersion.SelectedItem).Contains("Online"))
            {
                if (txt_SPAdminLogin.Text.Trim().Contains("\\"))
                {
                    jsonObj.sharepointAdminLogin = txt_SPAdminLogin.Text.Trim();
                }
                else
                {
                    if (txt_SPAdminLogin.Text.Trim().Contains(@"\"))
                    {
                        jsonObj.sharepointAdminLogin = txt_SPAdminLogin.Text.Trim().Replace(@"\", "\\");
                    }
                }

                if (txt_SPSecondAdminLogin.Text.Trim().Contains("\\"))
                {
                    jsonObj.additionalPiwikSiteCollAdmin = txt_SPSecondAdminLogin.Text.Trim();
                }
                else
                {
                    if (txt_SPSecondAdminLogin.Text.Trim().Contains(@"\"))
                    {
                        jsonObj.additionalPiwikSiteCollAdmin = txt_SPSecondAdminLogin.Text.Trim().Replace(@"\", "\\");
                    }

                    if(string.IsNullOrEmpty(txt_SPSecondAdminLogin.Text.Trim()))
                    {
                        jsonObj.additionalPiwikSiteCollAdmin = txt_SPSecondAdminLogin.Text.Trim();
                    }
                }
            }
            else
            {
                jsonObj.sharepointAdminLogin = txt_SPAdminLogin.Text.Trim();
                jsonObj.additionalPiwikSiteCollAdmin = txt_SPSecondAdminLogin.Text.Trim();
            }
            jsonObj.serviceUrlValue = txt_PiwikServiceUrl.Text.Trim();
            jsonObj.clientIdValue = txt_PiwikClientID.Text.Trim();
            jsonObj.clientSecretValue = txt_PiwikSecret.Text.Trim();
            jsonObj.containersUrlValue = txt_PiwikContainers.Text.Trim();

            jsonObj.onPremParams.activateFeatureStapplerOnDefault = (bool)chk_SPOnPremActivateFeatureStappler.Checked;

            jsonObj.useWebLogin = (bool)chk_SPO_UseWebLogin.Checked;
            jsonObj.onlineParams.includeAzureInstallation = (bool)chk_SPO_IncludeAzure.Checked;
            jsonObj.onlineParams.includeSharepointInstall = (bool)chk_SPO_IncludeSP.Checked;
            jsonObj.onlineParams.useSiteScope = (bool)chk_SPO_UseSiteScope.Checked;
            jsonObj.onlineParams.AzureTenant = txt_SPO_AzureTenant.Text.Trim();
            jsonObj.onlineParams.AzureSubscription = txt_SPO_AzureSubscription.Text.Trim();
            jsonObj.onlineParams.AzureResourceGroupName = txt_SPO_AzureResourceGroup.Text.Trim();
            jsonObj.onlineParams.AzureWebAppSuffix = txt_SPO_AzureWebAppSuffix.Text.Trim();
            jsonObj.onlineParams.AzureLocation = Convert.ToString(cmb_SPO_AzureLocation.SelectedItem);
            //jsonObj.onlineParams.AzureLocation = txt_SPO_AzureLocation.Text.Trim();
            jsonObj.onlineParams.piwikSiteId = txt_SPO_PiwikPROSiteId.Text.Trim();
        }

        public void FillValuesIntoForm()
        {
            cmb_SPVersion.SelectedItem = jsonObj.sharePointVersion;
            txt_SPUrl.Text = jsonObj.sharePointUrl;

            if (!Convert.ToString(cmb_SPVersion.SelectedItem).Contains("Online"))
            {
                if (jsonObj.sharepointAdminLogin.Contains(@"\"))
                {
                     txt_SPAdminLogin.Text = jsonObj.sharepointAdminLogin;
                }
                else
                {
                    if (jsonObj.sharepointAdminLogin.Contains("\\"))
                    {
                        txt_SPAdminLogin.Text = jsonObj.sharepointAdminLogin.Replace("\\", @"\");
                    }
                }

                if (jsonObj.additionalPiwikSiteCollAdmin.Contains(@"\"))
                {
                    txt_SPSecondAdminLogin.Text = jsonObj.additionalPiwikSiteCollAdmin;
                }
                else
                {
                    if (jsonObj.additionalPiwikSiteCollAdmin.Contains("\\"))
                    {
                        txt_SPSecondAdminLogin.Text = jsonObj.additionalPiwikSiteCollAdmin.Replace("\\", @"\");
                    }
                }
            }
            else
            {
                txt_SPAdminLogin.Text = jsonObj.sharepointAdminLogin;
                txt_SPSecondAdminLogin.Text = jsonObj.additionalPiwikSiteCollAdmin;
            }

            txt_PiwikServiceUrl.Text = jsonObj.serviceUrlValue;
            txt_PiwikClientID.Text = jsonObj.clientIdValue;
            txt_PiwikSecret.Text = jsonObj.clientSecretValue;
            txt_PiwikContainers.Text = jsonObj.containersUrlValue;

            chk_SPOnPremActivateFeatureStappler.Checked = (bool)jsonObj.onPremParams.activateFeatureStapplerOnDefault;

            chk_SPO_UseWebLogin.Checked = (bool)jsonObj.useWebLogin;
            chk_SPO_IncludeAzure.Checked = (bool)jsonObj.onlineParams.includeAzureInstallation;
            chk_SPO_IncludeSP.Checked = (bool)jsonObj.onlineParams.includeSharepointInstall;
            chk_SPO_UseSiteScope.Checked = (bool)jsonObj.onlineParams.useSiteScope;
            txt_SPO_AzureTenant.Text = jsonObj.onlineParams.AzureTenant;
            txt_SPO_AzureSubscription.Text = jsonObj.onlineParams.AzureSubscription;
            txt_SPO_AzureResourceGroup.Text = jsonObj.onlineParams.AzureResourceGroupName;
            txt_SPO_AzureWebAppSuffix.Text = jsonObj.onlineParams.AzureWebAppSuffix;
            cmb_SPO_AzureLocation.SelectedItem = jsonObj.onlineParams.AzureLocation;
            //txt_SPO_AzureLocation.Text = jsonObj.onlineParams.AzureLocation;
            txt_SPO_PiwikPROSiteId.Text = jsonObj.onlineParams.piwikSiteId;

        }
    }

    public class Constants
    {
        public string piwikAdminServerRelativeUrl { get; set; }
        public string filesSolutionFolder { get; set; }
        public string spfxPackagePath { get; set; }
        public string spfxPackagePath2019 { get; set; }
        public string templatePath { get; set; }
        public string tagManagerFolder { get; set; }
    }

    public class ConstantsOnline
    {
        public string appCatalogUrl { get; set; }
        public string piwikMetaSiteNameStored { get; set; }
        public string piwikIsTrackingActive { get; set; }
        public string webJobVersion { get; set; }
    }

    public class OnlineParams
    {
        public bool useSiteScope { get; set; }
        public string piwikSiteId { get; set; }
        public bool includeAzureInstallation { get; set; }
        public bool includeSharepointInstall { get; set; }
        public string AzureTenant { get; set; }
        public string AzureSubscription { get; set; }
        public string AzureResourceGroupName { get; set; }
        public string AzureWebAppSuffix { get; set; }
        public string AzureLocation { get; set; }
        public ConstantsOnline constantsOnline { get; set; }
    }

    public class ConstantsOnPrem
    {
        public string wspSolutionPath { get; set; }
        public string appCatalogUrl { get; set; }
        public string tenantAdminServerRelativeUrl { get; set; }
        public string MywspName2013 { get; set; }
        public string MywspName2019 { get; set; }
        public string timerJobName { get; set; }
        public string webJobVersion { get; set; }
    }

    public class OnPremParams
    {
        public bool activateFeatureStapplerOnDefault { get; set; }
        public bool activateTrackerOnOldConnectorSites { get; set; }
        public ConstantsOnPrem constantsOnPrem { get; set; }
    }

    public class InstallationConfig
    {
        public string sharePointUrl { get; set; }
        public bool useWebLogin { get; set; }
        public string sharePointVersion { get; set; }
        public string clientIdValue { get; set; }
        public string clientSecretValue { get; set; }
        public string serviceUrlValue { get; set; }
        public string containersUrlValue { get; set; }
        public string sharePointTenantAdminUrl { get; set; }
        public string additionalPiwikSiteCollAdmin { get; set; }
        public string sharepointAdminLogin { get; set; }
        public bool cancelInstallation { get; set; }
        public Constants constants { get; set; }
        public OnlineParams onlineParams { get; set; }
        public OnPremParams onPremParams { get; set; }
    }
}
