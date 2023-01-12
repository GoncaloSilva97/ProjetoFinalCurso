using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Models
{
    public class ConcertBandViewModel
    {
        
        public int ConcertId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Band.")]
        [Display(Name = "Band")]
        public int BandId { get; set; }

        public IEnumerable<SelectListItem> ListBands { get; set; }

        
    }
}
