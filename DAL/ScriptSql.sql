USE [ingenieria]
GO

IF OBJECT_ID('dbo.Bitacora',          'U') IS NOT NULL DROP TABLE dbo.Bitacora
IF OBJECT_ID('dbo.Usuario',           'U') IS NOT NULL DROP TABLE dbo.Usuario
IF OBJECT_ID('dbo.DigitoVerificador', 'U') IS NOT NULL DROP TABLE dbo.DigitoVerificador
GO

IF OBJECT_ID('dbo.Login',                               'P') IS NOT NULL DROP PROCEDURE dbo.Login
IF OBJECT_ID('dbo.InsertarUsuario',                     'P') IS NOT NULL DROP PROCEDURE dbo.InsertarUsuario
IF OBJECT_ID('dbo.IncrementarIntentosFallidos',         'P') IS NOT NULL DROP PROCEDURE dbo.IncrementarIntentosFallidos
IF OBJECT_ID('dbo.BloquearUsuario',                     'P') IS NOT NULL DROP PROCEDURE dbo.BloquearUsuario
IF OBJECT_ID('dbo.DesbloquearUsuario',                  'P') IS NOT NULL DROP PROCEDURE dbo.DesbloquearUsuario
IF OBJECT_ID('dbo.ActualizarUltimoLogin',               'P') IS NOT NULL DROP PROCEDURE dbo.ActualizarUltimoLogin
IF OBJECT_ID('dbo.ListarUsuarios',                      'P') IS NOT NULL DROP PROCEDURE dbo.ListarUsuarios
IF OBJECT_ID('dbo.ListarUsuariosParaVerificacion',      'P') IS NOT NULL DROP PROCEDURE dbo.ListarUsuariosParaVerificacion
IF OBJECT_ID('dbo.InsertarBitacora',                    'P') IS NOT NULL DROP PROCEDURE dbo.InsertarBitacora
IF OBJECT_ID('dbo.ListarBitacora',                      'P') IS NOT NULL DROP PROCEDURE dbo.ListarBitacora
IF OBJECT_ID('dbo.BackupBaseDeDatos',                   'P') IS NOT NULL DROP PROCEDURE dbo.BackupBaseDeDatos
IF OBJECT_ID('dbo.ActualizarDVHUsuario',                'P') IS NOT NULL DROP PROCEDURE dbo.ActualizarDVHUsuario
IF OBJECT_ID('dbo.ObtenerDVV',                          'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerDVV
IF OBJECT_ID('dbo.UpsertDVV',                           'P') IS NOT NULL DROP PROCEDURE dbo.UpsertDVV
GO

CREATE TABLE [dbo].[Usuario](
    [Id]               [int]           IDENTITY(1,1) NOT NULL,
    [Username]         [nvarchar](50)  NOT NULL,
    [Hash]             [nvarchar](255) NOT NULL,
    [Salt]             [nvarchar](50)  NOT NULL DEFAULT '',
    [Nombre]           [nvarchar](50)  NOT NULL,
    [Apellido]         [nvarchar](50)  NOT NULL,
    [IntentosFallidos] [int]           NOT NULL DEFAULT 0,
    [Bloqueado]        [bit]           NOT NULL DEFAULT 0,
    [UltimoLogin]      [datetime]      NULL,
    [DVH]              [nvarchar](64)  NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Usuario_Username] UNIQUE ([Username])
)
GO

CREATE TABLE [dbo].[Bitacora](
    [Id]        [int]           IDENTITY(1,1) NOT NULL,
    [Tipo]      [int]           NULL,
    [UsuarioId] [int]           NULL,
    [FechaHora] [datetime]      NOT NULL DEFAULT GETDATE(),
    [Detalle]   [nvarchar](MAX) NULL,
    CONSTRAINT [PK_Bitacora]        PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Bitacora_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([Id])
)
GO

CREATE TABLE [dbo].[DigitoVerificador](
    [NombreTabla] [nvarchar](50) NOT NULL,
    [DVV]         [nvarchar](64) NOT NULL,
    CONSTRAINT [PK_DigitoVerificador] PRIMARY KEY CLUSTERED ([NombreTabla] ASC)
)
GO

CREATE OR ALTER PROCEDURE [dbo].[Login]
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, [Hash], [Salt], Nombre, Apellido, Bloqueado, IntentosFallidos, UltimoLogin
    FROM Usuario
    WHERE Username = @Username;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarUsuario]
    @Username NVARCHAR(50),
    @Hash     NVARCHAR(255),
    @Salt     NVARCHAR(50),
    @Nombre   NVARCHAR(50),
    @Apellido NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Usuario ([Username], [Hash], [Salt], [Nombre], [Apellido])
    VALUES (@Username, @Hash, @Salt, @Nombre, @Apellido);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[IncrementarIntentosFallidos]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET IntentosFallidos = IntentosFallidos + 1 WHERE Id = @UsuarioId;
    SELECT IntentosFallidos FROM Usuario WHERE Id = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[BloquearUsuario]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET Bloqueado = 1 WHERE Id = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[DesbloquearUsuario]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET IntentosFallidos = 0, Bloqueado = 0 WHERE Id = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ActualizarUltimoLogin]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET UltimoLogin = GETDATE(), IntentosFallidos = 0 WHERE Id = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarUsuarios]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, Nombre, Apellido, IntentosFallidos, Bloqueado, UltimoLogin
    FROM Usuario
    ORDER BY Bloqueado DESC, Username ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarUsuariosParaVerificacion]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, [Hash], [Salt], Nombre, Apellido, IntentosFallidos, Bloqueado, UltimoLogin, DVH
    FROM Usuario
    ORDER BY Bloqueado DESC, Username ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarBitacora]
    @UsuarioId INT = NULL,
    @Tipo      INT,
    @Detalle   NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Bitacora (UsuarioId, Tipo, FechaHora, Detalle)
    VALUES (@UsuarioId, @Tipo, GETDATE(), @Detalle);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarBitacora]
    @Tipo      INT = NULL,
    @UsuarioId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT U.Id AS UsuarioId, U.Username, U.Nombre, U.Apellido,
           B.Tipo, B.Id, B.FechaHora, B.Detalle
    FROM Bitacora B
    LEFT JOIN Usuario U ON U.Id = B.UsuarioId
    WHERE (@Tipo IS NULL OR B.Tipo = @Tipo)
        AND (@UsuarioId IS NULL OR B.UsuarioId = @UsuarioId)
    ORDER BY B.FechaHora DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[BackupBaseDeDatos]
    @RutaArchivo NVARCHAR(MAX)
AS
BEGIN
    DECLARE @SQL NVARCHAR(1000)
    SET @SQL = 'BACKUP DATABASE [ingenieria] TO DISK = ''' + @RutaArchivo + ''' WITH FORMAT'
    EXEC sp_executesql @SQL
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ActualizarDVHUsuario]
    @UsuarioId INT,
    @DVH       NVARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET DVH = @DVH WHERE Id = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerDVV]
    @NombreTabla NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT NombreTabla, DVV FROM DigitoVerificador WHERE NombreTabla = @NombreTabla;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[UpsertDVV]
    @NombreTabla NVARCHAR(50),
    @DVV         NVARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM DigitoVerificador WHERE NombreTabla = @NombreTabla)
        UPDATE DigitoVerificador SET DVV = @DVV WHERE NombreTabla = @NombreTabla;
    ELSE
        INSERT INTO DigitoVerificador (NombreTabla, DVV) VALUES (@NombreTabla, @DVV);
END
GO

DBCC CHECKIDENT ('Usuario', RESEED, 1)
INSERT INTO Usuario (Username, Hash, Salt, Nombre, Apellido, DVH)
VALUES (
    'admin',
    '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
    'VQUk8CQ1S2V3oSbMWAp4qg==',
    'Admin',
    'Admin',
    NULL
)
GO