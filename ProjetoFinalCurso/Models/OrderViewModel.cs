using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoFinalCurso.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Models
{
    public class OrderViewModel : Order
    {
        public bool Verify { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a payment method.")]
        public int PaymentId { get; set; }

        public IEnumerable<SelectListItem> Payments { get; set; }
    }
}
