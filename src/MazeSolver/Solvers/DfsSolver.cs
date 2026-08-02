using System.Collections.Generic;
using MazeSolver.Maze;
using MazeSolver.Metrics;

namespace MazeSolver.Solvers
{
    /// <summary>
    /// Búsqueda en Profundidad (DFS) para explorar laberintos.
    /// </summary>
    public class DfsSolver : ISolver
    {
        public string Name => "DFS";

        /// <summary>
        /// Ejecuta el algoritmo DFS sobre el laberinto.
        /// </summary>
        /// <param name="maze">Laberinto representado como matriz de celdas.</param>
        /// <param name="start">Celda donde comienza la búsqueda.</param>
        /// <param name="goal">Celda objetivo.</param>
        /// <returns>Resultado con la ruta encontrada o indicador de fallo.</returns>
        public SolveResult Solve(MazeCell[,] maze, MazeCell start, MazeCell goal)
        {
            // Pendiente: usar Stack<MazeCell> para la exploración en profundidad
            throw new NotImplementedException();
        }
    }
}