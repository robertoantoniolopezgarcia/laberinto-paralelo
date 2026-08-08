using MazeSolver.Maze;
using MazeSolver.Metrics;

namespace MazeSolver.Metrics
{
    /// <summary>
    /// Resultado que devuelve un solver al terminar de buscar el camino.
    /// Esta es la base del contrato: Mirelys la va a completar con lo que
    /// necesite el módulo de métricas (ej. más estadísticas, serialización
    /// a CSV, etc.), pero la forma base no debería cambiar para no romper
    /// a los solvers que ya la usan.
    /// </summary>
    public class SolveResult
    {
        public string AlgorithmName { get; set; } = string.Empty;
        public bool PathFound { get; set; }
        public List<MazeCell> Path { get; set; } = new();
        public int NodesVisited { get; set; }
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// Longitud del camino encontrado.
        /// </summary>
        public int PathLength => Path.Count;

        public override string ToString()
        {
            return $"{AlgorithmName} | " +
                   $"PathFound: {PathFound} | " +
                   $"NodesVisited: {NodesVisited} | " +
                   $"PathLength: {PathLength} | " +
                   $"Time: {ElapsedMilliseconds} ms";
        }
    }
}