using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OloPizzaExercise.Models
{
   public class TopSellingPizzasViewModel
   {
      public TopSellingPizzasViewModel()
      {
         this.TopSellers = new Dictionary<string, int>();
      }

      public Dictionary<string, int> TopSellers { get; set; }
   }

   public class PizzaViewModel : IComparable<PizzaViewModel>
   {      
      public PizzaViewModel()
      {
         this.Id = new Guid();
         this.Toppings = new List<string>();
      }

      public Guid Id { get;  }

      public List<string> Toppings { get; set; }

      public int CompareTo(PizzaViewModel that)
      {
         if (this.Toppings == null || that.Toppings == null)
         {
            return Convert.ToInt32((this.Toppings == null && that.Toppings == null));
         }

         if (this.Toppings.Count != that.Toppings.Count)
         {
            return 0;
         }

         return Convert.ToInt32(this.Toppings.SequenceEqual(that.Toppings));
      }
   }
}