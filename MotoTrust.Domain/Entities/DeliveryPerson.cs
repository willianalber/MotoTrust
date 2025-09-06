using MotoTrust.Domain.Common;
using MotoTrust.Domain.Enums;

namespace MotoTrust.Domain.Entities;

public class DeliveryPerson : EntityBase
{
    public string Identificador { get; private set; }
    public string Nome { get; private set; }
    public string CNPJ { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string NumeroCNH { get; private set; }
    public LicenseType TipoCNH { get; private set; }
    public string? ImagemCNH { get; private set; }

    private DeliveryPerson() { } // EF Core

    public DeliveryPerson(string identificador, string nome, string cnpj, DateTime dataNascimento, 
                     string numeroCNH, LicenseType tipoCNH, string? imagemCNH = null)
    {
        // Validações básicas
        Identificador = identificador ?? throw new ArgumentNullException(nameof(identificador));
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        CNPJ = cnpj ?? throw new ArgumentNullException(nameof(cnpj));
        DataNascimento = dataNascimento;
        NumeroCNH = numeroCNH ?? throw new ArgumentNullException(nameof(numeroCNH));
        TipoCNH = tipoCNH;
        ImagemCNH = imagemCNH;
    }

    public void UpdateCNHImage(string imagemCNH)
    {
        if (string.IsNullOrWhiteSpace(imagemCNH))
            throw new ArgumentException("Imagem da CNH não pode ser vazia", nameof(imagemCNH));

        ImagemCNH = imagemCNH;
        Update();
    }
}
