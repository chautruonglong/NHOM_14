﻿using AutoAttendant.Models;
using AutoAttendant.ViewModel;
using Newtonsoft.Json;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllSubjectsPage : ContentPage
    {
        public static int checkCreateAllSubject = 0;
        [Obsolete]
        public AllSubjectsPage()
        {
            InitializeComponent();
            ShowAllSubject();
        }

        [Obsolete]
        protected override void OnAppearing() // goị trước khi screen page này xuất hiện
        {
            ReLoadSubjectList();
            base.OnAppearing();
        }

        [Obsolete]
        public void ReLoadSubjectList()
        {
            HomePage._lsjavm.SubjectAllCollection.Clear();
            ShowAllSubject();
            this.BindingContext = new ListSubjectAllViewModel();
            this.BindingContext = HomePage._lsjavm;
        }

        [Obsolete]
        public async Task<ObservableCollection<Subject>> HandleAllSubject() //get all subject by lecturer_id
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/subject/list/" + Data.Data.Instance.UserNui.lecturer_id.ToString() + "/all/";
                var result = await httpService.GetAsync(base_URL);
                var jsonSubject = await result.Content.ReadAsStringAsync();
                var listSubject = JsonConvert.DeserializeObject<ObservableCollection<Subject>>(jsonSubject);

                // order list subject by time slot
                var dayIndex = new List<string> { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
                listSubject = new ObservableCollection<Subject>(listSubject.OrderBy(r => dayIndex.IndexOf(r.day.ToUpper())));
                return listSubject;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "OK");
                return null;
            }
        }

        [Obsolete]
        public async void ShowAllSubject()
        {
            try
            {
                var listSubject = new ObservableCollection<Subject>(await HandleAllSubject()); // list Subject trả về từ HandelSubject
                if (listSubject.Count > 0)
                {
                    foreach (Subject subject in listSubject)  // duyet trong list subject để thêm vào lsjavm
                    {
                        HomePage._lsjavm.SubjectAllCollection.Add(subject);
                    }

                    this.BindingContext = new ListSubjectAllViewModel();
                    this.BindingContext = HomePage._lsjavm;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", "Fall in ShowAllSubject /n" +ex.Message, "OK");
            }
        }

        [Obsolete]
        private void SubjectClick(object sender, EventArgs e)
        {
            try
            {
                string message = string.Empty;
                Frame f = sender as Frame;
                var fContent = f.Content; // Lấy Content của Frame
                var myStacklayout = fContent.GetType(); // lấy kiểu của Content
                if (myStacklayout == typeof(StackLayout)) // check kiểu có phải Stack Layout ko
                {
                    StackLayout fStacklayout = (StackLayout)fContent;
                    var listChildren = fStacklayout.Children; // Lấy tập Children của StackLayout
                    var firstLabel = listChildren[0];

                    if (firstLabel.GetType() == typeof(Label))
                    {
                        Label subjectId = (Label)firstLabel;
                        string subject_id = subjectId.Text;


                        Navigation.PushAsync(new ListStudentForViewAllSubjectPage(subject_id));
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "OK");
            }
        }
    }
}