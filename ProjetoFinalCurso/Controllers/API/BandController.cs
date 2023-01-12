using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinalCurso.Data.Repository;

namespace ProjetoFinalCurso.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BandController : Controller
	{
        private readonly IBandRepository _BandRepository;

        public BandController(IBandRepository BandRepository)
        {
            _BandRepository = BandRepository;
        }

        [HttpGet]
        public IActionResult GetAirports()
        {
            return Ok(_BandRepository.GetAllWithUsers());
        }
    }
}
