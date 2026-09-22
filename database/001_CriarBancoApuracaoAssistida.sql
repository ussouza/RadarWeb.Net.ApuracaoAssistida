/*
    RadarWeb.Net.ApuracaoAssistida
    Script inicial do banco de dados.

    Observações:
    - Não são utilizadas migrations.
    - Execute este script manualmente no SQL Server.
    - O script cria apenas as tabelas de controle da integração.
    - As tabelas de débitos, créditos, pagamentos e recolhimentos
      serão modeladas após fecharmos os contratos dos respectivos retornos da API.
*/

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'CBS')
BEGIN
    EXEC('CREATE SCHEMA CBS');
END
GO

IF OBJECT_ID('CBS.RetornoSolicitacao', 'U') IS NULL
BEGIN
    CREATE TABLE CBS.RetornoSolicitacao
    (
        Id BIGINT IDENTITY(1,1) NOT NULL,
        SolicitacaoId BIGINT NOT NULL,
        NomeArquivo VARCHAR(255) NOT NULL,
        ContentType VARCHAR(100) NULL,
        TamanhoBytes BIGINT NULL,
        DataDownload DATETIME2 NOT NULL,
        Status VARCHAR(30) NOT NULL,
        HashSha256 VARCHAR(64) NULL,
        MensagemErro VARCHAR(2000) NULL,

        CONSTRAINT PK_RetornoSolicitacao PRIMARY KEY (Id),
        CONSTRAINT FK_RetornoSolicitacao_Solicitacao
            FOREIGN KEY (SolicitacaoId)
            REFERENCES CBS.Solicitacao (Id)
    );

    CREATE INDEX IX_RetornoSolicitacao_SolicitacaoId_HashSha256
        ON CBS.RetornoSolicitacao (SolicitacaoId, HashSha256);
END
GO

IF OBJECT_ID('CBS.Solicitacao', 'U') IS NULL
BEGIN
    CREATE TABLE CBS.Solicitacao
    (
        Id BIGINT IDENTITY(1,1) NOT NULL,
        EmpresaId INT NOT NULL,
        TipoConsulta VARCHAR(50) NOT NULL,
        TiqueteSolicitacao VARCHAR(200) NOT NULL,
        DataSolicitacao DATETIME2 NOT NULL,
        Status VARCHAR(30) NOT NULL,
        DataConclusao DATETIME2 NULL,
        DataUltimaNotificacao DATETIME2 NULL,
        Tentativas INT NOT NULL CONSTRAINT DF_Solicitacao_Tentativas DEFAULT (0),
        CodigoErro VARCHAR(100) NULL,
        MensagemErro VARCHAR(2000) NULL,
        DataProcessamento DATETIME2 NULL,

        CONSTRAINT PK_Solicitacao PRIMARY KEY (Id),
        CONSTRAINT UQ_Solicitacao_TiqueteSolicitacao UNIQUE (TiqueteSolicitacao),
        CONSTRAINT FK_Solicitacao_Empresa
            FOREIGN KEY (EmpresaId)
            REFERENCES CBS.Empresa (Id)
    );

    CREATE INDEX IX_Solicitacao_Empresa_Tipo_Data
        ON CBS.Solicitacao (EmpresaId, TipoConsulta, DataSolicitacao);
END
GO

IF OBJECT_ID('CBS.Empresa', 'U') IS NULL
BEGIN
    CREATE TABLE CBS.Empresa
    (
        Id INT IDENTITY(1,1) NOT NULL,
        CnpjBase VARCHAR(8) NOT NULL,
        Nome VARCHAR(200) NULL,
        Ativo BIT NOT NULL CONSTRAINT DF_Empresa_Ativo DEFAULT (1),
        DataCadastro DATETIME2 NOT NULL CONSTRAINT DF_Empresa_DataCadastro DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT PK_Empresa PRIMARY KEY (Id),
        CONSTRAINT UQ_Empresa_CnpjBase UNIQUE (CnpjBase)
    );
END
GO
