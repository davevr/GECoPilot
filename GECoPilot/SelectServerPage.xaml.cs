using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GECoPilot
{	
	public partial class SelectServerPage : ContentPage
	{	
        private ServerInfo[] savedServerList;
        public SummaryPage ReturnPage { get; set; }

		public SelectServerPage ()
		{
			InitializeComponent ();

       
            ServerListView.ItemTemplate = new DataTemplate(typeof(TextCell));
            ServerListView.ItemTemplate.SetBinding(TextCell.TextProperty, "name");
            ServerListView.ItemSelected += (sender, e) =>
            {
                    string curItem = "";
                    ServerInfo curInfo = (ServerInfo)e.SelectedItem;

                    if (curInfo == savedServerList[0]) curItem = "srv1";
                    else if (curInfo == savedServerList[1]) curItem = "srv2";
                    else curItem = "srv3";


                    GEServer.Instance.SetServer(curItem, (result) => 
                        {
                            AppSettings.Instance.Universe = curItem;
                            AppSettings.SaveSettings();
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    Navigation.PopModalAsync().ContinueWith((Page) =>
                                    {
                                        ReturnPage.UpdateBindings();
                                    });
                                   
                                });
                        });

            };

            Appearing += (object sender, EventArgs e) => 
                {
                    GEServer.Instance.GetServerList((serverList) => 
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                    savedServerList = new ServerInfo[] { serverList.srv1, serverList.srv2, serverList.srv3};
                                    ServerListView.ItemsSource = savedServerList;
                            });
                        });
                };
		}
	}
}

