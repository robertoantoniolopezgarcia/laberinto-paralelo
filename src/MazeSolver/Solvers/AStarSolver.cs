using System.Diagnostics;
using System.Collections.Generic;
using MazeSolver.Maze;
using MazeSolver.Metrics;

namespace MazeSolver.Solvers
{
    /// <summary>
    /// Implementación base del algoritmo A*.
    /// Este solver utiliza una heurística de distancia para estimar
    /// qué tan cerca se encuentra una celda del objetivo.
    ///
    /// La lógica completa del algoritmo se implementará sobre esta
    /// estructura base.
    /// </summary>
    public class AStarSolver : ISolver
    {
        /// <summary>
        /// Nombre del algoritmo.
        /// </summary>
        public string Name => "A*";

        /// <summary>
        /// Busca un camino entre la entrada y la salida del laberinto.
        /// </summary>
        public SolveResult Solve(MazeCell[,] maze, MazeCell start, MazeCell goal)
        {
            var stopwatch = Stopwatch.StartNew();

            // Estructuras principales utilizadas por el algoritmo A*.
            var openSet = new PriorityQueue<MazeCell, int>();
            var closedSet = new HashSet<MazeCell>();
            var cameFrom = new Dictionary<MazeCell, MazeCell>();
            var gScore = new Dictionary<MazeCell, int>();

            // La celda inicial comienza con un costo de 0.
            gScore[start] = 0;

            // Agregar la entrada a la cola de prioridad usando la heurística.
            openSet.Enqueue(start, CalculateHeuristic(start, goal));

            int nodesVisited = 0;

            // Continuar mientras existan celdas pendientes por explorar.
            while (openSet.Count > 0)
            {
                // Obtener la celda con la menor prioridad (menor costo estimado).
                var current = openSet.Dequeue();
                nodesVisited++;

                // Si la celda ya fue procesada, pasar a la siguiente.
                if (closedSet.Contains(current))
                    continue;

                // Marcar la celda como visitada para no volver a procesarla.
                closedSet.Add(current);

                // Si se llegó a la salida, reconstruir el camino encontrado.
                if (current == goal)
                {
                    stopwatch.Stop();

                    var path = new List<MazeCell>();
                    var step = goal;

                    // Reconstruir el camino retrocediendo desde la salida
                    // hasta la entrada utilizando los padres registrados.
                    path.Add(step);

                    while (cameFrom.ContainsKey(step))
                    {
                        step = cameFrom[step];
                        path.Add(step);
                    }

                    // El camino se construye al revés, por lo que se invierte
                    // antes de devolver el resultado.
                    path.Reverse();

                    return new SolveResult
                    {
                        AlgorithmName = Name,
                        PathFound = true,
                        Path = path,
                        NodesVisited = nodesVisited,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
                    };
                }

                // Explorar todas las celdas vecinas accesibles.
                foreach (var neighbor in GetNeighbors(maze, current))
                {
                    // Si la celda ya fue procesada, ignorarla.
                    if (closedSet.Contains(neighbor))
                        continue;

                    // El costo para llegar al vecino desde la celda actual
                    // aumenta en una unidad (todas las aristas tienen el mismo peso).
                    int tentativeGScore = gScore[current] + 1;

                    // Si el vecino aún no tiene un costo registrado o se encontró
                    // un camino más corto, actualizar su información.
                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;

                        // La prioridad se calcula sumando el costo recorrido (G)
                        // y la heurística estimada hasta la salida (H).
                        int priority = tentativeGScore + CalculateHeuristic(neighbor, goal);

                        openSet.Enqueue(neighbor, priority);
                    }
                }
            }

            stopwatch.Stop();

            return new SolveResult
            {
                AlgorithmName = Name,
                PathFound = false,
                Path = new List<MazeCell>(),
                NodesVisited = nodesVisited,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
            };
        }

        /// <summary>
        /// Calcula la distancia Manhattan entre dos celdas.
        /// Esta heurística estima qué tan lejos está una celda del objetivo.
        /// </summary>
        private static int CalculateHeuristic(MazeCell current, MazeCell goal)
        {
            return Math.Abs(current.Row - goal.Row) +
                   Math.Abs(current.Col - goal.Col);
        }

        /// <summary>
        /// Obtiene las celdas vecinas a las que es posible moverse desde la
        /// celda actual. Una dirección solo es válida si no existe una pared
        /// entre ambas celdas.
        /// </summary>
        private static IEnumerable<MazeCell> GetNeighbors(MazeCell[,] maze, MazeCell cell)
        {
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            // Verificar la celda superior.
            if (cell.Row > 0 && !cell.HasWall(Walls.Top))
                yield return maze[cell.Row - 1, cell.Col];

            // Verificar la celda inferior.
            if (cell.Row < rows - 1 && !cell.HasWall(Walls.Bottom))
                yield return maze[cell.Row + 1, cell.Col];

            // Verificar la celda izquierda.
            if (cell.Col > 0 && !cell.HasWall(Walls.Left))
                yield return maze[cell.Row, cell.Col - 1];

            // Verificar la celda derecha.
            if (cell.Col < cols - 1 && !cell.HasWall(Walls.Right))
                yield return maze[cell.Row, cell.Col + 1];
        }
    }
}
