using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeSolver.Maze;

namespace MazeSolver.Tests
{
    [TestClass]
    public class MazeGeneratorTests
    {
        [TestMethod]
        public void Generate_ReturnsMaze()
        {
            // Arrange
            int rows = 5;
            int cols = 5;

            // Act
            MazeCell[,] maze = MazeGenerator.Generate(rows, cols);

            // Assert
            Assert.IsNotNull(maze);
        }

        [TestMethod]
        public void Generate_CorrectDimensions()
        {
            // Arrange
            int rows = 5;
            int cols = 7;

            // Act
            MazeCell[,] maze = MazeGenerator.Generate(rows, cols);

            // Assert
            Assert.AreEqual(rows, maze.GetLength(0));
            Assert.AreEqual(cols, maze.GetLength(1));
        }

        [TestMethod]
        public void Generate_AllCellsVisited()
        {
            // Arrange
            int rows = 5;
            int cols = 5;

            // Act
            MazeCell[,] maze = MazeGenerator.Generate(rows, cols);

            // Assert
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Assert.IsTrue(
                        maze[row, col].VisitedDuringGeneration,
                        $"La celda ({row},{col}) no fue visitada durante la generacion.");
                }
            }
        }

        [TestMethod]
        public void Generate_MazeIsSolvable()
        {
            // Arrange
            int rows = 5;
            int cols = 5;

            // Act
            MazeCell[,] maze = MazeGenerator.Generate(rows, cols);

            MazeCell start = maze[0, 0];
            MazeCell goal = maze[rows - 1, cols - 1];

            Queue<MazeCell> queue = new Queue<MazeCell>();
            bool[,] visited = new bool[rows, cols];

            queue.Enqueue(start);
            visited[start.Row, start.Col] = true;

            bool pathFound = false;

            while (queue.Count > 0)
            {
                MazeCell current = queue.Dequeue();

                if (current.Row == goal.Row && current.Col == goal.Col)
                {
                    pathFound = true;
                    break;
                }

                // Arriba
                if (!current.HasWall(Walls.Top))
                {
                    int newRow = current.Row - 1;
                    int newCol = current.Col;

                    if (newRow >= 0 &&
                        newRow < rows &&
                        newCol >= 0 &&
                        newCol < cols &&
                        !visited[newRow, newCol])
                    {
                        visited[newRow, newCol] = true;
                        queue.Enqueue(maze[newRow, newCol]);
                    }
                }

                // Abajo
                if (!current.HasWall(Walls.Bottom))
                {
                    int newRow = current.Row + 1;
                    int newCol = current.Col;

                    if (newRow >= 0 &&
                        newRow < rows &&
                        newCol >= 0 &&
                        newCol < cols &&
                        !visited[newRow, newCol])
                    {
                        visited[newRow, newCol] = true;
                        queue.Enqueue(maze[newRow, newCol]);
                    }
                }

                // Izquierda
                if (!current.HasWall(Walls.Left))
                {
                    int newRow = current.Row;
                    int newCol = current.Col - 1;

                    if (newRow >= 0 &&
                        newRow < rows &&
                        newCol >= 0 &&
                        newCol < cols &&
                        !visited[newRow, newCol])
                    {
                        visited[newRow, newCol] = true;
                        queue.Enqueue(maze[newRow, newCol]);
                    }
                }

                // Derecha
                if (!current.HasWall(Walls.Right))
                {
                    int newRow = current.Row;
                    int newCol = current.Col + 1;

                    if (newRow >= 0 &&
                        newRow < rows &&
                        newCol >= 0 &&
                        newCol < cols &&
                        !visited[newRow, newCol])
                    {
                        visited[newRow, newCol] = true;
                        queue.Enqueue(maze[newRow, newCol]);
                    }
                }
            }

            // Assert
            Assert.IsTrue(
                pathFound,
                "El laberinto generado deberia tener un camino desde la entrada hasta la salida.");
        }

        [TestMethod]
        public void Generate_WithOneCell_ReturnsSingleCellMaze()
        {
            // Arrange
            int rows = 1;
            int cols = 1;

            // Act
            MazeCell[,] maze = MazeGenerator.Generate(rows, cols);

            // Assert
            Assert.IsNotNull(maze);
            Assert.AreEqual(1, maze.GetLength(0));
            Assert.AreEqual(1, maze.GetLength(1));
        }
    }
}