using TestBDonline.Scripts.Structs;
using System.Collections.Generic;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            Navigation.RemovePage(this);
        }

        private void LoadUserDataUI()
        {
            id.Text = $"ID: {UserData.ID} ({UserData.Nickname})";
            email.Text = UserData.Email;
            points.Text = UserData.Points.ToString();
            picker.SelectedIndex = (int)UserData.Status;
            toggle.IsToggled = UserData.RequirePasswordReset;
        }
    }
}