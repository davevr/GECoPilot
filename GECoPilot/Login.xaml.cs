using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GECoPilot
{	
	public partial class Login : ContentPage
	{	
		public Login ()
		{
			InitializeComponent ();

            DoLogin.Clicked += (object sender, EventArgs e) =>
            {
               
                GEServer.Instance.Login(AddrField.Text, PasswordField.Text, (result) =>
                    {
                        if (result == "")
                                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Navigation.PopModalAsync();
                                    });
                        else
                            DisplayAlert("Error", "Your credentials didn't work.  Please try again.", "ok");
                    });
            };
                     
		}
	}
}

