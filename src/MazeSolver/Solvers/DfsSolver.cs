using System.Collections.Generic;
using System.Diagnostics;
using MazeSolver.Maze;
using MazeSolver.Metrics;

namespace MazeSolver.Solvers
{
    /// <summary>
    /// Busqueda en Profundidad DFS para explorar laberintos.
    /// DFS se mete por un camino hasta el fondo y si no sale, retrocede y prueba otro.
    /// Usa una Pila Stack para ir guardando los nodos que va encontrando.
    /// No garantiza el camino mas corto, pero puede ser rapido si la salida esta
    /// en una rama profunda.
    /// 
    /// Esta version usa optimizaciones para laberintos grandes:
    
    /// - Matriz de visitados en vez de HashSet para ir mas rapido
    /// - Matriz de padres en vez de Diccionario
    /// - Vecinos revisados directamente sin llamar a funciones
    /// - Capacidad inicial de la pila para evitar redimensiones
    /// </summary>
    public class DfsSolver : ISolver
    {
        public string Name => "DFS";

        /// <summary>
        /// Ejecuta el DFS para encontrar un camino entre inicio y meta.
        /// Devuelve PathFound = false si los parametros son invalidos, las celdas
        /// estan fuera del laberinto, o no existe ningun camino entre inicio y meta.
        /// </summary>
        /// <param name="maze">El laberinto como matriz de celdas</param>
        /// <param name="start">Donde empieza la busqueda</param>
        /// <param name="goal">Donde termina la busqueda</param>
        /// <returns>Resultado con el camino si lo encuentra o indicador de fallo</returns>
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

            // Matriz para saber que celdas ya visitamos
            // E mas rapida que un HashSet
            var visited = new bool[rows, cols];

            // Matriz para guardar el padre de cada celda
            // asi despues podemos reconstruir el camino
            // null significa que esa celda aun no tiene padre
            var parent = new MazeCell?[rows, cols];

            // La pila es el corazon de DFS, usamos LIFO para ir en profundidad
            // Le damos capacidad inicial para que no tenga que crecer muchas veces
            int initialCapacity = (rows * cols) / 2;
            var stack = new Stack<MazeCell>(initialCapacity);

            // Empezamos con la celda de inicio
            stack.Push(start);
            visited[start.Row, start.Col] = true;
            // El padre del inicio queda null, eso indica que es la raiz

            bool pathFound = false;
            int nodesVisited = 0;

            // Bucle principal
            while (stack.Count > 0)
            {
                var current = stack.Pop();
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
                // Todo va inline para que sea lo mas rapido posible

                // Primero apilamos izquierda, despues abajo, despues derecha y al final arriba
                // Como la pila es LIFO, el ultimo en entrar es el primero en salir
                // Asi que arriba se va a explorar primero

                // Izquierda
                if (c > 0 && !current.HasWall(Walls.Left) && !visited[r, c - 1])
                {
                    visited[r, c - 1] = true;
                    parent[r, c - 1] = current;
                    stack.Push(maze[r, c - 1]);
                }

                // Abaj
                if (r < rows - 1 && !current.HasWall(Walls.Bottom) && !visited[r + 1, c])
                {
                    visited[r + 1, c] = true;
                    parent[r + 1, c] = current;
                    stack.Push(maze[r + 1, c]);
                }

                // Derecha
                if (c < cols - 1 && !current.HasWall(Walls.Right) && !visited[r, c + 1])
                {
                    visited[r, c + 1] = true;
                    parent[r, c + 1] = current;
                    stack.Push(maze[r, c + 1]);
                }

                // Arriba
                if (r > 0 && !current.HasWall(Walls.Top) && !visited[r - 1, c])
                {
                    visited[r - 1, c] = true;
                    parent[r - 1, c] = current;
                    stack.Push(maze[r - 1, c]);
                }
            }

            stopwatch.Stop();

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
        /// 
        /// <param name="parent">Matriz donde cada celda tiene su padre</param>
        /// <param name="goal">La celda objetivo desde donde empezamos a reconstruir</param>
        /// <returns>Lista de celdas desde inicio hasta meta</returns>
        private static List<MazeCell> ReconstructPath(MazeCell?[,] parent, MazeCell goal)
        {
            var path = new List<MazeCell>();
            MazeCell? current = goal;

            // Vamos subiendo desde la meta hasta el inicio
            // Cuando current sea null, llegamos al inicio
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