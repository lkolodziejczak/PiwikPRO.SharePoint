using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiwikPRO_InstallScriptUI
{
    public partial class Main : Form
    {
        InstallationConfig jsonObj;
        private string spVersion;
        ResourceManager rm;
        StringBuilder errors;
        public Main()
        {
            InitializeComponent();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            errors = new StringBuilder();
            if (ValidateData())
            {
                SaveConfig();
                isOnlySave = false;
                Application.Exit();
            }
            else
            {
                MessageBox.Show(Convert.ToString(errors), "Form Errors");
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            errors = new StringBuilder();
            if (ValidateData())
            {
                SaveConfig();
                isOnlySave = true;
            }
            else
            {
                MessageBox.Show(Convert.ToString(errors), "Form Errors");
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            SaveCancelInstall();
            Application.Exit();
        }        

        private void Main_Load(object sender, EventArgs e)
        {
            configPath = System.IO.Directory.GetCurrentDirectory() + "\\installationConfig.json";
            grp_SPOnline.Location = new Point(15, 319);
            rm = new ResourceManager("PiwikPRO_InstallScriptUI.ToolTipTexts", Assembly.GetExecutingAssembly());
            jsonObj = new InstallationConfig();
            LoadConfig();
            Chk_UseSiteScopeChanged();
            Chk_IncludeAzurePartChanged();
            if (Convert.ToString(cmb_SPVersion.SelectedItem).Contains("Online"))
            {
                spVersion = "Online";
                grp_SPOnline.Visible = true;
                grp_SPOnPrem.Visible = false;
            }
            else
            {
                spVersion = "OnPrem";
                grp_SPOnline.Visible = false;
                grp_SPOnPrem.Visible = true;
            }
        }

        private void cmb_SPVersion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void chk_SPO_UseSiteScope_CheckedChanged(object sender, EventArgs e)
        {
            Chk_UseSiteScopeChanged();
        }

        private void chk_SPO_IncludeAzure_CheckedChanged(object sender, EventArgs e)
        {
            Chk_IncludeAzurePartChanged();
        }

        private void Chk_UseSiteScopeChanged()
        {
            if (chk_SPO_UseSiteScope.Checked)
            {
                txt_SPO_PiwikPROSiteId.Visible = true;
                lbl_SPOnlinePiwikPROSiteId.Visible = true;
                lbl_PiwikPROSiteIDImportant.Visible = true;
                pct_SPO_PiwikPROSiteId.Visible = true;
                chk_SPO_IncludeAzure.Checked = false;
            }
            else
            {
                txt_SPO_PiwikPROSiteId.Visible = false;
                lbl_SPOnlinePiwikPROSiteId.Visible = false;
                lbl_PiwikPROSiteIDImportant.Visible = false;
                pct_SPO_PiwikPROSiteId.Visible = false;
            }
        }

        private void Chk_IncludeAzurePartChanged()
        {
            if(chk_SPO_IncludeAzure.Checked)
            {
                lbl_AzureLocation.Visible = true;
                lbl_AzureResourceGroup.Visible = true;
                lbl_AzureSubscription.Visible = true;
                lbl_AzureTenant.Visible = true;
                lbl_AzureWebAppSuffix.Visible = true;
                cmb_SPO_AzureLocation.Visible = true;
                txt_SPO_AzureResourceGroup.Visible = true;
                txt_SPO_AzureSubscription.Visible = true;
                txt_SPO_AzureTenant.Visible = true;
                txt_SPO_AzureWebAppSuffix.Visible = true;
                lbl_Azure1.Visible = true;
                lbl_Azure2.Visible = true;
                lbl_Azure3.Visible = true;
                lbl_Azure4.Visible = true;
                pct_SPO_AzureTenant.Visible = true;
                pct_SPO_AzureLocation.Visible = true;
                pct_SPO_AzureResourceGroup.Visible = true;
                pct_SPO_AzureSubscription.Visible = true;
                pct_SPO_AzureWebAppSuffix.Visible = true;
                chk_SPO_UseSiteScope.Checked = false;
            }
            else
            {
                lbl_AzureLocation.Visible = false;
                lbl_AzureResourceGroup.Visible = false;
                lbl_AzureSubscription.Visible = false;
                lbl_AzureTenant.Visible = false;
                lbl_AzureWebAppSuffix.Visible = false;
                cmb_SPO_AzureLocation.Visible = false;
                txt_SPO_AzureResourceGroup.Visible = false;
                txt_SPO_AzureSubscription.Visible = false;
                txt_SPO_AzureTenant.Visible = false;
                txt_SPO_AzureWebAppSuffix.Visible = false;
                lbl_Azure1.Visible = false;
                lbl_Azure2.Visible = false;
                lbl_Azure3.Visible = false;
                lbl_Azure4.Visible = false;
                pct_SPO_AzureTenant.Visible = false;
                pct_SPO_AzureLocation.Visible = false;
                pct_SPO_AzureResourceGroup.Visible = false;
                pct_SPO_AzureSubscription.Visible = false;
                pct_SPO_AzureWebAppSuffix.Visible = false;
            }
        }

        private void cmb_SPVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(cmb_SPVersion.SelectedItem).Contains("Online"))
            {
                grp_SPOnline.Visible = true;
                grp_SPOnPrem.Visible = false;
                spVersion = "Online";
            }
            else
            {
                grp_SPOnline.Visible = false;
                grp_SPOnPrem.Visible = true;
                spVersion = "OnPrem";
            }
        }

        private void pct_SPAdminLogin_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPAdminLogin_" + spVersion), (PictureBox)sender);
        }

        private void pct_SPSecondAdminLogin_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPSecondAdminLogin_" + spVersion), (PictureBox)sender);
        }

        private void pct_PiwikPROServiceUrl_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("PiwikPROServiceUrl"), (PictureBox)sender);
        }

        private void pct_SharepointURL_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SharepointURL_" + spVersion), (PictureBox)sender);
        }

        private void pct_PiwikPROClientID_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("PiwikPROClientID"), (PictureBox)sender);
        }

        private void pct_PiwikPROSecret_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("PiwikPROSecret"), (PictureBox)sender);
        }

        private void pct_PiwikPROContainers_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("PiwikPROContainer"), (PictureBox)sender);
        }

        private void pct_SPOnPremActivateFeatureStappler_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPActivateStapplerFeature"), (PictureBox)sender);
        }

        private void pct_SPO_UseWebLogin_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPOnline_UseWebLogin"), (PictureBox)sender);
        }

        private void pct_SPO_IncludeAzure_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPOnline_IncludeAzure"), (PictureBox)sender);
        }

        private void pct_SPO_AzureTenant_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPOnline_AzureTenant"), (PictureBox)sender);
        }

        private void pct_SPO_AzureSubscription_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPOnline_AzureSubscription"), (PictureBox)sender);
        }

        private void pct_SPO_AzureResourceGroup_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPOnline_AzureResourceGroup"), (PictureBox)sender);
        }

        private void pct_SPO_AzureWebAppSuffix_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPOnline_AzureWebAppSuffix"), (PictureBox)sender);
        }

        private void pct_SPO_AzureLocation_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPOnline_AzureLocation"), (PictureBox)sender);
        }

        private void pct_SPO_UseSiteScope_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPOnline_UseSiteScope"), (PictureBox)sender);
        }

        private void pct_SPO_PiwikPROSiteId_Click(object sender, EventArgs e)
        {
            toolTip_Piwik.Show(rm.GetString("SPOnline_PiwikPROSiteID"), (PictureBox)sender);
        }

        private void btn_TestLoadCFG_Click(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void cmb_SPO_AzureLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = true;
        }
    }
}
