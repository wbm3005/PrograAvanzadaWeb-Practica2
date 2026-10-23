# Práctica #2 – Registro de Mascotas

Programación Avanzada Web · Ingeniería en Sistemas de Computación · Universidad Fidélitas

Solución con dos proyectos ASP.NET Core (.NET 10):

| Proyecto | Rol |
|---|---|
| `Practica2.Api` | API REST (controladores MVC) que accede a SQL Server con **Dapper y procedimientos almacenados**. |
| `Practica2.Web` | Aplicación MVC con la plantilla Spark que consume el API mediante `HttpClient` tipado. |

## Requisitos

- .NET SDK 10 (la solución usa el formato `Practica2.Web.slnx`).
- SQL Server local con la base `Practica2`, las tablas de `database/01_schema_original.sql` y los procedimientos de `database/02_practica2_SPs.sql` ya aplicados.
- Certificado HTTPS de desarrollo de confianza: `dotnet dev-certs https --trust`.

## Configuración

`Practica2.Api/appsettings.json` (autenticación de Windows, sin usuario ni contraseña):

```json
"ConnectionStrings": {
  "Practica2": "Server=localhost;Database=Practica2;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

`Practica2.Web/appsettings.json` apunta al perfil `https` del API:

```json
"Api": { "BaseUrl": "https://localhost:7147/" }
```

## Ejecución

**Visual Studio:** clic derecho en la solución → *Configurar proyectos de inicio* → *Varios proyectos de inicio* → `Practica2.Api` y `Practica2.Web` en *Iniciar* (el API primero), ambos con el perfil `https`.

**Línea de comandos** (dos terminales, primero el API):

```powershell
dotnet run --project Practica2.Api --launch-profile https   # https://localhost:7147
dotnet run --project Practica2.Web --launch-profile https   # https://localhost:7155
```

## Endpoints del API

| Método y ruta | Respuesta |
|---|---|
| `GET api/clientes` | `[{ idCliente, cedula, nombre }]` (solo clientes activos) |
| `POST api/clientes` `{ cedula, nombre, correo }` | 201 `{ id, mensaje }` · 400 · 409 cédula repetida |
| `GET api/mascotas` | `[{ cedulaCliente, nombreCliente, nombreMascota, especie, peso }]` |
| `POST api/mascotas` `{ nombre, especie, raza, peso, idCliente }` | 201 `{ id, mensaje }` · 400 datos inválidos o cliente inexistente · 409 límite de 2 por especie |

Los errores salen como `ProblemDetails`; en los errores de negocio `detail` es el mensaje exacto del procedimiento almacenado.

## Requisitos del enunciado y dónde se cumplen

| Requisito | Dónde |
|---|---|
| Tablas Clientes y Mascotas sin modificar | `database/01_schema_original.sql` (solo referencia; no se alteró la BD) |
| Proyecto Web MVC (.NET Core) | `Practica2.Web` (`Controllers/`, `Models/`, `Views/`, `Services/`) |
| Proyecto API MVC (.NET Core) | `Practica2.Api` (`Controllers/`, `Models/`, `Data/`, `Repositories/`) |
| Dapper y procedimientos almacenados | `Practica2.Api/Repositories/*Repository.cs` (`CommandType.StoredProcedure`, parámetros tipados) |
| Layout con mensaje de bienvenida y 3 opciones de menú | `Practica2.Web/Views/Shared/_Layout.cshtml` |
| Registro de Clientes (Cédula, Nombre, Correo) | `Views/Clientes/Registro.cshtml`, `ClientesController`, `POST api/clientes` → `usp_Clientes_Registrar` |
| Registro de Mascotas (Nombre, Especie, Raza, Peso, Cliente en lista desplegable) | `Views/Mascotas/Registro.cshtml`, `MascotasController.Registro`, `POST api/mascotas` → `usp_Mascotas_Registrar`; lista con `GET api/clientes` → `usp_Clientes_Listar` |
| Consulta de Mascotas (cédula y nombre del cliente, nombre, especie y peso de la mascota) | `Views/Mascotas/Consulta.cshtml`, `GET api/mascotas` → `usp_Mascotas_Consultar` |
| La cédula no se puede repetir | `usp_Clientes_Registrar` (código 2) → HTTP 409 → mensaje en el formulario |
| Máximo 2 mascotas de la misma especie (por cliente, sin distinguir mayúsculas ni tildes) | `usp_Mascotas_Registrar` (código 4) → HTTP 409 → mensaje en el formulario |
| Campos obligatorios | DataAnnotations en ViewModels (validación en cliente y servidor), DTOs del API y validación en los SP (código 1) |

## Notas

- Todo el JavaScript está en `Practica2.Web/wwwroot/js/` (`site.js`: sidebar, alertas y bloqueo de doble envío; `validacion.js`: integración de jquery-validation con Bootstrap 5/Spark). Las vistas solo referencian scripts con `src`.
- La Web fija la cultura a Invariant para que el Peso acepte punto decimal (`12.5`) aunque Windows esté en una región con coma decimal.
- `Practica2.Api` referencia directamente `System.Security.Cryptography.Pkcs` 10.0.13: `Microsoft.Data.SqlClient` 7.1.1 trae la 9.0.18, que tiene vulnerabilidades conocidas (GHSA-846f-f764-76mx, GHSA-2fmv-rv2c-59x7).
- Plantilla [Spark Admin](https://sparkadminpro.gumroad.com/), distribuida por [ThemeWagon](https://themewagon.com/) (MIT); el crédito se conserva en el pie de página. Solo se copiaron a `wwwroot/lib/spark` los assets usados.
