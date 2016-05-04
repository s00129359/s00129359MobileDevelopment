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
    public sealed partial class Profile : Page
    {
        //user logged in
        public int UserLoggedIn = 1;
        //user
        private IMobileServiceTable<User> userTbl = App.MobileService.GetTable<User>();
        //ticket
        private IMobileServiceTable<Ticket> ticketTbl = App.MobileService.GetTable<Ticket>();        
        //route
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();

        public Profile()
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
            tbxemail.Text = "Cormac@Hotmail.com";
            tbxName.Text = "Cormac";
            tbxSurName.Text = "Hallinan";
            lstRoutes.Items.Add("1. IT Sligo to Grange");
            lstRoutes.Items.Add("1. IT Sligo to Bus Station");
            // getUserDetails();
            // loadTickets();
        }


        private async void getUserDetails()
        {
            List<User> user = await userTbl
                    .Where(id => id.UserId == UserLoggedIn)
                    .ToListAsync();
            foreach (var usr in user)
            {
                tbxemail.Text = usr.Email;
                tbxName.Text = usr.FirstName;
                tbxSurName.Text = usr.SeondName;
            }
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            List<User> user = await userTbl
                        .Where(id => id.UserId == UserLoggedIn)
                        .ToListAsync();
            var use = user.FirstOrDefault();
            if (use != null)
            {
                use.Email = tbxemail.Text;
                use.FirstName = tbxName.Text;
                use.SeondName = tbxSurName.Text;
                await userTbl.UpdateAsync(use);

            }
        }

        private async void loadTickets()
        {
       
            List<Ticket> tickets = await ticketTbl
                .Where(cid => cid.CustId == UserLoggedIn)
                .ToListAsync();

            foreach (var ticket in tickets)
            {
                List<Route> routes = await routeTbl
                    .Where(rid => rid.Route_id == ticket.RouteId)
                    .ToListAsync();
                foreach (var route in routes)
                {
                    string j = route.Departs + " " + route.Arrives;
                    lstRoutes.Items.Add(j);
                }

            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ViewTicket));
        }

    }
}
