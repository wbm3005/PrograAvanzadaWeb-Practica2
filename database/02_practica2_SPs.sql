USE [Practica2]
GO


/* ---------------------------------------------------------------------
   1) Registrar cliente
   Restricciones: campos obligatorios, cedula no repetida.
   Estado se guarda en 1 (activo) porque el formulario no lo pide.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE [dbo].[usp_Clientes_Registrar]
    @Cedula VARCHAR(50),
    @Nombre VARCHAR(100),
    @Correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @Cedula = LTRIM(RTRIM(ISNULL(@Cedula, '')));
    SET @Nombre = LTRIM(RTRIM(ISNULL(@Nombre, '')));
    SET @Correo = LTRIM(RTRIM(ISNULL(@Correo, '')));

    IF @Cedula = '' OR @Nombre = '' OR @Correo = ''
    BEGIN
        SELECT CAST(1 AS INT) AS Codigo,
               CAST('Todos los campos son obligatorios.' AS VARCHAR(200)) AS Mensaje,
               CAST(NULL AS BIGINT) AS Id;
        RETURN;
    END

    BEGIN TRY
        BEGIN TRAN;

        -- UPDLOCK + HOLDLOCK: evita que dos registros simultaneos con la
        -- misma cedula pasen la validacion al mismo tiempo.
        IF EXISTS (SELECT 1
                   FROM [dbo].[Clientes] WITH (UPDLOCK, HOLDLOCK)
                   WHERE Cedula = @Cedula)
        BEGIN
            -- Nada se escribio: COMMIT (y no ROLLBACK) para no destruir una
            -- transaccion externa si quien llama ya tenia una abierta.
            COMMIT TRAN;
            SELECT CAST(2 AS INT) AS Codigo,
                   CAST('La cédula ya está registrada.' AS VARCHAR(200)) AS Mensaje,
                   CAST(NULL AS BIGINT) AS Id;
            RETURN;
        END

        INSERT INTO [dbo].[Clientes] (Cedula, Nombre, Correo, Estado)
        VALUES (@Cedula, @Nombre, @Correo, 1);

        DECLARE @IdCliente BIGINT = SCOPE_IDENTITY();

        COMMIT TRAN;

        SELECT CAST(0 AS INT) AS Codigo,
               CAST('Cliente registrado correctamente.' AS VARCHAR(200)) AS Mensaje,
               @IdCliente AS Id;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO


/* ---------------------------------------------------------------------
   2) Listar clientes activos
   Alimenta la lista desplegable del formulario de mascotas.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE [dbo].[usp_Clientes_Listar]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdCliente,
           Cedula,
           Nombre
    FROM [dbo].[Clientes]
    WHERE Estado = 1
    ORDER BY Nombre, Cedula;
END
GO


/* ---------------------------------------------------------------------
   3) Registrar mascota
   Restricciones: campos obligatorios, peso mayor a 0, cliente existente
   y maximo 2 mascotas de la misma especie por cliente.
   La especie se compara sin distinguir mayusculas ni tildes
   ("Perro" = "perro" = "PERRO", "Pájaro" = "Pajaro").
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE [dbo].[usp_Mascotas_Registrar]
    @Nombre    VARCHAR(100),
    @Especie   VARCHAR(100),
    @Raza      VARCHAR(100),
    @Peso      DECIMAL(8, 2),
    @IdCliente BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @Nombre  = LTRIM(RTRIM(ISNULL(@Nombre, '')));
    SET @Especie = LTRIM(RTRIM(ISNULL(@Especie, '')));
    SET @Raza    = LTRIM(RTRIM(ISNULL(@Raza, '')));

    IF @Nombre = '' OR @Especie = '' OR @Raza = ''
       OR @Peso IS NULL OR @IdCliente IS NULL
    BEGIN
        SELECT CAST(1 AS INT) AS Codigo,
               CAST('Todos los campos son obligatorios.' AS VARCHAR(200)) AS Mensaje,
               CAST(NULL AS BIGINT) AS Id;
        RETURN;
    END

    IF @Peso <= 0
    BEGIN
        SELECT CAST(1 AS INT) AS Codigo,
               CAST('El peso debe ser mayor a cero.' AS VARCHAR(200)) AS Mensaje,
               CAST(NULL AS BIGINT) AS Id;
        RETURN;
    END

    BEGIN TRY
        BEGIN TRAN;

        IF NOT EXISTS (SELECT 1
                       FROM [dbo].[Clientes]
                       WHERE IdCliente = @IdCliente
                         AND Estado = 1)
        BEGIN
            COMMIT TRAN;   -- nada se escribio (ver nota en usp_Clientes_Registrar)
            SELECT CAST(3 AS INT) AS Codigo,
                   CAST('El cliente seleccionado no existe.' AS VARCHAR(200)) AS Mensaje,
                   CAST(NULL AS BIGINT) AS Id;
            RETURN;
        END

        -- Mismo bloqueo que en clientes: dos registros simultaneos no
        -- pueden sobrepasar el limite de 2 por especie.
        IF (SELECT COUNT(*)
            FROM [dbo].[Mascotas] WITH (UPDLOCK, HOLDLOCK)
            WHERE IdCliente = @IdCliente
              AND Especie COLLATE Latin1_General_CI_AI = @Especie COLLATE Latin1_General_CI_AI) >= 2
        BEGIN
            COMMIT TRAN;   -- nada se escribio (ver nota en usp_Clientes_Registrar)
            SELECT CAST(4 AS INT) AS Codigo,
                   CAST('El cliente ya tiene 2 mascotas de esta especie.' AS VARCHAR(200)) AS Mensaje,
                   CAST(NULL AS BIGINT) AS Id;
            RETURN;
        END

        INSERT INTO [dbo].[Mascotas] (Nombre, Especie, Raza, Peso, IdCliente)
        VALUES (@Nombre, @Especie, @Raza, @Peso, @IdCliente);

        DECLARE @IdMascota BIGINT = SCOPE_IDENTITY();

        COMMIT TRAN;

        SELECT CAST(0 AS INT) AS Codigo,
               CAST('Mascota registrada correctamente.' AS VARCHAR(200)) AS Mensaje,
               @IdMascota AS Id;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO


/* ---------------------------------------------------------------------
   4) Consultar mascotas
   Columnas exactas que pide el enunciado:
   Cedula del cliente, Nombre del cliente, Nombre de la mascota,
   Especie y Peso.
   --------------------------------------------------------------------- */
CREATE OR ALTER PROCEDURE [dbo].[usp_Mascotas_Consultar]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT c.Cedula  AS CedulaCliente,
           c.Nombre  AS NombreCliente,
           m.Nombre  AS NombreMascota,
           m.Especie AS Especie,
           m.Peso    AS Peso
    FROM [dbo].[Mascotas] AS m
    INNER JOIN [dbo].[Clientes] AS c
            ON c.IdCliente = m.IdCliente
    ORDER BY c.Nombre, m.Nombre;
END
GO
