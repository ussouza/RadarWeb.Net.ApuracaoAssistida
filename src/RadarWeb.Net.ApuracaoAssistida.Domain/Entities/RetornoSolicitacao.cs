namespace RadarWeb.Net.ApuracaoAssistida.Domain.Entities;

public sealed class RetornoSolicitacao
{
    public long Id { get; set; }

    public long SolicitacaoId { get; set; }

    public string NomeArquivo { get; set; } = string.Empty;

    public string? ContentType { get; set; }

    public long? TamanhoBytes { get; set; }

    public DateTime DataDownload { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? HashSha256 { get; set; }

    public string? MensagemErro { get; set; }

    public Solicitacao Solicitacao { get; set; } = null!;
}
