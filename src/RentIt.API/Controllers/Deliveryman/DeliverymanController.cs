using Microsoft.AspNetCore.Mvc;
using RentIt.Application.Commands.Deliveryman;
using RentIt.Application.Services.Interfaces;

namespace RentIt.API.Controllers.Deliveryman;

[ApiController]
[Route("entregadores")]
public class DeliverymanController : ControllerBase
{
    private readonly IDeliverymanService _service;

    public DeliverymanController(IDeliverymanService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDeliverymanRequest request, CancellationToken cancellationToken)
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