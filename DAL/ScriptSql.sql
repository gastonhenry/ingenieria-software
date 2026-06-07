USE [ingenieria]
GO

DECLARE @sql NVARCHAR(MAX) = N'';

-- 1) Drop todos los SP
SELECT @sql += 'DROP PROCEDURE [' + s.name + '].[' + p.name + '];' + CHAR(10)
FROM sys.procedures p
INNER JOIN sys.schemas s ON s.schema_id = p.schema_id
WHERE s.name = 'dbo';
EXEC sp_executesql @sql;

-- 2) Drop todas las FK
SET @sql = N'';
SELECT @sql += 'ALTER TABLE [' + s.name + '].[' + OBJECT_NAME(fk.parent_object_id)
    + '] DROP CONSTRAINT [' + fk.name + '];' + CHAR(10)
FROM sys.foreign_keys fk
INNER JOIN sys.schemas s ON s.schema_id = fk.schema_id
WHERE s.name = 'dbo';
EXEC sp_executesql @sql;

-- 3) Drop todas las tablas
SET @sql = N'';
SELECT @sql += 'DROP TABLE [' + s.name + '].[' + t.name + '];' + CHAR(10)
FROM sys.tables t
INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
WHERE s.name = 'dbo';
EXEC sp_executesql @sql;
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
    'admin@ingenieria.com',
    '+54 11 5555-0001',
    '99999999',
    'Av. Siempre Viva 742, CABA',
    NULL
),
(
    'pepe',
    '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
    'VQUk8CQ1S2V3oSbMWAp4qg==',
    'Pepe',
    'Pepe',
    'pepe@ingenieria.com',
    '+54 11 5555-0002',
    '30123456',
    'Calle Falsa 123, CABA',
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
WHERE Username IN ('admin', 'pepe');
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
INSERT INTO Idioma (Nombre) VALUES ('Español'), ('English')
GO

-- ============================================================
-- FormPrincipal — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formPpal NVARCHAR(80) = N'FormPrincipal';
DECLARE @ctrlPpal TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlPpal (Codigo, Es, En) VALUES
    -- Sesión / estado
    (N'lblSesion',                N'Sesión',                                              N'Session'),
    (N'tooltipSesionActiva',      N'Sesión activa',                                       N'Active session'),
    -- Menú top-level
    (N'menuInicio',               N'Inicio',                                              N'Home'),
    (N'menuUsuarios',             N'Usuarios',                                            N'Users'),
    (N'menuPermisos',             N'Permisos',                                            N'Permissions'),
    (N'menuBitacora',             N'Bitácora',                                            N'Audit Log'),
    (N'menuIdiomas',              N'Idiomas',                                             N'Languages'),
    (N'menuSeleccionIdioma',      N'Idioma ▾',                                            N'Language ▾'),
    (N'menuLogout',               N'Cerrar Sesión',                                       N'Logout'),
    -- Submenús
    (N'menuInsertarUsuario',      N'Registrar Usuario',                                   N'Register User'),
    (N'menuVerUsuarios',          N'Gestión de Usuarios',                                 N'Manage Users'),
    (N'menuGestionPermisos',      N'Gestión de Permisos',                                 N'Manage Permissions'),
    (N'menuAsignacionPermisos',   N'Asignación de Permisos a Usuarios',                   N'Assign Permissions to Users'),
    (N'menuVerBitacora',          N'Ver Bitácora',                                        N'View Audit Log'),
    (N'menuGestionIdiomas',       N'Gestión de Idiomas',                                  N'Manage Languages'),
    -- Items dinámicos del selector de idioma
    (N'itemSinIdiomas',           N'(sin idiomas disponibles)',                           N'(no languages available)'),
    (N'tooltipIdiomaEnProceso',   N'Idioma en proceso de creación: faltan traducciones.', N'Language in progress: translations missing.'),
    -- MessageBoxes
    (N'msgConfirmarLogout',       N'¿Querés cerrar sesión?',                              N'Do you want to log out?'),
    (N'msgConfirmarLogoutTitulo', N'Confirmar cierre de sesión',                          N'Confirm logout'),
    (N'msgErrorCambiarIdioma',    N'Error al cambiar idioma:',                            N'Error changing language:'),
    (N'msgError',                 N'Error',                                               N'Error');

INSERT INTO Control (Codigo, Form)
SELECT Codigo, @formPpal FROM @ctrlPpal;

DECLARE @idEsPpal INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnPpal INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsPpal, c.Id, t.Es
FROM @ctrlPpal t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPpal
UNION ALL
SELECT @idEnPpal, c.Id, t.En
FROM @ctrlPpal t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPpal;
GO

-- ============================================================
-- FormBitacora — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @form NVARCHAR(80) = N'FormBitacora';
DECLARE @ctrl TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrl (Codigo, Es, En) VALUES
    -- Designer (labels, group, buttons)
    (N'lblTitulo',         N'Bitácora',                                       N'Audit Log'),
    (N'grpFiltros',        N'Filtros',                                        N'Filters'),
    (N'lblTipo',           N'Tipo:',                                          N'Type:'),
    (N'lblUsuarioFiltro',  N'Usuario:',                                       N'User:'),
    (N'lblDesde',          N'Desde:',                                         N'From:'),
    (N'lblHasta',          N'Hasta:',                                         N'To:'),
    (N'btnLimpiar',        N'Limpiar',                                        N'Clear'),
    (N'lblTotal',          N'Total: {0}',                                     N'Total: {0}'),
    (N'btnActualizar',     N'Actualizar',                                     N'Refresh'),
    -- Items y headers dinámicos
    (N'cmbTipoTodos',      N'Todos',                                          N'All'),
    (N'colId',             N'ID',                                             N'ID'),
    (N'colUsuario',        N'Usuario',                                        N'User'),
    (N'colTipo',           N'Tipo',                                           N'Type'),
    (N'colFechaHora',      N'Fecha y Hora',                                   N'Date and Time'),
    (N'colDetalle',        N'Detalle',                                        N'Detail'),
    -- MessageBoxes
    (N'msgSinPermiso',     N'No tenés permiso para acceder a esta sección.',  N'You don''t have permission to access this section.'),
    (N'msgAccesoDenegado', N'Acceso denegado',                                N'Access denied'),
    (N'msgErrorCargar',    N'Error al cargar la bitácora:',                   N'Error loading audit log:'),
    (N'msgError',          N'Error',                                          N'Error');

INSERT INTO Control (Codigo, Form)
SELECT Codigo, @form FROM @ctrl;

DECLARE @idEsBit INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnBit INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsBit, c.Id, t.Es
FROM @ctrl t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @form
UNION ALL
SELECT @idEnBit, c.Id, t.En
FROM @ctrl t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @form;
GO

-- ============================================================
-- FormHome — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formHome NVARCHAR(80) = N'FormHome';
DECLARE @ctrlHome TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlHome (Codigo, Es, En) VALUES
    (N'lblTitulo',          N'Inicio',                                              N'Home'),
    (N'lblNovedadesTitulo', N'Novedades',                                           N'What''s New'),
    (N'lblNovedadesBody',   N'Bienvenido al sistema',                               N'Welcome to the system'),
    (N'lblAccesosTitulo',   N'Accesos rápidos',                                     N'Quick Access'),
    (N'btnAyuda',           N'Ayuda',                                               N'Help'),
    (N'btnContacto',        N'Contactános',                                         N'Contact us'),
    (N'msgProximamente',    N'Próximamente',                                        N'Coming soon'),
    (N'msgAyudaTitulo',     N'Ayuda',                                               N'Help'),
    (N'msgErrorMail',       N'No se pudo abrir el cliente de correo.',              N'Could not open mail client.'),
    (N'msgErrorMailEnvia',  N'Enviá un mail a',                                     N'Send an email to'),
    (N'msgContactoTitulo',  N'Contacto',                                            N'Contact');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formHome FROM @ctrlHome;

DECLARE @idEsHome INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnHome INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsHome, c.Id, t.Es FROM @ctrlHome t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formHome
UNION ALL
SELECT @idEnHome, c.Id, t.En FROM @ctrlHome t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formHome;
GO

-- ============================================================
-- FormUsuarios — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formUsr NVARCHAR(80) = N'FormUsuarios';
DECLARE @ctrlUsr TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlUsr (Codigo, Es, En) VALUES
    (N'lblTitulo',           N'Gestión de Usuarios',                                  N'Manage Users'),
    (N'grpFiltros',          N'Filtros',                                              N'Filters'),
    (N'lblEstado',           N'Estado:',                                              N'Status:'),
    (N'lblUsuarioFiltro',    N'Buscar usuario:',                                      N'Search user:'),
    (N'btnLimpiar',          N'Limpiar',                                              N'Clear'),
    (N'lblHint',             N'← Seleccioná un usuario',                              N'← Select a user'),
    (N'btnEditar',           N'Editar',                                               N'Edit'),
    (N'btnHistorial',        N'Historial',                                            N'History'),
    (N'btnBloquear',         N'Bloquear',                                             N'Block'),
    (N'btnDesbloquear',      N'Desbloquear',                                          N'Unblock'),
    (N'lblTotal',            N'Total: {0}',                                           N'Total: {0}'),
    (N'lblBloqueados',       N'Bloqueados: {0}',                                      N'Blocked: {0}'),
    (N'lblActivos',          N'Activos: {0}',                                         N'Active: {0}'),
    (N'itemTodos',           N'Todos',                                                N'All'),
    (N'itemActivos',         N'Activos',                                              N'Active'),
    (N'itemBloqueados',      N'Bloqueados',                                           N'Blocked'),
    (N'colId',               N'ID',                                                   N'ID'),
    (N'colUsername',         N'Usuario',                                              N'User'),
    (N'colNombre',           N'Nombre',                                               N'First Name'),
    (N'colApellido',         N'Apellido',                                             N'Last Name'),
    (N'colEmail',            N'Email',                                                N'Email'),
    (N'colEstado',           N'Estado',                                               N'Status'),
    (N'colIntentosFallidos', N'Int. Fallidos',                                        N'Failed Att.'),
    (N'colUltimoLogin',      N'Último Login',                                         N'Last Login'),
    (N'valorBloqueado',      N'Bloqueado',                                            N'Blocked'),
    (N'valorActivo',         N'Activo',                                               N'Active'),
    (N'msgSinPermiso',       N'No tenés permiso para acceder a esta sección.',        N'You don''t have permission to access this section.'),
    (N'msgAccesoDenegado',   N'Acceso denegado',                                      N'Access denied'),
    (N'msgErrorCargar',      N'Error al cargar usuarios:',                            N'Error loading users:'),
    (N'msgError',            N'Error',                                                N'Error'),
    (N'msgConfirmarBloqueo', N'¿Bloquear al usuario ''{0}''?',                        N'Block user ''{0}''?'),
    (N'msgConfirmarDesbloqueo', N'¿Desbloquear al usuario ''{0}''?',                  N'Unblock user ''{0}''?'),
    (N'msgConfirmar',        N'Confirmar',                                            N'Confirm');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formUsr FROM @ctrlUsr;

DECLARE @idEsUsr INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnUsr INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsUsr, c.Id, t.Es FROM @ctrlUsr t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUsr
UNION ALL
SELECT @idEnUsr, c.Id, t.En FROM @ctrlUsr t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formUsr;
GO

-- ============================================================
-- FormInsertarUsuario — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formIns NVARCHAR(80) = N'FormInsertarUsuario';
DECLARE @ctrlIns TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlIns (Codigo, Es, En) VALUES
    (N'lblTitulo',            N'Registrar Usuario',                              N'Register User'),
    (N'lblUsername',          N'Usuario:',                                       N'Username:'),
    (N'lblPassword',          N'Contraseña:',                                    N'Password:'),
    (N'lblConfirmarPassword', N'Repetir contraseña:',                            N'Repeat password:'),
    (N'lblNombre',            N'Nombre:',                                        N'First Name:'),
    (N'lblApellido',          N'Apellido:',                                      N'Last Name:'),
    (N'lblEmail',             N'Email:',                                         N'Email:'),
    (N'lblTelefono',          N'Teléfono:',                                      N'Phone:'),
    (N'lblDocumento',         N'Documento:',                                     N'ID Document:'),
    (N'lblDomicilio',         N'Domicilio:',                                     N'Address:'),
    (N'btnRegistrar',         N'Registrar',                                      N'Register'),
    (N'msgSinPermiso',        N'No tenés permiso para acceder a esta sección.',  N'You don''t have permission to access this section.'),
    (N'msgAccesoDenegado',    N'Acceso denegado',                                N'Access denied'),
    (N'msgCamposVacios',      N'Completá todos los campos.',                     N'Please fill in all fields.'),
    (N'msgAdvertencia',       N'Advertencia',                                    N'Warning'),
    (N'msgPasswordsDistintos',N'Las contraseñas no coinciden.',                  N'Passwords do not match.'),
    (N'msgUsuarioRegistrado', N'Usuario registrado correctamente.',              N'User registered successfully.'),
    (N'msgExito',             N'Éxito',                                          N'Success'),
    (N'msgError',             N'Error',                                          N'Error');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formIns FROM @ctrlIns;

DECLARE @idEsIns INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnIns INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsIns, c.Id, t.Es FROM @ctrlIns t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formIns
UNION ALL
SELECT @idEnIns, c.Id, t.En FROM @ctrlIns t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formIns;
GO

-- ============================================================
-- FormEditarUsuario — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formEdt NVARCHAR(80) = N'FormEditarUsuario';
DECLARE @ctrlEdt TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlEdt (Codigo, Es, En) VALUES
    (N'title',                 N'Editar Usuario',                                 N'Edit User'),
    (N'lblTitulo',             N'Editar Usuario',                                 N'Edit User'),
    (N'lblUsername',           N'Usuario:',                                       N'Username:'),
    (N'lblNombre',             N'Nombre:',                                        N'First Name:'),
    (N'lblApellido',           N'Apellido:',                                      N'Last Name:'),
    (N'lblEmail',              N'Email:',                                         N'Email:'),
    (N'lblTelefono',           N'Teléfono:',                                      N'Phone:'),
    (N'lblDocumento',          N'Documento:',                                     N'ID Document:'),
    (N'lblDomicilio',          N'Domicilio:',                                     N'Address:'),
    (N'lblIdioma',             N'Idioma preferido:',                              N'Preferred language:'),
    (N'idiomaSinSeleccion',    N'— (sin seleccionar)',                            N'— (not set)'),
    (N'btnGuardar',            N'Guardar cambios',                                N'Save changes'),
    (N'msgSinPermiso',         N'No tenés permiso para editar usuarios.',         N'You don''t have permission to edit users.'),
    (N'msgAccesoDenegado',     N'Acceso denegado',                                N'Access denied'),
    (N'msgUsuarioNoEncontrado',N'No se encontró el usuario.',                     N'User not found.'),
    (N'msgError',              N'Error',                                          N'Error'),
    (N'msgCamposVacios',       N'Completá todos los campos.',                     N'Please fill in all fields.'),
    (N'msgAdvertencia',        N'Advertencia',                                    N'Warning'),
    (N'msgUsuarioEditado',     N'Usuario editado correctamente.',                 N'User edited successfully.'),
    (N'msgSinCambios',         N'No hay cambios para guardar.',                   N'No changes to save.'),
    (N'msgInformacion',        N'Información',                                    N'Information'),
    (N'msgExito',              N'Éxito',                                          N'Success');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formEdt FROM @ctrlEdt;

DECLARE @idEsEdt INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnEdt INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsEdt, c.Id, t.Es FROM @ctrlEdt t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEdt
UNION ALL
SELECT @idEnEdt, c.Id, t.En FROM @ctrlEdt t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formEdt;
GO

-- ============================================================
-- FormHistorialUsuario — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formHis NVARCHAR(80) = N'FormHistorialUsuario';
DECLARE @ctrlHis TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlHis (Codigo, Es, En) VALUES
    (N'title',                    N'Historial de Usuario',                                          N'User History'),
    (N'lblTitulo',                N'Historial de Usuario',                                          N'User History'),
    (N'lblTituloDe',              N'Historial de: {0}',                                             N'History of: {0}'),
    (N'btnRestaurar',             N'Restaurar a esta versión',                                      N'Restore to this version'),
    (N'btnCerrar',                N'Cerrar',                                                        N'Close'),
    (N'leyendaActual',            N'● Versión actual',                                              N'● Current version'),
    (N'colFechaHora',             N'Fecha y Hora',                                                  N'Date and Time'),
    (N'colAccion',                N'Acción',                                                        N'Action'),
    (N'colModificadoPor',         N'Modificado por',                                                N'Modified by'),
    (N'colNombre',                N'Nombre',                                                        N'First Name'),
    (N'colApellido',              N'Apellido',                                                      N'Last Name'),
    (N'colEmail',                 N'Email',                                                         N'Email'),
    (N'colTelefono',              N'Teléfono',                                                      N'Phone'),
    (N'colDocumento',             N'Documento',                                                     N'ID Document'),
    (N'colDomicilio',             N'Domicilio',                                                     N'Address'),
    (N'colBloqueado',             N'Bloqueado',                                                     N'Blocked'),
    (N'colRestauracion',          N'Restaurado desde',                                              N'Restored from'),
    (N'valorSi',                  N'Sí',                                                            N'Yes'),
    (N'valorNo',                  N'No',                                                            N'No'),
    (N'valorSistema',             N'(sistema)',                                                     N'(system)'),
    (N'msgSinPermiso',            N'No tenés permiso para ver el historial de usuarios.',           N'You don''t have permission to view user history.'),
    (N'msgAccesoDenegado',        N'Acceso denegado',                                               N'Access denied'),
    (N'msgUsuarioNoEncontrado',   N'No se encontró el usuario.',                                    N'User not found.'),
    (N'msgError',                 N'Error',                                                         N'Error'),
    (N'msgErrorCargar',           N'Error al cargar el historial:',                                 N'Error loading history:'),
    (N'msgSelectVersion',         N'Seleccioná una versión a restaurar.',                           N'Select a version to restore.'),
    (N'msgValidacion',            N'Validación',                                                    N'Validation'),
    (N'msgConfirmarRestaurar',    N'¿Restaurar el usuario al estado del {0}?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Se sobrescribirán los datos actuales con los de esa versión. Quedará registrado como una nueva entrada en el historial.', N'Restore user to state of {0}?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Current data will be overwritten with that version''s data. A new entry will be recorded in the history.'),
    (N'msgConfirmarRestaurarTitulo', N'Confirmar restauración',                                     N'Confirm restore'),
    (N'msgRestauradoExito',       N'Usuario restaurado correctamente.',                             N'User restored successfully.'),
    (N'msgExito',                 N'Éxito',                                                         N'Success'),
    (N'msgSinPermisoRestaurar',   N'Para restaurar necesitás permiso de edición de usuarios.',      N'To restore you need user-edit permission.');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formHis FROM @ctrlHis;

DECLARE @idEsHis INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnHis INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsHis, c.Id, t.Es FROM @ctrlHis t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formHis
UNION ALL
SELECT @idEnHis, c.Id, t.En FROM @ctrlHis t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formHis;
GO

-- ============================================================
-- Enum AccionUsuarioHistorial — Traducciones ES/EN
-- ============================================================
DECLARE @formAccUH NVARCHAR(80) = N'AccionUsuarioHistorial';
DECLARE @ctrlAccUH TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlAccUH (Codigo, Es, En) VALUES
    (N'Alta',          N'Alta',          N'Created'),
    (N'Edicion',       N'Edición',       N'Edited'),
    (N'Bloqueo',       N'Bloqueo',       N'Blocked'),
    (N'Desbloqueo',    N'Desbloqueo',    N'Unblocked'),
    (N'Restauracion',  N'Restauración',  N'Restored');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formAccUH FROM @ctrlAccUH;

DECLARE @idEsAccUH INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnAccUH INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsAccUH, c.Id, t.Es FROM @ctrlAccUH t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formAccUH
UNION ALL
SELECT @idEnAccUH, c.Id, t.En FROM @ctrlAccUH t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formAccUH;
GO

-- ============================================================
-- FormPermisos — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formPer NVARCHAR(80) = N'FormPermisos';
DECLARE @ctrlPer TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlPer (Codigo, Es, En) VALUES
    (N'lblTitulo',              N'Gestión de Permisos',                                                N'Manage Permissions'),
    (N'lblCol1',                N'Roles',                                                              N'Roles'),
    (N'lblCol2',                N'Contenido',                                                          N'Contents'),
    (N'lblCol2De',              N'Contenido de: {0}',                                                  N'Contents of: {0}'),
    (N'lblCol3',                N'Roles y Permisos disponibles',                                       N'Available Roles and Permissions'),
    (N'btnNuevoRol',            N'Nuevo Rol',                                                          N'New Role'),
    (N'btnEliminarRol',         N'Borrar Rol',                                                         N'Delete Role'),
    (N'btnQuitar',              N'Sacar de la lista →',                                                N'Remove from list →'),
    (N'btnAgregar',             N'← Agregar a la lista',                                               N'← Add to list'),
    (N'msgSinPermiso',          N'No tenés permiso para gestionar permisos.',                          N'You don''t have permission to manage permissions.'),
    (N'msgAccesoDenegado',      N'Acceso denegado',                                                    N'Access denied'),
    (N'msgErrorCargarRoles',    N'Error al cargar roles:',                                             N'Error loading roles:'),
    (N'msgError',               N'Error',                                                              N'Error'),
    (N'msgErrorPrefix',         N'Error: {0}',                                                         N'Error: {0}'),
    (N'promptCodigoRol',        N'Código del rol:',                                                    N'Role code:'),
    (N'promptDescripcionRol',   N'Descripción (opcional):',                                            N'Description (optional):'),
    (N'msgSelectRol',           N'Seleccioná un rol a la izquierda.',                                  N'Select a role on the left.'),
    (N'msgValidacion',          N'Validación',                                                         N'Validation'),
    (N'msgConfirmarBorrarRol',  N'¿Borrar el rol ''{0}''?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Esta acción es irreversible. El rol se desasignará automáticamente de todos los usuarios que lo tengan asignado. Los permisos hijos NO se borran, sólo quedan sin padre.', N'Delete role ''{0}''?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'This action is irreversible. The role will be automatically unassigned from all users. Child permissions are NOT deleted, they just lose their parent.'),
    (N'msgConfirmarEliminacion',N'Confirmar eliminación',                                              N'Confirm deletion'),
    (N'msgSelectPermisos',      N'Seleccioná uno o más permisos de la lista de la derecha (Ctrl+click o Shift+click para varios).', N'Select one or more permissions from the right list (Ctrl+click or Shift+click for multiple).'),
    (N'msgFallaProc',           N'Se procesaron {0} de {1}. Falló en ''{2}'': {3}',                    N'Processed {0} of {1}. Failed at ''{2}'': {3}'),
    (N'msgOperacionInterrumpida',N'Operación interrumpida',                                            N'Operation interrupted'),
    (N'msgLimpiezaAuto',        N'Se agregaron {0}. Se quitaron {1} asignación(es) directa(s) cubiertas por este rol.', N'{0} added. {1} direct assignment(s) covered by this role were removed.'),
    (N'msgLimpiezaAutoTitulo',  N'Limpieza automática',                                                N'Auto cleanup'),
    (N'msgSelectHijos',         N'Seleccioná uno o más hijos a quitar (Ctrl+click o Shift+click para varios).', N'Select one or more children to remove (Ctrl+click or Shift+click for multiple).'),
    (N'promptIngresarTitulo',   N'Ingresar',                                                           N'Enter'),
    (N'btnAceptar',             N'Aceptar',                                                            N'Accept'),
    (N'btnCancelar',            N'Cancelar',                                                           N'Cancel');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formPer FROM @ctrlPer;

DECLARE @idEsPer INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnPer INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsPer, c.Id, t.Es FROM @ctrlPer t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPer
UNION ALL
SELECT @idEnPer, c.Id, t.En FROM @ctrlPer t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formPer;
GO

-- ============================================================
-- FormAsignacionPermisos — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formAsig NVARCHAR(80) = N'FormAsignacionPermisos';
DECLARE @ctrlAsig TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlAsig (Codigo, Es, En) VALUES
    (N'lblTitulo',              N'Asignación de Permisos a Usuarios',                                  N'Assign Permissions to Users'),
    (N'lblCol1',                N'Usuarios',                                                           N'Users'),
    (N'lblCol2',                N'Asignaciones del usuario',                                           N'User Assignments'),
    (N'lblCol2De',              N'Asignaciones de: {0}',                                               N'Assignments of: {0}'),
    (N'lblCol3',                N'Roles y Permisos disponibles',                                       N'Available Roles and Permissions'),
    (N'btnQuitar',              N'Quitar del usuario →',                                               N'Remove from user →'),
    (N'btnAsignar',             N'← Asignar al usuario',                                               N'← Assign to user'),
    (N'msgSinPermiso',          N'No tenés permiso para asignar permisos a usuarios.',                 N'You don''t have permission to assign permissions to users.'),
    (N'msgAccesoDenegado',      N'Acceso denegado',                                                    N'Access denied'),
    (N'msgErrorCargar',         N'Error al cargar usuarios:',                                          N'Error loading users:'),
    (N'msgError',               N'Error',                                                              N'Error'),
    (N'msgValidacion',          N'Validación',                                                         N'Validation'),
    (N'msgSelectUsuario',       N'Seleccioná un usuario a la izquierda.',                              N'Select a user on the left.'),
    (N'msgSelectPermisos',      N'Seleccioná uno o más permisos de la lista de la derecha (Ctrl+click o Shift+click para varios).', N'Select one or more permissions from the right list (Ctrl+click or Shift+click for multiple).'),
    (N'msgSelectAsignaciones',  N'Seleccioná una o más asignaciones a quitar (Ctrl+click o Shift+click para varias).',              N'Select one or more assignments to remove (Ctrl+click or Shift+click for multiple).'),
    (N'msgFallaProc',           N'Se procesaron {0} de {1}. Falló en ''{2}'': {3}',                    N'Processed {0} of {1}. Failed at ''{2}'': {3}'),
    (N'msgOperacionInterrumpida',N'Operación interrumpida',                                            N'Operation interrupted'),
    (N'msgLimpiezaAuto',        N'Se asignaron {0}. Se quitaron {1} asignación(es) directa(s) redundante(s) porque quedaron cubiertas.', N'{0} assigned. {1} redundant direct assignment(s) were removed because they became covered.'),
    (N'msgLimpiezaAutoTitulo',  N'Limpieza automática',                                                N'Auto cleanup');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formAsig FROM @ctrlAsig;

DECLARE @idEsAsig INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnAsig INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsAsig, c.Id, t.Es FROM @ctrlAsig t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formAsig
UNION ALL
SELECT @idEnAsig, c.Id, t.En FROM @ctrlAsig t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formAsig;
GO

-- ============================================================
-- FormIdiomas — Controles + Traducciones ES/EN
-- ============================================================
DECLARE @formIdi NVARCHAR(80) = N'FormIdiomas';
DECLARE @ctrlIdi TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlIdi (Codigo, Es, En) VALUES
    (N'lblTitulo',                 N'Gestión de Idiomas',                                                   N'Manage Languages'),
    (N'lblIdiomas',                N'Idiomas disponibles',                                                  N'Available Languages'),
    (N'lblTraducciones',           N'Traducciones',                                                         N'Translations'),
    (N'lblTraduccionesDe',         N'Traducciones — {0}',                                                   N'Translations — {0}'),
    (N'lblAlerta',                 N'⚠  Idioma en proceso de creación — completá todas las traducciones para poder activarlo.', N'⚠  Language in progress — complete all translations to activate it.'),
    (N'btnNuevoIdioma',            N'Nuevo Idioma',                                                         N'New Language'),
    (N'btnEliminarIdioma',         N'Eliminar Idioma',                                                      N'Delete Language'),
    (N'btnGuardar',                N'Guardar cambios',                                                      N'Save changes'),
    (N'colForm',                   N'Form',                                                                 N'Form'),
    (N'colCodigo',                 N'Código',                                                               N'Code'),
    (N'colTexto',                  N'Texto',                                                                N'Text'),
    (N'msgSinPermiso',             N'No tenés permiso para gestionar idiomas.',                             N'You don''t have permission to manage languages.'),
    (N'msgAccesoDenegado',         N'Acceso denegado',                                                      N'Access denied'),
    (N'msgErrorCargarIdiomas',     N'Error al cargar idiomas:',                                             N'Error loading languages:'),
    (N'msgErrorCargarTraducciones',N'Error al cargar traducciones:',                                        N'Error loading translations:'),
    (N'msgError',                  N'Error',                                                                N'Error'),
    (N'promptNombreIdioma',        N'Nombre del idioma:',                                                   N'Language name:'),
    (N'msgErrorPrefix',            N'Error: {0}',                                                           N'Error: {0}'),
    (N'msgSelectIdioma',           N'Seleccioná un idioma de la lista.',                                    N'Select a language from the list.'),
    (N'msgValidacion',             N'Validación',                                                           N'Validation'),
    (N'msgConfirmarBorrarIdioma',  N'¿Borrar el idioma ''{0}''?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Esta acción es irreversible y eliminará todas las traducciones asociadas.', N'Delete language ''{0}''?' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'This action is irreversible and will delete all associated translations.'),
    (N'msgConfirmarEliminacion',   N'Confirmar eliminación',                                                N'Confirm deletion'),
    (N'msgEspanolCompleto',        N'El idioma ''Español'' debe estar siempre completo: es el idioma base del sistema.' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Completá todas las filas antes de guardar.', N'The ''Español'' language must always be complete: it is the system base language.' + NCHAR(13) + NCHAR(10) + NCHAR(13) + NCHAR(10) + N'Complete all rows before saving.'),
    (N'msgNoSePuedeGuardar',       N'No se puede guardar',                                                  N'Cannot save'),
    (N'msgSinCambios',             N'No hay cambios para guardar.',                                         N'No changes to save.'),
    (N'msgInformacion',            N'Información',                                                          N'Information'),
    (N'msgGuardadoExito',          N'Se guardaron {0} traducción(es).',                                     N'Saved {0} translation(s).'),
    (N'msgCambiosGuardados',       N'Cambios guardados',                                                    N'Changes saved'),
    (N'msgErrorGuardar',           N'Error al guardar:',                                                    N'Error saving:'),
    (N'promptIngresarTitulo',      N'Ingresar',                                                             N'Enter'),
    (N'btnAceptar',                N'Aceptar',                                                              N'Accept'),
    (N'btnCancelar',               N'Cancelar',                                                             N'Cancel');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formIdi FROM @ctrlIdi;

DECLARE @idEsIdi INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnIdi INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsIdi, c.Id, t.Es FROM @ctrlIdi t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formIdi
UNION ALL
SELECT @idEnIdi, c.Id, t.En FROM @ctrlIdi t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formIdi;
GO

-- ============================================================
-- Errores BLL — Codigos + Traducciones ES/EN
-- ============================================================
DECLARE @formErr NVARCHAR(80) = N'Errores';
DECLARE @ctrlErr TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlErr (Codigo, Es, En) VALUES
    -- UsuarioService
    (N'ERR_LOGIN_GENERICO',             N'Ocurrió un error en Login.',                                             N'A login error occurred.'),
    (N'ERR_LOGOUT_GENERICO',            N'Ocurrió un error en Logout.',                                            N'A logout error occurred.'),
    (N'ERR_SIN_PERMISO_DESBLOQUEAR',    N'No tiene permisos para desbloquear usuarios.',                           N'You don''t have permission to unblock users.'),
    (N'ERR_NO_BLOQUEAR_ADMIN',          N'No podés bloquear al usuario administrador.',                            N'You cannot block the administrator user.'),
    (N'ERR_SIN_PERMISO_BLOQUEAR',       N'No tiene permisos para bloquear usuarios.',                              N'You don''t have permission to block users.'),
    (N'ERR_REGISTRO_DATOS_INCOMPLETOS', N'Faltan datos para registrar al usuario.',                                N'Missing data to register the user.'),
    (N'ERR_EDICION_DATOS_INCOMPLETOS',  N'Faltan datos para editar al usuario.',                                   N'Missing data to edit the user.'),
    (N'ERR_USERNAME_EXISTE',            N'Ya existe el username.',                                                 N'Username already exists.'),
    (N'ERR_USERNAME_EN_USO',            N'El username ''{0}'' ya está en uso por otro usuario.',                   N'Username ''{0}'' is already in use by another user.'),
    (N'ERR_SIN_PERMISO_EDITAR_USUARIO', N'No tenés permiso para editar usuarios.',                                 N'You don''t have permission to edit users.'),
    (N'ERR_SIN_PERMISO_HISTORIAL',      N'No tenés permiso para ver el historial de usuarios.',                    N'You don''t have permission to view user history.'),
    (N'ERR_HISTORIAL_NO_EXISTE',        N'La versión seleccionada no existe.',                                     N'The selected version does not exist.'),
    (N'ERR_USUARIO_NO_ENCONTRADO',      N'No se encontró el usuario.',                                             N'User not found.'),
    (N'ERR_SIN_PERMISO_ACCION',         N'No tenés permiso para ejecutar esta acción ({0}).',                      N'You don''t have permission to perform this action ({0}).'),
    (N'ERR_AUTOASIGNACION_PERMISOS',    N'No podés asignarte o quitarte permisos a vos mismo. Sólo el administrador puede hacerlo.', N'You cannot assign or revoke your own permissions. Only the administrator can do that.'),
    -- PermisoService
    (N'ERR_CODIGO_OBLIGATORIO',         N'Código es obligatorio.',                                                 N'Code is required.'),
    (N'ERR_PERMISO_CODIGO_DUPLICADO',   N'Ya existe un permiso con el código ''{0}''. Los códigos deben ser únicos.', N'A permission with code ''{0}'' already exists. Codes must be unique.'),
    (N'ERR_PERMISO_AUTOCONTENCION',     N'Un permiso no puede contenerse a sí mismo.',                             N'A permission cannot contain itself.'),
    (N'ERR_PERMISO_CICLICO',            N'La asignación generaría una referencia circular entre permisos.',        N'The assignment would create a circular reference between permissions.'),
    (N'ERR_ROL_NO_EXISTE',              N'El rol no existe.',                                                      N'The role does not exist.'),
    (N'ERR_SOLO_ADMIN_PERMISOS',        N'Sólo el usuario administrador puede gestionar permisos.',                N'Only the administrator can manage permissions.'),
    -- IdiomaService
    (N'ERR_IDIOMA_NOMBRE_OBLIGATORIO',  N'El nombre del idioma es obligatorio.',                                   N'Language name is required.'),
    (N'ERR_IDIOMA_NOMBRE_DUPLICADO',    N'Ya existe un idioma con el nombre ''{0}''. Los nombres deben ser únicos.', N'A language named ''{0}'' already exists. Names must be unique.'),
    (N'ERR_IDIOMA_NO_EXISTE',           N'El idioma no existe.',                                                   N'The language does not exist.'),
    (N'ERR_IDIOMA_PROTEGIDO_BORRAR',    N'El idioma ''{0}'' no puede eliminarse: es el idioma base del sistema.',  N'Language ''{0}'' cannot be deleted: it is the system base language.'),
    (N'ERR_IDIOMA_INCOMPLETO',          N'El idioma ''{0}'' está en proceso de creación. Completá todas las traducciones antes de activarlo.', N'Language ''{0}'' is in progress. Complete all translations before activating it.'),
    (N'ERR_IDIOMA_PROTEGIDO_VACIO',     N'El idioma ''{0}'' no admite traducciones vacías: es el idioma base del sistema y debe estar siempre completo.', N'Language ''{0}'' does not allow empty translations: it is the system base language and must always be complete.');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formErr FROM @ctrlErr;

DECLARE @idEsErr INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnErr INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsErr, c.Id, t.Es FROM @ctrlErr t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formErr
UNION ALL
SELECT @idEnErr, c.Id, t.En FROM @ctrlErr t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formErr;
GO

-- ============================================================
-- Enum TipoBitacora — Traducciones ES/EN
-- ============================================================
DECLARE @formTBit NVARCHAR(80) = N'TipoBitacora';
DECLARE @ctrlTBit TABLE (Codigo NVARCHAR(80), Es NVARCHAR(1000), En NVARCHAR(1000));

INSERT INTO @ctrlTBit (Codigo, Es, En) VALUES
    (N'Login',                  N'Inicio de sesión',         N'Login'),
    (N'Logout',                 N'Cierre de sesión',         N'Logout'),
    (N'RegistroUsuario',        N'Registro de usuario',      N'User registration'),
    (N'DesbloqueoUsuario',      N'Desbloqueo de usuario',    N'User unblock'),
    (N'BloqueoUsuario',         N'Bloqueo de usuario',       N'User block'),
    (N'Error',                  N'Error',                    N'Error'),
    (N'LoginFallido',           N'Inicio de sesión fallido', N'Failed login'),
    (N'IntentoAccesoBloqueado', N'Intento acceso bloqueado', N'Blocked access attempt'),
    (N'AltaPermiso',            N'Alta de permiso',          N'Permission created'),
    (N'AsignacionPermiso',      N'Asignación de permiso',    N'Permission assignment'),
    (N'AltaRol',                N'Alta de rol',              N'Role created'),
    (N'AsignacionRol',          N'Asignación de rol',        N'Role assignment'),
    (N'AltaIdioma',             N'Alta de idioma',           N'Language created'),
    (N'EdicionUsuario',         N'Edición de usuario',       N'User edited');

INSERT INTO Control (Codigo, Form) SELECT Codigo, @formTBit FROM @ctrlTBit;

DECLARE @idEsTBit INT = (SELECT Id FROM Idioma WHERE Nombre = N'Español');
DECLARE @idEnTBit INT = (SELECT Id FROM Idioma WHERE Nombre = N'English');

INSERT INTO Traduccion (IdIdioma, IdControl, Texto)
SELECT @idEsTBit, c.Id, t.Es FROM @ctrlTBit t
INNER JOIN Control c ON c.Codigo = t.Codigo AND c.Form = @formTBit
UNION ALL
SELECT @idEnTBit, c.Id, t.En FROM @ctrlTBit t
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