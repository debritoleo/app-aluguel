using Microsoft.AspNetCore.Mvc;
using RentIt.Application.Commands.Motorcycle;
using RentIt.Application.Services.Interfaces;

namespace RentIt.API.Controllers.Motorcycle;

[ApiController]
[Route("motos")]
public class MotorcycleController : ControllerBase
{
    private readonly IMotorcycleService _service;

    public MotorcycleController(IMotorcycleService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMotorcycleRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { erros = result.Errors });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        // TODO: Implementar consulta por ID
        return Ok();
    }
}