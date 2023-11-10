using Microsoft.AspNetCore.Mvc;
using Models;

namespace Services
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ItemService> _logger;

        public GenericController(IServiceProvider serviceProvider, ILogger<ItemService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string typeName)
        {
            dynamic service = GetServiceForType(typeName);
            var items = await service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery] string typeName, int id)
        {
            dynamic service = GetServiceForType(typeName);
            var item = await service.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] string typeName)
        {
            dynamic service = GetServiceForType(typeName);
            var createdItem = await service.CreateAsync();
            return CreatedAtAction(nameof(Get), new { typeName = typeName, id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromQuery] string typeName, int id, [FromBody] dynamic entity)
        {
            dynamic service = GetServiceForType(typeName);
            if (entity.Id != id)
            {
                return BadRequest();
            }

            await service.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromQuery] string typeName, int id)
        {
            dynamic service = GetServiceForType(typeName);
            await service.DeleteAsync(id);
            return NoContent();
        }

        private dynamic GetServiceForType(string typeName)
        {
            var assemblyContainingModels = typeof(Item).Assembly.FullName;
            var fullyQualifiedTypeName = $"Models.{typeName}, {assemblyContainingModels}";

            var type = Type.GetType(fullyQualifiedTypeName);
            if (type == null)
            {
                throw new ArgumentException($"Type '{fullyQualifiedTypeName}' not found", nameof(typeName));
            }

            var serviceType = typeof(IService<>).MakeGenericType(type);
            dynamic service = _serviceProvider.GetService(serviceType);
            if (service == null)
            {
                throw new InvalidOperationException($"Service for type '{typeName}' not registered.");
            }

            return service;
        }

    }
}
