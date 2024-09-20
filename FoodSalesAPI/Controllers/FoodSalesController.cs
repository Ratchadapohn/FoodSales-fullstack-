using FoodSalesAPI.Models;
using FoodSalesAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FoodSalesController : ControllerBase
{
    private readonly FoodSalesService _service;

    public FoodSalesController(FoodSalesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<FoodSales>>> GetAll() =>
        await _service.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<FoodSales>> GetById(string id)
    {
        var foodSale = await _service.GetByIdAsync(id);
        if (foodSale == null) return NotFound();
        return foodSale;
    }

    [HttpPost]
    public async Task<IActionResult> Create(FoodSales foodSale)
    {
        await _service.CreateAsync(foodSale);
        return CreatedAtAction(nameof(GetById), new { id = foodSale.Id }, foodSale);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, FoodSales foodSale)
    {
        var existingSale = await _service.GetByIdAsync(id);
        if (existingSale == null) return NotFound();
        await _service.UpdateAsync(id, foodSale);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var foodSale = await _service.GetByIdAsync(id);
        if (foodSale == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
