using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinalCurso.Data.Repository;

namespace ProjetoFinalCurso.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TicketController : Controller
	{
        private readonly ITicketRepository _TicketRepository;

        public TicketController(ITicketRepository TicketRepository)
        {
            _TicketRepository = TicketRepository;
        }

        [HttpGet]
        public IActionResult GetAirports()
        {
            return Ok(_TicketRepository.GetTicketsSimple());
        }
    }
}
