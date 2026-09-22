using Microsoft.AspNetCore.Mvc;
using RadarWeb.Net.ApuracaoAssistida.Application.DTOs;
using RadarWeb.Net.ApuracaoAssistida.Application.Interfaces;

namespace RadarWeb.Net.ApuracaoAssistida.Api.Controllers;

[ApiController]
[Route("api/webhooks/apuracao-cbs")]
public sealed class ApuracaoCbsWebhookController(IRetornoCbsService retornoService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] WebhookRetornoCbsDto payload,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(payload.TiqueteSolicitacao))
            return BadRequest("tiqueteSolicitacao é obrigatório.");

        if (!string.IsNullOrWhiteSpace(payload.CodigoErro))
            return Ok();

        if (string.IsNullOrWhiteSpace(payload.UrlAssinada))
            return BadRequest("urlAssinada é obrigatória para retorno concluído.");

        await retornoService.ProcessarWebhookAsync(
            payload.TiqueteSolicitacao,
            payload.UrlAssinada,
            payload.UrlAssinadaExpiraEm,
            cancellationToken);

        return Ok();
    }
}
