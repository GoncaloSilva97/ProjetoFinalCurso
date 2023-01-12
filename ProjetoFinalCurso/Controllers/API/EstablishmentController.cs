using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinalCurso.Data.Repository;

namespace ProjetoFinalCurso.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EstablishmentController : Controller
	{
        private readonly IEstablishmentRepository _EstablishmentRepository;

        public EstablishmentController(IEstablishmentRepository EstablishmentRepository)
        {
            _EstablishmentRepository = EstablishmentRepository;
        }

        [HttpGet]
        public IActionResult GetAirports()
        {
            return Ok(_EstablishmentRepository.GetAllSimple());
        }
    }
}
