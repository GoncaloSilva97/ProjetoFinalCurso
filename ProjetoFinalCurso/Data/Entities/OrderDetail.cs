using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Data.Entities
{
    public class OrderDetail : IEntity
    {
        public int Id { get; set; }

        [Required]
        public Ticket Ticket { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public int Quantity { get; set; }

        public decimal Value => Price * Quantity;
    }
}
