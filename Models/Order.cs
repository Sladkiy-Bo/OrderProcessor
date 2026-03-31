using System;

namespace OrderProcessor.Models
{
    public class Order
    {
        public int ID {set; get;}
        public decimal Amount{set; get;}
        public string Status{set; get;}
        public DateTime? ProcessedAt{set; get;}
    }
}