using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoFinalCurso.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Models
{
    public class EstablishmentViewModel : Establishment
    {
        [Required]
        [Display(Name = "User")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select an user.")]
        public string UserId { get; set; }

        public IEnumerable<SelectListItem> ListUsers { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
