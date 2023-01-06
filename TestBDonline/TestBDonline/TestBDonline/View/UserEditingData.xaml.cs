﻿using TestBDonline.Scripts.Structs;
using System.Collections.Generic;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TestBDonline.Scripts;
using System.Threading.Tasks;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserEditingData : ContentPage
    {
        private UserData UserData { get; set; }
        private Authentication Data { get; set; }

        public UserEditingData(Authentication data, UserData userEdited)
        {
            InitializeComponent();
            UserData = userEdited;
            Data = data;
            BindingContext = this;

            picker.ItemsSource = new List<string>()
            {
                "Admin",
                "User",
                "Banned"
            };
            LoadUserDataUI();
        }

        private async void Apply(object sender, EventArgs e)
        {
            await Data.GetUserDataByID(Data.UserData.ID);
            if (Data.UserData.Status == Status.admin)
            {
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


                    if (!await Task.Run(()=> Data.UpdateUserData(temp)))
                        await DisplayAlert("Błąd!", "Nie można zmienić danych użytkownika", "Ok");
                    else
                        Data.CreateNewLog(new EventData
                        {
                            Autor = Data.UserData.Nickname,
                            Date = DateTime.Now,
                            Details = $"Zmieniono dane użytkownika {UserData.Nickname}"                         
                        });
                    Navigation.RemovePage(this);
                }
                else
                    await DisplayAlert("Błąd!", "Źle wprowadzone dane!", "Ok");
            }
            else
                await DisplayAlert("Błąd!", "Być może straciłeś uprawnienia admina, zaloguj się jeszcze raz do systemu. Możesz nadal korzystać z podstawowych funkcji aplikacji.", "Ok");
        }

        private async void LogoutUser(object sender, EventArgs e)
        {           
            await Data.GetUserDataByID(Data.UserData.ID);
            if (Data.UserData.Status == Status.admin)
            {
                var usr = UserData;
                usr.IsActive = false;
                if (!await Task.Run(() => Data.UpdateUserData(usr)))
                    await DisplayAlert("Błąd!", "Nie można wylogować użytkownika", "Ok");
                else
                    Data.CreateNewLog(new EventData
                    {
                        Autor = Data.UserData.Nickname,
                        Date = DateTime.Now,
                        Details = $"Wylogowano użytkownika {UserData.Nickname}"
                    });
                Navigation.RemovePage(this);
            }
            else
                await DisplayAlert("Błąd!", "Być może straciłeś uprawnienia admina, zaloguj się jeszcze raz do systemu. Możesz nadal korzystać z podstawowych funkcji aplikacji.", "Ok");
        }

        private async void DeleteAccount(object sender, EventArgs e)
        {
            var res = await DisplayPromptAsync("Informacja", $"Czy napewno chcesz usunąć konto użytkownika '{UserData.Nickname}({UserData.ID})'? Jeśli tak wpisz 'potwierdzam'", "Potwierdź", "Anuluj");

            await Data.GetUserDataByID(Data.UserData.ID);
            if (Data.UserData.Status == Status.admin)
            {
                if (res == "potwierdzam")
                {
                    if (Data.DeleteAccount(UserData.ID))
                    {
                        await DisplayAlert("Informacja!", $"Usunięto użytkownika '{UserData.Nickname}'", "Ok");
                        Data.CreateNewLog(new EventData
                        {
                            Autor = Data.UserData.Nickname,
                            Date = DateTime.Now,
                            Details = $"Usunął użytkownika {UserData.Nickname}"
                        });
                        Navigation.RemovePage(this);
                    }
                    else
                        await DisplayAlert("Błąd!", $"Nie udało się usunąć użytkownika", "Ok");
                }
            }
            else
                await DisplayAlert("Błąd!", "Być może straciłeś uprawnienia admina, zaloguj się jeszcze raz do systemu. Możesz nadal korzystać z podstawowych funkcji aplikacji.", "Ok");
        }

        private void LoadUserDataUI()
        {
            id.Text = $"{UserData.Nickname} ({UserData.ID})\nPłeć: {UserData.Gender}";
            email.Text = UserData.Email;
            points.Text = UserData.Points.ToString();
            picker.SelectedIndex = (int)UserData.Status;
            toggle.IsChecked = UserData.RequirePasswordReset;
        }
    }
}