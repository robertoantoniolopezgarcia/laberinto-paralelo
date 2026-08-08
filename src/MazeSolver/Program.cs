using System;
using System.Diagnostics;
using MazeSolver.Maze;
using MazeSolver.Orchestration;
using MazeSolver.Solvers;

namespace MazeSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tamaño del laberinto configurable (default 1000x1000).
            int size = args.Length > 0 ? int.Parse(args[0]) : 1000;

            Console.WriteLine($"Generando laberinto de {size}x{size}...");
            var stopwatch = Stopwatch.StartNew();

            var maze = MazeGenerator.Generate(size, size, seed: 42);

            stopwatch.Stop();
            Console.WriteLine($"Laberinto generado en {stopwatch.ElapsedMilliseconds} ms");

            var start = maze[0, 0];
            var goal = maze[size - 1, size - 1];
            Console.WriteLine($"Entrada: {start}  Salida: {goal}");

            // TODO (Gabriel): reemplazar por el orquestador paralelo real.
            // var solvers = new ISolver[] { new BfsSolver(), new DfsSolver(), new AStarSolver(), new DijkstraSolver() };
            // var results = SolverOrchestrator.RunAll(maze, start, goal, solvers);

            // TODO (Mirelys): imprimir/exportar la comparativa de resultados.
            // PerformanceTracker.PrintSummary(results);
            // Solvers que se ejecutarán en paralelo.
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

            foreach (var result in results)
            {
                Console.WriteLine(
                    $"{result.Key} | " +
                    $"PathFound: {result.Value.PathFound} | " +
                    $"NodesVisited: {result.Value.NodesVisited} | " +
                    $"PathLength: {result.Value.PathLength} | " +
                    $"Time: {result.Value.ElapsedMilliseconds} ms");
            }

        }
    }
}