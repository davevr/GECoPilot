using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading;


namespace GECoPilot
{	
	public partial class SummaryPage : ContentPage
	{
        private bool _timerRunning = false;

		public SummaryPage ()
		{
			InitializeComponent ();

            AppSettings.LoadSettings();

            PlanetListView.ItemTemplate = new DataTemplate(typeof(TextCell));
            PlanetListView.ItemTemplate.SetBinding(TextCell.TextProperty, "name");

            RefreshBtn.Clicked += (object sender, EventArgs e) =>
                {
                    GEServer.Instance.Refresh((theStr) => 
                        {
                            UpdateBindings();
                        });
                };

            Appearing += (object sender, EventArgs e) => 
                {
                    if (!GEServer.Instance.IsSignedIn)
                    {
                        UpdateLogin();
                    }

                    if (!_timerRunning)
                    {
                        _timerRunning = true;
                        Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                RefreshSummary();

                            });
                            return true;
                        });
                    }
                    

                };

           
		}

        public void UpdateLogin()
        {
            if (String.IsNullOrEmpty(AppSettings.Instance.Username))
            {
                Login theLogin = new Login();
                theLogin.ReturnPage = this;
                Navigation.PushModalAsync(theLogin);
            }
            else
            {
                // attempt login
                GEServer.Instance.Login(AppSettings.Instance.Username, AppSettings.Instance.Password, (result) =>
                {
                    if (result == "")
                    {
                        GEServer.Instance.SetServer(AppSettings.Instance.Universe, (returnVal) =>
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                UpdateBindings();
                            });
                        });
                    }
                    else
                    {
                        DisplayAlert("Error", "Your credentials didn't work.  Please try again.", "ok");
                        AppSettings.Instance.Clear();
                        UpdateLogin();
                    }
                });
            }

        }

        public void SetServerStatus()
        {
            SelectServerPage thePage = new SelectServerPage();
            thePage.ReturnPage = this;
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushModalAsync(thePage);
                });
        }

        public void UpdateBindings()
        {
            PlanetListView.ItemsSource = GEServer.Instance.ServerState.planetSummaryList;
            SummaryArea.Text = GEServer.Instance.ServerState.SummaryText;
        }

        public void RefreshSummary()
        {
            if (GEServer.Instance.ServerState != null)
                SummaryArea.Text = GEServer.Instance.ServerState.SummaryText;
        }
	}
}

