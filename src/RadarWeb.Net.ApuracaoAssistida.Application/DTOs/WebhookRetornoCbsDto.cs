using System.Text.Json.Serialization;

namespace RadarWeb.Net.ApuracaoAssistida.Application.DTOs;

public sealed class WebhookRetornoCbsDto
{
    [JsonPropertyName("tiqueteSolicitacao")]
    public string TiqueteSolicitacao { get; set; } = string.Empty;

    [JsonPropertyName("urlAssinadaExpiraEm")]
    public DateTime UrlAssinadaExpiraEm { get; set; }

    [JsonPropertyName("urlAssinada")]
    public string? UrlAssinada { get; set; }

    [JsonPropertyName("codigoErro")]
    public string? CodigoErro { get; set; }

    [JsonPropertyName("mensagemErro")]
    public string? MensagemErro { get; set; }
}
