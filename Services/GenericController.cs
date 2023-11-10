using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[ApiController]
[Route("[controller]")]
public class GenericController<T> : ControllerBase where T : class, IEntity
{
    private readonly IService<T> _service;

    public GenericController(IService<T> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] T newEntity)
    {
        var createdItem = await _service.CreateAsync(newEntity);
        return CreatedAtAction(nameof(Get), new { id = GetItemId(createdItem) }, createdItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] T entity)
    {
        if (GetItemId(entity.Id) != id)
        {
            return BadRequest();
        }

        await _service.UpdateAsync(entity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    // Helper method to get the Id of the entity.
    // This requires reflection or a common interface that exposes Id.
    private object GetItemId(T entity)
    {
        var propertyInfo = entity.GetType().GetProperty("Id");
        return propertyInfo.GetValue(entity, null);
    }
}
