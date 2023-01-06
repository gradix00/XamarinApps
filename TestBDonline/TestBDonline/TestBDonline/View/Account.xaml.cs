using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TestBDonline.Scripts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        private async void SaveData(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            bar.Height = 15;
            var user = Data.UserData;
            user.Gender = (Scripts.Structs.Gender)picker.SelectedIndex;
            user.Email = login.Text;
            user.IsActive = true;

            if (await Task.Run(()=> Data.UpdateUserData(user)))
            {
                await DisplayAlert("Informacja", "Pomyślnie zapisano", "Ok");
                Data.CreateNewLog(new Scripts.Structs.EventData
                {
                    Autor = Data.UserData.Nickname,
                    Date = DateTime.Now,
                    Details = "Zmieniono własne dane personalne"
                });
            }
            else
                await DisplayAlert("Błąd", "Błąd zapisu danych", "Ok");
            indicator.IsVisible = false;
            bar.Height = 1;
        }

        private async void Logout(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());

            var user = Data.UserData;
            user.IsActive = false;
            await Task.Run(() => Data.UpdateUserData(user));
            Navigation.RemovePage(Navigation.NavigationStack[0]);
        }

        private void ChangePassword(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ResetPasswordPage(Data));
            Navigation.RemovePage(this);
        }
    }
}