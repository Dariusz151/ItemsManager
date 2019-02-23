using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemsManager.Domain
{
    public class FridgeItem
    {
        public int ID { get; set; }
        public string ArticleName { get; set; }
        public int Quantity { get; set; }
        public int Weight { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
    }
}
