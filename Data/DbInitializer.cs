using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pop_Simona_SADE;
using Pop_Simona_SADE.Models;

namespace Pop_Simona_SADE.Data
{
    public class DbInitializer
    {
        public static void Initialize(ExhibitionContext context)
        {
            context.Database.EnsureCreated();
            if (context.Paintings.Any())
            {
                return; // BD a fost creata anterior
            }
            var paintings = new Painting[]

            {
             new Painting{Title="Macii",Painter="Stefan Luchian",Price=Decimal.Parse("2500")},
             new Painting{Title="Guernica",Painter="Pablo Picasso",Price=Decimal.Parse("8750")},
             new Painting{Title="Autoportret",Painter="Nicolae Grigorescu",Price=Decimal.Parse("9850")}
            };
            foreach (Painting b in paintings)
            {
                context.Paintings.Add(b);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {

            new Customer{CustomerID=1050,Name="Popescu Marcela",BirthDate=DateTime.Parse("1979-09-01")},
            new Customer{CustomerID=1045,Name="Mihailescu Cornel",BirthDate=DateTime.Parse("1969-07-08")},

            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();


            var orders = new Order[]
            {
             new Order{PaintingID=1,CustomerID=1050,OrderDate=DateTime.Parse("02-25-2020")},
             new Order{PaintingID=3,CustomerID=1045,OrderDate=DateTime.Parse("09-28-2020")},
             new Order{PaintingID=1,CustomerID=1045,OrderDate=DateTime.Parse("10-28-2020")},
             new Order{PaintingID=2,CustomerID=1050, OrderDate=DateTime.Parse("09-28-2020")},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();


            var currents = new Current[]
            {
            new Current { CurrentName = "Impresionism", Particularity= "Evidențiază lumina, culoarea și lovitura pensulei." },
            new Current { CurrentName = "Suprarealism", Particularity = "Pune accentul pe activitatea spontană a imaginației." },
            new Current { CurrentName = "Realism", Particularity = "Critică mediul și individul." },
            };
            foreach (Current p in currents)
            {
                context.Currents.Add(p);
            }
            context.SaveChanges();
            var currentpaintings = new CurrentPainting[]
            {
            new CurrentPainting {
            PaintingID = paintings.Single(c => c.Title == "Macii" ).ID,
            CurrentID = currents.Single(i => i.CurrentName == "Impresionism").ID
            },
            new CurrentPainting {
            PaintingID = paintings.Single(c => c.Title == "Guernica" ).ID,
            CurrentID = currents.Single(i => i.CurrentName == "Suprarealism").ID
            },
            new CurrentPainting {
            PaintingID = paintings.Single(c => c.Title == "Autoportret" ).ID,
            CurrentID = currents.Single(i => i.CurrentName == "Realism").ID
            }
            };
            foreach (CurrentPainting pb in currentpaintings)
            {
                context.CurrentPaintings.Add(pb);
            }
            context.SaveChanges();
        }
    }
}
