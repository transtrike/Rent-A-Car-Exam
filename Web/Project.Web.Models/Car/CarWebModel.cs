using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Project.Common;

namespace Project.Web.Models.Car
{
    public class CarWebModel
    {
        public Guid Id { get; set; }

        [DataType(DataType.Text)]
        public string Brand { get; set; }

        [DataType(DataType.Text)]
        public string Model { get; set; }

        [DataType(DataType.Date)]
        public DateTime Year { get; set; }

        public int PassengersCount { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal PricePerDay { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Picture { get; set; }

        public CarStatus CarStatus { get; set; }
    }
}
