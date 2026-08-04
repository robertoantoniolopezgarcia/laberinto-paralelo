using System.Collections.Generic;
using MazeSolver.Maze;
using MazeSolver.Metrics;
using MazeSolver.Solvers;

namespace MazeSolver.Orchestration
{
    /// <summary>
    /// Recibe una lista de algoritmos (ISolver) y los ejecuta sobre el mismo
    /// laberinto. Esta clase es el punto central donde se conecta el trabajo
    /// de todos: el generador de laberinto, los solvers, y las métricas.
    ///
    /// Ver docs/EstrategiaParalelizacion.md para el detalle de la estrategia
    /// de paralelización (TPL) que se implementa en el próximo commit.
    /// </summary>
    public static class SolverOrchestrator
    {
        /// <summary>
        /// Punto de entrada del orquestador. Por ahora ejecuta los solvers
        /// de forma secuencial (uno detrás de otro) — en el próximo commit
        /// se agrega la versión paralela con Task Parallel Library.
        /// </summary>
        public static Dictionary<string, SolveResult> RunAll(
            MazeCell[,] maze,
            MazeCell start,
            MazeCell goal,
            IEnumerable<ISolver> solvers)
        {
            var results = new Dictionary<string, SolveResult>();

            foreach (var solver in solvers)
            {
                results[solver.Name] = solver.Solve(maze, start, goal);
            }

            return results;
        }
    }
}