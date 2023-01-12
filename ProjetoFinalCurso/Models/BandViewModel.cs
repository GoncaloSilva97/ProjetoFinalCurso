using Microsoft.AspNetCore.Http;
using ProjetoFinalCurso.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Models
{
    public class BandViewModel : Band
    {
        
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        
    }
}
