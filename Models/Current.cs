using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pop_Simona_SADE.Models
{
    public class Current
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Current Name")]
        [StringLength(50)]
        public string CurrentName { get; set; }
        [StringLength(70)]
        public string Particularity { get; set; }
        public ICollection<CurrentPainting> CurrentPaintings { get; set; }
    }
}
