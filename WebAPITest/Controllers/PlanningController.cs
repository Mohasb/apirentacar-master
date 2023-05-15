using Microsoft.AspNetCore.Mvc;
using WebAPITest.Interfaces;
using WebAPITest.Models;

namespace WebAPITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanningController : ControllerBase
    {
        private readonly ILogger<PlanningController> _logger;
        private readonly IGroupService _group;
        private readonly IBranchService _branch;
        private readonly IPlanningService _planning;
        private readonly ICarService _car;
        public PlanningController(ILogger<PlanningController> logger, IGroupService group, IBranchService branch, IPlanningService planning, ICarService car)
        {
            _logger = logger;
            _group = group;
            _branch = branch;
            _planning = planning;
            _car = car;
        }
        [HttpGet]
        public Task<ActionResult<IEnumerable<Planning>>> GetPlannings()
        {
            return _planning.GetAllPlanning();
        }

        [HttpPost("{StartingDate}&{EndingDate}")]
        public ActionResult CreateNewTable(DateTime StartingDate, DateTime EndingDate)
        {
            var listaGrupos = _group.GetAllGroups().Result.Value!;
            var listaSucursal = _branch.GetAllBranches().Result.Value!;
            while(StartingDate < EndingDate)
            {
                foreach(Branch sucursal in listaSucursal)
                {
                    foreach(Group grupo in listaGrupos)
                    {
                        var cantidadDisponible = _car.GetAvailableCarsByGroupBySucursal(sucursal.Name!, grupo.Name!).Result;
                        _planning.AddAvailable(new Planning(StartingDate, sucursal.Name!, grupo.Name!, cantidadDisponible));
                    }
                }
                StartingDate = StartingDate.AddMinutes(30);
            }
            return Ok();
        }
    }
}