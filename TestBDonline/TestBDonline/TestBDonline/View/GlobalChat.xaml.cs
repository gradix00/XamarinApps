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
        private DateTime lastDate = DateTime.UtcNow;

		public GlobalChat(Authentication data)
        {
            InitializeComponent();
            Data = data;
            LoadMessage(Data);
        }

        private void SentMessage(object sender, EventArgs e)
        {
            if (!Data.CreateNewMessageGlobalChat(new MessageData
            {
                Autor = Data.UserData.Nickname,
                Message = entry.Text,
                Date = DateTime.UtcNow
            })){
                DisplayAlert("Błąd wysyłania", "Nie udało się wysłać wiadomości :<", "Ok");
            }
            LoadMessage(Data);
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

        private void LoadMessage(Authentication data)
        {
            if (page.Children.Count > 0)
                page.Children.Clear();

            var tempList = data.GetAllMessagesData();
            foreach (var message in tempList)
            {
                var lb = new Label
                {
                    Text = $"{message.Date.Month} {new GetStatus().Months[message.Date.Month]} | {message.Date.Hour}:{message.Date.Minute}"
                };
                page.Children.Add(lb);

                Align align = data.UserData.Nickname == message.Autor ? Align.right : Align.left;

                if(align == Align.right)
                    lb.HorizontalOptions = LayoutOptions.End;

                page.Children.Add(CreateMessage(message, align));
            }
        }

        private Frame CreateMessage(MessageData data, Align align)
        {
            Frame frame = new Frame
            {
                BackgroundColor = Color.DimGray,
                BorderColor = Color.LightGoldenrodYellow,  
                HorizontalOptions = LayoutOptions.Start
            };

            string msg = (align == Align.left) ? $"{data.Autor}: {data.Message}" : $"ty: {data.Message}";
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

            frame.Content = label;
            return frame;
        }
    }
}