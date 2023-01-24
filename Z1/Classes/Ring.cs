using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    [Serializable]
    public class Ring
    {
        
        public string Name { get; set; } //sve je property zbog bindinga
        public double Price { get; set; }
        public DateTime Date { get; set; }

        public string PathImage { get; set; }

        public string PathData { get; set; }

        public int ID { get; set; }


        public Ring() //za serijalizaciju
        { 

        }


        public Ring(string name, double price, DateTime date, string pathI, string pathDat, int id)
        {
            Name = name;
            Price = price;
            Date = date;
            PathImage = pathI;
            PathData = pathDat;
            ID = id;
        }
    }
}
