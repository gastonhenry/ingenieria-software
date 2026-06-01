USE [ingenieria]
GO

IF OBJECT_ID('dbo.Bitacora',          'U') IS NOT NULL DROP TABLE dbo.Bitacora
IF OBJECT_ID('dbo.UsuarioPermiso',    'U') IS NOT NULL DROP TABLE dbo.UsuarioPermiso
IF OBJECT_ID('dbo.UsuarioRol',        'U') IS NOT NULL DROP TABLE dbo.UsuarioRol
IF OBJECT_ID('dbo.RolPermiso',        'U') IS NOT NULL DROP TABLE dbo.RolPermiso
IF OBJECT_ID('dbo.Rol',               'U') IS NOT NULL DROP TABLE dbo.Rol
IF OBJECT_ID('dbo.Permiso',           'U') IS NOT NULL DROP TABLE dbo.Permiso
IF OBJECT_ID('dbo.Usuario',           'U') IS NOT NULL DROP TABLE dbo.Usuario
IF OBJECT_ID('dbo.DigitoVerificador', 'U') IS NOT NULL DROP TABLE dbo.DigitoVerificador
IF OBJECT_ID('dbo.Traduccion',        'U') IS NOT NULL DROP TABLE dbo.Traduccion
IF OBJECT_ID('dbo.Control',           'U') IS NOT NULL DROP TABLE dbo.Control
IF OBJECT_ID('dbo.Idioma',            'U') IS NOT NULL DROP TABLE dbo.Idioma
GO

CREATE TABLE [dbo].[Usuario](
    [Id]               [int]           IDENTITY(1,1) NOT NULL,
    [Username]         [nvarchar](50)  NOT NULL,
    [Hash]             [nvarchar](255) NOT NULL,
    [Salt]             [nvarchar](50)  NOT NULL DEFAULT '',
    [Nombre]           [nvarchar](50)  NOT NULL,
    [Apellido]         [nvarchar](50)  NOT NULL,
    [Email]            [nvarchar](255) NOT NULL,
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

CREATE TABLE [dbo].[Permiso](
    [Id]          [int]           IDENTITY(1,1) NOT NULL,
    [Codigo]      [nvarchar](50)  NOT NULL,
    [Descripcion] [nvarchar](255) NULL,
    [Tipo]        [char](1)       NOT NULL,
    [IdPadre]     [int]           NULL,
    [DVH]         [nvarchar](64)  NULL,
    CONSTRAINT [PK_Permiso]        PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Permiso_Codigo] UNIQUE ([Codigo]),
    CONSTRAINT [CK_Permiso_Tipo]   CHECK ([Tipo] IN ('I','F')),
    CONSTRAINT [FK_Permiso_Padre]  FOREIGN KEY ([IdPadre]) REFERENCES [dbo].[Permiso] ([Id])
)
GO

CREATE TABLE [dbo].[Rol](
    [Id]          [int]           IDENTITY(1,1) NOT NULL,
    [Nombre]      [nvarchar](50)  NOT NULL,
    [Descripcion] [nvarchar](255) NULL,
    [DVH]         [nvarchar](64)  NULL,
    CONSTRAINT [PK_Rol]        PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Rol_Nombre] UNIQUE ([Nombre])
)
GO

CREATE TABLE [dbo].[RolPermiso](
    [RolId]     [int] NOT NULL,
    [PermisoId] [int] NOT NULL,
    CONSTRAINT [PK_RolPermiso]         PRIMARY KEY CLUSTERED ([RolId], [PermisoId]),
    CONSTRAINT [FK_RolPermiso_Rol]     FOREIGN KEY ([RolId])     REFERENCES [dbo].[Rol] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RolPermiso_Permiso] FOREIGN KEY ([PermisoId]) REFERENCES [dbo].[Permiso] ([Id])
)
GO

CREATE TABLE [dbo].[UsuarioRol](
    [UsuarioId] [int] NOT NULL,
    [RolId]     [int] NOT NULL,
    CONSTRAINT [PK_UsuarioRol]         PRIMARY KEY CLUSTERED ([UsuarioId], [RolId]),
    CONSTRAINT [FK_UsuarioRol_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UsuarioRol_Rol]     FOREIGN KEY ([RolId])     REFERENCES [dbo].[Rol] ([Id])
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

CREATE OR ALTER PROCEDURE [dbo].[Login]
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, [Hash], [Salt], Nombre, Apellido, Email, Bloqueado, IntentosFallidos, UltimoLogin
    FROM Usuario
    WHERE Username = @Username;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarUsuario]
    @Username NVARCHAR(50),
    @Hash     NVARCHAR(255),
    @Salt     NVARCHAR(50),
    @Nombre   NVARCHAR(50),
    @Apellido NVARCHAR(50),
    @Email    NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Usuario ([Username], [Hash], [Salt], [Nombre], [Apellido], [Email])
    VALUES (@Username, @Hash, @Salt, @Nombre, @Apellido, @Email);
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
    SELECT Id, Username, Nombre, Apellido, Email, IntentosFallidos, Bloqueado, UltimoLogin
    FROM Usuario
    ORDER BY Bloqueado DESC, Username ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarUsuariosParaVerificacion]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, [Hash], [Salt], Nombre, Apellido, Email, IntentosFallidos, Bloqueado, UltimoLogin, DVH
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

CREATE OR ALTER PROCEDURE [dbo].[ListarRoles]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Descripcion, DVH
    FROM Rol
    ORDER BY Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarRol]
    @Nombre      NVARCHAR(50),
    @Descripcion NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Rol (Nombre, Descripcion) VALUES (@Nombre, @Descripcion);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarPermisosDeRol]
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.Id, P.Codigo, P.Descripcion, P.Tipo, P.IdPadre, P.DVH
    FROM Permiso P
    INNER JOIN RolPermiso RP ON RP.PermisoId = P.Id
    WHERE RP.RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[AsignarPermisoARol]
    @RolId     INT,
    @PermisoId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM RolPermiso WHERE RolId = @RolId AND PermisoId = @PermisoId)
        INSERT INTO RolPermiso (RolId, PermisoId) VALUES (@RolId, @PermisoId);
END
GO

CREATE OR ALTER PROCEDURE [dbo].[QuitarPermisoDeRol]
    @RolId     INT,
    @PermisoId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM RolPermiso WHERE RolId = @RolId AND PermisoId = @PermisoId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ActualizarDVHRol]
    @RolId INT,
    @DVH   NVARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Rol SET DVH = @DVH WHERE Id = @RolId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[ListarRolesDeUsuario]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT R.Id, R.Nombre, R.Descripcion, R.DVH
    FROM Rol R
    INNER JOIN UsuarioRol UR ON UR.RolId = R.Id
    WHERE UR.UsuarioId = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[AsignarRolAUsuario]
    @UsuarioId INT,
    @RolId     INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM UsuarioRol WHERE UsuarioId = @UsuarioId AND RolId = @RolId)
        INSERT INTO UsuarioRol (UsuarioId, RolId) VALUES (@UsuarioId, @RolId);
END
GO

CREATE OR ALTER PROCEDURE [dbo].[QuitarRolDeUsuario]
    @UsuarioId INT,
    @RolId     INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM UsuarioRol WHERE UsuarioId = @UsuarioId AND RolId = @RolId;
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
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Desasignar el rol de todos los usuarios que lo tienen
    DELETE FROM UsuarioRol WHERE RolId = @RolId;
    -- RolPermiso se elimina por cascada (FK_RolPermiso_Rol ON DELETE CASCADE)
    DELETE FROM Rol WHERE Id = @RolId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EliminarFamiliaPermiso]
    @PermisoId INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Detach: hijos directos quedan sin padre (no se borran las patentes ni sub-familias)
    UPDATE Permiso SET IdPadre = NULL WHERE IdPadre = @PermisoId;
    -- Quitar la familia de cualquier rol que la tuviera asignada
    DELETE FROM RolPermiso     WHERE PermisoId = @PermisoId;
    -- Quitar la familia de cualquier usuario que la tuviera asignada directamente
    DELETE FROM UsuarioPermiso WHERE PermisoId = @PermisoId;
    -- Eliminar la familia
    DELETE FROM Permiso WHERE Id = @PermisoId AND Tipo = 'F';
END
GO

DBCC CHECKIDENT ('Usuario', RESEED, 1)
INSERT INTO Usuario (Username, Hash, Salt, Nombre, Apellido, Email, DVH)
VALUES (
    'admin',
    '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
    'VQUk8CQ1S2V3oSbMWAp4qg==',
    'Admin',
    'Admin',
    'admin@ingenieria.com',
    NULL
),
(
    'pepe',
    '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
    'VQUk8CQ1S2V3oSbMWAp4qg==',
    'Pepe',
    'Pepe',
    'pepe@ingenieria.com',
    NULL
)
GO

-- Permisos fijos que se crean cuando se desarrolla un nuevo módulo o funcionalidad.
INSERT INTO Permiso (Codigo, Descripcion, Tipo) VALUES
    ('VER_BITACORA', 'Habilita el menú Bitácora y el acceso al módulo.', 'I')
GO

-- =========================================================
-- MULTI-IDIOMA (patrón Observer)
-- =========================================================

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
INSERT INTO Idioma (Nombre) VALUES ('Español'), ('English')
GO