using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoFinalCurso.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Models
{
    public class ConcertViewModel  : Concert
    {
        [Required]
        [Display(Name = "Establishment")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select an establishment.")]
        public int EstablishmentId { get; set; }

        public IEnumerable<SelectListItem> ListEstablishments { get; set; }


        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
