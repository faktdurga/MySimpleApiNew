using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySimpleApi.Model;
using MySimpleApi.Services;

namespace MySimpleApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PizzaController : ControllerBase
    {

        [HttpGet]
        public ActionResult<List<Pizza>> GetAll() =>
        PizzaService.GetAll();

        [HttpGet("{id:int}")]
        public ActionResult<Pizza> Get(int id)
        {
            var pizza = PizzaService.Get(id);
            if(pizza == null)
            return NotFound();

            return pizza;
        }

        //[HttpGet("{name:string}")]
        //public ActionResult<Pizza> Get(string name)
        //{
        //    var pizza = PizzaService.GetbyName(name);
        //    if (pizza == null)
        //        return NotFound();

        //    return pizza;
        //}

        [HttpPost]
        public IActionResult Create(Pizza pizza)
        {
            PizzaService.Add(pizza);
            return CreatedAtAction(nameof(Create), new { id = pizza.Id }, pizza);
        }

        //[HttpPost("{id}")]
        //public IActionResult Update(int id, Pizza pizza)
        //{
        //    if (id != pizza.Id)
        //        return BadRequest();

        //    var existingPizza = PizzaService.Get(id);
        //    if (existingPizza is null)
        //        return NotFound();

        //    PizzaService.Update(pizza);

        //    return NoContent();
        //}

        //[HttpPost("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var pizza = PizzaService.Get(id);

        //    if (pizza is null)
        //        return NotFound();

        //    PizzaService.Delete(id);

        //    return NoContent();
        //}
    }
}
