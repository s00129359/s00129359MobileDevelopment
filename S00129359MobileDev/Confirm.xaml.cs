using Microsoft.WindowsAzure.MobileServices;
using S00129359MobileDev.Common;
using S00129359MobileDev.Data;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace S00129359MobileDev
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Confirm : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        //user
        private IMobileServiceTable<User> userTbl = App.MobileService.GetTable<User>();
        //route
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();
        //ticket
        private IMobileServiceTable<Ticket> ticketTbl = App.MobileService.GetTable<Ticket>();

        public int RouteId;
        public string TicketType;
        public string Depart;
        public string Arrive;
        public int Cost;

        //user logged in
        //when authentication added
        //this var will be = to whoever is logged in
        public int UserLoggedIn = 1;
        
        public Confirm()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            //while no azure
            //tBxArrive.Text = "Rosses Point";
            //tBxCost.Text = "4";
            //tBxDate.Text = "Wednesday";
            //tBxDeparts.Text = "IT Sligo";
            //tBxEnoughCredits.Text = "You have enough credits";
            //tBxReturn.Text = "Yes";
            //credsRemaining.Text += " 90";

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
                RouteId = prm.routeId;
                TicketType = prm.ticketType;

            }

            GetUsersCredits();
            RouteDetails();
            FillPage();


        }


        private async void GetUsersCredits()
        {
            List<User> itm = await userTbl
                .Where(id => id.UserId == UserLoggedIn)
                .ToListAsync();

            foreach (var item in itm)
            {
                credsRemaining.Text += item.Credits;
            }
        }

        private async void RouteDetails()
        {
            List<Route> itm = await routeTbl
                .Where(id => id.Route_id == RouteId)
                .ToListAsync();

            foreach (var item in itm)
            {
                Depart = item.Departs;
                Arrive = item.Arrives;
                Cost = item.Cost;
            }
            tBxDeparts.Text = Depart;
            tBxCost.Text = Cost.ToString();
            tBxArrive.Text = Arrive;

        }

        private void FillPage()
        {
           // credsRemaining += 
            if (TicketType == "Return")
            {
                tBxReturn.Text = "Yes";              
            }
            else if (TicketType == "Single")
            {
                tBxReturn.Text = "No";              
            }

            tBxDate.Text = DateTime.Now.Date.ToString("d");
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            //UpdateUser();

            AddTicket();

            //on ticket page
            Frame.Navigate(typeof(ViewTicket));
        }

        private async void AddTicket()
        {
            int highestId = 0;
            List<Ticket> route = await ticketTbl
                .OrderBy(r => r.ticketid)
                .ToListAsync();
            foreach (var rte in route)
            {
                highestId = rte.ticketid;
            }

            int nextId = 1 + highestId;
            tBxArrive.Text = nextId.ToString();
            // var ds = new Ticket() { CustId = UserLoggedIn, RouteId = RouteId, PurchaseDate = DateTime.Now.Date, TicketType = TicketType };

            // await ticketTbl.InsertAsync(ds);

            // var ds1 = new Ticket() { CustId = UserLoggedIn, RouteId = RouteId, PurchaseDate = DateTime.Now.Date, TicketType = TicketType };

            //  await ticketTbl.InsertAsync(ds1);

           // Frame.Navigate(typeof(ViewTicket), nextId);

        }

        private async void UpdateUser()
        {
            //calculates new credit amount
            var userCreds = await userTbl
                .Where(user => user.UserId == UserLoggedIn)
                .ToCollectionAsync();

            var use = userCreds.FirstOrDefault();

            if (use != null)
            {
                use.Credits -= Cost;
                await userTbl.UpdateAsync(use);

            }
        }
    }
}
