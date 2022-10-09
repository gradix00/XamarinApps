using System;
using System.Collections.Generic;
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

        public EventsLog()
        {
            InitializeComponent();
            LoadLogs(Data);
        }

        private void LoadLogs(Authentication data)
        {                      
            ObservableCollection<EventCell> eventCells = new ObservableCollection<EventCell>();
            var tempList = data.GetListAllEventLog();

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
        }

        private async void RefresPostsList(object sender, EventArgs e)
        {
            await System.Threading.Tasks.Task.Delay(750);
            LoadLogs(Data);
        }
    }
}