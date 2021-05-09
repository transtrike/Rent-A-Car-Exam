using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Common;
using Project.Data;
using Project.Data.Models;
using Project.Services.Interfaces;
using Project.Services.Models.Car;

namespace Project.Services.Services
{
    public class CarService : BaseService, ICarService
    {
        private readonly ProjectContext _context;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public CarService(ProjectContext context,
            IMapper mapper,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
            : base(context)
        {
            this._context = context;
            this._mapper = mapper;
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        public async Task<CarServiceModel> CreateCarAsync(CarServiceModel carServiceModel)
        {
            await EnsureBrandModelDoesNotExistAsync(carServiceModel.Brand, carServiceModel.Model);

            Car newCar = this._mapper.Map<Car>(carServiceModel);

            await this._context.AddAsync(newCar);
            await this.SaveChangesAsync();

            newCar = await this.GetCarByBrandAndModelAsync(carServiceModel.Brand, carServiceModel.Model);

            return this._mapper.Map<CarServiceModel>(newCar);
        }

        public async Task<List<CarServiceModel>> GetAllAvailableCarsAsync()
        {
            List<Car> allCars = await this._context.Cars.ToListAsync();
            List<Car> availableCars = new();

            foreach (var car in allCars)
            {
                bool isCarTaken = await this._context.RentedCars.AnyAsync(x => x.CarId == car.Id);
                if (!isCarTaken)
                    availableCars.Add(car);
            }

            return this._mapper.Map<List<CarServiceModel>>(availableCars);
        }

        public async Task<List<CarServiceModel>> GetAllCarsRentedByUserAsync()
        {
            List<Car> allCars = await this._context.Cars.ToListAsync();
            AppUser loggedInUser = await this.GetLoggedInUserAsync();
            List<Car> rentedCars = new();

            foreach (var car in allCars)
            {
                bool isCarTaken = await this._context.RentedCars
                    .AnyAsync(x => x.RenterId == Guid.Parse(loggedInUser.Id));

                if (isCarTaken)
                    rentedCars.Add(car);
            }

            return this._mapper.Map<List<CarServiceModel>>(rentedCars);
        }

        public async Task<List<CarServiceModel>> GetAllAvailableCarsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            List<Car> allCars = await this._context.Cars.ToListAsync();
            List<Car> availableCarsInRange = new();

            foreach (var car in allCars)
            {
                //TODO: TEST!!!!
                bool isCarTaken = await this._context.RentedCars
                    .AnyAsync(x => x.CarId == car.Id
                        && x.StartDate <= startDate
                        && x.StartDate >= endDate);

                if (!isCarTaken)
                    availableCarsInRange.Add(car);
            }

            return this._mapper.Map<List<CarServiceModel>>(availableCarsInRange);
        }

        /// <summary>
        /// Admin only!
        /// </summary>
        /// <returns></returns>
        public async Task<List<CarServiceModel>> GetAllCarsAsync()
        {
            List<Car> allCars = await this._context.Cars.ToListAsync();

            return this._mapper.Map<List<CarServiceModel>>(allCars);
        }

        public async Task<CarServiceModel> GetCarByIdAsync(Guid carId)
        {
            Car car = await QueryFullCarByIdAsync(carId);

            return this._mapper.Map<CarServiceModel>(car);
        }

        public async Task<CarStatus> GetCarStatusAsync(string status)
        {
            return await this._context.CarStatuses.FirstOrDefaultAsync(x => x.Name == status);
        }

        public async Task<CarServiceModel> RentACarAsync(RentACarServiceModel rentACarServiceModel)
        {
            bool isRented = await this._context.RentedCars.AnyAsync(x => x.CarId == rentACarServiceModel.CarId);
            if (isRented)
                throw new InvalidOperationException(string.Format(ConstantsClass.AlreadyRented, ConstantsClass.Car));

            Car carToBeRented = await this.QueryFullCarByIdAsync(rentACarServiceModel.CarId);
            AppUser issuer = await this.GetLoggedInUserAsync();

            RentedCars rentedCarRelation = new()
            {
                CarId = carToBeRented.Id,
                Car = carToBeRented,
                RenterId = Guid.Parse(issuer.Id),
                Renter = issuer,
                StartDate = rentACarServiceModel.RentalDate,
                EndDate = rentACarServiceModel.DueDate
            };

            //Default status
            carToBeRented.CarStatus = await this.GetCarStatusAsync(CarStatus.Awaited);

            await this._context.RentedCars.AddAsync(rentedCarRelation);
            this._context.Update(carToBeRented);
            bool succeeded = await this.SaveChangesAsync();

            return this._mapper.Map<CarServiceModel>(carToBeRented);
        }

        public async Task<CarServiceModel> EditCarAsync(CarServiceModel carServiceModel)
        {
            bool isAdmin = await this.IsLoggedInUserAdminAsync();
            Car car = this._mapper.Map<Car>(carServiceModel);

            if (isAdmin)
            {
                this._context.Update(car);
                bool succeeded = await this.SaveChangesAsync();

                if (!succeeded)
                    throw new InvalidOperationException(string.Format(ConstantsClass.UnableToEdit, nameof(Car)));

                return await this.GetCarByIdAsync(carServiceModel.Id);
            }

            //TODO: More validations
            throw new NotImplementedException();
        }

        public async Task<CarServiceModel> DeleteCarAsync(Guid carId)
        {
            Car car = await this.QueryFullCarByIdAsync(carId);

            this._context.Remove(car);
            bool successfull = await this.SaveChangesAsync();

            if (!successfull)
                throw new InvalidOperationException(string.Format(ConstantsClass.Error));

            return this._mapper.Map<CarServiceModel>(car);
        }

        /* Private methods */

        private async Task<Car> GetCarByBrandAndModelAsync(string brand, string model)
        {
            Car car = await this._context.Cars
                .FirstOrDefaultAsync(x => x.Brand == brand && x.Model == model);

            if (car is null)
                throw new ArgumentException(string.Format(ConstantsClass.DoesNotExist, nameof(Car)));

            return car;
        }

        private async Task<Car> QueryFullCarByIdAsync(Guid carId)
        {
            Car car = await this._context.Cars
                .Include(x => x.CarStatus)
                .FirstOrDefaultAsync(x => x.Id == carId);

            if (car is null)
                throw new ArgumentException(string.Format(ConstantsClass.DoesNotExist, nameof(Car)));

            return car;
        }

        private async Task EnsureBrandModelDoesNotExistAsync(string brand, string model)
        {
            bool exists = await this._context.Cars
                .AnyAsync(x => x.Brand == brand && x.Model == model);

            if (exists)
                throw new ArgumentException(string.Format(ConstantsClass.AlreadyExists, nameof(Car)));
        }

        private async Task<bool> IsLoggedInUserAdminAsync()
        {
            AppUser loggedUser = await this.GetLoggedInUserAsync();
            IList<string> userRoles = await this._userManager.GetRolesAsync(loggedUser);
            bool isAdmin = userRoles.Any(x => x == AppRole.Admin); //Check here if issuer is an admin

            return isAdmin;
        }

        private async Task<AppUser> GetLoggedInUserAsync()
        {
            return await this._userManager.GetUserAsync(this._signInManager.Context.User);
        }
    }
}
