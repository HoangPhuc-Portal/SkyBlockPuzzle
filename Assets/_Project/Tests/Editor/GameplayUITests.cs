using System;
using System.Reflection;
using NUnit.Framework;
public sealed class GameplayUITests
{
    [Test]
    public void MissingFillSpriteDoesNotBreakGhostOrPlacement()
    {
        var type = Type.GetType("UIRegressionChecks, Assembly-CSharp-Editor", true);
        try
        {
            string result = (string)type.GetMethod("GhostFallback").Invoke(null, null);
            StringAssert.StartsWith("PASS:", result);
            TestContext.WriteLine(result);
        }
        catch (TargetInvocationException error) { throw error.InnerException ?? error; }
    }

    [Test]
    public void BoardModelFindsClearPriorityWithoutMutatingState()
    {
        var type = Type.GetType("UIRegressionChecks, Assembly-CSharp-Editor", true);
        try
        {
            string result = (string)type.GetMethod("Model").Invoke(null, null);
            StringAssert.StartsWith("PASS:", result);
            TestContext.WriteLine(result);
        }
        catch (TargetInvocationException error) { throw error.InnerException ?? error; }
    }
}
