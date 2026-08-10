# Arquitectura del Sistema
## Diagrama de componentes

```mermaid
flowchart TD
    A[Program.cs] --> B[MazeGenerator]
    B --> C[SolverOrchestrator]

    C -->|RunSequential| D1[BfsSolver]
    C -->|RunSequential| D2[DfsSolver]
    C -->|RunSequential| D3[AStarSolver]
    C -->|RunSequential| D4[DijkstraSolver]

    C -->|RunAll - paralelo TPL| D1
    C -->|RunAll - paralelo TPL| D2
    C -->|RunAll - paralelo TPL| D3
    C -->|RunAll - paralelo TPL| D4

    D1 --> E[SolveResult]
    D2 --> E
    D3 --> E
    D4 --> E

    E --> F[PerformanceTracker]
    F --> G[(metrics/solver_results.csv)]

    style A fill:#2E5B8A,color:#fff
    style B fill:#4A7FAE,color:#fff
    style C fill:#4A7FAE,color:#fff
    style D1 fill:#E8A33D,color:#000
    style D2 fill:#E8A33D,color:#000
    style D3 fill:#E8A33D,color:#000
    style D4 fill:#E8A33D,color:#000
    style E fill:#5B8A5B,color:#fff
    style F fill:#5B8A5B,color:#fff
    style G fill:#888,color:#fff
```
## Flujo de ejecución

1. `Program.cs` genera el laberinto una sola vez con `MazeGenerator.Generate(size, size, seed: 42)`.
2. Se instancian los 4 solvers (`BfsSolver`, `DfsSolver`, `AStarSolver`, `DijkstraSolver`), todos implementando la interfaz común `ISolver`.
3. `SolverOrchestrator.RunSequential(...)` corre los 4 uno por uno — sirve de referencia base de tiempo.
4. `SolverOrchestrator.RunAll(...)` corre los 4 en paralelo con Task Parallel Library, guardando cada resultado en un `ConcurrentDictionary<string, SolveResult>` thread-safe (ver `docs/EstrategiaParalelizacion.md` para el detalle).
5. Se calcula speedup (tiempo secuencial ÷ tiempo paralelo) y eficiencia (speedup ÷ cantidad de algoritmos).
6. `PerformanceTracker` exporta los resultados de la corrida paralela a `metrics/solver_results.csv`.

## Namespaces del proyecto

| Namespace | Contenido |
|---|---|
| `MazeSolver` | `Program.cs` — punto de entrada |
| `MazeSolver.Maze` | `MazeCell`, `MazeGenerator` — generación del laberinto |
| `MazeSolver.Solvers` | `ISolver` y las 4 implementaciones (BFS, DFS, A*, Dijkstra) |
| `MazeSolver.Orchestration` | `SolverOrchestrator` — ejecución secuencial y paralela |
| `MazeSolver.Metrics` | `SolveResult`, `PerformanceTracker` — resultados y export a CSV |

## Contratos clave

- **`ISolver`** (`docs/ContratoSolvers.md`): todos los algoritmos reciben `(maze, start, goal)` y devuelven `SolveResult`, sin modificar el laberinto.
- **Thread-safety**: cada solver mantiene su propio estado interno (cola/pila/diccionario de visitados); el único punto compartido es el `ConcurrentDictionary` de resultados en `SolverOrchestrator.RunAll`.