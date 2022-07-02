using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiwikPRO_InstallScriptUI
{
    public partial class Main : Form
    {
        public bool ValidateData()
        {
            bool returner = true;
            errors.AppendLine("There are errors in the form, please correct the points below:");
            if (!Convert.ToString(cmb_SPVersion.SelectedItem).Contains("Online"))
            {
                if (!txt_SPAdminLogin.Text.Trim().Contains(@"\"))
                {
                    returner = false;
                    errors.AppendLine(@"Sharepoint Admin Login: is empty or filled in incorrect format. The correct is: domain\userid");
                }
                if (!String.IsNullOrEmpty(txt_SPSecondAdminLogin.Text.Trim()) && !txt_SPSecondAdminLogin.Text.Trim().Contains(@"\"))
                {
                    returner = false;
                    errors.AppendLine(@"Secondary Admin Login: is empty or filled in incorrect format. The correct is: domain\userid");
                }
            }
            else
            {
                if (!IsValidEmail(txt_SPAdminLogin.Text.Trim()))
                {
                    returner = false;
                    errors.AppendLine(@"Sharepoint Admin Login: is empty or is not an email of the user account.");
                }
                if (!String.IsNullOrEmpty(txt_SPSecondAdminLogin.Text.Trim()) && !IsValidEmail(txt_SPSecondAdminLogin.Text.Trim()))
                {
                    returner = false;
                    errors.AppendLine(@"Secondary Admin Login: is empty or is not an email of the user account.");
                }

                if(String.IsNullOrEmpty(txt_SPAdminTenantUrl.Text))
                {
                    returner = false;
                    errors.AppendLine(@"SharePoint tenant admin URL cannot be empty.");
                }
            }

            if(txt_PiwikClientID.Text.Length != 32)
            {
                returner = false;
                errors.AppendLine(@"Piwik PRO Client ID: wrong length.");
            }

            if (txt_PiwikSecret.Text.Length != 80)
            {
                returner = false;
                errors.AppendLine(@"Piwik PRO Secret: wrong length.");
            }

            return returner;
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
