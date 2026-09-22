using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RadarWeb.Net.ApuracaoAssistida.Application.DTOs;
using RadarWeb.Net.ApuracaoAssistida.Application.Interfaces;
using RadarWeb.Net.ApuracaoAssistida.Domain.Entities;
using RadarWeb.Net.ApuracaoAssistida.Infrastructure.Data;

namespace RadarWeb.Net.ApuracaoAssistida.Infrastructure.Services;

public sealed class RetornoCbsService(
    ApuracaoAssistidaDbContext db,
    IHttpClientFactory httpClientFactory) : IRetornoCbsService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task ProcessarWebhookAsync(
        string tiqueteSolicitacao,
        string urlAssinada,
        DateTime urlAssinadaExpiraEm,
        CancellationToken cancellationToken = default)
    {
        if (urlAssinadaExpiraEm <= DateTime.UtcNow)
            throw new InvalidOperationException("A urlAssinada recebida já está expirada.");

        var solicitacao = await db.Solicitacoes
            .SingleOrDefaultAsync(x => x.TiqueteSolicitacao == tiqueteSolicitacao, cancellationToken);

        if (solicitacao is null)
            throw new InvalidOperationException($"Solicitação não encontrada para o tíquete '{tiqueteSolicitacao}'.");

        using var client = httpClientFactory.CreateClient("ReceitaCbs");
        using var response = await client.GetAsync(urlAssinada, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

        var jaProcessado = await db.RetornosSolicitacao
            .AnyAsync(x => x.SolicitacaoId == solicitacao.Id && x.HashSha256 == hash, cancellationToken);

        if (jaProcessado)
            return;

        await using var stream = new MemoryStream(bytes);

        switch (NormalizarTipo(solicitacao.TipoConsulta))
        {
            case "DEBITO":
                await ProcessarDebitosAsync(stream, solicitacao, cancellationToken);
                break;

            case "CREDITO":
                await ProcessarCreditosAsync(stream, solicitacao, cancellationToken);
                break;

            case "PAGAMENTO":
                await ProcessarPagamentosAsync(stream, solicitacao, cancellationToken);
                break;

            case "RECOLHIMENTO":
                await ProcessarRecolhimentosAsync(stream, solicitacao, cancellationToken);
                break;

            default:
                throw new InvalidOperationException(
                    $"Tipo de consulta '{solicitacao.TipoConsulta}' não possui processador configurado.");
        }

        db.RetornosSolicitacao.Add(new RetornoSolicitacao
        {
            SolicitacaoId = solicitacao.Id,
            NomeArquivo = $"retorno-{tiqueteSolicitacao}.json",
            ContentType = "application/json",
            TamanhoBytes = bytes.LongLength,
            DataDownload = DateTime.UtcNow,
            Status = "PROCESSADO",
            HashSha256 = hash
        });

        solicitacao.Status = "CONCLUIDA";
        solicitacao.DataConclusao = DateTime.UtcNow;
        solicitacao.DataProcessamento = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);
    }

    private async Task ProcessarDebitosAsync(
        Stream stream,
        Solicitacao solicitacao,
        CancellationToken cancellationToken)
    {
        var retorno = await JsonSerializer.DeserializeAsync<RetornoCbsDto>(stream, JsonOptions, cancellationToken)
            ?? throw new InvalidOperationException("Arquivo de débitos vazio ou inválido.");

        ValidarTiquete(retorno.TiqueteSolicitacao, solicitacao.TiqueteSolicitacao);

        foreach (var apuracao in retorno.Apuracao)
        {
            foreach (var item in apuracao.Debitos ?? [])
            {
                await UpsertDebitoAsync(new Debito
                {
                    SolicitacaoId = solicitacao.Id,
                    Ni = retorno.Ni,
                    NiConsumidor = retorno.NiConsumidor,
                    Pa = apuracao.Pa,
                    Origem = item.Origem,
                    Documento = item.Documento,
                    Chave = item.Chave,
                    Emissao = item.Emissao,
                    Registro = item.Registro,
                    Atualizacao = item.Atualizacao,
                    CbsExcedente = item.Cbs.Excedente,
                    CbsApurado = item.Cbs.Apurado,
                    CbsInexigivel = item.Cbs.Inexigivel,
                    CbsSuspenso = item.Cbs.Suspenso,
                    CbsExtinto = item.Cbs.Extinto,
                    CbsSaldoDevedor = item.Cbs.SaldoDevedor
                }, cancellationToken);
            }
        }
    }

    private async Task ProcessarCreditosAsync(
        Stream stream,
        Solicitacao solicitacao,
        CancellationToken cancellationToken)
    {
        var retorno = await JsonSerializer.DeserializeAsync<RetornoCbsDto>(stream, JsonOptions, cancellationToken)
            ?? throw new InvalidOperationException("Arquivo de créditos vazio ou inválido.");

        ValidarTiquete(retorno.TiqueteSolicitacao, solicitacao.TiqueteSolicitacao);

        foreach (var apuracao in retorno.Apuracao)
        foreach (var item in apuracao.Creditos ?? [])
        {
            var cbs = item.Cbs;
            await UpsertCreditoAsync(new Credito
            {
                SolicitacaoId = solicitacao.Id,
                Ni = retorno.Ni,
                NiConsumidor = retorno.NiConsumidor,
                Pa = apuracao.Pa,
                Origem = item.Origem,
                Documento = item.Documento,
                Chave = item.Chave,
                Emissao = item.Emissao,
                Registro = item.Registro,
                Atualizacao = item.Atualizacao,
                CbsExcedentes = cbs.Excedentes,
                CbsApurado = cbs.Apurado,
                CbsInapropriavel = cbs.Apropriacao.Inapropriavel,
                CbsSuspenso = cbs.Apropriacao.Suspenso,
                CbsPrescrito = cbs.Apropriacao.Prescrito,
                CbsAApropriar = cbs.Apropriacao.AApropriar,
                CbsApropriado = cbs.Apropriacao.Apropriado,
                CbsInutilizavel = cbs.Apropriacao.Utilizacao.Inutilizavel,
                CbsUtilizado = cbs.Apropriacao.Utilizacao.Utilizado,
                CbsRestabelecido = cbs.Apropriacao.Utilizacao.Restabelecido,
                CbsSaldoCredor = cbs.Apropriacao.Utilizacao.NaoUtilizado.SaldoCredor,
                CbsPedidoRessarcimento = cbs.Apropriacao.Utilizacao.NaoUtilizado.PedidoRessarcimento
            }, cancellationToken);
        }
    }

    private async Task ProcessarPagamentosAsync(
        Stream stream,
        Solicitacao solicitacao,
        CancellationToken cancellationToken)
    {
        var retorno = await JsonSerializer.DeserializeAsync<RetornoPagamentosDto>(stream, JsonOptions, cancellationToken)
            ?? throw new InvalidOperationException("Arquivo de pagamentos vazio ou inválido.");

        ValidarTiquete(retorno.TiqueteSolicitacao, solicitacao.TiqueteSolicitacao);

        foreach (var apuracao in retorno.Apuracao)
        foreach (var pagamento in apuracao.Pagamentos)
        foreach (var item in pagamento.Composicao)
        {
            await UpsertPagamentoAsync(new Pagamento
            {
                SolicitacaoId = solicitacao.Id,
                Ni = retorno.Ni,
                NiConsumidor = retorno.NiConsumidor,
                DataArrecadacao = apuracao.DataArrecadacao,
                NumeroDarf = pagamento.NumeroDarf,
                Tipo = pagamento.Tipo,
                NiAdquirente = pagamento.NiAdquirente ?? "",
                Sequencial = item.Sequencial,
                Pa = item.Pa,
                Vencimento = item.Vencimento,
                NiContribuinte = item.NiContribuinte ?? "",
                ChaveDfe = item.ChaveDfe ?? "",
                Principal = item.Principal,
                Multa = item.Multa,
                Juros = item.Juros,
                Total = item.Total
            }, cancellationToken);
        }
    }

    private async Task ProcessarRecolhimentosAsync(
        Stream stream,
        Solicitacao solicitacao,
        CancellationToken cancellationToken)
    {
        var retorno = await JsonSerializer.DeserializeAsync<RetornoRecolhimentosDto>(stream, JsonOptions, cancellationToken)
            ?? throw new InvalidOperationException("Arquivo de recolhimentos vazio ou inválido.");

        ValidarTiquete(retorno.TiqueteSolicitacao, solicitacao.TiqueteSolicitacao);

        foreach (var apuracao in retorno.Apuracao)
        foreach (var pagamento in apuracao.Pagamentos)
        foreach (var item in pagamento.Composicao)
        {
            await UpsertRecolhimentoAsync(new Recolhimento
            {
                SolicitacaoId = solicitacao.Id,
                Ni = retorno.Ni,
                NiConsumidor = retorno.NiConsumidor,
                DataArrecadacao = apuracao.DataArrecadacao,
                NumeroDarf = pagamento.NumeroDarf,
                Tipo = pagamento.Tipo,
                Sequencial = item.Sequencial,
                Pa = item.Pa,
                Vencimento = item.Vencimento,
                NiFornecedor = item.NiFornecedor ?? "",
                ChaveDfe = item.ChaveDfe ?? "",
                Principal = item.Principal,
                Multa = item.Multa,
                Juros = item.Juros,
                Total = item.Total
            }, cancellationToken);
        }
    }

    private async Task UpsertDebitoAsync(Debito item, CancellationToken ct)
    {
        var atual = await db.Debitos.SingleOrDefaultAsync(
            x => x.Ni == item.Ni && x.Pa == item.Pa && x.Chave == item.Chave &&
                 x.Origem == item.Origem && x.Documento == item.Documento, ct);

        if (atual is null) db.Debitos.Add(item);
        else
        {
            item.Id = atual.Id;
            db.Entry(atual).CurrentValues.SetValues(item);
        }
    }

    private async Task UpsertCreditoAsync(Credito item, CancellationToken ct)
    {
        var atual = await db.Creditos.SingleOrDefaultAsync(
            x => x.Ni == item.Ni && x.Pa == item.Pa && x.Chave == item.Chave &&
                 x.Origem == item.Origem && x.Documento == item.Documento, ct);

        if (atual is null) db.Creditos.Add(item);
        else
        {
            item.Id = atual.Id;
            db.Entry(atual).CurrentValues.SetValues(item);
        }
    }

    private async Task UpsertPagamentoAsync(Pagamento item, CancellationToken ct)
    {
        var atual = await db.Pagamentos.SingleOrDefaultAsync(
            x => x.Ni == item.Ni && x.DataArrecadacao == item.DataArrecadacao &&
                 x.NumeroDarf == item.NumeroDarf && x.Tipo == item.Tipo &&
                 x.Sequencial == item.Sequencial && x.Pa == item.Pa &&
                 x.ChaveDfe == item.ChaveDfe, ct);

        if (atual is null) db.Pagamentos.Add(item);
        else
        {
            item.Id = atual.Id;
            db.Entry(atual).CurrentValues.SetValues(item);
        }
    }

    private async Task UpsertRecolhimentoAsync(Recolhimento item, CancellationToken ct)
    {
        var atual = await db.Recolhimentos.SingleOrDefaultAsync(
            x => x.Ni == item.Ni && x.DataArrecadacao == item.DataArrecadacao &&
                 x.NumeroDarf == item.NumeroDarf && x.Tipo == item.Tipo &&
                 x.Sequencial == item.Sequencial && x.Pa == item.Pa &&
                 x.ChaveDfe == item.ChaveDfe, ct);

        if (atual is null) db.Recolhimentos.Add(item);
        else
        {
            item.Id = atual.Id;
            db.Entry(atual).CurrentValues.SetValues(item);
        }
    }

    private static string NormalizarTipo(string tipo) =>
        tipo.Trim().ToUpperInvariant()
            .Replace("DÉBITO", "DEBITO")
            .Replace("CRÉDITO", "CREDITO")
            .Replace("PAGAMENTOS", "PAGAMENTO")
            .Replace("RECOLHIMENTOS", "RECOLHIMENTO");

    private static void ValidarTiquete(string recebido, string esperado)
    {
        if (!string.Equals(recebido, esperado, StringComparison.Ordinal))
            throw new InvalidOperationException("O tíquete do arquivo não corresponde à solicitação.");
    }
}
