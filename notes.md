# W01 Assignment Notes

## Part 1: Web API Evidence

### Additional Pizza Record Added:
- Id: 4, Name: "Hawaiian", IsGlutenFree: false

### Existing Records (from module):
- Id: 1, Name: "Classic Italian", IsGlutenFree: false
- Id: 2, Name: "Veggie", IsGlutenFree: true
- Id: 3, Name: "Meat Lovers", IsGlutenFree: false

### CRUD Testing Evidence:

**GET All Pizzas — 200 OK**
```
Request: GET http://localhost:5203/pizza
Response: [{"id":1,"name":"Classic Italian","isGlutenFree":false},{"id":2,"name":"Veggie","isGlutenFree":true},{"id":3,"name":"Meat Lovers","isGlutenFree":false},{"id":4,"name":"Hawaiian","isGlutenFree":false}]
```

**GET Single Pizza (id=4) — 200 OK**
```
Request: GET http://localhost:5203/pizza/4
Response: {"id":4,"name":"Hawaiian","isGlutenFree":false}
```

**POST New Pizza — 201 Created**
```
Request: POST http://localhost:5203/pizza
Body: {"id":5,"name":"BBQ Chicken","isGlutenFree":false}
Response: {"id":5,"name":"BBQ Chicken","isGlutenFree":false}
```

**PUT Update Pizza (id=4) — 204 No Content**
```
Request: PUT http://localhost:5203/pizza/4
Body: {"id":4,"name":"Hawaiian Supreme","isGlutenFree":false}
Response: (no content — 204 No Content)
```

**DELETE Pizza (id=5) — 204 No Content**
```
Request: DELETE http://localhost:5203/pizza/5
Response: (no content — 204 No Content)
```

**Final GET All (verification) — 200 OK**
```
Response: [{"id":1,"name":"Classic Italian","isGlutenFree":false},{"id":2,"name":"Veggie","isGlutenFree":true},{"id":3,"name":"Meat Lovers","isGlutenFree":false},{"id":4,"name":"Hawaiian Supreme","isGlutenFree":false}]
```

---

## Part 2: Sales Summary Function

### Function Code:
```csharp
static void GenerateSalesSummary(string directoryPath, string outputFileName)
{
    var report = new StringBuilder();
    decimal totalSales = 0;
    
    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");
    
    string[] files = Directory.GetFiles(directoryPath);
    var fileSales = new List<(string fileName, decimal sales)>();
    
    foreach (string file in files)
    {
        string content = File.ReadAllText(file);
        if (decimal.TryParse(content.Trim(), out decimal salesAmount))
        {
            totalSales += salesAmount;
            string fileName = Path.GetFileName(file);
            fileSales.Add((fileName, salesAmount));
        }
    }
    
    report.AppendLine($" Total Sales: {totalSales:C}");
    report.AppendLine();
    report.AppendLine(" Details:");
    
    foreach (var (fileName, sales) in fileSales)
    {
        report.AppendLine($"  {fileName}: {sales:C}");
    }
    
    File.WriteAllText(outputFileName, report.ToString());
}
```

### Function Call:
```csharp
GenerateSalesSummary("SalesFiles", "SalesSummary.txt");
```

### Output (SalesSummary.txt):
```
Sales Summary
----------------------------
 Total Sales: $16,014.58

 Details:
  store1.txt: $1,234.56
  store2.txt: $5,678.90
  store3.txt: $9,101.12
```
```

---

## Pizza.cs

```csharp
namespace PizzaAPI;

public class Pizza
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsGlutenFree { get; set; }
}
```

---

## Controllers/PizzaController.cs

```csharp
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
```

---

## SalesReport/Program.cs

```csharp
using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        string directoryPath = "SalesFiles";
        string outputFile = "SalesSummary.txt";
        
        GenerateSalesSummary(directoryPath, outputFile);
        Console.WriteLine("Sales summary generated. Check SalesSummary.txt");
    }

    static void GenerateSalesSummary(string directoryPath, string outputFileName)
    {
        var report = new StringBuilder();
        decimal totalSales = 0;
        
        report.AppendLine("Sales Summary");
        report.AppendLine("----------------------------");
        
        string[] files = Directory.GetFiles(directoryPath);
        var fileSales = new List<(string fileName, decimal sales)>();
        
        foreach (string file in files)
        {
            string content = File.ReadAllText(file);
            if (decimal.TryParse(content.Trim(), out decimal salesAmount))
            {
                totalSales += salesAmount;
                string fileName = Path.GetFileName(file);
                fileSales.Add((fileName, salesAmount));
            }
        }
        
        report.AppendLine($" Total Sales: {totalSales:C}");
        report.AppendLine();
        report.AppendLine(" Details:");
        
        foreach (var (fileName, sales) in fileSales)
        {
            report.AppendLine($"  {fileName}: {sales:C}");
        }
        
        File.WriteAllText(outputFileName, report.ToString());
    }
}
```

---