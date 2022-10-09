using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.Generic;
using TestBDonline.Scripts;
using TestBDonline.Scripts.Structs;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class News : ContentPage
    {
        private Authentication Data { get; set; }

        public News(Authentication data)
        {
            InitializeComponent();
            Title = "Nowości";
            Data = data;
            LoadNews(null, null);
        }

        private void OpenPageAccount(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Account(Data));
        }

        private async void LoadNews(object sender, EventArgs e)
        {
            await System.Threading.Tasks.Task.Delay(750);

            var postsList = Data.GetListAllPosts();
            page.Children.Clear();

            List<Layout> tempList = new List<Layout>();
            foreach(var post in postsList)
            {
                var layout = CreatePost(post);
                tempList.Add(layout);
            }
            tempList.Reverse();
            
            foreach(var layout in tempList)
                page.Children.Add(layout);
            refreshView.IsRefreshing = false;
        }

        private StackLayout CreatePost(PostData data)
        {
            StackLayout layout = new StackLayout
            {
                HeightRequest = 400
            };
            Label title = new Label
            {
                Text = data.Title,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            Label description = new Label
            {
                Text = data.Description,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };

            if (data.UrlImage != "")
            {
                Image img = new Image
                {
                    Source = data.UrlImage,
                    VerticalOptions = LayoutOptions.StartAndExpand
                };
                layout.Children.Add(img);
            }
            else
            {
                Label altImg = new Label
                {
                    Text = "No photo",
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                layout.Children.Add(altImg);
            }

            Label likes = new Label
            {
                Text = "Likes " + data.Likes.ToString(),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };
            Button btn = new Button
            {
                Text = "polub"
            };

            layout.Children.Add(title);
            layout.Children.Add(description);         
            layout.Children.Add(likes);
            layout.Children.Add(btn);
            return layout;
        }
    }
}