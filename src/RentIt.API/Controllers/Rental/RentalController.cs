﻿using Microsoft.AspNetCore.Mvc;
using RentIt.Application.Commands.Rental;
using RentIt.Application.Services.Interfaces;

namespace RentIt.API.Controllers.Rental;

[ApiController]
[Route("locacoes")]
public class RentalController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalController(IRentalService rentalService) => _rentalService = rentalService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRentalRequest request, CancellationToken cancellationToken)
    {
        var result = await _rentalService.CreateAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { erros = result.Errors });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        // Placeholder para consulta futura
        return Ok();
    }
}
