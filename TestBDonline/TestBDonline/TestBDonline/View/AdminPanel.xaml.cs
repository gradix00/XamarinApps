using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TestBDonline.Scripts;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPanel : ContentPage
    {
        private Authentication data;
        public AdminPanel(Authentication data)
        {
            InitializeComponent();
            this.data = data;
        }

        private void OpenPageAddingPost(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddingPostPage(data));
        }

        private void OpenPageEventsLog(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EventsLog());
        }

        private void OpenPageUsersManagement(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserManagement(data));
        }

        private async void ClearGlobalChat(object sender, EventArgs e)
        {
            var res = await DisplayPromptAsync("Informacja", "Czy napewno chcesz wyczyścić globalny czat? Jeśli tak wpisz 'potwierdzam'", "Potwierdź", "Anuluj");

            await data.GetUserDataByID(data.UserData.ID);
            if (data.UserData.Status == Scripts.Structs.Status.admin)
            {
                if (res == "potwierdzam")
                {
                    if (data.TruncateTable("GlobalChat"))
                    {
                        await DisplayAlert("Informacja", "Wyczyszczono cały globalny czat!", "Ok");
                        data.CreateNewLog(new Scripts.Structs.EventData
                        {
                            Autor = data.UserData.Nickname,
                            Date = DateTime.Now,
                            Details = $"Usunięto historie globalnego czatu"
                        });
                    }
                    else
                        await DisplayAlert("Informacja", "Nie udało się wyczyścić globalnego czatu", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Błąd!", "Być może straciłeś uprawnienia admina, zaloguj się jeszcze raz do systemu. Możesz nadal korzystać z podstawowych funkcji aplikacji.", "Ok");
            }
        }

        private async void ClearEventLog(object sender, EventArgs e)
        {
            var res = await DisplayPromptAsync("Informacja", "Czy napewno chcesz wyczyścić dziennik zdarzeń? Jeśli tak wpisz 'potwierdzam'", "Potwierdź", "Anuluj");


            await data.GetUserDataByID(data.UserData.ID);
            if (data.UserData.Status == Scripts.Structs.Status.admin)
            {
                if (res == "potwierdzam")
                {
                    if (data.TruncateTable("EventLog"))
                    {
                        await DisplayAlert("Informacja", "Wyczyszczono dziennik zdarzeń!", "Ok");
                        data.CreateNewLog(new Scripts.Structs.EventData
                        {
                            Autor = data.UserData.Nickname,
                            Date = DateTime.Now,
                            Details = $"Usunięto historie dziennika zdarzeń"
                        });
                    }
                    else
                        await DisplayAlert("Informacja", "Nie udało się wyczyścić dzienniku zdarzeń", "Ok");
                }
            }
            else
                await DisplayAlert("Błąd!", "Być może straciłeś uprawnienia admina, zaloguj się jeszcze raz do systemu. Możesz nadal korzystać z podstawowych funkcji aplikacji.", "Ok");
        }
    }
}