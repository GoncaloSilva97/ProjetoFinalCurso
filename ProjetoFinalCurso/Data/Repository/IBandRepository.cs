using ProjetoFinalCurso.Data.Entities;
using System.Linq;

namespace ProjetoFinalCurso.Data.Repository
{
    public interface IBandRepository : IGenericRepository<Band>
    {
        public IQueryable GetAllWithUsers();
    }
}
