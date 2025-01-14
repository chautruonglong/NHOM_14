﻿using AutoAttendant.Models;
using AutoAttendant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using System.Security.Cryptography;
using System.IO;
using Xamarin.Essentials;
using System.Net;
using AutoAttendant.ViewModel;
using System.Threading;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static ListRoomViewModel _lrvm = new ListRoomViewModel();
        public static ListScheduleViewModel _lsvm = new ListScheduleViewModel();
        public static int checkCreateListSchedule = 0; //avoid repeat schedule from ShowSchedule()
        //public static Lecture _lecture = new Lecture();
        public LoginPage()
        {
            InitializeComponent();
            GetSavedAccount();
        }
        #region Save Lastest Login
        public void SaveAccountLogined()
        {
            Preferences.Set("email", Entry_user.Text);
            Preferences.Set("password", Entry_password.Text);
        }

        public void GetSavedAccount()
        {
            Entry_user.Text = Preferences.Get("email", string.Empty);
            Entry_password.Text = Preferences.Get("password", string.Empty);
        }
        #endregion

        [Obsolete]
        
        public void paintLoading()
        {
            UserDialogs.Instance.ShowLoading("Please wait...");
        }

        [Obsolete]
        private async void LoginProcedure(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...");
                HomePage.base_URL = "http://" + Entry_Api.Text;
                //UserTemp userTemp = new UserTemp(Entry_user.Text, Entry_password.Text);
                UserTemp userTemp = new UserTemp(Entry_user.Text, HashPW.HashPassword(Entry_password.Text));
                var httpService = new HttpClient();
                httpService.Timeout = TimeSpan.FromSeconds(5);

                string jsonData = JsonConvert.SerializeObject(userTemp); // dung` UserTemp để post login vì chỉ cần email vs password
                //var base_URL = HomePage.base_URL + "/login";
                var base_URL = HomePage.base_URL + "/account/login/";

                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
         
                HttpResponseMessage response = await httpService.PostAsync(base_URL, content);
                var result = await response.Content.ReadAsStringAsync();

                //UserDialogs.Instance.HideLoading();
                //Data.Data.Instance.User = JsonConvert.DeserializeObject<User>(result); // dùng User để nhận json về vì có chứa thêm token, idLecture (static)
                Data.Data.Instance.UserNui = JsonConvert.DeserializeObject<UserNui>(result);
                UserNui userNui = JsonConvert.DeserializeObject<UserNui>(result);
                
                HomePage.lecturer_id = userNui.lecturer_id;
                HomePage.api_key = userNui.authorization;
                Data.Data.Instance.User = new User(Convert.ToInt32(userNui.lecturer_id), Entry_user.Text, Entry_password.Text);


                if (response.IsSuccessStatusCode)
                {
                    //UserDialogs.Instance.ShowLoading("Please wait...");
                    //await Task.Delay(600);
                    SaveAccountLogined(); // save user and password for next time
                    await Navigation.PushAsync(new HomePage());
                }
                else {
                    
                    //UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Login Fail", "Try Again");
                } 

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Login Fail", "Try Again");
                //await DisplayAlert("ERROR", ex.Message, "Try Again");
            }
            UserDialogs.Instance.HideLoading();
        }

        private void ForgotPassword(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage());
        }

        private void SignUp(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SignUpPage());
        }

        private void OpenApiEntry(object sender, EventArgs e) 
        {
            Entry_Api.IsVisible = true;
            HomePage.base_URL = "http://" + Entry_Api.Text;
        }

    }
}