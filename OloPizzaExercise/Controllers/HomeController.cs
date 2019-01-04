using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using OloPizzaExercise.Models;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OloPizzaExercise.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
         Dictionary<string, int> toppingsAndCount = new Dictionary<string, int>();
         List<PizzaViewModel> pizzas = new List<PizzaViewModel>();

         using (var httpClient = new HttpClient())
         {                       
            JavaScriptSerializer js = new JavaScriptSerializer();

            var json = await httpClient.GetStringAsync("http://files.olo.com/pizzas.json");

            JArray a = JArray.Parse(json);

            foreach (JObject o in a.Children<JObject>())
            {
               foreach (JProperty p in o.Properties())
               {
                  PizzaViewModel pizzaModel = new PizzaViewModel();

                  pizzaModel.Toppings = p.Value.ToObject<List<string>>();

                  pizzas.Add(pizzaModel);
               }
            }            
         }

         Dictionary<string, int> toppingCombos = new Dictionary<string, int>();

         foreach (PizzaViewModel pizza in pizzas)
         {
            pizza.Toppings.Sort();

            string toppings = String.Join(",", pizza.Toppings.ToArray());

            if (!toppingCombos.Keys.Contains(toppings))
            {
               toppingCombos.Add(toppings, 1);
            }
            else
            {
               toppingCombos[toppings]++;
            }
         }

         var results = toppingCombos.ToList();

         results.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

         results.Reverse();

         TopSellingPizzasViewModel topSellers = new TopSellingPizzasViewModel();

         topSellers.TopSellers = results.Take(20).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value); 
         
         return View(topSellers);
        }
    }
}