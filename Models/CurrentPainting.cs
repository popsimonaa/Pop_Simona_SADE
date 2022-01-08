using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pop_Simona_SADE.Models
{
    public class CurrentPainting
    {
        public int CurrentID { get; set; }
        public int PaintingID { get; set; }
        public Current Current { get; set; }
        public Painting Painting { get; set; }
    }
}
