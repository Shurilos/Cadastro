-- =============================================
-- WebApplication1 - Script de criação do banco
-- SQL Server
-- =============================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'WebApplication1Db')
BEGIN
    CREATE DATABASE WebApplication1Db;
END
GO

USE WebApplication1Db;
GO

-- Tabela: Usuarios
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        Id            INT IDENTITY(1,1) PRIMARY KEY,
        Nome          NVARCHAR(100)  NOT NULL,
        Sobrenome     NVARCHAR(100)  NOT NULL,
        Email         NVARCHAR(150)  NOT NULL,
        Cpf           NVARCHAR(14)   NOT NULL,
        Telefone      NVARCHAR(20)   NULL,
        Perfil        NVARCHAR(50)   NOT NULL,
        Senha         NVARCHAR(255)  NOT NULL,
        DataCadastro  DATETIME       NOT NULL DEFAULT GETDATE(),

        CONSTRAINT UQ_Usuarios_Email UNIQUE (Email),
        CONSTRAINT UQ_Usuarios_Cpf   UNIQUE (Cpf)
    );

    PRINT 'Tabela Usuarios criada com sucesso.';
END
ELSE
    PRINT 'Tabela Usuarios já existe.';
GO

-- Tabela: Empresas
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Empresas')
BEGIN
    CREATE TABLE Empresas (
        Id            INT IDENTITY(1,1) PRIMARY KEY,
        RazaoSocial   NVARCHAR(150)  NOT NULL,
        Cnpj          NVARCHAR(18)   NOT NULL,
        Email         NVARCHAR(150)  NOT NULL,
        Telefone      NVARCHAR(20)   NULL,
        Segmento      NVARCHAR(80)   NULL,
        Porte         NVARCHAR(30)   NULL,
        Endereco      NVARCHAR(200)  NULL,
        Cidade        NVARCHAR(100)  NULL,
        Estado        NVARCHAR(2)    NULL,
        DataCadastro  DATETIME       NOT NULL DEFAULT GETDATE(),

        CONSTRAINT UQ_Empresas_Cnpj UNIQUE (Cnpj)
    );

    PRINT 'Tabela Empresas criada com sucesso.';
END
ELSE
    PRINT 'Tabela Empresas já existe.';
GO

-- Dados de exemplo (opcional)
INSERT INTO Usuarios (Nome, Sobrenome, Email, Cpf, Telefone, Perfil, Senha)
VALUES ('João', 'Silva', 'joao@email.com', '123.456.789-00', '(11) 99999-0000', 'Administrador', 'senha_hash_aqui');

INSERT INTO Empresas (RazaoSocial, Cnpj, Email, Telefone, Segmento, Porte, Cidade, Estado)
VALUES ('Empresa Exemplo Ltda.', '00.000.000/0001-00', 'contato@empresa.com', '(11) 3000-0000', 'Tecnologia', 'Médio', 'São Paulo', 'SP');

PRINT 'Dados de exemplo inseridos.';
GO
