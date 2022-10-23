using TestBDonline.Scripts.Structs;
using System.Collections.Generic;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TestBDonline.Scripts;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserEditingData : ContentPage
    {
        private UserData UserData { get; set; }

        public UserEditingData(UserData data)
        {
            InitializeComponent();
            UserData = data;
            BindingContext = this;

            picker.ItemsSource = new List<string>()
            {
                "Admin",
                "User",
                "Banned"
            };
            LoadUserDataUI();
        }

        private void Apply(object sender, EventArgs e)
        {
            var auth = new Authentication();

            if (int.TryParse(points.Text, out int pt))
            {
                UserData temp = new UserData
                {
                    Nickname = UserData.Nickname,
                    Email = email.Text,
                    Points = pt,
                    ID = UserData.ID,
                    Status = (Status)picker.SelectedIndex,
                    Gender = UserData.Gender,
                    RequirePasswordReset = toggle.IsChecked
                };


                if (!auth.UpdateUserData(temp))
                    DisplayAlert("Błąd!", "Nie można zmienić danych użytkownika", "Ok");
                Navigation.RemovePage(this);
            }
            else
                DisplayAlert("Błąd!", "Źle wprowadzone dane!", "Ok");
        }

        private void LoadUserDataUI()
        {
            id.Text = $"ID: {UserData.ID} ({UserData.Nickname})\nPłeć: {UserData.Gender}";
            email.Text = UserData.Email;
            points.Text = UserData.Points.ToString();
            picker.SelectedIndex = (int)UserData.Status;
            toggle.IsChecked = UserData.RequirePasswordReset;
        }
    }
}