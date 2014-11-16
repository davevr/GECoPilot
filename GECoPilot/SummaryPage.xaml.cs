using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading;


namespace GECoPilot
{	
	public partial class SummaryPage : ContentPage
	{	
       

		public SummaryPage ()
		{
			InitializeComponent ();

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

                    Xamarin.Forms.Device.StartTimer(new TimeSpan(0,0,1), () => 
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    RefreshSummary();

                                });
                            return true;
                        });

                };

           
		}

        public void UpdateLogin()
        {
            Login theLogin = new Login();
            theLogin.ReturnPage = this;
            Navigation.PushModalAsync(theLogin);
        }

        public void SetServerStatus()
        {
            SelectServerPage thePage = new SelectServerPage();
            thePage.ReturnPage = this;
            Navigation.PushModalAsync(thePage);
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

