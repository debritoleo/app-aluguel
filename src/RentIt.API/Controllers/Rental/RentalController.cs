﻿using Microsoft.AspNetCore.Mvc;
using RentIt.Application.Commands.Rental;
using RentIt.Application.Queries.Rental;
using RentIt.Application.Requests.Rental;
using RentIt.Application.Services.Interfaces;

namespace RentIt.API.Controllers.Rental;

[ApiController]
[Route("locacoes")]
public class RentalController : ControllerBase
{
    private readonly IRentalService _rentalService;
    private readonly IRentalQueries _queries;

    public RentalController(IRentalService rentalService, IRentalQueries queries)
    {
        _rentalService = rentalService;
        _queries = queries;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRentalRequest request, CancellationToken cancellationToken)
    {
        var result = await _rentalService.CreateAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { mensagem = "Dados inválidos" });

        return Created(nameof(GetById), new { id = result.Value });
    }

    [HttpPut("{id}/devolucao")]
    public async Task<IActionResult> Return(string id, [FromBody] ReturnRentalRequest request, CancellationToken cancellationToken)
    {
        var result = await _rentalService.ReturnAsync(id, request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { mensagem = "Dados inválidos" });

        return Ok(new { mensagem = "Data de devolução informada com sucesso" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _queries.GetByIdAsync(id);

        if (result is null)
            return NotFound(new { mensagem = "Locação não encontrada." });

        return Ok(result);
    }
}
