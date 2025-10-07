using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoTrust.Application.Commands.Rental;
using MotoTrust.Application.DTOs;
using MotoTrust.Application.Queries.Rental;

namespace MotoTrust.API.Controllers;

[ApiController]
[Route("/locacao")]
public class RentalController : ControllerBase
{
    private readonly IMediator _mediator;

    public RentalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRental([FromBody] CreateRentalRequestDto request)
    {
        try
        {
            var command = new CreateRentalCommand
            {
                EntregadorId = request.EntregadorId,
                MotoId = request.MotoId,
                DataInicio = request.DataInicio,
                DataTermino = request.DataTermino,
                DataPrevisaoTermino = request.DataPrevisaoTermino,
                Plano = request.Plano
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateRental), result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetRentals()
    {
        try
        {
            var query = new GetAllRentalsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRental(Guid id)
    {
        try
        {
            var query = new GetRentalByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound(new ErrorResponseDto { Mensagem = "Locação não encontrada" });
                
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }

    [HttpPut("{id}/devolucao")]
    public async Task<IActionResult> UpdateReturnDate(Guid id, [FromBody] UpdateReturnDateRequestDto request)
    {
        try
        {
            var command = new UpdateReturnDateCommand
            {
                Id = id,
                DataDevolucao = request.DataDevolucao
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }
}