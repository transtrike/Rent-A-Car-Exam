using AutoMapper;
using Project.Services.Models.Car;
using Project.Web.Models.Car;

namespace Project.Web.Mappings
{
    public class CarMappings : Profile
    {
        public CarMappings()
        {
            CreateMap<CarWebModel, CarServiceModel>();
            CreateMap<CarServiceModel, CarWebModel>();

            CreateMap<RentACarWebModel, RentACarServiceModel>();
        }
    }
}
