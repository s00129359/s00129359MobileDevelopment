using S00129359MobileDev.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using S00129359MobileDev.Data;
using Microsoft.WindowsAzure.MobileServices;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace S00129359MobileDev
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Journeys : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        //Declare  passed vars
        public int routeId;
        public string day = "Monday";
        //could be empty if no reuturn
        public string returnDay = "Sunday";

        //single ticket by default
        public string ticketType = "Single";
        
        private IMobileServiceTable<Journey> journeyTbl = App.MobileService.GetTable<Journey>();
        //route
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();

        public Journeys()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            ////while no azure 
            //tblJourney.Text = "IT Sligo to Rosses Point";
            //tblJourneyDay.Text = "On Wednesday";

            //tblJourney1.Text = "IT Sligo to Rosses Point";
            //tblJourneyDay1.Text = "On Thursday";
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

         public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Params prm = e.NavigationParameter as Params;
            if (prm != null)
            {
                routeId = prm.routeId;
                day = prm.date;
                returnDay = prm.returnDate;
                ticketType = prm.ticketType;

            }

            PopulateJourney();
            PopulateReturnJourney();

        }

        private async void PopulateJourney()
        {
            List<Route> itm = await routeTbl
                 .Where(id => id.Route_id == routeId)
                 .ToListAsync();

            string depart = "", arrive = "";

            if (0 == 0)
            {
                foreach (var route in itm)
                {
                    depart = route.Departs;
                    arrive = route.Arrives;
                }
            }

            tblJourney.Text = depart + " TO " + arrive;

            //first journey
            tblJourneyDay.Text = "On a " + day;

            List<Journey> journ = await journeyTbl
                .Where(r => r.Route_id == routeId)
                .ToListAsync();

            string depTime, arrTime;
            int journeyId;
                foreach (var jour in journ)
                {
                    journeyId = jour.Journey_id;
                    arrTime = jour.ArrivalTime;
                    depTime = jour.DepartureTime;
                    string list = "Departs : " + depTime + ", Arrives : " + arrTime;

                   lstDepart.Items.Add(list);
                   }
            
        }

        private async void PopulateReturnJourney()
        {
            if (ticketType == "Single")
            {
                tblJourney1.Text = "No Return";
                lstDepart.ItemsSource = ("No Return Journey");
                lstDepart.IsEnabled = false;
            }
            else
            {
                List<Route> itm = await routeTbl
                     .Where(id => id.Route_id == routeId)
                     .ToListAsync();

                string depart = "", arrive = "";

                if (0 == 0)
                {
                    foreach (var route in itm)
                    {
                        depart = route.Departs;
                        arrive = route.Arrives;
                    }
                }

                //change the depart and return 
                //for return journey
                tblJourney1.Text = arrive + " TO " + depart;

                //return journey
                tblJourneyDay1.Text = "On a " + returnDay;

                List<Journey> journ = await journeyTbl
                    .Where(r => r.Route_id == routeId)
                    .ToListAsync();

                string depTime, arrTime;

                if (true)
                {
                    foreach (var jour in journ)
                    {
                        depTime = jour.ArrivalTime;
                        arrTime = jour.DepartureTime;
                        string list = "Departs : " + depTime + ", Arrives : " + arrTime;

                        lstReturn.Items.Add(list);
                    }
                }
            }

        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

            Params prm = new Params { routeId = routeId, date = day, returnDate = returnDay, ticketType = ticketType};

            Frame.Navigate(typeof(Confirm), prm);
        }
    }
}
