using System;

namespace SmartFridge.Models
{
    public class FridgeItem
    {
        public int ID { get; set; }
        public string ArticleName { get; set; }
        public int Quantity { get; set; }
        public int Weight { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
    }
}
