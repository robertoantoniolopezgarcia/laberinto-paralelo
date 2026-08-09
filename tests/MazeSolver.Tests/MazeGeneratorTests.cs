using MazeSolver.Maze;
using MazeSolver.Solvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MazeSolver.Tests
{
    [TestClass]
    public class MazeGeneratorTests
    {
        // Valida que el generador cree el laberinto con las dimensiones solicitadas.
        [TestMethod]
        public void Generate_ShouldCreateMazeWithCorrectDimensions()
        {
            int rows = 10;
            int cols = 15;

            var maze = MazeGenerator.Generate(rows, cols, seed: 42);

            Assert.AreEqual(rows, maze.GetLength(0));
            Assert.AreEqual(cols, maze.GetLength(1));
        }

        [TestMethod]
        public void Generate_ShouldBeResolvable()
        {
            int size = 20;

            var maze = MazeGenerator.Generate(size, size, seed: 42);

            var start = maze[0, 0];
            var goal = maze[size - 1, size - 1];

            var solver = new BfsSolver();

            var result = solver.Solve(maze, start, goal);

            Assert.IsTrue(result.PathFound);
            Assert.IsTrue(result.PathLength > 0);
        }

        [TestMethod]
        public void Generate_ShouldBeResolvableWithDifferentSeeds()
        {
            int size = 20;
            int[] seeds = { 1, 10, 42, 100, 999 };

            foreach (int seed in seeds)
            {
                var maze = MazeGenerator.Generate(size, size, seed);

                var start = maze[0, 0];
                var goal = maze[size - 1, size - 1];

                var solver = new BfsSolver();

                var result = solver.Solve(maze, start, goal);

                Assert.IsTrue(
                    result.PathFound,
                    $"El laberinto con seed={seed} no es resoluble.");
            }
        }

        // Comprueba que la generacion siga siendo resoluble en diferentes tama;os.
        [TestMethod]
        public void Generate_ShouldBeResolvableForDifferentSizes()
        {
            int[] sizes = { 1, 2, 5, 10, 20 };

            foreach (int size in sizes)
            {
                var maze = MazeGenerator.Generate(size, size, seed: 42);

                var start = maze[0, 0];
                var goal = maze[size - 1, size - 1];

                var solver = new BfsSolver();

                var result = solver.Solve(maze, start, goal);

                Assert.IsTrue(
                    result.PathFound,
                    $"El laberinto de tamaño {size}x{size} no es resoluble.");
            }
        }
    }
}