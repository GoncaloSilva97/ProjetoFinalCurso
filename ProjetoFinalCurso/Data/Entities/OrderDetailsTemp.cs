using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Data.Entities
{
    public class OrderDetailsTemp : IEntity
    {
        public int Id { get; set; }

        
        public User User { get; set; }

        
        public Ticket Ticket { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public int Quantity { get; set; }
        
        

        public decimal Value => Price * Quantity;
    }
}
