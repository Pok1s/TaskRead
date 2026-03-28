# TaskRead (Cloud Tasks)

Prosty projekt na zajęcia: lista zadań w React (Vite) + API w .NET, baza SQL.

**Nr albumu:** 96709  
**Repo:** https://github.com/Pok1s/TaskRead

Robię to samodzielnie.

## Azure

Wystawiłem wszystko w Azure. API działa pod HTTPS, front też.

- API (Swagger): https://cloudtasks-api-f7818cc6.azurewebsites.net/swagger  
- Front (Static Web Apps): https://gray-cliff-023fc1903.2.azurestaticapps.net  

Baza to Azure SQL — w portalu jest włączona i firewall tak, żeby łączyć się z kompa przy developmencie i żeby działały usługi z Azure.

Connection string do bazy trzymam w `backend/CloudTasks.Api/appsettings.Development.json` (sekcja `ConnectionStrings`). Tam trzeba wpisać swoje dane z Azure zamiast placeholderów. Migracje odpalałem lokalnie:

```bash
cd backend/CloudTasks.Api
dotnet ef database update
```

Na App Service w konfiguracji trzeba też ustawić connection string do tej samej bazy (produkcja nie bierze `appsettings.Development.json`).

Front na produkcję buduje się z `frontend/.env.production` — tam jest `VITE_API_URL` ustawione na adres API z góry.

## Jak odpalić lokalnie

Potrzebne: .NET 8, Node 20+, opcjonalnie Docker.

**Cały stack przez Docker** (z głównego folderu repo):

```bash
docker compose up -d --build
```

Front: http://localhost:5173, API: http://localhost:5050/swagger, SQL na porcie 1433 (dane jak w `appsettings.json`).

**Albo ręcznie:** `cd backend/CloudTasks.Api` → `dotnet run`, w drugim terminalu `cd frontend` → `npm install` → `npm run dev`. W `frontend/.env` jest `VITE_API_URL` na localhost.

Test skryptem: `./scripts/verify-api.sh` (jak nie działa, `chmod +x`).

## Struktura

`backend/CloudTasks.Api` — Web API, `frontend` — React, `docker-compose.yml` — lokalnie baza + API + Vite.
