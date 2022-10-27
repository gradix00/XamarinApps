using System;
using TestBDonline.Scripts;
using TestBDonline.Scripts.Structs;
using TestBDonline.Scripts.Dictionary;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestBDonline.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GlobalChat : ContentPage
	{
        enum Align 
        {
            left, 
            right
        }

		private Authentication Data;
        private int msg = 10;
        private DateTime lastDate = DateTime.UtcNow;

		public GlobalChat(Authentication data)
        {
            InitializeComponent();
            Data = data;
            LoadMessage(Data);
        }

        private void SentMessage(object sender, EventArgs e)
        {
            if (entry.Text != null)
            {
                if (!Data.CreateNewMessageGlobalChat(new MessageData
                {
                    Autor = Data.UserData.Nickname,
                    Message = entry.Text,
                    Date = DateTime.Now
                }))
                {
                    DisplayAlert("Błąd wysyłania", "Nie udało się wysłać wiadomości :<", "Ok");
                }
                LoadMessage(Data);
            }
            else
                DisplayAlert("Błąd!", "Musisz wpisać coś do pola tekstowego", "Ok");
            btn.IsEnabled = false;
            entry.Text = null;
        }

        private void EntryIsNotEmpty(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (entry.Text != null)
                btn.IsEnabled = true;
            else
                btn.IsEnabled = false;
        }

        private void Refresh(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Delay(750);
            LoadMessage(Data);
            refresh.IsRefreshing = false;
        }

        private void LoadMessage(Authentication data)
        {
            if (page.Children.Count > 0)
                page.Children.Clear();

            var tempList = data.GetAllMessagesData(msg);
            tempList.Reverse();

            if (msg > tempList.Count)
            {
                msg = tempList.Count;
                DisplayAlert("Informacja", "Nie ma już więcej wiadomości!", "Ok");
            }
            else
            {
                var btnLoad = new Button
                {
                    Text = "Załaduj więcej",
                    BackgroundColor = Color.LightGoldenrodYellow,
                    TextColor = Color.DimGray,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                btnLoad.Clicked += LoadMore;
                page.Children.Add(btnLoad);
            }
            foreach (var message in tempList)
            {
                var lb = new Label
                {
                    Text = $"{message.Date.Day} {new GetStatus().Months[message.Date.Month]} | {message.Date.Hour}:{message.Date.Minute}",
                    TextColor = Color.White
                };
                page.Children.Add(lb);

                Align align = data.UserData.Nickname == message.Autor ? Align.right : Align.left;

                if(align == Align.right)
                    lb.HorizontalOptions = LayoutOptions.End;

                page.Children.Add(CreateMessage(message, align));
            }
        }

        private void LoadMore(object sender, EventArgs e)
        {
            msg += 5;
            LoadMessage(Data);
        }

        private void InfoMessage(object sender, EventArgs e)
        {
            TapGestureRecognizer gesture = (TapGestureRecognizer)(sender as Frame).GestureRecognizers[0];
            DisplayAlert("Szczegóły", $"{gesture.CommandParameter}", "Ok");
        }

        private Frame CreateMessage(MessageData data, Align align)
        {
            Frame frame = new Frame
            {
                BackgroundColor = Color.DimGray,
                BorderColor = Color.LightGoldenrodYellow,  
                HorizontalOptions = LayoutOptions.Start,
            };

            string msg = (align == Align.left) ? $"{data.Autor}: {data.Message}" : $"{data.Message}";
            Label label = new Label
            {
                TextColor = Color.White,
                Text = msg,
            };

            if (align == Align.right)
            {
                frame.HorizontalOptions = LayoutOptions.EndAndExpand;
                frame.BackgroundColor = Color.CornflowerBlue;
            }

            var gesture = new TapGestureRecognizer();
            gesture.Tapped += InfoMessage;
            gesture.CommandParameter = $"Autor: {data.Autor}\nWiadomość: {data.Message}\nData: {data.Date}";
            frame.GestureRecognizers.Add(gesture);
            frame.Content = label;
            return frame;
        }
    }
}