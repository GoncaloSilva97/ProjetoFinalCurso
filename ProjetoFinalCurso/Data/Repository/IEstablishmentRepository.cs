using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoFinalCurso.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalCurso.Data.Repository
{
    public interface IEstablishmentRepository : IGenericRepository<Establishment>
    {
        public IQueryable<Establishment> GetAllSimple();

        public IQueryable<Establishment> GetAllWithUsers(bool b, User user);

        public IEnumerable<SelectListItem> GetComboUsers(bool b, User user);
    }
}
