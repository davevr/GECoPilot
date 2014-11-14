using System;
using Xamarin.Forms;

namespace GECoPilot
{
    public class App
    {

        public static Page GetMainPage()
        {   


            var nav = new NavigationPage(new SummaryPage());
            nav.Title = "GE Co-pilot";
               





            return nav;
        }
    }
}

