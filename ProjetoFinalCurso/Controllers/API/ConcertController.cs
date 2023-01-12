using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinalCurso.Data.Repository;

namespace ProjetoFinalCurso.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ConcertController : Controller
	{
        private readonly IConcertRepository _ConcertRepository;

        public ConcertController(IConcertRepository ConcertRepository)
        {
            _ConcertRepository = ConcertRepository;
        }

        [HttpGet]
        public IActionResult GetAirports()
        {
            return Ok(_ConcertRepository.GetConcertsSimple());
        }
    }
}
