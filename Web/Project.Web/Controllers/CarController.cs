using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Data.Models;
using Project.Services.Interfaces;
using Project.Services.Models.Car;
using Project.Web.Models.Car;

namespace Project.Web.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;

        public CarController(ICarService carService, IMapper mapper, SignInManager<AppUser> signInManager)
        {
            this._carService = carService;
            this._mapper = mapper;
            this._signInManager = signInManager;
        }

        // GET: CarController
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            List<CarServiceModel> carsServiceModel = await this._carService.GetAllAvailableCarsAsync();
            List<CarWebModel> carsWebModel = this._mapper.Map<List<CarWebModel>>(carsServiceModel);

            return View(carsWebModel);
        }

        // GET: CarController/Details/5
        [HttpGet]
        [Authorize]
        [Route(nameof(Details))]
        public async Task<ActionResult> Details(Guid carId)
        {
            CarServiceModel carServiceModel = await this._carService.GetCarByIdAsync(carId);
            CarWebModel carWebModel = this._mapper.Map<CarWebModel>(carServiceModel);

            return View(carWebModel);
        }

        //TODO: MAKE REQUEST ENDPOINT
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RentACar(Guid carId)
        {
            CarServiceModel carServiceModel = await this._carService.GetCarByIdAsync(carId);
            CarWebModel carWebModel = this._mapper.Map<CarWebModel>(carServiceModel);

            RentACarWebModel rentACarWebModel = new()
            {
                Car = carWebModel,
                RenterId = Guid.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };

            return View(rentACarWebModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRentedCarsByUser()
        {
            List<CarServiceModel> carsServiceModel = await this._carService.GetAllCarsRentedByUserAsync();
            List<CarWebModel> carsWebModel = this._mapper.Map<List<CarWebModel>>(carsServiceModel);

            return View(carsWebModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RentACar(RentACarWebModel rentACarWebModel)
        {
            RentACarServiceModel rentACarServiceModel = this._mapper.Map<RentACarServiceModel>(rentACarWebModel);

            CarServiceModel rentedCar = await this._carService.RentACarAsync(rentACarServiceModel);

            return RedirectToAction(nameof(Details), new { carId = rentedCar.Id });
        }

        // GET: CarController/Create
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<ActionResult> Create([FromForm] CarWebModel carWebModel)
        {
            CarServiceModel carServiceModel = this._mapper.Map<CarServiceModel>(carWebModel);

            await this._carService.CreateCarAsync(carServiceModel);

            return RedirectToAction(nameof(Index));
        }

        // GET: CarController/Edit/5
        [HttpGet]
        [Route(nameof(Edit))]
        [Authorize(Roles = AppRole.Admin + ",")]
        public async Task<ActionResult> Edit(Guid carId)
        {
            CarServiceModel carServiceModel = await this._carService.GetCarByIdAsync(carId);
            CarWebModel carWebModel = this._mapper.Map<CarWebModel>(carServiceModel);

            return View(carWebModel);
        }

        // POST: CarController/Edit/5
        [HttpPost]
        [Route(nameof(Edit))]
        [Authorize(Roles = AppRole.Admin + ",")]
        public async Task<ActionResult> Edit([FromForm] CarWebModel carWebModel)
        {
            CarServiceModel carServiceModel = this._mapper.Map<CarServiceModel>(carWebModel);

            await this._carService.EditCarAsync(carServiceModel);

            return RedirectToAction(nameof(Index));
        }

        // GET: CarController/Delete/5
        [HttpGet]
        [Route(nameof(Delete))]
        [Authorize(Roles = AppRole.Admin + ",")]
        public async Task<ActionResult> Delete(Guid carId)
        {
            CarServiceModel carServiceModel = await this._carService.DeleteCarAsync(carId);

            return RedirectToAction(nameof(Index));
        }
    }
}
