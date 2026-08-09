using MazeSolver.Maze;
using MazeSolver.Solvers;

namespace MazeSolver.Tests;

// Tests para BfsSolver.
// Cada test verifica un comportamiento especifico usando laberintos
// pequeños que construimos a mano en MazeTestHelper.
[TestClass]
public class BfsSolverTests
{
    private readonly BfsSolver _solver = new();

    // si el laberinto es null no deberia explotar, solo devolver PathFound = false

    [TestMethod]
    [Description("Si el laberinto es null, devuelve PathFound = false sin lanzar excepcion.")]
    public void Solve_MazeNull_DebeRetornar_PathFoundFalse()
    {
        var cell = new MazeCell(0, 0);
        var result = _solver.Solve(null!, cell, cell);

        Assert.IsFalse(result.PathFound);
        Assert.AreEqual(0, result.Path.Count);
    }

    [TestMethod]
    [Description("Si start es null, devuelve PathFound = false sin lanzar excepcion.")]
    public void Solve_StartNull_DebeRetornar_PathFoundFalse()
    {
        var maze = MazeTestHelper.CreateLinear1x3();
        var result = _solver.Solve(maze, null!, maze[0, 2]);

        Assert.IsFalse(result.PathFound);
    }

    [TestMethod]
    [Description("Si goal es null, devuelve PathFound = false sin lanzar excepcion.")]
    public void Solve_GoalNull_DebeRetornar_PathFoundFalse()
    {
        var maze = MazeTestHelper.CreateLinear1x3();
        var result = _solver.Solve(maze, maze[0, 0], null!);

        Assert.IsFalse(result.PathFound);
    }

    // si alguien pasa una celda con coordenadas imposibles tampoco deberia explotar

    [TestMethod]
    [Description("Si start esta fuera del laberinto, devuelve PathFound = false.")]
    public void Solve_StartFueraDelLaberinto_DebeRetornar_PathFoundFalse()
    {
        var maze = MazeTestHelper.CreateLinear1x3();
        var fueraDeRango = new MazeCell(99, 99);

        var result = _solver.Solve(maze, fueraDeRango, maze[0, 2]);

        Assert.IsFalse(result.PathFound);
    }

    [TestMethod]
    [Description("Si goal esta fuera del laberinto, devuelve PathFound = false.")]
    public void Solve_GoalFueraDelLaberinto_DebeRetornar_PathFoundFalse()
    {
        var maze = MazeTestHelper.CreateLinear1x3();
        var fueraDeRango = new MazeCell(99, 99);

        var result = _solver.Solve(maze, maze[0, 0], fueraDeRango);

        Assert.IsFalse(result.PathFound);
    }

    // si el inicio y la meta son la misma celda, ya llegamos sin movernos

    [TestMethod]
    [Description("Si inicio y meta son la misma celda, devuelve PathFound = true con un camino de 1 celda.")]
    public void Solve_InicioIgualMeta_DebeRetornar_CaminoDeUnaCelda()
    {
        var maze = MazeTestHelper.CreateLinear1x3();
        var start = maze[0, 1];

        var result = _solver.Solve(maze, start, start);

        Assert.IsTrue(result.PathFound);
        Assert.AreEqual(1, result.Path.Count);
        Assert.AreEqual(start.Row, result.Path[0].Row);
        Assert.AreEqual(start.Col, result.Path[0].Col);
    }

    // cuando no hay ningun paso abierto, no existe solucion posible

    [TestMethod]
    [Description("En un laberinto completamente bloqueado, devuelve PathFound = false.")]
    public void Solve_LaberintoSinSolucion_DebeRetornar_PathFoundFalse()
    {
        var maze = MazeTestHelper.CreateFullyBlocked2x2();

        var result = _solver.Solve(maze, maze[0, 0], maze[1, 1]);

        Assert.IsFalse(result.PathFound);
        Assert.AreEqual(0, result.Path.Count);
    }

    // pasillo recto 1x3: hay un solo camino y lo conocemos de antemano

    [TestMethod]
    [Description("En un pasillo recto de 3 celdas, encuentra el camino correcto paso a paso.")]
    public void Solve_LaberintoLineal_DebeEncontrar_CaminoCorrecto()
    {
        var maze  = MazeTestHelper.CreateLinear1x3();
        var start = maze[0, 0];
        var goal  = maze[0, 2];

        var result = _solver.Solve(maze, start, goal);

        Assert.IsTrue(result.PathFound);
        Assert.AreEqual(3, result.Path.Count);
        Assert.AreEqual(0, result.Path[0].Row); Assert.AreEqual(0, result.Path[0].Col);
        Assert.AreEqual(0, result.Path[1].Row); Assert.AreEqual(1, result.Path[1].Col);
        Assert.AreEqual(0, result.Path[2].Row); Assert.AreEqual(2, result.Path[2].Col);
    }

    [TestMethod]
    [Description("El camino devuelto empieza siempre en la celda de inicio.")]
    public void Solve_LaberintoLineal_Camino_DebeEmpezar_EnElInicio()
    {
        var maze  = MazeTestHelper.CreateLinear1x3();
        var start = maze[0, 0];
        var goal  = maze[0, 2];

        var result = _solver.Solve(maze, start, goal);

        Assert.IsTrue(result.PathFound);
        Assert.AreEqual(start.Row, result.Path[0].Row);
        Assert.AreEqual(start.Col, result.Path[0].Col);
    }

    [TestMethod]
    [Description("El camino devuelto termina siempre en la celda meta.")]
    public void Solve_LaberintoLineal_Camino_DebeTerminar_EnLaMeta()
    {
        var maze  = MazeTestHelper.CreateLinear1x3();
        var start = maze[0, 0];
        var goal  = maze[0, 2];

        var result = _solver.Solve(maze, start, goal);

        Assert.IsTrue(result.PathFound);
        var last = result.Path[^1];
        Assert.AreEqual(goal.Row, last.Row);
        Assert.AreEqual(goal.Col, last.Col);
    }

    // laberinto en L: solo existe una ruta, BFS tiene que encontrarla si o si

    [TestMethod]
    [Description("Cuando hay un unico camino en forma de L, BFS lo encuentra con 5 celdas.")]
    public void Solve_LaberintoEnL_DebeEncontrar_ElCaminoUnico()
    {
        var maze  = MazeTestHelper.CreateLShape3x3();
        var start = maze[0, 0];
        var goal  = maze[2, 2];

        var result = _solver.Solve(maze, start, goal);

        Assert.IsTrue(result.PathFound);
        Assert.AreEqual(5, result.Path.Count);
    }

    // con dos caminos de igual longitud, BFS siempre devuelve uno de 5 celdas

    [TestMethod]
    [Description("Con dos caminos disponibles de igual longitud, BFS encuentra uno de 5 celdas.")]
    public void Solve_DosCaminos_DebeEncontrar_UnCaminoValido()
    {
        var maze  = MazeTestHelper.CreateTwoPaths3x3();
        var start = maze[0, 0];
        var goal  = maze[2, 2];

        var result = _solver.Solve(maze, start, goal);

        Assert.IsTrue(result.PathFound);
        Assert.AreEqual(5, result.Path.Count);
    }

    // comprobaciones rapidas de las metricas

    [TestMethod]
    [Description("AlgorithmName debe ser BFS.")]
    public void Solve_AlgorithmName_DebeSerBFS()
    {
        var maze   = MazeTestHelper.CreateLinear1x3();
        var result = _solver.Solve(maze, maze[0, 0], maze[0, 2]);

        Assert.AreEqual("BFS", result.AlgorithmName);
    }

    [TestMethod]
    [Description("NodesVisited debe ser mayor que cero cuando se encuentra un camino.")]
    public void Solve_CuandoEncuentraCamino_NodesVisited_DebeSerMayorQueCero()
    {
        var maze   = MazeTestHelper.CreateLinear1x3();
        var result = _solver.Solve(maze, maze[0, 0], maze[0, 2]);

        Assert.IsTrue(result.NodesVisited > 0);
    }

    [TestMethod]
    [Description("Cuando no hay camino, NodesVisited igual refleja los nodos que se intentaron.")]
    public void Solve_SinSolucion_NodesVisited_DebeSerMayorQueCero()
    {
        var maze   = MazeTestHelper.CreateFullyBlocked2x2();
        var result = _solver.Solve(maze, maze[0, 0], maze[1, 1]);

        Assert.IsTrue(result.NodesVisited >= 1);
    }
}
