using RadarWeb.Net.ApuracaoAssistida.Domain.Entities;

namespace RadarWeb.Net.ApuracaoAssistida.Application.Interfaces;

public interface IApuracaoCbsRepository
{
    Task<Debito> InserirDebitoAsync(Debito entidade, CancellationToken cancellationToken = default);
    Task AtualizarDebitoAsync(Debito entidade, CancellationToken cancellationToken = default);
    Task<Debito> InserirOuAtualizarDebitoAsync(Debito entidade, CancellationToken cancellationToken = default);

    Task<Credito> InserirCreditoAsync(Credito entidade, CancellationToken cancellationToken = default);
    Task AtualizarCreditoAsync(Credito entidade, CancellationToken cancellationToken = default);
    Task<Credito> InserirOuAtualizarCreditoAsync(Credito entidade, CancellationToken cancellationToken = default);

    Task<Pagamento> InserirPagamentoAsync(Pagamento entidade, CancellationToken cancellationToken = default);
    Task AtualizarPagamentoAsync(Pagamento entidade, CancellationToken cancellationToken = default);
    Task<Pagamento> InserirOuAtualizarPagamentoAsync(Pagamento entidade, CancellationToken cancellationToken = default);

    Task<Recolhimento> InserirRecolhimentoAsync(Recolhimento entidade, CancellationToken cancellationToken = default);
    Task AtualizarRecolhimentoAsync(Recolhimento entidade, CancellationToken cancellationToken = default);
    Task<Recolhimento> InserirOuAtualizarRecolhimentoAsync(Recolhimento entidade, CancellationToken cancellationToken = default);
}
