using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalCurso.Data.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly DataContext _context;

        public TicketRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetTicketsSimple()
        {
            return _context.Tickets
                .Include(c => c.Concerto)
                .Include(c => c.Type)
                .Include(c => c.User);
        }

        public IQueryable GetTicketsWithConcert(bool b, User user)
        {
            

            if (!b)
            {
                return _context.Tickets
                .Include(c => c.Concerto)
                .Include(c => c.Type)
                .Include(c => c.User)
                .Where(o => o.User == user)
                .OrderBy(c => c.Type);
                    
            }

            return _context.Tickets
                .Include(c => c.Concerto)
                .Include(c => c.User)
                .OrderBy(c => c.Type);
        }

        public async Task<Ticket> GetbyIdTicketsWithConcertAsync(int id)
        {
            return await _context.Tickets
                .Include(c => c.Concerto)
                .Include(c => c.Type)
                .Include(c => c.User)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<TicketViewModel> AddConcertAsync(TicketViewModel model)
        {
            var concert = await _context.Concerts.FindAsync(model.ConcertId);
            var type = await _context.TicketTypes.FindAsync(model.TicketTypeId);

            model.Concerto = concert;
            model.Type = type;

            return model;
        }

       

        IEnumerable<SelectListItem> ITicketRepository.GetComboAllConcert()
        {
            var list = _context.Concerts.Select(p => new SelectListItem
            {
                Text = p.Title,
                Value = p.Id.ToString(),

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an concert...)",
                Value = "0",
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboConcert(bool b, User user)
        {
            var list = new List<SelectListItem>();


            if (!b)
            {
                var list2 = _context.Concerts
                    .Include(p => p.User)
                    .Where(p => p.User == user);

                foreach (var item in list2)
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.Title,
                        Value = item.Id.ToString()

                    });
                }



            }
            else
            {
                list = _context.Concerts.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()

                }).OrderBy(l => l.Text).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an concert...)",
                Value = "0",
            });

            return list;
        }

        IEnumerable<SelectListItem> ITicketRepository.GetComboTickets()
        {
            var list = new List<SelectListItem>();

            

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Ticket...)",
                Value = "0",
            });

            return list;
        }

        IEnumerable<SelectListItem> ITicketRepository.GetComboTicketTypes()
        {
            var list = new List<SelectListItem>();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a ticket type...)",
                Value = "0",
            });

            return list;
        }

        public async Task<List<SelectListItem>> VerifyType(int concertId)
        {
            var concert = await _context.Concerts.FindAsync(concertId);
            var list = new List<SelectListItem>();

            if (concert != null)
            {

                var tickets = await _context.Set<Ticket>()
                    .Include(p => p.Concerto)
                    .Include(p=> p.Type)
                    .Where(p => p.Concerto.Id == concertId)
                    .ToListAsync();

                //list = ticket type;

                list = _context.TicketTypes.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),

                }).ToList();

                if (tickets != null)
                {
                    foreach (var item in tickets)
                    {
                        SelectListItem delete = new SelectListItem
                        {
                            Text = "Flight Sold Out.",
                            Value = "0",
                        };


                        foreach (var item2 in list)
                        {
                            if (item.Type.Name == item2.Text)
                            {
                                delete = item2;
                            }
                        }

                        if (delete.Value != "0")
                        {
                            list.Remove(delete);
                        }

                    }
                }

                

                if (list.Count == 0)
                {


                    list.Insert(0, new SelectListItem
                    {
                        Text = "You have already created all possible tickets for this concert.",
                        Value = "0",
                    });
                }

            }
            else
            {
                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a concert...)",
                    Value = "0",
                });
            }

            return list;
        }

        public async Task<List<SelectListItem>> VerifyTickets(int concertId)
        {
            var concert = await _context.Concerts.FindAsync(concertId);
            var selectedTickets = new List<SelectListItem>();

            if (concert != null)
            {

                var concertTickets = await _context.Set<Ticket>()
                    .Include(p => p.Concerto)
                    .Include(p => p.Type)
                    .Where(p => p.Concerto.Id == concertId)
                    .ToListAsync();

                //list = ticket type;

                var allTickets = _context.Tickets.Select(p => new SelectListItem
                {
                    Text = p.Code,
                    Value = p.Id.ToString(),

                }).ToList();

                if (concertTickets != null)
                {
                    foreach (var item in concertTickets)
                    {
                        SelectListItem delete = new SelectListItem
                        {
                            Text = "Flight Sold Out.",
                            Value = "0",
                        };


                        foreach (var item2 in allTickets)
                        {
                            if (item.Id.ToString() == item2.Value)
                            {
                                selectedTickets.Add(item2);
                            }

                        }

                        if (delete.Value != "0")
                        {
                            allTickets.Remove(delete);
                        }



                    }
                }



                if (selectedTickets.Count == 0)
                {


                    selectedTickets.Insert(0, new SelectListItem
                    {
                        Text = "All tickets for this Concert are sold out.",
                        Value = "0",
                    });
                }

            }
            else
            {
                selectedTickets.Insert(0, new SelectListItem
                {
                    Text = "(Select a concert...)",
                    Value = "0",
                });
            }

            return selectedTickets;
        }
    }
}
