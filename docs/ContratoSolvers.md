# Contrato técnico de los Solvers

Todos los algoritmos de búsqueda (BFS, DFS, A*, Dijkstra) implementan la
interfaz `ISolver` (`src/SolucionadorLaberinto/Solvers/ISolver.cs`):

```csharp
public interface ISolver
{
    string Name { get; }
    SolveResult Solve(MazeCell[,] maze, MazeCell start, MazeCell goal);
}
```

## Entrada

- `maze`: la matriz `MazeCell[,]` generada por `MazeGenerator.Generate(...)`.
  Cada `MazeCell` expone `Row`, `Col` y `Walls` (usar `HasWall(Walls.Top)`,
  etc. para saber si se puede pasar hacia un vecino).
- `start` / `goal`: las celdas de entrada y salida del laberinto.

## Salida (`SolveResult`, en `src/MazeSolver/Metrics/SolveResult.cs`)

- `PathFound`: si se encontró un camino.
- `Path`: lista ordenada de `MazeCell` desde `start` hasta `goal`.
- `NodesVisited`: cuántas celdas revisó el algoritmo (para comparar cuán
  "exploratorio" fue cada uno).
- `ElapsedMilliseconds`: tiempo que tardó ese solver en particular.

## Reglas importantes

1. **No modificar el laberinto.** Los solvers solo leen `maze`; cada uno debe
   llevar su propio registro de celdas visitadas (no usar
   `MazeCell.VisitedDuringGeneration`, que es exclusivo del generador).
2. **Thread-safety.** Como los 4 solvers corren en paralelo sobre el mismo
   `maze`, cada uno debe trabajar con sus propias estructuras internas
   (ej. su propia cola/pila/diccionario de visitados) — nunca compartir
   estado mutable entre solvers.
3. Cualquier cambio a esta interfaz o a `SolveResult` se discute con el
   equipo antes de implementarlo, para no romper el trabajo de los demás.