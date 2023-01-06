using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TestBDonline.Scripts;
using TestBDonline.View.CustomViewCell;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventsLog : ContentPage
    {   
        private Authentication Data = new Authentication();
        private int records = 10;

        public EventsLog()
        {
            InitializeComponent();
            LoadLogs(Data);         
        }

        private async void LoadLogs(Authentication data)
        {                      
            ObservableCollection<EventCell> eventCells = new ObservableCollection<EventCell>();
            var tempList = await Task.Run(()=> data.GetListAllEventLog(records));

            foreach (var log in tempList)
            {
                eventCells.Add(new EventCell
                {
                    ID = log.ID,
                    Autor = log.Autor,
                    Details = log.Details,
                    Date = log.Date
                });
            }
            list.ItemsSource = eventCells;
            list.IsRefreshing = false;
            rec.Text = $"Dziennik zdarzeń ({tempList.Count})";

            if (records > tempList.Count)
            {
                records = tempList.Count;
                await DisplayAlert("Informacja", "Załadowano już wszystkie zdarzenia!", "Ok");
            }
        }

        private void LoadMore(object sender, EventArgs e)
        {
            records += 10;
            LoadLogs(Data);
        }

        private async void RefresPostsList(object sender, EventArgs e)
        {
            LoadLogs(Data);
        }

        private void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var log = (list.SelectedItem as EventCell);

            DisplayAlert($"Informacje o zdarzeniu ({log.ID})", $"Zmodyfikowane przez: {log.Autor}\nOpis: {log.Details}\nData: {log.Date}", "Ok");
        }
    }
}