using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoFinalCurso.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Models
{
    public class TicketViewModel : Ticket
    {
        [Required]
        [Display(Name = "Concert")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a concert.")]
        public int ConcertId { get; set; }
        [Required]
        [Display(Name = "Ticket Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a ticket type.")]
        public int TicketTypeId { get; set; }

        public IEnumerable<SelectListItem> ListConcerts { get; set; }

        public IEnumerable<SelectListItem> ListTicketTypes { get; set; }
    }
}
