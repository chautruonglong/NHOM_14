﻿using Acr.UserDialogs;
using AutoAttendant.Models;
using AutoAttendant.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomPage : ContentPage
    {
        ListRoomViewModel lrvm = new ListRoomViewModel();
        public RoomPage()
        {
            InitializeComponent();
            HandleDatePicker();
            this.BindingContext = new ListRoomViewModel();
        }


        private void RoomClick(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ClassPage());
        }

        private void HandleDatePicker()
        {
            //string message = string.Empty;
            var datePicker = new DatePicker
            {
                ClassId = "MyDatePicker",
                Date = DateTime.Now,
                IsVisible = false,
                IsEnabled = false
                
            };
            DatePickerLayout.Children.Add(datePicker);

            btnDatePicker.Clicked += (object sender, EventArgs e) =>
            {
                
                IsEnabled = true;
                datePicker.Focus();
            };
            
            datePicker.DateSelected += (object sender1, DateChangedEventArgs e) =>
            {
                string day = datePicker.Date.DayOfWeek.ToString();
                string date = datePicker.Date.ToString().Substring(0, 9);

                lb_date.Text = "Rooms on " + day + " " + date;
            };
        }

        [Obsolete]
        private async void AddRoom(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();

        }


        [Obsolete]
        private async void ShowPopUpAddRoom(object sender, EventArgs e) // Show Popup and handle data from popup
        {
            string roomName = String.Empty;
            string message = String.Empty;
            var page = new PopUpView();
            page.Action += async (sender1, stringparameter) =>
            {
                
                if(stringparameter != null)
                {
                    roomName = stringparameter; // get data tu` PopUp
                    roomName = roomName.Replace(" ", "");
                    roomName = roomName.ToUpper();
                    //message = roomName + " was added successfully";

                    Room room = new Room("1", roomName);
                    lrvm.RoomCollection.Add(room);
                    this.BindingContext = lrvm;

                    UserDialogs.Instance.Toast("Room " + roomName + " was added");

                }
                else
                {
                    await DisplayAlert("Notice", "Invalid Syntax" , "OK");
                    return;
                }
            };

            page.Disappearing += (c, d) =>
            {
                if (roomName != null)
                {
                    

                }
                
            };

            await PopupNavigation.Instance.PushAsync(page);
            //PopupNavigation.PushAsync(new PopUpView());
        }

        private void DeleteRoom(object sender, EventArgs e)
        {
            Image img = sender as Image;
            var stackLayout = img.Parent;
            var checkStack = stackLayout.GetType();
            if (checkStack == typeof(StackLayout))
            {
                StackLayout container = (StackLayout)stackLayout;
                var listChild = container.Children;

                var lb_room = listChild[0].GetType();
                if (lb_room == typeof(Label))
                {
                    Label lb = (Label)listChild[0];

                    var itemToRemove = lrvm.RoomCollection.Single(r => r.Name == lb.Text);
                    lrvm.RoomCollection.Remove(itemToRemove);
                }
                else
                {
                    DisplayAlert("Fail", "Fail", "Try Again");
                }
                
            }
        }
    }
}