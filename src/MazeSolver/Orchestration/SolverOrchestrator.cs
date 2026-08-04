using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazeSolver.Maze;
using MazeSolver.Metrics;
using MazeSolver.Solvers;

namespace MazeSolver.Orchestration
{
    /// <summary>
    /// Lanza todos los algoritmos de búsqueda (que implementan ISolver) como
    /// tareas paralelas independientes sobre el mismo laberinto, y centraliza
    /// sus resultados en una estructura compartida thread-safe.
    ///
    /// Ver docs/EstrategiaParalelizacion.md para el detalle de por qué se
    /// eligió Task.Run + Task.WaitAll y ConcurrentDictionary.
    /// </summary>
    public static class SolverOrchestrator
    {
        /// <summary>
        /// Corre todos los solvers recibidos en paralelo (TPL) y devuelve un
        /// diccionario thread-safe con el resultado de cada uno, indexado
        /// por el nombre del algoritmo (ISolver.Name).
        /// </summary>
        public static ConcurrentDictionary<string, SolveResult> RunAll(
            MazeCell[,] maze,
            MazeCell start,
            MazeCell goal,
            IEnumerable<ISolver> solvers)
        {
            var results = new ConcurrentDictionary<string, SolveResult>();

            var tasks = solvers.Select(solver => Task.Run(() =>
            {
                var result = solver.Solve(maze, start, goal);
                results[solver.Name] = result;
            })).ToArray();

            Task.WaitAll(tasks);

            return results;
        }

        /// <summary>
        /// Corre todos los solvers de forma secuencial, uno detrás de otro.
        /// Sirve como referencia para comparar tiempos contra RunAll (paralelo)
        /// y calcular speedup/eficiencia.
        /// </summary>
        public static Dictionary<string, SolveResult> RunSequential(
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