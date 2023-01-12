using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalCurso.Data.Entities
{
    public class Band : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Members { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Spotify")]
        public string Links { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty ? $"https://nfgtickets.azurewebsites.net/images/noimage.png"
            : $"https://nfgtickets.blob.core.windows.net/bands/{ImageId}";


        public User User { get; set; }
    }
}
