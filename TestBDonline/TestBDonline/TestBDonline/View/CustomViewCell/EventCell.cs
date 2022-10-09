using System;
using Xamarin.Forms;

namespace TestBDonline.View.CustomViewCell
{
    internal class EventCell : ViewCell
    {
        public int ID { get; set; }
        public string Autor { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; }
    }
}
