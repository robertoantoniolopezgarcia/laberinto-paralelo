using MazeSolver.Metrics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MazeSolver.Tests;

[TestClass]
public class SolveResultTests
{
    [TestMethod]
    public void Constructor_DebeInicializar_Path_Vacio()
    {
        // Arrange
        var result = new SolveResult();

        // Assert
        Assert.IsNotNull(result.Path);
        Assert.AreEqual(0, result.Path.Count);
    }
}