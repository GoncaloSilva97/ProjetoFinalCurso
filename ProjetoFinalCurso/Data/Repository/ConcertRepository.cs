using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalCurso.Data.Repository
{
    public class ConcertRepository : GenericRepository<Concert>, IConcertRepository
    {
        private readonly DataContext _context;

        public ConcertRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetConcertsSimple()
        {
            return _context.Concerts
                .Include(c => c.User)
                .OrderBy(c => c.Day);
        }

        public IQueryable GetConcertsWithBands(bool b, User user)
        {
            if (!b)
            {
                return _context.Concerts
                    .Include(c => c.Bands)
                    .Include(c => c.Establishmento)
                    .Include(u => u.User)
                    .Where(o => o.User == user)
                    .OrderByDescending(o => o.Day);

            }

            return _context.Concerts
                .Include(c => c.Bands)
                .Include(c => c.Establishmento)
                    .Include(u => u.User)
                    .OrderByDescending(o => o.Day);
        }

        public async Task<Concert> GetByIdConcertsWithBandsAsync(int id)
        {
            return await _context.Concerts
                .Include(c => c.Bands)
                .Include(c => c.User)    
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Band> GetBandAsync(int id)
        {
            return await _context.Bands.FindAsync(id);
        }

        public async Task<ConcertBandViewModel> AddBandAsync(ConcertBandViewModel model)
        {
            var country = await this.GetByIdConcertsWithBandsAsync(model.ConcertId);
            if (country == null)
            {
                return model;
            }

            var band = await _context.Bands.FindAsync(model.BandId);

            country.Bands.Add(band);
            _context.Concerts.Update(country);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<int> RemoveBandAsync(Band band)
        {
            var country = await _context.Concerts
                .Where(c => c.Bands.Any(ci => ci.Id == band.Id))
                .FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            country.Bands.Remove(band);

            await _context.SaveChangesAsync();
            return country.Id;
        }



        public IEnumerable<SelectListItem> GetComboBands(Concert concert)
        {
            

            var list = new List<SelectListItem>();
            if (concert != null)
            {

                list = _context.Bands.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()

                }).OrderBy(l => l.Text).ToList();

            inicio:

                if(concert.Bands != null)
                {
                    foreach (var item in list)
                    {
                        foreach (var item2 in concert.Bands)
                        {
                            if (item.Value == item2.Id.ToString())
                            {
                                list.Remove(item);
                                goto inicio;
                            }
                        }

                    }
                }

                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a band...)",
                    Value = "0"
                });

            }
            return list;

        }

        public IEnumerable<SelectListItem> GetComboEstablishments(bool b, User user)
        {
            var list = new List<SelectListItem>();

            
            if (!b)
            {
                var list2 =  _context.Establishments
                    .Include(p => p.User)
                    .Where(p => p.User == user);

                foreach (var item in list2)
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()

                    });
                }

            }
            else
            {
                list = _context.Establishments.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()

                }).OrderBy(l => l.Text).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a concert...)",
                Value = "0"
            });

            return list;
        }



        //public async Task<Concert> GetConcertAsync(Band band)
        //{
        //    return await _context.Concerts
        //        .Where(c => c.Bands.Any(ci => ci.Id == band.Id))
        //        .FirstOrDefaultAsync();
        //}
    }
}
