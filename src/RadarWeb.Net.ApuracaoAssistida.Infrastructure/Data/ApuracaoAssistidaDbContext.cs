using Microsoft.EntityFrameworkCore;
using RadarWeb.Net.ApuracaoAssistida.Domain.Entities;

namespace RadarWeb.Net.ApuracaoAssistida.Infrastructure.Data;

public sealed class ApuracaoAssistidaDbContext(DbContextOptions<ApuracaoAssistidaDbContext> options)
    : DbContext(options)
{
    public DbSet<Empresa> Empresas => Set<Empresa>();

    public DbSet<Solicitacao> Solicitacoes => Set<Solicitacao>();

    public DbSet<RetornoSolicitacao> RetornosSolicitacao => Set<RetornoSolicitacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("CBS");

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.ToTable("Empresa");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.CnpjBase)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsRequired();

            entity.HasIndex(x => x.CnpjBase)
                .IsUnique();

            entity.Property(x => x.Nome)
                .HasMaxLength(200);

            entity.Property(x => x.DataCadastro)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        modelBuilder.Entity<Solicitacao>(entity =>
        {
            entity.ToTable("Solicitacao");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.TipoConsulta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(x => x.TiqueteSolicitacao)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsRequired();

            entity.HasIndex(x => x.TiqueteSolicitacao)
                .IsUnique();

            entity.Property(x => x.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(x => x.CodigoErro)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(x => x.MensagemErro)
                .HasMaxLength(2000);

            entity.Property(x => x.DataSolicitacao)
                .HasColumnType("datetime2");

            entity.Property(x => x.DataConclusao)
                .HasColumnType("datetime2");

            entity.Property(x => x.DataUltimaNotificacao)
                .HasColumnType("datetime2");

            entity.Property(x => x.DataProcessamento)
                .HasColumnType("datetime2");

            entity.HasIndex(x => new { x.EmpresaId, x.TipoConsulta, x.DataSolicitacao });

            entity.HasOne(x => x.Empresa)
                .WithMany(x => x.Solicitacoes)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RetornoSolicitacao>(entity =>
        {
            entity.ToTable("RetornoSolicitacao");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.NomeArquivo)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.ContentType)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(x => x.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(x => x.HashSha256)
                .HasMaxLength(64)
                .IsUnicode(false);

            entity.Property(x => x.MensagemErro)
                .HasMaxLength(2000);

            entity.Property(x => x.DataDownload)
                .HasColumnType("datetime2");

            entity.HasIndex(x => new { x.SolicitacaoId, x.HashSha256 });

            entity.HasOne(x => x.Solicitacao)
                .WithMany()
                .HasForeignKey(x => x.SolicitacaoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
