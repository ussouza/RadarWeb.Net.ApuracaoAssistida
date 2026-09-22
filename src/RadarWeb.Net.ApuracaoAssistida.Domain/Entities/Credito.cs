namespace RadarWeb.Net.ApuracaoAssistida.Domain.Entities;

public sealed class Credito
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
    public decimal? CbsExcedentes { get; set; }
    public decimal CbsApurado { get; set; }
    public decimal? CbsInapropriavel { get; set; }
    public decimal? CbsSuspenso { get; set; }
    public decimal? CbsPrescrito { get; set; }
    public decimal? CbsAApropriar { get; set; }
    public decimal? CbsApropriado { get; set; }
    public decimal? CbsInutilizavel { get; set; }
    public decimal? CbsUtilizado { get; set; }
    public decimal? CbsRestabelecido { get; set; }
    public decimal? CbsSaldoCredor { get; set; }
    public decimal? CbsPedidoRessarcimento { get; set; }
    public Solicitacao Solicitacao { get; set; } = null!;
}
