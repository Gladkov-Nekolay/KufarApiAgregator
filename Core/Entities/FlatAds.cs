using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FlatAds
    {
        public double Size { get; set; }
        public int Floor { get; set; }
        public int Rooms { get; set; }
        public double PricePerSquareMeter { get; set; }
        public int? MetroStation { get; set; }
        public int Price { get; set; }
        public double[] Coordinates { get; set; }
    }
}