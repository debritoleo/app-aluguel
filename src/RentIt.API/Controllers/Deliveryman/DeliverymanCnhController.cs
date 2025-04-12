using Microsoft.AspNetCore.Mvc;
using RentIt.Application.Services.Interfaces;

namespace RentIt.API.Controllers.Deliveryman;

[ApiController]
[Route("entregadores/{id:guid}/cnh")]
public class DeliverymanCnhController : ControllerBase
{
    private readonly ICnhStorageService _storageService;

    public DeliverymanCnhController(ICnhStorageService storageService) => _storageService = storageService;

    [HttpPost]
    public async Task<IActionResult> Upload(Guid id, IFormFile arquivo, CancellationToken cancellationToken)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest(new { erros = new[] { "Arquivo é obrigatório." } });

        try
        {
            var fileName = await _storageService.SaveAsync(id, arquivo, cancellationToken);
            return Ok(new { arquivo = fileName });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { erros = new[] { ex.Message } });
        }
    }
}
