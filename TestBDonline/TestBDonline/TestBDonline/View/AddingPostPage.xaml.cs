using System;
using TestBDonline.Scripts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingPostPage : ContentPage
    {
        private Authentication Data { get; set; }

        public AddingPostPage(Authentication data)
        {
            InitializeComponent();
            Data = data;
            user.Text = $"Dodajesz post jako {data.UserData.Nickname} ({data.UserData.ID})";
        }

        private void LoadImage(object sender, EventArgs e)
        {
            img.Source = url.Text;
        }

        private void CreatePost(object sender, EventArgs e)
        {
            if (FieldsTextAreFill())
            {
                var post = new Scripts.Structs.PostData
                {
                    Autor = Data.UserData.Nickname,
                    Date = DateTime.Now,
                    Title = title.Text,
                    Description = description.Text,
                    UrlImage = url.Text
                };

                if (Data.CreatePost(post))
                {
                    DisplayAlert("Opublikowano post!", $"Dane postu:\nTytuł: {post.Title}\nOpis: {post.Description}\nUrl: {post.UrlImage}", "Ok");

                    Data.CreateNewLog(new Scripts.Structs.EventData
                    {
                        Autor = Data.UserData.Nickname,
                        Details = $"Opublikowano post; Tytuł: {post.Title}; Opis: {post.Description}",
                        Date = DateTime.UtcNow
                    });
                }
                else
                    DisplayAlert("Błąd tworzenia posta", "Być może nasza baza nie działa albo nie masz połączenia z internetem :<", "Ok");
                Navigation.RemovePage(this);
            }
            else
                DisplayAlert("Pola są puste!", "Wypełnij pola, aby utworzyć post", "Ok");
        }

        private bool FieldsTextAreFill()
        {
            if (title.Text == null || description.Text == null || url.Text == null)
                return false;
            return true;
        }
    }
}