USE [ingenieria]
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuario' AND xtype='U')
CREATE TABLE [dbo].[Usuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Hash] [nvarchar](255) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Apellido] [nvarchar](50) NOT NULL,
	[Salt] [nvarchar](50) NOT NULL DEFAULT ''
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BitacoraSesion' AND xtype='U')
CREATE TABLE [dbo].[BitacoraSesion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[FechaHoraInicio] [datetime] NOT NULL DEFAULT GETDATE(),
	[FechaHoraFin] [datetime] NULL,
	CONSTRAINT [PK_BitacoraSesion] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_BitacoraSesion_Usuario] FOREIGN KEY ([UsuarioId])
		REFERENCES [dbo].[Usuario] ([Id])
) ON [PRIMARY]
GO


CREATE OR ALTER PROCEDURE [dbo].[Login]
    @Username VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Username, [Hash], [Salt], Nombre, Apellido
    FROM Usuario
    WHERE Username = @username;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarBitacoraSesion]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

	DELETE FROM BitacoraSesion WHERE UsuarioId = @UsuarioId
      AND FechaHoraFin IS NULL;

    INSERT INTO BitacoraSesion (UsuarioId, FechaHoraInicio)
    VALUES (@UsuarioId, GETDATE());

    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[EditarBitacoraSesion]
    @BitacoraSesionId INT,
	@FechaHoraFin DATETIME = NULL
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE BitacoraSesion SET FechaHoraFin = @FechaHoraFin WHERE Id = @BitacoraSesionId
END
GO

CREATE OR ALTER PROCEDURE [dbo].[InsertarUsuario]
    @Username NVARCHAR(50),
    @Hash NVARCHAR(255),
    @Salt NVARCHAR(50),
    @Nombre NVARCHAR(50),
    @Apellido NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Usuario ([Username], [Hash], [Salt], [Nombre], [Apellido])
    VALUES (@Username, @Hash, @Salt, @Nombre, @Apellido);

    SELECT SCOPE_IDENTITY() AS Id;
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

DELETE Usuario
DBCC CHECKIDENT ('Usuario', RESEED, 0)
IF NOT EXISTS (SELECT * FROM Usuario WHERE Username = 'admin')
    INSERT INTO Usuario (Username, Hash, Salt, Nombre, Apellido)
    VALUES (
        'admin',
        '6e2cdcd54b07b8de670b1583026a554abd84bb7a7fa99b92f85244205cdbeff9',
        'VQUk8CQ1S2V3oSbMWAp4qg==',
        'Admin',
        'Admin'
    )
GO
