using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoFinalCurso.Data.Entities;

namespace ProjetoFinalCurso.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Band> Bands { get; set; }

        public DbSet<Concert> Concerts { get; set; }

        //public DbSet<ConcertBand> ConcertBands { get; set; }

        public DbSet<Establishment> Establishments { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderDetailsTemp> OrderDetailsTemps { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketType> TicketTypes { get; set; }



        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
