using MazeSolver.Maze;
using MazeSolver.Solvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MazeSolver.Tests;

[TestClass]
public class DijkstraSolverTests
{
    [TestMethod]
    public void Solve_DebeEncontrarUnCamino()
    {
        // Arrange
        var maze = MazeGenerator.Generate(5, 5, seed: 42);

        var start = maze[0, 0];
        var goal = maze[4, 4];

        var solver = new DijkstraSolver();

        // Act
        var result = solver.Solve(maze, start, goal);

        // Assert
        Assert.IsTrue(result.PathFound);
        Assert.IsNotNull(result.Path);
        Assert.IsTrue(result.Path.Count > 0);
    }
}
