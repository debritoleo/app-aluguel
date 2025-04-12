using Microsoft.AspNetCore.Mvc;
using RentIt.Application.Commands.Motorcycle;
using RentIt.Application.Queries.Motorcycle;
using RentIt.Application.Services.Interfaces;
using RentIt.Application.ViewModels;
using RentIt.Application.ViewModels.Motorcycle;

namespace RentIt.API.Controllers.Motorcycle;

[ApiController]
[Route("motos")]
public class MotorcycleController : ControllerBase
{
    private readonly IMotorcycleService _service;
    private readonly IMotorcycleQueries _queries;

    public MotorcycleController(IMotorcycleService service, IMotorcycleQueries queries)
    {
        _service = service;
        _queries = queries;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMotorcycleRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { erros = result.Errors });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MotorcycleViewModel>>> GetAll([FromQuery] string? placa, CancellationToken cancellationToken)
    {
        var result = await _queries.GetAllAsync(placa, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MotorcycleViewModel>> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _queries.GetByIdAsync(id, cancellationToken);

        if (result is null)
            return NotFound(new { erros = new[] { "Moto não encontrada." } });

        return Ok(result);
    }
}
