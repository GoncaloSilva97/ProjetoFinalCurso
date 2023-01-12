using Microsoft.EntityFrameworkCore;
using ProjetoFinalCurso.Data.Entities;
using System.Linq;

namespace ProjetoFinalCurso.Data.Repository
{
    public class BandRepository : GenericRepository<Band>, IBandRepository
    {
        private readonly DataContext _context;

        public BandRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Bands.Include(p => p.User);
        }
    }
}
