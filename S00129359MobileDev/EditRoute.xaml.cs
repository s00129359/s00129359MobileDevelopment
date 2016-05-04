﻿using Microsoft.WindowsAzure.MobileServices;
using S00129359MobileDev.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace S00129359MobileDev
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditRoute : Page
    {
        public int routeId;
        private IMobileServiceTable<Journey> journeyTbl = App.MobileService.GetTable<Journey>();
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();

        public EditRoute()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string sRouteId = e.Parameter as string;
            routeId = Convert.ToInt32(sRouteId);

           // Route();
           // getJourneys();
        }

        private async void Route()
        {
            List<Route> routes = await routeTbl
                .Where(r => r.Route_id == routeId)
                .ToListAsync();
            foreach (var route in routes)
            {
                tbRoute.Text = route.Departs = " To " + route.Arrives;
            }
        }

        private async void getJourneys()
        {
            List<Journey> journeys = await journeyTbl
                .Where(j => j.Route_id == routeId)
                .ToListAsync();

            foreach (var journey in journeys)
            {
                string data = journey.Journey_id + ". "+ "Departs "+ journey.DepartureTime + " Arrives " + journey.ArrivalTime;
                lstJourneys.Items.Add(data);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddJourney));
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Admin));
        }

        private void lstJourneys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = true;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            string reportSelcted = lstJourneys.SelectedItem.ToString();
            int indx = reportSelcted.LastIndexOf(".");
            string journeyId = reportSelcted.Substring(0, indx);

            Frame.Navigate(typeof(editJourney), journeyId);
        }

        private async void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            //delte the route you have clicked int0
            Route deleteRoute = await routeTbl
                .Where(r => r.Route_id == routeId)
                .ToListAsync();

            await routeTbl.DeleteAsync(deleteRoute);

            Frame.Navigate(typeof(Admin));
        }
    }
}
