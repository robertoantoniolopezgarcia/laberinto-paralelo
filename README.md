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

El primer argumento (`1000` en el ejemplo) es el tamaño del laberinto (N x N).
Si no se pasa ningún argumento, se usa 1000 por defecto.

Opcionalmente, se puede pasar un segundo argumento para elegir cuántos
algoritmos corren en paralelo al mismo tiempo, entre 2 y 4 (la cantidad
total de algoritmos disponibles):

```bash
dotnet run -- 1000 2
```

Si se omite, se usa el máximo (4) por defecto. Correr con menos núcleos
permite comparar cómo cambia la eficiencia según la cantidad de tareas
paralelas activas.

## Estructura del repositorio

maze-solver-parallel/
├── docs/ -> Documentación del proyecto
├── src/ -> Código fuente
├── tests/ -> Pruebas unitarias
└── metrics/ -> Resultados de comparativas (tiempos, speedup, eficiencia)

## Equipo

- Roberto Antonio López García (líder): estructura base del repositorio,
  generador de laberinto, orquestador paralelo (incluyendo la selección de
  núcleos) e integración final del sistema.
- Esmil Adames: solvers BFS y DFS.
- Oscar Disla: solvers A* y Dijkstra.
- Mirelys De La Rosa De La Rosa: métricas, cálculo de speedup/eficiencia
  y pruebas.

Nota: el equipo inició con 5 integrantes. Gabriel Villa Cordero, originalmente
asignado al orquestador paralelo, retiró la asignatura durante el desarrollo
del proyecto; su responsabilidad fue reasignada al líder del equipo.