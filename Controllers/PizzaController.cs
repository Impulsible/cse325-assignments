using Microsoft.AspNetCore.Mvc;

namespace PizzaAPI.Controllers;

[ApiController]
[Route("pizza")]
public class PizzaController : ControllerBase
{
    private static List<Pizza> pizzas = new List<Pizza>
    {
        new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false },
        new Pizza { Id = 2, Name = "Veggie", IsGlutenFree = true },
        new Pizza { Id = 3, Name = "Meat Lovers", IsGlutenFree = false },
        new Pizza { Id = 4, Name = "Hawaiian", IsGlutenFree = false }
    };

    [HttpGet]
    public ActionResult<List<Pizza>> GetAll()
    {
        return Ok(pizzas);
    }

    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = pizzas.FirstOrDefault(p => p.Id == id);
        if (pizza == null)
            return NotFound();
        return Ok(pizza);
    }

    [HttpPost]
    public ActionResult Create(Pizza pizza)
    {
        pizzas.Add(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, Pizza pizza)
    {
        var existingPizza = pizzas.FirstOrDefault(p => p.Id == id);
        if (existingPizza == null)
            return NotFound();
        
        existingPizza.Name = pizza.Name;
        existingPizza.IsGlutenFree = pizza.IsGlutenFree;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var pizza = pizzas.FirstOrDefault(p => p.Id == id);
        if (pizza == null)
            return NotFound();
        
        pizzas.Remove(pizza);
        return NoContent();
    }
}