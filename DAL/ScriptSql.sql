USE [ingenieria]
GO

IF OBJECT_ID('dbo.Bitacora', 'U') IS NOT NULL DROP TABLE dbo.Bitacora
IF OBJECT_ID('dbo.Usuario',  'U') IS NOT NULL DROP TABLE dbo.Usuario
GO

IF OBJECT_ID('dbo.Login',                      'P') IS NOT NULL DROP PROCEDURE dbo.Login
IF OBJECT_ID('dbo.InsertarUsuario',            'P') IS NOT NULL DROP PROCEDURE dbo.InsertarUsuario
IF OBJECT_ID('dbo.IncrementarIntentosFallidos','P') IS NOT NULL DROP PROCEDURE dbo.IncrementarIntentosFallidos
IF OBJECT_ID('dbo.BloquearUsuario',            'P') IS NOT NULL DROP PROCEDURE dbo.BloquearUsuario
IF OBJECT_ID('dbo.DesbloquearUsuario',         'P') IS NOT NULL DROP PROCEDURE dbo.DesbloquearUsuario
IF OBJECT_ID('dbo.ActualizarUltimoLogin',      'P') IS NOT NULL DROP PROCEDURE dbo.ActualizarUltimoLogin
IF OBJECT_ID('dbo.ListarUsuarios',             'P') IS NOT NULL DROP PROCEDURE dbo.ListarUsuarios
IF OBJECT_ID('dbo.InsertarBitacora',           'P') IS NOT NULL DROP PROCEDURE dbo.InsertarBitacora
IF OBJECT_ID('dbo.ListarBitacora',             'P') IS NOT NULL DROP PROCEDURE dbo.ListarBitacora
IF OBJECT_ID('dbo.BackupBaseDeDatos',          'P') IS NOT NULL DROP PROCEDURE dbo.BackupBaseDeDatos
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
    CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Usuario_Username] UNIQUE ([Username])
)
GO

CREATE TABLE [dbo].[Bitacora](
    [Id]        [int]           IDENTITY(1,1) NOT NULL,
    [Tipo]      [int]           NULL,
    [UsuarioId] [int]           NOT NULL,
    [FechaHora] [datetime]      NOT NULL DEFAULT GETDATE(),
    [Detalle]   [nvarchar](MAX) NULL,
    CONSTRAINT [PK_Bitacora]        PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Bitacora_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([Id])
)
GO

CREATE PROCEDURE [dbo].[Login]
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, [Hash], [Salt], Nombre, Apellido, Bloqueado, IntentosFallidos, UltimoLogin
    FROM Usuario
    WHERE Username = @Username;
END
GO

CREATE PROCEDURE [dbo].[InsertarUsuario]
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

CREATE PROCEDURE [dbo].[IncrementarIntentosFallidos]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET IntentosFallidos = IntentosFallidos + 1 WHERE Id = @UsuarioId;
    SELECT IntentosFallidos FROM Usuario WHERE Id = @UsuarioId;
END
GO

CREATE PROCEDURE [dbo].[BloquearUsuario]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET Bloqueado = 1 WHERE Id = @UsuarioId;
END
GO

CREATE PROCEDURE [dbo].[DesbloquearUsuario]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET IntentosFallidos = 0, Bloqueado = 0 WHERE Id = @UsuarioId;
END
GO

CREATE PROCEDURE [dbo].[ActualizarUltimoLogin]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET UltimoLogin = GETDATE() WHERE Id = @UsuarioId;
END
GO

CREATE PROCEDURE [dbo].[ListarUsuarios]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, Nombre, Apellido, Bloqueado, IntentosFallidos, UltimoLogin
    FROM Usuario
    ORDER BY Bloqueado DESC, Username ASC;
END
GO

CREATE PROCEDURE [dbo].[InsertarBitacora]
    @UsuarioId INT,
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

CREATE PROCEDURE [dbo].[ListarBitacora]
    @Tipo      INT = NULL,
    @UsuarioId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT U.Id AS UsuarioId, U.Username, U.Nombre, U.Apellido,
           B.Tipo, B.Id, B.FechaHora, B.Detalle
    FROM Bitacora B
    INNER JOIN Usuario U ON U.Id = B.UsuarioId
    WHERE (@Tipo      IS NULL OR B.Tipo      = @Tipo)
      AND (@UsuarioId IS NULL OR B.UsuarioId = @UsuarioId)
    ORDER BY B.FechaHora DESC;
END
GO

CREATE PROCEDURE [dbo].[BackupBaseDeDatos]
    @RutaArchivo NVARCHAR(MAX)
AS
BEGIN
    DECLARE @SQL NVARCHAR(1000)
    SET @SQL = 'BACKUP DATABASE [ingenieria] TO DISK = ''' + @RutaArchivo + ''' WITH FORMAT'
    EXEC sp_executesql @SQL
END
GO

DBCC CHECKIDENT ('Usuario', RESEED, 0)
INSERT INTO Usuario (Username, Hash, Salt, Nombre, Apellido)
VALUES (
    'admin',
    '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
    'VQUk8CQ1S2V3oSbMWAp4qg==',
    'Admin',
    'Admin'
)
GO