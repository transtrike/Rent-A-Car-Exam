using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Common;
using Project.Services.Models.Car;

namespace Project.Services.Interfaces
{
    public interface ICarService
    {
        Task<CarServiceModel> CreateCarAsync(CarServiceModel carServiceModel);

        Task<List<CarServiceModel>> GetAllCarsAsync();

        Task<List<CarServiceModel>> GetAllAvailableCarsAsync();

        Task<List<CarServiceModel>> GetAllAvailableCarsInDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<List<CarServiceModel>> GetAllCarsRentedByUserAsync();

        Task<CarStatus> GetCarStatusAsync(string status);

        Task<CarServiceModel> GetCarByIdAsync(Guid carId);

        Task<CarServiceModel> RentACarAsync(RentACarServiceModel rentACarServiceModel);

        Task<CarServiceModel> EditCarAsync(CarServiceModel carServiceModel);

        Task<CarServiceModel> DeleteCarAsync(Guid carId);
    }
}
