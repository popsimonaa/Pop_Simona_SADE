using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pop_Simona_SADE.Models
{
    public class Order
    {
         public int OrderID { get; set; }
         public int CustomerID { get; set; }
         public int PaintingID { get; set; }
         public DateTime OrderDate { get; set; }
         public Customer Customer { get; set; }
         public Painting Painting { get; set; }
 
    }
}
