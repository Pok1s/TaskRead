# Cloud Tasks — Artefakt 5 (system pod chmurę)

React (Vite) + ASP.NET Core Web API + Azure SQL Edge w Dockerze.

## Wymagania

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Node.js 20+](https://nodejs.org/)

---

## Szybki start

### 1. Baza (Docker)

Z katalogu repozytorium:

```bash
docker compose up -d
docker compose logs -f db
```

Poczekaj, aż kontener będzie **healthy** (`docker compose ps` — kolumna STATUS) albo minie **ok. 30–60 s** przy pierwszym uruchomieniu. Nie przerywaj startu przez Ctrl+C w pierwszych sekundach — to normalne, że w logach widać ostrzeżenia (`mssql.conf`, połączenie z `localhost:1431`): usługi w kontenerze startują sekwencyjnie.

SQL: `localhost:1433`, użytkownik `sa`, hasło `StrongPassword123!` (jak w `appsettings.json`).

**Trwałość (Artefakt 5.2):** dane plików bazy są w **`./docker/sql-data`** na dysku hosta. Po **`docker compose down -v`** kontenery i sieci są usuwane, ale **katalog `./docker/sql-data` zostaje** — po ponownym `docker compose up -d` te same dane są widoczne w UI.

> Uwaga z zajęć: czysty **named volume** Dockera (`sql-data:` w sekcji `volumes`) jest **usuwany** przez `docker compose down -v`. Ten projekt używa **persistencji na hoście** (`./docker/sql-data:/var/opt/mssql`), żeby spełnić scenariusz z PDF (dane po `down -v` + `up`).

### 2. Backend

```bash
cd backend/CloudTasks.Api
dotnet restore
dotnet run
```

- API (Docker Compose): `http://localhost:5050` — port **5050** (na macOS port **5000** bywa zajęty przez AirPlay).
- Swagger (tylko dev): `http://localhost:5050/swagger`

Migracje EF: folder `Migrations/`; przy starcie: `Database.Migrate()`.

### 3. Frontend

```bash
cd frontend
npm install
npm run dev
```

Adres API: `frontend/.env` → `VITE_API_URL=http://localhost:5050` (zgodnie z `docker compose`).

### Test API z terminala (Artefakt 5.1 — zrzuty)

```bash
chmod +x scripts/verify-api.sh
./scripts/verify-api.sh
```

Opcja: `API_URL=http://127.0.0.1:5050 ./scripts/verify-api.sh`

---

## Artefakt 5 — checklista i PDF

Jeden plik PDF: **`Artefakt05_Imie_Nazwisko_Twoj_numer_studenta.pdf`** (wszystkie screeny).

| Punkt | Co zrobić |
| --- | --- |
| **5.1** | Kod: `GetAll` / `GetById` → `TaskReadDto`; plik `backend/CloudTasks.Api/Controllers/TasksController.cs`. Zrzuty: build + test POST/GET (np. `verify-api.sh` lub Swagger). |
| **5.2** | Dodaj zadanie w React; zrób zrzut. Potem `docker compose down -v`, potem `docker compose up -d`, uruchom API + UI — zadanie nadal jest (dane w `./docker/sql-data`). |
| **5.3** | Zrzut folderu `backend/CloudTasks.Api/Migrations/` w IDE. |
| **5.4** | Zrzut React — dodanie zadania **bez** używania Swaggera. |
| **5.5** | `git push` + zrzut strony README na GitHubie, np. `https://github.com/Pok1s/TaskRead/blob/main/README.md` |

**Szablon HTML do sklejenia zrzutów i eksportu do PDF:** otwórz w przeglądarce plik `artefakt05/Artefakt05_szablon.html` → wklej screeny w „ramki” → Drukuj → Zapisz jako PDF.

---

## Struktura

```
TaskRead/
├── docker-compose.yml
├── docker/sql-data/          # dane SQL na hoście (git: tylko .gitkeep)
├── scripts/verify-api.sh
├── artefakt05/Artefakt05_szablon.html
├── backend/CloudTasks.Api/
└── frontend/
```
