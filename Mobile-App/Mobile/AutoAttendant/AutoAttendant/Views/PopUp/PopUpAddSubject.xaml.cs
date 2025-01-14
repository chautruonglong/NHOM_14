﻿using AutoAttendant.Models;
using AutoAttendant.Services;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views.PopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpAddSubject
    {
        [Obsolete]
        public PopUpAddSubject()
        {
            InitializeComponent();
            HandleDatePicker();
            GetRoomForAddSubject();
        }

        public EventHandler<string> Action;

        private void HandleDatePicker()
        {
            lb_date.Text = DateTime.Now.DayOfWeek.ToString();
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
                lb_date.Text = day;
            };
        }

        [Obsolete]
        private async void HandlePickerRoom(object sender, EventArgs e)
        {
            try
            {
                var index = PickerRoom.SelectedIndex;
                if (index != -1)
                {
                    btnSelectRoom.Text = PickerRoom.Items[index].ToString();
                    var room = HomePage._lrvm.RoomCollection.Single(r => r.room_id == btnSelectRoom.Text);
                    var day = lb_date.Text;

                    var httpService = new HttpClient();
                    var api_key = Data.Data.Instance.UserNui.authorization;
                    httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                    var base_URL = HomePage.base_URL + "/subject/time_slot/list/" + room.room_id + "/" + day + "/";
                    var result = await httpService.GetAsync(base_URL);
                    var contentListTimeSlot = await result.Content.ReadAsStringAsync();
                    var listTimslottInRoom = JsonConvert.DeserializeObject<List<String>>(contentListTimeSlot);

                    GetTimeSlot(listTimslottInRoom);
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "OK");
            }
        }

        public void GetTimeSlot(List<String> listTimeSlot)
        {
            usedTimeSlotLabel.Text = "|";
            //List<TimeSpan> listTimeSingle = new List<TimeSpan>();
            //foreach (string timeSlot in listTimeSlot)
            //{ 
            //    TimeSpan a=Convert.ToDateTime(timeSlot.Substring(0,5)).TimeOfDay;
            //    TimeSpan b = Convert.ToDateTime(timeSlot.Substring(5, 5)).TimeOfDay;
            //    listTimeSingle.Add(a);
            //    listTimeSingle.Add(b);
            //}

            //var ArrayTimeSingle = listTimeSingle.ToArray();
            //for (int i = 0; i < listTimeSlot.Count() - 1; i++)
            //{
            //    TimeSpan x = ArrayTimeSingle[2 * (i + 1)] - ArrayTimeSingle[2 * i + 1];
                
            //}
            foreach (string timeSlot  in listTimeSlot)
            {
                usedTimeSlotLabel.Text = usedTimeSlotLabel.Text + "   " + timeSlot + "   " + "|";
            }
        }

        private void OpenPicker(object sender, EventArgs e)
        {
            PickerRoom.IsEnabled = true;
            PickerRoom.Focus();
        }

        [Obsolete]
        public async void GetRoomForAddSubject()
        {
            try
            {
                var listRoom = HomePage._lrvm.RoomCollection;
                foreach (Room room in listRoom)
                {
                    PickerRoom.Items.Add(room.room_id);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

        [Obsolete]
        private async void SaveNewSubject(object sender, EventArgs e)
        {
            try
            {

       
                ListStd.room_id =btnSelectRoom.Text;
                ListStd.lecturer_id = Data.Data.Instance.Lecture.id;
                //
                TimeSpan timeBegin = Convert.ToDateTime(btnSelectTime.Text).TimeOfDay;
                var x = Convert.ToInt32(Entry_period.Text) - 1;
                TimeSpan period;
                if (Convert.ToDateTime(x+":00").TimeOfDay+ timeBegin > TimeSpan.Parse("12:00"))
                {
                    period = Convert.ToDateTime(x + ":30").TimeOfDay;
                } 
                else {  period = Convert.ToDateTime(x + ":50").TimeOfDay; }
                TimeSpan timeEnd = Convert.ToDateTime(btnSelectTime.Text).TimeOfDay+ period;
                ListStd.time_slot =timeBegin.ToString(@"hh\:mm") + "-"+ timeEnd.ToString(@"hh\:mm");
                //
                ListStd.day =lb_date.Text;
                if (ListStd.day.Equals(DateTime.Now.DayOfWeek.ToString()))
                {
                    if(SubjectPage.enableSubJectId == "-2")
                    {
                        var subject = new Subject(ListStd.subject_id, ListStd.lecturer_id, ListStd.room_id, ListStd.name, ListStd.time_slot, ListStd.day);
                        SubjectPage.enableSubJectId = subject.subject_id;
                        HomePage._lsjvm.SubjectCollection.Add(subject);
                    }
                    else if (SubjectPage.enableSubJectId == "-1")
                    {
                        var subject = new Subject(ListStd.subject_id, ListStd.lecturer_id, ListStd.room_id, ListStd.name, ListStd.time_slot, ListStd.day);
                        SubjectPage.enableSubJectId = subject.subject_id;
                        HomePage._lsjvm.SubjectCollection.Add(subject);
                    }
                    else
                    {
                        var subject = new Subject(ListStd.subject_id, ListStd.lecturer_id, ListStd.room_id, ListStd.name, ListStd.time_slot, ListStd.day);
                        HomePage._lsjvm.SubjectCollection.Add(subject);
                    }
                }
                var listtemp= HomePage._lsjvm.SubjectCollection.OrderBy(r => TimeSpan.Parse(r.time_slot.Substring(0, 5)));
                HomePage._lsjvm.SubjectCollection = new ObservableCollection<Subject>(listtemp);
                SendListStdToServer(ListStd);

                //Back to Subject Page after Post to server
            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
            }
            
        }

        [Obsolete]
        public async void SendListStdToServer(ListStd listStd)
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                string jsonListId = JsonConvert.SerializeObject(listStd);
                StringContent contentStdList = new StringContent(jsonListId, Encoding.UTF8, "application/json");
                var baseStdList_URL = HomePage.base_URL +"/subject/create/";
                HttpResponseMessage response = await httpService.PostAsync(baseStdList_URL, contentStdList);
                var message = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Notice", message, "OK");

                if (response.IsSuccessStatusCode)
                {
                    Action?.Invoke(this, "Add succesfully");
                    await PopupNavigation.Instance.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error ", "Fail in SendListoServer /n" + ex.Message, "OK");
            }
        }
        public  static ListStd ListStd = new ListStd();
        private async void ImportExcel(object sender, EventArgs e)
        {
            try
            {
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] {"com.microsoft.xlsx"} },
                    { DevicePlatform.Android, new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
                      //DevicePlatform.Android, new[] { "application/vnd.ms-excel" }

                    },
                });
                var pickerResult = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customFileType,
                    PickerTitle = "Pick an Excel file"
                });

                if (pickerResult != null)
                {
                    ExcelEngine excelEngine = new ExcelEngine();
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2016;
                    var fileStream = await pickerResult.OpenReadAsync();

                    var resourcePath = pickerResult.FullPath.ToString(); // lay ra path file
                    lb_ExcelFile.Text = resourcePath;

                    //Open the workbook
                    IWorkbook workbook = application.Workbooks.Open(fileStream);

                    //Access first worksheet from the workbook.
                    IWorksheet worksheet = workbook.Worksheets[0];
                    var numberOfStudent = (worksheet.Rows.Count() - 3);
                    var inforSubject = worksheet.Range["A5"].Text.Trim();
                    var lastindexOfNameSub = inforSubject.IndexOf("(");
                    var name_sub = inforSubject.Substring(5, lastindexOfNameSub - 5 - 1);
                    var id_sub = inforSubject.Substring(lastindexOfNameSub + 1, inforSubject.Count() - 2 - lastindexOfNameSub).Trim();
                    var x = worksheet.Range["B8"].Text.ToString();
                    List<StudentNui> StdNui_list = new List<StudentNui>();
                    for (int i = 8; i <= numberOfStudent; i++)
                    {
                        string id = "B" + i.ToString();
                        string name = "C" + i.ToString();
                        string class_name = "D" + i.ToString();
                        string phone = "E" + i.ToString();
                        string birth = "F" + i.ToString();
                        var std_id = worksheet.Range[id].Text.ToString().Trim();
                        var std_name = worksheet.Range[name].Text.ToString().Trim();
                        var std_class_name = worksheet.Range[class_name].Text.ToString().Trim();
                        var std_phone = worksheet.Range[phone].Text.ToString().Trim();
                        var std_birth = worksheet.Range[birth].DateTime;
                        var birthday=String.Format("{0:dd-MM-yyyy}", std_birth);

                        try
                        {
                            StudentNui std_nui = new StudentNui(std_id, std_name, std_phone, std_class_name, birthday, "");
                            StdNui_list.Add(std_nui);

                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Notice", ex.Message, "OK");
                        }
                    }
                    ListStd.name = name_sub;
                    ListStd.subject_id = id_sub;
                    ListStd.students = StdNui_list;
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

        public void ValidatePopUpAdd(string name, string roomName, string time_slot, string used_timeSlot, string day)
        {

        }

        [Obsolete]
        private async void CancelAddSubjectPopup(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }

        private void OpenPickerTime(object sender, EventArgs e)
        {
            PickerTime.IsEnabled = true;
            PickerTime.Focus();
        }

        private async void HandlePickerTime(object sender, EventArgs e)
        {
            try
            {
                var index = PickerTime.SelectedIndex;
                if (index != -1)
                {
                    btnSelectTime.Text = PickerTime.Items[index].ToString();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "OK");
            }
        }
    }
}