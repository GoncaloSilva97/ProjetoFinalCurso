using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalCurso.Data.Repository
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        IQueryable GetTicketsSimple();

        IQueryable GetTicketsWithConcert(bool b, User user);

        Task<Ticket> GetbyIdTicketsWithConcertAsync(int id);

        Task<TicketViewModel> AddConcertAsync(TicketViewModel model);

        IEnumerable<SelectListItem> GetComboAllConcert();

        IEnumerable<SelectListItem> GetComboConcert(bool b, User user);

        IEnumerable<SelectListItem> GetComboTickets();
        IEnumerable<SelectListItem> GetComboTicketTypes();

        public Task<List<SelectListItem>> VerifyType(int concertId);

        public Task<List<SelectListItem>> VerifyTickets(int concertId);
    }
}
