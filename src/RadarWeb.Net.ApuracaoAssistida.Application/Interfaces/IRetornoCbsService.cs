using RadarWeb.Net.ApuracaoAssistida.Application.DTOs;

namespace RadarWeb.Net.ApuracaoAssistida.Application.Interfaces;

public interface IRetornoCbsService
{
    Task ProcessarWebhookAsync(
        string tiqueteSolicitacao,
        string urlAssinada,
        DateTime urlAssinadaExpiraEm,
        CancellationToken cancellationToken = default);
}
