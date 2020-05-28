using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoinformationModeling.Web.Models;
using GeoinformationModeling.Web.Services.Abstractions;
using GeoinformationModeling.Web.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using GeoinformationModeling.DataAccess.Entities;
using System.Security.Claims;
using GeoinformationModeling.BusinessLogic.Services.Abstractions;

namespace GeoinformationModeling.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMatrixGeoinformaionService matrixGeoinformaionService;
        public readonly ISaveRiverDataService saveRiverDataService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMatrixGeoinformaionService matrixGeoinformaionService, IMapper mapper,
            ISaveRiverDataService saveRiverDataService)
        {
            this.matrixGeoinformaionService = matrixGeoinformaionService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.saveRiverDataService = saveRiverDataService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       public IActionResult GeoinformationModelingCalculator(int? riverId)
        {
            if (!riverId.HasValue)
            {
                return View(new GeoinformationModelingViewModel
                {
                    PiverParams = new Entities.RiverParams
                    {
                        LengthByX = 1,
                        SplitsNumberByX = 20,
                        LengthByT = 1,
                        SplitsNumberByT = 50,
                        OutputMultiplicityByX = 2,
                        OutputMultiplicityByT = 5
                    },
                    TaskParams = new TaskParams
                    {
                        GravityAcceleration = 9.8,
                        FreeSurfaceWidth = 20,
                        ChannelHydraulicRadius = 1,
                        ShaziCoefficient = 60,
                        AngleOfInclinationSine = 0.1,
                        AngleSine = 0.1,
                        Alpha = 1
                    },
                    MapPoints = new List<MapPointModel>()
                });
            }
            else
            {
                GeoinformationModelingViewModel model = new GeoinformationModelingViewModel { 
                    TaskParams = new TaskParams(), PiverParams = new Entities.RiverParams(), MapPoints = new List<MapPointModel>() };

                var res = saveRiverDataService.GetRiverById(riverId.Value);
                model.TaskParams = mapper.Map<TaskParams>(res);
                model.PiverParams = mapper.Map<Entities.RiverParams>(res);
                model.MapPoints = mapper.Map<List<MapPointModel>>(res.MapParamsList).OrderBy(t => t.SequenceNumber).ToList();
                model = mapper.Map<DataAccess.Entities.RiverParams, GeoinformationModelingViewModel>(res, model);
                return View(model);
            }
        }

        public ActionResult GetAllRivers()
        {
            List<GeoinformationModelingViewModel> rivers = mapper.Map<List<GeoinformationModelingViewModel>>(saveRiverDataService.GetAllRivers(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return View(rivers);
        }

        [HttpPost]
        public IActionResult Calculate(GeoinformationModelingViewModel geoinfotmationModelingViewModel)
        {
            MatrixesModel result = new MatrixesModel();
            try
            {
                geoinfotmationModelingViewModel.TaskParams.AngleSine = geoinfotmationModelingViewModel.TaskParams.AngleOfInclinationSine;
                result = matrixGeoinformaionService.GetMatrixes(geoinfotmationModelingViewModel);
            }
            catch(Exception ex)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
            return Json(new { result });
        }

        [HttpPost]
        public IActionResult SaveRiverData(GeoinformationModelingViewModel geoinfotmationModelingViewModel)
        {
            if(signInManager.IsSignedIn(User))
            {
                geoinfotmationModelingViewModel.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    saveRiverDataService.SaveRiverData(mapper.Map<DataAccess.Entities.RiverParams>(geoinfotmationModelingViewModel), mapper.Map<List<MapParams>>(geoinfotmationModelingViewModel.MapPoints));
            }
            return null;
        }

    }
}
