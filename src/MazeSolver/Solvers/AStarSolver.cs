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
            throw new NotImplementedException();
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
    }
}
