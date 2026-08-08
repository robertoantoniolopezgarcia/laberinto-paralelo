# Estrategia de Paralelización

## Enfoque elegido: Task Parallel Library (TPL) con `Task.Run` + `Task.WaitAll`

Cada uno de los 4 algoritmos de búsqueda (BFS, DFS, A*, Dijkstra) se lanza
como una `Task` independiente con `Task.Run(...)`. El orquestador
(`SolverOrchestrator`) espera a que todas terminen con `Task.WaitAll(...)`
antes de devolver los resultados.

Esto es más apropiado que `Parallel.ForEach` porque no estamos paralelizando
iteraciones sobre una colección uniforme (ej. recorrer 1000 filas igual);
estamos ejecutando 4 algoritmos **distintos entre sí**, cada uno con su
propia lógica interna — es paralelismo de tareas heterogéneas, no de datos.

## Estructura compartida de resultados

Todos los solvers leen la misma matriz `MazeCell[,]` (nunca la modifican,
según el contrato definido en `ContratoSolvers.md`), y cada uno escribe su
resultado en un `ConcurrentDictionary<string, SolveResult>` compartido,
usando el nombre del algoritmo (`ISolver.Name`) como clave.

Se usa `ConcurrentDictionary` en vez de un `Dictionary` normal porque varias
tareas escriben al mismo tiempo — un `Dictionary` común no es thread-safe y
podría corromperse o lanzar excepciones si dos tareas escriben a la vez.

## Flujo

1. `SolverOrchestrator.RunAll(maze, start, goal, solvers)` recibe la lista
   de algoritmos a correr.
2. Lanza una `Task` por cada uno con `Task.Run(...)`.
3. Cada tarea corre `solver.Solve(maze, start, goal)` y guarda el resultado
   en el diccionario compartido bajo su propia clave — sin pisar el trabajo
   de las demás tareas.
4. `Task.WaitAll(...)` bloquea hasta que las 4 terminen.
5. Se devuelve el diccionario completo, listo para que el módulo de
   métricas calcule speedup y eficiencia comparando los 4 resultados.