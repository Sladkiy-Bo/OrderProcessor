using System;

namespace OrderProcessor.Models
{
    public class Order
    {
        public int Id {get; set;}
        public string OrderNumber { get; set; } = string.Empty;
        public decimal Amount{get; set;}
        public string Status{get; set;}
        public DateTime? ProcessedAt{get; set;}
    }
}