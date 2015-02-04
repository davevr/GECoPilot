using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GECoPilot
{
    public partial class ScanGalaxyPage
    {
        private Task scanTask = null;
        private int curGalaxy, curSystem;
        private GalaxyList foundPlanetsList;
        private GalaxyList allPlanetsList;
        private CancellationToken token;
        private CancellationTokenSource tokenSource;
        private string scanName = "";
        private bool useRank = true;
        private int myRank = 0;
        private int rankRange = 15;
        private bool onlyLanxable = false;

        public ScanGalaxyPage()
        {
            InitializeComponent();

            ;

            ScanBtn.Clicked += (sender, eventArgs) =>
                {
                    tokenSource = new CancellationTokenSource();
                    token = tokenSource.Token;
                    scanTask = new Task(StartScan, token );
                    scanTask.Start();
                };

            CancelBtn.Clicked += (sender, eventArgs) =>
            {
                if (scanTask != null)
                {
                    tokenSource.Cancel();
                }
            };

            FilterBtn.Clicked += (sender, eventArgs) =>
                {
                    FilterBtn.IsEnabled = false;
                    scanName = UserNameField.Text;
                    useRank = UseRank.IsToggled;
                    rankRange = int.Parse(RankRangeField.Text);
                    onlyLanxable = Lanxable.IsToggled;
                    RecomputeFoundPlanets();

                };

            foundPlanetsList = new GalaxyList();
            allPlanetsList = new GalaxyList();
            allPlanetsList.CollectionChanged += allPlanetList_CollectionChanged;
            GalaxyListView.ItemsSource = foundPlanetsList;
        }

        void allPlanetList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           // RecomputeFoundPlanets();
        }

        private void RecomputeFoundPlanets()
        {
            GalaxyList matchingPlanets = new GalaxyList();

            foreach (GEGalaxyPlanet curPlanet in allPlanetsList)
            {
                int rank = int.MaxValue;
                int.TryParse(curPlanet.rank, out rank);

                if (!useRank || ((rank > myRank - rankRange) && (rank < myRank + rankRange) && (curPlanet.vacation == "0")))
                {
                    if (String.IsNullOrWhiteSpace(scanName) || (String.Compare(scanName, curPlanet.username, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        matchingPlanets.Add(curPlanet);
                    }
                }
            }

            Device.BeginInvokeOnMainThread(() =>
            {
               
                foundPlanetsList.Clear();
                foreach (GEGalaxyPlanet curPlanet in matchingPlanets)
                {
                    foundPlanetsList.Add(curPlanet);
                }
                FilterBtn.IsEnabled = true;
                StatusField.Text = "Showing " + foundPlanetsList.Count.ToString() + " of " + allPlanetsList.Count.ToString() + " targets";
                GalaxyListView.IsEnabled = true;
            });

        }

        private void StartScan()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ScanBtn.IsEnabled = false;
                CancelBtn.IsEnabled = true;
                allPlanetsList.Clear();
                GalaxyListView.IsEnabled = false;
            });
            curGalaxy = 1;
            curSystem = 1;

            ScanNextSystem();
        }

        private void ContinueScan()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ScanBtn.IsEnabled = false;
                CancelBtn.IsEnabled = true;

            });
  

            ScanNextSystem();
        }

        private void ScanNextSystem()
        {
             AsyncUpdateCount(curGalaxy, curSystem);
             GEServer.Instance.ScanGalaxy(curGalaxy, curSystem, (resultList) =>
                 {
                     if (resultList != null)
                     {
                         foreach (GEGalaxyPlanet curPlanet in resultList)
                         {
                             allPlanetsList.Add(curPlanet);
                         }
                     }

                     curSystem++;
                     if (curSystem > 500)
                     {
                         curSystem = 1;
                         curGalaxy++;
                     }
                     if (token.IsCancellationRequested || (curGalaxy > 5))
                     {
                         AsyncTaskComplete();
                     }
                     else
                         ScanNextSystem();
                 });

        }

        private void AsyncTaskComplete()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ScanBtn.IsEnabled = true;
                CancelBtn.IsEnabled = false;
                StatusField.Text = "Scanning found " + allPlanetsList.Count.ToString() + " targets";
                GalaxyListView.IsEnabled = false;

            });
            
        }

        private void AsyncUpdateCount(int gal, int sol)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                StatusField.Text = "Now scanning system " + sol.ToString() + " in galaxy " + gal.ToString();
                
                
            });
            
        }
    }
}
