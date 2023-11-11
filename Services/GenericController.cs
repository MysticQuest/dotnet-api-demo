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
            try
            {
                dynamic service = GetServiceForType(typeName);
                var items = await service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAll method.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving items.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery] string typeName, int id)
        {
            try
            {
                dynamic service = GetServiceForType(typeName);
                var item = await service.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Get method.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the item.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] string typeName)
        {
            try
            {
                dynamic service = GetServiceForType(typeName);
                var createdItem = await service.CreateAsync();
                if (createdItem == null)
                {
                    _logger.LogWarning("Created item was null for type {TypeName}.", typeName);
                    return NoContent();
                }
                return CreatedAtAction(nameof(Get), new { typeName = typeName, id = createdItem.Id }, createdItem);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "ArgumentException in Create method for type {TypeName}.", typeName);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "InvalidOperationException in Create method for type {TypeName}.", typeName);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in Create method for type {TypeName}.", typeName);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromQuery] string typeName, int id, [FromBody] dynamic entity)
        {
            try
            {
                dynamic service = GetServiceForType(typeName);
                if (entity.Id != id)
                {
                    return BadRequest("The ID in the body does not match the ID in the path.");
                }

                await service.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Update method.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the item.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromQuery] string typeName, int id)
        {
            try
            {
                dynamic service = GetServiceForType(typeName);
                await service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Delete method.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the item.");
            }
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
            var service = _serviceProvider.GetService(serviceType);
            if (service == null)
            {
                throw new InvalidOperationException($"Service for type '{typeName}' not registered.");
            }

            return service;
        }

    }
}
