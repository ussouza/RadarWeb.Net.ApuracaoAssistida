namespace RadarWeb.Net.ApuracaoAssistida.Domain.Entities;

public sealed class Empresa
{
    public int Id { get; set; }

    public string CnpjBase { get; set; } = string.Empty;

    public string? Nome { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCadastro { get; set; }

    public ICollection<Solicitacao> Solicitacoes { get; set; } = new List<Solicitacao>();
}
