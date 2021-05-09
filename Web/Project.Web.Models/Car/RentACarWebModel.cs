using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Web.Models.Car
{
    public class RentACarWebModel
    {
        [Required]
        public Guid CarId { get; set; }

        public CarWebModel Car { get; set; }

        [Required]
        public Guid RenterId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RentalDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
    }
}
