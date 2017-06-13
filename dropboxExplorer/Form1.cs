using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nemiro.OAuth;
using Nemiro.OAuth.LoginForms;

namespace dropboxExplorer
{
    public partial class Form1 : Form
    {
        private string CurrentPath = "/";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.AccessToken))
                //check for an access token in the application settings
            {
                this.GetAccessToken();
                //AccessToken is empty. receiving
            }
            else
            {
                this.GetFiles();
                // is not empty. Receiving a list of files
            }
        }

        private void GetAccessToken()
        {
            //creates instance of the dropbox login form
            var login = new DropboxLogin("zxz080y5wvs3sa2", "zxz080y5wvs3sa2");
            //show the form as a dialog
            login.Owner = this;
            login.ShowDialog();

           //after successful authentication writes the accesstoken to the app settings.
           if (login.IsSuccessfully)
            {
                Properties.Settings.Default.AccessToken = login.AccessToken.Value;
                Properties.Settings.Default.Save();
            }
           else
            {
                MessageBox.Show("Login or Password are incorrect.");
            }
        }

        private void GetFiles()
        {
            //request a list of files and folders on dropbox account
            OAuthUtility.GetAsync
                (
                "https://api.dropboxapi.com/1/metadata/auto/",
                new HttpParameterCollection
            {
                {"path", this.CurrentPath },
                {"Access_token", Properties.Settings.Default.AccessToken}

            },
                callback:GetFiles_Result
        );
        }
        private void GetFiles_Result(RequestResult result)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<RequestResult>(GetFiles_Result), result);
                return;
            }
            if (result.StatusCode == 200)
            {

            }
            else
            {
                MessageBox.Show("Oups... cant connect to server!");
            }
        }

    }
}
