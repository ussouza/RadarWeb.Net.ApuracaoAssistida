using Microsoft.EntityFrameworkCore;
using RadarWeb.Net.ApuracaoAssistida.Application.Interfaces;
using RadarWeb.Net.ApuracaoAssistida.Domain.Entities;
using RadarWeb.Net.ApuracaoAssistida.Infrastructure.Data;

namespace RadarWeb.Net.ApuracaoAssistida.Infrastructure.Repositories;

public sealed class ApuracaoCbsRepository(ApuracaoAssistidaDbContext dbContext) : IApuracaoCbsRepository
{
    public async Task<Debito> InserirDebitoAsync(Debito entidade, CancellationToken cancellationToken = default)
    {
        dbContext.Debitos.Add(entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entidade;
    }

    public async Task AtualizarDebitoAsync(Debito entidade, CancellationToken cancellationToken = default)
    {
        var atual = await dbContext.Debitos.SingleAsync(x => x.Id == entidade.Id, cancellationToken);
        Copiar(atual, entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Debito> InserirOuAtualizarDebitoAsync(Debito entidade, CancellationToken cancellationToken = default)
    {
        var atual = await dbContext.Debitos.SingleOrDefaultAsync(
            x => x.Ni == entidade.Ni &&
                 x.Pa == entidade.Pa &&
                 x.Chave == entidade.Chave &&
                 x.Origem == entidade.Origem &&
                 x.Documento == entidade.Documento,
            cancellationToken);

        if (atual is null)
            return await InserirDebitoAsync(entidade, cancellationToken);

        Copiar(atual, entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
        return atual;
    }

    public async Task<Credito> InserirCreditoAsync(Credito entidade, CancellationToken cancellationToken = default)
    {
        dbContext.Creditos.Add(entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entidade;
    }

    public async Task AtualizarCreditoAsync(Credito entidade, CancellationToken cancellationToken = default)
    {
        var atual = await dbContext.Creditos.SingleAsync(x => x.Id == entidade.Id, cancellationToken);
        Copiar(atual, entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Credito> InserirOuAtualizarCreditoAsync(Credito entidade, CancellationToken cancellationToken = default)
    {
        var atual = await dbContext.Creditos.SingleOrDefaultAsync(
            x => x.Ni == entidade.Ni &&
                 x.Pa == entidade.Pa &&
                 x.Chave == entidade.Chave &&
                 x.Origem == entidade.Origem &&
                 x.Documento == entidade.Documento,
            cancellationToken);

        if (atual is null)
            return await InserirCreditoAsync(entidade, cancellationToken);

        Copiar(atual, entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
        return atual;
    }

    public async Task<Pagamento> InserirPagamentoAsync(Pagamento entidade, CancellationToken cancellationToken = default)
    {
        dbContext.Pagamentos.Add(entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entidade;
    }

    public async Task AtualizarPagamentoAsync(Pagamento entidade, CancellationToken cancellationToken = default)
    {
        var atual = await dbContext.Pagamentos.SingleAsync(x => x.Id == entidade.Id, cancellationToken);
        Copiar(atual, entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Pagamento> InserirOuAtualizarPagamentoAsync(Pagamento entidade, CancellationToken cancellationToken = default)
    {
        var atual = await dbContext.Pagamentos.SingleOrDefaultAsync(
            x => x.Ni == entidade.Ni &&
                 x.DataArrecadacao == entidade.DataArrecadacao &&
                 x.NumeroDarf == entidade.NumeroDarf &&
                 x.Tipo == entidade.Tipo &&
                 x.Sequencial == entidade.Sequencial &&
                 x.Pa == entidade.Pa &&
                 x.ChaveDfe == entidade.ChaveDfe,
            cancellationToken);

        if (atual is null)
            return await InserirPagamentoAsync(entidade, cancellationToken);

        Copiar(atual, entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
        return atual;
    }

    public async Task<Recolhimento> InserirRecolhimentoAsync(Recolhimento entidade, CancellationToken cancellationToken = default)
    {
        dbContext.Recolhimentos.Add(entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entidade;
    }

    public async Task AtualizarRecolhimentoAsync(Recolhimento entidade, CancellationToken cancellationToken = default)
    {
        var atual = await dbContext.Recolhimentos.SingleAsync(x => x.Id == entidade.Id, cancellationToken);
        Copiar(atual, entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Recolhimento> InserirOuAtualizarRecolhimentoAsync(Recolhimento entidade, CancellationToken cancellationToken = default)
    {
        var atual = await dbContext.Recolhimentos.SingleOrDefaultAsync(
            x => x.Ni == entidade.Ni &&
                 x.DataArrecadacao == entidade.DataArrecadacao &&
                 x.NumeroDarf == entidade.NumeroDarf &&
                 x.Tipo == entidade.Tipo &&
                 x.Sequencial == entidade.Sequencial &&
                 x.Pa == entidade.Pa &&
                 x.ChaveDfe == entidade.ChaveDfe,
            cancellationToken);

        if (atual is null)
            return await InserirRecolhimentoAsync(entidade, cancellationToken);

        Copiar(atual, entidade);
        await dbContext.SaveChangesAsync(cancellationToken);
        return atual;
    }

    private static void Copiar(Debito destino, Debito origem)
    {
        destino.SolicitacaoId = origem.SolicitacaoId;
        destino.Ni = origem.Ni;
        destino.NiConsumidor = origem.NiConsumidor;
        destino.Pa = origem.Pa;
        destino.Origem = origem.Origem;
        destino.Documento = origem.Documento;
        destino.Chave = origem.Chave;
        destino.Emissao = origem.Emissao;
        destino.Registro = origem.Registro;
        destino.Atualizacao = origem.Atualizacao;
        destino.CbsExcedente = origem.CbsExcedente;
        destino.CbsApurado = origem.CbsApurado;
        destino.CbsInexigivel = origem.CbsInexigivel;
        destino.CbsSuspenso = origem.CbsSuspenso;
        destino.CbsExtinto = origem.CbsExtinto;
        destino.CbsSaldoDevedor = origem.CbsSaldoDevedor;
    }

    private static void Copiar(Credito destino, Credito origem)
    {
        destino.SolicitacaoId = origem.SolicitacaoId;
        destino.Ni = origem.Ni;
        destino.NiConsumidor = origem.NiConsumidor;
        destino.Pa = origem.Pa;
        destino.Origem = origem.Origem;
        destino.Documento = origem.Documento;
        destino.Chave = origem.Chave;
        destino.Emissao = origem.Emissao;
        destino.Registro = origem.Registro;
        destino.Atualizacao = origem.Atualizacao;
        destino.CbsExcedentes = origem.CbsExcedentes;
        destino.CbsApurado = origem.CbsApurado;
        destino.CbsInapropriavel = origem.CbsInapropriavel;
        destino.CbsSuspenso = origem.CbsSuspenso;
        destino.CbsPrescrito = origem.CbsPrescrito;
        destino.CbsAApropriar = origem.CbsAApropriar;
        destino.CbsApropriado = origem.CbsApropriado;
        destino.CbsInutilizavel = origem.CbsInutilizavel;
        destino.CbsUtilizado = origem.CbsUtilizado;
        destino.CbsRestabelecido = origem.CbsRestabelecido;
        destino.CbsSaldoCredor = origem.CbsSaldoCredor;
        destino.CbsPedidoRessarcimento = origem.CbsPedidoRessarcimento;
    }

    private static void Copiar(Pagamento destino, Pagamento origem)
    {
        destino.SolicitacaoId = origem.SolicitacaoId;
        destino.Ni = origem.Ni;
        destino.NiConsumidor = origem.NiConsumidor;
        destino.DataArrecadacao = origem.DataArrecadacao;
        destino.NumeroDarf = origem.NumeroDarf;
        destino.Tipo = origem.Tipo;
        destino.NiAdquirente = origem.NiAdquirente;
        destino.Sequencial = origem.Sequencial;
        destino.Pa = origem.Pa;
        destino.Vencimento = origem.Vencimento;
        destino.NiContribuinte = origem.NiContribuinte;
        destino.ChaveDfe = origem.ChaveDfe;
        destino.Principal = origem.Principal;
        destino.Multa = origem.Multa;
        destino.Juros = origem.Juros;
        destino.Total = origem.Total;
    }

    private static void Copiar(Recolhimento destino, Recolhimento origem)
    {
        destino.SolicitacaoId = origem.SolicitacaoId;
        destino.Ni = origem.Ni;
        destino.NiConsumidor = origem.NiConsumidor;
        destino.DataArrecadacao = origem.DataArrecadacao;
        destino.NumeroDarf = origem.NumeroDarf;
        destino.Tipo = origem.Tipo;
        destino.Sequencial = origem.Sequencial;
        destino.Pa = origem.Pa;
        destino.Vencimento = origem.Vencimento;
        destino.NiFornecedor = origem.NiFornecedor;
        destino.ChaveDfe = origem.ChaveDfe;
        destino.Principal = origem.Principal;
        destino.Multa = origem.Multa;
        destino.Juros = origem.Juros;
        destino.Total = origem.Total;
    }
}
