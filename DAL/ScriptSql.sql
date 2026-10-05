IF DB_ID('avanti') IS NULL CREATE DATABASE avanti;
GO

USE [avanti]
GO

-- 1) Drop SPs
DECLARE @sqlSp NVARCHAR(MAX) = N'';
SELECT @sqlSp = @sqlSp + 'DROP PROCEDURE [dbo].[' + p.name + '];' + CHAR(13) + CHAR(10)
FROM sys.procedures p
INNER JOIN sys.schemas s ON s.schema_id = p.schema_id
WHERE s.name = 'dbo';
IF LEN(@sqlSp) > 0 EXEC sp_executesql @sqlSp;
GO

-- 2) Drop FKs
IF OBJECT_ID('dbo.FK_Bitacora_Usuario','F')                  IS NOT NULL ALTER TABLE dbo.Bitacora         DROP CONSTRAINT FK_Bitacora_Usuario;
IF OBJECT_ID('dbo.FK_Usuario_Idioma','F')                    IS NOT NULL ALTER TABLE dbo.Usuario          DROP CONSTRAINT FK_Usuario_Idioma;
IF OBJECT_ID('dbo.FK_UsuarioPermiso_Usuario','F')            IS NOT NULL ALTER TABLE dbo.UsuarioPermiso   DROP CONSTRAINT FK_UsuarioPermiso_Usuario;
IF OBJECT_ID('dbo.FK_UsuarioPermiso_Permiso','F')            IS NOT NULL ALTER TABLE dbo.UsuarioPermiso   DROP CONSTRAINT FK_UsuarioPermiso_Permiso;
IF OBJECT_ID('dbo.FK_Permiso_Padre','F')                     IS NOT NULL ALTER TABLE dbo.Permiso          DROP CONSTRAINT FK_Permiso_Padre;
IF OBJECT_ID('dbo.FK_UsuarioHistorial_Usuario','F')          IS NOT NULL ALTER TABLE dbo.UsuarioHistorial DROP CONSTRAINT FK_UsuarioHistorial_Usuario;
IF OBJECT_ID('dbo.FK_UsuarioHistorial_ModificadoPor','F')    IS NOT NULL ALTER TABLE dbo.UsuarioHistorial DROP CONSTRAINT FK_UsuarioHistorial_ModificadoPor;
IF OBJECT_ID('dbo.FK_UsuarioHistorial_Restauracion','F')     IS NOT NULL ALTER TABLE dbo.UsuarioHistorial DROP CONSTRAINT FK_UsuarioHistorial_Restauracion;
IF OBJECT_ID('dbo.FK_Traduccion_Idioma','F')                 IS NOT NULL ALTER TABLE dbo.Traduccion       DROP CONSTRAINT FK_Traduccion_Idioma;
IF OBJECT_ID('dbo.FK_Traduccion_Control','F')                IS NOT NULL ALTER TABLE dbo.Traduccion       DROP CONSTRAINT FK_Traduccion_Control;
-- N01: hay que dropear estos FKs acá porque referencian Usuario y bloquean el drop de la tabla.
IF OBJECT_ID('dbo.FK_HistUnidad_Usuario','F')                IS NOT NULL ALTER TABLE dbo.HistorialEstadoUnidad DROP CONSTRAINT FK_HistUnidad_Usuario;
IF OBJECT_ID('dbo.FK_HistUnidad_Unidad','F')                 IS NOT NULL ALTER TABLE dbo.HistorialEstadoUnidad DROP CONSTRAINT FK_HistUnidad_Unidad;
IF OBJECT_ID('dbo.FK_ChecklistItem_Revisor','F')            IS NOT NULL ALTER TABLE dbo.ChecklistItem         DROP CONSTRAINT FK_ChecklistItem_Revisor;
IF OBJECT_ID('dbo.FK_ChecklistItem_Template','F')            IS NOT NULL ALTER TABLE dbo.ChecklistItem         DROP CONSTRAINT FK_ChecklistItem_Template;
IF OBJECT_ID('dbo.FK_ChecklistItem_Checklist','F')           IS NOT NULL ALTER TABLE dbo.ChecklistItem         DROP CONSTRAINT FK_ChecklistItem_Checklist;
IF OBJECT_ID('dbo.FK_Checklist_Creador','F')                 IS NOT NULL ALTER TABLE dbo.ChecklistPreparacion  DROP CONSTRAINT FK_Checklist_Creador;
IF OBJECT_ID('dbo.FK_Checklist_Unidad','F')                  IS NOT NULL ALTER TABLE dbo.ChecklistPreparacion  DROP CONSTRAINT FK_Checklist_Unidad;
IF OBJECT_ID('dbo.FK_Unidad_Comprador','F')                  IS NOT NULL ALTER TABLE dbo.Unidad                DROP CONSTRAINT FK_Unidad_Comprador;
IF OBJECT_ID('dbo.FK_Unidad_Persona','F')                   IS NOT NULL ALTER TABLE dbo.Unidad                DROP CONSTRAINT FK_Unidad_Persona;
IF OBJECT_ID('dbo.FK_Modelo_Marca','F')                      IS NOT NULL ALTER TABLE dbo.Modelo                DROP CONSTRAINT FK_Modelo_Marca;
IF OBJECT_ID('dbo.FK_Publicacion_Unidad','F')                IS NOT NULL ALTER TABLE dbo.PublicacionUnidad      DROP CONSTRAINT FK_Publicacion_Unidad;
IF OBJECT_ID('dbo.FK_ImagenUnidad_Unidad','F')               IS NOT NULL ALTER TABLE dbo.ImagenUnidad           DROP CONSTRAINT FK_ImagenUnidad_Unidad;
IF OBJECT_ID('dbo.FK_VentaUnidad_Unidad','F')                IS NOT NULL ALTER TABLE dbo.VentaUnidad            DROP CONSTRAINT FK_VentaUnidad_Unidad;
IF OBJECT_ID('dbo.FK_VentaUnidad_Comprador','F')             IS NOT NULL ALTER TABLE dbo.VentaUnidad            DROP CONSTRAINT FK_VentaUnidad_Comprador;
IF OBJECT_ID('dbo.FK_VentaUnidad_Vendedor','F')              IS NOT NULL ALTER TABLE dbo.VentaUnidad            DROP CONSTRAINT FK_VentaUnidad_Vendedor;
GO

-- 3) Drop tablas
-- N01 primero (dependen de Usuario).
IF OBJECT_ID('dbo.VentaUnidad','U')            IS NOT NULL DROP TABLE dbo.VentaUnidad;
IF OBJECT_ID('dbo.ImagenUnidad','U')           IS NOT NULL DROP TABLE dbo.ImagenUnidad;
IF OBJECT_ID('dbo.PublicacionUnidad','U')      IS NOT NULL DROP TABLE dbo.PublicacionUnidad;
IF OBJECT_ID('dbo.HistorialEstadoUnidad','U')  IS NOT NULL DROP TABLE dbo.HistorialEstadoUnidad;
IF OBJECT_ID('dbo.ChecklistItem','U')          IS NOT NULL DROP TABLE dbo.ChecklistItem;
IF OBJECT_ID('dbo.ChecklistPreparacion','U')   IS NOT NULL DROP TABLE dbo.ChecklistPreparacion;
IF OBJECT_ID('dbo.ChecklistItemTemplate','U')  IS NOT NULL DROP TABLE dbo.ChecklistItemTemplate;
IF OBJECT_ID('dbo.Unidad','U')                 IS NOT NULL DROP TABLE dbo.Unidad;
IF OBJECT_ID('dbo.Persona','U')               IS NOT NULL DROP TABLE dbo.Persona;
IF OBJECT_ID('dbo.Modelo','U')                 IS NOT NULL DROP TABLE dbo.Modelo;
IF OBJECT_ID('dbo.Marca','U')                  IS NOT NULL DROP TABLE dbo.Marca;
-- Base.
IF OBJECT_ID('dbo.UsuarioHistorial','U')  IS NOT NULL DROP TABLE dbo.UsuarioHistorial;
IF OBJECT_ID('dbo.UsuarioPermiso','U')    IS NOT NULL DROP TABLE dbo.UsuarioPermiso;
IF OBJECT_ID('dbo.Bitacora','U')          IS NOT NULL DROP TABLE dbo.Bitacora;
IF OBJECT_ID('dbo.DigitoVerificador','U') IS NOT NULL DROP TABLE dbo.DigitoVerificador;
IF OBJECT_ID('dbo.Permiso','U')           IS NOT NULL DROP TABLE dbo.Permiso;
IF OBJECT_ID('dbo.Traduccion','U')        IS NOT NULL DROP TABLE dbo.Traduccion;
IF OBJECT_ID('dbo.Control','U')           IS NOT NULL DROP TABLE dbo.Control;
IF OBJECT_ID('dbo.Usuario','U')           IS NOT NULL DROP TABLE dbo.Usuario;
IF OBJECT_ID('dbo.Idioma','U')            IS NOT NULL DROP TABLE dbo.Idioma;
GO

CREATE TABLE [dbo].[Idioma](
    [Id]     [int]          IDENTITY(1,1) NOT NULL,
    [Nombre] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_Idioma]        PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Idioma_Nombre] UNIQUE ([Nombre])
)
GO

CREATE TABLE [dbo].[Control](
    [Id]     [int]          IDENTITY(1,1) NOT NULL,
    [Codigo] [nvarchar](80) NOT NULL,
    [Form]   [nvarchar](80) NOT NULL,
    CONSTRAINT [PK_Control]            PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Control_Codigo_Form] UNIQUE ([Codigo], [Form])
)
GO

CREATE TABLE [dbo].[Traduccion](
    [Id]        [int]            IDENTITY(1,1) NOT NULL,
    [IdIdioma]  [int]            NOT NULL,
    [IdControl] [int]            NOT NULL,
    [Texto]     [nvarchar](1000) NOT NULL,
    CONSTRAINT [PK_Traduccion]            PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Traduccion_Idioma]     FOREIGN KEY ([IdIdioma])  REFERENCES [dbo].[Idioma] ([Id])  ON DELETE CASCADE,
    CONSTRAINT [FK_Traduccion_Control]    FOREIGN KEY ([IdControl]) REFERENCES [dbo].[Control] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_Traduccion_Idioma_Control] UNIQUE ([IdIdioma], [IdControl])
)
GO

CREATE TABLE [dbo].[Usuario](
    [Id]               [int]           IDENTITY(1,1) NOT NULL,
    [Username]         [nvarchar](50)  NOT NULL,
    [Hash]             [nvarchar](255) NOT NULL,
    [Salt]             [nvarchar](50)  NOT NULL DEFAULT '',
    [Nombre]           [nvarchar](50)  NOT NULL,
    [Apellido]         [nvarchar](50)  NOT NULL,
    [Email]            [nvarchar](255) NOT NULL,
    [Telefono]         [nvarchar](30)  NOT NULL,
    [Documento]        [nvarchar](30)  NOT NULL,
    [Domicilio]        [nvarchar](200) NOT NULL,
    [IntentosFallidos] [int]           NOT NULL DEFAULT 0,
    [Bloqueado]        [bit]           NOT NULL DEFAULT 0,
    [UltimoLogin]      [datetime]      NULL,
    [IdIdioma]         [int]           NULL,
    [DVH]              [nvarchar](64)  NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Usuario_Username] UNIQUE ([Username]),
    CONSTRAINT [FK_Usuario_Idioma]   FOREIGN KEY ([IdIdioma]) REFERENCES [dbo].[Idioma] ([Id])
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

CREATE TABLE [dbo].[Permiso](
    [Id]          [int]           IDENTITY(1,1) NOT NULL,
    [Codigo]      [nvarchar](50)  NOT NULL,
    [Descripcion] [nvarchar](255) NULL,
    [Tipo]        [char](1)       NOT NULL,
    [IdPadre]     [int]           NULL,
    [DVH]         [nvarchar](64)  NULL,
    CONSTRAINT [PK_Permiso]        PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Permiso_Codigo] UNIQUE ([Codigo]),
    CONSTRAINT [CK_Permiso_Tipo]   CHECK ([Tipo] IN ('I','R')),
    CONSTRAINT [FK_Permiso_Padre]  FOREIGN KEY ([IdPadre]) REFERENCES [dbo].[Permiso] ([Id])
)
GO

CREATE TABLE [dbo].[UsuarioPermiso](
    [UsuarioId] [int] NOT NULL,
    [PermisoId] [int] NOT NULL,
    CONSTRAINT [PK_UsuarioPermiso]         PRIMARY KEY CLUSTERED ([UsuarioId], [PermisoId]),
    CONSTRAINT [FK_UsuarioPermiso_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UsuarioPermiso_Permiso] FOREIGN KEY ([PermisoId]) REFERENCES [dbo].[Permiso] ([Id])
)
GO

CREATE TABLE [dbo].[UsuarioHistorial](
    [Id]                     [int]            IDENTITY(1,1) NOT NULL,
    [UsuarioId]              [int]            NOT NULL,
    [FechaHora]              [datetime]       NOT NULL DEFAULT GETDATE(),
    [Accion]                 [nvarchar](40)   NOT NULL,
    [ModificadoPorUsuarioId] [int]            NULL,
    [RestauracionId]         [int]            NULL,
    [Nombre]                 [nvarchar](50)   NOT NULL,
    [Apellido]               [nvarchar](50)   NOT NULL,
    [Email]                  [nvarchar](255)  NOT NULL,
    [Telefono]               [nvarchar](30)   NOT NULL,
    [Documento]              [nvarchar](30)   NOT NULL,
    [Domicilio]              [nvarchar](200)  NOT NULL,
    [Bloqueado]              [bit]            NOT NULL,
    CONSTRAINT [PK_UsuarioHistorial]                    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UsuarioHistorial_Usuario]            FOREIGN KEY ([UsuarioId])              REFERENCES [dbo].[Usuario] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UsuarioHistorial_ModificadoPor]      FOREIGN KEY ([ModificadoPorUsuarioId]) REFERENCES [dbo].[Usuario] ([Id]),
    CONSTRAINT [FK_UsuarioHistorial_Restauracion]       FOREIGN KEY ([RestauracionId])         REFERENCES [dbo].[UsuarioHistorial] ([Id])
)
GO

CREATE OR ALTER PROCEDURE [dbo].[Login]
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, [Hash], [Salt], Nombre, Apellido, Email, Telefono, Documento, Domicilio,
           Bloqueado, IntentosFallidos, UltimoLogin, IdIdioma
    FROM Usuario
    WHERE Username = @Username;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarUsuario]
    @Username  NVARCHAR(50),
    @Hash      NVARCHAR(255),
    @Salt      NVARCHAR(50),
    @Nombre    NVARCHAR(50),
    @Apellido  NVARCHAR(50),
    @Email     NVARCHAR(255),
    @Telefono  NVARCHAR(30),
    @Documento NVARCHAR(30),
    @Domicilio NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Usuario ([Username], [Hash], [Salt], [Nombre], [Apellido], [Email], [Telefono], [Documento], [Domicilio])
    VALUES (@Username, @Hash, @Salt, @Nombre, @Apellido, @Email, @Telefono, @Documento, @Domicilio);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EditarUsuario]
    @UsuarioId INT,
    @Username  NVARCHAR(50),
    @Nombre    NVARCHAR(50),
    @Apellido  NVARCHAR(50),
    @Email     NVARCHAR(255),
    @Telefono  NVARCHAR(30),
    @Documento NVARCHAR(30),
    @Domicilio NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario
       SET Username  = @Username,
           Nombre    = @Nombre,
           Apellido  = @Apellido,
           Email     = @Email,
           Telefono  = @Telefono,
           Documento = @Documento,
           Domicilio = @Domicilio
     WHERE Id = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerUsuarioPorId]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, [Hash], [Salt], Nombre, Apellido, Email, Telefono, Documento, Domicilio,
           Bloqueado, IntentosFallidos, UltimoLogin, IdIdioma, DVH
    FROM Usuario
    WHERE Id = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarUsuarioHistorial]
    @UsuarioId              INT,
    @Accion                 NVARCHAR(40),
    @ModificadoPorUsuarioId INT             = NULL,
    @RestauracionId         INT             = NULL,
    @Nombre                 NVARCHAR(50),
    @Apellido               NVARCHAR(50),
    @Email                  NVARCHAR(255),
    @Telefono               NVARCHAR(30),
    @Documento              NVARCHAR(30),
    @Domicilio              NVARCHAR(200),
    @Bloqueado              BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO UsuarioHistorial
        (UsuarioId, Accion, ModificadoPorUsuarioId, RestauracionId,
         Nombre, Apellido, Email, Telefono, Documento, Domicilio,
         Bloqueado)
    VALUES
        (@UsuarioId, @Accion, @ModificadoPorUsuarioId, @RestauracionId,
         @Nombre, @Apellido, @Email, @Telefono, @Documento, @Domicilio,
         @Bloqueado);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarUsuarioHistorial]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT H.Id, H.UsuarioId, H.FechaHora, H.Accion, H.ModificadoPorUsuarioId,
           U.Username AS ModificadoPorUsername,
           H.RestauracionId,
           HS.FechaHora AS RestauracionFechaHora,
           H.Nombre, H.Apellido, H.Email, H.Telefono, H.Documento, H.Domicilio,
           H.Bloqueado
    FROM UsuarioHistorial H
    LEFT JOIN Usuario U           ON U.Id  = H.ModificadoPorUsuarioId
    LEFT JOIN UsuarioHistorial HS ON HS.Id = H.RestauracionId
    WHERE H.UsuarioId = @UsuarioId
    ORDER BY H.FechaHora DESC, H.Id DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerUsuarioHistorialPorId]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT H.Id, H.UsuarioId, H.FechaHora, H.Accion, H.ModificadoPorUsuarioId,
           U.Username AS ModificadoPorUsername,
           H.RestauracionId,
           HS.FechaHora AS RestauracionFechaHora,
           H.Nombre, H.Apellido, H.Email, H.Telefono, H.Documento, H.Domicilio,
           H.Bloqueado
    FROM UsuarioHistorial H
    LEFT JOIN Usuario U           ON U.Id  = H.ModificadoPorUsuarioId
    LEFT JOIN UsuarioHistorial HS ON HS.Id = H.RestauracionId
    WHERE H.Id = @Id;
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
    SELECT Id, Username, Nombre, Apellido, Email, Telefono, Documento, Domicilio,
           IntentosFallidos, Bloqueado, UltimoLogin
    FROM Usuario
    ORDER BY Bloqueado DESC, Username ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarUsuariosParaVerificacion]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, [Hash], [Salt], Nombre, Apellido, Email, Telefono, Documento, Domicilio,
           IntentosFallidos, Bloqueado, UltimoLogin, DVH
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
    SELECT U.Id AS UsuarioId, U.Username, U.Nombre, U.Apellido, u.Email,
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
    SET @SQL = 'BACKUP DATABASE [avanti] TO DISK = ''' + @RutaArchivo + ''' WITH FORMAT'
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

CREATE OR ALTER PROCEDURE [dbo].[ListarPermisos]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Codigo, Descripcion, Tipo, IdPadre, DVH
    FROM Permiso
    ORDER BY Tipo DESC, Codigo ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarPermiso]
    @Codigo      NVARCHAR(50),
    @Descripcion NVARCHAR(255) = NULL,
    @Tipo        CHAR(1)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Permiso (Codigo, Descripcion, Tipo)
    VALUES (@Codigo, @Descripcion, @Tipo);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[AgregarHijoPermiso]
    @IdPadre INT,
    @IdHijo  INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @IdPadre = @IdHijo
    BEGIN
        RAISERROR ('Un permiso no puede contenerse a sí mismo.', 16, 1);
        RETURN;
    END

    UPDATE Permiso SET IdPadre = @IdPadre WHERE Id = @IdHijo;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[QuitarHijoPermiso]
    @IdHijo INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Permiso SET IdPadre = NULL WHERE Id = @IdHijo;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ActualizarDVHPermiso]
    @PermisoId INT,
    @DVH       NVARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Permiso SET DVH = @DVH WHERE Id = @PermisoId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarPermisosDirectosDeUsuario]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.Id, P.Codigo, P.Descripcion, P.Tipo, P.IdPadre, P.DVH
    FROM Permiso P
    INNER JOIN UsuarioPermiso UP ON UP.PermisoId = P.Id
    WHERE UP.UsuarioId = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[AsignarPermisoAUsuario]
    @UsuarioId INT,
    @PermisoId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM UsuarioPermiso WHERE UsuarioId = @UsuarioId AND PermisoId = @PermisoId)
        INSERT INTO UsuarioPermiso (UsuarioId, PermisoId) VALUES (@UsuarioId, @PermisoId);
END
GO

CREATE OR ALTER PROCEDURE [dbo].[QuitarPermisoDeUsuario]
    @UsuarioId INT,
    @PermisoId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM UsuarioPermiso WHERE UsuarioId = @UsuarioId AND PermisoId = @PermisoId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EliminarRol]
    @PermisoId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Permiso SET IdPadre = NULL WHERE IdPadre = @PermisoId;
    DELETE FROM UsuarioPermiso WHERE PermisoId = @PermisoId;
    DELETE FROM Permiso WHERE Id = @PermisoId AND Tipo = 'R';
END
GO

DBCC CHECKIDENT ('Usuario', RESEED, 1)
INSERT INTO Usuario (Username, Hash, Salt, Nombre, Apellido, Email, Telefono, Documento, Domicilio, DVH)
VALUES (
    'admin',
    '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
    'VQUk8CQ1S2V3oSbMWAp4qg==',
    'Admin',
    'Admin',
    'admin@avanti.com',
    '+54 11 5555-0001',
    '99999999',
    'Av. Siempre Viva 742, CABA',
    NULL
),
(
    'comprador',
    '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
    'VQUk8CQ1S2V3oSbMWAp4qg==',
    'Comprador',
    'Comercial',
    'comprador@avanti.com',
    '+54 11 5555-0002',
    '30123456',
    'Calle Falsa 123, CABA',
    NULL
),
(
    'taller',
    '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
    'VQUk8CQ1S2V3oSbMWAp4qg==',
    'Operador',
    'Taller',
    'taller@avanti.com',
    '+54 11 5555-0003',
    '30123457',
    'Av. Corrientes 1234, CABA',
    NULL
)
GO

-- Snapshot inicial (Alta) en historial para los usuarios seeded
INSERT INTO UsuarioHistorial
    (UsuarioId, Accion, ModificadoPorUsuarioId,
     Nombre, Apellido, Email, Telefono, Documento, Domicilio, Bloqueado)
SELECT Id, N'Alta', NULL,
       Nombre, Apellido, Email, Telefono, Documento, Domicilio, Bloqueado
FROM Usuario
WHERE Username IN ('admin', 'comprador', 'taller');
GO

-- Permisos fijos que se crean cuando se desarrolla un nuevo módulo o funcionalidad.
INSERT INTO Permiso (Codigo, Descripcion, Tipo) VALUES
    ('VER_BITACORA',       'Habilita el menú Bitácora y el acceso al módulo.',          'I'),
    ('GESTIONAR_USUARIOS', 'Habilita la gestión de usuarios (listado, bloqueo, etc.).', 'I'),
    ('REGISTRAR_USUARIO',  'Habilita el registro de nuevos usuarios.',                  'I'),
    ('EDITAR_USUARIO',     'Habilita la edición de datos de usuarios existentes.',      'I'),
    ('VER_HISTORIAL_USUARIO', 'Habilita ver el historial de cambios de un usuario.',    'I'),
    ('ASIGNAR_PERMISOS',   'Habilita la asignación de permisos a usuarios.',            'I'),
    ('GESTIONAR_PERMISOS', 'Habilita la gestión de permisos y roles (alta, jerarquía).', 'I'),
    ('GESTIONAR_IDIOMAS',  'Habilita la gestión de idiomas y traducciones.',             'I')
GO

-- Roles seed y asignaciones a usuarios seed.
INSERT INTO Permiso (Codigo, Descripcion, Tipo) VALUES
    ('COMPRADOR', 'Rol del área comercial: registra unidades ingresadas.', 'R'),
    ('TALLER',    'Rol del taller: revisa unidades y ejecuta preparación.', 'R')
GO

-- Hijos de los roles base del admin bypass (patrón Composite: IdPadre del permiso individual).
-- Nota: el seed de N01/N02 más abajo reasigna COMPRADOR/TALLER/GERENTE/VENDEDOR a sus
-- permisos del dominio. Los permisos de administración de usuarios/bitácora quedan sólo
-- para admin (admin bypass en UsuarioService.RequerirAdminOPermiso).
GO

-- Asignación de roles a usuarios seed.
INSERT INTO UsuarioPermiso (UsuarioId, PermisoId)
SELECT u.Id, p.Id FROM Usuario u, Permiso p
WHERE u.Username = 'comprador' AND p.Codigo = 'COMPRADOR';

INSERT INTO UsuarioPermiso (UsuarioId, PermisoId)
SELECT u.Id, p.Id FROM Usuario u, Permiso p
WHERE u.Username = 'taller' AND p.Codigo = 'TALLER';
GO

-- =========================================================
-- MULTI-IDIOMA (patrón Observer) — SPs
-- =========================================================

CREATE OR ALTER PROCEDURE [dbo].[ListarIdiomas]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre FROM Idioma ORDER BY Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarIdioma]
    @Nombre NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Idioma (Nombre) VALUES (@Nombre);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EliminarIdioma]
    @IdiomaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Idioma WHERE Id = @IdiomaId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarControles]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Codigo, Form FROM Control ORDER BY Form ASC, Codigo ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarControl]
    @Codigo NVARCHAR(80),
    @Form   NVARCHAR(80)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Control (Codigo, Form) VALUES (@Codigo, @Form);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarTraducciones]
    @IdiomaId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT T.Id, T.IdIdioma, T.IdControl, T.Texto, C.Codigo, C.Form
    FROM Traduccion T
    INNER JOIN Control C ON C.Id = T.IdControl
    WHERE (@IdiomaId IS NULL OR T.IdIdioma = @IdiomaId);
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarTraduccion]
    @IdIdioma  INT,
    @IdControl INT,
    @Texto     NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Traduccion WHERE IdIdioma = @IdIdioma AND IdControl = @IdControl)
        UPDATE Traduccion SET Texto = @Texto WHERE IdIdioma = @IdIdioma AND IdControl = @IdControl;
    ELSE
        INSERT INTO Traduccion (IdIdioma, IdControl, Texto) VALUES (@IdIdioma, @IdControl, @Texto);
END
GO

-- Idiomas seed
INSERT INTO Idioma (Nombre) VALUES ('Español'), ('English'), ('Deutsch')
GO

-- ============================================================
-- FormPrincipal — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formPpal NVARCHAR(80) = N'FormPrincipal';
DECLARE @ctrlPpal TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlPpal (Codigo, Es, En, De) VALUES
    -- Sesión / estado
    (N'lblSesion',                N'Sesión',                                              N'Session',                                                   N'Sitzung'),
    (N'tooltipSesionActiva',      N'Sesión activa',                                       N'Active session',                                            N'Aktive Sitzung'),
    -- Menú top-level
    (N'menuInicio',               N'Inicio',                                              N'Home',                                                      N'Startseite'),
    (N'menuUsuarios',             N'Usuarios',                                            N'Users',                                                     N'Benutzer'),
    (N'menuPermisos',             N'Permisos',                                            N'Permissions',                                               N'Berechtigungen'),
    (N'menuBitacora',             N'Bitácora',                                            N'Audit Log',                                                 N'Protokoll'),
    (N'menuIdiomas',              N'Idiomas',                                             N'Languages',                                                 N'Sprachen'),
    (N'menuSeleccionIdioma',      N'Idioma ▾',                                            N'Language ▾',                                                N'Sprache ▾'),
    (N'menuMantenimiento',        N'Mantenimiento',                                       N'Maintenance',                                               N'Wartung'),
    (N'menuLogout',               N'Cerrar Sesión',                                       N'Logout',                                                    N'Abmelden'),
    -- N01
    (N'menuUnidades',             N'Unidades',                                            N'Units',                                                     N'Einheiten'),
    (N'menuRegistrarUnidad',      N'Registrar Unidad',                                    N'Register Unit',                                             N'Einheit registrieren'),
    (N'menuVerUnidades',          N'Ver Unidades',                                        N'View Units',                                                N'Einheiten ansehen'),
    (N'menuTemplateChecklist',    N'Template de Checklist',                               N'Checklist Template',                                        N'Checklisten-Vorlage'),
    (N'menuPublicaciones',        N'Publicaciones',                                       N'Listings',                                                  N'Inserate'),
    (N'menuVerPublicaciones',     N'Ver Publicaciones',                                   N'View Listings',                                             N'Inserate ansehen'),
    -- Submenús
    (N'menuInsertarUsuario',      N'Registrar Usuario',                                   N'Register User',                                             N'Benutzer registrieren'),
    (N'menuVerUsuarios',          N'Gestión de Usuarios',                                 N'Manage Users',                                              N'Benutzerverwaltung'),
    (N'menuGestionPermisos',      N'Gestión de Permisos',                                 N'Manage Permissions',                                        N'Berechtigungen verwalten'),
    (N'menuAsignacionPermisos',   N'Asignación de Permisos a Usuarios',                   N'Assign Permissions to Users',                               N'Berechtigungen Benutzern zuweisen'),
    (N'menuVerBitacora',          N'Ver Bitácora',                                        N'View Audit Log',                                            N'Protokoll ansehen'),
    (N'menuGestionIdiomas',       N'Gestión de Idiomas',                                  N'Manage Languages',                                          N'Sprachen verwalten'),
    -- Items dinámicos del selector de idioma
    (N'itemSinIdiomas',           N'(sin idiomas disponibles)',                           N'(no languages available)',                                  N'(keine Sprachen verfügbar)'),
    (N'tooltipIdiomaEnProceso',   N'Idioma en proceso de creación: faltan traducciones.', N'Language in progress: translations missing.',               N'Sprache in Bearbeitung: Übersetzungen fehlen.'),
    -- MessageBoxes
    (N'msgConfirmarLogout',       N'¿Querés cerrar sesión?',                              N'Do you want to log out?',                                   N'Möchtest du dich abmelden?'),
    (N'msgConfirmarLogoutTitulo', N'Confirmar cierre de sesión',                          N'Confirm logout',                                            N'Abmeldung bestätigen'),
    (N'msgErrorCambiarIdioma',    N'Error al cambiar idioma:',                            N'Error changing language:',                                  N'Fehler beim Sprachwechsel:'),
    (N'msgError',                 N'Error',                                               N'Error',                                                     N'Fehler'),
    (N'msgMantenimientoErrorVerificar', N'No se pudo verificar la integridad antes de abrir mantenimiento.', N'Could not verify integrity before opening maintenance.', N'Integrität konnte vor dem Öffnen der Wartung nicht überprüft werden.'),
    (N'msgMantenimientoLogoutForzado',  N'Se realizó una restauración de la base de datos. Por seguridad la sesión se cerrará y volverás al login.', N'A database restore was performed. For security the session will be closed and you will return to the login screen.', N'Die Datenbank wurde wiederhergestellt. Aus Sicherheitsgründen wird die Sitzung beendet und du kehrst zur Anmeldung zurück.'),
    (N'msgMantenimientoLogoutForzadoTitulo', N'Sesión cerrada',                                 N'Session closed',                                       N'Sitzung beendet');

INSERT INTO Control (Codigo, Form)
SELECT Codigo, @formPpal FROM @ctrlPpal;

DECLARE @idEsPpal INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnPpal INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDePpal INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsPpal, c.Id, t.Es
FROM @ctrlPpal t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPpal
UNION ALL
SELECT @idEnPpal, c.Id, t.En
FROM @ctrlPpal t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPpal
UNION ALL
SELECT @idDePpal, c.Id, t.De
FROM @ctrlPpal t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPpal;
GO

-- ============================================================
-- FormBitacora — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @form NVARCHAR(80) = N'FormBitacora';
DECLARE @ctrl TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrl (Codigo, Es, En, De) VALUES
    -- Designer (labels, group, buttons)
    (N'lblTitulo',         N'Bitácora',                                       N'Audit Log',                                          N'Protokoll'),
    (N'grpFiltros',        N'Filtros',                                        N'Filters',                                            N'Filter'),
    (N'lblTipo',           N'Tipo:',                                          N'Type:',                                              N'Typ:'),
    (N'lblUsuarioFiltro',  N'Usuario:',                                       N'User:',                                              N'Benutzer:'),
    (N'lblDesde',          N'Desde:',                                         N'From:',                                              N'Von:'),
    (N'lblHasta',          N'Hasta:',                                         N'To:',                                                N'Bis:'),
    (N'btnLimpiar',        N'Limpiar',                                        N'Clear',                                              N'Löschen'),
    (N'lblTotal',          N'Total: {0}',                                     N'Total: {0}',                                         N'Gesamt: {0}'),
    (N'btnActualizar',     N'Actualizar',                                     N'Refresh',                                            N'Aktualisieren'),
    -- Items y headers dinámicos
    (N'cmbTipoTodos',      N'Todos',                                          N'All',                                                N'Alle'),
    (N'colId',             N'ID',                                             N'ID',                                                 N'ID'),
    (N'colUsuario',        N'Usuario',                                        N'User',                                               N'Benutzer'),
    (N'colTipo',           N'Tipo',                                           N'Type',                                               N'Typ'),
    (N'colFechaHora',      N'Fecha y Hora',                                   N'Date and Time',                                      N'Datum und Uhrzeit'),
    (N'colDetalle',        N'Detalle',                                        N'Detail',                                             N'Details'),
    -- MessageBoxes
    (N'msgSinPermiso',     N'No tenés permiso para acceder a esta sección.',  N'You don''t have permission to access this section.', N'Du hast keine Berechtigung für diesen Bereich.'),
    (N'msgAccesoDenegado', N'Acceso denegado',                                N'Access denied',                                      N'Zugriff verweigert'),
    (N'msgErrorCargar',    N'Error al cargar la bitácora:',                   N'Error loading audit log:',                           N'Fehler beim Laden des Protokolls:'),
    (N'msgError',          N'Error',                                          N'Error',                                              N'Fehler');

INSERT INTO Control (Codigo, Form)
SELECT Codigo, @form FROM @ctrl;

DECLARE @idEsBit INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnBit INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeBit INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsBit, c.Id, t.Es
FROM @ctrl t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @form
UNION ALL
SELECT @idEnBit, c.Id, t.En
FROM @ctrl t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @form
UNION ALL
SELECT @idDeBit, c.Id, t.De
FROM @ctrl t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @form;
GO

-- ============================================================
-- FormHome — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formHome NVARCHAR(80) = N'FormHome';
DECLARE @ctrlHome TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlHome (Codigo, Es, En, De) VALUES
    (N'lblTitulo',          N'Inicio',                                              N'Home',                                            N'Startseite'),
    (N'lblNovedadesTitulo', N'Novedades',                                           N'What''s New',                                     N'Neuigkeiten'),
    (N'lblNovedadesBody',   N'Bienvenido al sistema',                               N'Welcome to the system',                           N'Willkommen im System'),
    (N'lblAccesosTitulo',   N'Accesos rápidos',                                     N'Quick Access',                                    N'Schnellzugriff'),
    (N'btnVerUnidades',     N'Ver Unidades',                                        N'View Units',                                      N'Einheiten anzeigen'),
    (N'btnVerPublicaciones',N'Ver Publicaciones',                                   N'View Listings',                                   N'Inserate anzeigen'),
    (N'btnAyuda',           N'Ayuda',                                             N'Help',                                            N'Hilfe'),
    (N'btnContacto',        N'Contactános',                                         N'Contact us',                                      N'Kontaktiere uns'),
    (N'msgProximamente',    N'Próximamente',                                        N'Coming soon',                                     N'Demnächst'),
    (N'msgAyudaTitulo',     N'Ayuda',                                               N'Help',                                            N'Hilfe'),
    (N'msgErrorMail',       N'No se pudo abrir el cliente de correo.',              N'Could not open mail client.',                     N'E-Mail-Programm konnte nicht geöffnet werden.'),
    (N'msgErrorMailEnvia',  N'Enviá un mail a',                                     N'Send an email to',                                N'Sende eine E-Mail an'),
    (N'msgContactoTitulo',  N'Contacto',                                            N'Contact',                                         N'Kontakt');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formHome FROM @ctrlHome;

DECLARE @idEsHome INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnHome INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeHome INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsHome, c.Id, t.Es FROM @ctrlHome t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formHome
UNION ALL
SELECT @idEnHome, c.Id, t.En FROM @ctrlHome t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formHome
UNION ALL
SELECT @idDeHome, c.Id, t.De FROM @ctrlHome t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formHome;
GO

-- ============================================================
-- FormUsuarios — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formUsr NVARCHAR(80) = N'FormUsuarios';
DECLARE @ctrlUsr TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlUsr (Codigo, Es, En, De) VALUES
    (N'lblTitulo',           N'Gestión de Usuarios',                                  N'Manage Users',                                          N'Benutzerverwaltung'),
    (N'grpFiltros',          N'Filtros',                                              N'Filters',                                               N'Filter'),
    (N'lblEstado',           N'Estado:',                                              N'Status:',                                               N'Status:'),
    (N'lblUsuarioFiltro',    N'Buscar usuario:',                                      N'Search user:',                                          N'Benutzer suchen:'),
    (N'btnLimpiar',          N'Limpiar',                                              N'Clear',                                                 N'Löschen'),
    (N'lblHint',             N'← Seleccioná un usuario',                              N'← Select a user',                                       N'← Wähle einen Benutzer aus'),
    (N'btnEditar',           N'Editar',                                               N'Edit',                                                  N'Bearbeiten'),
    (N'btnHistorial',        N'Historial',                                            N'History',                                               N'Verlauf'),
    (N'btnBloquear',         N'Bloquear',                                             N'Block',                                                 N'Sperren'),
    (N'btnDesbloquear',      N'Desbloquear',                                          N'Unblock',                                               N'Entsperren'),
    (N'lblTotal',            N'Total: {0}',                                           N'Total: {0}',                                            N'Gesamt: {0}'),
    (N'lblBloqueados',       N'Bloqueados: {0}',                                      N'Blocked: {0}',                                          N'Gesperrt: {0}'),
    (N'lblActivos',          N'Activos: {0}',                                         N'Active: {0}',                                           N'Aktiv: {0}'),
    (N'itemTodos',           N'Todos',                                                N'All',                                                   N'Alle'),
    (N'itemActivos',         N'Activos',                                              N'Active',                                                N'Aktiv'),
    (N'itemBloqueados',      N'Bloqueados',                                           N'Blocked',                                               N'Gesperrt'),
    (N'colId',               N'ID',                                                   N'ID',                                                    N'ID'),
    (N'colUsername',         N'Usuario',                                              N'User',                                                  N'Benutzer'),
    (N'colNombre',           N'Nombre',                                               N'First Name',                                            N'Vorname'),
    (N'colApellido',         N'Apellido',                                             N'Last Name',                                             N'Nachname'),
    (N'colEmail',            N'Email',                                                N'Email',                                                 N'E-Mail'),
    (N'colTelefono',         N'Teléfono',                                             N'Phone',                                                 N'Telefon'),
    (N'colDocumento',        N'Documento',                                            N'ID Document',                                           N'Ausweis'),
    (N'colDomicilio',        N'Domicilio',                                            N'Address',                                               N'Adresse'),
    (N'colEstado',           N'Estado',                                               N'Status',                                                N'Status'),
    (N'colIntentosFallidos', N'Int. Fallidos',                                        N'Failed Att.',                                           N'Fehlversuche'),
    (N'colUltimoLogin',      N'Último Login',                                         N'Last Login',                                            N'Letzte Anmeldung'),
    (N'valorBloqueado',      N'Bloqueado',                                            N'Blocked',                                               N'Gesperrt'),
    (N'valorActivo',         N'Activo',                                               N'Active',                                                N'Aktiv'),
    (N'msgSinPermiso',       N'No tenés permiso para acceder a esta sección.',        N'You don''t have permission to access this section.',    N'Du hast keine Berechtigung für diesen Bereich.'),
    (N'msgAccesoDenegado',   N'Acceso denegado',                                      N'Access denied',                                         N'Zugriff verweigert'),
    (N'msgErrorCargar',      N'Error al cargar usuarios:',                            N'Error loading users:',                                  N'Fehler beim Laden der Benutzer:'),
    (N'msgError',            N'Error',                                                N'Error',                                                 N'Fehler'),
    (N'msgConfirmarBloqueo', N'¿Bloquear al usuario ''{0}''?',                        N'Block user ''{0}''?',                                   N'Benutzer ''{0}'' sperren?'),
    (N'msgConfirmarDesbloqueo', N'¿Desbloquear al usuario ''{0}''?',                  N'Unblock user ''{0}''?',                                 N'Benutzer ''{0}'' entsperren?'),
    (N'msgConfirmar',        N'Confirmar',                                            N'Confirm',                                               N'Bestätigen');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formUsr FROM @ctrlUsr;

DECLARE @idEsUsr INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnUsr INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeUsr INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsUsr, c.Id, t.Es FROM @ctrlUsr t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUsr
UNION ALL
SELECT @idEnUsr, c.Id, t.En FROM @ctrlUsr t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUsr
UNION ALL
SELECT @idDeUsr, c.Id, t.De FROM @ctrlUsr t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUsr;
GO

-- ============================================================
-- FormInsertarUsuario — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formIns NVARCHAR(80) = N'FormInsertarUsuario';
DECLARE @ctrlIns TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlIns (Codigo, Es, En, De) VALUES
    (N'lblTitulo',            N'Registrar Usuario',                              N'Register User',                                       N'Benutzer registrieren'),
    (N'lblUsername',          N'Usuario:',                                       N'Username:',                                           N'Benutzer:'),
    (N'lblPassword',          N'Contraseña:',                                    N'Password:',                                           N'Passwort:'),
    (N'lblConfirmarPassword', N'Repetir contraseña:',                            N'Repeat password:',                                    N'Passwort wiederholen:'),
    (N'lblNombre',            N'Nombre:',                                        N'First Name:',                                         N'Vorname:'),
    (N'lblApellido',          N'Apellido:',                                      N'Last Name:',                                          N'Nachname:'),
    (N'lblEmail',             N'Email:',                                         N'Email:',                                              N'E-Mail:'),
    (N'lblTelefono',          N'Teléfono:',                                      N'Phone:',                                              N'Telefon:'),
    (N'lblDocumento',         N'Documento:',                                     N'ID Document:',                                        N'Ausweis:'),
    (N'lblDomicilio',         N'Domicilio:',                                     N'Address:',                                            N'Adresse:'),
    (N'btnRegistrar',         N'Registrar',                                      N'Register',                                            N'Registrieren'),
    (N'msgSinPermiso',        N'No tenés permiso para acceder a esta sección.',  N'You don''t have permission to access this section.',  N'Du hast keine Berechtigung für diesen Bereich.'),
    (N'msgAccesoDenegado',    N'Acceso denegado',                                N'Access denied',                                       N'Zugriff verweigert'),
    (N'msgCamposVacios',      N'Completá todos los campos.',                     N'Please fill in all fields.',                          N'Bitte fülle alle Felder aus.'),
    (N'msgAdvertencia',       N'Advertencia',                                    N'Warning',                                             N'Warnung'),
    (N'msgPasswordsDistintos',N'Las contraseñas no coinciden.',                  N'Passwords do not match.',                             N'Die Passwörter stimmen nicht überein.'),
    (N'msgUsuarioRegistrado', N'Usuario registrado correctamente.',              N'User registered successfully.',                       N'Benutzer erfolgreich registriert.'),
    (N'msgExito',             N'Éxito',                                          N'Success',                                             N'Erfolg'),
    (N'msgError',             N'Error',                                          N'Error',                                               N'Fehler');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formIns FROM @ctrlIns;

DECLARE @idEsIns INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnIns INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeIns INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsIns, c.Id, t.Es FROM @ctrlIns t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formIns
UNION ALL
SELECT @idEnIns, c.Id, t.En FROM @ctrlIns t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formIns
UNION ALL
SELECT @idDeIns, c.Id, t.De FROM @ctrlIns t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formIns;
GO

-- ============================================================
-- FormEditarUsuario — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formEdt NVARCHAR(80) = N'FormEditarUsuario';
DECLARE @ctrlEdt TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlEdt (Codigo, Es, En, De) VALUES
    (N'title',                 N'Editar Usuario',                                 N'Edit User',                                       N'Benutzer bearbeiten'),
    (N'lblTitulo',             N'Editar Usuario',                                 N'Edit User',                                       N'Benutzer bearbeiten'),
    (N'lblUsername',           N'Usuario:',                                       N'Username:',                                       N'Benutzer:'),
    (N'lblNombre',             N'Nombre:',                                        N'First Name:',                                     N'Vorname:'),
    (N'lblApellido',           N'Apellido:',                                      N'Last Name:',                                      N'Nachname:'),
    (N'lblEmail',              N'Email:',                                         N'Email:',                                          N'E-Mail:'),
    (N'lblTelefono',           N'Teléfono:',                                      N'Phone:',                                          N'Telefon:'),
    (N'lblDocumento',          N'Documento:',                                     N'ID Document:',                                    N'Ausweis:'),
    (N'lblDomicilio',          N'Domicilio:',                                     N'Address:',                                        N'Adresse:'),
    (N'lblIdioma',             N'Idioma preferido:',                              N'Preferred language:',                             N'Bevorzugte Sprache:'),
    (N'idiomaSinSeleccion',    N'— (sin seleccionar)',                            N'— (not set)',                                     N'— (nicht ausgewählt)'),
    (N'btnGuardar',            N'Guardar cambios',                                N'Save changes',                                    N'Änderungen speichern'),
    (N'msgSinPermiso',         N'No tenés permiso para editar usuarios.',         N'You don''t have permission to edit users.',       N'Du hast keine Berechtigung, Benutzer zu bearbeiten.'),
    (N'msgAccesoDenegado',     N'Acceso denegado',                                N'Access denied',                                   N'Zugriff verweigert'),
    (N'msgUsuarioNoEncontrado',N'No se encontró el usuario.',                     N'User not found.',                                 N'Benutzer nicht gefunden.'),
    (N'msgError',              N'Error',                                          N'Error',                                           N'Fehler'),
    (N'msgCamposVacios',       N'Completá todos los campos.',                     N'Please fill in all fields.',                      N'Bitte fülle alle Felder aus.'),
    (N'msgAdvertencia',        N'Advertencia',                                    N'Warning',                                         N'Warnung'),
    (N'msgUsuarioEditado',     N'Usuario editado correctamente.',                 N'User edited successfully.',                       N'Benutzer erfolgreich bearbeitet.'),
    (N'msgSinCambios',         N'No hay cambios para guardar.',                   N'No changes to save.',                             N'Keine Änderungen zum Speichern.'),
    (N'msgInformacion',        N'Información',                                    N'Information',                                     N'Information'),
    (N'msgExito',              N'Éxito',                                          N'Success',                                         N'Erfolg');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formEdt FROM @ctrlEdt;

DECLARE @idEsEdt INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnEdt INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeEdt INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsEdt, c.Id, t.Es FROM @ctrlEdt t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEdt
UNION ALL
SELECT @idEnEdt, c.Id, t.En FROM @ctrlEdt t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEdt
UNION ALL
SELECT @idDeEdt, c.Id, t.De FROM @ctrlEdt t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEdt;
GO

-- ============================================================
-- FormHistorialUsuario — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formHis NVARCHAR(80) = N'FormHistorialUsuario';
DECLARE @ctrlHis TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlHis (Codigo, Es, En, De) VALUES
    (N'title',                    N'Historial de Usuario',                                          N'User History',                                              N'Benutzerverlauf'),
    (N'lblTitulo',                N'Historial de Usuario',                                          N'User History',                                              N'Benutzerverlauf'),
    (N'lblTituloDe',              N'Historial de: {0}',                                             N'History of: {0}',                                           N'Verlauf von: {0}'),
    (N'btnRestaurar',             N'Restaurar a esta versión',                                      N'Restore to this version',                                   N'Auf diese Version wiederherstellen'),
    (N'btnCerrar',                N'Cerrar',                                                        N'Close',                                                     N'Schließen'),
    (N'leyendaActual',            N'● Versión actual',                                              N'● Current version',                                         N'● Aktuelle Version'),
    (N'colFechaHora',             N'Fecha y Hora',                                                  N'Date and Time',                                             N'Datum und Uhrzeit'),
    (N'colAccion',                N'Acción',                                                        N'Action',                                                    N'Aktion'),
    (N'colModificadoPor',         N'Modificado por',                                                N'Modified by',                                               N'Geändert von'),
    (N'colNombre',                N'Nombre',                                                        N'First Name',                                                N'Vorname'),
    (N'colApellido',              N'Apellido',                                                      N'Last Name',                                                 N'Nachname'),
    (N'colEmail',                 N'Email',                                                         N'Email',                                                     N'E-Mail'),
    (N'colTelefono',              N'Teléfono',                                                      N'Phone',                                                     N'Telefon'),
    (N'colDocumento',             N'Documento',                                                     N'ID Document',                                               N'Ausweis'),
    (N'colDomicilio',             N'Domicilio',                                                     N'Address',                                                   N'Adresse'),
    (N'colBloqueado',             N'Bloqueado',                                                     N'Blocked',                                                   N'Gesperrt'),
    (N'colRestauracion',          N'Restaurado desde',                                              N'Restored from',                                             N'Wiederhergestellt von'),
    (N'valorSi',                  N'Sí',                                                            N'Yes',                                                       N'Ja'),
    (N'valorNo',                  N'No',                                                            N'No',                                                        N'Nein'),
    (N'valorSistema',             N'(sistema)',                                                     N'(system)',                                                  N'(System)'),
    (N'msgSinPermiso',            N'No tenés permiso para ver el historial de usuarios.',           N'You don''t have permission to view user history.',          N'Du hast keine Berechtigung, den Benutzerverlauf anzusehen.'),
    (N'msgAccesoDenegado',        N'Acceso denegado',                                               N'Access denied',                                             N'Zugriff verweigert'),
    (N'msgUsuarioNoEncontrado',   N'No se encontró el usuario.',                                    N'User not found.',                                           N'Benutzer nicht gefunden.'),
    (N'msgError',                 N'Error',                                                         N'Error',                                                     N'Fehler'),
    (N'msgErrorCargar',           N'Error al cargar el historial:',                                 N'Error loading history:',                                    N'Fehler beim Laden des Verlaufs:'),
    (N'msgSelectVersion',         N'Seleccioná una versión a restaurar.',                           N'Select a version to restore.',                              N'Wähle eine Version zum Wiederherstellen aus.'),
    (N'msgValidacion',            N'Validación',                                                    N'Validation',                                                N'Validierung'),
    (N'msgConfirmarRestaurar',    N'¿Restaurar el usuario al estado del {0}?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Se sobrescribirán los datos actuales con los de esa versión. Quedará registrado como una nueva entrada en el historial.', N'Restore user to state of {0}?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Current data will be overwritten with that version''s data. A new entry will be recorded in the history.', N'Benutzer auf den Stand vom {0} wiederherstellen?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Die aktuellen Daten werden mit denen dieser Version überschrieben. Dies wird als neuer Eintrag im Verlauf protokolliert.'),
    (N'msgConfirmarRestaurarTitulo', N'Confirmar restauración',                                     N'Confirm restore',                                           N'Wiederherstellung bestätigen'),
    (N'msgRestauradoExito',       N'Usuario restaurado correctamente.',                             N'User restored successfully.',                               N'Benutzer erfolgreich wiederhergestellt.'),
    (N'msgExito',                 N'Éxito',                                                         N'Success',                                                   N'Erfolg'),
    (N'msgSinPermisoRestaurar',   N'Para restaurar necesitás permiso de edición de usuarios.',      N'To restore you need user-edit permission.',                 N'Zum Wiederherstellen benötigst du die Berechtigung zum Bearbeiten von Benutzern.');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formHis FROM @ctrlHis;

DECLARE @idEsHis INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnHis INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeHis INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsHis, c.Id, t.Es FROM @ctrlHis t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formHis
UNION ALL
SELECT @idEnHis, c.Id, t.En FROM @ctrlHis t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formHis
UNION ALL
SELECT @idDeHis, c.Id, t.De FROM @ctrlHis t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formHis;
GO

-- ============================================================
-- Enum AccionUsuarioHistorial — Traducciones ES/EN
-- ============================================================
DECLARE @formAccUH NVARCHAR(80) = N'AccionUsuarioHistorial';
DECLARE @ctrlAccUH TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlAccUH (Codigo, Es, En, De) VALUES
    (N'Alta',          N'Alta',          N'Created',     N'Angelegt'),
    (N'Edicion',       N'Edición',       N'Edited',      N'Bearbeitet'),
    (N'Bloqueo',       N'Bloqueo',       N'Blocked',     N'Gesperrt'),
    (N'Desbloqueo',    N'Desbloqueo',    N'Unblocked',   N'Entsperrt'),
    (N'Restauracion',  N'Restauración',  N'Restored',    N'Wiederhergestellt');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formAccUH FROM @ctrlAccUH;

DECLARE @idEsAccUH INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnAccUH INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeAccUH INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsAccUH, c.Id, t.Es FROM @ctrlAccUH t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formAccUH
UNION ALL
SELECT @idEnAccUH, c.Id, t.En FROM @ctrlAccUH t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formAccUH
UNION ALL
SELECT @idDeAccUH, c.Id, t.De FROM @ctrlAccUH t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formAccUH;
GO

-- ============================================================
-- FormPermisos — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formPer NVARCHAR(80) = N'FormPermisos';
DECLARE @ctrlPer TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlPer (Codigo, Es, En, De) VALUES
    (N'lblTitulo',              N'Gestión de Permisos',                                                N'Manage Permissions',                                                                                                       N'Berechtigungen verwalten'),
    (N'lblCol1',                N'Roles',                                                              N'Roles',                                                                                                                    N'Rollen'),
    (N'lblCol2',                N'Contenido',                                                          N'Contents',                                                                                                                 N'Inhalt'),
    (N'lblCol2De',              N'Contenido de: {0}',                                                  N'Contents of: {0}',                                                                                                         N'Inhalt von: {0}'),
    (N'lblCol3',                N'Roles y Permisos disponibles',                                       N'Available Roles and Permissions',                                                                                          N'Verfügbare Rollen und Berechtigungen'),
    (N'btnNuevoRol',            N'Nuevo Rol',                                                          N'New Role',                                                                                                                 N'Neue Rolle'),
    (N'btnEliminarRol',         N'Borrar Rol',                                                         N'Delete Role',                                                                                                              N'Rolle löschen'),
    (N'btnQuitar',              N'Sacar de la lista →',                                                N'Remove from list →',                                                                                                       N'Aus der Liste entfernen →'),
    (N'btnAgregar',             N'← Agregar a la lista',                                               N'← Add to list',                                                                                                            N'← Zur Liste hinzufügen'),
    (N'msgSinPermiso',          N'No tenés permiso para gestionar permisos.',                          N'You don''t have permission to manage permissions.',                                                                        N'Du hast keine Berechtigung, Berechtigungen zu verwalten.'),
    (N'msgAccesoDenegado',      N'Acceso denegado',                                                    N'Access denied',                                                                                                            N'Zugriff verweigert'),
    (N'msgErrorCargarRoles',    N'Error al cargar roles:',                                             N'Error loading roles:',                                                                                                     N'Fehler beim Laden der Rollen:'),
    (N'msgError',               N'Error',                                                              N'Error',                                                                                                                    N'Fehler'),
    (N'msgErrorPrefix',         N'Error: {0}',                                                         N'Error: {0}',                                                                                                               N'Fehler: {0}'),
    (N'promptCodigoRol',        N'Código del rol:',                                                    N'Role code:',                                                                                                               N'Rollen-Code:'),
    (N'promptDescripcionRol',   N'Descripción (opcional):',                                            N'Description (optional):',                                                                                                  N'Beschreibung (optional):'),
    (N'msgSelectRol',           N'Seleccioná un rol a la izquierda.',                                  N'Select a role on the left.',                                                                                               N'Wähle eine Rolle auf der linken Seite aus.'),
    (N'msgValidacion',          N'Validación',                                                         N'Validation',                                                                                                               N'Validierung'),
    (N'msgConfirmarBorrarRol',  N'¿Borrar el rol ''{0}''?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Esta acción es irreversible. El rol se desasignará automáticamente de todos los usuarios que lo tengan asignado. Los permisos hijos NO se borran, sólo quedan sin padre.', N'Delete role ''{0}''?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'This action is irreversible. The role will be automatically unassigned from all users. Child permissions are NOT deleted, they just lose their parent.', N'Rolle ''{0}'' löschen?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Diese Aktion ist unumkehrbar. Die Rolle wird automatisch von allen zugewiesenen Benutzern entfernt. Untergeordnete Berechtigungen werden NICHT gelöscht, sie verlieren nur ihre übergeordnete Rolle.'),
    (N'msgConfirmarEliminacion',N'Confirmar eliminación',                                              N'Confirm deletion',                                                                                                         N'Löschen bestätigen'),
    (N'msgSelectPermisos',      N'Seleccioná uno o más permisos de la lista de la derecha (Ctrl+click o Shift+click para varios).', N'Select one or more permissions from the right list (Ctrl+click or Shift+click for multiple).',                                          N'Wähle eine oder mehrere Berechtigungen aus der rechten Liste aus (Strg+Klick oder Umschalt+Klick für mehrere).'),
    (N'msgFallaProc',           N'Se procesaron {0} de {1}. Falló en ''{2}'': {3}',                    N'Processed {0} of {1}. Failed at ''{2}'': {3}',                                                                             N'{0} von {1} verarbeitet. Fehler bei ''{2}'': {3}'),
    (N'msgOperacionInterrumpida',N'Operación interrumpida',                                            N'Operation interrupted',                                                                                                    N'Vorgang unterbrochen'),
    (N'msgLimpiezaAuto',        N'Se agregaron {0}. Se quitaron {1} asignación(es) directa(s) cubiertas por este rol.', N'{0} added. {1} direct assignment(s) covered by this role were removed.',                                                            N'{0} hinzugefügt. {1} direkte Zuweisung(en), die durch diese Rolle abgedeckt sind, wurden entfernt.'),
    (N'msgLimpiezaAutoTitulo',  N'Limpieza automática',                                                N'Auto cleanup',                                                                                                             N'Automatische Bereinigung'),
    (N'msgSelectHijos',         N'Seleccioná uno o más hijos a quitar (Ctrl+click o Shift+click para varios).', N'Select one or more children to remove (Ctrl+click or Shift+click for multiple).',                                       N'Wähle ein oder mehrere Kinder zum Entfernen aus (Strg+Klick oder Umschalt+Klick für mehrere).'),
    (N'promptIngresarTitulo',   N'Ingresar',                                                           N'Enter',                                                                                                                    N'Eingeben'),
    (N'btnAceptar',             N'Aceptar',                                                            N'Accept',                                                                                                                   N'OK'),
    (N'btnCancelar',            N'Cancelar',                                                           N'Cancel',                                                                                                                   N'Abbrechen');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formPer FROM @ctrlPer;

DECLARE @idEsPer INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnPer INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDePer INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsPer, c.Id, t.Es FROM @ctrlPer t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPer
UNION ALL
SELECT @idEnPer, c.Id, t.En FROM @ctrlPer t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPer
UNION ALL
SELECT @idDePer, c.Id, t.De FROM @ctrlPer t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPer;
GO

-- ============================================================
-- FormAsignacionPermisos — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formAsig NVARCHAR(80) = N'FormAsignacionPermisos';
DECLARE @ctrlAsig TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlAsig (Codigo, Es, En, De) VALUES
    (N'lblTitulo',              N'Asignación de Permisos a Usuarios',                                  N'Assign Permissions to Users',                                                                                             N'Berechtigungen Benutzern zuweisen'),
    (N'lblCol1',                N'Usuarios',                                                           N'Users',                                                                                                                   N'Benutzer'),
    (N'lblCol2',                N'Asignaciones del usuario',                                           N'User Assignments',                                                                                                        N'Zuweisungen des Benutzers'),
    (N'lblCol2De',              N'Asignaciones de: {0}',                                               N'Assignments of: {0}',                                                                                                     N'Zuweisungen von: {0}'),
    (N'lblCol3',                N'Roles y Permisos disponibles',                                       N'Available Roles and Permissions',                                                                                         N'Verfügbare Rollen und Berechtigungen'),
    (N'btnQuitar',              N'Quitar del usuario →',                                               N'Remove from user →',                                                                                                      N'Vom Benutzer entfernen →'),
    (N'btnAsignar',             N'← Asignar al usuario',                                               N'← Assign to user',                                                                                                        N'← Dem Benutzer zuweisen'),
    (N'msgSinPermiso',          N'No tenés permiso para asignar permisos a usuarios.',                 N'You don''t have permission to assign permissions to users.',                                                              N'Du hast keine Berechtigung, Benutzern Berechtigungen zuzuweisen.'),
    (N'msgAccesoDenegado',      N'Acceso denegado',                                                    N'Access denied',                                                                                                           N'Zugriff verweigert'),
    (N'msgErrorCargar',         N'Error al cargar usuarios:',                                          N'Error loading users:',                                                                                                    N'Fehler beim Laden der Benutzer:'),
    (N'msgError',               N'Error',                                                              N'Error',                                                                                                                   N'Fehler'),
    (N'msgValidacion',          N'Validación',                                                         N'Validation',                                                                                                              N'Validierung'),
    (N'msgSelectUsuario',       N'Seleccioná un usuario a la izquierda.',                              N'Select a user on the left.',                                                                                              N'Wähle einen Benutzer auf der linken Seite aus.'),
    (N'msgSelectPermisos',      N'Seleccioná uno o más permisos de la lista de la derecha (Ctrl+click o Shift+click para varios).', N'Select one or more permissions from the right list (Ctrl+click or Shift+click for multiple).',                                          N'Wähle eine oder mehrere Berechtigungen aus der rechten Liste aus (Strg+Klick oder Umschalt+Klick für mehrere).'),
    (N'msgSelectAsignaciones',  N'Seleccioná una o más asignaciones a quitar (Ctrl+click o Shift+click para varias).',              N'Select one or more assignments to remove (Ctrl+click or Shift+click for multiple).',                                       N'Wähle eine oder mehrere Zuweisungen zum Entfernen aus (Strg+Klick oder Umschalt+Klick für mehrere).'),
    (N'msgFallaProc',           N'Se procesaron {0} de {1}. Falló en ''{2}'': {3}',                    N'Processed {0} of {1}. Failed at ''{2}'': {3}',                                                                            N'{0} von {1} verarbeitet. Fehler bei ''{2}'': {3}'),
    (N'msgOperacionInterrumpida',N'Operación interrumpida',                                            N'Operation interrupted',                                                                                                   N'Vorgang unterbrochen'),
    (N'msgLimpiezaAuto',        N'Se asignaron {0}. Se quitaron {1} asignación(es) directa(s) redundante(s) porque quedaron cubiertas.', N'{0} assigned. {1} redundant direct assignment(s) were removed because they became covered.',                                          N'{0} zugewiesen. {1} redundante direkte Zuweisung(en) wurden entfernt, da sie nun abgedeckt sind.'),
    (N'msgLimpiezaAutoTitulo',  N'Limpieza automática',                                                N'Auto cleanup',                                                                                                            N'Automatische Bereinigung');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formAsig FROM @ctrlAsig;

DECLARE @idEsAsig INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnAsig INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeAsig INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsAsig, c.Id, t.Es FROM @ctrlAsig t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formAsig
UNION ALL
SELECT @idEnAsig, c.Id, t.En FROM @ctrlAsig t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formAsig
UNION ALL
SELECT @idDeAsig, c.Id, t.De FROM @ctrlAsig t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formAsig;
GO

-- ============================================================
-- FormIdiomas — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formIdi NVARCHAR(80) = N'FormIdiomas';
DECLARE @ctrlIdi TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlIdi (Codigo, Es, En, De) VALUES
    (N'lblTitulo',                 N'Gestión de Idiomas',                                                   N'Manage Languages',                                                                                                          N'Sprachen verwalten'),
    (N'lblIdiomas',                N'Idiomas disponibles',                                                  N'Available Languages',                                                                                                       N'Verfügbare Sprachen'),
    (N'lblTraducciones',           N'Traducciones',                                                         N'Translations',                                                                                                              N'Übersetzungen'),
    (N'lblTraduccionesDe',         N'Traducciones — {0}',                                                   N'Translations — {0}',                                                                                                        N'Übersetzungen — {0}'),
    (N'lblAlerta',                 N'⚠  Idioma en proceso de creación — completá todas las traducciones para poder activarlo.', N'⚠  Language in progress — complete all translations to activate it.',                                            N'⚠  Sprache in Bearbeitung — vervollständige alle Übersetzungen, um sie zu aktivieren.'),
    (N'btnNuevoIdioma',            N'Nuevo Idioma',                                                         N'New Language',                                                                                                              N'Neue Sprache'),
    (N'btnEliminarIdioma',         N'Eliminar Idioma',                                                      N'Delete Language',                                                                                                           N'Sprache löschen'),
    (N'btnGuardar',                N'Guardar cambios',                                                      N'Save changes',                                                                                                              N'Änderungen speichern'),
    (N'colForm',                   N'Form',                                                                 N'Form',                                                                                                                      N'Form'),
    (N'colCodigo',                 N'Código',                                                               N'Code',                                                                                                                      N'Code'),
    (N'colTexto',                  N'Texto',                                                                N'Text',                                                                                                                      N'Text'),
    (N'msgSinPermiso',             N'No tenés permiso para gestionar idiomas.',                             N'You don''t have permission to manage languages.',                                                                           N'Du hast keine Berechtigung, Sprachen zu verwalten.'),
    (N'msgAccesoDenegado',         N'Acceso denegado',                                                      N'Access denied',                                                                                                             N'Zugriff verweigert'),
    (N'msgErrorCargarIdiomas',     N'Error al cargar idiomas:',                                             N'Error loading languages:',                                                                                                  N'Fehler beim Laden der Sprachen:'),
    (N'msgErrorCargarTraducciones',N'Error al cargar traducciones:',                                        N'Error loading translations:',                                                                                               N'Fehler beim Laden der Übersetzungen:'),
    (N'msgError',                  N'Error',                                                                N'Error',                                                                                                                     N'Fehler'),
    (N'promptNombreIdioma',        N'Nombre del idioma:',                                                   N'Language name:',                                                                                                            N'Sprachname:'),
    (N'msgErrorPrefix',            N'Error: {0}',                                                           N'Error: {0}',                                                                                                                N'Fehler: {0}'),
    (N'msgSelectIdioma',           N'Seleccioná un idioma de la lista.',                                    N'Select a language from the list.',                                                                                          N'Wähle eine Sprache aus der Liste aus.'),
    (N'msgValidacion',             N'Validación',                                                           N'Validation',                                                                                                                N'Validierung'),
    (N'msgConfirmarBorrarIdioma',  N'¿Borrar el idioma ''{0}''?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Esta acción es irreversible y eliminará todas las traducciones asociadas.', N'Delete language ''{0}''?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'This action is irreversible and will delete all associated translations.', N'Sprache ''{0}'' löschen?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Diese Aktion ist unumkehrbar und entfernt alle zugehörigen Übersetzungen.'),
    (N'msgConfirmarEliminacion',   N'Confirmar eliminación',                                                N'Confirm deletion',                                                                                                          N'Löschen bestätigen'),
    (N'msgEspanolCompleto',        N'El idioma ''Español'' debe estar siempre completo: es el idioma base del sistema.' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Completá todas las filas antes de guardar.', N'The ''Español'' language must always be complete: it is the system base language.' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Complete all rows before saving.', N'Die Sprache ''Español'' muss immer vollständig sein: sie ist die Basissprache des Systems.' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Vervollständige alle Zeilen vor dem Speichern.'),
    (N'msgNoSePuedeGuardar',       N'No se puede guardar',                                                  N'Cannot save',                                                                                                               N'Speichern nicht möglich'),
    (N'msgSinCambios',             N'No hay cambios para guardar.',                                         N'No changes to save.',                                                                                                       N'Keine Änderungen zum Speichern.'),
    (N'msgInformacion',            N'Información',                                                          N'Information',                                                                                                               N'Information'),
    (N'msgGuardadoExito',          N'Se guardaron {0} traducción(es).',                                     N'Saved {0} translation(s).',                                                                                                 N'{0} Übersetzung(en) gespeichert.'),
    (N'msgCambiosGuardados',       N'Cambios guardados',                                                    N'Changes saved',                                                                                                             N'Änderungen gespeichert'),
    (N'msgErrorGuardar',           N'Error al guardar:',                                                    N'Error saving:',                                                                                                             N'Fehler beim Speichern:'),
    (N'promptIngresarTitulo',      N'Ingresar',                                                             N'Enter',                                                                                                                     N'Eingeben'),
    (N'btnAceptar',                N'Aceptar',                                                              N'Accept',                                                                                                                    N'OK'),
    (N'btnCancelar',               N'Cancelar',                                                             N'Cancel',                                                                                                                    N'Abbrechen');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formIdi FROM @ctrlIdi;

DECLARE @idEsIdi INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnIdi INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeIdi INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsIdi, c.Id, t.Es FROM @ctrlIdi t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formIdi
UNION ALL
SELECT @idEnIdi, c.Id, t.En FROM @ctrlIdi t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formIdi
UNION ALL
SELECT @idDeIdi, c.Id, t.De FROM @ctrlIdi t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formIdi;
GO

-- ============================================================
-- FormMantenimiento — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formMant NVARCHAR(80) = N'FormMantenimiento';
DECLARE @ctrlMant TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlMant (Codigo, Es, En, De) VALUES
    -- Designer / títulos
    (N'title',                       N'Mantenimiento',                                                       N'Maintenance',                                                                                                            N'Wartung'),
    (N'lblTituloOk',                 N'Modo Mantenimiento — Integridad OK',                                  N'Maintenance Mode — Integrity OK',                                                                                        N'Wartungsmodus — Integrität OK'),
    (N'lblTituloError',              N'Modo Mantenimiento — Error de integridad detectado',                  N'Maintenance Mode — Integrity error detected',                                                                            N'Wartungsmodus — Integritätsfehler erkannt'),
    (N'lblEstadoOk',                 N'La base de datos pasó la verificación de integridad. Podés cerrar este form.', N'Database integrity check passed. You can close this form.',                                                     N'Die Datenbankintegrität wurde erfolgreich überprüft. Du kannst dieses Fenster schließen.'),
    (N'lblEstadoError',              N'Detalle por entidad:',                                                N'Detail by entity:',                                                                                                      N'Details pro Entität:'),
    (N'lblBackupSeleccionado',       N'Backup a restaurar:',                                                 N'Backup to restore:',                                                                                                     N'Backup zum Wiederherstellen:'),
    (N'lblUsuarioMant',              N'Sesión: mantenimiento',                                               N'Session: maintenance',                                                                                                   N'Sitzung: Wartung'),
    -- Botones
    (N'btnBackup',                   N'Backup ahora',                                                        N'Backup now',                                                                                                             N'Jetzt sichern'),
    (N'btnRestaurar',                N'Restaurar selección',                                                 N'Restore selection',                                                                                                      N'Auswahl wiederherstellen'),
    (N'btnRecalcular',               N'Recalcular DVs',                                                      N'Recalculate DVs',                                                                                                        N'DVs neu berechnen'),
    (N'btnReverificar',              N'Reverificar',                                                         N'Re-verify',                                                                                                              N'Erneut prüfen'),
    (N'btnSalir',                    N'Salir',                                                               N'Exit',                                                                                                                   N'Beenden'),
    -- Líneas del detalle
    (N'detalleTabla',                N'== Tabla {0} ==',                                                     N'== Table {0} ==',                                                                                                        N'== Tabelle {0} =='),
    (N'detalleOk',                   N'  OK',                                                                N'  OK',                                                                                                                   N'  OK'),
    (N'detalleDvhInvalido',          N'  DVH inválido en {0} fila(s). Ids: {1}',                             N'  Invalid DVH in {0} row(s). Ids: {1}',                                                                                  N'  Ungültiger DVH in {0} Zeile(n). IDs: {1}'),
    (N'detalleDvvInvalido',          N'  DVV inválido (la suma de DVHs no coincide con el almacenado).',     N'  Invalid DVV (sum of DVHs does not match stored value).',                                                               N'  Ungültiger DVV (die Summe der DVHs stimmt nicht mit dem gespeicherten Wert überein).'),
    -- MessageBoxes
    (N'tituloBackup',                N'Backup',                                                              N'Backup',                                                                                                                 N'Backup'),
    (N'tituloRestaurar',             N'Restaurar',                                                           N'Restore',                                                                                                                N'Wiederherstellen'),
    (N'tituloRecalcular',            N'Recalcular',                                                          N'Recalculate',                                                                                                            N'Neu berechnen'),
    (N'tituloVerificacion',          N'Verificación',                                                        N'Verification',                                                                                                           N'Überprüfung'),
    (N'msgError',                    N'Error',                                                               N'Error',                                                                                                                  N'Fehler'),
    (N'msgBackupCreado',             N'Backup creado:' + NCHAR(10) + N'{0}',                                  N'Backup created:' + NCHAR(10) + N'{0}',                                                                                  N'Backup erstellt:' + NCHAR(10) + N'{0}'),
    (N'msgBackupError',              N'No se pudo crear el backup.',                                         N'Could not create the backup.',                                                                                           N'Backup konnte nicht erstellt werden.'),
    (N'msgRestoreSinSeleccion',      N'Seleccioná un archivo de backup válido.',                             N'Select a valid backup file.',                                                                                            N'Wähle eine gültige Backup-Datei aus.'),
    (N'msgRestoreConfirmar',         N'¿Confirmás restaurar la base de datos desde:' + NCHAR(10) + N'{0}' + NCHAR(10) + NCHAR(10) + N'Se sobrescribirá el estado actual.', N'Confirm restoring the database from:' + NCHAR(10) + N'{0}' + NCHAR(10) + NCHAR(10) + N'Current state will be overwritten.', N'Bestätigst du die Wiederherstellung der Datenbank aus:' + NCHAR(10) + N'{0}' + NCHAR(10) + NCHAR(10) + N'Der aktuelle Zustand wird überschrieben.'),
    (N'msgRestoreConfirmarTitulo',   N'Confirmar restauración',                                              N'Confirm restore',                                                                                                        N'Wiederherstellung bestätigen'),
    (N'msgRestoreOk',                N'Restore completado.',                                                 N'Restore completed.',                                                                                                     N'Wiederherstellung abgeschlossen.'),
    (N'msgRestoreError',             N'No se pudo restaurar el backup.',                                     N'Could not restore the backup.',                                                                                          N'Backup konnte nicht wiederhergestellt werden.'),
    (N'msgRecalcularConfirmar',      N'Recalcular los DVs sobrescribe los valores almacenados con lo calculado a partir del estado actual de la base.' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Sólo hacelo si confiás en que los datos actuales son correctos (por ejemplo, después de un Restore o de una intervención manual autorizada).' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'¿Continuar?', N'Recalculating DVs overwrites the stored values with those computed from the current database state.' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Only do this if you trust the current data is correct (for example, after a Restore or an authorized manual intervention).' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Continue?', N'Beim Neuberechnen der DVs werden die gespeicherten Werte mit den aus dem aktuellen Datenbankzustand berechneten überschrieben.' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Mache dies nur, wenn du den aktuellen Daten vertraust (zum Beispiel nach einer Wiederherstellung oder einem autorisierten manuellen Eingriff).' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Fortfahren?'),
    (N'msgRecalcularConfirmarTitulo',N'Confirmar recálculo',                                                 N'Confirm recalculation',                                                                                                  N'Neuberechnung bestätigen'),
    (N'msgRecalcularOk',             N'DVs recalculados correctamente.',                                     N'DVs recalculated successfully.',                                                                                         N'DVs erfolgreich neu berechnet.'),
    (N'msgRecalcularError',          N'No se pudieron recalcular los DVs.',                                  N'Could not recalculate DVs.',                                                                                             N'DVs konnten nicht neu berechnet werden.'),
    (N'msgIntegridadOk',             N'La base de datos quedó íntegra. Podés salir y volver al login.',      N'The database is integral again. You can exit and return to the login.',                                                  N'Die Datenbank ist wieder integer. Du kannst das Fenster schließen und zur Anmeldung zurückkehren.'),
    (N'msgReverificarError',         N'Error al reverificar la integridad.',                                 N'Error while re-verifying integrity.',                                                                                    N'Fehler bei der erneuten Integritätsprüfung.');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formMant FROM @ctrlMant;

DECLARE @idEsMant INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnMant INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeMant INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsMant, c.Id, t.Es FROM @ctrlMant t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formMant
UNION ALL
SELECT @idEnMant, c.Id, t.En FROM @ctrlMant t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formMant
UNION ALL
SELECT @idDeMant, c.Id, t.De FROM @ctrlMant t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formMant;
GO

-- ============================================================
-- Errores BLL — Codigos + Traducciones ES/EN
-- ============================================================
DECLARE @formErr NVARCHAR(80) = N'Errores';
DECLARE @ctrlErr TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlErr (Codigo, Es, En, De) VALUES
    -- UsuarioService
    (N'ERR_LOGIN_GENERICO',             N'Ocurrió un error en Login.',                                             N'A login error occurred.',                                                                                       N'Ein Anmeldefehler ist aufgetreten.'),
    (N'ERR_LOGOUT_GENERICO',            N'Ocurrió un error en Logout.',                                            N'A logout error occurred.',                                                                                      N'Ein Abmeldefehler ist aufgetreten.'),
    (N'ERR_SIN_PERMISO_DESBLOQUEAR',    N'No tiene permisos para desbloquear usuarios.',                           N'You don''t have permission to unblock users.',                                                                  N'Du hast keine Berechtigung, Benutzer zu entsperren.'),
    (N'ERR_NO_BLOQUEAR_ADMIN',          N'No podés bloquear al usuario administrador.',                            N'You cannot block the administrator user.',                                                                      N'Du kannst den Administrator-Benutzer nicht sperren.'),
    (N'ERR_SIN_PERMISO_BLOQUEAR',       N'No tiene permisos para bloquear usuarios.',                              N'You don''t have permission to block users.',                                                                    N'Du hast keine Berechtigung, Benutzer zu sperren.'),
    (N'ERR_REGISTRO_DATOS_INCOMPLETOS', N'Faltan datos para registrar al usuario.',                                N'Missing data to register the user.',                                                                            N'Fehlende Daten für die Benutzerregistrierung.'),
    (N'ERR_EDICION_DATOS_INCOMPLETOS',  N'Faltan datos para editar al usuario.',                                   N'Missing data to edit the user.',                                                                                N'Fehlende Daten für die Benutzerbearbeitung.'),
    (N'ERR_USERNAME_EXISTE',            N'Ya existe el username.',                                                 N'Username already exists.',                                                                                      N'Benutzername existiert bereits.'),
    (N'ERR_USERNAME_EN_USO',            N'El username ''{0}'' ya está en uso por otro usuario.',                   N'Username ''{0}'' is already in use by another user.',                                                           N'Der Benutzername ''{0}'' wird bereits von einem anderen Benutzer verwendet.'),
    (N'ERR_SIN_PERMISO_EDITAR_USUARIO', N'No tenés permiso para editar usuarios.',                                 N'You don''t have permission to edit users.',                                                                     N'Du hast keine Berechtigung, Benutzer zu bearbeiten.'),
    (N'ERR_SIN_PERMISO_HISTORIAL',      N'No tenés permiso para ver el historial de usuarios.',                    N'You don''t have permission to view user history.',                                                              N'Du hast keine Berechtigung, den Benutzerverlauf anzusehen.'),
    (N'ERR_HISTORIAL_NO_EXISTE',        N'La versión seleccionada no existe.',                                     N'The selected version does not exist.',                                                                          N'Die ausgewählte Version existiert nicht.'),
    (N'ERR_USUARIO_NO_ENCONTRADO',      N'No se encontró el usuario.',                                             N'User not found.',                                                                                               N'Benutzer nicht gefunden.'),
    (N'ERR_SIN_PERMISO_ACCION',         N'No tenés permiso para ejecutar esta acción ({0}).',                      N'You don''t have permission to perform this action ({0}).',                                                      N'Du hast keine Berechtigung, diese Aktion auszuführen ({0}).'),
    (N'ERR_AUTOASIGNACION_PERMISOS',    N'No podés asignarte o quitarte permisos a vos mismo. Sólo el administrador puede hacerlo.', N'You cannot assign or revoke your own permissions. Only the administrator can do that.',                N'Du kannst dir nicht selbst Berechtigungen zuweisen oder entziehen. Nur der Administrator darf dies tun.'),
    -- PermisoService
    (N'ERR_CODIGO_OBLIGATORIO',         N'Código es obligatorio.',                                                 N'Code is required.',                                                                                             N'Code ist erforderlich.'),
    (N'ERR_PERMISO_CODIGO_DUPLICADO',   N'Ya existe un permiso con el código ''{0}''. Los códigos deben ser únicos.', N'A permission with code ''{0}'' already exists. Codes must be unique.',                                       N'Es existiert bereits eine Berechtigung mit dem Code ''{0}''. Codes müssen eindeutig sein.'),
    (N'ERR_PERMISO_AUTOCONTENCION',     N'Un permiso no puede contenerse a sí mismo.',                             N'A permission cannot contain itself.',                                                                           N'Eine Berechtigung kann sich nicht selbst enthalten.'),
    (N'ERR_PERMISO_CICLICO',            N'La asignación generaría una referencia circular entre permisos.',        N'The assignment would create a circular reference between permissions.',                                         N'Die Zuweisung würde eine Zirkelreferenz zwischen Berechtigungen erzeugen.'),
    (N'ERR_ROL_NO_EXISTE',              N'El rol no existe.',                                                      N'The role does not exist.',                                                                                      N'Die Rolle existiert nicht.'),
    (N'ERR_SOLO_ADMIN_PERMISOS',        N'Sólo el usuario administrador puede gestionar permisos.',                N'Only the administrator can manage permissions.',                                                                N'Nur der Administrator kann Berechtigungen verwalten.'),
    -- IdiomaService
    (N'ERR_IDIOMA_NOMBRE_OBLIGATORIO',  N'El nombre del idioma es obligatorio.',                                   N'Language name is required.',                                                                                    N'Der Sprachname ist erforderlich.'),
    (N'ERR_IDIOMA_NOMBRE_DUPLICADO',    N'Ya existe un idioma con el nombre ''{0}''. Los nombres deben ser únicos.', N'A language named ''{0}'' already exists. Names must be unique.',                                              N'Eine Sprache mit dem Namen ''{0}'' existiert bereits. Namen müssen eindeutig sein.'),
    (N'ERR_IDIOMA_NO_EXISTE',           N'El idioma no existe.',                                                   N'The language does not exist.',                                                                                  N'Die Sprache existiert nicht.'),
    (N'ERR_IDIOMA_PROTEGIDO_BORRAR',    N'El idioma ''{0}'' no puede eliminarse: es el idioma base del sistema.',  N'Language ''{0}'' cannot be deleted: it is the system base language.',                                           N'Die Sprache ''{0}'' kann nicht gelöscht werden: sie ist die Basissprache des Systems.'),
    (N'ERR_IDIOMA_INCOMPLETO',          N'El idioma ''{0}'' está en proceso de creación. Completá todas las traducciones antes de activarlo.', N'Language ''{0}'' is in progress. Complete all translations before activating it.',           N'Die Sprache ''{0}'' ist in Bearbeitung. Vervollständige alle Übersetzungen, bevor du sie aktivierst.'),
    (N'ERR_IDIOMA_PROTEGIDO_VACIO',     N'El idioma ''{0}'' no admite traducciones vacías: es el idioma base del sistema y debe estar siempre completo.', N'Language ''{0}'' does not allow empty translations: it is the system base language and must always be complete.', N'Die Sprache ''{0}'' erlaubt keine leeren Übersetzungen: sie ist die Basissprache des Systems und muss immer vollständig sein.'),
    -- MantenimientoService
    (N'ERR_BACKUP_NO_EXISTE',           N'El archivo de backup ''{0}'' no existe.',                                N'Backup file ''{0}'' does not exist.',                                                                           N'Die Backup-Datei ''{0}'' existiert nicht.'),
    -- N01 — Persona / Unidad / Checklist
    (N'ERR_PERSONA_DATOS_INCOMPLETOS',  N'Faltan datos obligatorios de la persona.',                               N'Missing required person data.',                                                                                 N'Fehlende Pflichtdaten der Person.'),
    (N'ERR_PERSONA_TIPO_INVALIDO',      N'Tipo de persona inválido (esperado ''F'' o ''J'').',                     N'Invalid person type (expected ''F'' or ''J'').',                                                                N'Ungültiger Personentyp (erwartet ''F'' oder ''J'').'),
    (N'ERR_PERSONA_DOCUMENTO_DUPLICADO',N'Ya existe una persona con el documento ''{0}''.',                        N'A person with document ''{0}'' already exists.',                                                                N'Eine Person mit dem Dokument ''{0}'' existiert bereits.'),
    (N'ERR_PERSONA_NO_EXISTE',          N'La persona no existe.',                                                  N'The person does not exist.',                                                                                    N'Die Person existiert nicht.'),
    (N'ERR_PERSONA_EN_USO',             N'No se puede eliminar la persona ''{0}'' porque hay {1} unidad(es) asociada(s).', N'Cannot delete person ''{0}'' because {1} unit(s) are associated.',                                       N'Person ''{0}'' kann nicht gelöscht werden, weil {1} Einheit(en) zugeordnet sind.'),
    (N'ERR_PERSONA_FECHA_NACIMIENTO_INVALIDA', N'La fecha de nacimiento es inválida.',                             N'The birth date is invalid.',                                                                                    N'Das Geburtsdatum ist ungültig.'),
    (N'ERR_PERSONA_EMAIL_INVALIDO',     N'El email ''{0}'' no tiene un formato válido.',                           N'The email ''{0}'' is not in a valid format.',                                                                   N'Die E-Mail ''{0}'' hat kein gültiges Format.'),
    (N'ERR_UNIDAD_DATOS_INCOMPLETOS',   N'Faltan datos obligatorios de la unidad.',                                N'Missing required unit data.',                                                                                   N'Fehlende Pflichtdaten der Einheit.'),
    (N'ERR_DOMINIO_DUPLICADO',          N'Ya existe una unidad registrada con el dominio ''{0}''.',                N'A unit with plate ''{0}'' already exists.',                                                                     N'Es existiert bereits eine Einheit mit Kennzeichen ''{0}''.'),
    (N'ERR_DOMINIO_INVALIDO',           N'El dominio ''{0}'' no tiene un formato válido (AAA123 o AB123CD).',      N'The plate ''{0}'' is not in a valid format (AAA123 or AB123CD).',                                                N'Das Kennzeichen ''{0}'' hat kein gültiges Format (AAA123 oder AB123CD).'),
    (N'ERR_UNIDAD_NO_ENCONTRADA',       N'No se encontró la unidad solicitada.',                                   N'Requested unit not found.',                                                                                     N'Angeforderte Einheit nicht gefunden.'),
    (N'ERR_TRANSICION_INVALIDA',        N'Transición de estado inválida ({0} → {1}).',                             N'Invalid state transition ({0} → {1}).',                                                                         N'Ungültiger Zustandsübergang ({0} → {1}).'),
    (N'ERR_MOTIVO_RECHAZO_VACIO',       N'El motivo del rechazo es obligatorio.',                                  N'Rejection reason is required.',                                                                                 N'Ablehnungsgrund ist erforderlich.'),
    (N'ERR_CHECKLIST_VACIO',            N'El checklist no tiene ítems. Agregá al menos uno antes de enviar a autorización.', N'The checklist has no items. Add at least one before sending to authorization.',                       N'Die Checkliste enthält keine Elemente. Füge mindestens eines hinzu, bevor du sie zur Autorisierung sendest.'),
    (N'ERR_CHECKLIST_NO_EXISTE',        N'La unidad no tiene checklist asociado.',                                 N'The unit has no associated checklist.',                                                                         N'Der Einheit ist keine Checkliste zugeordnet.'),
    (N'ERR_CHECKLIST_ITEMS_PENDIENTES', N'Hay ítems del checklist sin resultado cargado. Completá todos antes de finalizar.', N'There are checklist items without a result. Complete all before finalizing.',                          N'Es gibt Checklisten-Elemente ohne Ergebnis. Vervollständige alle, bevor du abschließt.'),
    (N'ERR_CHECKLIST_ITEMS_INMUTABLES', N'Solo se pueden modificar los ítems del checklist mientras la unidad esté en Ingresado.', N'Checklist items can only be modified while the unit is in ''Ingresado''.',                        N'Checklisten-Elemente können nur geändert werden, solange die Einheit im Status ''Ingresado'' ist.'),
    (N'ERR_ITEM_NOMBRE_VACIO',          N'El nombre del ítem es obligatorio.',                                     N'Item name is required.',                                                                                        N'Elementname ist erforderlich.'),
    (N'ERR_ITEM_NO_EJECUTABLE',         N'Solo se pueden ejecutar ítems del checklist cuando la unidad está En preparación.', N'Checklist items can only be executed when the unit is ''In preparation''.',                            N'Checklisten-Elemente können nur ausgeführt werden, wenn die Einheit ''In Vorbereitung'' ist.'),
    (N'ERR_RESULTADO_ITEM_VACIO',       N'El resultado del ítem no puede estar vacío.',                            N'Item result cannot be empty.',                                                                                  N'Das Element-Ergebnis darf nicht leer sein.'),
    (N'ERR_TEMPLATE_NOMBRE_VACIO',      N'El nombre del item del template es obligatorio.',                        N'Template item name is required.',                                                                               N'Der Name des Vorlagenelements ist erforderlich.'),
    (N'ERR_TEMPLATE_NO_ENCONTRADO',     N'No se encontró el item del template.',                                   N'Template item not found.',                                                                                      N'Vorlagenelement nicht gefunden.'),
    (N'ERR_SIN_PERMISO_TEMPLATE_CHECKLIST', N'No tenés permiso para gestionar el template del checklist.',         N'You don''t have permission to manage the checklist template.',                                                  N'Du hast keine Berechtigung, die Checklisten-Vorlage zu verwalten.'),
    -- PublicacionService / ImagenUnidadService
    (N'ERR_PRECIO_INVALIDO',            N'El precio no puede ser negativo.',                                       N'The price cannot be negative.',                                                                                 N'Der Preis darf nicht negativ sein.'),
    (N'ERR_IMAGEN_ARCHIVO_NO_EXISTE',   N'El archivo ''{0}'' no existe.',                                          N'The file ''{0}'' does not exist.',                                                                              N'Die Datei ''{0}'' existiert nicht.'),
    -- MarcaService
    (N'ERR_MARCA_NOMBRE_OBLIGATORIO',   N'El nombre de la marca es obligatorio.',                                  N'Brand name is required.',                                                                                       N'Markenname ist erforderlich.'),
    (N'ERR_MARCA_NOMBRE_DUPLICADO',     N'Ya existe una marca con el nombre ''{0}''.',                             N'A brand named ''{0}'' already exists.',                                                                         N'Eine Marke mit dem Namen ''{0}'' existiert bereits.'),
    (N'ERR_MARCA_NO_EXISTE',            N'La marca no existe.',                                                    N'The brand does not exist.',                                                                                     N'Die Marke existiert nicht.'),
    (N'ERR_MARCA_EN_USO',               N'No se puede eliminar la marca ''{0}'' porque hay {1} unidad(es) asociada(s).', N'Cannot delete brand ''{0}'' because {1} unit(s) are associated.',                                       N'Marke ''{0}'' kann nicht gelöscht werden, weil {1} Einheit(en) zugeordnet sind.'),
    -- ModeloService
    (N'ERR_MODELO_NOMBRE_OBLIGATORIO',  N'El nombre del modelo es obligatorio.',                                   N'Model name is required.',                                                                                       N'Modellname ist erforderlich.'),
    (N'ERR_MODELO_MARCA_OBLIGATORIA',   N'Debés seleccionar una marca para el modelo.',                            N'You must select a brand for the model.',                                                                        N'Du musst eine Marke für das Modell auswählen.'),
    (N'ERR_MODELO_NOMBRE_DUPLICADO',    N'Ya existe un modelo con el nombre ''{0}'' para esta marca.',             N'A model named ''{0}'' already exists for this brand.',                                                          N'Ein Modell mit dem Namen ''{0}'' existiert bereits für diese Marke.'),
    (N'ERR_MODELO_NO_EXISTE',           N'El modelo no existe.',                                                   N'The model does not exist.',                                                                                     N'Das Modell existiert nicht.'),
    (N'ERR_MODELO_EN_USO',              N'No se puede eliminar el modelo ''{0}'' porque hay {1} unidad(es) asociada(s).', N'Cannot delete model ''{0}'' because {1} unit(s) are associated.',                                       N'Modell ''{0}'' kann nicht gelöscht werden, weil {1} Einheit(en) zugeordnet sind.');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formErr FROM @ctrlErr;

DECLARE @idEsErr INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnErr INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeErr INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsErr, c.Id, t.Es FROM @ctrlErr t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formErr
UNION ALL
SELECT @idEnErr, c.Id, t.En FROM @ctrlErr t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formErr
UNION ALL
SELECT @idDeErr, c.Id, t.De FROM @ctrlErr t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formErr;
GO

-- ============================================================
-- Enum TipoBitacora — Traducciones ES/EN
-- ============================================================
DECLARE @formTBit NVARCHAR(80) = N'TipoBitacora';
DECLARE @ctrlTBit TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlTBit (Codigo, Es, En, De) VALUES
    (N'Login',                  N'Inicio de sesión',         N'Login',                   N'Anmeldung'),
    (N'Logout',                 N'Cierre de sesión',         N'Logout',                  N'Abmeldung'),
    (N'RegistroUsuario',        N'Registro de usuario',      N'User registration',       N'Benutzerregistrierung'),
    (N'DesbloqueoUsuario',      N'Desbloqueo de usuario',    N'User unblock',            N'Benutzer entsperrt'),
    (N'BloqueoUsuario',         N'Bloqueo de usuario',       N'User block',              N'Benutzer gesperrt'),
    (N'Error',                  N'Error',                    N'Error',                   N'Fehler'),
    (N'LoginFallido',           N'Inicio de sesión fallido', N'Failed login',            N'Fehlgeschlagene Anmeldung'),
    (N'IntentoAccesoBloqueado', N'Intento acceso bloqueado', N'Blocked access attempt',  N'Blockierter Zugriffsversuch'),
    (N'AltaPermiso',            N'Alta de permiso',          N'Permission created',      N'Berechtigung erstellt'),
    (N'AsignacionPermiso',      N'Asignación de permiso',    N'Permission assignment',   N'Berechtigungszuweisung'),
    (N'AltaRol',                N'Alta de rol',              N'Role created',            N'Rolle erstellt'),
    (N'AsignacionRol',          N'Asignación de rol',        N'Role assignment',         N'Rollenzuweisung'),
    (N'AltaIdioma',             N'Alta de idioma',           N'Language created',        N'Sprache erstellt'),
    (N'EdicionUsuario',         N'Edición de usuario',       N'User edited',             N'Benutzer bearbeitet'),
    (N'Mantenimiento',          N'Mantenimiento',            N'Maintenance',             N'Wartung'),
    (N'RegistroUnidad',         N'Registro de unidad',                 N'Unit registered',                   N'Einheit registriert'),
    (N'TransicionUnidad',       N'Transición de estado de unidad',     N'Unit state transition',             N'Zustandsübergang der Einheit'),
    (N'GestionTemplateChecklist', N'Gestión de template de checklist', N'Checklist template management',     N'Verwaltung der Checklisten-Vorlage'),
    (N'AltaMarca',              N'Alta de marca',                      N'Brand created',                     N'Marke erstellt'),
    (N'AltaModelo',             N'Alta de modelo',                     N'Model created',                     N'Modell erstellt');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formTBit FROM @ctrlTBit;

DECLARE @idEsTBit INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnTBit INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeTBit INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsTBit, c.Id, t.Es FROM @ctrlTBit t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTBit
UNION ALL
SELECT @idEnTBit, c.Id, t.En FROM @ctrlTBit t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTBit
UNION ALL
SELECT @idDeTBit, c.Id, t.De FROM @ctrlTBit t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTBit;
GO

CREATE OR ALTER PROCEDURE [dbo].[ActualizarIdiomaUsuario]
    @UsuarioId INT,
    @IdIdioma  INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET IdIdioma = @IdIdioma WHERE Id = @UsuarioId;
END
GO

-- =========================================================
-- N01 — Gestión de compra y preparación de vehículos
-- Bloque autocontenido: drops idempotentes + tablas + SPs + seed.
-- =========================================================

-- Drops de FKs y tablas N01 (idempotentes, permiten re-run del script).
IF OBJECT_ID('dbo.FK_HistUnidad_Usuario','F')            IS NOT NULL ALTER TABLE dbo.HistorialEstadoUnidad DROP CONSTRAINT FK_HistUnidad_Usuario;
IF OBJECT_ID('dbo.FK_HistUnidad_Unidad','F')             IS NOT NULL ALTER TABLE dbo.HistorialEstadoUnidad DROP CONSTRAINT FK_HistUnidad_Unidad;
IF OBJECT_ID('dbo.FK_ChecklistItem_Revisor','F')        IS NOT NULL ALTER TABLE dbo.ChecklistItem         DROP CONSTRAINT FK_ChecklistItem_Revisor;
IF OBJECT_ID('dbo.FK_ChecklistItem_Template','F')        IS NOT NULL ALTER TABLE dbo.ChecklistItem         DROP CONSTRAINT FK_ChecklistItem_Template;
IF OBJECT_ID('dbo.FK_ChecklistItem_Checklist','F')       IS NOT NULL ALTER TABLE dbo.ChecklistItem         DROP CONSTRAINT FK_ChecklistItem_Checklist;
IF OBJECT_ID('dbo.FK_Checklist_Creador','F')             IS NOT NULL ALTER TABLE dbo.ChecklistPreparacion  DROP CONSTRAINT FK_Checklist_Creador;
IF OBJECT_ID('dbo.FK_Checklist_Unidad','F')              IS NOT NULL ALTER TABLE dbo.ChecklistPreparacion  DROP CONSTRAINT FK_Checklist_Unidad;
IF OBJECT_ID('dbo.FK_Unidad_Comprador','F')              IS NOT NULL ALTER TABLE dbo.Unidad                DROP CONSTRAINT FK_Unidad_Comprador;
IF OBJECT_ID('dbo.FK_Unidad_Persona','F')               IS NOT NULL ALTER TABLE dbo.Unidad                DROP CONSTRAINT FK_Unidad_Persona;
IF OBJECT_ID('dbo.FK_Modelo_Marca','F')                  IS NOT NULL ALTER TABLE dbo.Modelo                DROP CONSTRAINT FK_Modelo_Marca;
GO

IF OBJECT_ID('dbo.VentaUnidad','U')            IS NOT NULL DROP TABLE dbo.VentaUnidad;
IF OBJECT_ID('dbo.ImagenUnidad','U')           IS NOT NULL DROP TABLE dbo.ImagenUnidad;
IF OBJECT_ID('dbo.PublicacionUnidad','U')      IS NOT NULL DROP TABLE dbo.PublicacionUnidad;
IF OBJECT_ID('dbo.HistorialEstadoUnidad','U')  IS NOT NULL DROP TABLE dbo.HistorialEstadoUnidad;
IF OBJECT_ID('dbo.ChecklistItem','U')          IS NOT NULL DROP TABLE dbo.ChecklistItem;
IF OBJECT_ID('dbo.ChecklistPreparacion','U')   IS NOT NULL DROP TABLE dbo.ChecklistPreparacion;
IF OBJECT_ID('dbo.ChecklistItemTemplate','U')  IS NOT NULL DROP TABLE dbo.ChecklistItemTemplate;
IF OBJECT_ID('dbo.Unidad','U')                 IS NOT NULL DROP TABLE dbo.Unidad;
IF OBJECT_ID('dbo.Persona','U')               IS NOT NULL DROP TABLE dbo.Persona;
IF OBJECT_ID('dbo.Modelo','U')                 IS NOT NULL DROP TABLE dbo.Modelo;
IF OBJECT_ID('dbo.Marca','U')                  IS NOT NULL DROP TABLE dbo.Marca;
GO

-- ---------------------------------------------------------
-- Tablas
-- ---------------------------------------------------------

CREATE TABLE [dbo].[Marca](
    [Id]     [int]          IDENTITY(1,1) NOT NULL,
    [Nombre] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_Marca]        PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Marca_Nombre] UNIQUE ([Nombre])
)
GO

CREATE TABLE [dbo].[Modelo](
    [Id]             [int]          IDENTITY(1,1) NOT NULL,
    [Nombre]         [nvarchar](50) NOT NULL,
    [IdMarca]        [int]          NOT NULL,
    [TipoCarroceria] [int]          NOT NULL,         -- ver enum BE.Enums.TipoCarroceria
    CONSTRAINT [PK_Modelo]               PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Modelo_Nombre_Marca]  UNIQUE ([Nombre], [IdMarca]),
    CONSTRAINT [FK_Modelo_Marca]         FOREIGN KEY ([IdMarca]) REFERENCES [dbo].[Marca]([Id])
)
GO

CREATE TABLE [dbo].[Persona](
    [Id]              [int]            IDENTITY(1,1) NOT NULL,
    [TipoPersona]     [char](1)        NOT NULL,          -- 'F' Física / 'J' Jurídica
    [Documento]       [nvarchar](30)   NOT NULL,          -- DNI o CUIT
    [Nombre]          [nvarchar](150)  NOT NULL,          -- Nombre completo o Razón social
    [Domicilio]       [nvarchar](200)  NOT NULL,
    [Telefono]        [nvarchar](30)   NULL,
    [Email]           [nvarchar](255)  NULL,
    [EstadoCivil]     [int]            NOT NULL,          -- ver enum BE.Enums.EstadoCivil
    [FechaNacimiento] [date]           NOT NULL,
    [DVH]             [nvarchar](64)   NULL,
    CONSTRAINT [PK_Persona]           PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Persona_Tipo]      CHECK ([TipoPersona] IN ('F','J')),
    CONSTRAINT [UQ_Persona_Documento] UNIQUE ([Documento])
)
GO

CREATE TABLE [dbo].[Unidad](
    [Id]                 [int]            IDENTITY(1,1) NOT NULL,
    [Dominio]            [nvarchar](10)   NOT NULL,
    [Marca]              [nvarchar](50)   NOT NULL,
    [Modelo]             [nvarchar](50)   NOT NULL,
    [Anio]               [int]            NOT NULL,
    [Kilometraje]        [int]            NOT NULL,
    [PrecioCompra]       [decimal](18,2)  NOT NULL,
    [Descripcion]        [nvarchar](1000) NULL,
    [EstadoActual]       [tinyint]        NOT NULL DEFAULT 1,   -- ver enum BE.Enums.EstadoUnidad
    [IdPersona]         [int]            NOT NULL,
    [IdCompradorUsuario] [int]            NOT NULL,
    [FechaIngreso]       [datetime]       NOT NULL DEFAULT GETDATE(),
    [DVH]                [nvarchar](64)   NULL,
    CONSTRAINT [PK_Unidad]         PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Unidad_Dominio] UNIQUE ([Dominio]),
    CONSTRAINT [FK_Unidad_Persona]  FOREIGN KEY ([IdPersona])         REFERENCES [dbo].[Persona]([Id]),
    CONSTRAINT [FK_Unidad_Comprador] FOREIGN KEY ([IdCompradorUsuario]) REFERENCES [dbo].[Usuario]([Id])
)
GO

CREATE TABLE [dbo].[ChecklistItemTemplate](
    [Id]          [int]           IDENTITY(1,1) NOT NULL,
    [Nombre]      [nvarchar](100) NOT NULL,
    [Descripcion] [nvarchar](500) NULL,
    [Activo]      [bit]           NOT NULL DEFAULT 1,
    [DVH]         [nvarchar](64)  NULL,
    CONSTRAINT [PK_ChecklistItemTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[ChecklistPreparacion](
    [Id]               [int]      IDENTITY(1,1) NOT NULL,
    [IdUnidad]         [int]      NOT NULL,
    [FechaCreacion]    [datetime] NOT NULL DEFAULT GETDATE(),
    [IdCreadorUsuario] [int]      NOT NULL,
    CONSTRAINT [PK_ChecklistPreparacion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Checklist_Unidad]     UNIQUE ([IdUnidad]),
    CONSTRAINT [FK_Checklist_Unidad]     FOREIGN KEY ([IdUnidad])         REFERENCES [dbo].[Unidad]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Checklist_Creador]    FOREIGN KEY ([IdCreadorUsuario]) REFERENCES [dbo].[Usuario]([Id])
)
GO

CREATE TABLE [dbo].[ChecklistItem](
    [Id]                     [int]            IDENTITY(1,1) NOT NULL,
    [IdChecklist]            [int]            NOT NULL,
    [Nombre]                 [nvarchar](100)  NOT NULL,
    [Descripcion]            [nvarchar](500)  NULL,
    [EsDelTemplate]          [bit]            NOT NULL,
    [IdTemplateOrigen]       [int]            NULL,
    [ResultadoRevision]      [int]            NOT NULL DEFAULT 1,   -- 1=Pendiente, 2=OK, 3=Observado
    [ComentarioRevision]     [nvarchar](500)  NULL,
    [FechaRevision]          [datetime]       NULL,
    [IdUsuarioRevisor]       [int]            NULL,
    [CostoEstimado]          [decimal](18,2)  NULL,                 -- sólo tiene sentido en extras
    [EstadoAprobacion]       [int]            NOT NULL DEFAULT 1,   -- 1=Aprobado, 2=Propuesto, 3=Rechazado
    CONSTRAINT [PK_ChecklistItem]           PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ChecklistItem_Checklist] FOREIGN KEY ([IdChecklist])       REFERENCES [dbo].[ChecklistPreparacion]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ChecklistItem_Template]  FOREIGN KEY ([IdTemplateOrigen])  REFERENCES [dbo].[ChecklistItemTemplate]([Id]),
    CONSTRAINT [FK_ChecklistItem_Revisor]   FOREIGN KEY ([IdUsuarioRevisor])  REFERENCES [dbo].[Usuario]([Id])
)
GO

CREATE TABLE [dbo].[HistorialEstadoUnidad](
    [Id]             [int]           IDENTITY(1,1) NOT NULL,
    [IdUnidad]       [int]           NOT NULL,
    [EstadoOrigen]   [tinyint]       NULL,          -- NULL en el primer registro (alta)
    [EstadoDestino]  [tinyint]       NOT NULL,
    [IdUsuario]      [int]           NOT NULL,
    [FechaHora]      [datetime]      NOT NULL DEFAULT GETDATE(),
    [Motivo]         [nvarchar](500) NULL,
    CONSTRAINT [PK_HistorialEstadoUnidad] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_HistUnidad_Unidad]     FOREIGN KEY ([IdUnidad])  REFERENCES [dbo].[Unidad]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_HistUnidad_Usuario]    FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario]([Id])
)
GO

-- =========================================================
-- N02 — Publicación / Imágenes / Venta
-- =========================================================

CREATE TABLE [dbo].[PublicacionUnidad](
    [Id]                     [int]            IDENTITY(1,1) NOT NULL,
    [IdUnidad]               [int]            NOT NULL,
    [PrecioPublicacion]      [decimal](18,2)  NULL,
    [DescripcionPublicacion] [nvarchar](MAX)  NULL,
    [FechaCreacion]          [datetime]       NOT NULL DEFAULT GETDATE(),
    [FechaUltimaEdicion]     [datetime]       NULL,
    CONSTRAINT [PK_PublicacionUnidad]    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Publicacion_Unidad]   UNIQUE ([IdUnidad]),
    CONSTRAINT [FK_Publicacion_Unidad]   FOREIGN KEY ([IdUnidad]) REFERENCES [dbo].[Unidad]([Id]) ON DELETE CASCADE
)
GO

CREATE TABLE [dbo].[ImagenUnidad](
    [Id]          [int]           IDENTITY(1,1) NOT NULL,
    [IdUnidad]    [int]           NOT NULL,
    [RutaArchivo] [nvarchar](500) NOT NULL,          -- relativa a %ProgramData%\Avanti Auto\Imagenes ("{IdUnidad}\archivo.ext")
    [Orden]       [int]           NOT NULL DEFAULT 0,
    [FechaCarga]  [datetime]      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ImagenUnidad]        PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ImagenUnidad_Unidad] FOREIGN KEY ([IdUnidad]) REFERENCES [dbo].[Unidad]([Id]) ON DELETE CASCADE
)
GO

CREATE TABLE [dbo].[VentaUnidad](
    [Id]                     [int]           IDENTITY(1,1) NOT NULL,
    [IdUnidad]               [int]           NOT NULL,
    [Tipo]                   [int]           NOT NULL,                 -- 1=Venta, 2=Reserva
    [Activa]                 [bit]           NOT NULL DEFAULT 1,
    [IdPersonaComprador]     [int]           NOT NULL,
    [IdVendedorUsuario]      [int]           NOT NULL,
    [FechaOperacion]         [datetime]      NOT NULL DEFAULT GETDATE(),
    [PrecioAcordado]         [decimal](18,2) NOT NULL,
    [MontoSena]              [decimal](18,2) NULL,
    [FechaEstimadaFin]       [date]          NULL,
    [Descripcion]            [nvarchar](MAX) NULL,
    [ComentarioCancelacion]  [nvarchar](500) NULL,
    CONSTRAINT [PK_VentaUnidad]            PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VentaUnidad_Unidad]     FOREIGN KEY ([IdUnidad])           REFERENCES [dbo].[Unidad]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_VentaUnidad_Comprador]  FOREIGN KEY ([IdPersonaComprador]) REFERENCES [dbo].[Persona]([Id]),
    CONSTRAINT [FK_VentaUnidad_Vendedor]   FOREIGN KEY ([IdVendedorUsuario])  REFERENCES [dbo].[Usuario]([Id])
)
GO

-- ---------------------------------------------------------
-- SPs — Marca
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[InsertarMarca]
    @Nombre NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Marca (Nombre) VALUES (@Nombre);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarMarcas]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre FROM Marca ORDER BY Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerMarcaPorNombre]
    @Nombre NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre FROM Marca WHERE Nombre = @Nombre;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EditarMarca]
    @Id     INT,
    @Nombre NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Marca SET Nombre = @Nombre WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EliminarMarca]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Marca WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ContarUnidadesConMarca]
    @Nombre NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM Unidad WHERE Marca = @Nombre;
END
GO

-- ---------------------------------------------------------
-- SPs — Modelo
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[InsertarModelo]
    @Nombre         NVARCHAR(50),
    @IdMarca        INT,
    @TipoCarroceria INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Modelo (Nombre, IdMarca, TipoCarroceria) VALUES (@Nombre, @IdMarca, @TipoCarroceria);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EditarModelo]
    @Id             INT,
    @Nombre         NVARCHAR(50),
    @IdMarca        INT,
    @TipoCarroceria INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Modelo
       SET Nombre         = @Nombre,
           IdMarca        = @IdMarca,
           TipoCarroceria = @TipoCarroceria
     WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EliminarModelo]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Modelo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarModelos]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, IdMarca, TipoCarroceria FROM Modelo ORDER BY Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarModelosPorMarca]
    @IdMarca INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, IdMarca, TipoCarroceria
    FROM Modelo WHERE IdMarca = @IdMarca ORDER BY Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerModeloPorNombreYMarca]
    @Nombre  NVARCHAR(50),
    @IdMarca INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, IdMarca, TipoCarroceria
    FROM Modelo WHERE Nombre = @Nombre AND IdMarca = @IdMarca;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ContarUnidadesConModelo]
    @Nombre NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM Unidad WHERE Modelo = @Nombre;
END
GO

-- ---------------------------------------------------------
-- SPs — Persona
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[InsertarPersona]
    @TipoPersona     CHAR(1),
    @Documento       NVARCHAR(30),
    @Nombre          NVARCHAR(150),
    @Domicilio       NVARCHAR(200),
    @Telefono        NVARCHAR(30)  = NULL,
    @Email           NVARCHAR(255) = NULL,
    @EstadoCivil     INT,
    @FechaNacimiento DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Persona (TipoPersona, Documento, Nombre, Domicilio, Telefono, Email, EstadoCivil, FechaNacimiento)
    VALUES (@TipoPersona, @Documento, @Nombre, @Domicilio, @Telefono, @Email, @EstadoCivil, @FechaNacimiento);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EditarPersona]
    @PersonaId       INT,
    @TipoPersona     CHAR(1),
    @Documento       NVARCHAR(30),
    @Nombre          NVARCHAR(150),
    @Domicilio       NVARCHAR(200),
    @Telefono        NVARCHAR(30)  = NULL,
    @Email           NVARCHAR(255) = NULL,
    @EstadoCivil     INT,
    @FechaNacimiento DATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Persona
       SET TipoPersona     = @TipoPersona,
           Documento       = @Documento,
           Nombre          = @Nombre,
           Domicilio       = @Domicilio,
           Telefono        = @Telefono,
           Email           = @Email,
           EstadoCivil     = @EstadoCivil,
           FechaNacimiento = @FechaNacimiento
     WHERE Id = @PersonaId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EliminarPersona]
    @PersonaId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Persona WHERE Id = @PersonaId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerPersonaPorId]
    @PersonaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, TipoPersona, Documento, Nombre, Domicilio, Telefono, Email, EstadoCivil, FechaNacimiento, DVH
    FROM Persona WHERE Id = @PersonaId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerPersonaPorDocumento]
    @Documento NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, TipoPersona, Documento, Nombre, Domicilio, Telefono, Email, EstadoCivil, FechaNacimiento, DVH
    FROM Persona WHERE Documento = @Documento;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[BuscarPersonasPorDocumentoLike]
    @Fragmento NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, TipoPersona, Documento, Nombre, Domicilio, Telefono, Email, EstadoCivil, FechaNacimiento, DVH
    FROM Persona
    WHERE Documento LIKE N'%' + @Fragmento + N'%'
    ORDER BY Documento ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarPersonas]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, TipoPersona, Documento, Nombre, Domicilio, Telefono, Email, EstadoCivil, FechaNacimiento, DVH
    FROM Persona ORDER BY Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ActualizarDVHPersona]
    @PersonaId INT,
    @DVH       NVARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Persona SET DVH = @DVH WHERE Id = @PersonaId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarPersonasParaVerificacion]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, TipoPersona, Documento, Nombre,
           Domicilio,
           ISNULL(Telefono, N'') AS Telefono,
           ISNULL(Email,    N'') AS Email,
           EstadoCivil,
           FechaNacimiento,
           DVH
    FROM Persona ORDER BY Id ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ContarUnidadesConPersona]
    @PersonaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM Unidad WHERE IdPersona = @PersonaId;
END
GO

-- ---------------------------------------------------------
-- SPs — Unidad
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[InsertarUnidad]
    @Dominio            NVARCHAR(10),
    @Marca              NVARCHAR(50),
    @Modelo             NVARCHAR(50),
    @Anio               INT,
    @Kilometraje        INT,
    @PrecioCompra       DECIMAL(18,2),
    @Descripcion        NVARCHAR(1000) = NULL,
    @IdPersona         INT,
    @IdCompradorUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Unidad (Dominio, Marca, Modelo, Anio, Kilometraje, PrecioCompra,
                        Descripcion, EstadoActual, IdPersona, IdCompradorUsuario)
    VALUES (@Dominio, @Marca, @Modelo, @Anio, @Kilometraje, @PrecioCompra,
            @Descripcion, 1, @IdPersona, @IdCompradorUsuario);
    DECLARE @NuevoId INT = SCOPE_IDENTITY();

    -- Snapshot inicial en historial: alta → Ingresado.
    INSERT INTO HistorialEstadoUnidad (IdUnidad, EstadoOrigen, EstadoDestino, IdUsuario, Motivo)
    VALUES (@NuevoId, NULL, 1, @IdCompradorUsuario, NULL);

    SELECT @NuevoId AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerUnidadPorId]
    @UnidadId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Dominio, Marca, Modelo, Anio, Kilometraje, PrecioCompra, Descripcion,
           EstadoActual, IdPersona, IdCompradorUsuario, FechaIngreso, DVH
    FROM Unidad WHERE Id = @UnidadId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerUnidadPorDominio]
    @Dominio NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Dominio, Marca, Modelo, Anio, Kilometraje, PrecioCompra, Descripcion,
           EstadoActual, IdPersona, IdCompradorUsuario, FechaIngreso, DVH
    FROM Unidad WHERE Dominio = @Dominio;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarUnidades]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Dominio, Marca, Modelo, Anio, Kilometraje, PrecioCompra, Descripcion,
           EstadoActual, IdPersona, IdCompradorUsuario, FechaIngreso, DVH
    FROM Unidad ORDER BY FechaIngreso DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarUnidadesPorEstado]
    @Estado TINYINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Dominio, Marca, Modelo, Anio, Kilometraje, PrecioCompra, Descripcion,
           EstadoActual, IdPersona, IdCompradorUsuario, FechaIngreso, DVH
    FROM Unidad
    WHERE EstadoActual = @Estado
    ORDER BY FechaIngreso ASC;
END
GO

-- Transición de estado atómica: UPDATE unidad + INSERT historial en una sola operación
-- lógica. Se llama desde EscribirConTransaccion en el mapper.
CREATE OR ALTER PROCEDURE [dbo].[TransicionarEstadoUnidad]
    @UnidadId       INT,
    @EstadoOrigen   TINYINT,
    @EstadoDestino  TINYINT,
    @IdUsuario      INT,
    @Motivo         NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Unidad SET EstadoActual = @EstadoDestino WHERE Id = @UnidadId;
    INSERT INTO HistorialEstadoUnidad (IdUnidad, EstadoOrigen, EstadoDestino, IdUsuario, Motivo)
    VALUES (@UnidadId, @EstadoOrigen, @EstadoDestino, @IdUsuario, @Motivo);
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ActualizarDVHUnidad]
    @UnidadId INT,
    @DVH      NVARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Unidad SET DVH = @DVH WHERE Id = @UnidadId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarUnidadesParaVerificacion]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Dominio, Marca, Modelo, Anio, Kilometraje, PrecioCompra,
           ISNULL(Descripcion, N'') AS Descripcion,
           EstadoActual, IdPersona, IdCompradorUsuario, FechaIngreso, DVH
    FROM Unidad ORDER BY Id ASC;
END
GO

-- ---------------------------------------------------------
-- SPs — ChecklistItemTemplate (catálogo)
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[InsertarChecklistItemTemplate]
    @Nombre      NVARCHAR(100),
    @Descripcion NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ChecklistItemTemplate (Nombre, Descripcion, Activo)
    VALUES (@Nombre, @Descripcion, 1);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EditarDescripcionChecklistItemTemplate]
    @TemplateId  INT,
    @Descripcion NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ChecklistItemTemplate SET Descripcion = @Descripcion WHERE Id = @TemplateId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[BajaLogicaChecklistItemTemplate]
    @TemplateId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ChecklistItemTemplate SET Activo = 0 WHERE Id = @TemplateId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ReactivarChecklistItemTemplate]
    @TemplateId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ChecklistItemTemplate SET Activo = 1 WHERE Id = @TemplateId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarChecklistItemsTemplate]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Descripcion, Activo, DVH
    FROM ChecklistItemTemplate ORDER BY Activo DESC, Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarChecklistItemsTemplateActivos]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Descripcion, Activo, DVH
    FROM ChecklistItemTemplate WHERE Activo = 1 ORDER BY Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerChecklistItemTemplatePorId]
    @TemplateId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Descripcion, Activo, DVH
    FROM ChecklistItemTemplate WHERE Id = @TemplateId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ActualizarDVHChecklistItemTemplate]
    @TemplateId INT,
    @DVH        NVARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ChecklistItemTemplate SET DVH = @DVH WHERE Id = @TemplateId;
END
GO

-- ---------------------------------------------------------
-- SPs — Checklist de una unidad
-- ---------------------------------------------------------

-- Crea el checklist de una unidad y copia (snapshot) todos los items activos del template.
-- Devuelve el Id del checklist creado.
CREATE OR ALTER PROCEDURE [dbo].[CrearChecklistDeUnidad]
    @IdUnidad         INT,
    @IdCreadorUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ChecklistPreparacion (IdUnidad, IdCreadorUsuario)
    VALUES (@IdUnidad, @IdCreadorUsuario);
    DECLARE @IdChecklist INT = SCOPE_IDENTITY();

    -- Items del template arrancan en estado Aprobado (1) — son parte del standard.
    INSERT INTO ChecklistItem (IdChecklist, Nombre, Descripcion, EsDelTemplate, IdTemplateOrigen, EstadoAprobacion)
    SELECT @IdChecklist, Nombre, Descripcion, 1, Id, 1
    FROM ChecklistItemTemplate WHERE Activo = 1;

    SELECT @IdChecklist AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerChecklistPorUnidad]
    @IdUnidad INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, IdUnidad, FechaCreacion, IdCreadorUsuario
    FROM ChecklistPreparacion WHERE IdUnidad = @IdUnidad;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarItemsDeChecklist]
    @IdChecklist INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, IdChecklist, Nombre, Descripcion, EsDelTemplate, IdTemplateOrigen,
           ResultadoRevision, ComentarioRevision, FechaRevision, IdUsuarioRevisor,
           CostoEstimado, EstadoAprobacion
    FROM ChecklistItem
    WHERE IdChecklist = @IdChecklist
    ORDER BY EsDelTemplate DESC, Id ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[AgregarItemExtraAChecklist]
    @IdChecklist   INT,
    @Nombre        NVARCHAR(100),
    @Descripcion   NVARCHAR(500) = NULL,
    @CostoEstimado DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    -- Extras arrancan en estado Propuesto (2) con el costo estimado.
    INSERT INTO ChecklistItem (IdChecklist, Nombre, Descripcion, EsDelTemplate, IdTemplateOrigen,
                               CostoEstimado, EstadoAprobacion)
    VALUES (@IdChecklist, @Nombre, @Descripcion, 0, NULL, @CostoEstimado, 2);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[AprobarItemsExtraPropuestos]
    @IdChecklist INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Aprueba (EstadoAprobacion=1) todos los extras propuestos (EstadoAprobacion=2) del checklist.
    UPDATE ChecklistItem
       SET EstadoAprobacion = 1
     WHERE IdChecklist = @IdChecklist AND EsDelTemplate = 0 AND EstadoAprobacion = 2;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[RechazarItemsExtraPropuestos]
    @IdChecklist INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Rechaza (EstadoAprobacion=3) todos los extras propuestos del checklist.
    -- El motivo global se guarda en HistorialEstadoUnidad.Motivo al transicionar la unidad.
    UPDATE ChecklistItem
       SET EstadoAprobacion = 3
     WHERE IdChecklist = @IdChecklist AND EsDelTemplate = 0 AND EstadoAprobacion = 2;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ContarItemsExtraPropuestos]
    @IdChecklist INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM ChecklistItem
    WHERE IdChecklist = @IdChecklist AND EsDelTemplate = 0 AND EstadoAprobacion = 2;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EliminarItemExtraDeChecklist]
    @IdItem INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ChecklistItem WHERE Id = @IdItem AND EsDelTemplate = 0;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[MarcarItemChecklistRevisado]
    @IdItem             INT,
    @ResultadoRevision  INT,
    @ComentarioRevision NVARCHAR(500) = NULL,
    @IdUsuarioRevisor   INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ChecklistItem
       SET ResultadoRevision  = @ResultadoRevision,
           ComentarioRevision = @ComentarioRevision,
           FechaRevision      = GETDATE(),
           IdUsuarioRevisor   = @IdUsuarioRevisor
     WHERE Id = @IdItem;
END
GO

-- ---------------------------------------------------------
-- SPs — Historial de estados
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[ListarHistorialEstadoUnidad]
    @IdUnidad INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.Id, h.IdUnidad, h.EstadoOrigen, h.EstadoDestino, h.IdUsuario, h.FechaHora, h.Motivo,
           u.Username
    FROM HistorialEstadoUnidad h
    INNER JOIN Usuario u ON u.Id = h.IdUsuario
    WHERE h.IdUnidad = @IdUnidad
    ORDER BY h.FechaHora ASC, h.Id ASC;
END
GO

-- ---------------------------------------------------------
-- SPs — PublicacionUnidad
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[ObtenerPublicacionPorUnidad]
    @IdUnidad INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, IdUnidad, PrecioPublicacion, DescripcionPublicacion, FechaCreacion, FechaUltimaEdicion
    FROM PublicacionUnidad WHERE IdUnidad = @IdUnidad;
END
GO

-- Publicaciones ya iniciadas (alguien abrió el modal de publicación al menos una vez)
-- de unidades En venta (5) o Pendiente de venta (4).
CREATE OR ALTER PROCEDURE [dbo].[ListarPublicaciones]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.Id AS IdUnidad, u.Dominio, u.Marca, u.Modelo, u.Anio, u.Kilometraje, u.EstadoActual,
           p.PrecioPublicacion, p.DescripcionPublicacion, p.FechaCreacion, p.FechaUltimaEdicion,
           (SELECT COUNT(*) FROM ImagenUnidad i WHERE i.IdUnidad = u.Id) AS CantidadImagenes
    FROM PublicacionUnidad p
    INNER JOIN Unidad u ON u.Id = p.IdUnidad
    WHERE u.EstadoActual IN (4, 5)
    ORDER BY ISNULL(p.FechaUltimaEdicion, p.FechaCreacion) DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[CrearPublicacion]
    @IdUnidad INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM PublicacionUnidad WHERE IdUnidad = @IdUnidad)
    BEGIN
        INSERT INTO PublicacionUnidad (IdUnidad) VALUES (@IdUnidad);
    END
    SELECT Id FROM PublicacionUnidad WHERE IdUnidad = @IdUnidad;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ActualizarPublicacion]
    @IdUnidad               INT,
    @PrecioPublicacion      DECIMAL(18,2) = NULL,
    @DescripcionPublicacion NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE PublicacionUnidad
       SET PrecioPublicacion      = @PrecioPublicacion,
           DescripcionPublicacion = @DescripcionPublicacion,
           FechaUltimaEdicion     = GETDATE()
     WHERE IdUnidad = @IdUnidad;
END
GO

-- ---------------------------------------------------------
-- SPs — ImagenUnidad
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[AgregarImagenUnidad]
    @IdUnidad    INT,
    @RutaArchivo NVARCHAR(500),
    @Orden       INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ImagenUnidad (IdUnidad, RutaArchivo, Orden) VALUES (@IdUnidad, @RutaArchivo, @Orden);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EliminarImagenUnidad]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ImagenUnidad WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerImagenUnidadPorId]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, IdUnidad, RutaArchivo, Orden, FechaCarga
    FROM ImagenUnidad
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarImagenesPorUnidad]
    @IdUnidad INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, IdUnidad, RutaArchivo, Orden, FechaCarga
    FROM ImagenUnidad
    WHERE IdUnidad = @IdUnidad
    ORDER BY Orden ASC, Id ASC;
END
GO

-- ---------------------------------------------------------
-- SPs — VentaUnidad
-- ---------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[InsertarVentaUnidad]
    @IdUnidad              INT,
    @Tipo                  INT,
    @IdPersonaComprador    INT,
    @IdVendedorUsuario     INT,
    @FechaOperacion        DATETIME,
    @PrecioAcordado        DECIMAL(18,2),
    @MontoSena             DECIMAL(18,2) = NULL,
    @FechaEstimadaFin      DATE          = NULL,
    @Descripcion           NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO VentaUnidad (IdUnidad, Tipo, Activa, IdPersonaComprador, IdVendedorUsuario,
                             FechaOperacion, PrecioAcordado, MontoSena, FechaEstimadaFin, Descripcion)
    VALUES (@IdUnidad, @Tipo, 1, @IdPersonaComprador, @IdVendedorUsuario,
            @FechaOperacion, @PrecioAcordado, @MontoSena, @FechaEstimadaFin, @Descripcion);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[CancelarVentaActivaDeUnidad]
    @IdUnidad   INT,
    @Comentario NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE VentaUnidad
       SET Activa = 0,
           ComentarioCancelacion = @Comentario
     WHERE IdUnidad = @IdUnidad AND Activa = 1;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ObtenerVentaActivaDeUnidad]
    @IdUnidad INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, IdUnidad, Tipo, Activa, IdPersonaComprador, IdVendedorUsuario,
           FechaOperacion, PrecioAcordado, MontoSena, FechaEstimadaFin, Descripcion, ComentarioCancelacion
    FROM VentaUnidad
    WHERE IdUnidad = @IdUnidad AND Activa = 1;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarVentasPorUnidad]
    @IdUnidad INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, IdUnidad, Tipo, Activa, IdPersonaComprador, IdVendedorUsuario,
           FechaOperacion, PrecioAcordado, MontoSena, FechaEstimadaFin, Descripcion, ComentarioCancelacion
    FROM VentaUnidad
    WHERE IdUnidad = @IdUnidad
    ORDER BY FechaOperacion DESC, Id DESC;
END
GO

-- ---------------------------------------------------------
-- Seed — Permisos, roles, usuario Gerente
-- ---------------------------------------------------------

INSERT INTO Permiso (Codigo, Descripcion, Tipo) VALUES
    ('REGISTRAR_UNIDAD',            'Habilita registrar una unidad ingresada.',                   'I'),
    ('REVISAR_UNIDAD',              'Habilita revisar una unidad y armar/enviar checklist.',      'I'),
    ('AUTORIZAR_CHECKLIST',         'Habilita autorizar o rechazar el checklist propuesto.',      'I'),
    ('EJECUTAR_PREPARACION',        'Habilita ejecutar los items del checklist autorizado.',      'I'),
    ('AUTORIZAR_PUBLICACION',       'Habilita autorizar o rechazar la publicación a venta.',      'I'),
    ('CONSULTAR_UNIDAD',            'Habilita consultar la trazabilidad completa de una unidad.', 'I'),
    ('GESTIONAR_TEMPLATE_CHECKLIST','Habilita administrar el catálogo de items del checklist.',   'I'),
    ('GESTIONAR_MARCAS',            'Habilita dar de alta marcas en el catálogo.',                'I'),
    ('GESTIONAR_MODELOS',           'Habilita dar de alta modelos en el catálogo.',               'I'),
    ('GESTIONAR_PERSONAS',          'Habilita el ABM del catálogo de personas (compradores/vendedores).', 'I'),
    -- N02 — ventas
    ('GESTIONAR_PUBLICACION',       'Habilita editar precio/descripción/imágenes de la publicación de una unidad.', 'I'),
    ('REGISTRAR_VENTA',             'Habilita registrar una venta concretada sobre una unidad.',  'I'),
    ('GESTIONAR_RESERVA',           'Habilita reservar una unidad y cancelar reservas.',          'I'),
    ('PAUSAR_UNIDAD',               'Habilita pausar y reanudar unidades en venta.',              'I'),
    ('VER_PUBLICACIONES',           'Habilita el menú Publicaciones y el listado de publicaciones.', 'I')
GO

-- Roles nuevos: GERENTE y VENDEDOR
INSERT INTO Permiso (Codigo, Descripcion, Tipo) VALUES
    ('GERENTE',  'Rol gerencial: autoriza checklists y publicación a venta.',       'R'),
    ('VENDEDOR', 'Rol comercial de ventas: publica, vende, reserva y pausa unidades.', 'R')
GO

-- Anidar los permisos individuales bajo los roles correspondientes (patrón Composite).
-- Nota: IdPadre es escalar (un permiso vive bajo un solo rol). Los permisos compartidos
-- entre múltiples roles (CONSULTAR_UNIDAD, GESTIONAR_PERSONAS, y los que necesita el
-- Gerente además de su rol propio) se asignan directamente por usuario más abajo.

-- COMPRADOR: registra unidades (requiere dar de alta marca/modelo si no existen).
UPDATE Permiso SET IdPadre = (SELECT Id FROM Permiso WHERE Codigo = 'COMPRADOR')
    WHERE Codigo IN ('REGISTRAR_UNIDAD', 'GESTIONAR_MARCAS', 'GESTIONAR_MODELOS');

-- TALLER: revisa unidad (incluye armar/quitar items extra) y ejecuta la preparación.
UPDATE Permiso SET IdPadre = (SELECT Id FROM Permiso WHERE Codigo = 'TALLER')
    WHERE Codigo IN ('REVISAR_UNIDAD', 'EJECUTAR_PREPARACION');

-- GERENTE: autoriza presupuestos/publicación, pausa publicaciones y maneja el template.
UPDATE Permiso SET IdPadre = (SELECT Id FROM Permiso WHERE Codigo = 'GERENTE')
    WHERE Codigo IN ('AUTORIZAR_CHECKLIST', 'AUTORIZAR_PUBLICACION', 'PAUSAR_UNIDAD', 'GESTIONAR_TEMPLATE_CHECKLIST');

-- VENDEDOR: edita la publicación, vende y reserva unidades.
UPDATE Permiso SET IdPadre = (SELECT Id FROM Permiso WHERE Codigo = 'VENDEDOR')
    WHERE Codigo IN ('GESTIONAR_PUBLICACION', 'REGISTRAR_VENTA', 'GESTIONAR_RESERVA');
GO

-- Usuarios Gerente y Vendedor seed (mismo hash/salt que los demás, password '123456789')
INSERT INTO Usuario (Username, Hash, Salt, Nombre, Apellido, Email, Telefono, Documento, Domicilio, DVH)
VALUES
(
    'gerente',
    '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
    'VQUk8CQ1S2V3oSbMWAp4qg==',
    'Gerente',
    'Comercial',
    'gerente@avanti.com',
    '+54 11 5555-0004',
    '30123458',
    'Av. del Libertador 1000, CABA',
    NULL
),
(
    'vendedor',
    '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
    'VQUk8CQ1S2V3oSbMWAp4qg==',
    'Vendedor',
    'Comercial',
    'vendedor@avanti.com',
    '+54 11 5555-0005',
    '30123459',
    'Av. Cabildo 2500, CABA',
    NULL
)
GO

-- Snapshot inicial en historial de Usuario para gerente y vendedor
INSERT INTO UsuarioHistorial
    (UsuarioId, Accion, ModificadoPorUsuarioId,
     Nombre, Apellido, Email, Telefono, Documento, Domicilio, Bloqueado)
SELECT Id, N'Alta', NULL,
       Nombre, Apellido, Email, Telefono, Documento, Domicilio, Bloqueado
FROM Usuario WHERE Username IN ('gerente', 'vendedor');
GO

-- Asignar roles a los usuarios seed.
INSERT INTO UsuarioPermiso (UsuarioId, PermisoId)
SELECT u.Id, p.Id FROM Usuario u, Permiso p
WHERE u.Username = 'gerente' AND p.Codigo = 'GERENTE';

INSERT INTO UsuarioPermiso (UsuarioId, PermisoId)
SELECT u.Id, p.Id FROM Usuario u, Permiso p
WHERE u.Username = 'vendedor' AND p.Codigo = 'VENDEDOR';

-- Permisos compartidos — asignación directa a los usuarios (no viven bajo un rol porque
-- IdPadre es escalar y varios roles los necesitan).

-- CONSULTAR_UNIDAD → los 4 roles del dominio (ver listado + detalle de unidades).
INSERT INTO UsuarioPermiso (UsuarioId, PermisoId)
SELECT u.Id, p.Id FROM Usuario u, Permiso p
WHERE u.Username IN ('comprador', 'taller', 'gerente', 'vendedor')
  AND p.Codigo = 'CONSULTAR_UNIDAD';

-- GESTIONAR_PERSONAS → comprador, vendedor y gerente (dar de alta personas al registrar
-- unidad / vender / reservar). Taller no la necesita.
INSERT INTO UsuarioPermiso (UsuarioId, PermisoId)
SELECT u.Id, p.Id FROM Usuario u, Permiso p
WHERE u.Username IN ('comprador', 'vendedor', 'gerente')
  AND p.Codigo = 'GESTIONAR_PERSONAS';

-- VER_PUBLICACIONES → comprador, vendedor y gerente (taller no). Admin lo tiene por bypass.
INSERT INTO UsuarioPermiso (UsuarioId, PermisoId)
SELECT u.Id, p.Id FROM Usuario u, Permiso p
WHERE u.Username IN ('comprador', 'vendedor', 'gerente')
  AND p.Codigo = 'VER_PUBLICACIONES';

-- PAUSAR_UNIDAD → vendedor también (vive bajo el rol GERENTE por IdPadre; se asigna
-- directo al vendedor para que también pueda pausar/reanudar).
INSERT INTO UsuarioPermiso (UsuarioId, PermisoId)
SELECT u.Id, p.Id FROM Usuario u, Permiso p
WHERE u.Username = 'vendedor' AND p.Codigo = 'PAUSAR_UNIDAD';

-- El Gerente además puede hacer todo lo que hace Comprador y Vendedor: le asignamos
-- directamente los permisos que viven bajo esos roles (no podemos re-parentar porque
-- cada permiso tiene un único IdPadre).
INSERT INTO UsuarioPermiso (UsuarioId, PermisoId)
SELECT u.Id, p.Id FROM Usuario u, Permiso p
WHERE u.Username = 'gerente'
  AND p.Codigo IN ('REGISTRAR_UNIDAD', 'GESTIONAR_MARCAS', 'GESTIONAR_MODELOS',
                   'GESTIONAR_PUBLICACION', 'REGISTRAR_VENTA', 'GESTIONAR_RESERVA');
GO

-- Seed inicial del template de checklist (algunos items típicos para arrancar).
INSERT INTO ChecklistItemTemplate (Nombre, Descripcion, Activo) VALUES
    (N'Revisión general de motor',       N'Verificar niveles, fugas y estado general.',         1),
    (N'Revisión de sistema de frenos',   N'Pastillas, discos, líquido y respuesta del pedal.',  1),
    (N'Revisión de neumáticos',          N'Estado, presión y profundidad de dibujo.',           1),
    (N'Limpieza y detallado interior',   N'Aspirado, tapizados, tablero y vidrios.',            1),
    (N'Limpieza y detallado exterior',   N'Lavado, encerado y revisión de carrocería.',         1)
GO

-- Seed de marcas (50 marcas típicas a nivel mundial)
INSERT INTO Marca (Nombre) VALUES
    (N'Abarth'), (N'Acura'), (N'Alfa Romeo'), (N'Aston Martin'), (N'Audi'),
    (N'Bentley'), (N'BMW'), (N'Buick'), (N'BYD'), (N'Cadillac'),
    (N'Chery'), (N'Chevrolet'), (N'Chrysler'), (N'Citroën'), (N'Dacia'),
    (N'Daewoo'), (N'Dodge'), (N'DS Automobiles'), (N'Ferrari'), (N'Fiat'),
    (N'Ford'), (N'Geely'), (N'GMC'), (N'Great Wall'), (N'Honda'),
    (N'Hyundai'), (N'Infiniti'), (N'Isuzu'), (N'Jaguar'), (N'Jeep'),
    (N'Kia'), (N'Lamborghini'), (N'Land Rover'), (N'Lexus'), (N'Lincoln'),
    (N'Maserati'), (N'Mazda'), (N'McLaren'), (N'Mercedes-Benz'), (N'Mini'),
    (N'Mitsubishi'), (N'Nissan'), (N'Opel'), (N'Peugeot'), (N'Porsche'),
    (N'Ram'), (N'Renault'), (N'Rolls-Royce'), (N'Seat'), (N'Škoda'),
    (N'Smart'), (N'SsangYong'), (N'Subaru'), (N'Suzuki'), (N'Tesla'),
    (N'Toyota'), (N'Volkswagen'), (N'Volvo')
GO

-- Seed de modelos (50 modelos típicos). TipoCarroceria: 1=Sedán, 2=Hatchback, 3=SUV, 4=Pickup, 5=Coupe.
INSERT INTO Modelo (Nombre, IdMarca, TipoCarroceria)
SELECT v.Nombre, (SELECT Id FROM Marca WHERE Nombre = v.Marca), v.Tipo
FROM (VALUES
    -- Toyota
    (N'Corolla',       N'Toyota',       1),
    (N'Yaris',         N'Toyota',       2),
    (N'Hilux',         N'Toyota',       4),
    (N'RAV4',          N'Toyota',       3),
    (N'86',            N'Toyota',       5),
    -- Honda
    (N'Civic',         N'Honda',        1),
    (N'Fit',           N'Honda',        2),
    (N'CR-V',          N'Honda',        3),
    -- Ford
    (N'Focus',         N'Ford',         2),
    (N'Mustang',       N'Ford',         5),
    (N'Ranger',        N'Ford',         4),
    (N'EcoSport',      N'Ford',         3),
    (N'Fiesta',        N'Ford',         2),
    -- Chevrolet
    (N'Cruze',         N'Chevrolet',    1),
    (N'Onix',          N'Chevrolet',    2),
    (N'S10',           N'Chevrolet',    4),
    (N'Tracker',       N'Chevrolet',    3),
    (N'Camaro',        N'Chevrolet',    5),
    -- Volkswagen
    (N'Golf',          N'Volkswagen',   2),
    (N'Polo',          N'Volkswagen',   2),
    (N'Vento',         N'Volkswagen',   1),
    (N'Amarok',        N'Volkswagen',   4),
    (N'Tiguan',        N'Volkswagen',   3),
    -- Fiat
    (N'Cronos',        N'Fiat',         1),
    (N'Argo',          N'Fiat',         2),
    (N'Toro',          N'Fiat',         4),
    -- Renault
    (N'Clio',          N'Renault',      2),
    (N'Sandero',       N'Renault',      2),
    (N'Duster',        N'Renault',      3),
    (N'Megane',        N'Renault',      1),
    -- Peugeot
    (N'208',           N'Peugeot',      2),
    (N'3008',          N'Peugeot',      3),
    (N'308',           N'Peugeot',      2),
    -- Nissan
    (N'Sentra',        N'Nissan',       1),
    (N'Frontier',      N'Nissan',       4),
    (N'X-Trail',       N'Nissan',       3),
    -- BMW
    (N'Serie 3',       N'BMW',          1),
    (N'X5',            N'BMW',          3),
    (N'M4',            N'BMW',          5),
    -- Mercedes-Benz
    (N'Clase A',       N'Mercedes-Benz',2),
    (N'Clase C',       N'Mercedes-Benz',1),
    (N'GLA',           N'Mercedes-Benz',3),
    -- Audi
    (N'A3',            N'Audi',         2),
    (N'A4',            N'Audi',         1),
    (N'Q5',            N'Audi',         3),
    -- Jeep
    (N'Compass',       N'Jeep',         3),
    (N'Wrangler',      N'Jeep',         3),
    -- Hyundai
    (N'Tucson',        N'Hyundai',      3),
    -- Kia
    (N'Sportage',      N'Kia',          3),
    -- Tesla
    (N'Model 3',       N'Tesla',        1)
) AS v(Nombre, Marca, Tipo)
GO

-- =========================================================
-- i18n N01 — Enum EstadoUnidad + 4 forms nuevos
-- =========================================================

-- ---------------------------------------------------------
-- Enum EstadoUnidad
-- ---------------------------------------------------------
DECLARE @formEstU NVARCHAR(80) = N'EstadoUnidad';
DECLARE @ctrlEstU TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlEstU (Codigo, Es, En, De) VALUES
    (N'Ingresado',                      N'Ingresado',                           N'Received',                           N'Angenommen'),
    (N'RequiereAprobacionPresupuesto',  N'Requiere aprobación de presupuesto',  N'Requires budget approval',           N'Erfordert Kostengenehmigung'),
    (N'EnPreparacion',                  N'En preparación',                      N'In preparation',                     N'In Vorbereitung'),
    (N'PendienteVenta',        N'Pendiente de venta',        N'Pending sale',             N'Verkauf ausstehend'),
    (N'EnVenta',               N'En venta',                  N'For sale',                 N'Zum Verkauf'),
    (N'Vendido',               N'Vendido',                   N'Sold',                     N'Verkauft'),
    (N'Reservado',             N'Reservado',                 N'Reserved',                 N'Reserviert'),
    (N'Pausado',               N'Pausado',                   N'Paused',                   N'Pausiert');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formEstU FROM @ctrlEstU;

DECLARE @idEsEstU INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnEstU INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeEstU INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsEstU, c.Id, t.Es FROM @ctrlEstU t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEstU
UNION ALL
SELECT @idEnEstU, c.Id, t.En FROM @ctrlEstU t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEstU
UNION ALL
SELECT @idDeEstU, c.Id, t.De FROM @ctrlEstU t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEstU;
GO

-- ---------------------------------------------------------
-- FormRegistrarUnidad
-- ---------------------------------------------------------
DECLARE @formRegU NVARCHAR(80) = N'FormRegistrarUnidad';
DECLARE @ctrlRegU TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlRegU (Codigo, Es, En, De) VALUES
    (N'title',              N'Registrar Unidad',                                 N'Register Unit',                                            N'Einheit registrieren'),
    (N'lblTitulo',          N'Registrar Unidad',                                 N'Register Unit',                                            N'Einheit registrieren'),
    (N'grpUnidad',          N'Datos de la unidad',                               N'Unit data',                                                N'Einheiten-Daten'),
    (N'lblDominio',         N'Dominio:',                                         N'Plate:',                                                   N'Kennzeichen:'),
    (N'lblMarca',           N'Marca:',                                           N'Brand:',                                                   N'Marke:'),
    (N'lblModelo',          N'Modelo:',                                          N'Model:',                                                   N'Modell:'),
    (N'lblAnio',            N'Año:',                                             N'Year:',                                                    N'Jahr:'),
    (N'lblKilometraje',     N'Kilometraje:',                                     N'Mileage:',                                                 N'Kilometerstand:'),
    (N'lblPrecioCompra',    N'Precio compra:',                                   N'Purchase price:',                                          N'Kaufpreis:'),
    (N'lblDescripcion',     N'Descripción:',                                     N'Description:',                                             N'Beschreibung:'),
    (N'grpPersona',         N'Persona vendedora',                                N'Selling person',                                           N'Verkäufer (Person)'),
    (N'lblDocBusqueda',     N'DNI / CUIT:',                                      N'ID / Tax ID:',                                             N'Ausweis / Steuer-Nr:'),
    (N'btnBuscarPersona',   N'Buscar',                                           N'Search',                                                   N'Suchen'),
    (N'lblCoincidencias',   N'Coincidencias:',                                   N'Matches:',                                                 N'Treffer:'),
    (N'btnRegistrar',       N'Registrar',                                        N'Register',                                                 N'Registrieren'),
    (N'msgDocVacio',        N'Ingresá un DNI o CUIT (o parte) para buscar.',     N'Enter an ID/Tax ID (or part) to search.',                  N'Gib eine (Teil-)Dokumentnummer zum Suchen ein.'),
    (N'msgSinCoincidencias',N'No se encontraron personas con ese documento.',    N'No people found with that document.',                      N'Keine Personen mit diesem Dokument gefunden.'),
    (N'msgPersonaInvalida', N'Buscá y seleccioná una persona vendedora.',        N'Search and select a selling person.',                      N'Suche und wähle eine verkaufende Person aus.'),
    (N'msgAdvertencia',     N'Advertencia',                                      N'Warning',                                                  N'Warnung'),
    (N'msgInformacion',     N'Información',                                      N'Information',                                              N'Information'),
    (N'msgAnioInvalido',    N'Año inválido.',                                    N'Invalid year.',                                            N'Ungültiges Jahr.'),
    (N'msgKmInvalido',      N'Kilometraje inválido.',                            N'Invalid mileage.',                                         N'Ungültiger Kilometerstand.'),
    (N'msgPrecioInvalido',  N'Precio de compra inválido.',                       N'Invalid purchase price.',                                  N'Ungültiger Kaufpreis.'),
    (N'msgUnidadRegistrada',N'Unidad registrada con Id {0}.',                    N'Unit registered with Id {0}.',                             N'Einheit registriert mit Id {0}.'),
    (N'msgExito',           N'Éxito',                                            N'Success',                                                  N'Erfolg'),
    (N'msgError',           N'Error',                                            N'Error',                                                    N'Fehler');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formRegU FROM @ctrlRegU;

DECLARE @idEsRegU INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnRegU INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeRegU INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsRegU, c.Id, t.Es FROM @ctrlRegU t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRegU
UNION ALL
SELECT @idEnRegU, c.Id, t.En FROM @ctrlRegU t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRegU
UNION ALL
SELECT @idDeRegU, c.Id, t.De FROM @ctrlRegU t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRegU;
GO

-- ---------------------------------------------------------
-- FormTemplateChecklist
-- ---------------------------------------------------------
DECLARE @formTC NVARCHAR(80) = N'FormTemplateChecklist';
DECLARE @ctrlTC TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlTC (Codigo, Es, En, De) VALUES
    (N'title',                  N'Template de Checklist',                    N'Checklist Template',                            N'Checklisten-Vorlage'),
    (N'lblTitulo',              N'Template de Checklist',                    N'Checklist Template',                            N'Checklisten-Vorlage'),
    (N'colId',                  N'Id',                                       N'Id',                                            N'Id'),
    (N'colNombre',              N'Nombre',                                   N'Name',                                          N'Name'),
    (N'colDescripcion',         N'Descripción',                              N'Description',                                   N'Beschreibung'),
    (N'colActivo',              N'Activo',                                   N'Active',                                        N'Aktiv'),
    (N'btnNuevo',               N'Nuevo item',                               N'New item',                                      N'Neues Element'),
    (N'btnEditarDescripcion',   N'Editar descripción',                       N'Edit description',                              N'Beschreibung bearbeiten'),
    (N'btnBajaLogica',          N'Dar de baja',                              N'Deactivate',                                    N'Deaktivieren'),
    (N'btnReactivar',           N'Reactivar',                                N'Reactivate',                                    N'Reaktivieren'),
    (N'promptNombre',           N'Nombre del item (obligatorio):',           N'Item name (required):',                         N'Elementname (erforderlich):'),
    (N'promptDescripcion',      N'Descripción (opcional):',                  N'Description (optional):',                       N'Beschreibung (optional):'),
    (N'promptEditarDesc',       N'Nueva descripción (dejar vacía para borrar):', N'New description (leave empty to clear):',   N'Neue Beschreibung (leer lassen zum Löschen):'),
    (N'msgSeleccionar',         N'Seleccioná un item.',                      N'Select an item.',                               N'Wähle ein Element aus.'),
    (N'msgInformacion',         N'Info',                                     N'Info',                                          N'Info'),
    (N'msgConfirmarBaja',       N'¿Dar de baja lógica al item ''{0}''?',     N'Deactivate item ''{0}''?',                      N'Element ''{0}'' deaktivieren?'),
    (N'msgConfirmar',           N'Confirmar',                                N'Confirm',                                       N'Bestätigen'),
    (N'msgError',               N'Error',                                    N'Error',                                         N'Fehler');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formTC FROM @ctrlTC;

DECLARE @idEsTC INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnTC INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeTC INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsTC, c.Id, t.Es FROM @ctrlTC t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTC
UNION ALL
SELECT @idEnTC, c.Id, t.En FROM @ctrlTC t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTC
UNION ALL
SELECT @idDeTC, c.Id, t.De FROM @ctrlTC t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTC;
GO

-- ---------------------------------------------------------
-- FormUnidades
-- ---------------------------------------------------------
DECLARE @formUnd NVARCHAR(80) = N'FormUnidades';
DECLARE @ctrlUnd TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlUnd (Codigo, Es, En, De) VALUES
    (N'title',            N'Unidades',                        N'Units',                     N'Einheiten'),
    (N'lblTitulo',        N'Unidades',                        N'Units',                     N'Einheiten'),
    (N'lblFiltroEstado',  N'Estado:',                         N'State:',                    N'Zustand:'),
    (N'itemTodosEstados', N'(todos los estados)',             N'(all states)',              N'(alle Zustände)'),
    (N'lblBuscarDominio', N'Dominio:',                        N'Plate:',                    N'Kennzeichen:'),
    (N'btnBuscar',        N'Buscar',                          N'Search',                    N'Suchen'),
    (N'btnActualizar',    N'Actualizar',                      N'Refresh',                   N'Aktualisieren'),
    (N'btnVerDetalle',    N'Ver detalle',                     N'View details',              N'Details ansehen'),
    (N'colId',            N'Id',                              N'Id',                        N'Id'),
    (N'colDominio',       N'Dominio',                         N'Plate',                     N'Kennzeichen'),
    (N'colMarca',         N'Marca',                           N'Brand',                     N'Marke'),
    (N'colModelo',        N'Modelo',                          N'Model',                     N'Modell'),
    (N'colAnio',          N'Año',                             N'Year',                      N'Jahr'),
    (N'colKm',            N'Km',                              N'Mileage',                   N'Km'),
    (N'colEstado',        N'Estado',                          N'State',                     N'Zustand'),
    (N'colIngreso',       N'Ingreso',                         N'Received',                  N'Angenommen'),
    (N'lblTotal',         N'Total: {0}',                      N'Total: {0}',                N'Gesamt: {0}'),
    (N'msgSeleccionar',   N'Seleccioná una unidad.',          N'Select a unit.',            N'Wähle eine Einheit aus.'),
    (N'msgInformacion',   N'Info',                            N'Info',                      N'Info'),
    (N'msgError',         N'Error',                           N'Error',                     N'Fehler');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formUnd FROM @ctrlUnd;

DECLARE @idEsUnd INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnUnd INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeUnd INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsUnd, c.Id, t.Es FROM @ctrlUnd t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUnd
UNION ALL
SELECT @idEnUnd, c.Id, t.En FROM @ctrlUnd t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUnd
UNION ALL
SELECT @idDeUnd, c.Id, t.De FROM @ctrlUnd t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUnd;
GO

-- ---------------------------------------------------------
-- FormDetalleUnidad
-- ---------------------------------------------------------
DECLARE @formDetU NVARCHAR(80) = N'FormDetalleUnidad';
DECLARE @ctrlDetU TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlDetU (Codigo, Es, En, De) VALUES
    (N'title',                     N'Detalle de unidad',                                  N'Unit detail',                                         N'Einheit-Details'),
    (N'lblEstadoPrefix',           N'Estado: ',                                           N'State: ',                                             N'Zustand: '),
    (N'tabDatos',                  N'Datos',                                              N'Data',                                                N'Daten'),
    (N'tabChecklist',              N'Checklist',                                          N'Checklist',                                           N'Checkliste'),
    (N'tabHistorial',              N'Historial',                                          N'History',                                             N'Verlauf'),
    (N'lblChecklistVacio',         N'Aún no hay checklist para esta unidad.',             N'This unit has no checklist yet.',                     N'Für diese Einheit gibt es noch keine Checkliste.'),
    (N'lblChecklistInfo',          N'Checklist creado el {0} — {1} items.',               N'Checklist created on {0} — {1} items.',               N'Checkliste erstellt am {0} — {1} Elemente.'),
    (N'colChkOrigen',              N'Origen',                                             N'Source',                                              N'Herkunft'),
    (N'colChkNombre',              N'Nombre',                                             N'Name',                                                N'Name'),
    (N'colChkDescripcion',         N'Descripción',                                        N'Description',                                         N'Beschreibung'),
    (N'colChkEjecutado',           N'Ejecutado',                                          N'Executed',                                            N'Ausgeführt'),
    (N'colChkResultado',           N'Resultado',                                          N'Result',                                              N'Ergebnis'),
    (N'origenTemplate',            N'Template',                                           N'Template',                                            N'Vorlage'),
    (N'origenExtra',               N'Extra',                                              N'Extra',                                               N'Zusatz'),
    (N'valorEjecutadoOk',          N'OK',                                                 N'OK',                                                  N'OK'),
    (N'valorEjecutadoNo',          N'—',                                                  N'—',                                                   N'—'),
    (N'btnAgregarItemExtra',       N'Agregar item extra',                                 N'Add extra item',                                      N'Zusatzelement hinzufügen'),
    (N'btnQuitarItemExtra',        N'Quitar item extra',                                  N'Remove extra item',                                   N'Zusatzelement entfernen'),
    (N'lblResultado',              N'Resultado del ítem seleccionado:',                   N'Result of selected item:',                            N'Ergebnis des ausgewählten Elements:'),
    (N'btnMarcarItemEjecutado',    N'Marcar ejecutado',                                   N'Mark as executed',                                    N'Als ausgeführt markieren'),
    (N'colHistFecha',              N'Fecha/Hora',                                         N'Date/Time',                                           N'Datum/Uhrzeit'),
    (N'colHistOrigen',             N'Origen',                                             N'From',                                                N'Von'),
    (N'colHistDestino',            N'Destino',                                            N'To',                                                  N'Bis'),
    (N'colHistUsuario',            N'Usuario',                                            N'User',                                                N'Benutzer'),
    (N'colHistMotivo',             N'Motivo / obs.',                                      N'Reason / notes',                                      N'Grund / Notiz'),
    (N'btnMarcarImpecable',        N'Marcar impecable',                                   N'Mark as impeccable',                                  N'Als tadellos markieren'),
    (N'btnEnviarAutorizacion',     N'Enviar a autorización',                              N'Send to authorization',                               N'Zur Autorisierung senden'),
    (N'btnAutorizarChecklist',     N'Autorizar checklist',                                N'Authorize checklist',                                 N'Checkliste autorisieren'),
    (N'btnRechazarChecklist',      N'Rechazar checklist',                                 N'Reject checklist',                                    N'Checkliste ablehnen'),
    (N'btnFinalizarPreparacion',   N'Finalizar preparación',                              N'Finish preparation',                                  N'Vorbereitung abschließen'),
    (N'btnAutorizarPublicacion',   N'Autorizar publicación',                              N'Authorize publication',                               N'Veröffentlichung autorisieren'),
    (N'btnRechazarPublicacion',    N'Rechazar publicación',                               N'Reject publication',                                  N'Veröffentlichung ablehnen'),
    (N'promptObservaciones',       N'Observaciones (opcional):',                          N'Notes (optional):',                                   N'Anmerkungen (optional):'),
    (N'promptMotivoRechazo',       N'Motivo del rechazo (obligatorio):',                  N'Rejection reason (required):',                        N'Ablehnungsgrund (erforderlich):'),
    (N'promptNombreItem',          N'Nombre del item extra:',                             N'Extra item name:',                                    N'Name des Zusatzelements:'),
    (N'promptDescripcionItem',     N'Descripción (opcional):',                            N'Description (optional):',                             N'Beschreibung (optional):'),
    (N'msgChecklistVacio',         N'El checklist está vacío. Agregá al menos un ítem (extra) o cargá items en el template.', N'The checklist is empty. Add at least one (extra) item or load items in the template.', N'Die Checkliste ist leer. Füge mindestens ein (Zusatz-)Element hinzu oder lade Elemente in die Vorlage.'),
    (N'msgSeleccionarItem',        N'Seleccioná un ítem.',                                N'Select an item.',                                     N'Wähle ein Element aus.'),
    (N'msgNoBorrarTemplate',       N'No se puede eliminar un ítem del template.',         N'Cannot delete a template item.',                      N'Ein Vorlagenelement kann nicht gelöscht werden.'),
    (N'msgResultadoVacio',         N'Ingresá un resultado en el campo de texto.',         N'Enter a result in the text field.',                   N'Gib ein Ergebnis in das Textfeld ein.'),
    (N'msgInformacion',            N'Info',                                               N'Info',                                                N'Info'),
    (N'msgAdvertencia',            N'Advertencia',                                        N'Warning',                                             N'Warnung'),
    (N'msgError',                  N'Error',                                              N'Error',                                               N'Fehler'),
    (N'promptTitulo',              N'Ingresar',                                           N'Enter',                                               N'Eingeben'),
    (N'btnAceptar',                N'Aceptar',                                            N'OK',                                                  N'OK'),
    (N'btnCancelar',               N'Cancelar',                                           N'Cancel',                                              N'Abbrechen'),
    (N'vendedorTipoFisica',        N'Física',                                             N'Individual',                                          N'Natürlich'),
    (N'vendedorTipoJuridica',      N'Jurídica',                                           N'Company',                                             N'Juristisch'),
    (N'vendedorNoEncontrado',      N'(no encontrado)',                                    N'(not found)',                                         N'(nicht gefunden)');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formDetU FROM @ctrlDetU;

DECLARE @idEsDetU INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnDetU INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeDetU INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsDetU, c.Id, t.Es FROM @ctrlDetU t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formDetU
UNION ALL
SELECT @idEnDetU, c.Id, t.En FROM @ctrlDetU t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formDetU
UNION ALL
SELECT @idDeDetU, c.Id, t.De FROM @ctrlDetU t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formDetU;
GO

-- ---------------------------------------------------------
-- Enum TipoCarroceria
-- ---------------------------------------------------------
DECLARE @formTC2 NVARCHAR(80) = N'TipoCarroceria';
DECLARE @ctrlTC2 TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlTC2 (Codigo, Es, En, De) VALUES
    (N'Sedan',     N'Sedán',     N'Sedan',      N'Limousine'),
    (N'Hatchback', N'Hatchback', N'Hatchback',  N'Fließheck'),
    (N'SUV',       N'SUV',       N'SUV',        N'SUV'),
    (N'Pickup',    N'Pickup',    N'Pickup',     N'Pick-up'),
    (N'Coupe',     N'Coupe',     N'Coupe',      N'Coupé');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formTC2 FROM @ctrlTC2;

DECLARE @idEsTC2 INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnTC2 INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeTC2 INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsTC2, c.Id, t.Es FROM @ctrlTC2 t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTC2
UNION ALL
SELECT @idEnTC2, c.Id, t.En FROM @ctrlTC2 t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTC2
UNION ALL
SELECT @idDeTC2, c.Id, t.De FROM @ctrlTC2 t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTC2;
GO

-- ---------------------------------------------------------
-- FormRegistrarModelo
-- ---------------------------------------------------------
DECLARE @formRegMod NVARCHAR(80) = N'FormRegistrarModelo';
DECLARE @ctrlRegMod TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlRegMod (Codigo, Es, En, De) VALUES
    (N'title',              N'Registrar Modelo',                             N'Register Model',                                     N'Modell registrieren'),
    (N'lblTitulo',          N'Registrar Modelo',                             N'Register Model',                                     N'Modell registrieren'),
    (N'lblMarca',           N'Marca:',                                       N'Brand:',                                             N'Marke:'),
    (N'lblNombre',          N'Nombre:',                                      N'Name:',                                              N'Name:'),
    (N'lblTipoCarroceria',  N'Tipo carrocería:',                             N'Body type:',                                         N'Karosserie:'),
    (N'btnDarDeAlta',       N'Dar de alta',                                  N'Add',                                                N'Hinzufügen'),
    (N'btnEditar',          N'Editar',                                       N'Edit',                                               N'Bearbeiten'),
    (N'btnEliminar',        N'Eliminar',                                     N'Delete',                                             N'Löschen'),
    (N'lblExistentes',      N'Modelos existentes (filtra por marca):',       N'Existing models (filter by brand):',                 N'Vorhandene Modelle (nach Marke filtern):'),
    (N'msgSinPermiso',      N'No tenés permiso para dar de alta modelos.',   N'You don''t have permission to add models.',          N'Du hast keine Berechtigung, Modelle anzulegen.'),
    (N'msgAccesoDenegado',  N'Acceso denegado',                              N'Access denied',                                      N'Zugriff verweigert'),
    (N'msgNombreVacio',     N'Ingresá un nombre de modelo.',                 N'Enter a model name.',                                N'Gib einen Modellnamen ein.'),
    (N'msgMarcaVacia',      N'Seleccioná una marca.',                        N'Select a brand.',                                    N'Wähle eine Marke aus.'),
    (N'msgAdvertencia',     N'Advertencia',                                  N'Warning',                                            N'Warnung'),
    (N'msgModeloRegistrado',N'Modelo ''{0}'' registrado.',                   N'Model ''{0}'' registered.',                          N'Modell ''{0}'' registriert.'),
    (N'msgModeloEditado',   N'Modelo editado correctamente.',                N'Model edited successfully.',                         N'Modell erfolgreich bearbeitet.'),
    (N'msgSeleccionarModelo',N'Seleccioná un modelo de la lista.',           N'Select a model from the list.',                      N'Wähle ein Modell aus der Liste aus.'),
    (N'msgConfirmarEliminar',N'¿Eliminar el modelo ''{0}''?',                N'Delete model ''{0}''?',                              N'Modell ''{0}'' löschen?'),
    (N'msgConfirmar',       N'Confirmar',                                    N'Confirm',                                            N'Bestätigen'),
    (N'msgModeloEliminado', N'Modelo eliminado.',                            N'Model deleted.',                                     N'Modell gelöscht.'),
    (N'msgExito',           N'Éxito',                                        N'Success',                                            N'Erfolg'),
    (N'msgError',           N'Error',                                        N'Error',                                              N'Fehler'),
    (N'itemTodasLasMarcas', N'(todas las marcas)',                           N'(all brands)',                                       N'(alle Marken)');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formRegMod FROM @ctrlRegMod;

DECLARE @idEsRegMod INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnRegMod INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeRegMod INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsRegMod, c.Id, t.Es FROM @ctrlRegMod t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRegMod
UNION ALL
SELECT @idEnRegMod, c.Id, t.En FROM @ctrlRegMod t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRegMod
UNION ALL
SELECT @idDeRegMod, c.Id, t.De FROM @ctrlRegMod t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRegMod;
GO

-- ---------------------------------------------------------
-- Enum EstadoCivil
-- ---------------------------------------------------------
DECLARE @formEC NVARCHAR(80) = N'EstadoCivil';
DECLARE @ctrlEC TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlEC (Codigo, Es, En, De) VALUES
    (N'Soltero',            N'Soltero/a',            N'Single',          N'Ledig'),
    (N'Casado',             N'Casado/a',             N'Married',         N'Verheiratet'),
    (N'Divorciado',         N'Divorciado/a',         N'Divorced',        N'Geschieden'),
    (N'Viudo',              N'Viudo/a',              N'Widowed',         N'Verwitwet'),
    (N'UnionConvivencial',  N'Unión convivencial',   N'Civil union',     N'Lebenspartnerschaft');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formEC FROM @ctrlEC;

DECLARE @idEsEC INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnEC INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeEC INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsEC, c.Id, t.Es FROM @ctrlEC t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEC
UNION ALL
SELECT @idEnEC, c.Id, t.En FROM @ctrlEC t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEC
UNION ALL
SELECT @idDeEC, c.Id, t.De FROM @ctrlEC t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEC;
GO

-- ---------------------------------------------------------
-- TipoBitacora: AltaPersona
-- ---------------------------------------------------------
DECLARE @formTB2 NVARCHAR(80) = N'TipoBitacora';
DECLARE @ctrlTB2 TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));
INSERT INTO @ctrlTB2 (Codigo, Es, En, De) VALUES
    (N'AltaPersona', N'Alta de persona', N'Person created', N'Person erstellt');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formTB2 FROM @ctrlTB2;

DECLARE @idEsTB2 INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnTB2 INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeTB2 INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsTB2, c.Id, t.Es FROM @ctrlTB2 t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTB2
UNION ALL
SELECT @idEnTB2, c.Id, t.En FROM @ctrlTB2 t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTB2
UNION ALL
SELECT @idDeTB2, c.Id, t.De FROM @ctrlTB2 t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTB2;
GO

-- ---------------------------------------------------------
-- FormPrincipal: menuPersonas y menuRegistrarPersona
-- ---------------------------------------------------------
DECLARE @formPpal2 NVARCHAR(80) = N'FormPrincipal';
DECLARE @ctrlPpal2 TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));
INSERT INTO @ctrlPpal2 (Codigo, Es, En, De) VALUES
    (N'menuPersonas',           N'Personas',          N'People',          N'Personen'),
    (N'menuRegistrarPersona',   N'Registrar Persona', N'Register Person', N'Person registrieren');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formPpal2 FROM @ctrlPpal2;

DECLARE @idEsPpal2 INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnPpal2 INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDePpal2 INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsPpal2, c.Id, t.Es FROM @ctrlPpal2 t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPpal2
UNION ALL
SELECT @idEnPpal2, c.Id, t.En FROM @ctrlPpal2 t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPpal2
UNION ALL
SELECT @idDePpal2, c.Id, t.De FROM @ctrlPpal2 t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPpal2;
GO

-- ---------------------------------------------------------
-- FormRegistrarPersona
-- ---------------------------------------------------------
DECLARE @formRegP NVARCHAR(80) = N'FormRegistrarPersona';
DECLARE @ctrlRegP TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlRegP (Codigo, Es, En, De) VALUES
    (N'title',               N'Registrar Persona',                                   N'Register Person',                                       N'Person registrieren'),
    (N'lblTitulo',           N'Registrar Persona',                                   N'Register Person',                                       N'Person registrieren'),
    (N'lblTipoPersona',      N'Tipo:',                                               N'Type:',                                                 N'Typ:'),
    (N'rbFisica',            N'Física',                                              N'Individual',                                            N'Natürlich'),
    (N'rbJuridica',          N'Jurídica',                                            N'Company',                                               N'Juristisch'),
    (N'lblNombre',           N'Nombre / R.S.:',                                      N'Name / Company:',                                       N'Name / Firma:'),
    (N'lblDocumento',        N'DNI / CUIT:',                                         N'ID / Tax ID:',                                          N'Ausweis / Steuer-Nr:'),
    (N'lblDomicilio',        N'Domicilio:',                                          N'Address:',                                              N'Adresse:'),
    (N'lblTelefono',         N'Teléfono:',                                           N'Phone:',                                                N'Telefon:'),
    (N'lblEmail',            N'Email:',                                              N'Email:',                                                N'E-Mail:'),
    (N'lblEstadoCivil',      N'Estado civil:',                                       N'Marital status:',                                       N'Familienstand:'),
    (N'lblFechaNacimiento',  N'Fecha de nacimiento:',                                N'Birth date:',                                           N'Geburtsdatum:'),
    (N'btnDarDeAlta',        N'Dar de alta',                                         N'Add',                                                   N'Hinzufügen'),
    (N'btnEditar',           N'Editar',                                               N'Edit',                                                  N'Bearbeiten'),
    (N'btnEliminar',         N'Eliminar',                                             N'Delete',                                                N'Löschen'),
    (N'lblExistentes',       N'Personas registradas:',                               N'Registered people:',                                    N'Registrierte Personen:'),
    (N'msgSinPermiso',       N'No tenés permiso para dar de alta personas.',         N'You do not have permission to add people.',             N'Du hast keine Berechtigung, Personen anzulegen.'),
    (N'msgAccesoDenegado',   N'Acceso denegado',                                     N'Access denied',                                         N'Zugriff verweigert'),
    (N'msgCamposVacios',     N'Completá nombre, documento y domicilio.',             N'Fill in name, document and address.',                   N'Fülle Name, Dokument und Adresse aus.'),
    (N'msgAdvertencia',      N'Advertencia',                                         N'Warning',                                               N'Warnung'),
    (N'msgPersonaRegistrada',N'Persona registrada.',                                 N'Person registered.',                                    N'Person registriert.'),
    (N'msgPersonaEditada',   N'Persona editada correctamente.',                      N'Person edited successfully.',                           N'Person erfolgreich bearbeitet.'),
    (N'msgSeleccionarPersona',N'Seleccioná una persona de la lista.',                N'Select a person from the list.',                        N'Wähle eine Person aus der Liste aus.'),
    (N'msgConfirmar',        N'Confirmar',                                           N'Confirm',                                               N'Bestätigen'),
    (N'msgPersonaEliminada', N'Persona eliminada.',                                  N'Person deleted.',                                       N'Person gelöscht.'),
    (N'msgExito',            N'Éxito',                                               N'Success',                                               N'Erfolg'),
    (N'msgError',            N'Error',                                               N'Error',                                                 N'Fehler');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formRegP FROM @ctrlRegP;

DECLARE @idEsRegP INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnRegP INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeRegP INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsRegP, c.Id, t.Es FROM @ctrlRegP t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRegP
UNION ALL
SELECT @idEnRegP, c.Id, t.En FROM @ctrlRegP t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRegP
UNION ALL
SELECT @idDeRegP, c.Id, t.De FROM @ctrlRegP t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRegP;
GO

-- =========================================================
-- Seed de personas (10 físicas + 10 jurídicas)
-- EstadoCivil: 1=Soltero, 2=Casado, 3=Divorciado, 4=Viudo, 5=UnionConvivencial
-- =========================================================

INSERT INTO Persona (TipoPersona, Documento, Nombre, Domicilio, Telefono, Email, EstadoCivil, FechaNacimiento) VALUES
    ('F', N'20123456',  N'Juan Pérez',         N'Av. Rivadavia 1234, CABA',        N'+54 11 4123-4567', N'juan.perez@gmail.com',         1, '1985-03-15'),
    ('F', N'27234567',  N'María González',     N'Calle Alsina 456, CABA',          N'+54 11 4234-5678', N'maria.gonzalez@yahoo.com',     2, '1978-07-22'),
    ('F', N'30345678',  N'Carlos Rodríguez',   N'Av. Córdoba 789, CABA',           N'+54 11 4345-6789', N'carlos.rodriguez@hotmail.com', 2, '1972-11-05'),
    ('F', N'25456789',  N'Laura Fernández',    N'Calle Lavalle 321, CABA',         N'+54 11 4456-7890', N'laura.fernandez@gmail.com',    1, '1990-05-30'),
    ('F', N'32567890',  N'Diego Martínez',     N'Av. Santa Fe 654, CABA',          N'+54 11 4567-8901', N'diego.martinez@gmail.com',     3, '1988-09-18'),
    ('F', N'28678901',  N'Sofía López',        N'Calle Esmeralda 987, CABA',       N'+54 11 4678-9012', N'sofia.lopez@gmail.com',        1, '1992-01-12'),
    ('F', N'33789012',  N'Martín Gómez',       N'Av. Belgrano 234, Avellaneda',    N'+54 11 4789-0123', N'martin.gomez@yahoo.com',       2, '1980-06-25'),
    ('F', N'26890123',  N'Valentina Silva',    N'Calle Suipacha 567, CABA',        N'+54 11 4890-1234', N'valentina.silva@gmail.com',    4, '1965-10-08'),
    ('F', N'34901234',  N'Nicolás Torres',     N'Av. Corrientes 890, CABA',        N'+54 11 4901-2345', N'nicolas.torres@gmail.com',     1, '1995-04-20'),
    ('F', N'29012345',  N'Camila Ruiz',        N'Calle Florida 123, San Isidro',   N'+54 11 4012-3456', N'camila.ruiz@gmail.com',        5, '1983-12-03')
GO

INSERT INTO Persona (TipoPersona, Documento, Nombre, Domicilio, Telefono, Email, EstadoCivil, FechaNacimiento) VALUES
    ('J', N'30700123456', N'Transporte del Sur S.A.',        N'Av. Independencia 2500, CABA',     N'+54 11 5000-1001', N'contacto@transportedelsur.com.ar', 1, '2000-01-15'),
    ('J', N'30701234567', N'Logística Patagonia S.R.L.',     N'Ruta 3 Km 1420, Comodoro Rivadavia',N'+54 297 455-2002', N'ventas@logpatagonia.com.ar',       1, '2005-06-20'),
    ('J', N'30702345678', N'Autopartes del Litoral S.A.',    N'Av. Alem 1800, Rosario',           N'+54 341 456-3003', N'info@autopartesdellitoral.com',    1, '1998-03-10'),
    ('J', N'30703456789', N'Flota Norte S.R.L.',             N'Av. San Martín 450, Salta',        N'+54 387 489-4004', N'admin@flotanorte.com.ar',          1, '2010-11-05'),
    ('J', N'30704567890', N'Comercial Mitre S.A.',           N'Av. Mitre 980, Avellaneda',        N'+54 11 4201-5005', N'ventas@comercialmitre.com',        1, '1995-09-22'),
    ('J', N'30705678901', N'Grupo Andino S.A.',              N'Calle Chile 1234, Mendoza',        N'+54 261 423-6006', N'contacto@grupoandino.com.ar',      1, '2003-07-14'),
    ('J', N'30706789012', N'Distribuidora Cuyo S.R.L.',      N'Av. Las Heras 2100, Mendoza',      N'+54 261 434-7007', N'ventas@distrocuyo.com.ar',         1, '2008-02-28'),
    ('J', N'30707890123', N'Tecnomotor Argentina S.A.',      N'Av. Monroe 3400, CABA',            N'+54 11 4790-8008', N'info@tecnomotor.com.ar',           1, '1992-12-01'),
    ('J', N'30708901234', N'Taller Mecánico Córdoba S.R.L.', N'Av. Colón 2800, Córdoba',          N'+54 351 487-9009', N'taller@mcc.com.ar',                1, '2012-05-19'),
    ('J', N'30709012345', N'RentaCar Buenos Aires S.A.',     N'Av. del Libertador 7500, CABA',    N'+54 11 4788-0100', N'reservas@rentacarba.com.ar',       1, '2001-10-30')
GO

-- =========================================================
-- Seed de 20 unidades distribuidas en los 5 estados (4 c/u)
-- EstadoActual: 1=Ingresado, 2=RequiereAprobacionPresupuesto, 3=EnPreparacion, 4=PendienteVenta, 5=EnVenta
-- =========================================================

DECLARE @IdAdmin INT = (SELECT Id FROM Usuario WHERE Username = 'admin');

INSERT INTO Unidad (Dominio, Marca, Modelo, Anio, Kilometraje, PrecioCompra, Descripcion, EstadoActual, IdPersona, IdCompradorUsuario) VALUES
    -- 4 Ingresado
    ('ABC123',  N'Toyota',       N'Corolla',   2020,  45000,  8500000.00, N'Único dueño, service oficial al día.',  1, (SELECT Id FROM Persona WHERE Documento = N'20123456'), @IdAdmin),
    ('DEF456',  N'Ford',         N'Fiesta',    2018,  72000,  5200000.00, N'En buen estado general.',               1, (SELECT Id FROM Persona WHERE Documento = N'27234567'), @IdAdmin),
    ('AB123CD', N'Volkswagen',   N'Golf',      2021,  28000, 11500000.00, N'Impecable, aún en garantía.',           1, (SELECT Id FROM Persona WHERE Documento = N'30700123456'), @IdAdmin),
    ('GHI789',  N'Fiat',         N'Cronos',    2019,  58000,  6300000.00, N'Nafta, caja manual.',                   1, (SELECT Id FROM Persona WHERE Documento = N'30345678'), @IdAdmin),
    -- 4 RequiereAprobacionPresupuesto (vinieron desde EnPreparacion con extras propuestos)
    ('EF456GH', N'Chevrolet',    N'Onix',      2022,  18000,  9200000.00, N'Casi 0km.',                             2, (SELECT Id FROM Persona WHERE Documento = N'25456789'), @IdAdmin),
    ('JKL012',  N'Honda',        N'Civic',     2017,  92000,  7100000.00, N'Service completo.',                     2, (SELECT Id FROM Persona WHERE Documento = N'30701234567'), @IdAdmin),
    ('IJ789KL', N'Renault',      N'Duster',    2020,  61000,  8900000.00, N'4x2, caja manual.',                     2, (SELECT Id FROM Persona WHERE Documento = N'32567890'), @IdAdmin),
    ('MNO345',  N'Peugeot',      N'208',       2021,  34000,  7800000.00, N'Full con techo panorámico.',            2, (SELECT Id FROM Persona WHERE Documento = N'30702345678'), @IdAdmin),
    -- 4 EnPreparacion
    ('MN012OP', N'Toyota',       N'Hilux',     2019, 110000, 15200000.00, N'4x4 SRX. En preparación.',              3, (SELECT Id FROM Persona WHERE Documento = N'28678901'), @IdAdmin),
    ('PQR678',  N'Ford',         N'Ranger',    2020, 85000,  18500000.00, N'Doble cabina.',                         3, (SELECT Id FROM Persona WHERE Documento = N'30703456789'), @IdAdmin),
    ('QR345ST', N'Chevrolet',    N'Cruze',     2018,  87000,  6700000.00, N'LTZ automático.',                       3, (SELECT Id FROM Persona WHERE Documento = N'33789012'), @IdAdmin),
    ('STU901',  N'Nissan',       N'Sentra',    2017, 105000,  5800000.00, N'Exclusive full.',                       3, (SELECT Id FROM Persona WHERE Documento = N'30704567890'), @IdAdmin),
    -- 4 PendienteVenta
    ('UV678WX', N'Jeep',         N'Compass',   2022,  22000, 14800000.00, N'Longitude. Lista para venta.',          4, (SELECT Id FROM Persona WHERE Documento = N'26890123'), @IdAdmin),
    ('VWX234',  N'Hyundai',      N'Tucson',    2019,  68000, 11200000.00, N'4x2 2.0 nafta.',                        4, (SELECT Id FROM Persona WHERE Documento = N'30705678901'), @IdAdmin),
    ('YZ901AB', N'BMW',          N'Serie 3',   2020,  40000, 22500000.00, N'320i Sport. Impecable.',                4, (SELECT Id FROM Persona WHERE Documento = N'34901234'), @IdAdmin),
    ('XYZ567',  N'Audi',         N'A3',        2018,  73000, 13900000.00, N'Sportback. Único dueño.',               4, (SELECT Id FROM Persona WHERE Documento = N'30706789012'), @IdAdmin),
    -- 4 EnVenta
    ('CD234EF', N'Mercedes-Benz',N'Clase A',   2021,  31000, 24000000.00, N'A200 Progressive. Publicado.',          5, (SELECT Id FROM Persona WHERE Documento = N'29012345'), @IdAdmin),
    ('ZAB890',  N'Toyota',       N'Yaris',     2022,  12000,  8800000.00, N'XLS CVT. En venta.',                    5, (SELECT Id FROM Persona WHERE Documento = N'30707890123'), @IdAdmin),
    ('GH567IJ', N'Kia',          N'Sportage',  2019,  65000, 12300000.00, N'LX AT. Impecable.',                     5, (SELECT Id FROM Persona WHERE Documento = N'30708901234'), @IdAdmin),
    ('BCD123',  N'Tesla',        N'Model 3',   2022,  18000, 38500000.00, N'Long Range AWD. Publicado.',            5, (SELECT Id FROM Persona WHERE Documento = N'30709012345'), @IdAdmin)
GO

-- Historial de estados: insertamos las transiciones correspondientes al estado actual de cada unidad.
-- Nota: por simplicidad el "autor" de cada transición es admin; en el flujo real serían distintos usuarios.
DECLARE @IdAdmin2 INT = (SELECT Id FROM Usuario WHERE Username = 'admin');

-- Todas las unidades: entrada inicial (alta → Ingresado)
INSERT INTO HistorialEstadoUnidad (IdUnidad, EstadoOrigen, EstadoDestino, IdUsuario, Motivo)
SELECT u.Id, NULL, 1, @IdAdmin2, NULL
FROM Unidad u
WHERE u.EstadoActual IN (1, 2, 3, 4, 5);

-- Transiciones adicionales según el estado destino final:
-- Todas las unidades que pasaron de Ingresado: 1 -> 3 (EnPreparacion, el Encargado toma la unidad)
INSERT INTO HistorialEstadoUnidad (IdUnidad, EstadoOrigen, EstadoDestino, IdUsuario, Motivo)
SELECT u.Id, 1, 3, @IdAdmin2, N'Encargado toma la unidad para preparación.'
FROM Unidad u WHERE u.EstadoActual IN (2, 3, 4, 5);

-- Las que están en RequiereAprobacionPresupuesto (2): EnPreparacion -> RequiereAprobacionPresupuesto (3 -> 2)
INSERT INTO HistorialEstadoUnidad (IdUnidad, EstadoOrigen, EstadoDestino, IdUsuario, Motivo)
SELECT u.Id, 3, 2, @IdAdmin2, N'Encargado detecta extras y envía presupuesto al Gerente.'
FROM Unidad u WHERE u.EstadoActual = 2;

-- Las que llegaron a PendienteVenta (4): EnPreparacion -> PendienteVenta (3 -> 4)
INSERT INTO HistorialEstadoUnidad (IdUnidad, EstadoOrigen, EstadoDestino, IdUsuario, Motivo)
SELECT u.Id, 3, 4, @IdAdmin2, N'Preparación finalizada.'
FROM Unidad u WHERE u.EstadoActual IN (4, 5);

-- Las que llegaron a EnVenta (5): PendienteVenta -> EnVenta (4 -> 5)
INSERT INTO HistorialEstadoUnidad (IdUnidad, EstadoOrigen, EstadoDestino, IdUsuario, Motivo)
SELECT u.Id, 4, 5, @IdAdmin2, N'Publicación aprobada.'
FROM Unidad u WHERE u.EstadoActual = 5;
GO

-- Checklists de las unidades seed que ya salieron de Ingresado (mismo resultado que
-- CrearChecklistDeUnidad: snapshot de los items activos del template, en estado Aprobado).
DECLARE @IdAdmin3 INT = (SELECT Id FROM Usuario WHERE Username = 'admin');

INSERT INTO ChecklistPreparacion (IdUnidad, IdCreadorUsuario)
SELECT u.Id, @IdAdmin3 FROM Unidad u WHERE u.EstadoActual IN (2, 3, 4, 5);

INSERT INTO ChecklistItem (IdChecklist, Nombre, Descripcion, EsDelTemplate, IdTemplateOrigen, EstadoAprobacion)
SELECT cp.Id, t.Nombre, t.Descripcion, 1, t.Id, 1
FROM ChecklistPreparacion cp
CROSS JOIN ChecklistItemTemplate t
WHERE t.Activo = 1;

-- RequiereAprobacionPresupuesto (2): un extra Propuesto con costo, pendiente de decisión del Gerente.
INSERT INTO ChecklistItem (IdChecklist, Nombre, Descripcion, EsDelTemplate, IdTemplateOrigen, CostoEstimado, EstadoAprobacion)
SELECT cp.Id, N'Reparación de paragolpes', N'Raspones y fisura en paragolpes delantero.', 0, NULL, 150000.00, 2
FROM ChecklistPreparacion cp
INNER JOIN Unidad u ON u.Id = cp.IdUnidad
WHERE u.EstadoActual = 2;

-- PendienteVenta (4) y EnVenta (5): preparación finalizada → todos los items revisados OK.
UPDATE ci
   SET ResultadoRevision = 2,
       FechaRevision     = GETDATE(),
       IdUsuarioRevisor  = @IdAdmin3
FROM ChecklistItem ci
INNER JOIN ChecklistPreparacion cp ON cp.Id = ci.IdChecklist
INNER JOIN Unidad u               ON u.Id  = cp.IdUnidad
WHERE u.EstadoActual IN (4, 5);
GO

-- Publicaciones iniciadas de las unidades seed: las 4 En venta y 2 de las Pendiente de venta
-- (las otras 2 quedan sin publicación iniciada, así no aparecen en el listado de Publicaciones).
INSERT INTO PublicacionUnidad (IdUnidad, PrecioPublicacion, DescripcionPublicacion, FechaCreacion, FechaUltimaEdicion)
SELECT u.Id, v.Precio, v.Descripcion, DATEADD(DAY, -v.DiasCreacion, GETDATE()),
       CASE WHEN v.DiasEdicion IS NULL THEN NULL ELSE DATEADD(DAY, -v.DiasEdicion, GETDATE()) END
FROM (VALUES
    (N'CD234EF', 28900000.00, N'Mercedes-Benz Clase A200 Progressive 2021, 31.000 km. Service oficial al día, único dueño. Tapizado de cuero, pantalla dual MBUX, cámara de retroceso y sensores de estacionamiento. Lista para transferir.', 12, 3),
    (N'ZAB890',  10950000.00, N'Toyota Yaris XLS CVT 2022 con sólo 12.000 km. Garantía de fábrica vigente, Toyota Safety Sense, Apple CarPlay / Android Auto. Impecable estado general.', 10, 2),
    (N'GH567IJ', 14800000.00, N'Kia Sportage LX AT 2019, 65.000 km. Cubiertas nuevas, frenos revisados y detallado completo interior/exterior. Ideal familia.', 8, NULL),
    (N'BCD123',  45500000.00, N'Tesla Model 3 Long Range AWD 2022, 18.000 km. Autonomía de más de 500 km, Autopilot, techo de cristal. Cargador de pared incluido.', 6, 1),
    (N'UV678WX', 17900000.00, N'Jeep Compass Longitude 2022, 22.000 km. Preparación finalizada, pendiente de aprobación final.', 2, NULL),
    (N'YZ901AB', NULL,        NULL,                                                                                                                    1, NULL)
) AS v(Dominio, Precio, Descripcion, DiasCreacion, DiasEdicion)
INNER JOIN Unidad u ON u.Dominio = v.Dominio;
GO

-- ---------------------------------------------------------
-- Enum EstadoAprobacionItem
-- ---------------------------------------------------------
DECLARE @formEAI NVARCHAR(80) = N'EstadoAprobacionItem';
DECLARE @ctrlEAI TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlEAI (Codigo, Es, En, De) VALUES
    (N'Aprobado',   N'Aprobado',   N'Approved',   N'Genehmigt'),
    (N'Propuesto',  N'Propuesto',  N'Proposed',   N'Vorgeschlagen'),
    (N'Rechazado',  N'Rechazado',  N'Rejected',   N'Abgelehnt');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formEAI FROM @ctrlEAI;

DECLARE @idEsEAI INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnEAI INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeEAI INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsEAI, c.Id, t.Es FROM @ctrlEAI t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEAI
UNION ALL
SELECT @idEnEAI, c.Id, t.En FROM @ctrlEAI t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEAI
UNION ALL
SELECT @idDeEAI, c.Id, t.De FROM @ctrlEAI t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEAI;
GO

-- ---------------------------------------------------------
-- Enum ResultadoRevision
-- ---------------------------------------------------------
DECLARE @formRR NVARCHAR(80) = N'ResultadoRevision';
DECLARE @ctrlRR TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlRR (Codigo, Es, En, De) VALUES
    (N'Pendiente', N'Pendiente', N'Pending',   N'Ausstehend'),
    (N'OK',        N'OK',        N'OK',        N'OK'),
    (N'Observado', N'Observado', N'Observed',  N'Beanstandet');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formRR FROM @ctrlRR;

DECLARE @idEsRR INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnRR INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeRR INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsRR, c.Id, t.Es FROM @ctrlRR t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRR
UNION ALL
SELECT @idEnRR, c.Id, t.En FROM @ctrlRR t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRR
UNION ALL
SELECT @idDeRR, c.Id, t.De FROM @ctrlRR t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formRR;
GO

-- ---------------------------------------------------------
-- Enum TipoOperacionVenta
-- ---------------------------------------------------------
DECLARE @formTOV NVARCHAR(80) = N'TipoOperacionVenta';
DECLARE @ctrlTOV TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlTOV (Codigo, Es, En, De) VALUES
    (N'Venta',   N'Venta',   N'Sale',       N'Verkauf'),
    (N'Reserva', N'Reserva', N'Reservation',N'Reservierung');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formTOV FROM @ctrlTOV;

DECLARE @idEsTOV INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnTOV INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeTOV INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsTOV, c.Id, t.Es FROM @ctrlTOV t INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTOV
UNION ALL
SELECT @idEnTOV, c.Id, t.En FROM @ctrlTOV t INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTOV
UNION ALL
SELECT @idDeTOV, c.Id, t.De FROM @ctrlTOV t INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTOV;
GO

-- ============================================================
-- Fix i18n — Nuevas claves en FormUnidades (filtros Marca/Modelo/Año,
-- "(Todos)", botón ayuda flujo, Marcar/Desmarcar todos) y
-- FormDetalleUnidad (botones de la nueva máquina de estados y revisión).
-- ============================================================

DECLARE @formUndFix NVARCHAR(80) = N'FormUnidades';
DECLARE @ctrlUndFix TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlUndFix (Codigo, Es, En, De) VALUES
    (N'lblFiltroMarca',    N'Marca:',             N'Brand:',             N'Marke:'),
    (N'lblFiltroModelo',   N'Modelo:',            N'Model:',             N'Modell:'),
    (N'lblFiltroAnio',     N'Año:',               N'Year:',              N'Jahr:'),
    (N'itemTodos',         N'(Todos)',            N'(All)',              N'(Alle)'),
    (N'btnAyudaEstados',   N'Flujo de estados',   N'State flow',         N'Zustandsablauf'),
    (N'btnMarcarTodos',    N'Marcar todos',       N'Select all',         N'Alle markieren'),
    (N'btnDesmarcarTodos', N'Desmarcar todos',    N'Clear all',          N'Alle abwählen');

INSERT INTO Control (Codigo, Form)
SELECT f.Codigo, @formUndFix
FROM @ctrlUndFix f
WHERE NOT EXISTS (SELECT 1 FROM Control c WHERE c.Codigo = f.Codigo AND c.Form = @formUndFix);

DECLARE @idEsUndFix INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnUndFix INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeUndFix INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsUndFix, c.Id, t.Es FROM @ctrlUndFix t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUndFix
WHERE NOT EXISTS (SELECT 1 FROM Traduccion x WHERE x.IdIdioma = @idEsUndFix AND x.IdControl = c.Id)
UNION ALL
SELECT @idEnUndFix, c.Id, t.En FROM @ctrlUndFix t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUndFix
WHERE NOT EXISTS (SELECT 1 FROM Traduccion x WHERE x.IdIdioma = @idEnUndFix AND x.IdControl = c.Id)
UNION ALL
SELECT @idDeUndFix, c.Id, t.De FROM @ctrlUndFix t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUndFix
WHERE NOT EXISTS (SELECT 1 FROM Traduccion x WHERE x.IdIdioma = @idDeUndFix AND x.IdControl = c.Id);
GO

DECLARE @formDetUFix NVARCHAR(80) = N'FormDetalleUnidad';
DECLARE @ctrlDetUFix TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlDetUFix (Codigo, Es, En, De) VALUES
    -- Nuevos botones de la máquina de estados y revisión por ítem
    (N'btnTomarPreparacion',    N'Preparar Unidad',                     N'Prepare Unit',                 N'Einheit vorbereiten'),
    (N'btnEnviarPresupuesto',   N'Enviar presupuesto al Gerente',       N'Send budget',                  N'Kostenvoranschlag senden'),
    (N'btnAprobarPresupuesto',  N'Aprobar presupuesto',                 N'Approve budget',               N'Kostenvoranschlag genehmigen'),
    (N'btnRechazarPresupuesto', N'Rechazar presupuesto',                N'Reject budget',                N'Kostenvoranschlag ablehnen'),
    (N'btnMarcarOK',            N'Marcar OK',                           N'Mark OK',                      N'Als OK markieren'),
    (N'btnMarcarObservado',     N'Marcar Observado',                    N'Mark Observed',                N'Als beanstandet markieren'),
    (N'lblComentarioRevision',  N'Comentario (obligatorio al Observar):', N'Comment (required if Observed):', N'Kommentar (bei Beanstandung erforderlich):'),
    -- N02 — botones del detalle de unidad
    (N'btnPublicacion',         N'Publicación',                         N'Publication',                  N'Veröffentlichung'),
    (N'btnVender',              N'Vender',                              N'Sell',                         N'Verkaufen'),
    (N'btnReservar',            N'Reservar',                            N'Reserve',                      N'Reservieren'),
    (N'btnPausar',              N'Pausar',                              N'Pause',                        N'Pausieren'),
    (N'btnReanudar',            N'Reanudar',                            N'Resume',                       N'Fortsetzen'),
    (N'btnCancelarReserva',     N'Cancelar reserva',                    N'Cancel reservation',           N'Reservierung stornieren'),
    (N'promptMotivoCancelacion',N'Motivo de la cancelación de la reserva (obligatorio):', N'Reason for reservation cancellation (required):', N'Grund für die Stornierung der Reservierung (erforderlich):'),
    (N'msgMotivoCancelacionObligatorio', N'El motivo de la cancelación es obligatorio.', N'The cancellation reason is required.', N'Der Stornierungsgrund ist erforderlich.'),
    -- Columnas nuevas del checklist
    (N'colChkAprobacion',       N'Aprobación',                          N'Approval',                     N'Genehmigung'),
    (N'colChkCosto',            N'Costo',                               N'Cost',                         N'Kosten'),
    (N'colChkRevision',         N'Revisión',                            N'Review',                       N'Prüfung'),
    (N'colChkComentario',       N'Comentario',                          N'Comment',                      N'Kommentar'),
    -- Validaciones del nuevo flujo
    (N'msgComentarioObservadoObligatorio', N'Al marcar un ítem como Observado el comentario es obligatorio.', N'When marking an item as Observed the comment is required.', N'Beim Markieren eines Elements als beanstandet ist der Kommentar erforderlich.'),
    (N'promptCostoItem',        N'Costo estimado (en pesos):',          N'Estimated cost (in pesos):',   N'Geschätzte Kosten (in Pesos):'),
    (N'msgCostoInvalido',       N'Costo inválido.',                     N'Invalid cost.',                N'Ungültige Kosten.');

INSERT INTO Control (Codigo, Form)
SELECT f.Codigo, @formDetUFix
FROM @ctrlDetUFix f
WHERE NOT EXISTS (SELECT 1 FROM Control c WHERE c.Codigo = f.Codigo AND c.Form = @formDetUFix);

DECLARE @idEsDetUFix INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnDetUFix INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDeDetUFix INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsDetUFix, c.Id, t.Es FROM @ctrlDetUFix t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formDetUFix
WHERE NOT EXISTS (SELECT 1 FROM Traduccion x WHERE x.IdIdioma = @idEsDetUFix AND x.IdControl = c.Id)
UNION ALL
SELECT @idEnDetUFix, c.Id, t.En FROM @ctrlDetUFix t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formDetUFix
WHERE NOT EXISTS (SELECT 1 FROM Traduccion x WHERE x.IdIdioma = @idEnDetUFix AND x.IdControl = c.Id)
UNION ALL
SELECT @idDeDetUFix, c.Id, t.De FROM @ctrlDetUFix t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formDetUFix
WHERE NOT EXISTS (SELECT 1 FROM Traduccion x WHERE x.IdIdioma = @idDeDetUFix AND x.IdControl = c.Id);
GO

-- ---------------------------------------------------------
-- FormPublicaciones — listado + vista de la publicación
-- ---------------------------------------------------------
DECLARE @formPubs NVARCHAR(80) = N'FormPublicaciones';
DECLARE @ctrlPubs TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlPubs (Codigo, Es, En, De) VALUES
    (N'title',                 N'Publicaciones',                                          N'Listings',                                            N'Inserate'),
    (N'lblTitulo',             N'Publicaciones',                                          N'Listings',                                            N'Inserate'),
    (N'lblBuscarDominio',      N'Dominio:',                                               N'Plate:',                                              N'Kennzeichen:'),
    (N'lblFiltroMarca',        N'Marca:',                                                 N'Brand:',                                              N'Marke:'),
    (N'lblFiltroModelo',       N'Modelo:',                                                N'Model:',                                              N'Modell:'),
    (N'lblFiltroAnio',         N'Año:',                                                   N'Year:',                                               N'Jahr:'),
    (N'lblPrecioDesde',        N'Precio desde:',                                          N'Price from:',                                         N'Preis ab:'),
    (N'lblPrecioHasta',        N'Hasta:',                                                 N'To:',                                                 N'Bis:'),
    (N'lblKmHasta',            N'Km hasta:',                                              N'Max. km:',                                            N'Max. km:'),
    (N'lblBuscarTexto',        N'Descripción:',                                           N'Description:',                                        N'Beschreibung:'),
    (N'lblFiltroEstado',       N'Estado:',                                                N'State:',                                              N'Zustand:'),
    (N'chkSoloConPrecio',      N'Sólo con precio',                                        N'Only with price',                                     N'Nur mit Preis'),
    (N'itemTodos',             N'(Todos)',                                                N'(All)',                                               N'(Alle)'),
    (N'btnLimpiar',            N'Limpiar filtros',                                        N'Clear filters',                                       N'Filter löschen'),
    (N'btnActualizar',         N'Actualizar',                                             N'Refresh',                                             N'Aktualisieren'),
    (N'btnVerDetalle',         N'Ver detalle',                                            N'View details',                                        N'Details ansehen'),
    (N'btnEditarPublicacion',  N'Editar publicación',                                     N'Edit listing',                                        N'Inserat bearbeiten'),
    (N'colDominio',            N'Dominio',                                                N'Plate',                                               N'Kennzeichen'),
    (N'colMarca',              N'Marca',                                                  N'Brand',                                               N'Marke'),
    (N'colModelo',             N'Modelo',                                                 N'Model',                                               N'Modell'),
    (N'colAnio',               N'Año',                                                    N'Year',                                                N'Jahr'),
    (N'colKm',                 N'Km',                                                     N'Mileage',                                             N'Km'),
    (N'colEstado',             N'Estado',                                                 N'State',                                               N'Zustand'),
    (N'colPrecio',             N'Precio',                                                 N'Price',                                               N'Preis'),
    (N'colFotos',              N'Fotos',                                                  N'Photos',                                              N'Fotos'),
    (N'colActualizacion',      N'Última edición',                                         N'Last edited',                                         N'Zuletzt bearbeitet'),
    (N'lblTotal',              N'Total: {0}',                                             N'Total: {0}',                                          N'Gesamt: {0}'),
    (N'lblDetalleVacio',       N'Seleccioná una publicación y presioná "Ver detalle".',   N'Select a listing and press "View details".',          N'Wähle ein Inserat aus und klicke auf "Details ansehen".'),
    (N'lblSubtituloPub',       N'Dominio {0}  ·  {1} km',                                 N'Plate {0}  ·  {1} km',                                N'Kennzeichen {0}  ·  {1} km'),
    (N'lblFechasPub',          N'Publicación iniciada el {0:dd/MM/yyyy}',                 N'Listing started on {0:dd/MM/yyyy}',                   N'Inserat erstellt am {0:dd/MM/yyyy}'),
    (N'lblFechasPubEditada',   N'Publicación iniciada el {0:dd/MM/yyyy}  ·  Última edición {1:dd/MM/yyyy HH:mm}', N'Listing started on {0:dd/MM/yyyy}  ·  Last edited {1:dd/MM/yyyy HH:mm}', N'Inserat erstellt am {0:dd/MM/yyyy}  ·  Zuletzt bearbeitet {1:dd/MM/yyyy HH:mm}'),
    (N'lblDescripcionTitulo',  N'Descripción',                                            N'Description',                                         N'Beschreibung'),
    (N'lblFotosTitulo',        N'Fotos ({0})',                                            N'Photos ({0})',                                        N'Fotos ({0})'),
    (N'lblSinFotos',           N'Esta unidad no tiene fotos cargadas.',                   N'This unit has no photos.',                            N'Für diese Einheit sind keine Fotos vorhanden.'),
    (N'tooltipAmpliar',        N'Click para ampliar',                                     N'Click to enlarge',                                    N'Zum Vergrößern klicken'),
    (N'lblAyudaVisor',         N'Esc para cerrar  ·  ← → para navegar',                   N'Esc to close  ·  ← → to navigate',                    N'Esc zum Schließen  ·  ← → zum Blättern'),
    (N'valorSinPrecio',        N'Precio a consultar',                                     N'Price on request',                                    N'Preis auf Anfrage'),
    (N'valorSinPrecioCorto',   N'—',                                                      N'—',                                                   N'—'),
    (N'valorSinDescripcion',   N'(sin descripción)',                                      N'(no description)',                                    N'(keine Beschreibung)'),
    (N'msgSeleccionar',        N'Seleccioná una publicación.',                            N'Select a listing.',                                   N'Wähle ein Inserat aus.'),
    (N'msgInformacion',        N'Info',                                                   N'Info',                                                N'Info'),
    (N'msgError',              N'Error',                                                  N'Error',                                               N'Fehler');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formPubs FROM @ctrlPubs;

DECLARE @idEsPubs INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnPubs INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDePubs INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsPubs, c.Id, t.Es FROM @ctrlPubs t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPubs
UNION ALL
SELECT @idEnPubs, c.Id, t.En FROM @ctrlPubs t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPubs
UNION ALL
SELECT @idDePubs, c.Id, t.De FROM @ctrlPubs t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPubs;
GO

-- ---------------------------------------------------------
-- FormPublicacion — modal de edición de la publicación
-- ---------------------------------------------------------
DECLARE @formPub NVARCHAR(80) = N'FormPublicacion';
DECLARE @ctrlPub TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000), De NVARCHAR(1000));

INSERT INTO @ctrlPub (Codigo, Es, En, De) VALUES
    (N'title',                  N'Publicación',                  N'Listing',                    N'Inserat'),
    (N'lblTitulo',              N'Publicación de la unidad',     N'Unit listing',               N'Inserat der Einheit'),
    (N'lblPrecio',              N'Precio ($):',                  N'Price ($):',                 N'Preis ($):'),
    (N'lblDescripcion',         N'Descripción:',                 N'Description:',               N'Beschreibung:'),
    (N'btnGuardar',             N'Guardar',                      N'Save',                       N'Speichern'),
    (N'lblImagenes',            N'Imágenes:',                    N'Images:',                    N'Bilder:'),
    (N'btnAgregarImagen',       N'Agregar imagen',               N'Add image',                  N'Bild hinzufügen'),
    (N'btnQuitarImagen',        N'Quitar imagen',                N'Remove image',               N'Bild entfernen'),
    (N'filtroImagenes',         N'Imágenes',                     N'Images',                     N'Bilder'),
    (N'msgPrecioInvalido',      N'Precio inválido.',             N'Invalid price.',             N'Ungültiger Preis.'),
    (N'msgAdvertencia',         N'Advertencia',                  N'Warning',                    N'Warnung'),
    (N'msgPublicacionGuardada', N'Publicación guardada.',        N'Listing saved.',             N'Inserat gespeichert.'),
    (N'msgExito',               N'Éxito',                        N'Success',                    N'Erfolg'),
    (N'msgError',               N'Error',                        N'Error',                      N'Fehler');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formPub FROM @ctrlPub;

DECLARE @idEsPub INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnPub INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');
DECLARE @idDePub INT = (SELECT Id FROM Idioma WHERE Nombre = N'Deutsch');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsPub, c.Id, t.Es FROM @ctrlPub t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPub
UNION ALL
SELECT @idEnPub, c.Id, t.En FROM @ctrlPub t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPub
UNION ALL
SELECT @idDePub, c.Id, t.De FROM @ctrlPub t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPub;
GO
