using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ProjetoFinalCurso.Models
{
    public class AddItemViewModel
    {
        [Required]
        [Display(Name = "Concert")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Concert.")]
        public int ConcertId { get; set; }

        [Required]
        [Display(Name = "Ticket")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Ticket.")]
        public int TicketId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public int Quantity { get; set; }

        public IEnumerable<SelectListItem> Tickets { get; set; }

        public IEnumerable<SelectListItem> Concerts { get; set; }
    }
}
