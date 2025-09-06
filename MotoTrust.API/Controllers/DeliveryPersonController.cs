using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoTrust.Application.Commands.DeliveryPerson;
using MotoTrust.Application.DTOs;

namespace MotoTrust.API.Controllers;

[ApiController]
[Route("api/entregadores")]
public class DeliveryPersonController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeliveryPersonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDeliveryPerson([FromBody] CreateDeliveryPersonRequestDto request)
    {
        try
        {
            var command = new CreateDeliveryPersonCommand
            {
                Identificador = request.Identificador,
                Nome = request.Nome,
                CNPJ = request.CNPJ,
                DataNascimento = request.DataNascimento,
                NumeroCNH = request.NumeroCNH,
                TipoCNH = request.TipoCNH,
                ImagemCNH = request.ImagemCNH
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateDeliveryPerson), result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }

    [HttpPost("{id}/cnh")]
    public async Task<IActionResult> UpdateCNH(Guid id, [FromBody] UpdateCNHRequestDto request)
    {
        try
        {
            var command = new UpdateCNHCommand
            {
                Id = id,
                ImagemCNH = request.ImagemCNH
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(UpdateCNH), result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }
}
