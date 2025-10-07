using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Queries.DeliveryPerson;

public class GetAllDeliveryPersonsQueryHandler : IRequestHandler<GetAllDeliveryPersonsQuery, List<GetDeliveryPersonResponseDto>>
{
    private readonly IDeliveryPersonRepository _deliveryPersonRepository;

    public GetAllDeliveryPersonsQueryHandler(IDeliveryPersonRepository deliveryPersonRepository)
    {
        _deliveryPersonRepository = deliveryPersonRepository;
    }

    public async Task<List<GetDeliveryPersonResponseDto>> Handle(GetAllDeliveryPersonsQuery request, CancellationToken cancellationToken)
    {
        var deliveryPersons = await _deliveryPersonRepository.GetAllAsync();
        
        var result = deliveryPersons
            .Where(d => d.IsActive)
            .Select(d => new GetDeliveryPersonResponseDto
            {
                Id = d.Id,
                Identificador = d.Identificador,
                Nome = d.Nome,
                CNPJ = d.CNPJ,
                DataNascimento = d.DataNascimento,
                NumeroCNH = d.NumeroCNH,
                TipoCNH = d.TipoCNH.ToString(),
                CreatedAt = d.CreatedAt
            })
            .OrderBy(d => d.Nome)
            .ToList();

        return result;
    }
}


