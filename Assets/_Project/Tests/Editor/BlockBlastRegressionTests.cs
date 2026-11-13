using System;
using System.Reflection;
using NUnit.Framework;

public class BlockBlastRegressionTests
{
    [Test]
    public void PlacementPreviewClearAndScoreRemainSynchronized()
    {
        // Runtime scripts use Unity's predefined assembly. The test assembly invokes the
        // Editor harness by reflection rather than changing gameplay assembly boundaries.
        var harness = Type.GetType("BlockBlastRegressionChecks, Assembly-CSharp-Editor", true);
        try
        {
            var result = (string)harness.GetMethod("Run", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
            StringAssert.StartsWith("PASS (8 groups)", result);
            TestContext.WriteLine(result);
        }
        catch (TargetInvocationException exception)
        {
            throw exception.InnerException ?? exception;
        }
    }
}
