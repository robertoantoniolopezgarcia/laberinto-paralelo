using System;
using MazeSolver.Maze;
using MazeSolver.Metrics;

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
            throw new NotImplementedException();
        }
    }
}

