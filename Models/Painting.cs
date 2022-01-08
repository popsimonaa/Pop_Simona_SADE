using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pop_Simona_SADE.Models
{
    public class Painting
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Painter { get; set; }
        public decimal Price { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<CurrentPainting> CurrentPaintings { get; set; }
    }
}
