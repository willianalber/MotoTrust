using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoTrust.Application.Commands.DeliveryPerson;
using MotoTrust.Application.DTOs;
using MotoTrust.Application.Queries.DeliveryPerson;

namespace MotoTrust.API.Controllers;

[ApiController]
[Route("/entregadores")]
public class DeliveryPersonController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeliveryPersonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetDeliveryPersons()
    {
        try
        {
            var query = new GetAllDeliveryPersonsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
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

    [HttpGet("{id}/cnh")]
    public async Task<IActionResult> GetCNHImage(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("cnh/{fileName}")]
    public async Task<IActionResult> GetCNHImageByFileName(string fileName)
    {
        throw new NotImplementedException();
    }
}
