namespace RadarWeb.Net.ApuracaoAssistida.Domain.Entities;

public sealed class Pagamento
{
    public long Id { get; set; }
    public long SolicitacaoId { get; set; }
    public string Ni { get; set; } = string.Empty;
    public string? NiConsumidor { get; set; }
    public DateTime DataArrecadacao { get; set; }
    public string NumeroDarf { get; set; }
    public int Tipo { get; set; }
    public string NiAdquirente { get; set; }
    public int Sequencial { get; set; }
    public string Pa { get; set; } = string.Empty;
    public DateTime Vencimento { get; set; }
    public string NiContribuinte { get; set; }
    public string ChaveDfe { get; set; }
    public decimal Principal { get; set; }
    public decimal Multa { get; set; }
    public decimal Juros { get; set; }
    public decimal Total { get; set; }
    public Solicitacao Solicitacao { get; set; } = null!;
}
