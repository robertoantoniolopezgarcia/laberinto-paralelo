using System;
using System.Collections.Generic;

namespace MazeSolver.Maze
{
    /// <summary>
    /// Genera un laberinto resoluble usando el algoritmo de backtracking
    /// aleatorio (randomized DFS): parte de la entrada, va "tumbando" paredes
    /// hacia celdas vecinas no visitadas, y retrocede cuando se topa con un
    /// callejón sin salida, hasta cubrir todas las celdas.
    ///
    /// El resultado queda completamente en memoria como una matriz 2D
    /// (MazeCell[,]) — no se lee ni se escribe nada en disco.
    /// </summary>
    public static class MazeGenerator
    {
        /// <summary>
        /// Genera un laberinto de rows x cols. La entrada es (0,0) y la
        /// salida es (rows-1, cols-1).
        /// </summary>
        /// <param name="rows">Cantidad de filas (alto del laberinto).</param>
        /// <param name="cols">Cantidad de columnas (ancho del laberinto).</param>
        /// <param name="seed">
        /// Semilla opcional para reproducir el mismo laberinto entre corridas
        /// (útil para comparar los 4 algoritmos sobre el mismo laberinto exacto).
        /// </param>
        public static MazeCell[,] Generate(int rows, int cols, int? seed = null)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("El laberinto debe tener al menos 1 fila y 1 columna.");

            var random = seed.HasValue ? new Random(seed.Value) : new Random();
            var grid = new MazeCell[rows, cols];

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    grid[r, c] = new MazeCell(r, c);

            // Backtracking iterativo con pila explícita (evita desbordar el
            // stack de llamadas en laberintos grandes, ej. 1000x1000).
            var stack = new Stack<MazeCell>();
            var start = grid[0, 0];
            start.VisitedDuringGeneration = true;
            stack.Push(start);

            while (stack.Count > 0)
            {
                var current = stack.Peek();
                var unvisitedNeighbors = GetUnvisitedNeighbors(grid, current, rows, cols);

                if (unvisitedNeighbors.Count == 0)
                {
                    // Callejón sin salida: retroceder.
                    stack.Pop();
                    continue;
                }

                var (neighbor, wallToCurrent, wallToNeighbor) =
                    unvisitedNeighbors[random.Next(unvisitedNeighbors.Count)];

                // Tumbar la pared entre la celda actual y la vecina elegida.
                current.RemoveWall(wallToCurrent);
                neighbor.RemoveWall(wallToNeighbor);

                neighbor.VisitedDuringGeneration = true;
                stack.Push(neighbor);
            }

            return grid;
        }

        private static List<(MazeCell neighbor, Walls wallToCurrent, Walls wallToNeighbor)>
            GetUnvisitedNeighbors(MazeCell[,] grid, MazeCell cell, int rows, int cols)
        {
            var result = new List<(MazeCell, Walls, Walls)>(4);

            if (cell.Row > 0 && !grid[cell.Row - 1, cell.Col].VisitedDuringGeneration)
                result.Add((grid[cell.Row - 1, cell.Col], Walls.Top, Walls.Bottom));

            if (cell.Row < rows - 1 && !grid[cell.Row + 1, cell.Col].VisitedDuringGeneration)
                result.Add((grid[cell.Row + 1, cell.Col], Walls.Bottom, Walls.Top));

            if (cell.Col > 0 && !grid[cell.Row, cell.Col - 1].VisitedDuringGeneration)
                result.Add((grid[cell.Row, cell.Col - 1], Walls.Left, Walls.Right));

            if (cell.Col < cols - 1 && !grid[cell.Row, cell.Col + 1].VisitedDuringGeneration)
                result.Add((grid[cell.Row, cell.Col + 1], Walls.Right, Walls.Left));

            return result;
        }
    }
}