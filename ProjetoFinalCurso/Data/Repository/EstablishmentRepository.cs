using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalCurso.Data.Repository
{
    public class EstablishmentRepository  : GenericRepository<Establishment>, IEstablishmentRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public EstablishmentRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public IQueryable<Establishment> GetAllSimple()
        {
            return _context.Establishments.Include(p => p.User);
        }

        public IQueryable<Establishment> GetAllWithUsers(bool b, User user)
        {
            
            if (!b)
            {
                return _context.Establishments
                    .Include(u => u.User)
                    .Where(o => o.User == user)
                    .OrderByDescending(o => o.Name);
                
            }


            return _context.Establishments
                    .Include(u => u.User)
                    .OrderByDescending(o => o.Name);

        }

        public IEnumerable<SelectListItem> GetComboUsers(bool b, User user)
        {

            var list = new List<SelectListItem>();


            if (!b)
            {
                var list2 = _context.Users
                    .Where(p => p.Id == user.Id);

                foreach (var item in list2)
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.UserName,
                        Value = item.Id.ToString()

                    });
                }

            }
            else
            {
                list = _context.Users.Select(c => new SelectListItem
                {
                    Text = c.UserName,
                    Value = c.Id.ToString()

                }).OrderBy(l => l.Text).ToList();
            }



            return list;

        }
    }
}
