/* Script de Setup para o projeto POO2_SistemaDeEmpregados
   1. Abra este arquivo no Visual Studio
   2. Verifique se a conexão está apontando para (localdb)\MSSQLLocalDB
   3. Clique em "Executar" (botão verde de "Play")
*/

USE [master];
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'DBEmpregadoGUI')
BEGIN
    CREATE DATABASE [DBEmpregadoGUI];
    PRINT 'Banco de dados [DBEmpregadoGUI] criado.';
END
ELSE
BEGIN
    PRINT 'Banco de dados [DBEmpregadoGUI] já existe.';
END
GO

USE [DBEmpregadoGUI];
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Empregado' and xtype='U')
BEGIN
    CREATE TABLE [dbo].[Empregado] (
        [Matricula] INT            NOT NULL PRIMARY KEY IDENTITY(1,1),
        [CPF]       NVARCHAR (MAX) NULL,
        [Nome]      NVARCHAR (MAX) NULL,
        [Endereco]  NVARCHAR (MAX) NULL
    );
    PRINT 'Tabela [Empregado] criada.';
END
ELSE
BEGIN
    PRINT 'Tabela [Empregado] já existe.';
END
GO

PRINT 'Setup do banco de dados concluído.';