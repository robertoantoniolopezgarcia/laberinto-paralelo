

namespace MazeSolver.Solvers
{
    /// <summary>
    /// Contrato común que deben cumplir todos los algoritmos de búsqueda
    /// (BFS, DFS, A*, Dijkstra). Cada uno recibe el mismo laberinto y la
    /// misma entrada/salida, y devuelve un SolveResult con el camino
    /// encontrado y sus métricas.
    ///
    /// Al programar contra esta interfaz, el orquestador paralelo puede
    /// tratar a los 4 algoritmos de forma genérica (ej. una lista de
    /// ISolver, cada uno corriendo en su propia Task), sin importarle
    /// los detalles internos de cada uno.
    /// </summary>
    public interface ISolver
    {
        /// <summary>Nombre descriptivo del algoritmo (ej. "BFS", "A*").</summary>
        string Name { get; }

        /// <summary>
        /// Busca el camino más corto (o el que encuentre, según el algoritmo)
        /// entre 'start' y 'goal' dentro del laberinto dado.
        /// </summary>
        /// <param name="maze">Matriz del laberinto ya generado.</param>
        /// <param name="start">Celda de entrada.</param>
        /// <param name="goal">Celda de salida.</param>
        SolveResult Solve(MazeCell[,] maze, MazeCell start, MazeCell goal);
    }
}