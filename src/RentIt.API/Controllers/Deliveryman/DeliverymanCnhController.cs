using Microsoft.AspNetCore.Mvc;
using RentIt.Application.Requests.Deliveryman;
using RentIt.Application.Services.Interfaces;

namespace RentIt.API.Controllers.Deliveryman;

[ApiController]
[Route("entregadores/{id}/cnh")]
public class DeliverymanCnhController : ControllerBase
{
    private readonly ICnhStorageService _storageService;

    public DeliverymanCnhController(ICnhStorageService storageService) => _storageService = storageService;

    [HttpPost]
    public async Task<IActionResult> Upload(Guid id, [FromBody] UploadCnhRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CnhBase64))
            return BadRequest(new { mensagem = "Dados inválidos" });

        _ = await _storageService.SaveBase64Async(id, request.CnhBase64, cancellationToken);

        return Created();
    }
}
