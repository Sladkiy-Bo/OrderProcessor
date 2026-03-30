using System;

namespace OrderProcessor.Models
{
    public class Order
    {
        public uint ID {set; get;}
        public decimal Amount{set; get;}
        public string Status{set; get;}
        public string ProcessedAt{set; get;}
    }
}