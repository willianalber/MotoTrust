using MotoTrust.Domain.Common;

namespace MotoTrust.Domain.Entities;

public class MotorcycleNotification : EntityBase
{
    public Guid MotorcycleId { get; private set; }
    public string Identificador { get; private set; }
    public int Ano { get; private set; }
    public string Modelo { get; private set; }
    public string Placa { get; private set; }
    public string TipoNotificacao { get; private set; }
    public string Mensagem { get; private set; }
    public DateTime DataEvento { get; private set; }
    public DateTime ProcessadoEm { get; private set; }

    private MotorcycleNotification() { }

    public MotorcycleNotification(Guid motorcycleId, string identificador, int ano, string modelo, 
                                string placa, string tipoNotificacao, string mensagem, DateTime dataEvento)
    {
        MotorcycleId = motorcycleId;
        Identificador = identificador;
        Ano = ano;
        Modelo = modelo;
        Placa = placa;
        TipoNotificacao = tipoNotificacao;
        Mensagem = mensagem;
        DataEvento = dataEvento;
        ProcessadoEm = DateTime.UtcNow;
    }
}
