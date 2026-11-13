using System;
using System.Reflection;
using NUnit.Framework;

public class BubblePopTests
{
    [TestCase("SpriteSelection")]
    [TestCase("EffectPool")]
    [TestCase("ClearSnapshots")]
    public void RandomSpritesAndBubblePopRemainIndependentOfGameplay(string check)
    {
        var type = Type.GetType("BubblePopRegressionChecks, Assembly-CSharp-Editor", true);
        try
        {
            var result = (string)type.GetMethod(check).Invoke(null, null);
            StringAssert.StartsWith("PASS:", result);
            TestContext.WriteLine(result);
        }
        catch (TargetInvocationException exception)
        {
            throw exception.InnerException ?? exception;
        }
    }
}
