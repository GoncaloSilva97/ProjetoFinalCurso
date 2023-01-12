using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Data.Entities
{
    public class TicketType : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Type")]
        public string Name { get; set; }

        public User User { get; set; }
    }
}
