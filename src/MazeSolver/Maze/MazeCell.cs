using System;

namespace MazeSolver.Maze
{
    /// <summary>
    /// Representa las paredes de una celda. Se puede combinar con OR bit a bit
    /// (ej: Top | Left significa que la celda tiene pared arriba y a la izquierda).
    /// </summary>
    [Flags]
    public enum Walls
    {
        None = 0,
        Top = 1,
        Right = 2,
        Bottom = 4,
        Left = 8,
        All = Top | Right | Bottom | Left
    }

    /// <summary>
    /// Una celda individual del laberinto. Todas las celdas empiezan con las
    /// 4 paredes puestas; el generador las va "tumbando" para abrir caminos.
    /// </summary>
    public class MazeCell
    {
        public int Row { get; }
        public int Col { get; }
        public Walls Walls { get; set; } = Walls.All;

        // Usado únicamente durante la GENERACIÓN del laberinto (backtracking),
        // no confundir con el "visitado" que use cada algoritmo de búsqueda:
        // cada solver debe llevar su propio registro de visitados aparte.
        public bool VisitedDuringGeneration { get; set; } = false;

        public MazeCell(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public bool HasWall(Walls wall) => (Walls & wall) == wall;

        public void RemoveWall(Walls wall) => Walls &= ~wall;

        public override string ToString() => $"({Row},{Col})";
    }
}