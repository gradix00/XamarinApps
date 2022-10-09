using System;
using System.Collections.Generic;
using System.Text;

namespace TestBDonline.Scripts.Structs
{
    public struct PostData
    {
        public string Autor { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public int Likes { get; set; }
    }
}
