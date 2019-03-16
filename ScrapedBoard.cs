using System;
using System.Collections.Generic;
using System.Text;

namespace SurfboardBrokerScrape
{
    class ScrapedBoard
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public int? Height { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Source { get; set; } = "SurfboardBroker";
        public bool FromInternalUser { get; set; } = false;
        public DateTime Created { get; set; }
        public int Zip { get; set; } = 92010;
        public string City { get; set; } = "San Diego";
    }
}
