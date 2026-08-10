# Prueba end-to-end — Laberinto 10x10

Prueba de integración completa del sistema: generación del laberinto,
ejecución de los 4 algoritmos (secuencial y paralelo), y exportación de
resultados a CSV.

## Comando ejecutado

```bash
cd src/MazeSolver
dotnet run -- 10
```

## Salida de consola

Generando laberinto de 10x10...
Laberinto generado en 1 ms
Entrada: (0,0) Salida: (9,9)
Resultados de los solvers:
DFS | PathFound: True | NodesVisited: 66 | PathLength: 31 | Time: 0 ms
BFS | PathFound: True | NodesVisited: 38 | PathLength: 31 | Time: 0 ms
Dijkstra | PathFound: True | NodesVisited: 38 | PathLength: 31 | Time: 0 ms
A* | PathFound: True | NodesVisited: 35 | PathLength: 31 | Time: 0 ms
Comparativa secuencial vs paralelo:
Tiempo total secuencial: 6 ms
Tiempo total paralelo: 2 ms
Speedup: 3.00x
Eficiencia: 75.0%

## Resultado exportado

Ver `metrics/solver_results_10x10.csv` con el detalle por algoritmo
(PathFound, NodesVisited, PathLength, ElapsedMilliseconds).

## Observaciones

- Los 4 algoritmos encontraron el mismo camino (longitud 31), lo cual es
  esperado: el generador crea un laberinto "perfecto" (sin ciclos), donde
  solo existe un único camino posible entre la entrada y la salida.
- La diferencia real entre algoritmos está en cuánto exploraron para
  llegar a ese camino: A* fue el más eficiente (35 nodos visitados),
  seguido de BFS y Dijkstra (38 nodos cada uno), y DFS fue el que más
  nodos visitó (66) por seguir un solo camino hasta el fondo antes de
  retroceder.
- En este tamaño (10x10), el speedup medido fue de 3.00x con una
  eficiencia del 75% sobre 4 tareas — un resultado favorable, aunque en
  laberintos tan chicos el tiempo real de cómputo es tan bajo (0-1 ms
  por algoritmo) que estas mediciones pueden variar bastante entre
  corridas. La comparativa se vuelve más significativa y estable en
  laberintos grandes (1000x1000+), donde el trabajo de cada algoritmo
  es mucho mayor al overhead de crear las tareas paralelas.

  