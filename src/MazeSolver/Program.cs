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
            // Tamaño del laberinto configurable (default 1000x1000).
            //Revisa que el usuario haya ingresado un argumento, si no lo hizo se usa 1000 x 1000
            int size = args.Length > 0 ? int.Parse(args[0]) : 1000;

            // Se crean las diferentes estrategias de resolucion
            // que seran ejecutadas por el orquestador paralelo.
            // Se crean antes de leer el segundo argumento porque su cantidad
            // define el máximo de núcleos que tiene sentido usar.

            var solvers = new ISolver[]
            {
                new BfsSolver(),
                new DfsSolver(),
                new AStarSolver(),
                new DijkstraSolver()
            };

            // Núcleos a usar en la ejecución paralela (segundo argumento,
            // opcional). Mínimo 2, porque con 1 núcleo el resultado sería
            // prácticamente igual al modo secuencial, que ya se mide aparte.
            // Máximo la cantidad de solvers, porque pedir más núcleos que
            // tareas disponibles no tiene ningún efecto adicional.
            int minCores = 2;
            int maxCores = solvers.Length;
            int cores = maxCores;

            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out cores) || cores < minCores || cores > maxCores)
                {
                    Console.WriteLine($"Núcleos inválidos. Debe ser un número entre {minCores} y {maxCores}.");
                    return;
                }
            }

            Console.WriteLine($"Generando laberinto de {size}x{size}...");
            Console.WriteLine($"Núcleos a usar en la ejecución paralela: {cores} (de un máximo útil de {maxCores})");
            var stopwatch = Stopwatch.StartNew();

            var maze = MazeGenerator.Generate(size, size, seed: 42);

            stopwatch.Stop();
            Console.WriteLine($"Laberinto generado en {stopwatch.ElapsedMilliseconds} ms");

            var start = maze[0, 0];
            var goal = maze[size - 1, size - 1];
            Console.WriteLine($"Entrada: {start}  Salida: {goal}");

            // --- Ejecucion SECUENCIAL (uno detras de otro) ---
            // Sirve de referencia para poder calcular speedup y eficiencia
            // contra la version paralela.
            var sequentialStopwatch = Stopwatch.StartNew();
            var sequentialResults = SolverOrchestrator.RunSequential(
                maze,
                start,
                goal,
                solvers);
            sequentialStopwatch.Stop();

            // --- Ejecucion PARALELA (limitada a la cantidad de nucleos elegida) ---
            var parallelStopwatch = Stopwatch.StartNew();
            var results = SolverOrchestrator.RunAll(
                maze,
                start,
                goal,
                solvers,
                cores);
            parallelStopwatch.Stop();

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

            // --- Comparativa secuencial vs paralelo ---
            double speedup = (double)sequentialStopwatch.ElapsedMilliseconds / parallelStopwatch.ElapsedMilliseconds;
            double efficiency = speedup / cores;

            Console.WriteLine();
            Console.WriteLine("Comparativa secuencial vs paralelo:");
            Console.WriteLine($"Núcleos usados:          {cores}");
            Console.WriteLine($"Tiempo total secuencial: {sequentialStopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Tiempo total paralelo:   {parallelStopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Speedup:    {speedup:F2}x");
            Console.WriteLine($"Eficiencia: {efficiency:P1}");

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