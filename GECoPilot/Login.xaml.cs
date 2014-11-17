using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GECoPilot
{	
	public partial class Login : ContentPage
	{	
        public SummaryPage ReturnPage { get; set; }

		public Login ()
		{
			InitializeComponent ();

            DoLogin.Clicked += (object sender, EventArgs e) =>
            {
               
                GEServer.Instance.Login(AddrField.Text, PasswordField.Text, (result) =>
                    {
                        if (result == "")
                        {
                            AppSettings.Instance.Username = AddrField.Text;
                            AppSettings.Instance.Password = PasswordField.Text;
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    Navigation.PopModalAsync();
                                    ReturnPage.SetServerStatus();
                                });
                        }
                        else
                            DisplayAlert("Error", "Your credentials didn't work.  Please try again.", "ok");
                    });
            };
                     
		}
	}
}

