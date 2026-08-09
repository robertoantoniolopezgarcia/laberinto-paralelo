using System;
using System.Diagnostics;
using System.IO;
using MazeSolver.Maze;
using MazeSolver.Orchestration;
using MazeSolver.Solvers;
using MazeSolver.Metrics;

namespace MazeSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tama;o del laberinto configurable (default 1000x1000).
            int size = args.Length > 0 ? int.Parse(args[0]) : 1000;

            Console.WriteLine($"Generando laberinto de {size}x{size}...");
            var stopwatch = Stopwatch.StartNew();

            var maze = MazeGenerator.Generate(size, size, seed: 42);

            stopwatch.Stop();
            Console.WriteLine($"Laberinto generado en {stopwatch.ElapsedMilliseconds} ms");

            var start = maze[0, 0];
            var goal = maze[size - 1, size - 1];
            Console.WriteLine($"Entrada: {start}  Salida: {goal}");

            // Se crean las diferentes estrategias de resolucion
            // que seran ejecutadas por el orquestador paralelo.

            var solvers = new ISolver[]
            {
    new BfsSolver(),
    new DfsSolver(),
    new AStarSolver(),
    new DijkstraSolver()
            };

            // Ejecutar todos los algoritmos en paralelo.
            var results = SolverOrchestrator.RunAll(
                maze,
                start,
                goal,
                solvers);

            Console.WriteLine();
            Console.WriteLine("Resultados de los solvers:");

            // Se muestran en consola las metricas obtenidas por cada algoritmo:
            // si encontro el camino, nodos visitados, longitud del camino
            // y tiempo de ejecucion.

            foreach (var result in results)
            {
                Console.WriteLine(
                    $"{result.Key} | " +
                    $"PathFound: {result.Value.PathFound} | " +
                    $"NodesVisited: {result.Value.NodesVisited} | " +
                    $"PathLength: {result.Value.PathLength} | " +
                    $"Time: {result.Value.ElapsedMilliseconds} ms");
            }

            var tracker = new PerformanceTracker();

            var metricsDirectory = Path.Combine(
                AppContext.BaseDirectory,
                "metrics");

            Directory.CreateDirectory(metricsDirectory);

            var csvPath = Path.Combine(
                metricsDirectory,
                "solver_results.csv");

            tracker.ExportToCsv(
                results.Values,
                csvPath);

            Console.WriteLine();
            Console.WriteLine($"Resultados exportados a: {csvPath}");

        }
    }
}