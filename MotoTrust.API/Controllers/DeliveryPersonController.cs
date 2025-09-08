using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoTrust.Application.Commands.DeliveryPerson;
using MotoTrust.Application.DTOs;
using MotoTrust.Application.Interfaces;

namespace MotoTrust.API.Controllers;

[ApiController]
[Route("/entregadores")]
public class DeliveryPersonController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IImageStorageService _imageStorageService;

    public DeliveryPersonController(IMediator mediator, IImageStorageService imageStorageService)
    {
        _mediator = mediator;
        _imageStorageService = imageStorageService;
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
        try
        {
            // Aqui você precisaria buscar o entregador pelo ID e obter o nome do arquivo
            // Por simplicidade, vou criar um endpoint que recebe o nome do arquivo diretamente
            return BadRequest(new ErrorResponseDto { Mensagem = "Endpoint em desenvolvimento" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }

    [HttpGet("cnh/{fileName}")]
    public async Task<IActionResult> GetCNHImageByFileName(string fileName)
    {
        try
        {
            var imageBytes = await _imageStorageService.GetImageAsync(fileName);
            return File(imageBytes, "image/png", fileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound(new ErrorResponseDto { Mensagem = "Imagem não encontrada" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto { Mensagem = ex.Message });
        }
    }
}
