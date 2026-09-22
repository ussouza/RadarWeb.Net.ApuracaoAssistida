namespace RadarWeb.Net.ApuracaoAssistida.Domain.Entities;

public sealed class Debito
{
    public long Id { get; set; }
    public long SolicitacaoId { get; set; }
    public string Ni { get; set; } = string.Empty;
    public string? NiConsumidor { get; set; }
    public string Pa { get; set; } = string.Empty;
    public int Origem { get; set; }
    public int Documento { get; set; }
    public string Chave { get; set; } = string.Empty;
    public DateTime Emissao { get; set; }
    public DateTime Registro { get; set; }
    public DateTime Atualizacao { get; set; }
    public decimal? CbsExcedente { get; set; }
    public decimal CbsApurado { get; set; }
    public decimal? CbsInexigivel { get; set; }
    public decimal? CbsSuspenso { get; set; }
    public decimal? CbsExtinto { get; set; }
    public decimal CbsSaldoDevedor { get; set; }
    public Solicitacao Solicitacao { get; set; } = null!;
}
