using System;
using Project.Services.Models.User;

namespace Project.Services.Models.Car
{
    public class RentACarServiceModel
    {
        //Guid renterId, Guid carId, DateTime startDate, DateTime endDate
        public Guid RenterId { get; set; }
        public UserServiceModel Renter { get; set; }

        public Guid CarId { get; set; }
        public CarServiceModel Car { get; set; }

        public DateTime RentalDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
