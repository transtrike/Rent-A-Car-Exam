using System;

namespace Project.Data.Models
{
    public class RentedCars : BaseClass
    {
        public Guid CarId { get; set; }
        public Car Car { get; set; }

        public Guid RenterId { get; set; }
        public AppUser Renter { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
