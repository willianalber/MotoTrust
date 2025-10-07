using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoTrust.Application.Commands.Motorcycle;
using MotoTrust.Application.DTOs;
using MotoTrust.Application.Queries.Motorcycle;

namespace MotoTrust.API.Controllers;

[ApiController]
[Route("/motos")]
public class MotorcycleController : ControllerBase
{
    private readonly IMediator _mediator;

    public MotorcycleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMotorcycle([FromBody] CreateMotorcycleRequestDto request)
    {
        try
        {
            var command = new CreateMotorcycleCommand
            {
                Identificador = request.Identificador,
                Ano = request.Ano,
                Modelo = request.Modelo,
                Placa = request.Placa,
                Marca = request.Marca,
                Cor = request.Cor,
                CapacidadeMotor = request.CapacidadeMotor,
                ValorDiaria = request.ValorDiaria
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMotorcycle), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMotorcycle(Guid id)
    {
        try
        {
            var query = new GetMotorcycleByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound(new ErrorResponseDto { Mensagem = "Moto não encontrada" });
                
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetMotorcycles([FromQuery] string? placa)
    {
        try
        {
            if (string.IsNullOrEmpty(placa))
            {
                var allQuery = new GetAllMotorcyclesQuery();
                var allResult = await _mediator.Send(allQuery);
                return Ok(allResult);
            }
            else
            {
                var query = new GetMotorcyclesByPlateQuery(placa);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }

    [HttpPut("{id}/placa")]
    public async Task<IActionResult> UpdateMotorcyclePlate(Guid id, [FromBody] UpdateMotorcyclePlateRequestDto request)
    {
        try
        {
            var command = new UpdateMotorcyclePlateCommand
            {
                Id = id,
                Placa = request.Placa
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMotorcycle(Guid id)
    {
        try
        {
            var command = new DeleteMotorcycleCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }
}
