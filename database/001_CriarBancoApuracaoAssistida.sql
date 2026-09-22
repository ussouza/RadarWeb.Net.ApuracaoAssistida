/*
    RadarWeb.Net.ApuracaoAssistida
    Script inicial do banco de dados.

    Observações:
    - Não são utilizadas migrations.
    - Execute este script manualmente no SQL Server.
    - As tabelas de retorno seguem os contratos publicados pela Receita Federal.
    - Pagamentos e recolhimentos possuem uma linha por item de composição,
      preservando os campos do objeto pai junto ao item.
*/

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'CBS')
BEGIN
    EXEC('CREATE SCHEMA CBS');
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

IF OBJECT_ID('CBS.Debito', 'U') IS NULL
BEGIN
    CREATE TABLE CBS.Debito
    (
        Id BIGINT IDENTITY(1,1) NOT NULL,
        SolicitacaoId BIGINT NOT NULL,
        Ni VARCHAR(8) NOT NULL,
        NiConsumidor VARCHAR(20) NULL,
        Pa VARCHAR(7) NOT NULL,
        Origem INT NOT NULL,
        Documento INT NOT NULL,
        Chave VARCHAR(50) NOT NULL,
        Emissao DATETIME2 NOT NULL,
        Registro DATETIME2 NOT NULL,
        Atualizacao DATETIME2 NOT NULL,
        CbsExcedente DECIMAL(18,2) NULL,
        CbsApurado DECIMAL(18,2) NOT NULL,
        CbsInexigivel DECIMAL(18,2) NULL,
        CbsSuspenso DECIMAL(18,2) NULL,
        CbsExtinto DECIMAL(18,2) NULL,
        CbsSaldoDevedor DECIMAL(18,2) NOT NULL,

        CONSTRAINT PK_Debito PRIMARY KEY (Id),
        CONSTRAINT FK_Debito_Solicitacao FOREIGN KEY (SolicitacaoId) REFERENCES CBS.Solicitacao(Id),
        CONSTRAINT UQ_Debito_Chave UNIQUE (Ni, Pa, Chave, Origem, Documento)
    );
END
GO

IF OBJECT_ID('CBS.Credito', 'U') IS NULL
BEGIN
    CREATE TABLE CBS.Credito
    (
        Id BIGINT IDENTITY(1,1) NOT NULL,
        SolicitacaoId BIGINT NOT NULL,
        Ni VARCHAR(8) NOT NULL,
        NiConsumidor VARCHAR(20) NULL,
        Pa VARCHAR(7) NOT NULL,
        Origem INT NOT NULL,
        Documento INT NOT NULL,
        Chave VARCHAR(50) NOT NULL,
        Emissao DATETIME2 NOT NULL,
        Registro DATETIME2 NOT NULL,
        Atualizacao DATETIME2 NOT NULL,
        CbsExcedentes DECIMAL(18,2) NULL,
        CbsApurado DECIMAL(18,2) NOT NULL,
        CbsInapropriavel DECIMAL(18,2) NULL,
        CbsSuspenso DECIMAL(18,2) NULL,
        CbsPrescrito DECIMAL(18,2) NULL,
        CbsAApropriar DECIMAL(18,2) NULL,
        CbsApropriado DECIMAL(18,2) NULL,
        CbsInutilizavel DECIMAL(18,2) NULL,
        CbsUtilizado DECIMAL(18,2) NULL,
        CbsRestabelecido DECIMAL(18,2) NULL,
        CbsSaldoCredor DECIMAL(18,2) NULL,
        CbsPedidoRessarcimento DECIMAL(18,2) NULL,

        CONSTRAINT PK_Credito PRIMARY KEY (Id),
        CONSTRAINT FK_Credito_Solicitacao FOREIGN KEY (SolicitacaoId) REFERENCES CBS.Solicitacao(Id),
        CONSTRAINT UQ_Credito_Chave UNIQUE (Ni, Pa, Chave, Origem, Documento)
    );
END
GO

IF OBJECT_ID('CBS.Pagamento', 'U') IS NULL
BEGIN
    CREATE TABLE CBS.Pagamento
    (
        Id BIGINT IDENTITY(1,1) NOT NULL,
        SolicitacaoId BIGINT NOT NULL,
        Ni VARCHAR(8) NOT NULL,
        NiConsumidor VARCHAR(20) NULL,
        DataArrecadacao DATETIME2 NOT NULL,
        NumeroDarf VARCHAR(50) NOT NULL CONSTRAINT DF_Pagamento_NumeroDarf DEFAULT (''),
        Tipo INT NOT NULL,
        NiAdquirente VARCHAR(20) NOT NULL CONSTRAINT DF_Pagamento_NiAdquirente DEFAULT (''),
        Sequencial INT NOT NULL,
        Pa VARCHAR(7) NOT NULL,
        Vencimento DATETIME2 NOT NULL,
        NiContribuinte VARCHAR(20) NOT NULL CONSTRAINT DF_Pagamento_NiContribuinte DEFAULT (''),
        ChaveDfe VARCHAR(50) NOT NULL CONSTRAINT DF_Pagamento_ChaveDfe DEFAULT (''),
        Principal DECIMAL(18,2) NOT NULL,
        Multa DECIMAL(18,2) NOT NULL,
        Juros DECIMAL(18,2) NOT NULL,
        Total DECIMAL(18,2) NOT NULL,

        CONSTRAINT PK_Pagamento PRIMARY KEY (Id),
        CONSTRAINT FK_Pagamento_Solicitacao FOREIGN KEY (SolicitacaoId) REFERENCES CBS.Solicitacao(Id),
        CONSTRAINT UQ_Pagamento_Composicao UNIQUE
            (Ni, DataArrecadacao, NumeroDarf, Tipo, Sequencial, Pa, ChaveDfe)
    );
END
GO

IF OBJECT_ID('CBS.Recolhimento', 'U') IS NULL
BEGIN
    CREATE TABLE CBS.Recolhimento
    (
        Id BIGINT IDENTITY(1,1) NOT NULL,
        SolicitacaoId BIGINT NOT NULL,
        Ni VARCHAR(8) NOT NULL,
        NiConsumidor VARCHAR(20) NULL,
        DataArrecadacao DATETIME2 NOT NULL,
        NumeroDarf VARCHAR(50) NOT NULL CONSTRAINT DF_Recolhimento_NumeroDarf DEFAULT (''),
        Tipo INT NOT NULL,
        Sequencial INT NOT NULL,
        Pa VARCHAR(7) NOT NULL,
        Vencimento DATETIME2 NOT NULL,
        NiFornecedor VARCHAR(20) NOT NULL CONSTRAINT DF_Recolhimento_NiFornecedor DEFAULT (''),
        ChaveDfe VARCHAR(50) NOT NULL CONSTRAINT DF_Recolhimento_ChaveDfe DEFAULT (''),
        Principal DECIMAL(18,2) NOT NULL,
        Multa DECIMAL(18,2) NOT NULL,
        Juros DECIMAL(18,2) NOT NULL,
        Total DECIMAL(18,2) NOT NULL,

        CONSTRAINT PK_Recolhimento PRIMARY KEY (Id),
        CONSTRAINT FK_Recolhimento_Solicitacao FOREIGN KEY (SolicitacaoId) REFERENCES CBS.Solicitacao(Id),
        CONSTRAINT UQ_Recolhimento_Composicao UNIQUE
            (Ni, DataArrecadacao, NumeroDarf, Tipo, Sequencial, Pa, ChaveDfe)
    );
END
GO

