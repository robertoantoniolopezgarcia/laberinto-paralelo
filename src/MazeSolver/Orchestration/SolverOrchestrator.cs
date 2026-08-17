using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        /// <param name="maxDegreeOfParallelism">
        /// Cantidad máxima de solvers que pueden correr al mismo tiempo.
        /// Si no se especifica, no hay límite (corren todos a la vez).
        /// El límite real nunca puede superar la cantidad de solvers recibidos,
        /// ya que pedir más paralelismo del que hay trabajo disponible no
        /// tiene ningún efecto.
        /// </param>
        public static ConcurrentDictionary<string, SolveResult> RunAll(
            MazeCell[,] maze,
            MazeCell start,
            MazeCell goal,
            IEnumerable<ISolver> solvers,
            int maxDegreeOfParallelism = int.MaxValue)
        {
            var solverList = solvers as IList<ISolver> ?? solvers.ToList();
            var results = new ConcurrentDictionary<string, SolveResult>();

            // El límite real nunca supera la cantidad de solvers: pedir más
            // núcleos que tareas disponibles no aporta ningún beneficio.
            var limit = Math.Min(maxDegreeOfParallelism, solverList.Count);
            using var semaphore = new SemaphoreSlim(limit);

            var tasks = solverList.Select(solver => Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var result = solver.Solve(maze, start, goal);
                    results[solver.Name] = result;
                }
                finally
                {
                    semaphore.Release();
                }
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