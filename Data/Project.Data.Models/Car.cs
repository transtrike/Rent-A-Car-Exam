
using System;
using Project.Common;

namespace Project.Data.Models
{
    public class Car : BaseClass
    {
        public string Brand { get; set; }

        public string Model { get; set; }

        public DateTime Year { get; set; }

        public int PassengersCount { get; set; }

        public string Description { get; set; }

        public decimal PricePerDay { get; set; }

        public string PictureUrl { get; set; }

        public CarStatus CarStatus { get; set; }
    }
}
