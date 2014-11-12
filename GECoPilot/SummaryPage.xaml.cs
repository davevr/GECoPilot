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
                        Navigation.PushModalAsync(new Login());
                    }
                    else if (GEServer.Instance.ServerState == null)
                    {
                        Navigation.PushModalAsync(new SelectServerPage());
                    }
                    else
                    {
                        PlanetListView.ItemsSource = GEServer.Instance.ServerState.planetSummaryList;
                    }
                };
		}
	}
}

