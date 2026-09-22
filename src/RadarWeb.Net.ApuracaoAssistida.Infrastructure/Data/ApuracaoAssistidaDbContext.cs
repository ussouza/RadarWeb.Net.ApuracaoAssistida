using Microsoft.EntityFrameworkCore;
using RadarWeb.Net.ApuracaoAssistida.Domain.Entities;

namespace RadarWeb.Net.ApuracaoAssistida.Infrastructure.Data;

public sealed class ApuracaoAssistidaDbContext(DbContextOptions<ApuracaoAssistidaDbContext> options)
    : DbContext(options)
{
    public DbSet<Empresa> Empresas => Set<Empresa>();

    public DbSet<Solicitacao> Solicitacoes => Set<Solicitacao>();

    public DbSet<RetornoSolicitacao> RetornosSolicitacao => Set<RetornoSolicitacao>();

    public DbSet<Debito> Debitos => Set<Debito>();

    public DbSet<Credito> Creditos => Set<Credito>();

    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();

    public DbSet<Recolhimento> Recolhimentos => Set<Recolhimento>();

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

        modelBuilder.Entity<Debito>(entity =>
        {
            entity.ToTable("Debito");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Ni).HasMaxLength(8).IsUnicode(false).IsRequired();
            entity.Property(x => x.NiConsumidor).HasMaxLength(20).IsUnicode(false);
            entity.Property(x => x.Pa).HasMaxLength(7).IsUnicode(false).IsRequired();
            entity.Property(x => x.Chave).HasMaxLength(50).IsUnicode(false).IsRequired();
            entity.Property(x => x.Emissao).HasColumnType("datetime2");
            entity.Property(x => x.Registro).HasColumnType("datetime2");
            entity.Property(x => x.Atualizacao).HasColumnType("datetime2");
            entity.Property(x => x.CbsExcedente).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsApurado).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsInexigivel).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsSuspenso).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsExtinto).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsSaldoDevedor).HasColumnType("decimal(18,2)");
            entity.HasIndex(x => new { x.Ni, x.Pa, x.Chave, x.Origem, x.Documento }).IsUnique();
            entity.HasOne(x => x.Solicitacao).WithMany().HasForeignKey(x => x.SolicitacaoId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Credito>(entity =>
        {
            entity.ToTable("Credito");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Ni).HasMaxLength(8).IsUnicode(false).IsRequired();
            entity.Property(x => x.NiConsumidor).HasMaxLength(20).IsUnicode(false);
            entity.Property(x => x.Pa).HasMaxLength(7).IsUnicode(false).IsRequired();
            entity.Property(x => x.Chave).HasMaxLength(50).IsUnicode(false).IsRequired();
            entity.Property(x => x.Emissao).HasColumnType("datetime2");
            entity.Property(x => x.Registro).HasColumnType("datetime2");
            entity.Property(x => x.Atualizacao).HasColumnType("datetime2");
            entity.Property(x => x.CbsExcedentes).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsApurado).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsInapropriavel).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsSuspenso).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsPrescrito).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsAApropriar).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsApropriado).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsInutilizavel).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsUtilizado).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsRestabelecido).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsSaldoCredor).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CbsPedidoRessarcimento).HasColumnType("decimal(18,2)");
            entity.HasIndex(x => new { x.Ni, x.Pa, x.Chave, x.Origem, x.Documento }).IsUnique();
            entity.HasOne(x => x.Solicitacao).WithMany().HasForeignKey(x => x.SolicitacaoId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.ToTable("Pagamento");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Ni).HasMaxLength(8).IsUnicode(false).IsRequired();
            entity.Property(x => x.NiConsumidor).HasMaxLength(20).IsUnicode(false);
            entity.Property(x => x.DataArrecadacao).HasColumnType("datetime2");
            entity.Property(x => x.NumeroDarf).HasMaxLength(50).IsUnicode(false).IsRequired();
            entity.Property(x => x.NiAdquirente).HasMaxLength(20).IsUnicode(false).IsRequired();
            entity.Property(x => x.Pa).HasMaxLength(7).IsUnicode(false).IsRequired();
            entity.Property(x => x.Vencimento).HasColumnType("datetime2");
            entity.Property(x => x.NiContribuinte).HasMaxLength(20).IsUnicode(false).IsRequired();
            entity.Property(x => x.ChaveDfe).HasMaxLength(50).IsUnicode(false).IsRequired();
            entity.Property(x => x.Principal).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Multa).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Juros).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Total).HasColumnType("decimal(18,2)");
            entity.HasIndex(x => new { x.Ni, x.DataArrecadacao, x.NumeroDarf, x.Tipo, x.Sequencial, x.Pa, x.ChaveDfe }).IsUnique();
            entity.HasOne(x => x.Solicitacao).WithMany().HasForeignKey(x => x.SolicitacaoId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Recolhimento>(entity =>
        {
            entity.ToTable("Recolhimento");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Ni).HasMaxLength(8).IsUnicode(false).IsRequired();
            entity.Property(x => x.NiConsumidor).HasMaxLength(20).IsUnicode(false);
            entity.Property(x => x.DataArrecadacao).HasColumnType("datetime2");
            entity.Property(x => x.NumeroDarf).HasMaxLength(50).IsUnicode(false);
            entity.Property(x => x.Pa).HasMaxLength(7).IsUnicode(false).IsRequired();
            entity.Property(x => x.Vencimento).HasColumnType("datetime2");
            entity.Property(x => x.NiFornecedor).HasMaxLength(20).IsUnicode(false);
            entity.Property(x => x.ChaveDfe).HasMaxLength(50).IsUnicode(false);
            entity.Property(x => x.Principal).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Multa).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Juros).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Total).HasColumnType("decimal(18,2)");
            entity.HasIndex(x => new { x.Ni, x.DataArrecadacao, x.NumeroDarf, x.Tipo, x.Sequencial, x.Pa, x.ChaveDfe }).IsUnique();
            entity.HasOne(x => x.Solicitacao).WithMany().HasForeignKey(x => x.SolicitacaoId).OnDelete(DeleteBehavior.Restrict);
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
