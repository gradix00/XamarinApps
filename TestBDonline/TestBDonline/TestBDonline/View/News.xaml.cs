using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.Generic;
using TestBDonline.Scripts;
using TestBDonline.Scripts.Structs;
using System.Threading.Tasks;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class News : ContentPage
    {
        private Authentication Data { get; set; }

        public News(Authentication data)
        {
            InitializeComponent();
            Data = data;
            LoadNews(null, null);
        }

        private async void OpenPageAccount(object sender, EventArgs e)
        {
            await Data.GetUserDataByID(Data.UserData.ID);
            await Navigation.PushAsync(new Account(Data));
        }

        private void OpenPageGlobalChat(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GlobalChat(Data));
        }

        private async void LoadNews(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            refreshView.IsVisible = false;
            var postsList = await Task.Run(()=> Data.GetListAllPosts(true));
            page.Children.Clear();

            List<Frame> tempList = new List<Frame>();
            foreach(var post in postsList)
            {
                var frame = CreatePost(post);
                tempList.Add(frame);
            }
            tempList.Reverse();

            foreach (var layout in tempList)
                page.Children.Add(layout);

            refreshView.IsRefreshing = false;
            indicator.IsVisible = false;
            refreshView.IsVisible = true;
        }

        private Frame CreatePost(PostData data)
        {
            Frame frame = new Frame
            {
                BorderColor = Color.CornflowerBlue,
                BackgroundColor = Color.White,
                Margin = new Thickness(0, 7, 0, 7),
                CornerRadius = 15
            };
            StackLayout layout = new StackLayout
            {
                MinimumHeightRequest = 400
            };
            Label title = new Label
            {
                Text = data.Title,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.CornflowerBlue
            };
            layout.Children.Add(title);

            Label description = new Label
            {
                Text = data.Description,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.CornflowerBlue
            };

            ImageSource src;
            try
            {
                src = ImageSource.FromUri(new Uri(data.UrlImage));
            }
            catch (Exception)
            {
                src = "https://adishop.az/images/product_empty.png";
            }
            Image img = new Image
            {
                Source = src,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            layout.Children.Add(img);

            Button btn = new Button
            {
                Text = $"Polub ({data.Likes})",
                BackgroundColor = Color.CornflowerBlue,
                TextColor = Color.White
            };

            frame.Content = layout;
            layout.Children.Add(description);
            layout.Children.Add(btn);
            return frame;
        }
    }
}