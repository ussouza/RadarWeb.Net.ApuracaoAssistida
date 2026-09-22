using System.Text.Json.Serialization;

namespace RadarWeb.Net.ApuracaoAssistida.Application.DTOs;

public sealed class RetornoCbsDto
{
    [JsonPropertyName("tiqueteSolicitacao")]
    public string TiqueteSolicitacao { get; set; } = string.Empty;

    [JsonPropertyName("ni")]
    public string Ni { get; set; } = string.Empty;

    [JsonPropertyName("niConsumidor")]
    public string? NiConsumidor { get; set; }

    [JsonPropertyName("geradoEm")]
    public DateTime GeradoEm { get; set; }

    [JsonPropertyName("apuracao")]
    public List<ApuracaoDebitoCreditoDto> Apuracao { get; set; } = [];
}

public sealed class ApuracaoDebitoCreditoDto
{
    [JsonPropertyName("pa")]
    public string Pa { get; set; } = string.Empty;

    [JsonPropertyName("debitos")]
    public List<DebitoRetornoDto>? Debitos { get; set; }

    [JsonPropertyName("creditos")]
    public List<CreditoRetornoDto>? Creditos { get; set; }
}

public sealed class DebitoRetornoDto
{
    [JsonPropertyName("origem")]
    public int Origem { get; set; }

    [JsonPropertyName("documento")]
    public int Documento { get; set; }

    [JsonPropertyName("chave")]
    public string Chave { get; set; } = string.Empty;

    [JsonPropertyName("emissao")]
    public DateTime Emissao { get; set; }

    [JsonPropertyName("registro")]
    public DateTime Registro { get; set; }

    [JsonPropertyName("atualizacao")]
    public DateTime Atualizacao { get; set; }

    [JsonPropertyName("cbs")]
    public CbsDebitoDto Cbs { get; set; } = new();
}

public sealed class CbsDebitoDto
{
    [JsonPropertyName("apurado")]
    public decimal Apurado { get; set; }

    [JsonPropertyName("excedente")]
    public decimal? Excedente { get; set; }

    [JsonPropertyName("inexigivel")]
    public decimal? Inexigivel { get; set; }

    [JsonPropertyName("suspenso")]
    public decimal? Suspenso { get; set; }

    [JsonPropertyName("extinto")]
    public decimal? Extinto { get; set; }

    [JsonPropertyName("saldoDevedor")]
    public decimal SaldoDevedor { get; set; }
}

public sealed class CreditoRetornoDto
{
    [JsonPropertyName("origem")]
    public int Origem { get; set; }

    [JsonPropertyName("documento")]
    public int Documento { get; set; }

    [JsonPropertyName("chave")]
    public string Chave { get; set; } = string.Empty;

    [JsonPropertyName("emissao")]
    public DateTime Emissao { get; set; }

    [JsonPropertyName("registro")]
    public DateTime Registro { get; set; }

    [JsonPropertyName("atualizacao")]
    public DateTime Atualizacao { get; set; }

    [JsonPropertyName("cbs")]
    public CbsCreditoDto Cbs { get; set; } = new();
}

public sealed class CbsCreditoDto
{
    [JsonPropertyName("apurado")]
    public decimal Apurado { get; set; }

    [JsonPropertyName("excedentes")]
    public decimal? Excedentes { get; set; }

    [JsonPropertyName("apropriacao")]
    public ApropriacaoCreditoDto Apropriacao { get; set; } = new();
}

public sealed class ApropriacaoCreditoDto
{
    [JsonPropertyName("inapropriavel")]
    public decimal? Inapropriavel { get; set; }

    [JsonPropertyName("suspenso")]
    public decimal? Suspenso { get; set; }

    [JsonPropertyName("prescrito")]
    public decimal? Prescrito { get; set; }

    [JsonPropertyName("aApropriar")]
    public decimal? AApropriar { get; set; }

    [JsonPropertyName("apropriado")]
    public decimal? Apropriado { get; set; }

    [JsonPropertyName("utilizacao")]
    public UtilizacaoCreditoDto Utilizacao { get; set; } = new();
}

public sealed class UtilizacaoCreditoDto
{
    [JsonPropertyName("inutilizavel")]
    public decimal? Inutilizavel { get; set; }

    [JsonPropertyName("utilizado")]
    public decimal? Utilizado { get; set; }

    [JsonPropertyName("restabelecido")]
    public decimal? Restabelecido { get; set; }

    [JsonPropertyName("naoUtilizado")]
    public NaoUtilizadoCreditoDto NaoUtilizado { get; set; } = new();
}

public sealed class NaoUtilizadoCreditoDto
{
    [JsonPropertyName("saldoCredor")]
    public decimal? SaldoCredor { get; set; }

    [JsonPropertyName("pedidoRessarcimento")]
    public decimal? PedidoRessarcimento { get; set; }
}

public sealed class RetornoPagamentosDto
{
    [JsonPropertyName("tiqueteSolicitacao")]
    public string TiqueteSolicitacao { get; set; } = string.Empty;

    [JsonPropertyName("ni")]
    public string Ni { get; set; } = string.Empty;

    [JsonPropertyName("niConsumidor")]
    public string? NiConsumidor { get; set; }

    [JsonPropertyName("geradoEm")]
    public DateTime GeradoEm { get; set; }

    [JsonPropertyName("apuracao")]
    public List<ApuracaoPagamentosDto> Apuracao { get; set; } = [];
}

public sealed class ApuracaoPagamentosDto
{
    [JsonPropertyName("dataArrecadacao")]
    public DateTime DataArrecadacao { get; set; }

    [JsonPropertyName("pagamentos")]
    public List<PagamentoRetornoDto> Pagamentos { get; set; } = [];
}

public sealed class PagamentoRetornoDto
{
    [JsonPropertyName("numeroDARF")]
    public string NumeroDarf { get; set; } = string.Empty;

    [JsonPropertyName("tipo")]
    public int Tipo { get; set; }

    [JsonPropertyName("niAdquirente")]
    public string? NiAdquirente { get; set; }

    [JsonPropertyName("composicao")]
    public List<ComposicaoPagamentoDto> Composicao { get; set; } = [];
}

public sealed class ComposicaoPagamentoDto
{
    [JsonPropertyName("sequencial")]
    public int Sequencial { get; set; }

    [JsonPropertyName("pa")]
    public string Pa { get; set; } = string.Empty;

    [JsonPropertyName("vencimento")]
    public DateTime Vencimento { get; set; }

    [JsonPropertyName("ni_contribuinte")]
    public string? NiContribuinte { get; set; }

    [JsonPropertyName("chaveDFE")]
    public string? ChaveDfe { get; set; }

    [JsonPropertyName("principal")]
    public decimal Principal { get; set; }

    [JsonPropertyName("multa")]
    public decimal Multa { get; set; }

    [JsonPropertyName("juros")]
    public decimal Juros { get; set; }

    [JsonPropertyName("total")]
    public decimal Total { get; set; }
}

public sealed class RetornoRecolhimentosDto
{
    [JsonPropertyName("tiqueteSolicitacao")]
    public string TiqueteSolicitacao { get; set; } = string.Empty;

    [JsonPropertyName("ni")]
    public string Ni { get; set; } = string.Empty;

    [JsonPropertyName("niConsumidor")]
    public string? NiConsumidor { get; set; }

    [JsonPropertyName("geradoEm")]
    public DateTime GeradoEm { get; set; }

    [JsonPropertyName("apuracao")]
    public List<ApuracaoRecolhimentosDto> Apuracao { get; set; } = [];
}

public sealed class ApuracaoRecolhimentosDto
{
    [JsonPropertyName("dataArrecadacao")]
    public DateTime DataArrecadacao { get; set; }

    [JsonPropertyName("pagamentos")]
    public List<RecolhimentoRetornoDto> Pagamentos { get; set; } = [];
}

public sealed class RecolhimentoRetornoDto
{
    [JsonPropertyName("numeroDARF")]
    public string NumeroDarf { get; set; } = string.Empty;

    [JsonPropertyName("tipo")]
    public int Tipo { get; set; }

    [JsonPropertyName("composicao")]
    public List<ComposicaoRecolhimentoDto> Composicao { get; set; } = [];
}

public sealed class ComposicaoRecolhimentoDto
{
    [JsonPropertyName("sequencial")]
    public int Sequencial { get; set; }

    [JsonPropertyName("pa")]
    public string Pa { get; set; } = string.Empty;

    [JsonPropertyName("vencimento")]
    public DateTime Vencimento { get; set; }

    [JsonPropertyName("niFornecedor")]
    public string? NiFornecedor { get; set; }

    [JsonPropertyName("chaveDFE")]
    public string? ChaveDfe { get; set; }

    [JsonPropertyName("principal")]
    public decimal Principal { get; set; }

    [JsonPropertyName("multa")]
    public decimal Multa { get; set; }

    [JsonPropertyName("juros")]
    public decimal Juros { get; set; }

    [JsonPropertyName("total")]
    public decimal Total { get; set; }
}
