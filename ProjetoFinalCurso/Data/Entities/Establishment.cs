using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Data.Entities
{
    public class Establishment : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string City { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty ? $"https://nfgtickets.azurewebsites.net/images/noimage.png"
            : $"https://nfgtickets.blob.core.windows.net/establishments/{ImageId}";

        
        //[Required]
        //[Display(Name = "Data for Tranfer")]
        //public string Transfer { get; set; }
        //[Required]
        //[Display(Name = "Data for MbWay")]
        //public string MBWay { get; set; }

        public User User { get; set; }
    }
}
