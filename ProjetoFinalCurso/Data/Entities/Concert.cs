using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoFinalCurso.Data.Entities
{
    public class Concert : IEntity
    {
        public int Id { get; set; }
        
        public Establishment Establishmento { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime Day { get; set; }

        public ICollection<Band> Bands { get; set; }


        [Display(Name = "Number of Bands")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity => Bands == null ? 0 : Bands.Count;

        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty ? $"https://nfgtickets.azurewebsites.net/images/noimage.png"
            : $"https://nfgtickets.blob.core.windows.net/concerts/{ImageId}";


        public User User { get; set; }
    }
}
