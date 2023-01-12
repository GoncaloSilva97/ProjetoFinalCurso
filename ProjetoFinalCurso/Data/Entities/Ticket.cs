using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Data.Entities
{
    public class Ticket : IEntity
    {
        public int Id { get; set; }

        public string Code { get; set; }
        [Required]
        public Concert Concerto { get; set; }
        [Required]
        [Display(Name = "Type")]
        public TicketType Type { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public int Stock { get; set; }

        public User User { get; set; }

    }
}
