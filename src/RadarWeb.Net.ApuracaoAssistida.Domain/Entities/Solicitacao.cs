namespace RadarWeb.Net.ApuracaoAssistida.Domain.Entities;

public sealed class Solicitacao
{
    public long Id { get; set; }

    public int EmpresaId { get; set; }

    public string TipoConsulta { get; set; } = string.Empty;

    public string TiqueteSolicitacao { get; set; } = string.Empty;

    public DateTime DataSolicitacao { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime? DataConclusao { get; set; }

    public DateTime? DataUltimaNotificacao { get; set; }

    public int Tentativas { get; set; }

    public string? CodigoErro { get; set; }

    public string? MensagemErro { get; set; }

    public DateTime? DataProcessamento { get; set; }

    public Empresa Empresa { get; set; } = null!;
}
