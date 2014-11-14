using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GECoPilot
{	
	public partial class SummaryPage : ContentPage
	{	
       

		public SummaryPage ()
		{
			InitializeComponent ();

            PlanetListView.ItemTemplate = new DataTemplate(typeof(TextCell));
            PlanetListView.ItemTemplate.SetBinding(TextCell.TextProperty, "name");


            Appearing += (object sender, EventArgs e) => 
                {
                    if (!GEServer.Instance.IsSignedIn)
                    {
                        UpdateLogin();
                    }

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
	}
}

