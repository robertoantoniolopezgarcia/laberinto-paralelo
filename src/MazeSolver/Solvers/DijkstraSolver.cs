using MazeSolver.Maze;
using MazeSolver.Metrics;
using System;
using System.Diagnostics;

namespace MazeSolver.Solvers
{
    /// <summary>
    /// Implementación base del algoritmo de Dijkstra.
    /// Este solver calcula el camino más corto considerando
    /// el costo acumulado desde la entrada hasta cada celda.
    ///
    /// La lógica completa del algoritmo se implementará sobre esta
    /// estructura base.
    /// </summary>
    public class DijkstraSolver : ISolver
    {
        /// <summary>
        /// Nombre del algoritmo.
        /// </summary>
        public string Name => "Dijkstra";

        /// <summary>
        /// Busca un camino entre la entrada y la salida del laberinto.
        /// </summary>
        public SolveResult Solve(MazeCell[,] maze, MazeCell start, MazeCell goal)
        {
            var stopwatch = Stopwatch.StartNew();

            // Estructuras principales utilizadas por el algoritmo de Dijkstra.
            var openSet = new PriorityQueue<MazeCell, int>();
            var closedSet = new HashSet<MazeCell>();
            var cameFrom = new Dictionary<MazeCell, MazeCell>();
            var distances = new Dictionary<MazeCell, int>();

            // La celda inicial comienza con una distancia de 0.
            distances[start] = 0;

            // Agregar la celda inicial a la cola de prioridad.
            openSet.Enqueue(start, 0);

            int nodesVisited = 0;

            // Continuar mientras existan celdas pendientes por explorar.
            // Continuar mientras existan celdas pendientes por explorar.
            while (openSet.Count > 0)
            {
                // Obtener la celda con la menor distancia acumulada.
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

                    // Todas las celdas tienen el mismo costo de recorrido,
                    // por lo que avanzar a un vecino incrementa la distancia en una unidad.
                    int tentativeDistance = distances[current] + 1;

                    // Si el vecino aún no tiene una distancia registrada o se encontró
                    // un camino más corto, actualizar su información.
                    if (!distances.ContainsKey(neighbor) || tentativeDistance < distances[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        distances[neighbor] = tentativeDistance;

                        // En Dijkstra la prioridad corresponde únicamente
                        // a la distancia acumulada desde la entrada.
                        openSet.Enqueue(neighbor, tentativeDistance);
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
        /// Obtiene las celdas vecinas accesibles desde la celda actual.
        /// Solo se devuelven aquellas direcciones donde no existe una pared.
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

