using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TestBDonline.Scripts;
using System;
using System.Collections.Generic;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Account : ContentPage
    {
        Authentication Data { get; set; }
        public Account(Authentication data)
        {
            InitializeComponent();
            Data = data;
            nick.Text = $"{data.UserData.Nickname}";
            pts.Text = $"Twoje punkty\n{data.UserData.Points}";

            picker.ItemsSource = new List<string>()
            {
                "Unidentified",
                "Woman",
                "Man"
            };
            picker.SelectedIndex = (int)Data.UserData.Gender;
            login.Text = Data.UserData.Email;
        }

        private void SaveData(object sender, EventArgs e)
        {
            var user = Data.UserData;
            user.Gender = (Scripts.Structs.Gender)picker.SelectedIndex;
            user.Email = login.Text;

            if (Data.UpdateUserData(user))
            {
                DisplayAlert("Informacja", "Pomyślnie zapisano", "Ok");
                Data.CreateNewLog(new Scripts.Structs.EventData
                {
                    Autor = Data.UserData.Nickname,
                    Date = DateTime.Now,
                    Details = "Zmieniono własne dane personalne"
                });
            }
            else
                DisplayAlert("Błąd", "Błąd zapisu danych", "Ok");
        }

        private void Logout(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(this);
        }

        private void ChangePassword(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ResetPasswordPage(Data));
            Navigation.RemovePage(this);
        }
    }
}