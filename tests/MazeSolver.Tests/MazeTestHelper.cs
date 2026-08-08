using MazeSolver.Maze;

namespace MazeSolver.Tests;

/// <summary>
/// Utilidades para crear laberintos pequeños de forma manual en los tests.
/// Al construirlos a mano (sin el generador aleatorio) sabemos exactamente
/// qué caminos existen y podemos verificar resultados de forma determinista.
/// </summary>
internal static class MazeTestHelper
{
    // Crea una cuadricula donde todas las celdas tienen las 4 paredes cerradas.
    // Es el punto de partida: despues abrimos solo los pasos que necesitamos.
    internal static MazeCell[,] CreateBlocked(int rows, int cols)
    {
        var maze = new MazeCell[rows, cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                maze[r, c] = new MazeCell(r, c); // arranca con Walls.All
        return maze;
    }

    // Abre el paso entre dos celdas adyacentes.
    // Elimina la pared de los dos lados para que sea transitable en ambas direcciones.
    internal static void Open(MazeCell[,] maze, int r1, int c1, int r2, int c2)
    {
        var a = maze[r1, c1];
        var b = maze[r2, c2];

        if (r2 == r1 - 1) { a.RemoveWall(Walls.Top);    b.RemoveWall(Walls.Bottom); }
        if (r2 == r1 + 1) { a.RemoveWall(Walls.Bottom); b.RemoveWall(Walls.Top);    }
        if (c2 == c1 - 1) { a.RemoveWall(Walls.Left);   b.RemoveWall(Walls.Right);  }
        if (c2 == c1 + 1) { a.RemoveWall(Walls.Right);  b.RemoveWall(Walls.Left);   }
    }

    // Un pasillo recto de 3 celdas: [0,0] - [0,1] - [0,2]
    // Hay un solo camino posible y mide 3 celdas.
    internal static MazeCell[,] CreateLinear1x3()
    {
        var maze = CreateBlocked(1, 3);
        Open(maze, 0, 0, 0, 1);
        Open(maze, 0, 1, 0, 2);
        return maze;
    }

    // Un laberinto 3x3 con forma de L:
    //
    //   [0,0] - [0,1] - [0,2]
    //                    |
    //                   [1,2]
    //                    |
    //                   [2,2]
    //
    // Solo hay un camino: (0,0) -> (0,1) -> (0,2) -> (1,2) -> (2,2), 5 celdas.
    internal static MazeCell[,] CreateLShape3x3()
    {
        var maze = CreateBlocked(3, 3);
        Open(maze, 0, 0, 0, 1);
        Open(maze, 0, 1, 0, 2);
        Open(maze, 0, 2, 1, 2);
        Open(maze, 1, 2, 2, 2);
        return maze;
    }

    // Un laberinto 3x3 con dos rutas posibles entre (0,0) y (2,2):
    //
    //   [0,0] - [0,1] - [0,2]
    //    |                |
    //   [1,0]            [1,2]
    //    |                |
    //   [2,0] - [2,1] - [2,2]
    //
    // Las dos rutas tienen 5 celdas. Sirve para comparar que BFS y DFS
    // llegan a la meta aunque tomen caminos distintos.
    internal static MazeCell[,] CreateTwoPaths3x3()
    {
        var maze = CreateBlocked(3, 3);
        Open(maze, 0, 0, 1, 0);
        Open(maze, 1, 0, 2, 0);
        Open(maze, 2, 0, 2, 1);
        Open(maze, 2, 1, 2, 2);
        Open(maze, 0, 0, 0, 1);
        Open(maze, 0, 1, 0, 2);
        Open(maze, 0, 2, 1, 2);
        Open(maze, 1, 2, 2, 2);
        return maze;
    }

    // Un 2x2 donde no se puede ir a ninguna parte.
    // Util para verificar que el solver dice PathFound = false correctamente.
    internal static MazeCell[,] CreateFullyBlocked2x2()
    {
        return CreateBlocked(2, 2); // Todas las paredes cerradas
    }
}
