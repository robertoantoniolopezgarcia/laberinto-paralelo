using System.Collections.Generic;
using MazeSolver.Maze;
using MazeSolver.Metrics;

namespace MazeSolver.Solvers
{
    /// <summary>
    /// Busqueda en Anchura BFS para encontrar el camino mas corto en un laberinto.
    /// </summary>
    public class BfsSolver : ISolver
    {
        public string Name => "BFS";

        /// <summary>
        /// Ejecuta el algoritmo BFS sobre el laberinto.
        /// </summary>
        /// <param name="maze">Laberinto representado como matriz de celdas.</param>
        /// <param name="start">Celda donde comienza la búsqueda.</param>
        /// <param name="goal">Celda objetivo.</param>
        /// <returns>Resultado con la ruta encontrada o indicador de fallo.</returns>
        public SolveResult Solve(MazeCell[,] maze, MazeCell start, MazeCell goal)
        {
            // Por ahora solo esta la estructura base
            // La logica de exploracion con cola y reconstruccion de ruta va aqui.
            throw new NotImplementedException();
        }
    }
}