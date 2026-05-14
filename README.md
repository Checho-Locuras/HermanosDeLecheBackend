## Hermanos de Leche — Backend (.NET 10)

API para la plataforma rural‑social (con humor) donde los **lecheros** registran qué **vacas** han tomado su leche, el historial y estadísticas agregadas.

### Requisitos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0) **para compilar** (`net10.0`)
- Para **ejecutar** sin SDK basta el runtime **ASP.NET Core 10** (`Microsoft.AspNetCore.App`). En Windows: `winget install Microsoft.DotNet.AspNetCore.10` (o el SDK 10, que ya lo incluye).
- PostgreSQL accesible desde tu máquina (Railway, Docker o local)

### Estructura de la solución

| Proyecto | Rol |
|----------|-----|
| `HermanosDeLeche.Api` | Web API, Swagger, JWT, CORS, middleware de errores |
| `HermanosDeLeche.Domain` | Entidades, DTOs, interfaces |
| `HermanosDeLeche.Service` | Servicios, repositorios, ADO.NET + **Npgsql** (sin ORM) |

### Base de datos (SQL manual)

En la carpeta `database/`:

1. **`init.sql`** — crea extensiones (si aplica), tablas, claves, índices y reglas `CHECK`.
2. **`seed.sql`** — datos opcionales de demostración (varios lecheros, vacas e ingestas).
3. **`seed_3_lecheros_5_vacas.sql`** — 3 lecheros y 5 vacas de demostración (contraseña `Lechero123!` para los tres).
4. **`verify_schema.sql`** — muestra la base conectada y las tablas en `public` (útil para diagnosticar).

#### Aplicar en PostgreSQL (Railway u otro)

Con `psql` y la URL pública (ejemplo del proyecto; **rota credenciales si las expones en público**):

```bash
psql "postgresql://postgres:TU_PASSWORD@TU_HOST:TU_PUERTO/railway" -f database/init.sql
psql "postgresql://postgres:TU_PASSWORD@TU_HOST:TU_PUERTO/railway" -f database/seed.sql
```

En el cliente web de Railway: pestaña **Query** / **Data** → elige la base de datos **`railway`** (no la plantilla **`postgres`**, que suele estar vacía). Luego pega y ejecuta `init.sql` y los seeds que necesites.

**¿No ves tablas?** Comprueba el nombre de la base en la URL (`...postgresql://...@host:puerto/**railway**`) o en variables `PGDATABASE` / `DATABASE_URL`. La API de este repo usa **`Database=railway`** en la cadena Npgsql.

#### Usuarios de demostración (`seed.sql`)

Todos los lecheros creados en el seed comparten la contraseña: **`Lechero123!`**

Usernames: `don_tiple`, `mlechera`, `pepe_p`.

### Cadena de conexión

Por defecto, `HermanosDeLeche.Api/appsettings.json` trae una cadena **local de ejemplo** (`localhost`, base `hermanos`). Ajusta usuario, contraseña y base a tu PostgreSQL, o sobreescribe con variables de entorno (recomendado en Railway):

`ConnectionStrings__Default=Host=...;Port=...;Username=...;Password=...;Database=...;SSL Mode=Require;Trust Server Certificate=true`

Si usas Railway, copia la URL/credenciales desde el panel del servicio Postgres **sin subirlas al repositorio público**.

**Seguridad:** no subas credenciales reales a repos públicos. En producción usa variables de entorno (`ConnectionStrings__Default`, `Jwt__Key`, etc.).

### JWT

Configuración bajo `Jwt` en `appsettings.json`:

- `Key`: mínimo **32 caracteres** (obligatorio para HS256).
- En Railway: variable de entorno `Jwt__Key` con un valor largo y aleatorio.

### Ejecutar en local

En la raíz del repositorio **no** hay un `.csproj` ejecutable: `dotnet run` sin argumentos mostrará que no encuentra proyecto. Usa `--project` o entra en `HermanosDeLeche.Api/`.

Desde la raíz del repositorio (recomendado; abre Swagger según `launchSettings`):

```powershell
dotnet restore HermanosDeLeche.sln
dotnet build HermanosDeLeche.sln
dotnet run --project HermanosDeLeche.Api\HermanosDeLeche.Api.csproj --launch-profile https
```

Solo HTTP:

```powershell
dotnet run --project HermanosDeLeche.Api\HermanosDeLeche.Api.csproj --launch-profile http
```

- Swagger UI: `https://localhost:7288/swagger` o `http://localhost:5288/swagger` (puertos en `Properties/launchSettings.json`). Si ves *address already in use*, cierra otra instancia de `dotnet run` o cambia esos puertos.
- Healthcheck: `GET /health`

### Docker

```bash
docker compose up --build
```

Ajusta `ConnectionStrings__Default` en `docker-compose.yml` para que el contenedor alcance tu PostgreSQL (por ejemplo `host.docker.internal` en Windows/Mac).

### Despliegue en Railway

1. Crea un servicio **PostgreSQL** y obtén la URL pública.
2. Ejecuta `database/init.sql` y opcionalmente `seed.sql` contra esa base.
3. Crea un servicio **Web** desde este repo (Dockerfile o Nixpacks con .NET 10).
4. Variables de entorno recomendadas:
   - `ConnectionStrings__Default` = cadena Npgsql completa (`Host=...;Port=...;Username=...;Password=...;Database=...;SSL Mode=Require;Trust Server Certificate=true`).
   - `Jwt__Key` = clave larga (≥ 32 caracteres).
   - Railway inyecta `PORT`; la API ya escucha en `0.0.0.0:PORT` cuando existe esa variable.

### Endpoints principales

| Área | Método | Ruta |
|------|--------|------|
| Auth | POST | `/api/auth/register`, `/api/auth/login` |
| Lecheros | GET | `/api/milkmen`, `/api/milkmen/{id}`, `/api/milkmen/hermanos-de-leche` (JWT: otros lecheros con vacas en común por ingestas) |
| Vacas | POST, GET, PUT, DELETE | `/api/cows`, `/api/cows/{id}` |
| Ingestas | POST, GET | `/api/intakes`, `/api/intakes/cow/{cowId}`, `/api/intakes/milkman/{milkmanId}` |
| Stats | GET | `/api/stats/dashboard`, `/api/stats/top-thirsty-cows`, `/api/stats/top-milk-consumption`, `/api/stats/top-generous-milkmen` |

**Autorización:** operaciones que modifican datos del lechero autenticado (`POST/PUT/DELETE` vacas, `POST` ingestas) requieren encabezado `Authorization: Bearer <jwt>` obtenido en login/register.

### Notas de modelo

- Las **vacas** tienen `milkman_id` (dueño que las registró).
- **`cow_milk_intakes`** guarda el historial: quién suministró (`milkman_id`), qué vaca (`cow_id`), litros, fecha y observaciones.
- Estadísticas “top” usan solo vacas/lecheros que **tienen al menos una ingesta** (no aparecen filas con totales en cero).

### Tecnologías

.NET 10, PostgreSQL, Npgsql (ADO.NET directo: `NpgsqlConnection`, `NpgsqlCommand`, `NpgsqlDataReader`), Swagger, JWT Bearer, BCrypt para contraseñas, middleware global de excepciones.
