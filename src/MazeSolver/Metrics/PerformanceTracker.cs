using System;
using System.Diagnostics;
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
    }
}
