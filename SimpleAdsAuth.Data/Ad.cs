using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAdsAuth.Data
{
    public class Ad
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }
    }
}
