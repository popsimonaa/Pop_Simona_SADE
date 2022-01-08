using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pop_Simona_SADE.Models.ExhibitionViewModels
{
    public class CurrentIndexData
    {
        public IEnumerable<Current> Currents { get; set; }
        public IEnumerable<Painting> Paintings { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
