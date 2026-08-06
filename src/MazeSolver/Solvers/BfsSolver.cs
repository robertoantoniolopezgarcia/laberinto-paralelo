using System.Collections.Generic;
using System.Diagnostics;
using MazeSolver.Maze;
using MazeSolver.Metrics;

namespace MazeSolver.Solvers
{
    /// <summary>
    /// Busqueda en Anchura BFS para encontrar el camino mas corto en un laberinto
    /// BFS explora por niveles usando una Cola Queue, asi que el primer camino que encuentra es el mas corto
    /// 
    /// Esta version tiene optimizaciones para laberintos grandes de 1M de celdas
    /// usa bool[,] en vez de HashSet para los visitados, y matriz de padres en vez de Dictionary
    /// tambien evita iteradores y yield para no crear objetos extras
    /// </summary>
    public class BfsSolver : ISolver
    {
        public string Name => "BFS";

        /// <summary>
        /// Ejecuta el BFS para encontrar el camino mas corto entre inicio y meta.
        /// Devuelve PathFound = false si los parametros son invalidos, las celdas
        /// estan fuera del laberinto, o no existe ningun camino entre inicio y meta.
        /// </summary>
        /// <param name="maze">El laberinto como matriz de celdas</param>
        /// <param name="start">Donde empieza la busqueda</param>
        /// <param name="goal">Donde termina la busqueda</param>
        /// <returns>El resultado con la ruta y las metricas</returns>
        public SolveResult Solve(MazeCell[,] maze, MazeCell start, MazeCell goal)
        {
            // --- Guardas de entrada ---

            // 1. Parametros nulos: no se puede operar sin laberinto o celdas validas.
            if (maze == null || start == null || goal == null)
                return new SolveResult { AlgorithmName = Name, PathFound = false };

            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            // 2. Coordenadas fuera del laberinto: evita IndexOutOfRangeException.
            if (start.Row < 0 || start.Row >= rows || start.Col < 0 || start.Col >= cols ||
                goal.Row  < 0 || goal.Row  >= rows || goal.Col  < 0 || goal.Col  >= cols)
                return new SolveResult { AlgorithmName = Name, PathFound = false };

            // 3. Caso trivial: inicio y meta son la misma celda.
            if (start.Row == goal.Row && start.Col == goal.Col)
                return new SolveResult
                {
                    AlgorithmName  = Name,
                    PathFound      = true,
                    Path           = new List<MazeCell> { start },
                    NodesVisited   = 1,
                    ElapsedMilliseconds = 0
                };
            var stopwatch = Stopwatch.StartNew();

            // Una matriz de booleanos para saber que celdas ya visitamos
            // Es mas rapido que un HashSet porque no hay que calcular hashes
            var visited = new bool[rows, cols];

            // Matriz para guardar quien fue el padre de cada celda
            // podemos reconstruir el camino despues
            // Si es null significa que esa celda aun no tiene padre
            var parent = new MazeCell?[rows, cols];

            // La cola es el corazon del BFS, con capacidad inicial para evitar redimensiones
            // En un laberinto de 1M de celdas, aproximadamente la mitad son accesibles
            int initialCapacity = (rows * cols) / 2;
            var queue = new Queue<MazeCell>(initialCapacity);

            // Empezamos con la celda de inicio
            queue.Enqueue(start);
            visited[start.Row, start.Col] = true;
            // parent de iniciio queda null, eso indica que es la raiz

            bool pathFound = false;
            int nodesVisited = 0;

            // El bucle principal
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                nodesVisited++;

                // Si llegamos a la meta, terminamos
                if (current.Row == goal.Row && current.Col == goal.Col)
                {
                    pathFound = true;
                    break;
                }

                int r = current.Row;
                int c = current.Col;

                // Revisamos los 4 vecinos sin usar funciones ni iteradores
                // Todo esta inline para que sea lo mas rapido posible

                // Vecino de arriba
                if (r > 0 && !current.HasWall(Walls.Top) && !visited[r - 1, c])
                {
                    visited[r - 1, c] = true;
                    parent[r - 1, c] = current;
                    queue.Enqueue(maze[r - 1, c]);
                }

                // Vecino de la derecha
                if (c < cols - 1 && !current.HasWall(Walls.Right) && !visited[r, c + 1])
                {
                    visited[r, c + 1] = true;
                    parent[r, c + 1] = current;
                    queue.Enqueue(maze[r, c + 1]);
                }

                // Vecino de abajo
                if (r < rows - 1 && !current.HasWall(Walls.Bottom) && !visited[r + 1, c])
                {
                    visited[r + 1, c] = true;
                    parent[r + 1, c] = current;
                    queue.Enqueue(maze[r + 1, c]);
                }

                // Vecino de la izquierda
                if (c > 0 && !current.HasWall(Walls.Left) && !visited[r, c - 1])
                {
                    visited[r, c - 1] = true;
                    parent[r, c - 1] = current;
                    queue.Enqueue(maze[r, c - 1]);
                }
            }

            stopwatch.Stop();

            // Armamos el resultado
            return new SolveResult
            {
                AlgorithmName = Name,
                PathFound = pathFound,
                Path = pathFound ? ReconstructPath(parent, goal) : new List<MazeCell>(),
                NodesVisited = nodesVisited,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
            };
        }

        /// <summary>
        /// Reconstruye el camino desde la meta hasta el inicio usando la matriz de padres
        /// y luego lo invierte para que quede en orden correcto
        /// </summary>
        /// <param name="parent">Matriz de padres donde parent[r,c] es la celda desde la que llegamos</param>
        /// <param name="goal">La celda objetivo desde donde empezamos a reconstruir</param>
        /// <returns>Lista de celdas ordenada desde inicio hasta meta</returns>
        private static List<MazeCell> ReconstructPath(MazeCell?[,] parent, MazeCell goal)
        {
            var path = new List<MazeCell>();
            MazeCell? current = goal;

            // Vamos subiendo desde la meta hasta el inicio
            // Cuando current sea null, significa que llegamos al inicio
            while (current != null)
            {
                path.Add(current);
                current = parent[current.Row, current.Col];
            }

            // El camino quedo al reves, meta->inicio, asi que lo damos vuelta
            path.Reverse();
            return path;
        }
    }
}