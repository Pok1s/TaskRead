#!/usr/bin/env bash
# Weryfikacja zapisu/odczytu API (Artefakt 5.1) — uruchom gdy działa `dotnet run` (backend) i `docker compose up -d` (baza).
set -euo pipefail
API="${API_URL:-http://localhost:5000}"

echo "=== POST /api/tasks ==="
curl -sS -X POST "${API}/api/tasks" \
  -H "Content-Type: application/json" \
  -d '{"name":"Test z verify-api.sh"}' | tee /tmp/task-post.json
echo ""

echo "=== GET /api/tasks ==="
curl -sS "${API}/api/tasks" | tee /tmp/task-get.json
echo ""

echo "=== GET /api/tasks/1 (jeśli istnieje id=1) ==="
curl -sS -o /tmp/task-byid.json -w "HTTP %{http_code}\n" "${API}/api/tasks/1" || true
cat /tmp/task-byid.json 2>/dev/null || true
echo ""
echo "Gotowe — zrób screeny z terminala + Swagger (opcjonalnie)."
