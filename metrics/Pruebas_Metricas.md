# Ejecuciones de métricas

Se realizaron pruebas del resolvedor de laberintos utilizando diferentes
tamaños de laberinto y cantidades de núcleos para observar el comportamiento
de la ejecución paralela.

## Tamaños utilizados

- 1000x1000
- 2000x2000
- 5000x5000

## Núcleos utilizados

- 2 núcleos
- 3 núcleos
- 4 núcleos

## Métricas registradas

Cada archivo CSV contiene los resultados de los cuatro algoritmos:

- BFS
- DFS
- Dijkstra
- A*

Las métricas registradas son:

- PathFound: indica si se encontró el camino.
- NodesVisited: cantidad de nodos visitados.
- PathLength: longitud del camino encontrado.
- ElapsedMilliseconds: tiempo de ejecución del algoritmo.

## Archivos

Cada archivo sigue el formato:

solver_results_[tamaño]_[núcleos]cores.csv

Por ejemplo:

solver_results_1000x1000_2cores.csv

corresponde a la ejecución de un laberinto de 1000x1000 utilizando
2 núcleos.
