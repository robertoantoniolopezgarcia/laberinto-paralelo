# Resolvedor de Laberintos Gigantes

Aplicación de consola en C# que genera un laberinto grande y calcula el camino
más corto entre la entrada y la salida usando varios algoritmos de búsqueda
(BFS, DFS, A* y Dijkstra) ejecutados **en paralelo** con Task Parallel Library (TPL).

Ver `/docs/Introduccion.md` para la explicación completa del proyecto,
la arquitectura y el reparto de tareas del equipo.

## Cómo correr el proyecto

```bash
cd src/MazeSolver
dotnet run -- 1000
```

El argumento (`1000` en el ejemplo) es el tamaño del laberinto (N x N). Si no
se pasa ningún argumento, se usa 1000 por defecto.

## Estructura del repositorio

maze-solver-parallel/
├── docs/ -> Documentación del proyecto
├── src/ -> Código fuente
├── tests/ -> Pruebas unitarias
└── metrics/ -> Resultados de comparativas (tiempos, speedup, eficiencia)

## Equipo

- Roberto Antonio López García (líder) — Generador de laberinto + estructura base
- Esmil Adames — Solver BFS + DFS
- Oscar Disla — Solver A* + Dijkstra
- Gabriel Villa Cordero — Orquestador paralelo (TPL)
- Mirelys De La Rosa De La Rosa — Métricas y pruebas

