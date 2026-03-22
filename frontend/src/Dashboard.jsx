import { useCallback, useEffect, useState } from 'react'

const apiBase = import.meta.env.VITE_API_URL ?? 'http://localhost:5000'

async function fetchTasks(signal) {
  const res = await fetch(`${apiBase}/api/tasks`, { signal })
  if (!res.ok) throw new Error(`HTTP ${res.status}`)
  return res.json()
}

async function createTask(name) {
  const res = await fetch(`${apiBase}/api/tasks`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name }),
  })
  if (!res.ok) {
    const text = await res.text()
    throw new Error(text || `HTTP ${res.status}`)
  }
  return res.json()
}

export default function Dashboard() {
  const [tasks, setTasks] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [newName, setNewName] = useState('')
  const [saving, setSaving] = useState(false)

  const load = useCallback(async (signal) => {
    setLoading(true)
    setError(null)
    try {
      const data = await fetchTasks(signal)
      setTasks(data)
    } catch (e) {
      if (e.name === 'AbortError') return
      setError(e.message ?? String(e))
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    const ac = new AbortController()
    load(ac.signal)
    return () => ac.abort()
  }, [load])

  async function handleSubmit(e) {
    e.preventDefault()
    const name = newName.trim()
    if (!name || saving) return
    setSaving(true)
    setError(null)
    try {
      await createTask(name)
      setNewName('')
      await load()
    } catch (err) {
      setError(err.message ?? String(err))
    } finally {
      setSaving(false)
    }
  }

  return (
    <main className="dashboard">
      <header className="dashboard__header">
        <h1>Zadania</h1>
        <p className="dashboard__hint">
          Dodawanie wyłącznie przez UI (POST <code>/api/tasks</code>) — bez Swaggera.
        </p>
      </header>

      <section className="dashboard__form-section">
        <h2>Nowe zadanie</h2>
        <form className="task-form" onSubmit={handleSubmit}>
          <label htmlFor="task-name">Nazwa</label>
          <div className="task-form__row">
            <input
              id="task-name"
              type="text"
              value={newName}
              onChange={(e) => setNewName(e.target.value)}
              placeholder="np. Przygotować deploy"
              autoComplete="off"
              disabled={saving}
            />
            <button type="submit" disabled={saving || !newName.trim()}>
              {saving ? 'Zapisywanie…' : 'Dodaj'}
            </button>
          </div>
        </form>
      </section>

      <section className="dashboard__list-section">
        <h2>Lista</h2>
        {loading && <p>Ładowanie…</p>}
        {error && <p className="dashboard__error" role="alert">{error}</p>}
        {!loading && !error && tasks.length === 0 && (
          <p className="dashboard__empty">Brak zadań — dodaj pierwsze powyżej.</p>
        )}
        {!loading && tasks.length > 0 && (
          <ul className="task-list">
            {tasks.map((t) => (
              <li key={t.id} className="task-list__item">
                <span className="task-list__name">{t.name}</span>
                <span className="task-list__status">
                  {t.isCompleted ? 'ukończone' : 'otwarte'}
                </span>
              </li>
            ))}
          </ul>
        )}
      </section>
    </main>
  )
}
