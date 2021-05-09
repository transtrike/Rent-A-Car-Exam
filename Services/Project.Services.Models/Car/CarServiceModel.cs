using System;
using Microsoft.AspNetCore.Http;
using Project.Common;

namespace Project.Services.Models.Car
{
    public class CarServiceModel
    {
        public Guid Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public DateTime Year { get; set; }

        public int PassengersCount { get; set; }

        public string Description { get; set; }

        public decimal PricePerDay { get; set; }

        public IFormFile Picture { get; set; }

        public string PictureUrl { get; set; }

        public CarStatus CarStatus { get; set; }
    }
}
