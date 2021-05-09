using AutoMapper;
using Project.Data.Models;
using Project.Services.Models.Car;

namespace Project.Services.Mappings
{
    public class CarMappings : Profile
    {
        public CarMappings()
        {
            CreateMap<CarServiceModel, Car>();
            CreateMap<Car, CarServiceModel>();
        }
    }
}
