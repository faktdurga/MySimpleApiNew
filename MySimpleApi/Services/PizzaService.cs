using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySimpleApi.Model;

namespace MySimpleApi.Services
{
    public static class PizzaService
    {
        static List<Pizza> Pizzas { get; }
        static int nextId = 3;
        static PizzaService()
        {
            Pizzas = new List<Pizza>
            {
                new Pizza{ Id=1, Name = "Classic Italian", IsGluntenFree = false },
                new Pizza{Id=2, Name="Veggie", IsGluntenFree = true}
            };


        }

        public static List<Pizza> GetAll() => Pizzas;

        public static Pizza? Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);

        //public static Pizza? GetbyName(string name) => Pizzas.FirstOrDefault(p => p.Name == name);

        public static void Add(Pizza pizza)
        {
            pizza.Id = nextId++;
            Pizzas.Add(pizza);
        }

        public static void Delete(int id)
        {
            var pizza = Get(id);
            if (pizza is null)
                return;

            Pizzas.Remove(pizza);
        }

        public static void Update(Pizza pizza)
        {
            var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
            if (index == -1)
                return;

            Pizzas[index] = pizza;
        }

    }
}
