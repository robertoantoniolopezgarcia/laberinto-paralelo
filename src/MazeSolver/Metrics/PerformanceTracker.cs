using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using MazeSolver.Maze;
using MazeSolver.Solvers;


namespace MazeSolver.Metrics
{
    public class PerformanceTracker
    {
        // En futuras versiones esta clase podrá registrar
        // excepciones, consumo de memoria y uso de CPU.

        public SolveResult Measure(
            ISolver solver,
            MazeCell[,] maze,
            MazeCell start,
            MazeCell goal)
        {
            if (solver == null)
            {
                throw new ArgumentNullException(nameof(solver));
            }

            var stopwatch = Stopwatch.StartNew();

            var result = solver.Solve(maze, start, goal);

            stopwatch.Stop();

            result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            return result;
        }

        // exporta los resultados de los diferentes algoritmos
        // a un archivo CSV para facilitar su comparacion.
        //
        // El archivo contiene:
        // - Nombre del algoritmo.
        // - Si encontro el camino.
        // - Cantidad de nodos visitados.
        // - Longitud del camino.
        // - Tiempo de ejecución en milisegundos.
        public void ExportToCsv(IEnumerable<SolveResult> results, string filePath)
        {
            using var writer = new StreamWriter(filePath);

            writer.WriteLine("Algorithm,PathFound,NodesVisited,PathLength,ElapsedMilliseconds");

            foreach (var result in results)
            {
                writer.WriteLine(
                    $"{result.AlgorithmName}," +
                    $"{result.PathFound}," +
                    $"{result.NodesVisited}," +
                    $"{result.PathLength}," +
                    $"{result.ElapsedMilliseconds}");
            }
        }
    }
}
